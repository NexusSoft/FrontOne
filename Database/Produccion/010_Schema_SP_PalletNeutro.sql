USE FrontOne;
GO

-- Pallet Neutro: un ajuste de Merma o Diferencia a Favor para cerrar una Corrida cuando Kilos
-- Procesados no cierra exacto contra Kilos a Procesar (Finalizar Corrida exige esa igualdad).
-- Reutiliza Produccion.Pallet/PalletDetalle — folio especial "0-{LoteFolio}" (no consecutivo, se
-- identifica con EsNeutro) en vez del folio normal por SEQUENCE. A diferencia de un Pallet real,
-- el Kilogramos de su única línea puede ser negativo (Diferencia a Favor resta de KilosProcesados).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.Pallet') AND name = 'EsNeutro')
BEGIN
    ALTER TABLE Produccion.Pallet ADD EsNeutro BIT NOT NULL CONSTRAINT DF_Produccion_Pallet_EsNeutro DEFAULT (0);
END
GO

-- Folio normal ocupa 7 dígitos (SEQUENCE); el Pallet Neutro usa "0-{LoteFolio}" (9+ caracteres) —
-- se amplía la columna, no rompe los folios ya guardados (siguen cabiendo igual).
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Produccion.Pallet') AND name = 'Folio' AND max_length < 40)
BEGIN
    ALTER TABLE Produccion.Pallet ALTER COLUMN Folio NVARCHAR(20) NOT NULL;
END
GO

-- Agrega EsNeutro al SELECT de encabezado (mismo cuerpo vigente de 005_Alter_Pallet_ProductoEncabezado.sql).
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
        p.ProductoTerminadoId,
        p.PorcentajeMateriaSeca,
        p.PesoReal,
        p.Bloqueado,
        p.FechaBloqueo,
        p.NoReempaque,
        p.PrimeraCorrida,
        p.EsNeutro,
        ISNULL(tot.TotalCajas, 0) AS TotalCajas,
        ISNULL(tot.TotalKilogramos, 0) AS TotalKilogramos,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) = 0 THEN ''
            WHEN tot.ProductosDistintos > 1 THEN 'Mixto'
            ELSE ISNULL(prim.DescripcionSap, '')
        END AS ProductoDescripcion,
        CASE
            WHEN ISNULL(tot.ProductosDistintos, 0) = 0 THEN ''
            WHEN tot.ProductosDistintos > 1 THEN ''
            ELSE ISNULL(prim.CodigoSap, '')
        END AS ProductoCodigoSap,
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
        SELECT TOP 1 pt.DescripcionSap, pt.CodigoSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = p.Id
        ORDER BY d.Id
    ) AS prim
    WHERE (@Id IS NULL OR p.Id = @Id)
    ORDER BY p.FechaCreacion DESC, p.Id DESC;
END
GO

CREATE OR ALTER PROCEDURE Produccion.sp_Pallet_CrearNeutro
    @CorridaId INT,
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

    DECLARE @LoteId INT, @LoteFolio NVARCHAR(20), @LineaProduccionId INT, @Estatus TINYINT;
    SELECT @LoteId = c.LoteId, @LoteFolio = l.Folio, @LineaProduccionId = l.LineaProduccionId, @Estatus = c.Estatus
    FROM Produccion.Corrida c
    INNER JOIN Lotes.Lote l ON l.Id = c.LoteId
    WHERE c.Id = @CorridaId;

    IF @LoteId IS NULL
    BEGIN
        THROW 50000, 'La corrida ya no existe.', 1;
    END

    IF @Estatus <> 1
    BEGIN
        THROW 50000, 'La corrida ya fue finalizada: no se le pueden capturar más diferencias.', 1;
    END

    IF EXISTS (
        SELECT 1 FROM Produccion.Pallet p
        INNER JOIN Produccion.PalletDetalle pd ON pd.PalletId = p.Id
        WHERE p.EsNeutro = 1 AND pd.CorridaId = @CorridaId
    )
    BEGIN
        THROW 50000, 'Este lote ya tiene un Pallet Neutro capturado: elimínalo antes de capturar uno nuevo.', 1;
    END

    DECLARE @Presentacion TINYINT;
    SELECT @Presentacion = Presentacion FROM Catalogos.ProductoTerminado WHERE Id = @ProductoTerminadoId;

    IF @Presentacion IS NULL OR @Presentacion <> 2
    BEGIN
        THROW 50000, 'El producto elegido debe ser Presentación Granel.', 1;
    END

    DECLARE @Folio NVARCHAR(50) = N'0-' + @LoteFolio;

    BEGIN TRANSACTION;

    INSERT INTO Produccion.Pallet
        (Folio, FechaCreacion, HoraCreacion, Estatus, LineaProduccionId, EsMixto, ProductoTerminadoId,
         PorcentajeMateriaSeca, Bloqueado, PrimeraCorrida, EsNeutro, FechaCreacionRegistro)
    VALUES
        (@Folio, CAST(SYSDATETIME() AS DATE), CAST(SYSDATETIME() AS TIME), 1, @LineaProduccionId, 0,
         @ProductoTerminadoId, 0, 0, 0, 1, SYSDATETIME());

    DECLARE @PalletId INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO Produccion.PalletDetalle
        (PalletId, CorridaId, LoteId, ProductoTerminadoId, Cajas, Kilogramos, PorcentajeMateriaSeca, CajasPorPallet)
    VALUES
        (@PalletId, @CorridaId, @LoteId, @ProductoTerminadoId, NULL, @Kilogramos, 0, NULL);

    UPDATE Produccion.Corrida
    SET KilosProcesados = KilosProcesados + @Kilogramos
    WHERE Id = @CorridaId;

    EXEC Produccion.sp_Pallet_Recalcular @PalletId;

    COMMIT TRANSACTION;

    SELECT @PalletId AS Id;
END
GO
