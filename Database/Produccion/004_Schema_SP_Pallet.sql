USE FrontOne;
GO

-- Módulo Pallets: con un Lote ya "En Proceso" (Corrida con Estatus = 1), se arman los Pallets
-- con las cajas de Producto Terminado que salen de ese Lote. Cada línea de detalle consume
-- Kilogramos del saldo de la Corrida — es el mecanismo que finalmente llena
-- Produccion.Corrida.KilosProcesados (columna que quedó lista desde el módulo Corridas pero que
-- hasta ahora nadie actualizaba).
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Produccion')
BEGIN
    EXEC('CREATE SCHEMA Produccion');
END
GO

-- Folio consecutivo de 7 dígitos (0000001, 0000002...), mismo patrón que Acopio.SeqOrdenCorteFolio.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqPalletFolio' AND schema_id = SCHEMA_ID('Produccion'))
BEGIN
    CREATE SEQUENCE Produccion.SeqPalletFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

-- Estatus del Pallet: 1 Vacío, 2 Incompleto, 3 Completo, 4 Excedido, 5 Empacado, 6 Reempacado.
-- Nunca se captura a mano: 1-4 los recalcula sp_Pallet_Recalcular con cada movimiento del
-- detalle; 5 lo fija sp_Pallet_Bloquear y a partir de ahí ya no se recalcula (el pallet está
-- físicamente armado); 6 queda reservado para el futuro módulo de Reempaques, que además
-- llenará NoReempaque — este módulo solo expone la columna, no la toca.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Produccion.Pallet'))
BEGIN
    CREATE TABLE Produccion.Pallet
    (
        Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Produccion_Pallet PRIMARY KEY,
        Folio                   NVARCHAR(7)     NOT NULL,
        FechaCreacion           DATE            NOT NULL,
        HoraCreacion            TIME(0)         NOT NULL,
        Estatus                 TINYINT         NOT NULL CONSTRAINT DF_Produccion_Pallet_Estatus DEFAULT (1),
        LineaProduccionId       INT             NOT NULL,
        EsMixto                 BIT             NOT NULL CONSTRAINT DF_Produccion_Pallet_EsMixto DEFAULT (0),
        PorcentajeMateriaSeca   DECIMAL(5,2)    NOT NULL CONSTRAINT DF_Produccion_Pallet_PctMateriaSeca DEFAULT (0),
        PesoReal                DECIMAL(10,2)   NULL,
        Bloqueado               BIT             NOT NULL CONSTRAINT DF_Produccion_Pallet_Bloqueado DEFAULT (0),
        FechaBloqueo            DATETIME2       NULL,
        NoReempaque             NVARCHAR(20)    NULL,
        PrimeraCorrida          BIT             NOT NULL CONSTRAINT DF_Produccion_Pallet_PrimeraCorrida DEFAULT (1),
        FechaCreacionRegistro   DATETIME2       NOT NULL CONSTRAINT DF_Produccion_Pallet_FechaCreacionRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Produccion_Pallet_Folio UNIQUE (Folio),
        CONSTRAINT FK_Produccion_Pallet_LineaProduccion FOREIGN KEY (LineaProduccionId) REFERENCES Catalogos.LineaProduccion (Id)
    );
END
GO

-- Detalle del Pallet. Kilogramos y CajasPorPallet son SNAPSHOT tomados del Producto Terminado
-- al momento de agregar la línea (PesoNeto × Cajas y CajasPorPallet respectivamente): si el
-- catálogo cambia después, lo ya armado no se altera. LoteId se guarda directo además de vía
-- CorridaId para que el reporte/consultas no dependan siempre del join a Corrida.
-- Ninguna FK lleva ON DELETE CASCADE (regla dura del proyecto).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Produccion.PalletDetalle'))
BEGIN
    CREATE TABLE Produccion.PalletDetalle
    (
        Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Produccion_PalletDetalle PRIMARY KEY,
        PalletId                INT             NOT NULL,
        CorridaId               INT             NOT NULL,
        LoteId                  INT             NOT NULL,
        ProductoTerminadoId     INT             NOT NULL,
        Cajas                   INT             NOT NULL,
        Kilogramos              DECIMAL(10,2)   NOT NULL,
        PorcentajeMateriaSeca   DECIMAL(5,2)    NOT NULL CONSTRAINT DF_Produccion_PalletDetalle_PctMateriaSeca DEFAULT (0),
        CajasPorPallet          INT             NOT NULL,
        CONSTRAINT FK_Produccion_PalletDetalle_Pallet FOREIGN KEY (PalletId) REFERENCES Produccion.Pallet (Id),
        CONSTRAINT FK_Produccion_PalletDetalle_Corrida FOREIGN KEY (CorridaId) REFERENCES Produccion.Corrida (Id),
        CONSTRAINT FK_Produccion_PalletDetalle_Lote FOREIGN KEY (LoteId) REFERENCES Lotes.Lote (Id),
        CONSTRAINT FK_Produccion_PalletDetalle_ProductoTerminado FOREIGN KEY (ProductoTerminadoId) REFERENCES Catalogos.ProductoTerminado (Id)
    );

    CREATE INDEX IX_Produccion_PalletDetalle_PalletId ON Produccion.PalletDetalle (PalletId);
    CREATE INDEX IX_Produccion_PalletDetalle_CorridaId ON Produccion.PalletDetalle (CorridaId);
END
GO

-- ============================================================================
-- Recálculo de Estatus y % Materia Seca del encabezado.
--
-- Objetivo de cajas: se toma el snapshot CajasPorPallet de la PRIMERA línea capturada (la de
-- menor Id) — la que define el formato de caja del pallet. En un pallet de un solo producto es
-- exactamente el CajasPorPallet de ese producto; en uno mixto es el del producto que lo abrió,
-- que en la práctica es el que fija cuántas cajas caben físicamente en la tarima. Así el
-- criterio Completo/Incompleto/Excedido es UNA sola regla, sin caso especial por EsMixto.
--
-- % Materia Seca = Σ(Cajas × %MS) / Σ(Cajas) — promedio ponderado por cajas (un promedio simple
-- daría un número que no representa el pallet cuando las líneas tienen conteos muy distintos).
--
-- Un pallet ya Bloqueado (Estatus 5 Empacado o 6 Reempacado) NO se recalcula nunca: está
-- físicamente armado y su estatus es definitivo.
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
    DECLARE @SumaPonderada DECIMAL(18,4) = 0;
    DECLARE @Objetivo INT = 0;

    SELECT @TotalCajas = ISNULL(SUM(Cajas), 0),
           @SumaPonderada = ISNULL(SUM(CAST(Cajas AS DECIMAL(18,4)) * PorcentajeMateriaSeca), 0)
    FROM Produccion.PalletDetalle
    WHERE PalletId = @PalletId;

    SELECT TOP 1 @Objetivo = CajasPorPallet
    FROM Produccion.PalletDetalle
    WHERE PalletId = @PalletId
    ORDER BY Id;

    UPDATE Produccion.Pallet
    SET Estatus = CASE
                      WHEN @TotalCajas = 0 THEN 1                        -- Vacío
                      WHEN @Objetivo <= 0 THEN 2                         -- sin objetivo válido: se queda Incompleto
                      WHEN @TotalCajas < @Objetivo THEN 2                -- Incompleto
                      WHEN @TotalCajas = @Objetivo THEN 3                -- Completo
                      ELSE 4                                             -- Excedido
                  END,
        PorcentajeMateriaSeca = CASE WHEN @TotalCajas = 0 THEN 0
                                     ELSE CAST(@SumaPonderada / @TotalCajas AS DECIMAL(5,2))
                                END
    WHERE Id = @PalletId;
END
GO

-- ============================================================================
-- Encabezado
-- ============================================================================

-- Trae el encabezado con los agregados que necesita el listado (Cajas y Kilogramos sumados del
-- detalle, y la descripción del Producto: la del único producto capturado, o 'Mixto' si hay más
-- de uno). Sin @Id devuelve todos, ordenados por Fecha descendente.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.Folio,
        p.FechaCreacion,
        p.HoraCreacion,
        p.Estatus,
        p.LineaProduccionId,
        lp.Nombre AS LineaProduccionNombre,
        p.EsMixto,
        p.PorcentajeMateriaSeca,
        p.PesoReal,
        p.Bloqueado,
        p.FechaBloqueo,
        p.NoReempaque,
        p.PrimeraCorrida,
        ISNULL(tot.TotalCajas, 0) AS TotalCajas,
        ISNULL(tot.TotalKilogramos, 0) AS TotalKilogramos,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) = 0 THEN ''
            WHEN tot.ProductosDistintos > 1 THEN 'Mixto'
            ELSE ISNULL(prim.DescripcionSap, '')
        END AS ProductoDescripcion,
        p.FechaCreacionRegistro
    FROM Produccion.Pallet p
    INNER JOIN Catalogos.LineaProduccion lp ON lp.Id = p.LineaProduccionId
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas,
               SUM(d.Kilogramos) AS TotalKilogramos,
               COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d
        WHERE d.PalletId = p.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = p.Id
        ORDER BY d.Id
    ) AS prim
    WHERE (@Id IS NULL OR p.Id = @Id)
    ORDER BY p.FechaCreacion DESC, p.Id DESC;
END
GO

-- Detalle del Pallet. LoteEnProceso = 1 mientras la Corrida de esa línea siga En Proceso
-- (Estatus = 1); si la Corrida ya se finalizó, la UI deja la línea de solo lectura aunque el
-- Pallet en sí siga sin bloquear (los kilos ya se cerraron del lado de la Corrida).
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
        CAST(CASE WHEN c.Estatus = 1 THEN 1 ELSE 0 END AS BIT) AS LoteEnProceso
    FROM Produccion.PalletDetalle d
    INNER JOIN Produccion.Corrida c ON c.Id = d.CorridaId
    INNER JOIN Lotes.Lote l ON l.Id = d.LoteId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE d.PalletId = @PalletId
    ORDER BY d.Id;
END
GO

-- Lotes disponibles para capturar una línea de Pallet: los que están En Proceso (Corrida con
-- Estatus = 1) y todavía tienen saldo. Mismo OUTER APPLY de Huerta/Sagarpa/Productor que ya usa
-- sp_Corrida_Obtener. Es un SP distinto de sp_Corrida_ObtenerLotesDisponibles a propósito: aquel
-- lista Lotes SIN corrida (para abrirla), este lista Lotes CON corrida abierta (para consumirla).
-- @LineaProduccionId filtra a los Lotes de la misma línea de producción del encabezado del Pallet.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerLotesEnProceso
    @LineaProduccionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id AS CorridaId,
        l.Id AS LoteId,
        l.Folio AS LoteFolio,
        ISNULL(l.CodigoTrazabilidad, '') AS CodigoTrazabilidad,
        l.LineaProduccionId,
        l.PorcentajeMateriaSeca,
        c.KilosAProcesar,
        c.KilosProcesados,
        c.KilosAProcesar - c.KilosProcesados AS KilosDisponibles,
        primera.HuertaNombre,
        primera.RegistroSagarpa,
        primera.ProductorNombre
    FROM Produccion.Corrida c
    INNER JOIN Lotes.Lote l ON l.Id = c.LoteId
    OUTER APPLY (
        SELECT TOP 1
            h.Nombre AS HuertaNombre, h.RegistroSagarpa, pr.NombreProductor AS ProductorNombre
        FROM Lotes.LoteRecepcion det
        INNER JOIN Recepcion.RecepcionFrutaOrdenCorte roc ON roc.RecepcionFrutaId = det.RecepcionFrutaId
        INNER JOIN Acopio.OrdenCorte oc ON oc.Id = roc.OrdenCorteId
        INNER JOIN Catalogos.Huerta h ON h.Id = oc.HuertaId
        INNER JOIN Catalogos.Productor pr ON pr.Id = h.ProductorId
        WHERE det.LoteId = l.Id
        ORDER BY det.Id
    ) AS primera
    WHERE c.Estatus = 1
      AND (@LineaProduccionId IS NULL OR l.LineaProduccionId = @LineaProduccionId)
    ORDER BY l.Folio;
END
GO

-- Alta del encabezado: folio server-side desde la SEQUENCE, Fecha/Hora del servidor, Estatus
-- arranca en 1 (Vacío) porque todavía no tiene detalle. PrimeraCorrida = 1 siempre en los
-- Pallets nacidos de este módulo (a diferencia de los que en el futuro genere Reempaques) —
-- es la marca para el reporte de pago a productor.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Insertar
    @LineaProduccionId INT,
    @EsMixto BIT = 0,
    @PesoReal DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Folio NVARCHAR(7) =
        RIGHT('0000000' + CAST(NEXT VALUE FOR Produccion.SeqPalletFolio AS VARCHAR(7)), 7);

    INSERT INTO Produccion.Pallet
        (Folio, FechaCreacion, HoraCreacion, Estatus, LineaProduccionId, EsMixto, PesoReal, PrimeraCorrida)
    VALUES
        (@Folio, CAST(SYSDATETIME() AS DATE), CAST(SYSDATETIME() AS TIME(0)), 1, @LineaProduccionId, @EsMixto, @PesoReal, 1);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Solo se pueden tocar Línea de Producción, Es Mixto y Peso Real. Folio/Fecha/Hora/Estatus/
-- % Materia Seca nunca se capturan a mano. Un Pallet bloqueado no admite ningún cambio.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ActualizarEncabezado
    @Id INT,
    @LineaProduccionId INT,
    @EsMixto BIT,
    @PesoReal DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @Id AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se puede modificar.', 1;
    END

    UPDATE Produccion.Pallet
    SET LineaProduccionId = @LineaProduccionId,
        EsMixto = @EsMixto,
        PesoReal = @PesoReal
    WHERE Id = @Id;
END
GO

-- Bloquear es definitivo: congela encabezado y detalle, y deja el Pallet en Estatus 5 (Empacado).
-- No hay SP de desbloqueo a propósito.
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_Bloquear
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @Id AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado.', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Produccion.PalletDetalle WHERE PalletId = @Id)
    BEGIN
        THROW 50000, 'No se puede bloquear un pallet vacío: agrega al menos una línea de detalle.', 1;
    END

    UPDATE Produccion.Pallet
    SET Estatus = 5, Bloqueado = 1, FechaBloqueo = SYSDATETIME()
    WHERE Id = @Id;
END
GO

-- Eliminar un Pallet completo revierte los Kilogramos de TODAS sus líneas contra la Corrida
-- correspondiente antes de borrarlas — si no, los kilos consumidos quedarían fantasma en
-- Corrida.KilosProcesados. Un Pallet bloqueado no se puede eliminar.
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

    BEGIN TRANSACTION;

    UPDATE c
    SET c.KilosProcesados = c.KilosProcesados - agg.Kilos
    FROM Produccion.Corrida c
    INNER JOIN (
        SELECT CorridaId, SUM(Kilogramos) AS Kilos
        FROM Produccion.PalletDetalle
        WHERE PalletId = @Id
        GROUP BY CorridaId
    ) AS agg ON agg.CorridaId = c.Id;

    DELETE FROM Produccion.PalletDetalle WHERE PalletId = @Id;
    DELETE FROM Produccion.Pallet WHERE Id = @Id;

    COMMIT TRANSACTION;
END
GO

-- ============================================================================
-- Detalle
-- ============================================================================

-- Agrega una línea: calcula Kilogramos = PesoNeto del Producto Terminado × Cajas (snapshot,
-- igual que los Kilos de Recepción se calculan del lado del servidor), valida que la Corrida
-- siga En Proceso y que el saldo del Lote alcance, y acumula los kilos en Corrida.KilosProcesados.
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

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se le pueden agregar líneas.', 1;
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

    BEGIN TRANSACTION;

    DECLARE @LoteId INT, @KilosAProcesar DECIMAL(10,2), @KilosProcesados DECIMAL(10,2), @Estatus TINYINT;
    SELECT @LoteId = LoteId, @KilosAProcesar = KilosAProcesar, @KilosProcesados = KilosProcesados, @Estatus = Estatus
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

    IF @KilosAProcesar - @KilosProcesados < @Kilogramos
    BEGIN
        THROW 50000, 'Saldo insuficiente en el lote: las cajas capturadas superan los kilogramos disponibles de la corrida.', 1;
    END

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

-- Editar una línea aplica el DELTA de kilogramos contra la Corrida (no vuelve a sumar el total).
-- Rechaza si el Pallet está bloqueado o si la Corrida de la línea ya se finalizó.
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

    DECLARE @PalletId INT, @CorridaId INT, @KilosAnteriores DECIMAL(10,2);
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @KilosAnteriores = Kilogramos
    FROM Produccion.PalletDetalle
    WHERE Id = @Id;

    IF @PalletId IS NULL
    BEGIN
        THROW 50000, 'La línea de detalle ya no existe.', 1;
    END

    IF EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'Este pallet ya está bloqueado: no se pueden modificar sus líneas.', 1;
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

    BEGIN TRANSACTION;

    DECLARE @KilosAProcesar DECIMAL(10,2), @KilosProcesados DECIMAL(10,2), @Estatus TINYINT;
    SELECT @KilosAProcesar = KilosAProcesar, @KilosProcesados = KilosProcesados, @Estatus = Estatus
    FROM Produccion.Corrida WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @CorridaId;

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida de este lote ya fue finalizada: esta línea ya no se puede modificar.', 1;
    END

    DECLARE @Kilogramos DECIMAL(10,2) = CAST(@PesoNeto * @Cajas AS DECIMAL(10,2));
    DECLARE @Delta DECIMAL(10,2) = @Kilogramos - @KilosAnteriores;

    IF @KilosAProcesar - @KilosProcesados < @Delta
    BEGIN
        THROW 50000, 'Saldo insuficiente en el lote: las cajas capturadas superan los kilogramos disponibles de la corrida.', 1;
    END

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

-- Eliminar una línea devuelve sus kilogramos al saldo de la Corrida y recalcula el encabezado.
CREATE OR ALTER PROCEDURE Produccion.sp_PalletDetalle_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @PalletId INT, @CorridaId INT, @Kilogramos DECIMAL(10,2);
    SELECT @PalletId = PalletId, @CorridaId = CorridaId, @Kilogramos = Kilogramos
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

    BEGIN TRANSACTION;

    DELETE FROM Produccion.PalletDetalle WHERE Id = @Id;

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados - @Kilogramos
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;
END
GO

-- Origen de datos del reporte "Papeleta de Pallet": encabezado + membrete de la empresa en una
-- sola fila (mismo criterio que Recepcion.sp_RecepcionFruta_ObtenerParaReporte).
CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_ObtenerParaReporte
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.Folio,
        p.FechaCreacion,
        p.HoraCreacion,
        p.Estatus,
        lp.Nombre AS LineaProduccionNombre,
        p.EsMixto,
        p.PorcentajeMateriaSeca,
        p.PesoReal,
        p.Bloqueado,
        p.NoReempaque,
        p.PrimeraCorrida,
        ISNULL(tot.TotalCajas, 0) AS TotalCajas,
        ISNULL(tot.TotalKilogramos, 0) AS TotalKilogramos
    FROM Produccion.Pallet p
    INNER JOIN Catalogos.LineaProduccion lp ON lp.Id = p.LineaProduccionId
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas, SUM(d.Kilogramos) AS TotalKilogramos
        FROM Produccion.PalletDetalle d
        WHERE d.PalletId = p.Id
    ) AS tot
    WHERE p.Id = @Id;
END
GO
