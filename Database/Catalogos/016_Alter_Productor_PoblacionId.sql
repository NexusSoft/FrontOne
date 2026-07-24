USE FrontOne;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.Productor') AND name = 'Poblacion')
BEGIN
    ALTER TABLE Catalogos.Productor DROP COLUMN Poblacion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.Productor') AND name = 'PoblacionId')
BEGIN
    ALTER TABLE Catalogos.Productor ADD PoblacionId INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Catalogos_Productor_Poblacion')
BEGIN
    ALTER TABLE Catalogos.Productor
        ADD CONSTRAINT FK_Catalogos_Productor_Poblacion FOREIGN KEY (PoblacionId) REFERENCES Catalogos.Poblacion (Id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Catalogos_Productor_PoblacionId')
BEGIN
    CREATE INDEX IX_Catalogos_Productor_PoblacionId ON Catalogos.Productor (PoblacionId);
END
GO
