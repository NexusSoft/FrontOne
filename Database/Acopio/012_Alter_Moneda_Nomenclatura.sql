USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.Moneda') AND name = 'Nomenclatura')
BEGIN
    ALTER TABLE Acopio.Moneda ADD Nomenclatura NVARCHAR(10) NOT NULL CONSTRAINT DF_Acopio_Moneda_Nomenclatura DEFAULT ('');
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Nomenclatura, Activo FROM Acopio.Moneda WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Insertar
    @Nombre NVARCHAR(50), @Nomenclatura NVARCHAR(10), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.Moneda (Nombre, Nomenclatura, Activo) VALUES (@Nombre, @Nomenclatura, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Actualizar
    @Id INT, @Nombre NVARCHAR(50), @Nomenclatura NVARCHAR(10), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.Moneda SET Nombre = @Nombre, Nomenclatura = @Nomenclatura, Activo = @Activo WHERE Id = @Id;
END
GO
