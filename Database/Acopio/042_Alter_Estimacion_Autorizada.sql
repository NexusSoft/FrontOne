USE FrontOne;
GO

-- Autorización de la Estimación: se otorga/retira desde FrontOne.Web (pendiente, sesión aparte).
-- Mientras Autorizada = 1, EstimacionService.ActualizarAsync rechaza cualquier edición (hay que
-- desautorizar primero) y OrdenCorteEditarForm/OrdenCorteService solo permiten elegir/guardar
-- estimaciones autorizadas cuya Huerta coincida con la de la Orden de Corte.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.Estimacion') AND name = 'Autorizada')
BEGIN
    ALTER TABLE Acopio.Estimacion ADD Autorizada BIT NOT NULL CONSTRAINT DF_Acopio_Estimacion_Autorizada DEFAULT (0);
END
GO

-- Preferencia de captura: si está marcada, EstimacionForm carga automáticamente la vigencia de
-- Lista de Precios más reciente en vez de que el usuario la busque a mano. Se persiste para que
-- el checkbox refleje la última opción elegida al reabrir la Estimación para editarla.
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.Estimacion') AND name = 'UsarListaMasReciente')
BEGIN
    ALTER TABLE Acopio.Estimacion ADD UsarListaMasReciente BIT NOT NULL CONSTRAINT DF_Acopio_Estimacion_UsarListaMasReciente DEFAULT (0);
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
        e.ListaPrecioFecha, e.ListaPrecioProductorId, e.TipoLista, e.PrecioSugerido, e.Cerrada, e.FechaCreacion,
        e.Autorizada, e.UsarListaMasReciente
    FROM Acopio.Estimacion e
    INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
    WHERE (@Id IS NULL OR e.Id = @Id)
    ORDER BY e.FechaCreacion DESC;
END
GO

-- Carga inicial del picker: ahora acepta filtrar por Huerta y por Autorizada (usado desde
-- OrdenCorteEditarForm, que solo debe ofrecer estimaciones autorizadas de la huerta ya elegida).
-- Sin parámetros se comporta igual que antes (usado por EstimacionForm para reabrir cualquiera).
CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_ObtenerTop100
    @HuertaId         INT = NULL,
    @SoloAutorizadas  BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (100)
        e.Id, e.Folio, e.Fecha, h.Nombre AS HuertaNombre, e.PrecioSugerido, e.Cerrada, e.Autorizada
    FROM Acopio.Estimacion e
    INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
    WHERE (@HuertaId IS NULL OR e.HuertaId = @HuertaId)
      AND (@SoloAutorizadas = 0 OR e.Autorizada = 1)
    ORDER BY e.FechaCreacion DESC;
END
GO

-- Búsqueda por texto (Folio o nombre de Huerta), TOP 500 — mismos filtros opcionales de Huerta/
-- Autorizada que el TOP 100.
CREATE OR ALTER PROCEDURE Acopio.sp_Estimacion_Buscar
    @Filtro           NVARCHAR(200),
    @HuertaId         INT = NULL,
    @SoloAutorizadas  BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (500)
        e.Id, e.Folio, e.Fecha, h.Nombre AS HuertaNombre, e.PrecioSugerido, e.Cerrada, e.Autorizada
    FROM Acopio.Estimacion e
    INNER JOIN Catalogos.Huerta h ON h.Id = e.HuertaId
    WHERE (e.Folio LIKE '%' + @Filtro + '%' OR h.Nombre LIKE '%' + @Filtro + '%')
      AND (@HuertaId IS NULL OR e.HuertaId = @HuertaId)
      AND (@SoloAutorizadas = 0 OR e.Autorizada = 1)
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
    @PrecioSugerido         DECIMAL(18,4),
    @UsarListaMasReciente   BIT = 0
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
         ListaPrecioFecha, ListaPrecioProductorId, TipoLista, PrecioSugerido, UsarListaMasReciente)
    VALUES
        (@Folio, @Fecha, @HuertaId, @RegistroSagarpa, @Kilos, @AcopiadorId, @AcopiadorNombre,
         @PorcentajeCat1, @PorcentajeCat2, @PorcentajeNal,
         @PorcentajeCalibre32, @PorcentajeCalibre36, @PorcentajeCalibre40, @PorcentajeCalibre48,
         @PorcentajeCalibre60, @PorcentajeCalibre70, @PorcentajeCalibre84, @PorcentajeCalibre90,
         @PorcentajeBorona, @PorcentajeCanica, @PorcentajeCuarta, @PorcentajeDesecho, @PorcentajeProceso,
         @ListaPrecioFecha, @ListaPrecioProductorId, @TipoLista, @PrecioSugerido, @UsarListaMasReciente);

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
    @PrecioSugerido         DECIMAL(18,4),
    @UsarListaMasReciente   BIT = 0
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
        PrecioSugerido = @PrecioSugerido,
        UsarListaMasReciente = @UsarListaMasReciente
    WHERE Id = @Id AND Cerrada = 0 AND Autorizada = 0;
END
GO
