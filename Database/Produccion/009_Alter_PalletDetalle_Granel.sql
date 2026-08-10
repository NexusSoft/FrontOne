USE FrontOne;
GO

-- Pallets a granel: cuando el Producto Terminado de una línea es Presentación Granel (2), no hay
-- Cajas que empacar — el usuario captura Kilogramos directo. Cajas/CajasPorPallet pasan a NULL en
-- esa línea (no aplican). Producto Caja (1) conserva exactamente el comportamiento actual.
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'Cajas' AND is_nullable = 0)
BEGIN
    ALTER TABLE Produccion.PalletDetalle ALTER COLUMN Cajas INT NULL;
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'CajasPorPallet' AND is_nullable = 0)
BEGIN
    ALTER TABLE Produccion.PalletDetalle ALTER COLUMN CajasPorPallet INT NULL;
END
GO

-- ============================================================================
-- Recálculo de Estatus y % Materia Seca del encabezado.
--
-- % Materia Seca ahora pondera por KILOGRAMOS (no por Cajas) para TODAS las líneas, Caja y Granel
-- por igual — más correcto matemáticamente y resuelve el caso Granel sin lógica especial (una
-- línea Granel no tiene Cajas con las que ponderar).
--
-- Estatus: un Pallet NO mixto cuyo Producto de encabezado es Granel no tiene "Cajas Objetivo" (el
-- concepto no aplica, confirmado con el usuario) — con al menos una línea capturada (Kilogramos >
-- 0) el pallet ya es Completo, sin objetivo que cumplir. El resto de casos (Caja no mixto, mixto)
-- no cambia: siguen evaluando @TotalCajas contra @Objetivo.
-- ============================================================================
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Recalcular
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        RETURN;
    END

    DECLARE @TotalCajas INT = 0;
    DECLARE @TotalKilogramos DECIMAL(18,2) = 0;
    DECLARE @SumaPonderadaKilos DECIMAL(18,4) = 0;
    DECLARE @Objetivo INT = 0;
    DECLARE @EsMixto BIT, @ProductoTerminadoId INT;

    SELECT @EsMixto = EsMixto, @ProductoTerminadoId = ProductoTerminadoId
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    SELECT @TotalCajas = ISNULL(SUM(Cajas), 0),
           @TotalKilogramos = ISNULL(SUM(Kilogramos), 0),
           @SumaPonderadaKilos = ISNULL(SUM(Kilogramos * PorcentajeMateriaSeca), 0)
    FROM Produccion.PalletDetalle
    WHERE PalletId = @PalletId;

    DECLARE @Presentacion TINYINT = NULL;

    IF @EsMixto = 0
    BEGIN
        SELECT @Objetivo = ISNULL(CajasPorPallet, 0), @Presentacion = Presentacion
        FROM Catalogos.ProductoTerminado
        WHERE Id = @ProductoTerminadoId;
    END
    ELSE
    BEGIN
        SELECT TOP 1 @Objetivo = CajasPorPallet
        FROM Produccion.PalletDetalle
        WHERE PalletId = @PalletId
        ORDER BY Id;
    END

    UPDATE Produccion.Pallet
    SET Estatus = CASE
                      WHEN @TotalKilogramos = 0 THEN 1                    -- Vacío
                      WHEN @EsMixto = 0 AND @Presentacion = 2 THEN 3      -- Granel no mixto: algo capturado ya es Completo
                      WHEN @Objetivo <= 0 THEN 2                         -- sin objetivo válido: se queda Incompleto
                      WHEN @TotalCajas < @Objetivo THEN 2                -- Incompleto
                      WHEN @TotalCajas = @Objetivo THEN 3                -- Completo
                      ELSE 4                                             -- Excedido
                  END,
        PorcentajeMateriaSeca = CASE WHEN @TotalKilogramos = 0 THEN 0
                                     ELSE CAST(@SumaPonderadaKilos / @TotalKilogramos AS DECIMAL(5,2))
                                END
    WHERE Id = @PalletId;
END
GO

-- Agrega una línea. Para pallets NO mixtos se IGNORA el @ProductoTerminadoId que mande el cliente
-- y se sustituye por el del encabezado (defensa en profundidad, igual que antes). Rama por
-- Presentación del producto resuelto:
--   Caja: exactamente la lógica ya vigente — @Cajas obligatorio > 0, valida PesoNeto/CajasPorPallet
--         configurados, tope de cajas si no-mixto, Kilogramos = PesoNeto * Cajas.
--   Granel: @Kilogramos obligatorio > 0; Cajas/CajasPorPallet se insertan NULL; sin validación de
--           PesoNeto/CajasPorPallet del catálogo (no aplican); sin tope de cajas (no hay objetivo).
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Insertar
    @PalletId INT,
    @CorridaId INT,
    @ProductoTerminadoId INT,
    @Cajas INT = NULL,
    @Kilogramos DECIMAL(10,2) = NULL,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @EsMixto BIT, @ProductoEncabezadoId INT, @PalletBloqueado BIT;
    SELECT @EsMixto = EsMixto, @ProductoEncabezadoId = ProductoTerminadoId, @PalletBloqueado = Bloqueado
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    IF @PalletBloqueado = 1
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se le pueden agregar líneas.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        SET @ProductoTerminadoId = @ProductoEncabezadoId;
    END

    DECLARE @Presentacion TINYINT, @PesoNeto DECIMAL(10,2), @CajasPorPallet INT;
    SELECT @Presentacion = Presentacion, @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet
    FROM Catalogos.ProductoTerminado
    WHERE Id = @ProductoTerminadoId;

    DECLARE @Kilos DECIMAL(10,2);
    DECLARE @CajasFinal INT = NULL;
    DECLARE @CajasPorPalletFinal INT = NULL;

    IF @Presentacion = 2 -- Granel
    BEGIN
        IF @Kilogramos IS NULL OR @Kilogramos <= 0
        BEGIN
            THROW 50000, 'Captura los Kilogramos de esta línea.', 1;
        END

        SET @Kilos = @Kilogramos;
    END
    ELSE -- Caja
    BEGIN
        IF @Cajas IS NULL OR @Cajas <= 0
        BEGIN
            THROW 50000, 'Las cajas deben ser mayores a cero.', 1;
        END

        IF @PesoNeto IS NULL OR @PesoNeto <= 0
        BEGIN
            THROW 50000, 'El producto terminado seleccionado no tiene Peso Neto configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
        END

        IF @CajasPorPallet IS NULL OR @CajasPorPallet <= 0
        BEGIN
            THROW 50000, 'El producto terminado seleccionado no tiene Cajas por Pallet configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
        END

        IF @EsMixto = 0
        BEGIN
            DECLARE @CajasExistentes INT = ISNULL((SELECT SUM(Cajas) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId), 0);
            IF (@CajasExistentes + @Cajas) > @CajasPorPallet
            BEGIN
                THROW 50000, 'No se puede exceder el objetivo de cajas del producto: ya hay cajas capturadas que, sumadas a esta línea, superan el total configurado en Cajas por Pallet.', 1;
            END
        END

        SET @Kilos = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));
        SET @CajasFinal = @Cajas;
        SET @CajasPorPalletFinal = @CajasPorPallet;
    END

    BEGIN TRANSACTION;

    DECLARE @LoteId INT, @Estatus TINYINT;
    SELECT @LoteId = LoteId, @Estatus = Estatus
    FROM Produccion.Corrida WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @CorridaId;

    IF @LoteId IS NULL
    BEGIN
        THROW 50000, 'El lote seleccionado ya no tiene una corrida abierta.', 1;
    END

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida de este lote ya fue finalizada: ya no se le pueden cargar cajas.', 1;
    END

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, LoteId, ProductoTerminadoId, Cajas, Kilogramos, PorcentajeMateriaSeca, CajasPorPallet)
    VALUES
        (@PalletId, @CorridaId, @LoteId, @ProductoTerminadoId, @CajasFinal, @Kilos, ISNULL(@PorcentajeMateriaSeca, 0), @CajasPorPalletFinal);

    DECLARE @NuevoId INT = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Kilos
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @NuevoId AS Id;
END
GO

-- Editar una línea: mismo patrón condicional Caja/Granel que el Insertar. El delta de Kilogramos
-- contra la Corrida sigue igual en ambos casos.
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Actualizar
    @Id INT,
    @ProductoTerminadoId INT,
    @Cajas INT = NULL,
    @Kilogramos DECIMAL(10,2) = NULL,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @PalletId INT, @CorridaId INT, @KilosAnteriores DECIMAL(10,2);
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @KilosAnteriores = Kilogramos
    FROM Produccion.PalletDetalle
    WHERE Id = @Id;

    IF @PalletId IS NULL
    BEGIN
        THROW 50000, 'La línea de detalle ya no existe.', 1;
    END

    DECLARE @EsMixto BIT, @ProductoEncabezadoId INT, @PalletBloqueado BIT;
    SELECT @EsMixto = EsMixto, @ProductoEncabezadoId = ProductoTerminadoId, @PalletBloqueado = Bloqueado
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    IF @PalletBloqueado = 1
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se pueden modificar sus líneas.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        SET @ProductoTerminadoId = @ProductoEncabezadoId;
    END

    DECLARE @Presentacion TINYINT, @PesoNeto DECIMAL(10,2), @CajasPorPallet INT;
    SELECT @Presentacion = Presentacion, @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet
    FROM Catalogos.ProductoTerminado
    WHERE Id = @ProductoTerminadoId;

    DECLARE @Kilos DECIMAL(10,2);
    DECLARE @CajasFinal INT = NULL;
    DECLARE @CajasPorPalletFinal INT = NULL;

    IF @Presentacion = 2 -- Granel
    BEGIN
        IF @Kilogramos IS NULL OR @Kilogramos <= 0
        BEGIN
            THROW 50000, 'Captura los Kilogramos de esta línea.', 1;
        END

        SET @Kilos = @Kilogramos;
    END
    ELSE -- Caja
    BEGIN
        IF @Cajas IS NULL OR @Cajas <= 0
        BEGIN
            THROW 50000, 'Las cajas deben ser mayores a cero.', 1;
        END

        IF @PesoNeto IS NULL OR @PesoNeto <= 0
        BEGIN
            THROW 50000, 'El producto terminado seleccionado no tiene Peso Neto configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
        END

        IF @CajasPorPallet IS NULL OR @CajasPorPallet <= 0
        BEGIN
            THROW 50000, 'El producto terminado seleccionado no tiene Cajas por Pallet configurado: captúralo en el catálogo de Productos Terminados antes de usarlo en un pallet.', 1;
        END

        IF @EsMixto = 0
        BEGIN
            DECLARE @CajasExistentes INT = ISNULL(
                (SELECT SUM(Cajas) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId AND Id <> @Id), 0);
            IF (@CajasExistentes + @Cajas) > @CajasPorPallet
            BEGIN
                THROW 50000, 'No se puede exceder el objetivo de cajas del producto: ya hay cajas capturadas que, sumadas a esta línea, superan el total configurado en Cajas por Pallet.', 1;
            END
        END

        SET @Kilos = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));
        SET @CajasFinal = @Cajas;
        SET @CajasPorPalletFinal = @CajasPorPallet;
    END

    BEGIN TRANSACTION;

    DECLARE @Estatus TINYINT;
    SELECT @Estatus = Estatus
    FROM Produccion.Corrida WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @CorridaId;

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida de este lote ya fue finalizada: esta línea ya no se puede modificar.', 1;
    END

    DECLARE @Delta DECIMAL(10,2) = @Kilos - @KilosAnteriores;

    UPDATE Produccion.PalletDetalle
    SET ProductoTerminadoId = @ProductoTerminadoId,
        Cajas = @CajasFinal,
        Kilogramos = @Kilos,
        PorcentajeMateriaSeca = ISNULL(@PorcentajeMateriaSeca, 0),
        CajasPorPallet = @CajasPorPalletFinal
    WHERE Id = @Id;

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Delta
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;
END
GO
