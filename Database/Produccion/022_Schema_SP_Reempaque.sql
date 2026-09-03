USE FrontOne;
GO

-- Módulo Reempaques: desarma uno o más pallets ya armados (nunca Pallet Neutro, nunca uno que ya
-- se reempacó) y libera sus kilos, reservados por lote, para que se depositen en pallets del
-- módulo de Pallets — el mismo Produccion.Pallet/PalletDetalle del proceso normal, nunca una tabla
-- paralela. Así un pallet normal incompleto (ej. [15498], 50 de 80 cajas) o uno que ya nació de un
-- reempaque anterior (ej. [15879], 60 de 66) se puede completar con cajas de un reempaque nuevo.
--
-- No toca la Corrida original ni la Liquidación (Gastos.GastoLote) del lote — proceso completamente
-- aparte, igual de válido si el lote ya se envió a SAP. Los kilos de cada línea del pallet origen
-- regresan reservados a su mismo LoteId, pero solo disponibles dentro de este folio (Produccion.
-- ReempaqueDetalle), nunca al saldo general de la Corrida. Al cerrar, cada pallet origen pasa a
-- Estatus 6 (Reempacado) y llena NoReempaque — columnas que ya existían en Produccion.Pallet
-- reservadas a propósito para este módulo.
--
-- Mismo patrón "mini-corrida" que Produccion.Corrida/Pallet: KilosAProcesar fijo al agregar
-- pallets de entrada, KilosProcesados se acumula con cada línea que sale hacia un pallet destino, y
-- el cierre exige saldo en 0 EN CADA LOTE POR SEPARADO (nunca compensado entre lotes) — el Pallet
-- Neutro de Reempaque (mismos productos MERMA/DIFERENCIA PESO A FAVOR que el neutro de producción)
-- es la válvula para llegar a 0 cuando no ajusta exacto.
--
-- La salida (qué pallet destino recibió qué kilos) vive en Produccion.PalletDetalle, con las
-- columnas que agrega 024_Alter_PalletDetalle_OrigenReempaque.sql — este script solo crea el
-- encabezado del reempaque y el saldo de entrada; 024 debe correr después.
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================================
-- Tablas
-- ============================================================================

-- Folio del PROCESO de reempaque: consecutivo propio, independiente del de Pallet — no tiene nada
-- que ver con la numeración de pallets.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqReempaqueFolio' AND schema_id = SCHEMA_ID('Produccion'))
BEGIN
    CREATE SEQUENCE Produccion.SeqReempaqueFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Produccion.Reempaque'))
BEGIN
    CREATE TABLE Produccion.Reempaque
    (
        Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Produccion_Reempaque PRIMARY KEY,
        Folio                   NVARCHAR(7)     NOT NULL,
        FechaCreacion           DATE            NOT NULL,
        HoraCreacion            TIME(0)         NOT NULL,
        Motivo                  NVARCHAR(300)   NOT NULL,
        Estatus                 TINYINT         NOT NULL CONSTRAINT DF_Produccion_Reempaque_Estatus DEFAULT (1), -- 1 Abierto, 2 Cerrado
        KilosAProcesar          DECIMAL(10,2)   NOT NULL CONSTRAINT DF_Produccion_Reempaque_KilosAProcesar DEFAULT (0),
        KilosProcesados         DECIMAL(10,2)   NOT NULL CONSTRAINT DF_Produccion_Reempaque_KilosProcesados DEFAULT (0),
        FechaCierre             DATETIME2       NULL,
        FechaCreacionRegistro   DATETIME2       NOT NULL CONSTRAINT DF_Produccion_Reempaque_FechaCreacionRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Produccion_Reempaque_Folio UNIQUE (Folio)
    );
END
GO

-- Saldo GUARDADO (no derivado) de kilos disponibles por línea de pallet origen, reservado a este
-- folio. Una fila por cada línea de PalletDetalle del pallet que entra — un pallet de 3 lotes
-- genera 3 filas. KilosDisponibles puede quedar en negativo temporalmente si la salida real pesó
-- más que lo reservado (igual que Corrida.KilosProcesados puede exceder KilosAProcesar) — el
-- Pallet Neutro de Reempaque lo regresa a 0 en cualquier dirección; sp_Reempaque_Cerrar exige 0
-- exacto en TODAS las filas, sin compensar entre ellas.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Produccion.ReempaqueDetalle'))
BEGIN
    CREATE TABLE Produccion.ReempaqueDetalle
    (
        Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Produccion_ReempaqueDetalle PRIMARY KEY,
        ReempaqueId             INT             NOT NULL CONSTRAINT FK_Produccion_ReempaqueDetalle_Reempaque REFERENCES Produccion.Reempaque (Id),
        PalletOrigenId          INT             NOT NULL CONSTRAINT FK_Produccion_ReempaqueDetalle_PalletOrigen REFERENCES Produccion.Pallet (Id),
        PalletDetalleOrigenId   INT             NOT NULL CONSTRAINT FK_Produccion_ReempaqueDetalle_PalletDetalleOrigen REFERENCES Produccion.PalletDetalle (Id),
        LoteId                  INT             NOT NULL CONSTRAINT FK_Produccion_ReempaqueDetalle_Lote REFERENCES Lotes.Lote (Id),
        ProductoTerminadoOrigenId INT           NOT NULL CONSTRAINT FK_Produccion_ReempaqueDetalle_ProductoOrigen REFERENCES Catalogos.ProductoTerminado (Id),
        CajasEntrada            INT             NULL,
        KilosEntrada            DECIMAL(10,2)   NOT NULL,
        KilosDisponibles        DECIMAL(10,2)   NOT NULL,
        FechaCreacionRegistro   DATETIME2       NOT NULL CONSTRAINT DF_Produccion_ReempaqueDetalle_FechaCreacionRegistro DEFAULT (SYSUTCDATETIME())
    );
END
GO

-- ============================================================================
-- Encabezado del Reempaque
-- ============================================================================

CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_Insertar
    @Motivo NVARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Motivo IS NULL OR LTRIM(RTRIM(@Motivo)) = ''
    BEGIN
        THROW 50000, 'Captura el motivo del reempaque.', 1;
    END

    DECLARE @Folio NVARCHAR(7) =
        RIGHT('0000000' + CAST(NEXT VALUE FOR Produccion.SeqReempaqueFolio AS VARCHAR(7)), 7);

    INSERT INTO Produccion.Reempaque (Folio, FechaCreacion, HoraCreacion, Motivo, Estatus)
    VALUES (@Folio, CAST(SYSDATETIME() AS DATE), CAST(SYSDATETIME() AS TIME(0)), @Motivo, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- @Folio permite el hipervínculo Pallet.NoReempaque -> Reempaque, y el hipervínculo inverso desde
-- la columna "No. de Reempaque" del detalle de un Pallet (mismo criterio que
-- OrdenCorteService.ObtenerPorFolioAsync, ver GastoLoteForm.cs).
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_Obtener
    @Id INT = NULL,
    @Folio NVARCHAR(7) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.Id, r.Folio, r.FechaCreacion, r.HoraCreacion, r.Motivo, r.Estatus,
        r.KilosAProcesar, r.KilosProcesados, r.KilosAProcesar - r.KilosProcesados AS Diferencia,
        r.FechaCierre, r.FechaCreacionRegistro
    FROM Produccion.Reempaque r
    WHERE (@Id IS NULL OR r.Id = @Id)
      AND (@Folio IS NULL OR r.Folio = @Folio)
    ORDER BY r.FechaCreacionRegistro DESC, r.Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_ObtenerParaListado
AS
BEGIN
    SET NOCOUNT ON;
    EXEC Produccion.sp_Reempaque_Obtener @Id = NULL;
END
GO

-- ============================================================================
-- Entrada: pallets origen
-- ============================================================================

-- Candidatos a entrar: con identificador (no Neutro), no reempacados ya (Estatus <> 6), y
-- efectivamente armados (Bloqueado = 1 — un pallet Incompleto no tiene caso reempacarlo, todavía
-- se está construyendo por la vía normal). Excluye los que ya están reservados en OTRO reempaque
-- abierto (regla: un pallet entra a lo sumo a un folio a la vez).
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_ObtenerPalletsOrigenDisponibles
    @Folio NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        p.Id, p.Folio, p.FechaCreacion, p.Estatus,
        tot.TotalCajas, tot.TotalKilogramos,
        CAST(NULL AS INT) AS CajasObjetivo, -- no aplica en modo Origen (el pallet ya está Bloqueado)
        p.EsMixto,
        CASE WHEN tot.ProductosDistintos > 1 THEN 'Mixto' ELSE ISNULL(prim.DescripcionSap, '') END AS ProductoDescripcion
    FROM Produccion.Pallet p
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
      AND p.Estatus <> 6
      AND p.Bloqueado = 1
      AND NOT EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle rd WHERE rd.PalletOrigenId = p.Id)
      AND (@Folio IS NULL OR @Folio = '' OR p.Folio LIKE '%' + @Folio + '%')
    ORDER BY p.FechaCreacionRegistro DESC;
END
GO

-- Agrega el pallet completo (regla: nunca una parte). Genera una fila de saldo POR CADA línea de
-- su detalle, para no perder a qué lote pertenece cada kilo. No toca Produccion.Corrida en
-- absoluto — la corrida original nunca se reabre. El pallet origen pasa a Estatus = 6 Reempacado
-- de inmediato (no hasta cerrar el folio): desde que se toma para un reempaque queda reservado y
-- no debe verse disponible/Empacado en otras pantallas.
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_AgregarPalletOrigen
    @ReempaqueId INT,
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Reempaque WHERE Id = @ReempaqueId AND Estatus <> 1)
    BEGIN
        THROW 50000, 'Este reempaque ya está cerrado.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND EsNeutro = 1)
    BEGIN
        THROW 50000, 'Un Pallet Neutro (Merma/Diferencia a Favor) no puede entrar a un reempaque.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Estatus = 6)
    BEGIN
        THROW 50000, 'Este pallet ya fue reempacado antes: no puede volver a entrar.', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Solo se pueden reempacar pallets ya armados (bloqueados).', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle WHERE PalletOrigenId = @PalletId)
    BEGIN
        THROW 50000, 'Este pallet ya está reservado en otro reempaque.', 1;
    END

    BEGIN TRANSACTION;

    INSERT INTO Produccion.ReempaqueDetalle
        (ReempaqueId, PalletOrigenId, PalletDetalleOrigenId, LoteId, ProductoTerminadoOrigenId,
         CajasEntrada, KilosEntrada, KilosDisponibles)
    SELECT
        @ReempaqueId, @PalletId, d.Id, d.LoteId, d.ProductoTerminadoId,
        d.Cajas, d.Kilogramos, d.Kilogramos
    FROM Produccion.PalletDetalle d
    WHERE d.PalletId = @PalletId;

    UPDATE Produccion.Reempaque
    SET KilosAProcesar = KilosAProcesar + (SELECT SUM(Kilogramos) FROM Produccion.PalletDetalle WHERE PalletId = @PalletId)
    WHERE Id = @ReempaqueId;

    UPDATE Produccion.Pallet
    SET Estatus = 6, NoReempaque = (SELECT Folio FROM Produccion.Reempaque WHERE Id = @ReempaqueId)
    WHERE Id = @PalletId;

    COMMIT TRANSACTION;
END
GO

-- Solo se puede quitar si todavía no se consumió ningún kilo de sus líneas (KilosDisponibles =
-- KilosEntrada en todas). Regresa el pallet origen a Estatus = 5 Empacado y limpia NoReempaque —
-- vuelve a estar disponible para otro reempaque.
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_QuitarPalletOrigen
    @ReempaqueId INT,
    @PalletId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (
        SELECT 1 FROM Produccion.ReempaqueDetalle
        WHERE ReempaqueId = @ReempaqueId AND PalletOrigenId = @PalletId AND KilosDisponibles <> KilosEntrada)
    BEGIN
        THROW 50000, 'Ya se usaron kilos de este pallet en la salida: no se puede quitar.', 1;
    END

    BEGIN TRANSACTION;

    UPDATE Produccion.Reempaque
    SET KilosAProcesar = KilosAProcesar - ISNULL(
        (SELECT SUM(KilosEntrada) FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @ReempaqueId AND PalletOrigenId = @PalletId), 0)
    WHERE Id = @ReempaqueId;

    DELETE FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @ReempaqueId AND PalletOrigenId = @PalletId;

    UPDATE Produccion.Pallet
    SET Estatus = 5, NoReempaque = NULL
    WHERE Id = @PalletId;

    COMMIT TRANSACTION;
END
GO

-- Grid de Entrada: [No. Pallet, Lote, Producto, Cajas, Kilogramos] + KilosDisponibles para el
-- panel de control por lote.
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_ObtenerDetalleEntrada
    @ReempaqueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        rd.Id, rd.ReempaqueId, rd.PalletOrigenId, po.Folio AS PalletFolio,
        rd.LoteId, l.Folio AS LoteFolio, l.PorcentajeMateriaSeca,
        rd.ProductoTerminadoOrigenId, pt.DescripcionSap AS ProductoDescripcion,
        rd.CajasEntrada, rd.KilosEntrada, rd.KilosDisponibles
    FROM Produccion.ReempaqueDetalle rd
    INNER JOIN Produccion.Pallet po ON po.Id = rd.PalletOrigenId
    INNER JOIN Lotes.Lote l ON l.Id = rd.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = rd.ProductoTerminadoOrigenId
    WHERE rd.ReempaqueId = @ReempaqueId
    ORDER BY rd.Id;
END
GO

-- ============================================================================
-- Cierre
-- ============================================================================

-- Exige saldo 0 EN CADA LÍNEA de ReempaqueDetalle (nunca compensado entre lotes). Si pasa: marca
-- el Reempaque Cerrado y marca cada pallet ORIGEN distinto como Estatus 6 (Reempacado) con su
-- NoReempaque (redundante con sp_Reempaque_AgregarPalletOrigen, que ya lo hizo, pero se repite
-- aquí para dejarlo explícito). Los pallets DESTINO no se tocan: siguen su vida normal en el
-- módulo de Pallets y se pueden completar después con otra corrida o con otro reempaque.
CREATE OR ALTER PROCEDURE Produccion.sp_Reempaque_Cerrar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Reempaque WHERE Id = @Id AND Estatus <> 1)
    BEGIN
        THROW 50000, 'Este reempaque ya está cerrado.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @Id AND KilosDisponibles <> 0)
    BEGIN
        DECLARE @Lotes NVARCHAR(500) = (
            SELECT STRING_AGG(l.Folio, ', ')
            FROM Produccion.ReempaqueDetalle rd
            INNER JOIN Lotes.Lote l ON l.Id = rd.LoteId
            WHERE rd.ReempaqueId = @Id AND rd.KilosDisponibles <> 0);
        DECLARE @Msg NVARCHAR(600) = N'No se puede cerrar: los siguientes lotes no cuadran en 0 kg: ' + @Lotes + '.';
        THROW 50000, @Msg, 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @Id)
    BEGIN
        THROW 50000, 'Este reempaque no tiene pallets de entrada.', 1;
    END

    BEGIN TRANSACTION;

    DECLARE @Folio NVARCHAR(7) = (SELECT Folio FROM Produccion.Reempaque WHERE Id = @Id);

    UPDATE Produccion.Reempaque SET Estatus = 2, FechaCierre = SYSDATETIME() WHERE Id = @Id;

    UPDATE p
    SET p.Estatus = 6, p.NoReempaque = @Folio
    FROM Produccion.Pallet p
    WHERE p.Id IN (SELECT DISTINCT PalletOrigenId FROM Produccion.ReempaqueDetalle WHERE ReempaqueId = @Id);

    COMMIT TRANSACTION;
END
GO
