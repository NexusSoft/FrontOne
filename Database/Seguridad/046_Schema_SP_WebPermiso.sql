USE FrontOne;
GO

-- Permisos del sitio web (FrontOne.Web): modelo separado del genérico Pantalla/Accion/Permiso,
-- mismo criterio que Seguridad.MovilPermiso. PantallaCodigo no lleva FK a ninguna tabla: el
-- catálogo de páginas web vive en código (FrontOne.Domain.Constants.PantallasWebDisponibles).
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Seguridad' AND t.name = 'WebPermiso')
BEGIN
    CREATE TABLE Seguridad.WebPermiso
    (
        Id             INT           NOT NULL CONSTRAINT PK_Seguridad_WebPermiso PRIMARY KEY IDENTITY(1,1),
        RolId          INT           NOT NULL CONSTRAINT FK_Seguridad_WebPermiso_Rol REFERENCES Seguridad.Rol (Id),
        PantallaCodigo NVARCHAR(50)  NOT NULL,
        Consultar      BIT           NOT NULL CONSTRAINT DF_Seguridad_WebPermiso_Consultar DEFAULT (0),
        Crear          BIT           NOT NULL CONSTRAINT DF_Seguridad_WebPermiso_Crear DEFAULT (0),
        Modificar      BIT           NOT NULL CONSTRAINT DF_Seguridad_WebPermiso_Modificar DEFAULT (0),
        Eliminar       BIT           NOT NULL CONSTRAINT DF_Seguridad_WebPermiso_Eliminar DEFAULT (0),
        CONSTRAINT UQ_Seguridad_WebPermiso UNIQUE (RolId, PantallaCodigo)
    );
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_WebPermiso_ObtenerPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RolId, PantallaCodigo, Consultar, Crear, Modificar, Eliminar
    FROM Seguridad.WebPermiso
    WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_WebPermiso_EliminarPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.WebPermiso WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_WebPermiso_Insertar
    @RolId          INT,
    @PantallaCodigo NVARCHAR(50),
    @Consultar      BIT,
    @Crear          BIT,
    @Modificar      BIT,
    @Eliminar       BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.WebPermiso (RolId, PantallaCodigo, Consultar, Crear, Modificar, Eliminar)
    VALUES (@RolId, @PantallaCodigo, @Consultar, @Crear, @Modificar, @Eliminar);
END
GO

-- A diferencia de sp_Usuario_ObtenerMovilPermisos (shape propio para la app móvil), este SP
-- regresa exactamente el shape Modulo/Pantalla/Accion de sp_Usuario_ObtenerPermisos porque se
-- mapea directo a PermisoDto (PermissionService.ObtenerWebPermisosAsync) y de ahí a los claims
-- "permisoWeb" de la cookie de FrontOne.Web.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerWebPermisos
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Otorgados AS (
        SELECT DISTINCT wp.PantallaCodigo, wp.Consultar, wp.Crear, wp.Modificar, wp.Eliminar
        FROM Seguridad.UsuarioRol ur
        INNER JOIN Seguridad.WebPermiso wp ON wp.RolId = ur.RolId
        WHERE ur.UsuarioId = @UsuarioId
    )
    SELECT CASE WHEN PantallaCodigo = 'AccesoWeb' THEN 'Seguridad' ELSE 'AplicacionWeb' END AS Modulo,
           PantallaCodigo AS Pantalla, 'Consultar' AS Accion
    FROM Otorgados WHERE Consultar = 1
    UNION
    SELECT CASE WHEN PantallaCodigo = 'AccesoWeb' THEN 'Seguridad' ELSE 'AplicacionWeb' END,
           PantallaCodigo, 'Crear'
    FROM Otorgados WHERE Crear = 1
    UNION
    SELECT CASE WHEN PantallaCodigo = 'AccesoWeb' THEN 'Seguridad' ELSE 'AplicacionWeb' END,
           PantallaCodigo, 'Modificar'
    FROM Otorgados WHERE Modificar = 1
    UNION
    SELECT CASE WHEN PantallaCodigo = 'AccesoWeb' THEN 'Seguridad' ELSE 'AplicacionWeb' END,
           PantallaCodigo, 'Eliminar'
    FROM Otorgados WHERE Eliminar = 1;
END
GO
