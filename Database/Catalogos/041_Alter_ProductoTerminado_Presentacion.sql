USE FrontOne;
GO

-- Presentación del Producto Terminado: 1 Caja (empacado en cajas, comportamiento actual), 2 Granel
-- (sin cajas, se pesa directo). Campo 100% local de FrontOne, no sincroniza con SAP. Default 1
-- (Caja) para que todo lo ya sincronizado desde SAP quede exactamente como hoy, sin romper nada.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.ProductoTerminado') AND name = 'Presentacion')
BEGIN
    ALTER TABLE Catalogos.ProductoTerminado ADD Presentacion TINYINT NOT NULL
        CONSTRAINT DF_Catalogos_ProductoTerminado_Presentacion DEFAULT (1);
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
           Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
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
           Id, CodigoSap, DescripcionSap, DescripcionExtranjeraSap, Activo, CodigoUpc, CodigoPlu, CodigoGtin,
           MateriaPrimaItemCode, CategoriaId, TipoProductoId, CalibreApeamId, CalibreCodigoExterno,
           MercadoDestinoPaisId, MarcaId, VariedadId, PesoEstandarId, PesoNeto, PesoPromedio, CajasPorPallet,
           Presentacion, FechaCreacion
    FROM Catalogos.ProductoTerminado
    WHERE CodigoSap LIKE '%' + @Filtro + '%'
       OR DescripcionSap LIKE '%' + @Filtro + '%'
    ORDER BY DescripcionSap;
END
GO

-- Actualización completa de los campos de negocio. Defensa en profundidad: si @Presentacion = 2
-- (Granel), se fuerzan a NULL PesoEstandarId/PesoNeto/CajasPorPallet sin importar lo que mande el
-- cliente — esos campos no aplican a un producto a granel (mismo criterio ya usado con
-- EsMixto/ProductoTerminadoId en Pallets: el servidor no confía ciegamente en la UI).
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
    @CajasPorPallet         INT           = NULL,
    @Presentacion           TINYINT       = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @Presentacion = 2
    BEGIN
        SET @PesoEstandarId = NULL;
        SET @PesoNeto = NULL;
        SET @CajasPorPallet = NULL;
    END

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
        CajasPorPallet = @CajasPorPallet,
        Presentacion = @Presentacion
    WHERE Id = @Id;
END
GO
