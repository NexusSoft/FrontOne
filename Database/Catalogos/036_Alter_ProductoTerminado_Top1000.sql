USE FrontOne;
GO

-- El listado de Productos Terminados crece rápido vía sincronización SAP — se sube el TOP
-- inicial de 100 a 1000 (pedido explícito del usuario), renombrando el SP para que el nombre
-- siga reflejando la cantidad real que trae (mismo criterio que sp_Productor_ObtenerTop100).
IF OBJECT_ID('Catalogos.sp_ProductoTerminado_ObtenerTop100') IS NOT NULL
BEGIN
    DROP PROCEDURE Catalogos.sp_ProductoTerminado_ObtenerTop100;
END
GO

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
