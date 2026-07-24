USE FrontOne;
GO

-- Los índices filtrados exigen QUOTED_IDENTIFIER ON en la sesión que los crea.
SET QUOTED_IDENTIFIER ON;
GO

-- Lista de precios de corte: un renglón por empresa/proveedor de corte (CardCode/CardName
-- vienen de SAP, grupo de proveedores "Cosecha"), con 3 precios que se capturan a mano en
-- FrontOne. Sin vigencia por fecha (a diferencia de Acopio.ListaPrecioFruta) — se captura
-- tipo grid editable, igual que Acarreo.ListaPrecioAcarreo.
-- Incluye un renglón especial "Otros" (CardCode NULL, EsOtros = 1) que no viene de SAP: sirve
-- de precio por defecto cuando la empresa de corte usada en un Acuerdo de Corte no tiene
-- precio propio capturado en esta lista.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte'))
BEGIN
    CREATE TABLE Acopio.ListaPrecioCorte
    (
        Id              INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Acopio_ListaPrecioCorte PRIMARY KEY,
        CardCode        NVARCHAR(20)        NULL,
        CardName        NVARCHAR(200)       NOT NULL,
        PrecioKg        DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_PrecioKg DEFAULT (0),
        PrecioDia       DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_PrecioDia DEFAULT (0),
        CuadrillaApoyo  DECIMAL(18,2)       NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_CuadrillaApoyo DEFAULT (0),
        EsOtros         BIT                 NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_EsOtros DEFAULT (0),
        Activo          BIT                 NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_Activo DEFAULT (1)
    );
END
GO

-- Un CardCode de SAP no puede repetirse en la lista (NULL solo lo usa el renglón "Otros",
-- por eso el índice filtra CardCode IS NOT NULL). Fuera del IF de creación de tabla para que
-- pueda re-ejecutarse el script sin volver a intentar crear la tabla.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'UQ_Acopio_ListaPrecioCorte_CardCode')
BEGIN
    CREATE UNIQUE INDEX UQ_Acopio_ListaPrecioCorte_CardCode
        ON Acopio.ListaPrecioCorte (CardCode)
        WHERE CardCode IS NOT NULL;
END
GO

-- Solo puede existir un renglón "Otros".
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'UQ_Acopio_ListaPrecioCorte_EsOtros')
BEGIN
    CREATE UNIQUE INDEX UQ_Acopio_ListaPrecioCorte_EsOtros
        ON Acopio.ListaPrecioCorte (EsOtros)
        WHERE EsOtros = 1;
END
GO
