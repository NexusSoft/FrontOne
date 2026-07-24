USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Acarreo')
BEGIN
    EXEC('CREATE SCHEMA Acarreo');
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acarreo.Zona'))
BEGIN
    CREATE TABLE Acarreo.Zona
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acarreo_Zona PRIMARY KEY,
        Zona    NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Acarreo_Zona_Activo DEFAULT (1),
        CONSTRAINT UQ_Acarreo_Zona_Zona UNIQUE (Zona)
    );
END
GO
