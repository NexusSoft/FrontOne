USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
GO

-- =============================================================================
-- Importación inicial masiva de Productores y Huertas desde el Excel del usuario
-- "Huertas y Productores.xlsx" (hoja LISTADO GRA. ABR-OCT.2026, 54,137 filas).
--
-- Los TSV de staging se generan con el script Python exportar_import.py
-- (scratchpad de la sesión) a partir del Excel:
--   - stg_productores.tsv: Clave, Nombre, PoblacionId, Municipio  (33,696 filas)
--   - stg_huertas.tsv:     Nombre, Productor, PoblacionId, Municipio, Sagarpa,
--                          Superficie, Altura, Latitud, Longitud  (54,137 filas)
--
-- Decisiones acordadas con el usuario:
--   - Cartilla del Excel: se ignora (no es derivable del SAGARPA, sin campo destino).
--   - Domicilio del productor: el de su PRIMERA huerta en el archivo.
--   - Clave de productor: secuencial 000001..033696 en orden de aparición.
--   - EstadoId = 16 (Michoacán de Ocampo) para todo.
-- Este script es idempotente a nivel de guardas: solo corre si Productor y
-- Huerta están vacías (importación inicial, no re-ejecutable sobre datos).
-- =============================================================================

IF EXISTS (SELECT 1 FROM Catalogos.Productor) OR EXISTS (SELECT 1 FROM Catalogos.Huerta)
BEGIN
    RAISERROR('Las tablas Productor/Huerta no están vacías — importación inicial abortada.', 16, 1);
    SET NOEXEC ON;
END
GO

-- ----------------------------------------------------------------------------
-- Staging
-- ----------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#StgProductor') IS NOT NULL DROP TABLE #StgProductor;
CREATE TABLE #StgProductor
(
    Clave       NVARCHAR(6)     NOT NULL,
    Nombre      NVARCHAR(200)   NOT NULL,
    PoblacionId INT             NOT NULL,
    Municipio   NVARCHAR(100)   NOT NULL
);

IF OBJECT_ID('tempdb..#StgHuerta') IS NOT NULL DROP TABLE #StgHuerta;
CREATE TABLE #StgHuerta
(
    Nombre      NVARCHAR(150)   NOT NULL,
    Productor   NVARCHAR(200)   NOT NULL,
    PoblacionId INT             NOT NULL,
    Municipio   NVARCHAR(100)   NOT NULL,
    Sagarpa     NVARCHAR(50)    NOT NULL,
    Superficie  DECIMAL(10,2)   NOT NULL,
    Altura      DECIMAL(10,2)   NOT NULL,
    Latitud     DECIMAL(9,6)    NOT NULL,
    Longitud    DECIMAL(9,6)    NOT NULL
);

BULK INSERT #StgProductor
FROM 'C:\Users\Sistemas\AppData\Local\Temp\claude\C--Users-Sistemas-Documents-Visual-Studio-2017-FrontOne\ff483040-a20d-4e45-9781-f9f9a5ea12ab\scratchpad\stg_productores.tsv'
WITH (CODEPAGE = '65001', FIELDTERMINATOR = '\t', ROWTERMINATOR = '0x0a', TABLOCK);

BULK INSERT #StgHuerta
FROM 'C:\Users\Sistemas\AppData\Local\Temp\claude\C--Users-Sistemas-Documents-Visual-Studio-2017-FrontOne\ff483040-a20d-4e45-9781-f9f9a5ea12ab\scratchpad\stg_huertas.tsv'
WITH (CODEPAGE = '65001', FIELDTERMINATOR = '\t', ROWTERMINATOR = '0x0a', TABLOCK);

DECLARE @stgProductores INT = (SELECT COUNT(*) FROM #StgProductor);
DECLARE @stgHuertas INT = (SELECT COUNT(*) FROM #StgHuerta);
PRINT CONCAT('Staging cargado: ', @stgProductores, ' productores, ', @stgHuertas, ' huertas');

-- 33,190 productores tras normalizar espacios/mayúsculas en el nombre (el Excel traía
-- 506 variantes del mismo productor con espacios extra o case distinto).
IF @stgProductores <> 33190 OR @stgHuertas <> 54137
BEGIN
    RAISERROR('Conteo de staging inesperado — importación abortada.', 16, 1);
    SET NOEXEC ON;
END
GO

-- ----------------------------------------------------------------------------
-- Inserción definitiva (transaccional)
-- ----------------------------------------------------------------------------
BEGIN TRANSACTION;

BEGIN TRY
    INSERT INTO Catalogos.Productor (Clave, NombreProductor, PoblacionId, Municipio, EstadoId, DiasCredito, Activo)
    SELECT Clave, Nombre, PoblacionId, Municipio, 16, 0, 1
    FROM #StgProductor;

    PRINT CONCAT('Productores insertados: ', @@ROWCOUNT);

    INSERT INTO Catalogos.Huerta
        (Nombre, ProductorId, PoblacionId, Municipio, EstadoId, RegistroSagarpa,
         Superficie, Altura, Latitud, Longitud, CertificadoGlobalGap, Activo)
    SELECT h.Nombre, p.Id, h.PoblacionId, h.Municipio, 16, h.Sagarpa,
           h.Superficie, h.Altura, h.Latitud, h.Longitud, 0, 1
    FROM #StgHuerta h
    INNER JOIN Catalogos.Productor p ON p.NombreProductor = h.Productor;

    DECLARE @huertasInsertadas INT = @@ROWCOUNT;
    PRINT CONCAT('Huertas insertadas: ', @huertasInsertadas);

    IF @huertasInsertadas <> 54137
    BEGIN
        RAISERROR('El join Huerta-Productor no cubrió todas las filas — rollback.', 16, 1);
    END

    COMMIT TRANSACTION;
    PRINT 'Importación completada y confirmada.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT CONCAT('ERROR — rollback ejecutado: ', ERROR_MESSAGE());
    THROW;
END CATCH
GO

SET NOEXEC OFF;
GO
