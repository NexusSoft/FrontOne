USE FrontOne;
GO

-- Catálogos chicos nuevos que necesita Acuerdo de Corte.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.Variedad'))
BEGIN
    CREATE TABLE Acopio.Variedad
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_Variedad PRIMARY KEY,
        Nombre NVARCHAR(100)     NOT NULL,
        Activo BIT               NOT NULL CONSTRAINT DF_Acopio_Variedad_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_Variedad_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.TipoComercializacion'))
BEGIN
    CREATE TABLE Acopio.TipoComercializacion
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_TipoComercializacion PRIMARY KEY,
        Nombre NVARCHAR(100)     NOT NULL,
        Activo BIT               NOT NULL CONSTRAINT DF_Acopio_TipoComercializacion_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_TipoComercializacion_Nombre UNIQUE (Nombre)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.Moneda'))
BEGIN
    CREATE TABLE Acopio.Moneda
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_Moneda PRIMARY KEY,
        Nombre NVARCHAR(50)      NOT NULL,
        Activo BIT               NOT NULL CONSTRAINT DF_Acopio_Moneda_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_Moneda_Nombre UNIQUE (Nombre)
    );
END
GO

-- Folio consecutivo de 7 dígitos (0000001, 0000002...), independiente del Id.
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqAcuerdoCorteFolio' AND schema_id = SCHEMA_ID('Acopio'))
BEGIN
    CREATE SEQUENCE Acopio.SeqAcuerdoCorteFolio AS INT START WITH 1 INCREMENT BY 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.AcuerdoCorte'))
BEGIN
    CREATE TABLE Acopio.AcuerdoCorte
    (
        Id                      INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acopio_AcuerdoCorte PRIMARY KEY,
        Folio                   NVARCHAR(7)         NOT NULL,
        ProductorId             INT                 NOT NULL,
        FechaInicio             DATE                NOT NULL,
        FechaFin                DATE                NOT NULL,
        ProductoId              INT                 NOT NULL,
        VariedadId              INT                 NOT NULL,
        TipoComercializacionId  INT                 NOT NULL,
        TipoCorteId             INT                 NOT NULL,
        -- Exactamente uno de los dos según Acopio.TipoPago.NecesitaListaPrecios del TipoCorte
        -- elegido (validado en Application, no aquí): precio fijo o referencia a una vigencia
        -- de Acopio.ListaPrecioFruta (fecha + productor de esa lista, NULL = lista general).
        Precio                  DECIMAL(18,4)       NULL,
        ListaPrecioFecha        DATE                NULL,
        ListaPrecioProductorId  INT                 NULL,
        MonedaId                INT                 NOT NULL,
        Observaciones           NVARCHAR(500)       NULL,
        Activo                  BIT                 NOT NULL CONSTRAINT DF_Acopio_AcuerdoCorte_Activo DEFAULT (1),
        FechaCreacion           DATETIME2           NOT NULL CONSTRAINT DF_Acopio_AcuerdoCorte_FechaCreacion DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT UQ_Acopio_AcuerdoCorte_Folio UNIQUE (Folio),
        CONSTRAINT FK_Acopio_AcuerdoCorte_Productor FOREIGN KEY (ProductorId) REFERENCES Catalogos.Productor (Id),
        CONSTRAINT FK_Acopio_AcuerdoCorte_Producto FOREIGN KEY (ProductoId) REFERENCES Catalogos.Producto (Id),
        CONSTRAINT FK_Acopio_AcuerdoCorte_Variedad FOREIGN KEY (VariedadId) REFERENCES Acopio.Variedad (Id),
        CONSTRAINT FK_Acopio_AcuerdoCorte_TipoComercializacion FOREIGN KEY (TipoComercializacionId) REFERENCES Acopio.TipoComercializacion (Id),
        CONSTRAINT FK_Acopio_AcuerdoCorte_TipoCorte FOREIGN KEY (TipoCorteId) REFERENCES Acopio.TipoCorte (Id),
        CONSTRAINT FK_Acopio_AcuerdoCorte_Moneda FOREIGN KEY (MonedaId) REFERENCES Acopio.Moneda (Id),
        CONSTRAINT FK_Acopio_AcuerdoCorte_ListaPrecioProductor FOREIGN KEY (ListaPrecioProductorId) REFERENCES Catalogos.Productor (Id)
    );
END
GO
