USE FrontOne;
GO

-- Kg mínimos de acarreo por zona, según el número de cajas del viaje (300/400/500) — se usará
-- en la programación de acarreo de una orden de corte, igual que los precios ya existentes en
-- Acarreo.ListaPrecioAcarreo (que van por Municipio, no por Zona).
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acarreo.Zona') AND name = 'KgMinimo300')
BEGIN
    ALTER TABLE Acarreo.Zona ADD KgMinimo300 DECIMAL(18,2) NOT NULL CONSTRAINT DF_Acarreo_Zona_KgMinimo300 DEFAULT (0);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acarreo.Zona') AND name = 'KgMinimo400')
BEGIN
    ALTER TABLE Acarreo.Zona ADD KgMinimo400 DECIMAL(18,2) NOT NULL CONSTRAINT DF_Acarreo_Zona_KgMinimo400 DEFAULT (0);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acarreo.Zona') AND name = 'KgMinimo500')
BEGIN
    ALTER TABLE Acarreo.Zona ADD KgMinimo500 DECIMAL(18,2) NOT NULL CONSTRAINT DF_Acarreo_Zona_KgMinimo500 DEFAULT (0);
END
GO
