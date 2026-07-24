USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Poblacion'))
BEGIN
    CREATE TABLE Catalogos.Poblacion
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Poblacion PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_Poblacion_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_Poblacion_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Producto'))
BEGIN
    CREATE TABLE Catalogos.Producto
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Producto PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_Producto_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_Producto_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.StatusHuerta'))
BEGIN
    CREATE TABLE Catalogos.StatusHuerta
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_StatusHuerta PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_StatusHuerta_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_StatusHuerta_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.SistemaRiego'))
BEGIN
    CREATE TABLE Catalogos.SistemaRiego
    (
        Id      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_SistemaRiego PRIMARY KEY,
        Nombre  NVARCHAR(100)      NOT NULL,
        Activo  BIT                NOT NULL CONSTRAINT DF_Catalogos_SistemaRiego_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_SistemaRiego_Nombre UNIQUE (Nombre)
    );
END
GO
