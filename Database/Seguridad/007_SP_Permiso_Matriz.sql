USE FrontOne;
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Permiso_ObtenerEstructura
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.Id   AS ModuloId,
        m.Nombre AS ModuloNombre,
        p.Id   AS PantallaId,
        p.Nombre AS PantallaNombre,
        a.Id   AS AccionId,
        a.Nombre AS AccionNombre
    FROM Seguridad.Modulo m
    INNER JOIN Seguridad.Pantalla p ON p.ModuloId = m.Id
    CROSS JOIN Seguridad.Accion a
    ORDER BY m.Nombre, p.Nombre, a.Nombre;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Permiso_ObtenerPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PantallaId, AccionId
    FROM Seguridad.Permiso
    WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Permiso_EliminarPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.Permiso WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_Permiso_Insertar
    @RolId      INT,
    @PantallaId INT,
    @AccionId   INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
    VALUES (@RolId, @PantallaId, @AccionId);
END
GO
