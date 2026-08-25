USE FrontOne;
GO

-- Acarreo pasa a tratarse igual que Cosecha: es un servicio (1 "unidad" de acarreo por
-- Recepción), no una cantidad de kilos. Cantidad siempre 1, el precio total ya calculado
-- (Acopio.OrdenCorte.PrecioAcarreo * PesoNeto) va completo en Precio Unitario, e
-- Importe = Cantidad * Precio Unitario (antes Cantidad era PesoNeto, lo que inflaba el
-- Importe: PrecioAcarreo * PesoNeto * PesoNeto).
CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcion_ObtenerBase
    @GastoLoteId INT,
    @TipoGasto   TINYINT -- 1 = Cosecha, 2 = Acarreo
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LoteId INT = (SELECT LoteId FROM Gastos.GastoLote WHERE Id = @GastoLoteId);

    INSERT INTO Gastos.GastoRecepcion (GastoLoteId, LoteRecepcionId, TipoGasto, CargoA)
    SELECT @GastoLoteId, det.Id, @TipoGasto, 1
    FROM Lotes.LoteRecepcion det
    WHERE det.LoteId = @LoteId
      AND NOT EXISTS (
          SELECT 1 FROM Gastos.GastoRecepcion gr
          WHERE gr.LoteRecepcionId = det.Id AND gr.TipoGasto = @TipoGasto
      );

    SELECT
        gr.Id AS GastoRecepcionId, gr.LoteRecepcionId, gr.CargoA,
        rf.Id AS RecepcionFrutaId, rf.Folio AS RecepcionFolio, rf.Fecha, rf.PesoNeto, rf.PesoProductor,
        oc.Id AS OrdenCorteId, oc.Folio AS OrdenCorteFolio,
        CASE WHEN @TipoGasto = 1 THEN oc.JefeCuadrillaNombre ELSE oc.TransportistaNombre END AS Proveedor,
        CAST(1 AS DECIMAL(18,4)) AS Cantidad,
        CASE
            WHEN @TipoGasto = 1 AND gr.PrecioUnitarioManual IS NOT NULL THEN gr.PrecioUnitarioManual
            WHEN @TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
            WHEN @TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
            ELSE oc.PrecioAcarreo * rf.PesoNeto
        END AS PrecioUnitario,
        CASE
            WHEN @TipoGasto = 1 AND gr.PrecioUnitarioManual IS NOT NULL THEN gr.PrecioUnitarioManual * rf.PesoNeto
            WHEN @TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
            WHEN @TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
            ELSE oc.PrecioAcarreo * rf.PesoNeto
        END AS Importe
    FROM Gastos.GastoRecepcion gr
    INNER JOIN Lotes.LoteRecepcion det ON det.Id = gr.LoteRecepcionId
    INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    WHERE gr.GastoLoteId = @GastoLoteId AND gr.TipoGasto = @TipoGasto
    ORDER BY rf.Fecha;
END
GO
