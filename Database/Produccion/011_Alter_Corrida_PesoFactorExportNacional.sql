USE FrontOne;
GO

-- Peso Factor: se reemplaza la fórmula simple (Kilos reales del Lote / Σ Kilogramos teóricos de
-- TODOS los Pallets) por la fórmula real del sistema legado (Fronterra, Calcula_Factor_Update),
-- que separa Exportación de Nacional y resta la Merma antes de comparar contra el teórico:
--
--   KgExportacion = Σ Kilogramos de líneas Caja (Presentacion = 1)          — siempre teórico
--   KgNacional    = Σ Kilogramos de líneas Granel (Presentacion = 2) que NO sean Pallet Neutro
--                   — el Kilogramos de una línea Granel ya es el valor real capturado directo,
--                   no hace falta el fallback teórico que sí necesita el legacy (n_peso_pal)
--   KgMerma       = Σ Kilogramos de las líneas de Pallet Neutro (EsNeutro = 1) del producto MERMA
--
--   PesoFactor = 1                                                          si KgExportacion = 0
--              = (KilosAProcesar − (KgNacional + KgMerma)) / KgExportacion  en cualquier otro caso
--
-- KgExportacion = 0 (sin líneas Caja en la Corrida) da factor = 1: todo lo producido ya se pesó
-- real en báscula (Granel), no hay nada que corregir contra un peso teórico.
-- Las líneas Diferencia a Favor (también Granel, EsNeutro = 1) quedan fuera del cálculo — son un
-- ajuste de balance de KilosProcesados, no producto Nacional real ni Merma.
CREATE OR ALTER PROCEDURE Produccion.sp_Corrida_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id, c.LoteId, l.Folio AS LoteFolio, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        l.Kilogramos,
        primera.HuertaNombre, primera.RegistroSagarpa, primera.ProductorNombre, primera.Beneficiario,
        c.FechaHoraInicio, c.FechaHoraFin, c.Estatus, c.KilosAProcesar, c.KilosProcesados,
        CASE
            WHEN ISNULL(factorCalc.KgExportacion, 0) = 0 THEN CAST(1 AS DECIMAL(10, 6))
            ELSE CAST((c.KilosAProcesar - (ISNULL(factorCalc.KgNacional, 0) + ISNULL(factorCalc.KgMerma, 0))) / factorCalc.KgExportacion AS DECIMAL(10, 6))
        END AS PesoFactor,
        c.FechaCreacion
    FROM Produccion.Corrida c
    INNER JOIN Lotes.Lote l ON l.Id = c.LoteId
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, pr.NombreProductor AS ProductorNombre,
            oc.PagarCorteANombre AS Beneficiario
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    OUTER APPLY (
        SELECT
            SUM(CASE WHEN pt.Presentacion = 1 THEN pd.Kilogramos ELSE 0 END) AS KgExportacion,
            SUM(CASE WHEN pt.Presentacion = 2 AND pal.EsNeutro = 0 THEN pd.Kilogramos ELSE 0 END) AS KgNacional,
            SUM(CASE WHEN pal.EsNeutro = 1 AND pt.DescripcionSap = N'MERMA' THEN pd.Kilogramos ELSE 0 END) AS KgMerma
        FROM Produccion.PalletDetalle pd
        INNER JOIN Produccion.Pallet pal ON pal.Id = pd.PalletId
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = pd.ProductoTerminadoId
        WHERE pd.CorridaId = c.Id
    ) AS factorCalc
    WHERE (@Id IS NULL OR c.Id = @Id)
    ORDER BY c.FechaCreacion DESC;
END
GO
