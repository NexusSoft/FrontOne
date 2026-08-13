USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_SupervisorHuerta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Acopio.SupervisorHuerta WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_SupervisorHuerta_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.SupervisorHuerta (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_SupervisorHuerta_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.SupervisorHuerta SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_SupervisorHuerta_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.SupervisorHuerta WHERE Id = @Id;
END
GO
