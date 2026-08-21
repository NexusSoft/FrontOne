USE FrontOne;
GO

-- Re-anclar Acopio.ListaPrecioFruta: deja de identificarse por ItemCode/ItemName (SAP, en
-- vivo) + VariedadId, y pasa a identificarse por la combinación CategoriaId+CalibreApeamId,
-- tomada del catálogo local Catalogos.MateriaPrima (ya no se llama a SAP en absoluto). El
-- LookUpEdit de Variedad se retira del todo — decisión confirmada con el usuario. Al mismo
-- tiempo, las columnas de precio Lista1/Lista2/Lista3 se renombran a Convencional/Organico/
-- Nacional (identificador técnico sin acento en "Organico", igual que el resto del proyecto
-- — el caption con acento "Orgánica" lo sigue resolviendo FrontOne.Domain.Constants.
-- ListasPrecioFruta.Nombres, que no cambia).
--
-- El módulo sigue en pruebas: se descarta lo capturado en vez de migrarlo (no hay forma
-- automática de mapear ItemCode -> CategoriaId/CalibreApeamId sin intervención manual fila
-- por fila) — mismo criterio que 014_Alter_GastoFrutaCategoria_MateriaPrima.sql.
TRUNCATE TABLE Acopio.ListaPrecioFruta;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Acopio_ListaPrecioFruta_ItemCode' AND object_id = OBJECT_ID('Acopio.ListaPrecioFruta'))
    DROP INDEX IX_Acopio_ListaPrecioFruta_ItemCode ON Acopio.ListaPrecioFruta;
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Acopio_ListaPrecioFruta_Variedad')
    ALTER TABLE Acopio.ListaPrecioFruta DROP CONSTRAINT FK_Acopio_ListaPrecioFruta_Variedad;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'VariedadId')
    ALTER TABLE Acopio.ListaPrecioFruta DROP COLUMN VariedadId;
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'ItemCode')
    ALTER TABLE Acopio.ListaPrecioFruta DROP COLUMN ItemCode;
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'ItemName')
    ALTER TABLE Acopio.ListaPrecioFruta DROP COLUMN ItemName;
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'Lista1')
    ALTER TABLE Acopio.ListaPrecioFruta DROP COLUMN Lista1;
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'Lista2')
    ALTER TABLE Acopio.ListaPrecioFruta DROP COLUMN Lista2;
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'Lista3')
    ALTER TABLE Acopio.ListaPrecioFruta DROP COLUMN Lista3;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'CategoriaId')
    ALTER TABLE Acopio.ListaPrecioFruta ADD CategoriaId INT NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'CalibreApeamId')
    ALTER TABLE Acopio.ListaPrecioFruta ADD CalibreApeamId INT NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'Convencional')
    ALTER TABLE Acopio.ListaPrecioFruta ADD Convencional DECIMAL(18,4) NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'Organico')
    ALTER TABLE Acopio.ListaPrecioFruta ADD Organico DECIMAL(18,4) NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta') AND name = 'Nacional')
    ALTER TABLE Acopio.ListaPrecioFruta ADD Nacional DECIMAL(18,4) NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Acopio_ListaPrecioFruta_Categoria')
    ALTER TABLE Acopio.ListaPrecioFruta
        ADD CONSTRAINT FK_Acopio_ListaPrecioFruta_Categoria FOREIGN KEY (CategoriaId) REFERENCES Catalogos.Categoria (Id);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Acopio_ListaPrecioFruta_CalibreApeam')
    ALTER TABLE Acopio.ListaPrecioFruta
        ADD CONSTRAINT FK_Acopio_ListaPrecioFruta_CalibreApeam FOREIGN KEY (CalibreApeamId) REFERENCES Catalogos.CalibreApeam (Id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Acopio_ListaPrecioFruta_CategoriaId_CalibreApeamId' AND object_id = OBJECT_ID('Acopio.ListaPrecioFruta'))
    CREATE INDEX IX_Acopio_ListaPrecioFruta_CategoriaId_CalibreApeamId ON Acopio.ListaPrecioFruta (CategoriaId, CalibreApeamId);
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lpf.Id, lpf.CategoriaId, c.Nombre AS CategoriaNombre, lpf.CalibreApeamId, ca.Nombre AS CalibreApeamNombre,
           lpf.Convencional, lpf.Organico, lpf.Nacional, lpf.FechaInicio, lpf.FechaFin, lpf.ProductorId, lpf.Activo, lpf.FechaCreacion
    FROM Acopio.ListaPrecioFruta lpf
    JOIN Catalogos.Categoria c ON c.Id = lpf.CategoriaId
    JOIN Catalogos.CalibreApeam ca ON ca.Id = lpf.CalibreApeamId
    WHERE (@Id IS NULL OR lpf.Id = @Id)
    ORDER BY lpf.FechaInicio DESC, lpf.CategoriaId, lpf.CalibreApeamId;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Insertar
    @CategoriaId    INT,
    @CalibreApeamId INT,
    @Convencional   DECIMAL(18,4),
    @Organico       DECIMAL(18,4),
    @Nacional       DECIMAL(18,4),
    @FechaInicio    DATE,
    @FechaFin       DATE = NULL,
    @ProductorId    INT = NULL,
    @Activo         BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Acopio.ListaPrecioFruta (CategoriaId, CalibreApeamId, Convencional, Organico, Nacional, FechaInicio, FechaFin, ProductorId, Activo)
    VALUES (@CategoriaId, @CalibreApeamId, @Convencional, @Organico, @Nacional, @FechaInicio, @FechaFin, @ProductorId, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- CategoriaId/ItemName no se tocan aquí (identidad de la combinación); solo se puede corregir
-- la vigencia (fecha inicio/fin) y, de paso, los precios/estado activo.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_Actualizar
    @Id           INT,
    @Convencional DECIMAL(18,4),
    @Organico     DECIMAL(18,4),
    @Nacional     DECIMAL(18,4),
    @FechaInicio  DATE,
    @FechaFin     DATE = NULL,
    @Activo       BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Acopio.ListaPrecioFruta
    SET Convencional = @Convencional,
        Organico = @Organico,
        Nacional = @Nacional,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

-- Traslape: NULL-safe en ProductorId, llave pasa a CategoriaId+CalibreApeamId (ya no ItemCode
-- ni Variedad).
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ExisteTraslape
    @CategoriaId    INT,
    @CalibreApeamId INT,
    @FechaInicio    DATE,
    @FechaFin       DATE = NULL,
    @ProductorId    INT = NULL,
    @IdExcluir      INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
        SELECT 1 FROM Acopio.ListaPrecioFruta
        WHERE CategoriaId = @CategoriaId AND CalibreApeamId = @CalibreApeamId
          AND Activo = 1
          AND (@IdExcluir IS NULL OR Id <> @IdExcluir)
          AND FechaInicio <= ISNULL(@FechaFin, @FechaInicio)
          AND ISNULL(FechaFin, FechaInicio) >= @FechaInicio
          AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId)
    ) THEN 1 ELSE 0 END AS BIT) AS Existe;
END
GO

-- Vigencia navegable: fecha + productor (general o especial) — la variedad ya no aplica.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerFechas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT lpf.FechaInicio, lpf.ProductorId, p.NombreProductor AS ProductorNombre
    FROM Acopio.ListaPrecioFruta lpf
    LEFT JOIN Catalogos.Productor p ON p.Id = lpf.ProductorId
    WHERE lpf.Activo = 1
    ORDER BY lpf.FechaInicio DESC, ProductorNombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerPorFecha
    @Fecha       DATE,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lpf.Id, lpf.CategoriaId, c.Nombre AS CategoriaNombre, lpf.CalibreApeamId, ca.Nombre AS CalibreApeamNombre,
           lpf.Convencional, lpf.Organico, lpf.Nacional, lpf.FechaInicio, lpf.FechaFin, lpf.ProductorId, lpf.Activo, lpf.FechaCreacion
    FROM Acopio.ListaPrecioFruta lpf
    JOIN Catalogos.Categoria c ON c.Id = lpf.CategoriaId
    JOIN Catalogos.CalibreApeam ca ON ca.Id = lpf.CalibreApeamId
    WHERE lpf.FechaInicio = @Fecha
      AND lpf.Activo = 1
      AND ((@ProductorId IS NULL AND lpf.ProductorId IS NULL) OR lpf.ProductorId = @ProductorId)
    ORDER BY lpf.CategoriaId, lpf.CalibreApeamId;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_EliminarPorFecha
    @Fecha       DATE,
    @ProductorId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Acopio.ListaPrecioFruta
    WHERE FechaInicio = @Fecha
      AND ((@ProductorId IS NULL AND ProductorId IS NULL) OR ProductorId = @ProductorId);
END
GO

-- Universo de combinaciones capturables: solo materias primas activas en SAP, con Categoría y
-- Calibre APEAM ya asignados en FrontOne. Varias Materia Prima pueden compartir la misma
-- combinación (empaques distintos del mismo Categoría+Calibre) — por eso DISTINCT.
CREATE OR ALTER PROCEDURE Acopio.sp_ListaPrecioFruta_ObtenerCombinacionesMateriaPrima
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT mp.CategoriaId, c.Nombre AS CategoriaNombre, mp.CalibreApeamId, ca.Nombre AS CalibreApeamNombre
    FROM Catalogos.MateriaPrima mp
    JOIN Catalogos.Categoria c ON c.Id = mp.CategoriaId
    JOIN Catalogos.CalibreApeam ca ON ca.Id = mp.CalibreApeamId
    WHERE mp.Activo = 1 AND mp.CategoriaId IS NOT NULL AND mp.CalibreApeamId IS NOT NULL
    ORDER BY CategoriaNombre, CalibreApeamNombre;
END
GO
