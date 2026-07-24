USE FrontOne;
GO

-- Variedad
CREATE OR ALTER PROCEDURE Acopio.sp_Variedad_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Acopio.Variedad WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Variedad_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.Variedad (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Variedad_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.Variedad SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Variedad_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.Variedad WHERE Id = @Id;
END
GO

-- TipoComercializacion
CREATE OR ALTER PROCEDURE Acopio.sp_TipoComercializacion_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Acopio.TipoComercializacion WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoComercializacion_Insertar
    @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.TipoComercializacion (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoComercializacion_Actualizar
    @Id INT, @Nombre NVARCHAR(100), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.TipoComercializacion SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_TipoComercializacion_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.TipoComercializacion WHERE Id = @Id;
END
GO

-- Moneda
CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Activo FROM Acopio.Moneda WHERE (@Id IS NULL OR Id = @Id) ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Insertar
    @Nombre NVARCHAR(50), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Acopio.Moneda (Nombre, Activo) VALUES (@Nombre, @Activo);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Actualizar
    @Id INT, @Nombre NVARCHAR(50), @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Acopio.Moneda SET Nombre = @Nombre, Activo = @Activo WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Acopio.sp_Moneda_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Acopio.Moneda WHERE Id = @Id;
END
GO
