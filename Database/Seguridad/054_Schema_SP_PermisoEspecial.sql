USE FrontOne;
GO

-- Permisos especiales por rol: modelo independiente de los permisos de pantallas web.
-- Codigo no lleva FK a ninguna tabla: el catálogo de acciones vive en código
-- (FrontOne.Domain.Constants.PermisosEspecialesDisponibles).
IF NOT EXISTS (SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
               WHERE s.name = 'Seguridad' AND t.name = 'PermisoEspecial')
BEGIN
    CREATE TABLE Seguridad.PermisoEspecial
    (
        Id             INT           NOT NULL CONSTRAINT PK_Seguridad_PermisoEspecial PRIMARY KEY IDENTITY(1,1),
        RolId          INT           NOT NULL CONSTRAINT FK_Seguridad_PermisoEspecial_Rol REFERENCES Seguridad.Rol (Id),
        Codigo         NVARCHAR(50)  NOT NULL,
        Habilitado     BIT           NOT NULL CONSTRAINT DF_Seguridad_PermisoEspecial_Habilitado DEFAULT (0),
        CONSTRAINT UQ_Seguridad_PermisoEspecial UNIQUE (RolId, Codigo)
    );
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoEspecial_ObtenerPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, RolId, Codigo, Habilitado
    FROM Seguridad.PermisoEspecial
    WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoEspecial_EliminarPorRol
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Seguridad.PermisoEspecial WHERE RolId = @RolId;
END
GO

CREATE OR ALTER PROCEDURE Seguridad.sp_PermisoEspecial_Insertar
    @RolId          INT,
    @Codigo         NVARCHAR(50),
    @Habilitado     BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Seguridad.PermisoEspecial (RolId, Codigo, Habilitado)
    VALUES (@RolId, @Codigo, @Habilitado);
END
GO

-- Obtiene los códigos habilitados por cualquiera de los roles del usuario.
CREATE OR ALTER PROCEDURE Seguridad.sp_Usuario_ObtenerPermisosEspeciales
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT pe.Codigo
    FROM Seguridad.UsuarioRol ur
    INNER JOIN Seguridad.PermisoEspecial pe ON pe.RolId = ur.RolId
    WHERE ur.UsuarioId = @UsuarioId AND pe.Habilitado = 1;
END
GO
