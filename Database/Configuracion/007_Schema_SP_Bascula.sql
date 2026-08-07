USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Configuracion')
BEGIN
    EXEC('CREATE SCHEMA Configuracion');
END
GO

-- Tabla singleton (mismo criterio que Configuracion.Empresa): siempre existe exactamente un
-- registro Id = 1 con los parámetros del puerto serie de la báscula. Nunca se inserta ni se
-- elimina desde la aplicación, solo se actualiza — por eso NO entra al arreglo @Tablas de
-- Database/Utilidades/Inicializar_Datos_Produccion.sql (un TRUNCATE la dejaría vacía y
-- rompería sp_Bascula_Obtener, que siempre espera esa fila).
--
-- Parity y StopBits se guardan como el valor numérico de los enums System.IO.Ports.Parity y
-- System.IO.Ports.StopBits, para no tener que traducir cadenas en el repositorio.
-- PatronLectura es una expresión regular con un grupo de captura que aísla el número dentro de
-- la trama cruda que manda la báscula (ej. 'ST,GS,\s*([0-9.]+)\s*kg'); si va en NULL/vacío, el
-- servicio de lectura toma el primer número que encuentre en la trama.
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Configuracion' AND t.name = 'Bascula')
BEGIN
    CREATE TABLE Configuracion.Bascula
    (
        Id            INT           NOT NULL CONSTRAINT PK_Configuracion_Bascula PRIMARY KEY
                                     CONSTRAINT CK_Configuracion_Bascula_Id CHECK (Id = 1),
        Puerto        NVARCHAR(10)  NOT NULL CONSTRAINT DF_Configuracion_Bascula_Puerto DEFAULT (''),
        BaudRate      INT           NOT NULL CONSTRAINT DF_Configuracion_Bascula_BaudRate DEFAULT (9600),
        Parity        TINYINT       NOT NULL CONSTRAINT DF_Configuracion_Bascula_Parity DEFAULT (0),
        DataBits      TINYINT       NOT NULL CONSTRAINT DF_Configuracion_Bascula_DataBits DEFAULT (8),
        StopBits      TINYINT       NOT NULL CONSTRAINT DF_Configuracion_Bascula_StopBits DEFAULT (1),
        PatronLectura NVARCHAR(200) NULL,
        FechaModificacion DATETIME2 NOT NULL CONSTRAINT DF_Configuracion_Bascula_FechaModificacion DEFAULT (SYSDATETIME())
    );

    INSERT INTO Configuracion.Bascula (Id, Puerto)
    VALUES (1, '');
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Bascula_Obtener
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Puerto, BaudRate, Parity, DataBits, StopBits, PatronLectura, FechaModificacion
    FROM Configuracion.Bascula
    WHERE Id = 1;
END
GO

CREATE OR ALTER PROCEDURE Configuracion.sp_Bascula_Actualizar
    @Puerto        NVARCHAR(10),
    @BaudRate      INT,
    @Parity        TINYINT,
    @DataBits      TINYINT,
    @StopBits      TINYINT,
    @PatronLectura NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Configuracion.Bascula
    SET Puerto = @Puerto,
        BaudRate = @BaudRate,
        Parity = @Parity,
        DataBits = @DataBits,
        StopBits = @StopBits,
        PatronLectura = @PatronLectura,
        FechaModificacion = SYSDATETIME()
    WHERE Id = 1;
END
GO
