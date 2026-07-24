USE FrontOne;
GO

-- Catálogo Floración: qué floración de la huerta corresponde a la Orden de Corte (ej. "Marceña",
-- "Aventajada"...). Columna de datos se llama literal "Floracion" (pedido explícito del
-- usuario), el SP la expone como "Nombre" para mantener la convención del resto de catálogos.
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID('Acopio.Floracion'))
BEGIN
    CREATE TABLE Acopio.Floracion
    (
        Id        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Acopio_Floracion PRIMARY KEY,
        Floracion NVARCHAR(100)     NOT NULL,
        Activo    BIT               NOT NULL CONSTRAINT DF_Acopio_Floracion_Activo DEFAULT (1),
        CONSTRAINT UQ_Acopio_Floracion_Floracion UNIQUE (Floracion)
    );
END
GO
