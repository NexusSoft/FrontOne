USE FrontOne;
GO

-- Estimación de precio sugerido por Huerta/Categoría/Calibre, cruzada contra una vigencia de
-- Acopio.ListaPrecioFruta. Folio consecutivo de 7 dígitos por MAX(Folio)+1 con (UPDLOCK, HOLDLOCK)
-- dentro de la transacción del insert — mismo criterio ya migrado para AcuerdoCorte/OrdenCorte en
-- 031_SP_Folio_Reutilizable.sql, no SEQUENCE. Cerrada se pone en 1 cuando una Orden de Corte se
-- guarda referenciándola (Acopio.OrdenCorte.EstimacionId) — a partir de ahí ya no se puede editar.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.Estimacion'))
BEGIN
    CREATE TABLE Acopio.Estimacion
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acopio_Estimacion PRIMARY KEY,
        Folio                   NVARCHAR(7)         NOT NULL,
        Fecha                   DATE                NOT NULL,
        HuertaId                INT                 NOT NULL,
        RegistroSagarpa         NVARCHAR(50)        NULL,
        Kilos                   DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Acopio_Estimacion_Kilos DEFAULT (0),
        PorcentajeCat1          DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCat1 DEFAULT (0),
        PorcentajeCat2          DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCat2 DEFAULT (0),
        PorcentajeNal           DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeNal DEFAULT (0),
        PorcentajeCalibre32     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre32 DEFAULT (0),
        PorcentajeCalibre36     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre36 DEFAULT (0),
        PorcentajeCalibre40     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre40 DEFAULT (0),
        PorcentajeCalibre48     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre48 DEFAULT (0),
        PorcentajeCalibre60     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre60 DEFAULT (0),
        PorcentajeCalibre70     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre70 DEFAULT (0),
        PorcentajeCalibre84     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre84 DEFAULT (0),
        PorcentajeCalibre90     DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCalibre90 DEFAULT (0),
        PorcentajeBorona        DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeBorona DEFAULT (0),
        PorcentajeCanica        DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCanica DEFAULT (0),
        PorcentajeCuarta        DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeCuarta DEFAULT (0),
        PorcentajeDesecho       DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeDesecho DEFAULT (0),
        PorcentajeProceso       DECIMAL(9,4)        NOT NULL CONSTRAINT DF_Acopio_Estimacion_PorcentajeProceso DEFAULT (0),
        ListaPrecioFecha        DATE                NOT NULL,
        ListaPrecioProductorId  INT                 NULL,
        TipoLista               TINYINT             NOT NULL,
        PrecioSugerido          DECIMAL(18,4)       NOT NULL,
        Cerrada                 BIT                 NOT NULL CONSTRAINT DF_Acopio_Estimacion_Cerrada DEFAULT (0),
        FechaCreacion           DATETIME2           NOT NULL CONSTRAINT DF_Acopio_Estimacion_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Acopio_Estimacion_Folio UNIQUE (Folio),
        CONSTRAINT CK_Acopio_Estimacion_TipoLista CHECK (TipoLista IN (0, 1, 2)),
        CONSTRAINT FK_Acopio_Estimacion_Huerta FOREIGN KEY (HuertaId) REFERENCES Catalogos.Huerta (Id)
    );
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.Id, e.Folio, e.Fecha, e.HuertaId, h.Nombre AS HuertaNombre, e.RegistroSagarpa, e.Kilos,
        e.PorcentajeCat1, e.PorcentajeCat2, e.PorcentajeNal,
        e.PorcentajeCalibre32, e.PorcentajeCalibre36, e.PorcentajeCalibre40, e.PorcentajeCalibre48,
        e.PorcentajeCalibre60, e.PorcentajeCalibre70, e.PorcentajeCalibre84, e.PorcentajeCalibre90,
        e.PorcentajeBorona, e.PorcentajeCanica, e.PorcentajeCuarta, e.PorcentajeDesecho, e.PorcentajeProceso,
        e.ListaPrecioFecha, e.ListaPrecioProductorId, e.TipoLista, e.PrecioSugerido, e.Cerrada, e.FechaCreacion
    FROM Acopio.Estimacion e
    INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
    WHERE (@Id IS NULL OR e.Id = @Id)
    ORDER BY e.FechaCreacion DESC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_ObtenerPorFolio
    @Folio NVARCHAR(7)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id INT = (SELECT Id FROM Acopio.Estimacion WHERE Folio = @Folio);
    EXEC Acopio.sp_Estimacion_Obtener @Id = @Id;
END
GO

-- Carga inicial del picker (sin filtro), TOP 100 más recientes — mismo criterio que Huerta/
-- Productor (buscador embebido de catálogo con volumen alto).
CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_ObtenerTop100
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (100)
        e.Id, e.Folio, e.Fecha, h.Nombre AS HuertaNombre, e.PrecioSugerido, e.Cerrada
    FROM Acopio.Estimacion e
    INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
    ORDER BY e.FechaCreacion DESC;
END
GO

-- Búsqueda por texto (Folio o nombre de Huerta), TOP 500.
CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Buscar
    @Filtro NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (500)
        e.Id, e.Folio, e.Fecha, h.Nombre AS HuertaNombre, e.PrecioSugerido, e.Cerrada
    FROM Acopio.Estimacion e
    INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
    WHERE e.Folio LIKE '%' + @Filtro + '%'
       OR h.Nombre LIKE '%' + @Filtro + '%'
    ORDER BY e.FechaCreacion DESC;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Insertar
    @Fecha                  DATE,
    @HuertaId               INT,
    @RegistroSagarpa        NVARCHAR(50) = NULL,
    @Kilos                  DECIMAL(18,2),
    @PorcentajeCat1         DECIMAL(9,4),
    @PorcentajeCat2         DECIMAL(9,4),
    @PorcentajeNal          DECIMAL(9,4),
    @PorcentajeCalibre32    DECIMAL(9,4),
    @PorcentajeCalibre36    DECIMAL(9,4),
    @PorcentajeCalibre40    DECIMAL(9,4),
    @PorcentajeCalibre48    DECIMAL(9,4),
    @PorcentajeCalibre60    DECIMAL(9,4),
    @PorcentajeCalibre70    DECIMAL(9,4),
    @PorcentajeCalibre84    DECIMAL(9,4),
    @PorcentajeCalibre90    DECIMAL(9,4),
    @PorcentajeBorona       DECIMAL(9,4),
    @PorcentajeCanica       DECIMAL(9,4),
    @PorcentajeCuarta       DECIMAL(9,4),
    @PorcentajeDesecho      DECIMAL(9,4),
    @PorcentajeProceso      DECIMAL(9,4),
    @ListaPrecioFecha       DATE,
    @ListaPrecioProductorId INT = NULL,
    @TipoLista              TINYINT,
    @PrecioSugerido         DECIMAL(18,4)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Folio NVARCHAR(7);

    BEGIN TRANSACTION;

    SELECT @Folio = RIGHT('0000000' + CAST(ISNULL(MAX(TRY_CAST(Folio AS INT)), 0) + 1 AS VARCHAR(7)), 7)
    FROM Acopio.Estimacion WITH (UPDLOCK, HOLDLOCK);

    INSERT INTO Acopio.Estimacion
        (Folio, Fecha, HuertaId, RegistroSagarpa, Kilos,
         PorcentajeCat1, PorcentajeCat2, PorcentajeNal,
         PorcentajeCalibre32, PorcentajeCalibre36, PorcentajeCalibre40, PorcentajeCalibre48,
         PorcentajeCalibre60, PorcentajeCalibre70, PorcentajeCalibre84, PorcentajeCalibre90,
         PorcentajeBorona, PorcentajeCanica, PorcentajeCuarta, PorcentajeDesecho, PorcentajeProceso,
         ListaPrecioFecha, ListaPrecioProductorId, TipoLista, PrecioSugerido)
    VALUES
        (@Folio, @Fecha, @HuertaId, @RegistroSagarpa, @Kilos,
         @PorcentajeCat1, @PorcentajeCat2, @PorcentajeNal,
         @PorcentajeCalibre32, @PorcentajeCalibre36, @PorcentajeCalibre40, @PorcentajeCalibre48,
         @PorcentajeCalibre60, @PorcentajeCalibre70, @PorcentajeCalibre84, @PorcentajeCalibre90,
         @PorcentajeBorona, @PorcentajeCanica, @PorcentajeCuarta, @PorcentajeDesecho, @PorcentajeProceso,
         @ListaPrecioFecha, @ListaPrecioProductorId, @TipoLista, @PrecioSugerido);

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

-- No toca Folio/Cerrada — el folio no cambia al editar (igual criterio que AcuerdoCorte/
-- ListaPrecioFruta) y Cerrada solo la cambia sp_Estimacion_MarcarCerrada.
CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Actualizar
    @Id                     INT,
    @Fecha                  DATE,
    @HuertaId               INT,
    @RegistroSagarpa        NVARCHAR(50) = NULL,
    @Kilos                  DECIMAL(18,2),
    @PorcentajeCat1         DECIMAL(9,4),
    @PorcentajeCat2         DECIMAL(9,4),
    @PorcentajeNal          DECIMAL(9,4),
    @PorcentajeCalibre32    DECIMAL(9,4),
    @PorcentajeCalibre36    DECIMAL(9,4),
    @PorcentajeCalibre40    DECIMAL(9,4),
    @PorcentajeCalibre48    DECIMAL(9,4),
    @PorcentajeCalibre60    DECIMAL(9,4),
    @PorcentajeCalibre70    DECIMAL(9,4),
    @PorcentajeCalibre84    DECIMAL(9,4),
    @PorcentajeCalibre90    DECIMAL(9,4),
    @PorcentajeBorona       DECIMAL(9,4),
    @PorcentajeCanica       DECIMAL(9,4),
    @PorcentajeCuarta       DECIMAL(9,4),
    @PorcentajeDesecho      DECIMAL(9,4),
    @PorcentajeProceso      DECIMAL(9,4),
    @ListaPrecioFecha       DATE,
    @ListaPrecioProductorId INT = NULL,
    @TipoLista              TINYINT,
    @PrecioSugerido         DECIMAL(18,4)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.Estimacion
    SET Fecha = @Fecha,
        HuertaId = @HuertaId,
        RegistroSagarpa = @RegistroSagarpa,
        Kilos = @Kilos,
        PorcentajeCat1 = @PorcentajeCat1,
        PorcentajeCat2 = @PorcentajeCat2,
        PorcentajeNal = @PorcentajeNal,
        PorcentajeCalibre32 = @PorcentajeCalibre32,
        PorcentajeCalibre36 = @PorcentajeCalibre36,
        PorcentajeCalibre40 = @PorcentajeCalibre40,
        PorcentajeCalibre48 = @PorcentajeCalibre48,
        PorcentajeCalibre60 = @PorcentajeCalibre60,
        PorcentajeCalibre70 = @PorcentajeCalibre70,
        PorcentajeCalibre84 = @PorcentajeCalibre84,
        PorcentajeCalibre90 = @PorcentajeCalibre90,
        PorcentajeBorona = @PorcentajeBorona,
        PorcentajeCanica = @PorcentajeCanica,
        PorcentajeCuarta = @PorcentajeCuarta,
        PorcentajeDesecho = @PorcentajeDesecho,
        PorcentajeProceso = @PorcentajeProceso,
        ListaPrecioFecha = @ListaPrecioFecha,
        ListaPrecioProductorId = @ListaPrecioProductorId,
        TipoLista = @TipoLista,
        PrecioSugerido = @PrecioSugerido
    WHERE Id = @Id AND Cerrada = 0;
END
GO

-- Se llama desde Acopio.sp_OrdenCorte_Insertar/_Actualizar cuando la orden referencia una
-- Estimación (idempotente — no valida el estado previo, un WHERE Cerrada = 0 evita rehacer el
-- update de más si ya estaba cerrada).
CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_MarcarCerrada
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.Estimacion
    SET Cerrada = 1
    WHERE Id = @Id AND Cerrada = 0;
END
GO
