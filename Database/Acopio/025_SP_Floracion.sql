USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Floracion_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Floracion AS Nombre, Activo FROM Acopio.Floracion WHERE (@Id IS NULL OR Id = @Id) ORDER BY Floracion;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Floracion_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.Floracion (Floracion, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Floracion_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.Floracion SET Floracion = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Floracion_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.Floracion WHERE Id = @Id;
END
GO
