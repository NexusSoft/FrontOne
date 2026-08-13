USE FrontOne;
GO

-- Permisos de la aplicación móvil (FrontOne.Android): modelo separado del genérico
-- Pantalla/Accion/Permiso, mismo criterio que Seguridad.ReportePermiso. PantallaCodigo no lleva
-- FK a ninguna tabla: el catálogo de pantallas móviles vive en código
-- (FrontOne.Domain.Constants.PantallasMovilDisponibles).
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Seguridad' AND t.name = 'MovilPermiso')
BEGIN
    CREATE TABLE Seguridad.MovilPermiso
    (
        Id             INT           NOT NULL CONSTRAINT PK_Seguridad_MovilPermiso PRIMARY KEY IDENTITY(1,1),
        RolId          INT           NOT NULL CONSTRAINT FK_Seguridad_MovilPermiso_Rol REFERENCES Seguridad.Rol (Id),
        PantallaCodigo NVARCHAR(50)  NOT NULL,
        Consultar      BIT           NOT NULL CONSTRAINT DF_Seguridad_MovilPermiso_Consultar DEFAULT (0),
        Crear          BIT           NOT NULL CONSTRAINT DF_Seguridad_MovilPermiso_Crear DEFAULT (0),
        Modificar      BIT           NOT NULL CONSTRAINT DF_Seguridad_MovilPermiso_Modificar DEFAULT (0),
        Eliminar       BIT           NOT NULL CONSTRAINT DF_Seguridad_MovilPermiso_Eliminar DEFAULT (0),
        CONSTRAINT UQ_Seguridad_MovilPermiso UNIQUE (RolId, PantallaCodigo)
    );
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_MovilPermiso_ObtenerPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RolId, PantallaCodigo, Consultar, Crear, Modificar, Eliminar
    FROM Seguridad.MovilPermiso
    WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_MovilPermiso_EliminarPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.MovilPermiso WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_MovilPermiso_Insertar
    @RolId          INT,
    @PantallaCodigo NVARCHAR(50),
    @Consultar      BIT,
    @Crear          BIT,
    @Modificar      BIT,
    @Eliminar       BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.MovilPermiso (RolId, PantallaCodigo, Consultar, Crear, Modificar, Eliminar)
    VALUES (@RolId, @PantallaCodigo, @Consultar, @Crear, @Modificar, @Eliminar);
END
GO

-- Se llama desde el mismo flujo de login que Seguridad.sp_Usuario_ObtenerPermisos (mismo
-- shape de salida: Modulo/Pantalla/Accion, una fila por acción otorgada) para que
-- FrontOne.Android pueda concatenar ambos resultados sin transformar nada. Como el catálogo de
-- pantallas móviles no vive en SQL, el Modulo se resuelve a mano (AccesoMovil es del módulo
-- Seguridad, todo lo demás es AplicacionMovil). Se usa UNION (no UNION ALL) para deduplicar solo
-- si el usuario tiene el permiso otorgado por más de un rol.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerMovilPermisos
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Otorgados AS (
        SELECT DISTINCT pm.PantallaCodigo, pm.Consultar, pm.Crear, pm.Modificar, pm.Eliminar
        FROM Seguridad.UsuarioRol ur
        INNER JOIN Seguridad.MovilPermiso pm ON pm.RolId = ur.RolId
        WHERE ur.UsuarioId = @UsuarioId
    )
    SELECT CASE WHEN PantallaCodigo = 'AccesoMovil' THEN 'Seguridad' ELSE 'AplicacionMovil' END AS Modulo,
           PantallaCodigo AS Pantalla, 'Consultar' AS Accion
    FROM Otorgados WHERE Consultar = 1
    UNION
    SELECT CASE WHEN PantallaCodigo = 'AccesoMovil' THEN 'Seguridad' ELSE 'AplicacionMovil' END,
           PantallaCodigo, 'Crear'
    FROM Otorgados WHERE Crear = 1
    UNION
    SELECT CASE WHEN PantallaCodigo = 'AccesoMovil' THEN 'Seguridad' ELSE 'AplicacionMovil' END,
           PantallaCodigo, 'Modificar'
    FROM Otorgados WHERE Modificar = 1
    UNION
    SELECT CASE WHEN PantallaCodigo = 'AccesoMovil' THEN 'Seguridad' ELSE 'AplicacionMovil' END,
           PantallaCodigo, 'Eliminar'
    FROM Otorgados WHERE Eliminar = 1;
END
GO
