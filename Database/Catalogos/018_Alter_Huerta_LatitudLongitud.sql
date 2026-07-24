USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.Huerta') AND name = 'Latitud')
BEGIN
    ALTER TABLE Catalogos.Huerta ADD Latitud DECIMAL(9,6) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.Huerta') AND name = 'Longitud')
BEGIN
    ALTER TABLE Catalogos.Huerta ADD Longitud DECIMAL(9,6) NULL;
END
GO
