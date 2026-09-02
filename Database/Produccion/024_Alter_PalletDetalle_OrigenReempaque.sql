USE FrontOne;
GO

-- Una línea de Produccion.PalletDetalle ahora puede nacer de una Corrida (proceso normal) o de un
-- Reempaque (módulo Reempaques, ver 022_Schema_SP_Reempaque.sql) — exactamente una de las dos,
-- nunca ninguna ni las dos. Esto es lo que permite completar un pallet ya existente (normal o
-- nacido de un reempaque anterior) con cajas que salen de desarmar otro pallet: la caja vive en la
-- MISMA tabla Produccion.PalletDetalle sin importar su origen, y el origen se distingue por línea,
-- no por tabla. LoteId sigue NOT NULL en ambos casos — la trazabilidad de lote nunca se pierde.
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'CorridaId' AND is_nullable = 0)
BEGIN
    ALTER TABLE Produccion.PalletDetalle ALTER COLUMN CorridaId INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.PalletDetalle') AND name = 'ReempaqueDetalleId')
BEGIN
    ALTER TABLE Produccion.PalletDetalle ADD ReempaqueDetalleId INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Produccion_PalletDetalle_ReempaqueDetalle')
BEGIN
    ALTER TABLE Produccion.PalletDetalle ADD CONSTRAINT FK_Produccion_PalletDetalle_ReempaqueDetalle
        FOREIGN KEY (ReempaqueDetalleId) REFERENCES Produccion.ReempaqueDetalle (Id);
END
GO

-- Exactamente un origen por línea, nunca los dos ni ninguno.
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Produccion_PalletDetalle_Origen')
BEGIN
    ALTER TABLE Produccion.PalletDetalle ADD CONSTRAINT CK_Produccion_PalletDetalle_Origen
        CHECK ((CorridaId IS NOT NULL AND ReempaqueDetalleId IS NULL)
            OR (CorridaId IS NULL AND ReempaqueDetalleId IS NOT NULL));
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Produccion_PalletDetalle_ReempaqueDetalleId')
BEGIN
    CREATE INDEX IX_Produccion_PalletDetalle_ReempaqueDetalleId
        ON Produccion.PalletDetalle (ReempaqueDetalleId) WHERE ReempaqueDetalleId IS NOT NULL;
END
GO

-- ============================================================================
-- Ajustes a los SPs existentes de Pallet: CorridaId ahora puede ser NULL.
-- ============================================================================

-- Agrega Origen/ReempaqueFolio para la trazabilidad visible en PalletEditarForm (columna "Origen"
-- + hipervínculo "No. de Reempaque"). LoteEnProceso queda en 0 para una línea de reempaque — no
-- hay Corrida que consultar, y su saldo se gobierna desde Produccion.ReempaqueDetalle.
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
        d.VoiceCodeHigh,
        d.ReempaqueDetalleId,
        r.Folio AS ReempaqueFolio,
        CASE WHEN d.ReempaqueDetalleId IS NULL THEN 'Corrida' ELSE 'Reempaque' END AS OrigenDescripcion
    FROM Produccion.PalletDetalle d
    LEFT JOIN Produccion.Corrida c ON c.Id = d.CorridaId
    LEFT JOIN Produccion.ReempaqueDetalle rd ON rd.Id = d.ReempaqueDetalleId
    LEFT JOIN Produccion.Reempaque r ON r.Id = rd.ReempaqueId
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE d.PalletId = @PalletId
    ORDER BY d.Id;
END
GO

-- Rechaza si la línea vino de un reempaque: editar cajas exigiría reajustar el saldo del folio de
-- reempaque, algo que este SP no contempla. Borrar y recapturar desde Reempaques es suficiente.
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

    IF EXISTS (SELECT 1 FROM Produccion.PalletDetalle WHERE Id = @Id AND ReempaqueDetalleId IS NOT NULL)
    BEGIN
        THROW 50000, 'Esta línea proviene de un reempaque: modifícala desde el módulo de Reempaques.', 1;
    END

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
        VoiceCodeLow = NULL,
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

-- Eliminar una línea revierte su origen: si nació de una Corrida, revierte KilosProcesados de la
-- Corrida (como siempre); si nació de un Reempaque, devuelve el kilaje a ReempaqueDetalle.
-- KilosDisponibles y lo resta de Reempaque.KilosProcesados, rechazando si ese reempaque ya cerró.
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @PalletId INT, @CorridaId INT, @ReempaqueDetalleId INT, @Kilogramos DECIMAL(10,2);
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @ReempaqueDetalleId = ReempaqueDetalleId, @Kilogramos = Kilogramos
    FROM Produccion.PalletDetalle
    WHERE Id = @Id;

    IF @PalletId IS NULL
    BEGIN
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se pueden eliminar sus líneas.', 1;
    END

    IF @ReempaqueDetalleId IS NOT NULL AND EXISTS (
        SELECT 1 FROM Produccion.ReempaqueDetalle rd
        INNER JOIN Produccion.Reempaque r ON r.Id = rd.ReempaqueId
        WHERE rd.Id = @ReempaqueDetalleId AND r.Estatus <> 1)
    BEGIN
        THROW 50000, 'El reempaque que originó esta línea ya está cerrado: no se puede eliminar.', 1;
    END

    BEGIN TRANSACTION;

    DELETE FROM Produccion.PalletDetalle WHERE Id = @Id;

    IF @CorridaId IS NOT NULL
    BEGIN
        UPDATE Produccion.Corrida
        SET KilosProcesados = KilosProcesados - @Kilogramos
        WHERE Id = @CorridaId;
    END
    ELSE
    BEGIN
        UPDATE Produccion.ReempaqueDetalle SET KilosDisponibles = KilosDisponibles + @Kilogramos WHERE Id = @ReempaqueDetalleId;

        UPDATE r
        SET r.KilosProcesados = r.KilosProcesados - @Kilogramos
        FROM Produccion.Reempaque r
        INNER JOIN Produccion.ReempaqueDetalle rd ON rd.ReempaqueId = r.Id
        WHERE rd.Id = @ReempaqueDetalleId;
    END

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;
END
GO

-- Eliminar un Pallet completo revierte cada línea según su propio origen (Corrida o Reempaque)
-- antes de borrarlas, agrupando por cada origen distinto.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @Id AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se puede eliminar.', 1;
    END

    IF EXISTS (
        SELECT 1 FROM Produccion.PalletDetalle d
        INNER JOIN Produccion.ReempaqueDetalle rd ON rd.Id = d.ReempaqueDetalleId
        INNER JOIN Produccion.Reempaque r ON r.Id = rd.ReempaqueId
        WHERE d.PalletId = @Id AND r.Estatus <> 1)
    BEGIN
        THROW 50000, 'Este pallet tiene líneas de un reempaque ya cerrado: no se puede eliminar.', 1;
    END

    BEGIN TRANSACTION;

    UPDATE c
    SET c.KilosProcesados = c.KilosProcesados - agg.Kilos
    FROM Produccion.Corrida c
    INNER JOIN (
        SELECT CorridaId, SUM(Kilogramos) AS Kilos
        FROM Produccion.PalletDetalle
        WHERE PalletId = @Id AND CorridaId IS NOT NULL
        GROUP BY CorridaId
    ) AS agg ON agg.CorridaId = c.Id;

    UPDATE rd
    SET rd.KilosDisponibles = rd.KilosDisponibles + agg.Kilos
    FROM Produccion.ReempaqueDetalle rd
    INNER JOIN (
        SELECT ReempaqueDetalleId, SUM(Kilogramos) AS Kilos
        FROM Produccion.PalletDetalle
        WHERE PalletId = @Id AND ReempaqueDetalleId IS NOT NULL
        GROUP BY ReempaqueDetalleId
    ) AS agg ON agg.ReempaqueDetalleId = rd.Id;

    UPDATE r
    SET r.KilosProcesados = r.KilosProcesados - agg.Kilos
    FROM Produccion.Reempaque r
    INNER JOIN (
        SELECT rd.ReempaqueId, SUM(d.Kilogramos) AS Kilos
        FROM Produccion.PalletDetalle d
        INNER JOIN Produccion.ReempaqueDetalle rd ON rd.Id = d.ReempaqueDetalleId
        WHERE d.PalletId = @Id
        GROUP BY rd.ReempaqueId
    ) AS agg ON agg.ReempaqueId = r.Id;

    DELETE FROM Produccion.PalletDetalle WHERE PalletId = @Id;
    DELETE FROM Produccion.Pallet WHERE Id = @Id;

    COMMIT TRANSACTION;
END
GO

-- ============================================================================
-- Reempaques: salida hacia Produccion.Pallet/PalletDetalle
-- ============================================================================

-- Candidatos a recibir cajas de un reempaque: sin identificador de reempaque previo pendiente
-- (EsNeutro = 0), no bloqueados y en Estatus Vacío, Incompleto o En Proceso (1, 2, 7) — un pallet
-- Completo, Excedido, Empacado o Reempacado no puede recibir más cajas. En Proceso (7) es el
-- Granel no mixto, que nunca llega a un tope real de kilogramos (ver
-- 025_Alter_Pallet_GranelEnProceso.sql). Excluye los pallets que son ORIGEN de este mismo folio
-- (evita que un pallet reciba cajas de sí mismo).
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_ObtenerPalletsDestinoDisponibles
    @ReempaqueId INT,
    @Folio NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        p.Id, p.Folio, p.FechaCreacion, p.Estatus,
        tot.TotalCajas, tot.TotalKilogramos,
        CASE
            WHEN p.EsMixto = 1 THEN NULL
            WHEN p.ProductoTerminadoId IS NOT NULL THEN pte.CajasPorPallet
            ELSE NULL
        END AS CajasObjetivo,
        p.EsMixto,
        CASE WHEN tot.ProductosDistintos > 1 THEN 'Mixto' ELSE ISNULL(prim.DescripcionSap, '') END AS ProductoDescripcion
    FROM Produccion.Pallet p
    LEFT JOIN Catalogos.ProductoTerminado pte ON pte.Id = p.ProductoTerminadoId
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas, SUM(d.Kilogramos) AS TotalKilogramos,
               COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d WHERE d.PalletId = p.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = p.Id ORDER BY d.Id
    ) AS prim
    WHERE p.EsNeutro = 0
      AND p.Bloqueado = 0
      AND p.Estatus IN (1, 2, 7)
      AND NOT EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle rd WHERE rd.ReempaqueId = @ReempaqueId AND rd.PalletOrigenId = p.Id)
      AND (@Folio IS NULL OR @Folio = '' OR p.Folio LIKE '%' + @Folio + '%')
    ORDER BY p.FechaCreacionRegistro DESC;
END
GO

-- Grid de Salida: toda línea de Produccion.PalletDetalle que nació de este reempaque, sea cual sea
-- el pallet destino en el que terminó.
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_ObtenerDetalleSalida
    @ReempaqueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.Id AS PalletDetalleId, d.PalletId, pa.Folio AS PalletFolio, pa.Estatus AS PalletEstatus, pa.EsNeutro,
        d.LoteId, l.Folio AS LoteFolio,
        d.ProductoTerminadoId, pt.DescripcionSap AS ProductoDescripcion,
        d.Cajas, d.Kilogramos, d.ReempaqueDetalleId
    FROM Produccion.PalletDetalle d
    INNER JOIN Produccion.ReempaqueDetalle rd ON rd.Id = d.ReempaqueDetalleId
    INNER JOIN Produccion.Pallet pa ON pa.Id = d.PalletId
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE rd.ReempaqueId = @ReempaqueId
    ORDER BY d.Id;
END
GO

-- Gemelo de sp_PalletDetalle_Insertar pero contra el saldo de una línea de reempaque en vez de una
-- Corrida. No valida que el consumo exceda KilosDisponibles — deliberado, igual que el flujo
-- normal: el saldo puede quedar negativo temporalmente y el Ajuste Neutro lo cuadra antes de
-- cerrar. Devuelve el mismo contrato que sp_PalletDetalle_Insertar (Id, CodigoGtin,
-- CodigoTrazabilidad, FechaLote) para que PalletService.RecalcularGs1VoiceCodePalletAsync
-- funcione sin ramificar por origen.
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_InsertarDesdeReempaque
    @PalletId INT,
    @ReempaqueDetalleId INT,
    @ProductoTerminadoId INT,
    @Cajas INT = NULL,
    @Kilogramos DECIMAL(10,2) = NULL,
    @PorcentajeMateriaSeca DECIMAL(5,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @EsMixto BIT, @ProductoEncabezadoId INT, @Bloqueado BIT, @EsNeutro BIT, @EstatusPallet TINYINT;
    SELECT @EsMixto = EsMixto, @ProductoEncabezadoId = ProductoTerminadoId, @Bloqueado = Bloqueado,
           @EsNeutro = EsNeutro, @EstatusPallet = Estatus
    FROM Produccion.Pallet WHERE Id = @PalletId;

    -- Estatus 7 En Proceso = Granel no mixto, que nunca llega a un tope real de kilogramos (ver
    -- 025_Alter_Pallet_GranelEnProceso.sql) — sigue aceptando más mientras no esté Bloqueado.
    IF @Bloqueado = 1 OR @EsNeutro = 1 OR @EstatusPallet NOT IN (1, 2, 7)
    BEGIN
        THROW 50000, 'Solo se pueden agregar cajas a un pallet Vacío o Incompleto que no esté empacado.', 1;
    END

    DECLARE @ReempaqueId INT, @Estatus TINYINT;
    SELECT @ReempaqueId = rd.ReempaqueId, @Estatus = r.Estatus
    FROM Produccion.ReempaqueDetalle rd
    INNER JOIN Produccion.Reempaque r ON r.Id = rd.ReempaqueId
    WHERE rd.Id = @ReempaqueDetalleId;

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'Este reempaque ya está cerrado.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @ReempaqueId AND PalletOrigenId = @PalletId)
    BEGIN
        THROW 50000, 'Este pallet es origen del reempaque: no puede recibir cajas de sí mismo.', 1;
    END

    IF @EsMixto = 0
    BEGIN
        SET @ProductoTerminadoId = @ProductoEncabezadoId;
    END

    DECLARE @Presentacion TINYINT, @PesoNeto DECIMAL(10,2), @CajasPorPallet INT, @CodigoGtin NVARCHAR(20);
    SELECT @Presentacion = Presentacion, @PesoNeto = PesoNeto, @CajasPorPallet = CajasPorPallet, @CodigoGtin = CodigoGtin
    FROM Catalogos.ProductoTerminado WHERE Id = @ProductoTerminadoId;

    DECLARE @Kilos DECIMAL(10,2), @CajasFinal INT = NULL, @CajasPorPalletFinal INT = NULL;

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
            THROW 50000, 'El producto terminado seleccionado no tiene Peso Neto configurado.', 1;
        END
        IF @CajasPorPallet IS NULL OR @CajasPorPallet <= 0
        BEGIN
            THROW 50000, 'El producto terminado seleccionado no tiene Cajas por Pallet configurado.', 1;
        END

        IF @EsMixto = 0
        BEGIN
            DECLARE @CajasExistentes INT = ISNULL((SELECT SUM(Cajas) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId), 0);
            IF (@CajasExistentes + @Cajas) > @CajasPorPallet
            BEGIN
                THROW 50000, 'No se puede exceder el objetivo de cajas del producto.', 1;
            END
        END

        SET @Kilos = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));
        SET @CajasFinal = @Cajas;
        SET @CajasPorPalletFinal = @CajasPorPallet;
    END

    DECLARE @LoteId INT;
    SELECT @LoteId = LoteId FROM Produccion.ReempaqueDetalle WHERE Id = @ReempaqueDetalleId;

    DECLARE @CodigoTrazabilidad NVARCHAR(16), @FechaLote DATE;
    SELECT @CodigoTrazabilidad = CodigoTrazabilidad, @FechaLote = Fecha FROM Lotes.Lote WHERE Id = @LoteId;

    DECLARE @CodigoGs1128 NVARCHAR(60) = CASE
        WHEN @CodigoGtin IS NOT NULL AND @CodigoTrazabilidad IS NOT NULL
        THEN '(01)' + @CodigoGtin + '(13)' + FORMAT(@FechaLote, 'yyMMdd') + '(10)' + @CodigoTrazabilidad
        ELSE NULL
    END;

    BEGIN TRANSACTION;

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, ReempaqueDetalleId, LoteId, ProductoTerminadoId, Cajas, Kilogramos,
         PorcentajeMateriaSeca, CajasPorPallet, CodigoGs1128)
    VALUES
        (@PalletId, NULL, @ReempaqueDetalleId, @LoteId, @ProductoTerminadoId, @CajasFinal, @Kilos,
         ISNULL(@PorcentajeMateriaSeca, 0), @CajasPorPalletFinal, @CodigoGs1128);

    DECLARE @NuevoId INT = CAST(SCOPE_IDENTITY() AS INT);

    UPDATE Produccion.ReempaqueDetalle SET KilosDisponibles = KilosDisponibles - @Kilos WHERE Id = @ReempaqueDetalleId;
    UPDATE Produccion.Reempaque SET KilosProcesados = KilosProcesados + @Kilos WHERE Id = @ReempaqueId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @NuevoId AS Id, @CodigoGtin AS CodigoGtin, @CodigoTrazabilidad AS CodigoTrazabilidad, @FechaLote AS FechaLote;
END
GO

-- Pallet Neutro del reempaque: UN solo pallet por folio de reempaque (folio sintético
-- '0-R' + FolioReempaque, distinguible del neutro de corrida '0-' + FolioLote), con una línea de
-- detalle por cada ReempaqueDetalleId ajustado. Se crea la primera vez y se reusa en las
-- siguientes llamadas del mismo folio — evita la violación de UNIQUE que un folio constante por
-- reempaque provocaría si cada línea generara su propio pallet. Mismos productos SAP
-- (MERMA/DIFERENCIA PESO A FAVOR) y mismo criterio de signo que el neutro de producción normal:
-- MERMA positivo, DIFERENCIA PESO A FAVOR negativo (decidido por el caller).
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_CrearNeutro
    @ReempaqueId INT,
    @ReempaqueDetalleId INT,
    @ProductoTerminadoId INT,
    @Kilogramos DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Kilogramos IS NULL OR @Kilogramos = 0
    BEGIN
        THROW 50000, 'Captura un monto de Kilogramos distinto de cero.', 1;
    END

    DECLARE @FolioReempaque NVARCHAR(7), @Estatus TINYINT;
    SELECT @FolioReempaque = Folio, @Estatus = Estatus FROM Produccion.Reempaque WHERE Id = @ReempaqueId;

    IF @FolioReempaque IS NULL
    BEGIN
        THROW 50000, 'El reempaque ya no existe.', 1;
    END

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'Este reempaque ya está cerrado.', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle WHERE Id = @ReempaqueDetalleId AND ReempaqueId = @ReempaqueId)
    BEGIN
        THROW 50000, 'La línea de saldo ya no existe.', 1;
    END

    IF EXISTS (
        SELECT 1 FROM Produccion.PalletDetalle d
        INNER JOIN Produccion.Pallet p ON p.Id = d.PalletId
        WHERE p.EsNeutro = 1 AND d.ReempaqueDetalleId = @ReempaqueDetalleId)
    BEGIN
        THROW 50000, 'Este lote ya tiene un Pallet Neutro capturado: elimínalo antes de capturar uno nuevo.', 1;
    END

    DECLARE @Presentacion TINYINT;
    SELECT @Presentacion = Presentacion FROM Catalogos.ProductoTerminado WHERE Id = @ProductoTerminadoId;

    IF @Presentacion IS NULL OR @Presentacion <> 2
    BEGIN
        THROW 50000, 'El producto elegido debe ser Presentación Granel.', 1;
    END

    DECLARE @Folio NVARCHAR(20) = N'0-R' + @FolioReempaque;
    DECLARE @PalletNeutroId INT = (SELECT Id FROM Produccion.Pallet WHERE Folio = @Folio);

    BEGIN TRANSACTION;

    IF @PalletNeutroId IS NULL
    BEGIN
        DECLARE @LineaProduccionId INT;
        SELECT TOP 1 @LineaProduccionId = po.LineaProduccionId
        FROM Produccion.ReempaqueDetalle rd
        INNER JOIN Produccion.Pallet po ON po.Id = rd.PalletOrigenId
        WHERE rd.Id = @ReempaqueDetalleId;

        INSERT INTO Produccion.Pallet
            (Folio, FechaCreacion, HoraCreacion, Estatus, LineaProduccionId, EsMixto,
             PorcentajeMateriaSeca, Bloqueado, EsNeutro, PrimeraCorrida)
        VALUES
            (@Folio, CAST(SYSDATETIME() AS DATE), CAST(SYSDATETIME() AS TIME(0)), 1, @LineaProduccionId, 0,
             0, 0, 1, 1);

        SET @PalletNeutroId = CAST(SCOPE_IDENTITY() AS INT);
    END

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, ReempaqueDetalleId, LoteId, ProductoTerminadoId, Cajas, Kilogramos, PorcentajeMateriaSeca, CajasPorPallet)
    SELECT
        @PalletNeutroId, NULL, @ReempaqueDetalleId, LoteId, @ProductoTerminadoId, NULL, @Kilogramos, 0, NULL
    FROM Produccion.ReempaqueDetalle WHERE Id = @ReempaqueDetalleId;

    UPDATE Produccion.ReempaqueDetalle SET KilosDisponibles = KilosDisponibles - @Kilogramos WHERE Id = @ReempaqueDetalleId;
    UPDATE Produccion.Reempaque SET KilosProcesados = KilosProcesados + @Kilogramos WHERE Id = @ReempaqueId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletNeutroId;

    COMMIT TRANSACTION;

    SELECT @PalletNeutroId AS Id;
END
GO

-- Solo se puede eliminar un reempaque Abierto y SIN líneas de salida capturadas todavía (si ya
-- construyó pallets destino, hay que quitar esas líneas desde Pallets primero — eliminar el
-- reempaque no debe borrar cajas que ya viven en otro pallet). Revierte cada pallet origen a
-- Estatus = 5 Empacado con NoReempaque en NULL antes de borrar el saldo y el encabezado.
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Reempaque WHERE Id = @Id AND Estatus <> 1)
    BEGIN
        THROW 50000, 'Este reempaque ya está cerrado: no se puede eliminar.', 1;
    END

    IF EXISTS (
        SELECT 1 FROM Produccion.PalletDetalle d
        INNER JOIN Produccion.ReempaqueDetalle rd ON rd.Id = d.ReempaqueDetalleId
        WHERE rd.ReempaqueId = @Id)
    BEGIN
        THROW 50000, 'Este reempaque ya tiene cajas capturadas en pallets destino: quítalas antes de eliminarlo.', 1;
    END

    BEGIN TRANSACTION;

    UPDATE p
    SET p.Estatus = 5, p.NoReempaque = NULL
    FROM Produccion.Pallet p
    WHERE p.Id IN (SELECT DISTINCT PalletOrigenId FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @Id);

    DELETE FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @Id;
    DELETE FROM Produccion.Reempaque WHERE Id = @Id;

    COMMIT TRANSACTION;
END
GO
