USE FrontOne;
GO

-- Catálogo compartido por las pestañas Cosecha y Acarreo de Gastos: cada ajuste indica si es
-- de Cosecha o Acarreo (TipoGasto) y si suma o resta al total (Signo).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Gastos.TipoAjuste'))
BEGIN
    CREATE TABLE Gastos.TipoAjuste (
        Id        INT IDENTITY PRIMARY KEY,
        Nombre    NVARCHAR(100) NOT NULL,
        TipoGasto TINYINT NOT NULL, -- 1 = Cosecha, 2 = Acarreo
        Signo     TINYINT NOT NULL, -- 1 = A Favor (suma), 2 = En Contra (resta)
        Activo    BIT NOT NULL DEFAULT 1,
        CONSTRAINT UQ_Gastos_TipoAjuste_Nombre UNIQUE (Nombre)
    );
END
GO
