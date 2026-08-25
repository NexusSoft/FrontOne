USE FrontOne;
GO

-- Precio unitario capturado a mano desde Acopio.ListaPrecioCorte (botón "Actualizar Precio" en
-- la pestaña Cosecha de GastoLoteForm). Mientras esté NULL, sp_GastoRecepcion_ObtenerBase sigue
-- calculando en vivo desde Acopio.OrdenCorte.CostoKg/PagoDia como hasta ahora; en cuanto se
-- captura, ese valor manda y ya no se recalcula solo hasta que se vuelva a presionar el botón.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gastos.GastoRecepcion') AND name = 'PrecioUnitarioManual')
BEGIN
    ALTER TABLE Gastos.GastoRecepcion ADD PrecioUnitarioManual DECIMAL(18,4) NULL;
END
GO

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
            WHEN @TipoGasto = 1 AND gr.PrecioUnitarioManual IS NOT NULL THEN gr.PrecioUnitarioManual
            WHEN @TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
            WHEN @TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
            ELSE oc.PrecioAcarreo
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

-- Toma el precio vigente de Acopio.ListaPrecioCorte para la Empresa de Corte de la Orden de
-- Corte de esta recepción (por CardCode; si la empresa no tiene precio propio capturado, cae al
-- renglón "Otros") y lo deja fijo en PrecioUnitarioManual. Solo aplica a Cosecha (TipoGasto = 1)
-- — Acarreo sigue tomando su precio de Acopio.OrdenCorte.PrecioAcarreo sin cambios.
CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcion_ActualizarPrecioCorte
    @GastoRecepcionId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CardCode NVARCHAR(20), @PesoNeto DECIMAL(18,2), @TipoGasto TINYINT;

    SELECT
        @CardCode = oc.JefeCuadrillaCardCode,
        @PesoNeto = rf.PesoNeto,
        @TipoGasto = gr.TipoGasto
    FROM Gastos.GastoRecepcion gr
    INNER JOIN Lotes.LoteRecepcion det ON det.Id = gr.LoteRecepcionId
    INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    WHERE gr.Id = @GastoRecepcionId;

    IF @CardCode IS NULL
    BEGIN
        THROW 50000, 'No se encontró la orden de corte de esta recepción.', 1;
    END

    IF @TipoGasto <> 1
    BEGIN
        THROW 50000, 'Solo se puede actualizar el precio de Corte de Fruta en la pestaña de Cosecha.', 1;
    END

    DECLARE @PrecioKg DECIMAL(18,2);

    SELECT @PrecioKg = PrecioKg FROM Acopio.ListaPrecioCorte WHERE CardCode = @CardCode AND Activo = 1;

    IF @PrecioKg IS NULL
    BEGIN
        SELECT @PrecioKg = PrecioKg FROM Acopio.ListaPrecioCorte WHERE EsOtros = 1 AND Activo = 1;
    END

    IF @PrecioKg IS NULL
    BEGIN
        THROW 50000, 'No hay un precio vigente en la Lista de Precios de Corte para esta empresa.', 1;
    END

    UPDATE Gastos.GastoRecepcion SET PrecioUnitarioManual = @PrecioKg WHERE Id = @GastoRecepcionId;

    SELECT @PrecioKg AS PrecioUnitario, @PrecioKg * @PesoNeto AS Importe;
END
GO
