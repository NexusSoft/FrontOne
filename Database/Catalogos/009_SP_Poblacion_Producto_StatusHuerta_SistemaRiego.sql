USE FrontOne;
GO

-- Poblacion
CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Catalogos.Poblacion WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Catalogos.Poblacion (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Catalogos.Poblacion SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Poblacion_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Catalogos.Poblacion WHERE Id = @Id;
END
GO

-- Producto
CREATE OR ALTER PROCEDURE Catalogos.sp_Producto_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Catalogos.Producto WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Producto_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Catalogos.Producto (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Producto_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Catalogos.Producto SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_Producto_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Catalogos.Producto WHERE Id = @Id;
END
GO

-- StatusHuerta
CREATE OR ALTER PROCEDURE Catalogos.sp_StatusHuerta_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Catalogos.StatusHuerta WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_StatusHuerta_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Catalogos.StatusHuerta (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_StatusHuerta_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Catalogos.StatusHuerta SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_StatusHuerta_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Catalogos.StatusHuerta WHERE Id = @Id;
END
GO

-- SistemaRiego
CREATE OR ALTER PROCEDURE Catalogos.sp_SistemaRiego_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Catalogos.SistemaRiego WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_SistemaRiego_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Catalogos.SistemaRiego (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_SistemaRiego_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Catalogos.SistemaRiego SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Catalogos.sp_SistemaRiego_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Catalogos.SistemaRiego WHERE Id = @Id;
END
GO
