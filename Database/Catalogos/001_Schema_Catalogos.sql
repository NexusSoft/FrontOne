USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Catalogos')
    EXEC('CREATE SCHEMA Catalogos');
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Pais'))
BEGIN
    CREATE TABLE Catalogos.Pais
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Pais PRIMARY KEY,
        Clave   NVARCHAR(3)        NOT NULL,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_Pais_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_Pais_Clave UNIQUE (Clave),
        CONSTRAINT UQ_Catalogos_Pais_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Estado'))
BEGIN
    CREATE TABLE Catalogos.Estado
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Estado PRIMARY KEY,
        PaisId  INT                NOT NULL,
        Clave   NVARCHAR(10)       NOT NULL,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_Estado_Activo DEFAULT (1),
        CONSTRAINT FK_Catalogos_Estado_Pais FOREIGN KEY (PaisId) REFERENCES Catalogos.Pais (Id),
        CONSTRAINT UQ_Catalogos_Estado_Pais_Nombre UNIQUE (PaisId, Nombre),
        CONSTRAINT UQ_Catalogos_Estado_Pais_Clave UNIQUE (PaisId, Clave)
    );

    CREATE INDEX IX_Catalogos_Estado_PaisId ON Catalogos.Estado (PaisId);
END
GO
