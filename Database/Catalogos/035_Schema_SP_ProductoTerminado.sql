USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- =============================================================================
-- Catalogos.ProductoTerminado: catálogo maestro de productos terminados.
-- Nace ÚNICAMENTE de la sincronización con SAP (grupo de artículos "PT"): las
-- columnas CodigoSap/DescripcionSap/DescripcionExtranjeraSap/Activo son espejo
-- de SAP y solo las toca la sincronización. Las FKs de catálogo de negocio
-- (Categoria/TipoProducto/CalibreApeam/MercadoDestinoPais/Marca/Variedad/
-- PesoEstandar) y los campos derivados (PesoNeto/PesoPromedio/CajasPorPallet)
-- son NULLABLE porque un producto recién sincronizado todavía no tiene
-- captura de negocio — el usuario los completa después desde
-- ProductoTerminadoEditarForm. Ninguna FK lleva CASCADE (regla dura del proyecto).
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.ProductoTerminado'))
BEGIN
    CREATE TABLE Catalogos.ProductoTerminado
    (
        Id                          INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_ProductoTerminado PRIMARY KEY,
        CodigoSap                   NVARCHAR(50)        NOT NULL,
        DescripcionSap              NVARCHAR(200)       NOT NULL,
        DescripcionExtranjeraSap    NVARCHAR(200)       NULL,
        Activo                      BIT                 NOT NULL CONSTRAINT DF_Catalogos_ProductoTerminado_Activo DEFAULT (1),
        CodigoUpc                   NVARCHAR(20)        NULL,
        CodigoPlu                   NVARCHAR(20)        NULL,
        CodigoGtin                  NVARCHAR(20)        NULL,
        MateriaPrimaItemCode        NVARCHAR(50)        NULL,
        CategoriaId                 INT                 NULL,
        TipoProductoId               INT                 NULL,
        CalibreApeamId              INT                 NULL,
        CalibreCodigoExterno        NVARCHAR(50)        NULL,
        MercadoDestinoPaisId        INT                 NULL,
        MarcaId                     INT                 NULL,
        VariedadId                  INT                 NULL,
        PesoEstandarId               INT                 NULL,
        PesoNeto                    DECIMAL(10,3)       NULL,
        PesoPromedio                DECIMAL(10,3)       NULL,
        CajasPorPallet              INT                 NULL,
        FechaCreacion               DATETIME2           NOT NULL CONSTRAINT DF_Catalogos_ProductoTerminado_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Catalogos_ProductoTerminado_CodigoSap UNIQUE (CodigoSap),
        CONSTRAINT FK_Catalogos_ProductoTerminado_Categoria FOREIGN KEY (CategoriaId) REFERENCES Catalogos.Categoria (Id),
        CONSTRAINT FK_Catalogos_ProductoTerminado_TipoProducto FOREIGN KEY (TipoProductoId) REFERENCES Catalogos.TipoProducto (Id),
        CONSTRAINT FK_Catalogos_ProductoTerminado_CalibreApeam FOREIGN KEY (CalibreApeamId) REFERENCES Catalogos.CalibreApeam (Id),
        CONSTRAINT FK_Catalogos_ProductoTerminado_Pais FOREIGN KEY (MercadoDestinoPaisId) REFERENCES Catalogos.Pais (Id),
        CONSTRAINT FK_Catalogos_ProductoTerminado_Marca FOREIGN KEY (MarcaId) REFERENCES Catalogos.Marca (Id),
        CONSTRAINT FK_Catalogos_ProductoTerminado_Variedad FOREIGN KEY (VariedadId) REFERENCES Acopio.Variedad (Id),
        CONSTRAINT FK_Catalogos_ProductoTerminado_PesoEstandar FOREIGN KEY (PesoEstandarId) REFERENCES Catalogos.PesoEstandar (Id)
    );
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno, MercadoDestinoPaisId, MarcaId,
           VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet, FechaCreacion
    FROM Catalogos.ProductoTerminado
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY DescripcionSap;
END
GO

-- Carga inicial del listado (sin filtro), para que el grid nunca se vea vacío al abrir.
-- TOP 1000 (no 100): este catálogo crece rápido vía sincronización SAP, pedido explícito del
-- usuario de subir el límite inicial — ver 036_Alter_ProductoTerminado_Top1000.sql.
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_ObtenerTop1000
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1000)
           Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno, MercadoDestinoPaisId, MarcaId,
           VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet, FechaCreacion
    FROM Catalogos.ProductoTerminado
    ORDER BY FechaCreacion DESC;
END
GO

-- Búsqueda por texto (código o descripción SAP), TOP 500.
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Buscar
    @Filtro NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (500)
           Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno, MercadoDestinoPaisId, MarcaId,
           VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet, FechaCreacion
    FROM Catalogos.ProductoTerminado
    WHERE CodigoSap LIKE '%' + @Filtro + '%'
       OR DescripcionSap LIKE '%' + @Filtro + '%'
    ORDER BY DescripcionSap;
END
GO

-- Alta mínima, usada únicamente por la sincronización con SAP (SincronizarConSapAsync).
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Insertar
    @CodigoSap                NVARCHAR(50),
    @DescripcionSap           NVARCHAR(200),
    @DescripcionExtranjeraSap NVARCHAR(200) = NULL,
    @Activo                   BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.ProductoTerminado (CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo)
    VALUES (@CodigoSap, @DescripcionSap, @DescripcionExtranjeraSap, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Actualiza solo los campos espejo de SAP, usado por la sincronización cuando cambia el nombre en SAP.
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_ActualizarDatosSap
    @Id                       INT,
    @DescripcionSap           NVARCHAR(200),
    @DescripcionExtranjeraSap NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.ProductoTerminado
    SET DescripcionSap = @DescripcionSap,
        DescripcionExtranjeraSap = @DescripcionExtranjeraSap
    WHERE Id = @Id;
END
GO

-- Reactiva/desactiva el registro (espejo de Valid en SAP), usado por la sincronización.
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_ActualizarActivo
    @Id     INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.ProductoTerminado
    SET Activo = @Activo
    WHERE Id = @Id;
END
GO

-- Actualización completa de los campos de negocio, capturados por el usuario en ProductoTerminadoEditarForm.
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Actualizar
    @Id                     INT,
    @CodigoUpc              NVARCHAR(20)  = NULL,
    @CodigoPlu              NVARCHAR(20)  = NULL,
    @CodigoGtin             NVARCHAR(20)  = NULL,
    @CategoriaId            INT           = NULL,
    @TipoProductoId          INT           = NULL,
    @CalibreApeamId         INT           = NULL,
    @CalibreCodigoExterno   NVARCHAR(50)  = NULL,
    @MercadoDestinoPaisId   INT           = NULL,
    @MarcaId                INT           = NULL,
    @VariedadId             INT           = NULL,
    @PesoEstandarId          INT           = NULL,
    @PesoNeto               DECIMAL(10,3) = NULL,
    @PesoPromedio           DECIMAL(10,3) = NULL,
    @CajasPorPallet         INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.ProductoTerminado
    SET CodigoUpc = @CodigoUpc,
        CodigoPlu = @CodigoPlu,
        CodigoGtin = @CodigoGtin,
        CategoriaId = @CategoriaId,
        TipoProductoId = @TipoProductoId,
        CalibreApeamId = @CalibreApeamId,
        CalibreCodigoExterno = @CalibreCodigoExterno,
        MercadoDestinoPaisId = @MercadoDestinoPaisId,
        MarcaId = @MarcaId,
        VariedadId = @VariedadId,
        PesoEstandarId = @PesoEstandarId,
        PesoNeto = @PesoNeto,
        PesoPromedio = @PesoPromedio,
        CajasPorPallet = @CajasPorPallet
    WHERE Id = @Id;
END
GO
