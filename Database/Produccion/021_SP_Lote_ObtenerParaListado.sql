USE FrontOne;
GO

-- Listado de Lotes para la pantalla de Producción (FrontOne.Web) — combina el encabezado del
-- Lote con su Corrida asociada (si ya existe) para traer Kilos Procesados y Estatus en una sola
-- fila. Un Lote sin Corrida todavía no tiene Kilos Procesados (0) ni Estatus definido
-- (0 = Sin Iniciar; 1 = En Proceso y 2 = Procesado vienen de Produccion.Corrida.Estatus).
-- Beneficiario/Huerta/Productor se resuelven igual que en Produccion.sp_Corrida_Obtener: de la
-- primera Recepción de Fruta agregada al Lote.
CREATE OR ALTER PROCEDURE Produccion.sp_Lote_ObtenerParaListado
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id,
        l.Folio,
        l.Fecha,
        primera.HuertaNombre,
        primera.ProductorNombre,
        primera.Beneficiario,
        l.Kilogramos AS KilosRecibidos,
        ISNULL(c.KilosProcesados, 0) AS KilosProcesados,
        (
            SELECT COUNT(*)
            FROM Lotes.LoteRecepcion det
            WHERE det.LoteId = l.Id
        ) AS Recepciones,
        l.PorcentajeMateriaSeca,
        ISNULL(c.Estatus, 0) AS Estatus
    FROM Lotes.Lote l
    LEFT JOIN Produccion.Corrida c ON c.LoteId = l.Id
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, pr.NombreProductor AS ProductorNombre,
            oc.PagarCorteANombre AS Beneficiario
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    ORDER BY l.FechaCreacion DESC;
END
GO
