USE FrontOne;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Acopio.TipoPago') AND name = 'NecesitaListaPrecios')
BEGIN
    ALTER TABLE Acopio.TipoPago
        ADD NecesitaListaPrecios BIT NOT NULL CONSTRAINT DF_Acopio_TipoPago_NecesitaListaPrecios DEFAULT (0);
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, NecesitaListaPrecios, Activo FROM Acopio.TipoPago WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Insertar
    @Nombre NVARCHAR(100), @NecesitaListaPrecios BIT = 0, @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.TipoPago (Nombre, NecesitaListaPrecios, Activo) VALUES (@Nombre, @NecesitaListaPrecios, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoPago_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @NecesitaListaPrecios BIT = 0, @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.TipoPago SET Nombre = @Nombre, NecesitaListaPrecios = @NecesitaListaPrecios, Activo = @Activo WHERE Id = @Id;
END
GO
