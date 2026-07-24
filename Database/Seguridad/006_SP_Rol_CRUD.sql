USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nombre, Descripcion, Activo
    FROM Seguridad.Rol
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Insertar
    @Nombre      NVARCHAR(100),
    @Descripcion NVARCHAR(300) = NULL,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.Rol (Nombre, Descripcion, Activo)
    VALUES (@Nombre, @Descripcion, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Actualizar
    @Id          INT,
    @Nombre      NVARCHAR(100),
    @Descripcion NVARCHAR(300) = NULL,
    @Activo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Seguridad.Rol
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Activo = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Rol_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.Permiso WHERE RolId = @Id;
    DELETE FROM Seguridad.UsuarioRol WHERE RolId = @Id;
    DELETE FROM Seguridad.Rol WHERE Id = @Id;
END
GO
