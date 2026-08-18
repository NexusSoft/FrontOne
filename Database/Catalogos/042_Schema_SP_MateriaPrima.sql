USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- =============================================================================
-- Catalogos.MateriaPrima: catálogo maestro de materias primas, clon estructural
-- simplificado de Catalogos.ProductoTerminado. Nace ÚNICAMENTE de la
-- sincronización con SAP (grupo de artículos "MP" — el mismo grupo que ya
-- alimenta en vivo el LookUpEdit de Materia Prima en ProductoTerminadoEditarForm,
-- solo que aquí sí se persiste como catálogo local): CodigoSap/DescripcionSap/
-- Activo son espejo de SAP y solo las toca la sincronización. CategoriaId/
-- CalibreApeamId son NULLABLE porque una materia prima recién sincronizada
-- todavía no tiene captura de negocio — el usuario la completa después desde
-- MateriaPrimaEditarForm. Ninguna FK lleva CASCADE (regla dura del proyecto).
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.MateriaPrima'))
BEGIN
    CREATE TABLE Catalogos.MateriaPrima
    (
        Id              INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_MateriaPrima PRIMARY KEY,
        CodigoSap       NVARCHAR(50)        NOT NULL,
        DescripcionSap  NVARCHAR(200)       NOT NULL,
        Activo          BIT                 NOT NULL CONSTRAINT DF_Catalogos_MateriaPrima_Activo DEFAULT (1),
        CategoriaId     INT                 NULL,
        CalibreApeamId  INT                 NULL,
        FechaCreacion   DATETIME2           NOT NULL CONSTRAINT DF_Catalogos_MateriaPrima_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Catalogos_MateriaPrima_CodigoSap UNIQUE (CodigoSap),
        CONSTRAINT FK_Catalogos_MateriaPrima_Categoria FOREIGN KEY (CategoriaId) REFERENCES Catalogos.Categoria (Id),
        CONSTRAINT FK_Catalogos_MateriaPrima_CalibreApeam FOREIGN KEY (CalibreApeamId) REFERENCES Catalogos.CalibreApeam (Id)
    );
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT mp.Id, mp.CodigoSap, mp.DescripcionSap, mp.Activo, mp.CategoriaId, mp.CalibreApeamId, mp.FechaCreacion,
           c.Nombre AS CategoriaNombre, ca.Nombre AS CalibreApeamNombre
    FROM Catalogos.MateriaPrima mp
    LEFT JOIN Catalogos.Categoria c ON c.Id = mp.CategoriaId
    LEFT JOIN Catalogos.CalibreApeam ca ON ca.Id = mp.CalibreApeamId
    WHERE (@Id IS NULL OR mp.Id = @Id)
    ORDER BY mp.DescripcionSap;
END
GO

-- Carga inicial del listado (sin filtro), TOP 1000 desde el inicio (mismo criterio ya
-- adoptado para ProductoTerminado — ver 036_Alter_ProductoTerminado_Top1000.sql).
CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_ObtenerTop1000
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1000)
           mp.Id, mp.CodigoSap, mp.DescripcionSap, mp.Activo, mp.CategoriaId, mp.CalibreApeamId, mp.FechaCreacion,
           c.Nombre AS CategoriaNombre, ca.Nombre AS CalibreApeamNombre
    FROM Catalogos.MateriaPrima mp
    LEFT JOIN Catalogos.Categoria c ON c.Id = mp.CategoriaId
    LEFT JOIN Catalogos.CalibreApeam ca ON ca.Id = mp.CalibreApeamId
    ORDER BY mp.FechaCreacion DESC;
END
GO

-- Búsqueda por texto (código o descripción SAP), TOP 500.
CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_Buscar
    @Filtro NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (500)
           mp.Id, mp.CodigoSap, mp.DescripcionSap, mp.Activo, mp.CategoriaId, mp.CalibreApeamId, mp.FechaCreacion,
           c.Nombre AS CategoriaNombre, ca.Nombre AS CalibreApeamNombre
    FROM Catalogos.MateriaPrima mp
    LEFT JOIN Catalogos.Categoria c ON c.Id = mp.CategoriaId
    LEFT JOIN Catalogos.CalibreApeam ca ON ca.Id = mp.CalibreApeamId
    WHERE mp.CodigoSap LIKE '%' + @Filtro + '%'
       OR mp.DescripcionSap LIKE '%' + @Filtro + '%'
    ORDER BY mp.DescripcionSap;
END
GO

-- Alta mínima, usada únicamente por la sincronización con SAP (SincronizarConSapAsync).
CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_Insertar
    @CodigoSap      NVARCHAR(50),
    @DescripcionSap NVARCHAR(200),
    @Activo         BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.MateriaPrima (CodigoSap, DescripcionSap, Activo)
    VALUES (@CodigoSap, @DescripcionSap, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Actualiza solo el campo espejo de SAP, usado por la sincronización cuando cambia el nombre en SAP.
CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_ActualizarDatosSap
    @Id             INT,
    @DescripcionSap NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.MateriaPrima
    SET DescripcionSap = @DescripcionSap
    WHERE Id = @Id;
END
GO

-- Reactiva/desactiva el registro (espejo de Valid en SAP), usado por la sincronización.
CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_ActualizarActivo
    @Id     INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.MateriaPrima
    SET Activo = @Activo
    WHERE Id = @Id;
END
GO

-- Actualización completa de los campos de negocio, capturados por el usuario en MateriaPrimaEditarForm.
CREATE OR ALTER PROCEDURE Catalogos.sp_MateriaPrima_Actualizar
    @Id             INT,
    @CategoriaId    INT = NULL,
    @CalibreApeamId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Catalogos.MateriaPrima
    SET CategoriaId = @CategoriaId,
        CalibreApeamId = @CalibreApeamId
    WHERE Id = @Id;
END
GO
