USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Recepcion.RecepcionFruta') AND name = 'NombreBascula')
BEGIN
    ALTER TABLE Recepcion.RecepcionFruta ADD NombreBascula NVARCHAR(100) NULL;
END
GO
