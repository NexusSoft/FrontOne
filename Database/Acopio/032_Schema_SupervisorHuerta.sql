USE FrontOne;
GO

-- Catálogo de Supervisores de Huerta (captura de campo del módulo Incidencias) — lookup simple,
-- mismo patrón que Acopio.TipoPago.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.SupervisorHuerta'))
BEGIN
    CREATE TABLE Acopio.SupervisorHuerta
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_SupervisorHuerta PRIMARY KEY,
        Nombre NVARCHAR(100)     NOT NULL,
        Activo BIT               NOT NULL CONSTRAINT DF_Acopio_SupervisorHuerta_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_SupervisorHuerta_Nombre UNIQUE (Nombre)
    );
END
GO
