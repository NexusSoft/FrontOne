USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.Huerta'))
BEGIN
    CREATE TABLE Catalogos.Huerta
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Catalogos_Huerta PRIMARY KEY,
        Nombre                  NVARCHAR(150)       NOT NULL,
        ProductorId             INT                 NOT NULL,
        Ubicacion               NVARCHAR(200)       NULL,
        PoblacionId             INT                 NULL,
        Municipio               NVARCHAR(100)       NULL,
        EstadoId                INT                 NULL,
        ProductoId              INT                 NULL,
        EncargadoNombre         NVARCHAR(150)       NULL,
        EncargadoTelefono       NVARCHAR(30)        NULL,
        Observaciones           NVARCHAR(500)       NULL,
        RegistroSagarpa         NVARCHAR(50)        NULL,
        CertificadoGlobalGap    BIT                 NOT NULL CONSTRAINT DF_Catalogos_Huerta_GlobalGap DEFAULT (0),
        RegistroFda             NVARCHAR(50)        NULL,
        NumeroGlobalGap         NVARCHAR(50)        NULL,
        Superficie              DECIMAL(10,2)       NULL,
        Altura                  DECIMAL(10,2)       NULL,
        NumeroArboles           INT                 NULL,
        EdadArboles             INT                 NULL,
        SistemaRiegoId          INT                 NULL,
        PorcentajeMecanizacion  DECIMAL(5,2)         NULL,
        StatusHuertaId          INT                 NULL,
        FechaCambioStatus       DATE                NULL,
        FechaVencimiento        DATE                NULL,
        Activo                  BIT                 NOT NULL CONSTRAINT DF_Catalogos_Huerta_Activo DEFAULT (1),
        CONSTRAINT FK_Catalogos_Huerta_Productor FOREIGN KEY (ProductorId) REFERENCES Catalogos.Productor (Id),
        CONSTRAINT FK_Catalogos_Huerta_Poblacion FOREIGN KEY (PoblacionId) REFERENCES Catalogos.Poblacion (Id),
        CONSTRAINT FK_Catalogos_Huerta_Estado FOREIGN KEY (EstadoId) REFERENCES Catalogos.Estado (Id),
        CONSTRAINT FK_Catalogos_Huerta_Producto FOREIGN KEY (ProductoId) REFERENCES Catalogos.Producto (Id),
        CONSTRAINT FK_Catalogos_Huerta_SistemaRiego FOREIGN KEY (SistemaRiegoId) REFERENCES Catalogos.SistemaRiego (Id),
        CONSTRAINT FK_Catalogos_Huerta_StatusHuerta FOREIGN KEY (StatusHuertaId) REFERENCES Catalogos.StatusHuerta (Id)
    );

    CREATE INDEX IX_Catalogos_Huerta_ProductorId ON Catalogos.Huerta (ProductorId);

    -- Único filtrado: dos huertas no pueden compartir el mismo Registro SAGARPA (NULL/vacío no cuenta).
    CREATE UNIQUE INDEX UQ_Catalogos_Huerta_RegistroSagarpa
        ON Catalogos.Huerta (RegistroSagarpa)
        WHERE RegistroSagarpa IS NOT NULL;
END
GO
