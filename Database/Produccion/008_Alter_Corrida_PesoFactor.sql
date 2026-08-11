USE FrontOne;
GO

-- Peso Factor: el peso estándar del producto (Catalogos.ProductoTerminado.PesoNeto) no siempre
-- coincide con el peso real recepcionado del lote. Cada línea de Produccion.PalletDetalle ya
-- guarda un Kilogramos TEÓRICO (Cajas × PesoNeto), mientras que Lotes.Lote.Kilogramos es el
-- peso REAL recepcionado. Factor = Kilogramos reales del Lote / Σ Kilogramos teóricos de todas
-- las líneas de Pallet de esa Corrida — multiplicar ese factor por cualquier peso estándar y
-- sumar reconstruye los kilos reales. NULL mientras la corrida no tenga ninguna línea de Pallet
-- (evita división entre cero).
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
            WHEN ISNULL(pesoTeo.SumaKilogramosTeoricos, 0) = 0 THEN NULL
            ELSE CAST(l.Kilogramos / pesoTeo.SumaKilogramosTeoricos AS DECIMAL(10, 6))
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
        SELECT SUM(pd.Kilogramos) AS SumaKilogramosTeoricos
        FROM Produccion.PalletDetalle pd
        WHERE pd.CorridaId = c.Id
    ) AS pesoTeo
    WHERE (@Id IS NULL OR c.Id = @Id)
    ORDER BY c.FechaCreacion DESC;
END
GO
