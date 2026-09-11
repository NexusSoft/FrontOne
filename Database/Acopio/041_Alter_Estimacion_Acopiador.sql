USE FrontOne;
GO

-- Acopiador que está capturando la Estimación — snapshot igual criterio que
-- Acopio.OrdenCorte.JefeAcopioId/JefeAcopioNombre (el catálogo Acopio.JefeAcopio puede cambiar
-- después sin alterar estimaciones ya guardadas). Opcional: no siempre se conoce al capturar.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.Estimacion') AND name = 'AcopiadorId')
BEGIN
    ALTER TABLE Acopio.Estimacion ADD AcopiadorId INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.Estimacion') AND name = 'AcopiadorNombre')
BEGIN
    ALTER TABLE Acopio.Estimacion ADD AcopiadorNombre NVARCHAR(200) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Acopio_Estimacion_JefeAcopio')
BEGIN
    ALTER TABLE Acopio.Estimacion
        ADD CONSTRAINT FK_Acopio_Estimacion_JefeAcopio FOREIGN KEY (AcopiadorId) REFERENCES Acopio.JefeAcopio (Id);
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        e.Id, e.Folio, e.Fecha, e.HuertaId, h.Nombre AS HuertaNombre, e.RegistroSagarpa, e.Kilos,
        e.AcopiadorId, e.AcopiadorNombre,
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

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Insertar
    @Fecha                  DATE,
    @HuertaId               INT,
    @RegistroSagarpa        NVARCHAR(50) = NULL,
    @Kilos                  DECIMAL(18,2),
    @AcopiadorId            INT = NULL,
    @AcopiadorNombre        NVARCHAR(200) = NULL,
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
        (Folio, Fecha, HuertaId, RegistroSagarpa, Kilos, AcopiadorId, AcopiadorNombre,
         PorcentajeCat1, PorcentajeCat2, PorcentajeNal,
         PorcentajeCalibre32, PorcentajeCalibre36, PorcentajeCalibre40, PorcentajeCalibre48,
         PorcentajeCalibre60, PorcentajeCalibre70, PorcentajeCalibre84, PorcentajeCalibre90,
         PorcentajeBorona, PorcentajeCanica, PorcentajeCuarta, PorcentajeDesecho, PorcentajeProceso,
         ListaPrecioFecha, ListaPrecioProductorId, TipoLista, PrecioSugerido)
    VALUES
        (@Folio, @Fecha, @HuertaId, @RegistroSagarpa, @Kilos, @AcopiadorId, @AcopiadorNombre,
         @PorcentajeCat1, @PorcentajeCat2, @PorcentajeNal,
         @PorcentajeCalibre32, @PorcentajeCalibre36, @PorcentajeCalibre40, @PorcentajeCalibre48,
         @PorcentajeCalibre60, @PorcentajeCalibre70, @PorcentajeCalibre84, @PorcentajeCalibre90,
         @PorcentajeBorona, @PorcentajeCanica, @PorcentajeCuarta, @PorcentajeDesecho, @PorcentajeProceso,
         @ListaPrecioFecha, @ListaPrecioProductorId, @TipoLista, @PrecioSugerido);

    COMMIT TRANSACTION;

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id, @Folio AS Folio;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Actualizar
    @Id                     INT,
    @Fecha                  DATE,
    @HuertaId               INT,
    @RegistroSagarpa        NVARCHAR(50) = NULL,
    @Kilos                  DECIMAL(18,2),
    @AcopiadorId            INT = NULL,
    @AcopiadorNombre        NVARCHAR(200) = NULL,
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
        AcopiadorId = @AcopiadorId,
        AcopiadorNombre = @AcopiadorNombre,
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
