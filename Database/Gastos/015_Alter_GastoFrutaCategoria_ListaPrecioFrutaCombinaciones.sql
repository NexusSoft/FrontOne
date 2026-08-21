USE FrontOne;
GO

-- Acopio.ListaPrecioFruta dejó de estar anclada a SAP (ItemCode) + Variedad, ahora se ancla a
-- Categoría+Calibre APEAM (ver 036_Alter_ListaPrecioFruta_Combinaciones.sql). El match de
-- precio "a banda" tiene que pivotear por Catalogos.MateriaPrima (CodigoSap = mismo código
-- SAP que Catalogos.ProductoTerminado.MateriaPrimaItemCode) para llegar a CategoriaId/
-- CalibreApeamId y de ahí a la lista de precios — ya no hay VariedadId que igualar. Este
-- cambio se repite igual en los 2 procedimientos que resuelven precio (007 y el de resumen
-- por mercado dentro de 011), copiados uno del otro, no compartidos vía función/vista.
CREATE OR ALTER PROCEDURE Gastos.sp_GastoFrutaCategoria_Obtener
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

    DECLARE @Precio DECIMAL(18,4), @ListaPrecioFecha DATE, @ListaPrecioProductorId INT,
            @ListaPrecioNumero TINYINT, @NecesitaListaPrecios BIT;
    SELECT TOP 1
        @Precio = ac.Precio,
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

    DECLARE @KgRecepcionado DECIMAL(18,2) = (
        SELECT ISNULL(SUM(rf.PesoNeto), 0)
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFruta rf ON rf.Id = det.RecepcionFrutaId
        WHERE det.LoteId = @LoteId
    );

    ;WITH Detalle AS (
        SELECT
            pd.Kilogramos,
            pt.MateriaPrimaItemCode,
            CASE WHEN pt.DescripcionSap LIKE 'Z)%' THEN N'Merma'
                 WHEN pais.Clave = 'MX' THEN N'Nacional'
                 ELSE N'Exportación'
            END AS Mercado,
            CASE WHEN @NecesitaListaPrecios = 0 THEN @Precio
                 ELSE (
                    SELECT CASE @ListaPrecioNumero WHEN 1 THEN lpf.Convencional WHEN 2 THEN lpf.Organico WHEN 3 THEN lpf.Nacional END
                    FROM Catalogos.MateriaPrima mp
                    JOIN Acopio.ListaPrecioFruta lpf ON lpf.CategoriaId = mp.CategoriaId AND lpf.CalibreApeamId = mp.CalibreApeamId
                    WHERE mp.CodigoSap = pt.MateriaPrimaItemCode
                      AND lpf.FechaInicio = @ListaPrecioFecha
                      AND ((@ListaPrecioProductorId IS NULL AND lpf.ProductorId IS NULL) OR lpf.ProductorId = @ListaPrecioProductorId)
                 )
            END AS CostoRealCalculado,
            CASE WHEN @CostoEstimadoListaPrecioFecha IS NULL THEN NULL
                 ELSE (
                    SELECT CASE @CostoEstimadoListaPrecioNumero WHEN 1 THEN lpf.Convencional WHEN 2 THEN lpf.Organico WHEN 3 THEN lpf.Nacional END
                    FROM Catalogos.MateriaPrima mp
                    JOIN Acopio.ListaPrecioFruta lpf ON lpf.CategoriaId = mp.CategoriaId AND lpf.CalibreApeamId = mp.CalibreApeamId
                    WHERE mp.CodigoSap = pt.MateriaPrimaItemCode
                      AND lpf.FechaInicio = @CostoEstimadoListaPrecioFecha
                      AND ((@CostoEstimadoListaPrecioProductorId IS NULL AND lpf.ProductorId IS NULL) OR lpf.ProductorId = @CostoEstimadoListaPrecioProductorId)
                 )
            END AS CostoEstimadoCalculado
        FROM Produccion.PalletDetalle pd
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = pd.ProductoTerminadoId
        LEFT JOIN Catalogos.Pais pais ON pais.Id = pt.MercadoDestinoPaisId
        WHERE pd.LoteId = @LoteId
          AND pt.MateriaPrimaItemCode IS NOT NULL
    ),
    PorMateriaPrima AS (
        SELECT
            MateriaPrimaItemCode,
            MAX(Mercado) AS Mercado,
            SUM(Kilogramos) AS KilogramosSeleccionados,
            SUM(Kilogramos * CostoRealCalculado) / NULLIF(SUM(CASE WHEN CostoRealCalculado IS NOT NULL THEN Kilogramos END), 0) AS CostoRealCalculado,
            SUM(Kilogramos * CostoEstimadoCalculado) / NULLIF(SUM(CASE WHEN CostoEstimadoCalculado IS NOT NULL THEN Kilogramos END), 0) AS CostoEstimadoCalculado
        FROM Detalle
        GROUP BY MateriaPrimaItemCode
    ),
    Total AS (
        SELECT SUM(KilogramosSeleccionados) AS TotalKilogramos FROM PorMateriaPrima
    )
    SELECT
        pm.MateriaPrimaItemCode,
        -- El nombre ya no pasa por ListaPrecioFruta (que ya no tiene ItemName) — sale directo
        -- de Catalogos.MateriaPrima, más simple y sin depender de que exista una vigencia.
        ISNULL(
            (SELECT TOP 1 mp.DescripcionSap FROM Catalogos.MateriaPrima mp WHERE mp.CodigoSap = pm.MateriaPrimaItemCode),
            pm.MateriaPrimaItemCode
        ) AS MateriaPrimaNombre,
        pm.Mercado,
        pm.KilogramosSeleccionados,
        CASE WHEN t.TotalKilogramos > 0 THEN pm.KilogramosSeleccionados * 100.0 / t.TotalKilogramos ELSE 0 END AS Porcentaje,
        CASE WHEN t.TotalKilogramos > 0 THEN @KgRecepcionado * pm.KilogramosSeleccionados / t.TotalKilogramos ELSE 0 END AS KilogramosComprados,
        ISNULL(gfc.CostoRealUnitario, pm.CostoRealCalculado) AS CostoRealUnitario,
        CAST(CASE WHEN gfc.CostoRealUnitario IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS CostoRealEsManual,
        ISNULL(gfc.CostoEstimadoUnitario, pm.CostoEstimadoCalculado) AS CostoEstimadoUnitario,
        CAST(CASE WHEN gfc.CostoEstimadoUnitario IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS CostoEstimadoEsManual
    FROM PorMateriaPrima pm
    CROSS JOIN Total t
    LEFT JOIN Gastos.GastoFrutaCategoria gfc ON gfc.GastoLoteId = @GastoLoteId AND gfc.MateriaPrimaItemCode = pm.MateriaPrimaItemCode
    ORDER BY MateriaPrimaNombre;
END
GO

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

    DECLARE @Precio DECIMAL(18,4), @ListaPrecioFecha DATE, @ListaPrecioProductorId INT,
            @ListaPrecioNumero TINYINT, @NecesitaListaPrecios BIT;
    SELECT TOP 1
        @Precio = ac.Precio,
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
                    SELECT CASE @ListaPrecioNumero WHEN 1 THEN lpf.Convencional WHEN 2 THEN lpf.Organico WHEN 3 THEN lpf.Nacional END
                    FROM Catalogos.MateriaPrima mp
                    JOIN Acopio.ListaPrecioFruta lpf ON lpf.CategoriaId = mp.CategoriaId AND lpf.CalibreApeamId = mp.CalibreApeamId
                    WHERE mp.CodigoSap = pt.MateriaPrimaItemCode
                      AND lpf.FechaInicio = @ListaPrecioFecha
                      AND ((@ListaPrecioProductorId IS NULL AND lpf.ProductorId IS NULL) OR lpf.ProductorId = @ListaPrecioProductorId)
                 )
            END AS CostoRealCalculado,
            CASE WHEN @CostoEstimadoListaPrecioFecha IS NULL THEN NULL
                 ELSE (
                    SELECT CASE @CostoEstimadoListaPrecioNumero WHEN 1 THEN lpf.Convencional WHEN 2 THEN lpf.Organico WHEN 3 THEN lpf.Nacional END
                    FROM Catalogos.MateriaPrima mp
                    JOIN Acopio.ListaPrecioFruta lpf ON lpf.CategoriaId = mp.CategoriaId AND lpf.CalibreApeamId = mp.CalibreApeamId
                    WHERE mp.CodigoSap = pt.MateriaPrimaItemCode
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
    SELECT Mercado, Kilogramos,
           Kilogramos * 100.0 / NULLIF(SUM(Kilogramos) OVER (), 0) AS Porcentaje,
           ImporteReal, ImporteEstimado
    FROM PorMercado
    UNION ALL
    SELECT N'Total', SUM(Kilogramos), 100.0, SUM(ImporteReal), SUM(ImporteEstimado)
    FROM PorMercado;
END
GO
