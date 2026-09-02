USE FrontOne;
GO

-- Grupo de artículos SAP de origen (ItemGroups.GroupName: "PT" Producto Terminado, "ST"
-- Semiterminado) — la sincronización ahora trae ambos grupos (antes solo "PT"), y esta columna
-- deja distinguirlos en el listado. Espejo de SAP, nunca se captura a mano; se fija una sola vez
-- al insertar (el grupo de un artículo no cambia en la práctica).
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.ProductoTerminado') AND name = 'GrupoSap')
BEGIN
    ALTER TABLE Catalogos.ProductoTerminado ADD GrupoSap NVARCHAR(2) NULL;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, CodigoSap, GrupoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           Presentacion, FechaCreacion
    FROM Catalogos.ProductoTerminado
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY DescripcionSap;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_ObtenerTop1000
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1000)
           Id, CodigoSap, GrupoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           Presentacion, FechaCreacion
    FROM Catalogos.ProductoTerminado
    ORDER BY FechaCreacion DESC;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Buscar
    @Filtro NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (500)
           Id, CodigoSap, GrupoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           Presentacion, FechaCreacion
    FROM Catalogos.ProductoTerminado
    WHERE CodigoSap LIKE '%' + @Filtro + '%'
       OR DescripcionSap LIKE '%' + @Filtro + '%'
    ORDER BY DescripcionSap;
END
GO

-- Alta mínima, usada únicamente por la sincronización con SAP (SincronizarConSapAsync).
CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Insertar
    @CodigoSap                NVARCHAR(50),
    @GrupoSap                 NVARCHAR(2) = NULL,
    @DescripcionSap           NVARCHAR(200),
    @DescripcionExtranjeraSap NVARCHAR(200) = NULL,
    @Activo                   BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Catalogos.ProductoTerminado (CodigoSap, GrupoSap, DescripcionSap, DescripcionExtranjeraSap, Activo)
    VALUES (@CodigoSap, @GrupoSap, @DescripcionSap, @DescripcionExtranjeraSap, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO
