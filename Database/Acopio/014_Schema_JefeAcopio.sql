USE FrontOne;
GO

-- Jefes de Acopio: personal interno (no proveedor/cliente), solo datos básicos + contacto.
-- Sin RFC, sin Días de Crédito, sin Usuario/Password de portal, sin Organización — a diferencia
-- de Catalogos.Productor, aquí no aplican esos campos de negocio.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.JefeAcopio'))
BEGIN
    CREATE TABLE Acopio.JefeAcopio
    (
        Id              INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acopio_JefeAcopio PRIMARY KEY,
        Clave           NVARCHAR(6)        NOT NULL,
        Nombre          NVARCHAR(200)      NOT NULL,
        Domicilio       NVARCHAR(200)      NULL,
        Colonia         NVARCHAR(100)      NULL,
        CodigoPostal    NVARCHAR(10)       NULL,
        PoblacionId     INT                NULL CONSTRAINT FK_Acopio_JefeAcopio_Poblacion REFERENCES Catalogos.Poblacion(Id),
        MunicipioId     INT                NULL CONSTRAINT FK_Acopio_JefeAcopio_Municipio REFERENCES Catalogos.Municipio(Id),
        EstadoId        INT                NULL CONSTRAINT FK_Acopio_JefeAcopio_Estado REFERENCES Catalogos.Estado(Id),
        Telefono        NVARCHAR(30)       NULL,
        Celular         NVARCHAR(30)       NULL,
        Email           NVARCHAR(150)      NULL,
        Observaciones   NVARCHAR(500)      NULL,
        Activo          BIT                NOT NULL CONSTRAINT DF_Acopio_JefeAcopio_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_JefeAcopio_Clave UNIQUE (Clave)
    );
END
GO
