USE FrontOne;
GO

-- "Relación de Gastos" del Reporte de Proceso: une las filas base (Cosecha/Acarreo) y sus
-- ajustes de TODAS las Recepciones del Lote en un solo result set, con CXP/CAP (Con cargo a
-- Empresa / Con cargo a Productor) como columnas BIT independientes según Gastos.*.CargoA.
CREATE OR ALTER PROCEDURE Gastos.sp_GastoRecepcion_ObtenerParaReporte
    @GastoLoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Base AS (
        SELECT
            CASE WHEN gr.TipoGasto = 1 THEN N'Cosecha' ELSE N'Acarreo' END AS TipoGasto,
            CASE WHEN gr.TipoGasto = 1 THEN oc.JefeCuadrillaNombre ELSE oc.TransportistaNombre END AS Proveedor,
            CASE WHEN gr.TipoGasto = 1 THEN 1 ELSE rf.PesoNeto END AS Cantidad,
            CASE
                WHEN gr.TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
                WHEN gr.TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
                ELSE oc.PrecioAcarreo
            END AS PrecioUnitario,
            CASE
                WHEN gr.TipoGasto = 1 AND rf.PesoNeto < 4000 THEN oc.PagoDia
                WHEN gr.TipoGasto = 1 THEN oc.CostoKg * rf.PesoNeto
                ELSE oc.PrecioAcarreo * rf.PesoNeto
            END AS Importe,
            gr.CargoA
        FROM Gastos.GastoRecepcion gr
        INNER JOIN Lotes.LoteRecepcion det ON det.Id = gr.LoteRecepcionId
        INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = rf.Id
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        WHERE gr.GastoLoteId = @GastoLoteId
    ),
    Ajustes AS (
        SELECT
            CASE WHEN ta.TipoGasto = 1 THEN N'Cosecha' ELSE N'Acarreo' END AS TipoGasto,
            ta.Nombre AS Proveedor,
            1 AS Cantidad,
            CASE WHEN ta.Signo = 2 THEN -gra.Monto ELSE gra.Monto END AS PrecioUnitario,
            CASE WHEN ta.Signo = 2 THEN -gra.Monto ELSE gra.Monto END AS Importe,
            gra.CargoA
        FROM Gastos.GastoRecepcionAjuste gra
        INNER JOIN Gastos.TipoAjuste ta ON ta.Id = gra.TipoAjusteId
        WHERE gra.GastoLoteId = @GastoLoteId
    )
    SELECT
        TipoGasto, Proveedor, Cantidad, PrecioUnitario, Importe,
        CAST(CASE WHEN CargoA = 1 THEN 1 ELSE 0 END AS BIT) AS CXP,
        CAST(CASE WHEN CargoA = 2 THEN 1 ELSE 0 END AS BIT) AS CAP
    FROM Base
    UNION ALL
    SELECT
        TipoGasto, Proveedor, Cantidad, PrecioUnitario, Importe,
        CAST(CASE WHEN CargoA = 1 THEN 1 ELSE 0 END AS BIT) AS CXP,
        CAST(CASE WHEN CargoA = 2 THEN 1 ELSE 0 END AS BIT) AS CAP
    FROM Ajustes
    ORDER BY TipoGasto, Proveedor;
END
GO
