USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerPorNombreUsuario
    @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        NombreUsuario,
        NombreCompleto,
        Email,
        PasswordEncriptado,
        Activo,
        FechaCreacion
    FROM Seguridad.Usuario
    WHERE NombreUsuario = @NombreUsuario;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerPermisos
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        m.Nombre AS Modulo,
        p.Nombre AS Pantalla,
        a.Nombre AS Accion
    FROM Seguridad.UsuarioRol ur
    INNER JOIN Seguridad.Permiso pe ON pe.RolId = ur.RolId
    INNER JOIN Seguridad.Pantalla p ON p.Id = pe.PantallaId
    INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
    INNER JOIN Seguridad.Accion a ON a.Id = pe.AccionId
    WHERE ur.UsuarioId = @UsuarioId;
END
GO
