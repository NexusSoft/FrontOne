USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Obtener
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, NombreUsuario, NombreCompleto, Email, PasswordEncriptado, Activo, FechaCreacion
    FROM Seguridad.Usuario
    WHERE (@Id IS NULL OR Id = @Id)
    ORDER BY NombreUsuario;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Insertar
    @NombreUsuario      NVARCHAR(50),
    @NombreCompleto     NVARCHAR(150),
    @Email              NVARCHAR(150) = NULL,
    @PasswordEncriptado NVARCHAR(500),
    @Activo             BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.Usuario (NombreUsuario, NombreCompleto, Email, PasswordEncriptado, Activo)
    VALUES (@NombreUsuario, @NombreCompleto, @Email, @PasswordEncriptado, @Activo);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Actualizar
    @Id                 INT,
    @NombreUsuario      NVARCHAR(50),
    @NombreCompleto     NVARCHAR(150),
    @Email              NVARCHAR(150) = NULL,
    @PasswordEncriptado NVARCHAR(500),
    @Activo             BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Seguridad.Usuario
    SET NombreUsuario      = @NombreUsuario,
        NombreCompleto     = @NombreCompleto,
        Email              = @Email,
        PasswordEncriptado = @PasswordEncriptado,
        Activo             = @Activo
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_Eliminar
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.UsuarioRol WHERE UsuarioId = @Id;
    DELETE FROM Seguridad.Usuario WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerRoles
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RolId FROM Seguridad.UsuarioRol WHERE UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_UsuarioRol_EliminarPorUsuario
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.UsuarioRol WHERE UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_UsuarioRol_Insertar
    @UsuarioId INT,
    @RolId     INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.UsuarioRol (UsuarioId, RolId)
    VALUES (@UsuarioId, @RolId);
END
GO
