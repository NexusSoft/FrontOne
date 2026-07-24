USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Zona AS Nombre, Activo FROM Acarreo.Zona WHERE (@Id IS NULL OR Id = @Id) ORDER BY Zona;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acarreo.Zona (Zona, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acarreo.Zona SET Zona = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acarreo.sp_Zona_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acarreo.Zona WHERE Id = @Id;
END
GO
