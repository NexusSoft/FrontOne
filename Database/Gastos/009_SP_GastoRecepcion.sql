USE FrontOne;
GO

-- Fila base (no borrable en la UI) de Cosecha o Acarreo por cada Recepción del Lote. Cantidad/
-- PrecioUnitario/Importe se calculan en vivo, nunca se persisten. Cosecha es un servicio (1
-- "unidad" del servicio de cuadrilla por Recepción), no una cantidad de kilos: Cantidad siempre
-- es 1 y el precio total ya calculado (Acopio.OrdenCorte.CostoKg * PesoNeto si PesoNeto >= 4000
-- kg, o PagoDia si es menor) va completo en Precio Unitario = Importe. Acarreo sí es por
-- kilogramo real (Acopio.OrdenCorte.PrecioAcarreo × PesoNeto), Cantidad = PesoNeto. Antes de
-- seleccionar, crea la fila Gastos.GastoRecepcion (CargoA = Empresa por default) para cualquier
-- Recepción del Lote que todavía no la tenga para este TipoGasto, para que la UI siempre tenga
-- algo que editar.
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
        CASE WHEN @TipoGasto = 1 THEN 1 ELSE rf.PesoNeto END AS Cantidad,
        CASE
            WHEN @TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
            WHEN @TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
            ELSE oc.PrecioAcarreo
        END AS PrecioUnitario,
        CASE
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

CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcion_ActualizarCargoA
    @Id     INT,
    @CargoA TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Gastos.GastoRecepcion SET CargoA = @CargoA WHERE Id = @Id;
END
GO
