USE FrontOne;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.Productor') AND name = 'Fax')
    ALTER TABLE Catalogos.Productor DROP COLUMN Fax;
GO
