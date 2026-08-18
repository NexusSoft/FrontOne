USE FrontOne;
GO

-- Grid de la pestaña Fruta: 1 fila por Materia Prima del Lote (Catalogos.ProductoTerminado.
-- MateriaPrimaItemCode — el mismo campo del combo "Materia Prima" de ProductoTerminadoEditarForm,
-- ej. "EXP CAL 14 CAT 1"). NO es Catalogos.Categoria (catálogo genérico, 2-3 valores) ni tampoco
-- 1 fila por Producto Terminado (demasiado granular: cada empaque/calibre es su propio Producto
-- Terminado aunque comparta Materia Prima) — corregido dos veces a pedido del usuario tras ver
-- el grid en vivo. Varios Producto Terminado que comparten la misma Materia Prima se SUMAN en
-- una sola fila. El nombre a mostrar sale de Acopio.ListaPrecioFruta.ItemName (mismo ItemCode,
-- la vigencia más reciente que exista) porque MateriaPrimaItemCode es solo el código, sin nombre
-- propio cacheado en FrontOne; si todavía no hay ninguna Lista de Precio capturada para ese
-- código, se muestra el código tal cual. Kilogramos Seleccionados sale de
-- Produccion.PalletDetalle (lo que realmente se empacó/clasificó); Kilogramos Comprados es el
-- total recepcionado del Lote (Recepcion.RecepcionFruta.PesoNeto) repartido con el mismo % que
-- Seleccionados, porque RecepcionFruta no tiene desglose propio (decisión confirmada con el
-- usuario). El Costo Real/Estimado de cada línea se resuelve por Fijo (Acopio.AcuerdoCorte.Precio)
-- o Banda (match por MateriaPrimaItemCode + Variedad + vigencia contra Acopio.ListaPrecioFruta,
-- columna Lista{N} según AcuerdoCorte.ListaPrecioNumero / GastoLote.CostoEstimadoListaPrecioNumero).
-- Gastos.GastoFrutaCategoria.CostoRealUnitario/CostoEstimadoUnitario, cuando no es NULL,
-- sobreescribe el calculado (edición manual).
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
            -- Mercado por línea (México = Nacional, otro país = Exportación; Merma por
            -- convención de nombre del Producto Terminado, prefijo "Z)") — mismo criterio que
            -- sp_GastoFrutaCategoria_ObtenerResumenMercado.
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
          AND pt.MateriaPrimaItemCode IS NOT NULL
    ),
    PorMateriaPrima AS (
        SELECT
            MateriaPrimaItemCode,
            -- Una Materia Prima siempre cae en un solo Mercado; MAX es solo para resolver el
            -- tipo de agregación de SQL Server, no una regla de negocio real.
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
        ISNULL(
            (SELECT TOP 1 lpf.ItemName FROM Acopio.ListaPrecioFruta lpf WHERE lpf.ItemCode = pm.MateriaPrimaItemCode ORDER BY lpf.FechaInicio DESC),
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

CREATE OR ALTER PROCEDURE Gastos.sp_GastoFrutaCategoria_GuardarCostoReal
    @GastoLoteId         INT,
    @MateriaPrimaItemCode NVARCHAR(50),
    @CostoRealUnitario   DECIMAL(18,4) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Gastos.GastoFrutaCategoria WHERE GastoLoteId = @GastoLoteId AND MateriaPrimaItemCode = @MateriaPrimaItemCode)
    BEGIN
        UPDATE Gastos.GastoFrutaCategoria
        SET CostoRealUnitario = @CostoRealUnitario
        WHERE GastoLoteId = @GastoLoteId AND MateriaPrimaItemCode = @MateriaPrimaItemCode;
    END
    ELSE
    BEGIN
        INSERT INTO Gastos.GastoFrutaCategoria (GastoLoteId, MateriaPrimaItemCode, CostoRealUnitario)
        VALUES (@GastoLoteId, @MateriaPrimaItemCode, @CostoRealUnitario);
    END
END
GO

CREATE OR ALTER PROCEDURE Gastos.sp_GastoFrutaCategoria_GuardarCostoEstimado
    @GastoLoteId           INT,
    @MateriaPrimaItemCode  NVARCHAR(50),
    @CostoEstimadoUnitario DECIMAL(18,4) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Gastos.GastoFrutaCategoria WHERE GastoLoteId = @GastoLoteId AND MateriaPrimaItemCode = @MateriaPrimaItemCode)
    BEGIN
        UPDATE Gastos.GastoFrutaCategoria
        SET CostoEstimadoUnitario = @CostoEstimadoUnitario
        WHERE GastoLoteId = @GastoLoteId AND MateriaPrimaItemCode = @MateriaPrimaItemCode;
    END
    ELSE
    BEGIN
        INSERT INTO Gastos.GastoFrutaCategoria (GastoLoteId, MateriaPrimaItemCode, CostoEstimadoUnitario)
        VALUES (@GastoLoteId, @MateriaPrimaItemCode, @CostoEstimadoUnitario);
    END
END
GO
