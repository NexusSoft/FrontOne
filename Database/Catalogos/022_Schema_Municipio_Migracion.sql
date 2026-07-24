USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
GO

-- =============================================================================
-- Reestructura de la jerarquía geográfica: País -> Estado -> Municipio -> Población.
--
-- Antes: Poblacion colgaba de Estado (y sus 238 registros eran en realidad
-- municipios), y Huerta/Productor guardaban la localidad como texto libre
-- en la columna Municipio.
--
-- Este script:
--   1. Crea Catalogos.Municipio (los 238 registros viejos de Poblacion pasan aquí).
--   2. Importa el catálogo INEGI (staging TSV): 238 municipios ya cubiertos +
--      28,652 localidades como nuevas Poblacion colgando de Municipio.
--   3. Agrega MunicipioId a Poblacion, Huerta y Productor.
--   4. Remapea Huerta/Productor: PoblacionId viejo (municipio) -> MunicipioId;
--      texto Municipio (localidad) -> PoblacionId nuevo (match sin acentos/caso;
--      las localidades sin match INEGI se insertan como Poblacion nuevas).
--   5. Elimina las Poblacion viejas, la columna Poblacion.EstadoId y las
--      columnas de texto Huerta.Municipio / Productor.Municipio.
--
-- Nota técnica: SQL Server resuelve nombres de COLUMNA en tiempo de compilación
-- del batch completo (a diferencia de nombres de tabla, que sí son diferidos),
-- así que cada ALTER TABLE ADD columna va en su propio batch (separado con GO)
-- antes de que algo más la referencie. Como TRY/CATCH no puede cruzar GO,
-- la atomicidad la da SET XACT_ABORT ON (rollback automático ante cualquier
-- error en la transacción, sin necesitar TRY/CATCH).
--
-- Guardas: solo corre si Catalogos.Municipio no existe (migración única).
-- =============================================================================

IF EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Municipio'))
BEGIN
    RAISERROR('Catalogos.Municipio ya existe — migración ya aplicada, abortando.', 16, 1);
    SET NOEXEC ON;
END
GO

SET XACT_ABORT ON;
GO

-- ----------------------------------------------------------------------------
-- 1. Tabla Municipio
-- ----------------------------------------------------------------------------
CREATE TABLE Catalogos.Municipio
(
    Id       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Catalogos_Municipio PRIMARY KEY,
    Nombre   NVARCHAR(150)     NOT NULL,
    EstadoId INT               NOT NULL,
    Activo   BIT               NOT NULL CONSTRAINT DF_Catalogos_Municipio_Activo DEFAULT (1),
    CONSTRAINT FK_Catalogos_Municipio_Estado FOREIGN KEY (EstadoId) REFERENCES Catalogos.Estado (Id),
    CONSTRAINT UQ_Catalogos_Municipio_EstadoNombre UNIQUE (EstadoId, Nombre)
);
GO

-- ----------------------------------------------------------------------------
-- 2. Staging INEGI
-- ----------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#StgMunicipio') IS NOT NULL DROP TABLE #StgMunicipio;
CREATE TABLE #StgMunicipio (EstadoId INT NOT NULL, Nombre NVARCHAR(150) NOT NULL);

IF OBJECT_ID('tempdb..#StgPoblacion') IS NOT NULL DROP TABLE #StgPoblacion;
CREATE TABLE #StgPoblacion (EstadoId INT NOT NULL, Municipio NVARCHAR(150) NOT NULL, Nombre NVARCHAR(150) NOT NULL);

BULK INSERT #StgMunicipio
FROM 'C:\Users\Sistemas\AppData\Local\Temp\claude\C--Users-Sistemas-Documents-Visual-Studio-2017-FrontOne\ff483040-a20d-4e45-9781-f9f9a5ea12ab\scratchpad\stg_municipios.tsv'
WITH (CODEPAGE = '65001', FIELDTERMINATOR = '\t', ROWTERMINATOR = '0x0a', TABLOCK);

BULK INSERT #StgPoblacion
FROM 'C:\Users\Sistemas\AppData\Local\Temp\claude\C--Users-Sistemas-Documents-Visual-Studio-2017-FrontOne\ff483040-a20d-4e45-9781-f9f9a5ea12ab\scratchpad\stg_poblaciones.tsv'
WITH (CODEPAGE = '65001', FIELDTERMINATOR = '\t', ROWTERMINATOR = '0x0a', TABLOCK);

DECLARE @m INT = (SELECT COUNT(*) FROM #StgMunicipio);
DECLARE @p INT = (SELECT COUNT(*) FROM #StgPoblacion);
PRINT CONCAT('Staging: ', @m, ' municipios, ', @p, ' poblaciones INEGI');
IF @m <> 238 OR @p <> 28652
BEGIN
    RAISERROR('Conteo de staging inesperado — abortando.', 16, 1);
    SET NOEXEC ON;
END
GO

BEGIN TRANSACTION;
GO

-- ----------------------------------------------------------------------------
-- 3. Insertar municipios y poblaciones INEGI
-- ----------------------------------------------------------------------------
INSERT INTO Catalogos.Municipio (Nombre, EstadoId)
SELECT Nombre, EstadoId FROM #StgMunicipio;
PRINT CONCAT('Municipios insertados: ', @@ROWCOUNT);
GO

ALTER TABLE Catalogos.Poblacion ADD MunicipioId INT NULL;
GO

-- El índice único viejo (Nombre, EstadoId) asumía que Poblacion = municipio
-- (único por estado). Ahora Poblacion = localidad, y el mismo nombre de
-- localidad puede repetirse en distintos municipios de un mismo estado
-- (INEGI trae este caso real, ej. "Acaspoles" en más de un municipio de
-- Jalisco) — se reemplaza por unicidad (Nombre, MunicipioId) en el paso 5.
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Catalogos_Poblacion_Nombre_Estado')
    DROP INDEX UQ_Catalogos_Poblacion_Nombre_Estado ON Catalogos.Poblacion;
GO

INSERT INTO Catalogos.Poblacion (Nombre, EstadoId, MunicipioId, Activo)
SELECT s.Nombre, s.EstadoId, m.Id, 1
FROM #StgPoblacion s
INNER JOIN Catalogos.Municipio m
    ON m.EstadoId = s.EstadoId AND m.Nombre = s.Municipio;
PRINT CONCAT('Poblaciones INEGI insertadas: ', @@ROWCOUNT);
GO

-- ----------------------------------------------------------------------------
-- 4. Remapeo Huerta / Productor
-- ----------------------------------------------------------------------------
ALTER TABLE Catalogos.Huerta ADD MunicipioId INT NULL;
ALTER TABLE Catalogos.Productor ADD MunicipioId INT NULL;
GO

-- 4a. MunicipioId: la Poblacion vieja (sin MunicipioId) era el municipio.
UPDATE h SET h.MunicipioId = m.Id
FROM Catalogos.Huerta h
INNER JOIN Catalogos.Poblacion pv ON pv.Id = h.PoblacionId AND pv.MunicipioId IS NULL
INNER JOIN Catalogos.Municipio m
    ON m.EstadoId = pv.EstadoId
   AND m.Nombre COLLATE Latin1_General_CI_AI = pv.Nombre COLLATE Latin1_General_CI_AI;
PRINT CONCAT('Huertas con MunicipioId: ', @@ROWCOUNT);

UPDATE p SET p.MunicipioId = m.Id
FROM Catalogos.Productor p
INNER JOIN Catalogos.Poblacion pv ON pv.Id = p.PoblacionId AND pv.MunicipioId IS NULL
INNER JOIN Catalogos.Municipio m
    ON m.EstadoId = pv.EstadoId
   AND m.Nombre COLLATE Latin1_General_CI_AI = pv.Nombre COLLATE Latin1_General_CI_AI;
PRINT CONCAT('Productores con MunicipioId: ', @@ROWCOUNT);

-- El import original (sesión previa) dejó un CR (CHAR 13) pegado al final de
-- Productor.Municipio en las 33,190 filas (era el último campo del TSV de esa
-- carga y el ROWTERMINATOR solo cortaba LF) — se limpia aquí antes de usarlo
-- para el match de localidades no-INEGI en los pasos 4b/4c.
UPDATE Catalogos.Productor SET Municipio = REPLACE(Municipio, CHAR(13), '') WHERE Municipio LIKE '%' + CHAR(13);
PRINT CONCAT('Productores con CR limpiado en Municipio: ', @@ROWCOUNT);

IF EXISTS (SELECT 1 FROM Catalogos.Huerta WHERE MunicipioId IS NULL AND PoblacionId IS NOT NULL)
    RAISERROR('Huertas sin municipio remapeado — rollback.', 16, 1);
IF EXISTS (SELECT 1 FROM Catalogos.Productor WHERE MunicipioId IS NULL AND PoblacionId IS NOT NULL)
    RAISERROR('Productores sin municipio remapeado — rollback.', 16, 1);
GO

-- 4b. Localidades de Huerta/Productor (texto Municipio, ya en #StgHuertaMunicipio
--     via join directo a la tabla real) que NO están en INEGI: se insertan como
--     Poblacion nuevas bajo su municipio. El texto original sigue vivo en
--     Huerta.Municipio/Productor.Municipio hasta el paso 5.
INSERT INTO Catalogos.Poblacion (Nombre, EstadoId, MunicipioId, Activo)
SELECT DISTINCT h.Municipio, m.EstadoId, h.MunicipioId, 1
FROM Catalogos.Huerta h
INNER JOIN Catalogos.Municipio m ON m.Id = h.MunicipioId
WHERE h.Municipio IS NOT NULL
  AND NOT EXISTS (
    SELECT 1 FROM Catalogos.Poblacion pn
    WHERE pn.MunicipioId = h.MunicipioId
      AND pn.Nombre COLLATE Latin1_General_CI_AI = h.Municipio COLLATE Latin1_General_CI_AI);
PRINT CONCAT('Poblaciones nuevas desde huertas (no INEGI): ', @@ROWCOUNT);

INSERT INTO Catalogos.Poblacion (Nombre, EstadoId, MunicipioId, Activo)
SELECT DISTINCT p.Municipio, m.EstadoId, p.MunicipioId, 1
FROM Catalogos.Productor p
INNER JOIN Catalogos.Municipio m ON m.Id = p.MunicipioId
WHERE p.Municipio IS NOT NULL
  AND NOT EXISTS (
    SELECT 1 FROM Catalogos.Poblacion pn
    WHERE pn.MunicipioId = p.MunicipioId
      AND pn.Nombre COLLATE Latin1_General_CI_AI = p.Municipio COLLATE Latin1_General_CI_AI);
PRINT CONCAT('Poblaciones nuevas desde productores (no INEGI): ', @@ROWCOUNT);
GO

-- 4c. Repuntar PoblacionId al registro nuevo (localidad).
UPDATE h SET h.PoblacionId = pn.Id
FROM Catalogos.Huerta h
INNER JOIN Catalogos.Poblacion pn
    ON pn.MunicipioId = h.MunicipioId
   AND pn.Nombre COLLATE Latin1_General_CI_AI = h.Municipio COLLATE Latin1_General_CI_AI
WHERE h.Municipio IS NOT NULL;
PRINT CONCAT('Huertas repuntadas a poblacion nueva: ', @@ROWCOUNT);

UPDATE p SET p.PoblacionId = pn.Id
FROM Catalogos.Productor p
INNER JOIN Catalogos.Poblacion pn
    ON pn.MunicipioId = p.MunicipioId
   AND pn.Nombre COLLATE Latin1_General_CI_AI = p.Municipio COLLATE Latin1_General_CI_AI
WHERE p.Municipio IS NOT NULL;
PRINT CONCAT('Productores repuntados a poblacion nueva: ', @@ROWCOUNT);

-- Verificación: nadie debe seguir apuntando a una Poblacion vieja (municipio).
IF EXISTS (
    SELECT 1 FROM Catalogos.Huerta h
    INNER JOIN Catalogos.Poblacion pv ON pv.Id = h.PoblacionId
    WHERE pv.MunicipioId IS NULL)
    RAISERROR('Quedan huertas apuntando a poblaciones viejas — rollback.', 16, 1);
IF EXISTS (
    SELECT 1 FROM Catalogos.Productor p
    INNER JOIN Catalogos.Poblacion pv ON pv.Id = p.PoblacionId
    WHERE pv.MunicipioId IS NULL)
    RAISERROR('Quedan productores apuntando a poblaciones viejas — rollback.', 16, 1);
GO

-- ----------------------------------------------------------------------------
-- 5. Limpieza: poblaciones viejas, columnas obsoletas, FKs e índices nuevos
-- ----------------------------------------------------------------------------
DELETE FROM Catalogos.Poblacion WHERE MunicipioId IS NULL;
PRINT CONCAT('Poblaciones viejas (municipios) eliminadas: ', @@ROWCOUNT);
GO

ALTER TABLE Catalogos.Poblacion ALTER COLUMN MunicipioId INT NOT NULL;
GO

-- Poblacion: fuera EstadoId (FK + índice + columna)
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Catalogos_Poblacion_Estado')
    ALTER TABLE Catalogos.Poblacion DROP CONSTRAINT FK_Catalogos_Poblacion_Estado;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Catalogos_Poblacion_EstadoId')
    DROP INDEX IX_Catalogos_Poblacion_EstadoId ON Catalogos.Poblacion;
GO

ALTER TABLE Catalogos.Poblacion DROP COLUMN EstadoId;
GO

ALTER TABLE Catalogos.Poblacion
    ADD CONSTRAINT FK_Catalogos_Poblacion_Municipio FOREIGN KEY (MunicipioId) REFERENCES Catalogos.Municipio (Id);
CREATE INDEX IX_Catalogos_Poblacion_MunicipioId ON Catalogos.Poblacion (MunicipioId);
CREATE UNIQUE INDEX UQ_Catalogos_Poblacion_Nombre_Municipio ON Catalogos.Poblacion (Nombre, MunicipioId);
GO

-- Huerta / Productor: fuera texto Municipio, FKs e índices para MunicipioId
ALTER TABLE Catalogos.Huerta DROP COLUMN Municipio;
GO
ALTER TABLE Catalogos.Huerta
    ADD CONSTRAINT FK_Catalogos_Huerta_Municipio FOREIGN KEY (MunicipioId) REFERENCES Catalogos.Municipio (Id);
CREATE INDEX IX_Catalogos_Huerta_MunicipioId ON Catalogos.Huerta (MunicipioId);
GO

ALTER TABLE Catalogos.Productor DROP COLUMN Municipio;
GO
ALTER TABLE Catalogos.Productor
    ADD CONSTRAINT FK_Catalogos_Productor_Municipio FOREIGN KEY (MunicipioId) REFERENCES Catalogos.Municipio (Id);
CREATE INDEX IX_Catalogos_Productor_MunicipioId ON Catalogos.Productor (MunicipioId);
GO

COMMIT TRANSACTION;
PRINT 'Migración Municipio completada y confirmada.';
GO

SET NOEXEC OFF;
GO
