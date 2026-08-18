USE FrontOne;
GO

-- Encabezado para el Reporte de Proceso / Liquidación al Productor — mismo shape que
-- Gastos.sp_GastoLote_ObtenerEncabezado, en un SP propio porque los reportes nunca deben
-- compartir el SP que usa la UI de captura (regla del proyecto, ver ReportePallet/
-- ReporteRecepcionFruta).
CREATE OR ALTER PROCEDURE Gastos.sp_GastoLote_ObtenerParaReporte
    @GastoLoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LoteId INT = (SELECT LoteId FROM Gastos.GastoLote WHERE Id = @GastoLoteId);

    SELECT
        l.Id AS LoteId, l.Folio AS LoteFolio, ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        c.FechaHoraFin AS FechaCorrida, l.Kilogramos,
        primera.HuertaNombre, primera.RegistroSagarpa, primera.ProductorNombre, primera.VariedadNombre,
        primera.TipoCorteNombre, primera.TipoPagoNombre
    FROM Lotes.Lote l
    INNER JOIN Produccion.Corrida c ON c.LoteId = l.Id
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, pr.NombreProductor AS ProductorNombre,
            v.Nombre AS VariedadNombre, tc.Nombre AS TipoCorteNombre, tp.Nombre AS TipoPagoNombre
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        INNER JOIN Acopio.Variedad v ON v.Id = oc.VariedadId
        INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
        INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE l.Id = @LoteId;
END
GO

-- Resumen por Mercado del Reporte de Proceso: mismo cálculo de Costo Real/Estimado por línea
-- que Gastos.sp_GastoFrutaCategoria_Obtener, agrupado por Mercado en vez de por Categoría.
-- Mercado se deriva de Catalogos.ProductoTerminado.MercadoDestinoPaisId (México = Nacional,
-- cualquier otro país = Exportación); "Merma" se identifica por convención de nombre de
-- Categoría (prefijo "Z)"), a falta de un campo dedicado en el catálogo hoy.
CREATE OR ALTER PROCEDURE Gastos.sp_GastoFrutaCategoria_ObtenerResumenMercado
    @GastoLoteId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LoteId INT;
    DECLARE @CostoEstimadoListaPrecioFecha DATE, @CostoEstimadoListaPrecioProductorId INT, @CostoEstimadoListaPrecioNumero TINYINT;
    SELECT @LoteId = LoteId,
           @CostoEstimadoListaPrecioFecha = CostoEstimadoListaPrecioFecha,
           @CostoEstimadoListaPrecioProductorId = CostoEstimadoListaPrecioProductorId,
           @CostoEstimadoListaPrecioNumero = CostoEstimadoListaPrecioNumero
    FROM Gastos.GastoLote
    WHERE Id = @GastoLoteId;

    DECLARE @VariedadId INT, @Precio DECIMAL(18,4), @ListaPrecioFecha DATE, @ListaPrecioProductorId INT,
            @ListaPrecioNumero TINYINT, @NecesitaListaPrecios BIT;
    SELECT TOP 1
        @VariedadId = oc.VariedadId, @Precio = ac.Precio,
        @ListaPrecioFecha = ac.ListaPrecioFecha, @ListaPrecioProductorId = ac.ListaPrecioProductorId,
        @ListaPrecioNumero = ac.ListaPrecioNumero, @NecesitaListaPrecios = tp.NecesitaListaPrecios
    FROM Lotes.LoteRecepcion det
    INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
    INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
    INNER JOIN Acopio.AcuerdoCorte ac ON ac.Id = oc.AcuerdoCorteId
    INNER JOIN Acopio.TipoCorte tc ON tc.Id = ac.TipoCorteId
    INNER JOIN Acopio.TipoPago tp ON tp.Id = tc.TipoPagoId
    WHERE det.LoteId = @LoteId
    ORDER BY det.Id;

    ;WITH Detalle AS (
        SELECT
            pd.Kilogramos,
            CASE WHEN pt.DescripcionSap LIKE 'Z)%' THEN N'Merma'
                 WHEN pais.Clave = 'MX' THEN N'Nacional'
                 ELSE N'Exportación'
            END AS Mercado,
            CASE WHEN @NecesitaListaPrecios = 0 THEN @Precio
                 ELSE (
                    SELECT CASE @ListaPrecioNumero WHEN 1 THEN lpf.Lista1 WHEN 2 THEN lpf.Lista2 WHEN 3 THEN lpf.Lista3 END
                    FROM Acopio.ListaPrecioFruta lpf
                    WHERE lpf.ItemCode = pt.MateriaPrimaItemCode AND lpf.VariedadId = @VariedadId
                      AND lpf.FechaInicio = @ListaPrecioFecha
                      AND ((@ListaPrecioProductorId IS NULL AND lpf.ProductorId IS NULL) OR lpf.ProductorId = @ListaPrecioProductorId)
                 )
            END AS CostoRealCalculado,
            CASE WHEN @CostoEstimadoListaPrecioFecha IS NULL THEN NULL
                 ELSE (
                    SELECT CASE @CostoEstimadoListaPrecioNumero WHEN 1 THEN lpf.Lista1 WHEN 2 THEN lpf.Lista2 WHEN 3 THEN lpf.Lista3 END
                    FROM Acopio.ListaPrecioFruta lpf
                    WHERE lpf.ItemCode = pt.MateriaPrimaItemCode AND lpf.VariedadId = @VariedadId
                      AND lpf.FechaInicio = @CostoEstimadoListaPrecioFecha
                      AND ((@CostoEstimadoListaPrecioProductorId IS NULL AND lpf.ProductorId IS NULL) OR lpf.ProductorId = @CostoEstimadoListaPrecioProductorId)
                 )
            END AS CostoEstimadoCalculado
        FROM Produccion.PalletDetalle pd
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = pd.ProductoTerminadoId
        LEFT JOIN Catalogos.Pais pais ON pais.Id = pt.MercadoDestinoPaisId
        WHERE pd.LoteId = @LoteId
    ),
    PorMercado AS (
        SELECT
            Mercado,
            SUM(Kilogramos) AS Kilogramos,
            SUM(Kilogramos * CostoRealCalculado) AS ImporteReal,
            SUM(Kilogramos * CostoEstimadoCalculado) AS ImporteEstimado
        FROM Detalle
        GROUP BY Mercado
    )
    -- Sin ORDER BY aquí a propósito: T-SQL exige que el ORDER BY de un UNION use una columna
    -- real del SELECT, y agregar una columna "Orden" extra solo para eso arriesga que Dapper
    -- la confunda al mapear directo a ResumenMercadoDto (record). El orden final
    -- (Exportación/Nacional/Merma/Total) se aplica en C#, en GastoFrutaCategoriaService.
    SELECT Mercado, Kilogramos,
           Kilogramos * 100.0 / NULLIF(SUM(Kilogramos) OVER (), 0) AS Porcentaje,
           ImporteReal, ImporteEstimado
    FROM PorMercado
    UNION ALL
    SELECT N'Total', SUM(Kilogramos), 100.0, SUM(ImporteReal), SUM(ImporteEstimado)
    FROM PorMercado;
END
GO
