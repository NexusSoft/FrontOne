USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Acopio')
    EXEC('CREATE SCHEMA Acopio');
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.ListaPrecioFruta'))
BEGIN
    CREATE TABLE Acopio.ListaPrecioFruta
    (
        Id            INT IDENTITY(1,1)   NOT NULL CONSTRAINT PK_Acopio_ListaPrecioFruta PRIMARY KEY,
        ItemCode      NVARCHAR(50)        NOT NULL,
        ItemName      NVARCHAR(200)       NOT NULL,
        Lista1        DECIMAL(18,4)       NOT NULL,
        Lista2        DECIMAL(18,4)       NOT NULL,
        Lista3        DECIMAL(18,4)       NOT NULL,
        FechaInicio   DATE                NOT NULL,
        FechaFin      DATE                NULL,
        Activo        BIT                 NOT NULL CONSTRAINT DF_Acopio_ListaPrecioFruta_Activo DEFAULT (1),
        FechaCreacion DATETIME2           NOT NULL CONSTRAINT DF_Acopio_ListaPrecioFruta_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        -- El rango vigente jamás debe cruzar la fecha fin antes de la fecha inicio.
        CONSTRAINT CK_Acopio_ListaPrecioFruta_Rango CHECK (FechaFin IS NULL OR FechaFin >= FechaInicio)
    );

    CREATE INDEX IX_Acopio_ListaPrecioFruta_ItemCode ON Acopio.ListaPrecioFruta (ItemCode);
END
GO
