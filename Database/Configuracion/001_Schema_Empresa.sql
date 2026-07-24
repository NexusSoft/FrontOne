USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Configuracion')
BEGIN
    EXEC('CREATE SCHEMA Configuracion');
END
GO

-- Tabla singleton: siempre existe exactamente un registro (Id = 1) con los datos de la
-- empresa que se imprimen en el membrete de los reportes. Nunca se inserta ni se elimina
-- desde la aplicación, solo se actualiza.
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Configuracion' AND t.name = 'Empresa')
BEGIN
    CREATE TABLE Configuracion.Empresa
    (
        Id                INT           NOT NULL CONSTRAINT PK_Configuracion_Empresa PRIMARY KEY
                                         CONSTRAINT CK_Configuracion_Empresa_Id CHECK (Id = 1),
        RazonSocial       NVARCHAR(200) NOT NULL CONSTRAINT DF_Configuracion_Empresa_RazonSocial DEFAULT (''),
        Domicilio         NVARCHAR(300) NULL,
        Rfc               NVARCHAR(20)  NULL,
        Telefono          NVARCHAR(30)  NULL,
        Correo            NVARCHAR(150) NULL,
        Logo              VARBINARY(MAX) NULL,
        FechaModificacion DATETIME2     NOT NULL CONSTRAINT DF_Configuracion_Empresa_FechaModificacion DEFAULT (SYSDATETIME())
    );

    INSERT INTO Configuracion.Empresa (Id, RazonSocial)
    VALUES (1, '');
END
GO
