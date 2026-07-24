USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Productor'))
BEGIN
    CREATE TABLE Catalogos.Productor
    (
        Id                  INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Productor PRIMARY KEY,
        Clave               NVARCHAR(6)        NOT NULL,
        FechaRegistro       DATETIME2          NOT NULL CONSTRAINT DF_Catalogos_Productor_FechaRegistro DEFAULT (SYSUTCDATETIME()),
        NombreProductor     NVARCHAR(200)      NOT NULL,
        Domicilio           NVARCHAR(200)      NULL,
        Colonia             NVARCHAR(100)      NULL,
        CodigoPostal        NVARCHAR(10)       NULL,
        Poblacion           NVARCHAR(100)      NULL,
        Municipio           NVARCHAR(100)      NULL,
        EstadoId            INT                NULL,
        Rfc                 NVARCHAR(20)       NULL,
        Telefono            NVARCHAR(30)       NULL,
        Fax                 NVARCHAR(30)       NULL,
        Celular             NVARCHAR(30)       NULL,
        Email               NVARCHAR(150)      NULL,
        Organizacion        NVARCHAR(150)      NULL,
        Observaciones       NVARCHAR(500)      NULL,
        Usuario             NVARCHAR(50)       NULL,
        PasswordEncriptado  NVARCHAR(500)      NULL,
        DiasCredito         INT                NOT NULL CONSTRAINT DF_Catalogos_Productor_DiasCredito DEFAULT (0),
        Activo              BIT                NOT NULL CONSTRAINT DF_Catalogos_Productor_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_Productor_Clave UNIQUE (Clave),
        CONSTRAINT FK_Catalogos_Productor_Estado FOREIGN KEY (EstadoId) REFERENCES Catalogos.Estado (Id)
    );

    CREATE INDEX IX_Catalogos_Productor_EstadoId ON Catalogos.Productor (EstadoId);
END
GO
