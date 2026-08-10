USE FrontOne;
GO

-- Se permite que las cajas capturadas en un Pallet excedan los Kilogramos disponibles de la
-- Corrida (KilosAProcesar - KilosProcesados): el peso estándar del producto terminado no siempre
-- coincide exactamente con el peso real recepcionado del lote. La diferencia (a favor o en contra)
-- se corregirá más adelante en el futuro módulo de Ajuste de Lotes (Diferencia a Favor / Merma).
-- Por lo demás, ambos SPs quedan igual que en 005_Alter_Pallet_ProductoEncabezado.sql — solo se
-- quita el THROW de saldo insuficiente; KilosProcesados sigue acumulándose igual (puede quedar en
-- negativo respecto al saldo, es esperado hasta que exista el ajuste).

CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Insertar
    @PalletId INT,
    @CorridaId INT,
    @ProductoTerminadoId INT,
    @Cajas INT,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Cajas IS NULL OR @Cajas <= 0
    BEGIN
        THROW 50000, 'Las cajas deben ser mayores a cero.', 1;
    END

    DECLARE @EsMixto BIT, @ProductoEncabezadoId INT, @PalletBloqueado BIT;
    SELECT @EsMixto = EsMixto, @ProductoEncabezadoId = ProductoTerminadoId, @PalletBloqueado = Bloqueado
    FROM Produccion.Pallet
    WHERE Id = @PalletId;

    IF @PalletBloqueado = 1
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se pueden agregar líneas.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        SET @ProductoTerminadoId = @ProductoEncabezadoId;
    END

    DECLARE @PesoNeto DECIMAL(10,2), @CajasPorPallet INT;
    SELECT @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet
    FROM Catalogos.ProductoTerminado
    WHERE Id = @ProductoTerminadoId;

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

    DECLARE @Kilogramos DECIMAL(10,2) = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, LoteId, ProductoTerminadoId, Cajas, Kilogramos, PorcentajeMateriaSeca, CajasPorPallet)
    VALUES
        (@PalletId, @CorridaId, @LoteId, @ProductoTerminadoId, @Cajas, @Kilogramos, ISNULL(@PorcentajeMateriaSeca, 0), @CajasPorPallet);

    DECLARE @NuevoId INT = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Kilogramos
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @NuevoId AS Id;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Actualizar
    @Id INT,
    @ProductoTerminadoId INT,
    @Cajas INT,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Cajas IS NULL OR @Cajas <= 0
    BEGIN
        THROW 50000, 'Las cajas deben ser mayores a cero.', 1;
    END

    DECLARE @PalletId INT, @CorridaId INT, @KilosAnteriores DECIMAL(10,2), @CajasAnteriores INT;
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @KilosAnteriores = Kilogramos, @CajasAnteriores = Cajas
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

    DECLARE @PesoNeto DECIMAL(10,2), @CajasPorPallet INT;
    SELECT @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet
    FROM Catalogos.ProductoTerminado
    WHERE Id = @ProductoTerminadoId;

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

    BEGIN TRANSACTION;

    DECLARE @Estatus TINYINT;
    SELECT @Estatus = Estatus
    FROM Produccion.Corrida WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @CorridaId;

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida de este lote ya fue finalizada: esta línea ya no se puede modificar.', 1;
    END

    DECLARE @Kilogramos DECIMAL(10,2) = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));
    DECLARE @Delta DECIMAL(10,2) = @Kilogramos - @KilosAnteriores;

    UPDATE Produccion.PalletDetalle
    SET ProductoTerminadoId = @ProductoTerminadoId,
        Cajas = @Cajas,
        Kilogramos = @Kilogramos,
        PorcentajeMateriaSeca = ISNULL(@PorcentajeMateriaSeca, 0),
        CajasPorPallet = @CajasPorPallet
    WHERE Id = @Id;

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Delta
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;
END
GO
