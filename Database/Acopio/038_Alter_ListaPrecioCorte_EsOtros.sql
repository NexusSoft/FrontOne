USE FrontOne;
GO

-- Los índices filtrados exigen QUOTED_IDENTIFIER ON en la sesión que los crea.
SET QUOTED_IDENTIFIER ON;
GO

-- Faltaba en ambientes donde Acopio.ListaPrecioCorte se creó antes de que 018_Schema_ListaPrecioCorte.sql
-- incluyera EsOtros (renglón "Otros": precio por defecto cuando la empresa de corte no tiene precio propio).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.ListaPrecioCorte') AND name = 'EsOtros')
BEGIN
    ALTER TABLE Acopio.ListaPrecioCorte ADD EsOtros BIT NOT NULL CONSTRAINT DF_Acopio_ListaPrecioCorte_EsOtros DEFAULT (0);
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
