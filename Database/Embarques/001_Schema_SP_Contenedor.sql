USE FrontOne;
GO

-- Módulo Contenedor: surte un pedido de venta ya capturado y ABIERTO en SAP con pallets físicos
-- ya armados en Producción (Estatus 3 Completo, 4 Excedido, 5 Empacado o 7 En Proceso granel).
-- Cliente/pedido se guardan como SNAPSHOT del momento en que se elige el pedido (mismo criterio
-- que Produccion.PalletDetalle.CajasPorPallet) — si el pedido cambia después en SAP, el contenedor
-- ya guardado no se altera.
--
-- Al agregar un pallet al contenedor pasa a Estatus = 8 (Embarcado, ver 026_Alter_Pallet_Embarcado
-- en Database/Produccion) para sacarlo del circuito de Reempaques; al quitarlo regresa a 5 Empacado.
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Embarques')
BEGIN
    EXEC('CREATE SCHEMA Embarques');
END
GO

-- Folio consecutivo de 7 dígitos, mismo patrón que Produccion.SeqPalletFolio/SeqReempaqueFolio.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqContenedorFolio' AND schema_id = SCHEMA_ID('Embarques'))
BEGIN
    CREATE SEQUENCE Embarques.SeqContenedorFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Embarques.Contenedor'))
BEGIN
    CREATE TABLE Embarques.Contenedor
    (
        Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Embarques_Contenedor PRIMARY KEY,
        Folio                   NVARCHAR(7)     NOT NULL,
        Fecha                   DATE            NOT NULL,
        SapDocEntry             INT             NOT NULL,
        SapDocNum               INT             NOT NULL,
        FolioFronterra          NVARCHAR(50)    NULL,
        CardCode                NVARCHAR(50)    NOT NULL,
        CardName                NVARCHAR(200)   NOT NULL,
        Observaciones           NVARCHAR(500)   NULL,
        FechaCreacionRegistro   DATETIME2       NOT NULL CONSTRAINT DF_Embarques_Contenedor_FechaCreacionRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Embarques_Contenedor_Folio UNIQUE (Folio)
    );
END
GO

-- Un pallet solo puede estar en un contenedor a la vez (UNIQUE en PalletId) — mismo criterio que
-- "un pallet entra a lo sumo a un folio de reempaque a la vez".
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Embarques.ContenedorPallet'))
BEGIN
    CREATE TABLE Embarques.ContenedorPallet
    (
        Id                      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Embarques_ContenedorPallet PRIMARY KEY,
        ContenedorId            INT             NOT NULL,
        PalletId                INT             NOT NULL,
        Posicion                INT             NOT NULL,
        Temperatura             DECIMAL(5,2)    NULL,
        FechaCreacionRegistro   DATETIME2       NOT NULL CONSTRAINT DF_Embarques_ContenedorPallet_FechaCreacionRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Embarques_ContenedorPallet_PalletId UNIQUE (PalletId),
        CONSTRAINT FK_Embarques_ContenedorPallet_Contenedor FOREIGN KEY (ContenedorId) REFERENCES Embarques.Contenedor (Id),
        CONSTRAINT FK_Embarques_ContenedorPallet_Pallet FOREIGN KEY (PalletId) REFERENCES Produccion.Pallet (Id)
    );

    CREATE INDEX IX_Embarques_ContenedorPallet_ContenedorId ON Embarques.ContenedorPallet (ContenedorId);
END
GO

-- ============================================================================
-- Encabezado
-- ============================================================================

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id, c.Folio, c.Fecha, c.SapDocEntry, c.SapDocNum, c.FolioFronterra,
        c.CardCode, c.CardName, c.Observaciones, c.FechaCreacionRegistro,
        ISNULL(tot.TotalPallets, 0) AS TotalPallets
    FROM Embarques.Contenedor c
    OUTER APPLY (
        SELECT COUNT(*) AS TotalPallets FROM Embarques.ContenedorPallet cp WHERE cp.ContenedorId = c.Id
    ) AS tot
    WHERE (@Id IS NULL OR c.Id = @Id)
    ORDER BY c.FechaCreacionRegistro DESC;
END
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_Insertar
    @Fecha DATE,
    @SapDocEntry INT,
    @SapDocNum INT,
    @FolioFronterra NVARCHAR(50) = NULL,
    @CardCode NVARCHAR(50),
    @CardName NVARCHAR(200),
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Embarques.Contenedor WHERE SapDocEntry = @SapDocEntry)
    BEGIN
        THROW 50000, 'Ya existe un contenedor para este pedido de SAP.', 1;
    END

    DECLARE @Folio NVARCHAR(7) =
        RIGHT('0000000' + CAST(NEXT VALUE FOR Embarques.SeqContenedorFolio AS VARCHAR(7)), 7);

    INSERT INTO Embarques.Contenedor (Folio, Fecha, SapDocEntry, SapDocNum, FolioFronterra, CardCode, CardName, Observaciones)
    VALUES (@Folio, @Fecha, @SapDocEntry, @SapDocNum, @FolioFronterra, @CardCode, @CardName, @Observaciones);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_Actualizar
    @Id INT,
    @Fecha DATE,
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Embarques.Contenedor
    SET Fecha = @Fecha, Observaciones = @Observaciones
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Embarques.ContenedorPallet WHERE ContenedorId = @Id)
    BEGIN
        THROW 50000, 'No se puede eliminar el contenedor: primero quita todos sus pallets.', 1;
    END

    DELETE FROM Embarques.Contenedor WHERE Id = @Id;
END
GO

-- ============================================================================
-- Pallets del contenedor (Tab Embarque, sección izquierda)
-- ============================================================================

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_ObtenerPallets
    @ContenedorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cp.Id AS ContenedorPalletId,
        CAST(ROW_NUMBER() OVER (ORDER BY cp.Posicion, cp.Id) AS INT) AS NoRegistro,
        cp.PalletId,
        p.Folio AS PalletFolio,
        cp.Posicion,
        cp.Temperatura,
        ISNULL(tot.TotalCajas, 0) AS Cajas,
        ISNULL(tot.TotalKilogramos, 0) AS Kilogramos
    FROM Embarques.ContenedorPallet cp
    INNER JOIN Produccion.Pallet p ON p.Id = cp.PalletId
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas, SUM(d.Kilogramos) AS TotalKilogramos
        FROM Produccion.PalletDetalle d WHERE d.PalletId = cp.PalletId
    ) AS tot
    WHERE cp.ContenedorId = @ContenedorId
    ORDER BY cp.Posicion, cp.Id;
END
GO

-- Resumen agrupado por Calibre de Exportación (Tab Embarque, sección derecha-abajo).
CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_ObtenerResumen
    @ContenedorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ISNULL(pt.CalibreCodigoExterno, '(Sin calibre)') AS CalibreExportacion,
        COUNT(DISTINCT cp.PalletId) AS TotalPallets,
        SUM(d.Cajas) AS TotalCajas,
        SUM(d.Kilogramos) AS TotalKilogramos
    FROM Embarques.ContenedorPallet cp
    INNER JOIN Produccion.PalletDetalle d ON d.PalletId = cp.PalletId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE cp.ContenedorId = @ContenedorId
    GROUP BY pt.CalibreCodigoExterno
    ORDER BY CalibreExportacion;
END
GO

-- Cajas/Kilogramos ya surtidos por código SAP de producto (Tab Pedido) — alimenta el Status
-- Pendiente/Surtido comparando contra la Cantidad Cajas del pedido en SAP.
CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_ObtenerSurtido
    @ContenedorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pt.CodigoSap,
        SUM(d.Cajas) AS CajasSurtidas,
        SUM(d.Kilogramos) AS KilogramosSurtidos
    FROM Embarques.ContenedorPallet cp
    INNER JOIN Produccion.PalletDetalle d ON d.PalletId = cp.PalletId
    INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
    WHERE cp.ContenedorId = @ContenedorId
    GROUP BY pt.CodigoSap;
END
GO

-- Candidatos a agregar: armados (Bloqueado = 1), no Neutro, en Estatus 3/4/5/7, y que no estén
-- ya en ALGÚN contenedor (el UNIQUE de PalletId lo garantiza en BD, esto solo evita el intento).
-- @CodigosSap: CSV de Catalogos.ProductoTerminado.CodigoSap (típicamente los productos del pedido
-- que todavía no están 100% surtidos, ver ContenedorService.ObtenerLineasPedidoAsync) — NULL o ''
-- no filtra por producto. Solo aplica sobre pallets no mixtos: un pallet Mixto puede traer varios
-- productos y no hay una sola columna que comparar, así que siempre se incluye si pasa el resto de
-- filtros (el usuario revisa su detalle antes de agregarlo).
CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_ObtenerPalletsDisponibles
    @Folio NVARCHAR(50) = NULL,
    @CodigosSap NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100
        p.Id, p.Folio, p.FechaCreacion, p.Estatus,
        tot.TotalCajas, tot.TotalKilogramos,
        p.EsMixto,
        CASE WHEN tot.ProductosDistintos > 1 THEN 'Mixto' ELSE ISNULL(prim.DescripcionSap, '') END AS ProductoDescripcion
    FROM Produccion.Pallet p
    OUTER APPLY (
        SELECT SUM(d.Cajas) AS TotalCajas, SUM(d.Kilogramos) AS TotalKilogramos,
               COUNT(DISTINCT d.ProductoTerminadoId) AS ProductosDistintos
        FROM Produccion.PalletDetalle d WHERE d.PalletId = p.Id
    ) AS tot
    OUTER APPLY (
        SELECT TOP 1 pt.DescripcionSap, pt.CodigoSap
        FROM Produccion.PalletDetalle d
        INNER JOIN Catalogos.ProductoTerminado pt ON pt.Id = d.ProductoTerminadoId
        WHERE d.PalletId = p.Id ORDER BY d.Id
    ) AS prim
    WHERE p.EsNeutro = 0
      AND p.Estatus IN (3, 4, 5, 7)
      AND p.Bloqueado = 1
      AND NOT EXISTS (SELECT 1 FROM Embarques.ContenedorPallet cp WHERE cp.PalletId = p.Id)
      AND (@Folio IS NULL OR @Folio = '' OR p.Folio LIKE '%' + @Folio + '%')
      AND (
            @CodigosSap IS NULL OR @CodigosSap = ''
            OR tot.ProductosDistintos > 1
            OR prim.CodigoSap IN (SELECT value FROM STRING_SPLIT(@CodigosSap, ','))
          )
    ORDER BY p.FechaCreacionRegistro DESC;
END
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_AgregarPallet
    @ContenedorId INT,
    @PalletId INT,
    @Posicion INT,
    @Temperatura DECIMAL(5,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM Produccion.Pallet WHERE Id = @PalletId AND EsNeutro = 0 AND Estatus IN (3, 4, 5, 7) AND Bloqueado = 1)
    BEGIN
        THROW 50000, 'El pallet no está disponible para embarcarse (debe estar Completo, Excedido, Empacado o En Proceso, y no ser Neutro).', 1;
    END

    IF EXISTS (SELECT 1 FROM Embarques.ContenedorPallet WHERE PalletId = @PalletId)
    BEGIN
        THROW 50000, 'Este pallet ya está asignado a un contenedor.', 1;
    END

    IF EXISTS (SELECT 1 FROM Embarques.ContenedorPallet WHERE ContenedorId = @ContenedorId AND Posicion = @Posicion)
    BEGIN
        THROW 50000, 'Ya existe un pallet en esa posición dentro del contenedor.', 1;
    END

    BEGIN TRANSACTION;

    INSERT INTO Embarques.ContenedorPallet (ContenedorId, PalletId, Posicion, Temperatura)
    VALUES (@ContenedorId, @PalletId, @Posicion, @Temperatura);

    UPDATE Produccion.Pallet
    SET Estatus = 8, Bloqueado = 1, FechaBloqueo = ISNULL(FechaBloqueo, SYSUTCDATETIME())
    WHERE Id = @PalletId;

    COMMIT TRANSACTION;
END
GO

CREATE OR ALTER PROCEDURE Embarques.sp_Contenedor_QuitarPallet
    @ContenedorPalletId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @PalletId INT = (SELECT PalletId FROM Embarques.ContenedorPallet WHERE Id = @ContenedorPalletId);

    IF @PalletId IS NULL
    BEGIN
        THROW 50000, 'El renglón ya no existe.', 1;
    END

    BEGIN TRANSACTION;

    DELETE FROM Embarques.ContenedorPallet WHERE Id = @ContenedorPalletId;

    UPDATE Produccion.Pallet
    SET Estatus = 5, Bloqueado = 1
    WHERE Id = @PalletId;

    COMMIT TRANSACTION;
END
GO
