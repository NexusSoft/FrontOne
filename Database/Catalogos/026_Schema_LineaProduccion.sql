USE FrontOne;
GO

-- Catálogo simple para el módulo Lotes: línea de producción del embarque (mercado destino).
-- Clon exacto del patrón Acopio.Variedad (Id/Nombre/Activo).
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Catalogos.LineaProduccion'))
BEGIN
    CREATE TABLE Catalogos.LineaProduccion
    (
        Id     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Catalogos_LineaProduccion PRIMARY KEY,
        Nombre NVARCHAR(100)     NOT NULL,
        Activo BIT               NOT NULL CONSTRAINT DF_Catalogos_LineaProduccion_Activo DEFAULT (1),
        CONSTRAINT UQ_Catalogos_LineaProduccion_Nombre UNIQUE (Nombre)
    );
END
GO
