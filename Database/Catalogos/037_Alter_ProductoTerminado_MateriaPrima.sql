USE FrontOne;
GO

-- Materia Prima: código de artículo SAP del grupo "MP" asociado al Producto Terminado. Se
-- guarda como texto libre (ItemCode de SAP), no como FK a un catálogo FrontOne — la lista de
-- materias primas viene siempre en vivo de SAP (mismo criterio que Variedad/Categoria pero sin
-- catálogo local propio, porque el maestro real es SAP, no FrontOne).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.ProductoTerminado') AND name = 'MateriaPrimaItemCode')
BEGIN
    ALTER TABLE Catalogos.ProductoTerminado ADD MateriaPrimaItemCode NVARCHAR(50) NULL;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           FechaCreacion
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
           Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           FechaCreacion
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
           Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           FechaCreacion
    FROM Catalogos.ProductoTerminado
    WHERE CodigoSap LIKE '%' + @Filtro + '%'
       OR DescripcionSap LIKE '%' + @Filtro + '%'
    ORDER BY DescripcionSap;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_ProductoTerminado_Actualizar
    @Id                     INT,
    @CodigoUpc              NVARCHAR(20)  = NULL,
    @CodigoPlu              NVARCHAR(20)  = NULL,
    @CodigoGtin             NVARCHAR(20)  = NULL,
    @MateriaPrimaItemCode   NVARCHAR(50)  = NULL,
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
        MateriaPrimaItemCode = @MateriaPrimaItemCode,
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
