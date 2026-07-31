USE FrontOne;
GO

-- Tabla singleton: siempre existe exactamente un registro (Id = 1) con los datos de la
-- licencia de TECIT TBarCode.NET usada para generar códigos de barras en los reportes.
-- Nunca se inserta ni se elimina desde la aplicación, solo se actualiza.
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Configuracion' AND t.name = 'LicenciaTecit')
BEGIN
    CREATE TABLE Configuracion.LicenciaTecit
    (
        Id                INT           NOT NULL CONSTRAINT PK_Configuracion_LicenciaTecit PRIMARY KEY
                                         CONSTRAINT CK_Configuracion_LicenciaTecit_Id CHECK (Id = 1),
        Licenciatario     NVARCHAR(200) NOT NULL CONSTRAINT DF_Configuracion_LicenciaTecit_Licenciatario DEFAULT (''),
        ClaveLicencia     NVARCHAR(400) NULL,
        TipoLicencia      NVARCHAR(50)  NULL,
        NumeroLicencias   INT           NULL,
        Producto          NVARCHAR(100) NULL,
        FechaModificacion DATETIME2     NOT NULL CONSTRAINT DF_Configuracion_LicenciaTecit_FechaModificacion DEFAULT (SYSDATETIME())
    );

    INSERT INTO Configuracion.LicenciaTecit (Id, Licenciatario)
    VALUES (1, '');
END
GO
