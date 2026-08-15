USE FrontOne;
GO

-- Agrega Productor, FechaLote y FechaOrdenCorteMax al detalle de la etiqueta de Pallet — mismo
-- patrón de joins que ya usa Produccion.sp_Pallet_ObtenerEtiquetaCaja (LoteRecepcion ->
-- RecepcionFrutaOrdenCorte -> OrdenCorte -> Huerta/Productor, más el MAX de fecha de corte).
-- Los 3 campos son constantes por Lote (el mismo valor se repite en todas las filas de
-- PalletDetalle agrupadas bajo ese Lote), así que se envuelven en MAX() para no tener que
-- ampliar el GROUP BY existente.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerEtiquetaPalletDetalle
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Folio             AS NoLote,
        h.RegistroSagarpa   AS RegistroSagarpa,
        h.Nombre            AS Huerta,
        MAX(pr.NombreProductor) AS Productor,
        MAX(l.Fecha)        AS FechaLote,
        MAX(ocMax.FechaOrdenCorteMax) AS FechaOrdenCorteMax,
        SUM(d.Cajas)        AS Cajas,
        SUM(d.Kilogramos)   AS Kilogramos
    FROM Produccion.PalletDetalle d
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    OUTER APPLY (
        SELECT TOP 1 h2.Nombre, h2.RegistroSagarpa, h2.ProductorId
        FROM Lotes.LoteRecepcion lr
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = lr.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h2 ON h2.Id = oc.HuertaId
        WHERE lr.LoteId = l.Id
        ORDER BY lr.Id
    ) AS h
    LEFT JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
    OUTER APPLY (
        SELECT MAX(oc2.Fecha) AS FechaOrdenCorteMax
        FROM Lotes.LoteRecepcion lr2
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc2 ON roc2.RecepcionFrutaId = lr2.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc2 ON oc2.Id = roc2.OrdenCorteId
        WHERE lr2.LoteId = l.Id
    ) AS ocMax
    WHERE d.PalletId = @PalletId
    GROUP BY l.Folio, h.Nombre, h.RegistroSagarpa
    ORDER BY l.Folio;
END
GO
