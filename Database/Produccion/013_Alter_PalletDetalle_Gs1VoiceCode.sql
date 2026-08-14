USE FrontOne;
GO

-- Código GS1-128 completo (01)GTIN(13)FechaYYMMDD(10)CódigoTrazabilidad y VoiceCode (Low/High, ver
-- FrontOne.Shared/Utils/VoicePickCodeCalculator.cs) a nivel de LÍNEA de detalle del Pallet — se
-- calculan y guardan una sola vez al capturar/editar la línea (no se recalculan cada vez que se
-- imprime la etiqueta de Caja). El GS1 se arma aquí mismo en T-SQL (simple concat de texto); el
-- VoiceCode requiere CRC-16 (no práctico en T-SQL) y se calcula en C# (PalletService) justo después
-- del INSERT/UPDATE, usando el GTIN/CodigoTrazabilidad/FechaLote que este mismo SP regresa en su
-- SELECT final — así el caller no necesita una segunda consulta.
--
-- Si el producto no tiene GTIN capturado o el Lote todavía no tiene Código de Trazabilidad (puede
-- pasar, se completa poco después de crear el Lote), la línea se guarda igual y CodigoGs1128 queda
-- NULL — nunca bloquea la captura del pallet por un dato de catálogo incompleto (decisión explícita
-- del usuario).
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'CodigoGs1128')
BEGIN
    ALTER TABLE Produccion.PalletDetalle ADD CodigoGs1128 NVARCHAR(60) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'VoiceCodeLow')
BEGIN
    ALTER TABLE Produccion.PalletDetalle ADD VoiceCodeLow CHAR(2) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'VoiceCodeHigh')
BEGIN
    ALTER TABLE Produccion.PalletDetalle ADD VoiceCodeHigh CHAR(2) NULL;
END
GO

-- Mismo cuerpo que 009_Alter_PalletDetalle_Granel.sql (Caja/Granel), + resolución de
-- CodigoGtin/CodigoTrazabilidad/FechaLote y armado de CodigoGs1128, + esos 3 datos crudos en el
-- SELECT final para que PalletService calcule el VoiceCode sin una segunda consulta.
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

    DECLARE @Presentacion TINYINT, @PesoNeto DECIMAL(10,2), @CajasPorPallet INT, @CodigoGtin NVARCHAR(20);
    SELECT @Presentacion = Presentacion, @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet, @CodigoGtin = CodigoGtin
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

    DECLARE @CodigoTrazabilidad NVARCHAR(16), @FechaLote DATE;
    SELECT @CodigoTrazabilidad = CodigoTrazabilidad, @FechaLote = Fecha
    FROM Lotes.Lote
    WHERE Id = @LoteId;

    DECLARE @CodigoGs1128 NVARCHAR(60) = CASE
        WHEN @CodigoGtin IS NOT NULL AND @CodigoTrazabilidad IS NOT NULL
        THEN '(01)' + @CodigoGtin + '(13)' + FORMAT(@FechaLote, 'yyMMdd') + '(10)' + @CodigoTrazabilidad
        ELSE NULL
    END;

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, LoteId, ProductoTerminadoId, Cajas, Kilogramos, PorcentajeMateriaSeca, CajasPorPallet, CodigoGs1128)
    VALUES
        (@PalletId, @CorridaId, @LoteId, @ProductoTerminadoId, @CajasFinal, @Kilos, ISNULL(@PorcentajeMateriaSeca, 0), @CajasPorPalletFinal, @CodigoGs1128);

    DECLARE @NuevoId INT = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Kilos
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @NuevoId AS Id, @CodigoGtin AS CodigoGtin, @CodigoTrazabilidad AS CodigoTrazabilidad, @FechaLote AS FechaLote;
END
GO

-- Mismo patrón que Insertar: recalcula CodigoGs1128 con el LoteId ya asociado a la línea (no cambia
-- en Actualizar) y el ProductoTerminadoId nuevo (si cambió, cambia el GTIN).
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

    DECLARE @PalletId INT, @CorridaId INT, @LoteId INT, @KilosAnteriores DECIMAL(10,2);
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @LoteId = LoteId, @KilosAnteriores = Kilogramos
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

    DECLARE @Presentacion TINYINT, @PesoNeto DECIMAL(10,2), @CajasPorPallet INT, @CodigoGtin NVARCHAR(20);
    SELECT @Presentacion = Presentacion, @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet, @CodigoGtin = CodigoGtin
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

    DECLARE @CodigoTrazabilidad NVARCHAR(16), @FechaLote DATE;
    SELECT @CodigoTrazabilidad = CodigoTrazabilidad, @FechaLote = Fecha
    FROM Lotes.Lote
    WHERE Id = @LoteId;

    DECLARE @CodigoGs1128 NVARCHAR(60) = CASE
        WHEN @CodigoGtin IS NOT NULL AND @CodigoTrazabilidad IS NOT NULL
        THEN '(01)' + @CodigoGtin + '(13)' + FORMAT(@FechaLote, 'yyMMdd') + '(10)' + @CodigoTrazabilidad
        ELSE NULL
    END;

    UPDATE Produccion.PalletDetalle
    SET ProductoTerminadoId = @ProductoTerminadoId,
        Cajas = @CajasFinal,
        Kilogramos = @Kilos,
        PorcentajeMateriaSeca = ISNULL(@PorcentajeMateriaSeca, 0),
        CajasPorPallet = @CajasPorPalletFinal,
        CodigoGs1128 = @CodigoGs1128,
        VoiceCodeLow = NULL,   -- se recalcula abajo en la app, vía sp_PalletDetalle_ActualizarVoiceCode
        VoiceCodeHigh = NULL
    WHERE Id = @Id;

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Delta
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @CodigoGtin AS CodigoGtin, @CodigoTrazabilidad AS CodigoTrazabilidad, @FechaLote AS FechaLote;
END
GO

-- Actualiza el VoiceCode Low/High (calculado en C# vía VoicePickCodeCalculator, no práctico en
-- T-SQL) justo después de Insertar/Actualizar la línea — SP chico y dedicado, separado del
-- Insertar/Actualizar principal para no acoplar la lógica de CRC-16 al flujo transaccional de arriba.
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_ActualizarVoiceCode
    @Id            INT,
    @VoiceCodeLow  CHAR(2) = NULL,
    @VoiceCodeHigh CHAR(2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Produccion.PalletDetalle
    SET VoiceCodeLow = @VoiceCodeLow,
        VoiceCodeHigh = @VoiceCodeHigh
    WHERE Id = @Id;
END
GO

-- Agrega las 3 columnas nuevas al detalle que ya trae el grid de captura del Pallet.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerDetalle
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Id,
        d.PalletId,
        d.CorridaId,
        d.LoteId,
        l.Folio AS LoteFolio,
        d.ProductoTerminadoId,
        pt.CodigoSap AS ProductoCodigoSap,
        pt.DescripcionSap AS ProductoDescripcion,
        d.Cajas,
        d.Kilogramos,
        d.PorcentajeMateriaSeca,
        d.CajasPorPallet,
        CAST(CASE WHEN c.Estatus = 1 THEN 1 ELSE 0 END AS BIT) AS LoteEnProceso,
        d.CodigoGs1128,
        d.VoiceCodeLow,
        d.VoiceCodeHigh
    FROM Produccion.PalletDetalle d
    INNER JOIN Produccion.Corrida c ON c.Id = d.CorridaId
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE d.PalletId = @PalletId
    ORDER BY d.Id;
END
GO
