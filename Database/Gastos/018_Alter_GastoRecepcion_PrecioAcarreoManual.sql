USE FrontOne;
GO

-- Corrige un bug real: Acopio.OrdenCorte.PrecioAcarreo ya es el precio fijo del tramo
-- (Precio300/400/500 de Acarreo.ListaPrecioAcarreo, ver OrdenCorteEditarForm.CalcularPrecioAcarreo)
-- copiado tal cual al crear la Orden — NO es una tarifa por kg. sp_GastoRecepcion_ObtenerBase
-- lo multiplicaba por rf.PesoNeto como si lo fuera, inflando el Importe (caso real: $5,500 x
-- 8,195 kg = $45,072,500 en vez de $5,500). Ahora Acarreo se trata igual que Cosecha: PrecioUnitario
-- es el monto ya completo, Cantidad = 1, Importe = PrecioUnitario. También agrega el mismo
-- mecanismo de override manual que Cosecha (PrecioUnitarioManual) para el botón "Actualizar
-- Precio" de la pestaña Acarreo, que jala el precio vigente de Acarreo.ListaPrecioAcarreo por
-- Municipio de la Huerta + tramo de Cajas Entregadas.
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
            WHEN @TipoGasto = 2 AND gr.PrecioUnitarioManual IS NOT NULL THEN gr.PrecioUnitarioManual
            ELSE oc.PrecioAcarreo
        END AS PrecioUnitario,
        CASE
            WHEN @TipoGasto = 1 AND gr.PrecioUnitarioManual IS NOT NULL THEN gr.PrecioUnitarioManual * rf.PesoNeto
            WHEN @TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
            WHEN @TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
            WHEN @TipoGasto = 2 AND gr.PrecioUnitarioManual IS NOT NULL THEN gr.PrecioUnitarioManual
            ELSE oc.PrecioAcarreo
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

-- Toma el precio vigente de Acarreo.ListaPrecioAcarreo para el Municipio de la Huerta de la
-- Orden de Corte de esta recepción, en el tramo (300/400/500) según CajasEntregadas de esa
-- Orden — mismo criterio que ya usa OrdenCorteEditarForm al calcular PrecioAcarreo — y lo deja
-- fijo en PrecioUnitarioManual. Solo aplica a Acarreo (TipoGasto = 2).
CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcion_ActualizarPrecioAcarreo
    @GastoRecepcionId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MunicipioId INT, @CajasEntregadas SMALLINT, @TipoGasto TINYINT;

    SELECT
        @MunicipioId = h.MunicipioId,
        @CajasEntregadas = oc.CajasEntregadas,
        @TipoGasto = gr.TipoGasto
    FROM Gastos.GastoRecepcion gr
    INNER JOIN Lotes.LoteRecepcion det ON det.Id = gr.LoteRecepcionId
    INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = rf.Id
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
    WHERE gr.Id = @GastoRecepcionId;

    IF @MunicipioId IS NULL
    BEGIN
        THROW 50000, 'No se encontró la huerta de la orden de corte de esta recepción.', 1;
    END

    IF @TipoGasto <> 2
    BEGIN
        THROW 50000, 'Solo se puede actualizar el precio de Acarreo de Fruta en la pestaña de Acarreo.', 1;
    END

    DECLARE @Precio300 DECIMAL(18,2), @Precio400 DECIMAL(18,2), @Precio500 DECIMAL(18,2);

    SELECT @Precio300 = Precio300, @Precio400 = Precio400, @Precio500 = Precio500
    FROM Acarreo.ListaPrecioAcarreo
    WHERE MunicipioId = @MunicipioId AND Activo = 1;

    IF @Precio300 IS NULL
    BEGIN
        THROW 50000, 'No hay un precio vigente en la Lista de Precios de Acarreo para el municipio de esta huerta.', 1;
    END

    DECLARE @Precio DECIMAL(18,2) = CASE @CajasEntregadas WHEN 300 THEN @Precio300 WHEN 400 THEN @Precio400 ELSE @Precio500 END;

    UPDATE Gastos.GastoRecepcion SET PrecioUnitarioManual = @Precio WHERE Id = @GastoRecepcionId;

    SELECT @Precio AS PrecioUnitario, @Precio AS Importe;
END
GO
