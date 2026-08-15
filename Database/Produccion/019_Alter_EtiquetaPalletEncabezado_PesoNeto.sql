USE FrontOne;
GO

-- Agrega FechaProcesado (Pallet.FechaCreacion) y PesoEstandar (peso por caja del producto, para
-- el campo "Presentación" de las etiquetas detalladas) al encabezado de la etiqueta de Pallet.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaPalletEncabezado
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pa.Folio            AS NoPallet,
        pa.FechaCreacion    AS FechaProcesado,
        pa.Estatus          AS Estatus,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) > 1 THEN 'Mixto'
            ELSE ISNULL(prim.DescripcionSap, '')
        END AS NombreProducto,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) > 1 THEN NULL
            ELSE prim.PesoNeto
        END AS PesoEstandar
    FROM Produccion.Pallet pa
    OUTER APPLY (
        SELECT COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d
        WHERE d.PalletId = pa.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap, pt.PesoNeto
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = pa.Id
        ORDER BY d.Id
    ) AS prim
    WHERE pa.Id = @PalletId;
END
GO
