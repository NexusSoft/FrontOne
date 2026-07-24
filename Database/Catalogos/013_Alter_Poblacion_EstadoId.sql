USE FrontOne;
GO

SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Catalogos.Poblacion') AND name = 'EstadoId')
BEGIN
    ALTER TABLE Catalogos.Poblacion ADD EstadoId INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Catalogos_Poblacion_Estado')
BEGIN
    ALTER TABLE Catalogos.Poblacion
        ADD CONSTRAINT FK_Catalogos_Poblacion_Estado FOREIGN KEY (EstadoId) REFERENCES Catalogos.Estado (Id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Catalogos_Poblacion_EstadoId')
BEGIN
    CREATE INDEX IX_Catalogos_Poblacion_EstadoId ON Catalogos.Poblacion (EstadoId);
END
GO

IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_Catalogos_Poblacion_Nombre')
BEGIN
    ALTER TABLE Catalogos.Poblacion DROP CONSTRAINT UQ_Catalogos_Poblacion_Nombre;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Catalogos_Poblacion_Nombre_Estado')
BEGIN
    CREATE UNIQUE INDEX UQ_Catalogos_Poblacion_Nombre_Estado ON Catalogos.Poblacion (Nombre, EstadoId) WHERE EstadoId IS NOT NULL;
END
GO
