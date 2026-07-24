USE FrontOne;
GO

-- Catálogo de tipos de pago (lookup simple, se referencia desde TipoCorte).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.TipoPago'))
BEGIN
    CREATE TABLE Acopio.TipoPago
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_TipoPago PRIMARY KEY,
        Nombre NVARCHAR(100)     NOT NULL,
        Activo BIT               NOT NULL CONSTRAINT DF_Acopio_TipoPago_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_TipoPago_Nombre UNIQUE (Nombre)
    );
END
GO

-- Catálogo de tipos de corte: tolerancia de gramaje fuera de norma, si exige daño mínimo,
-- y con qué tipo de pago se liquida.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.TipoCorte'))
BEGIN
    CREATE TABLE Acopio.TipoCorte
    (
        Id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_TipoCorte PRIMARY KEY,
        Nombre          NVARCHAR(100)     NOT NULL,
        FueraDeNormaGr  DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Acopio_TipoCorte_FueraDeNormaGr DEFAULT (0),
        DanioMinimo     BIT               NOT NULL CONSTRAINT DF_Acopio_TipoCorte_DanioMinimo DEFAULT (0),
        TipoPagoId      INT               NOT NULL,
        Activo          BIT               NOT NULL CONSTRAINT DF_Acopio_TipoCorte_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_TipoCorte_Nombre UNIQUE (Nombre),
        CONSTRAINT FK_Acopio_TipoCorte_TipoPago FOREIGN KEY (TipoPagoId) REFERENCES Acopio.TipoPago (Id)
    );
END
GO
