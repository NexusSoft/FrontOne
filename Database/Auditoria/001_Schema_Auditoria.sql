USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Auditoria')
    EXEC('CREATE SCHEMA Auditoria');
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Auditoria.Registro'))
BEGIN
    CREATE TABLE Auditoria.Registro
    (
        Id                  BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Auditoria_Registro PRIMARY KEY,
        Usuario             NVARCHAR(50)          NOT NULL,
        Fecha               DATETIME2             NOT NULL,
        Equipo              NVARCHAR(100)         NOT NULL,
        Ip                  NVARCHAR(45)          NOT NULL,
        Accion              NVARCHAR(50)          NOT NULL,
        Modulo              NVARCHAR(100)         NOT NULL,
        ValoresAnteriores   NVARCHAR(MAX)         NULL,
        ValoresNuevos       NVARCHAR(MAX)         NULL
    );

    CREATE INDEX IX_Auditoria_Registro_Fecha ON Auditoria.Registro (Fecha DESC);
    CREATE INDEX IX_Auditoria_Registro_Usuario ON Auditoria.Registro (Usuario);
END
GO
