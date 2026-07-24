USE FrontOne;
GO

-- Usuario manager con rol Administrador y todos los permisos.
-- La contraseña se guarda cifrada con el CryptoService de la aplicación (AES-256).

-- Acciones base
INSERT INTO Seguridad.Accion (Nombre)
SELECT v.Nombre
FROM (VALUES ('Consultar'), ('Crear'), ('Modificar'), ('Eliminar')) AS v(Nombre)
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Accion a WHERE a.Nombre = v.Nombre);
GO

-- Módulo y pantallas de Seguridad
INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Seguridad', 'Usuarios, roles, permisos y configuración'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Seguridad');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, v.Nombre, v.Descripcion
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Usuarios', 'Administración de usuarios'),
    ('Roles', 'Administración de roles y permisos'),
    ('ConfiguracionConexiones', 'Credenciales SQL Server y SAP HANA')
) AS v(Nombre, Descripcion)
WHERE m.Nombre = 'Seguridad'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = v.Nombre);
GO

-- Rol Administrador
INSERT INTO Seguridad.Rol (Nombre, Descripcion, Activo)
SELECT 'Administrador', 'Acceso total al sistema', 1
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Rol WHERE Nombre = 'Administrador');
GO

-- Permisos: rol Administrador sobre TODAS las pantallas y TODAS las acciones existentes
INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO

-- Usuario manager
INSERT INTO Seguridad.Usuario (NombreUsuario, NombreCompleto, Email, PasswordEncriptado, Activo)
SELECT 'manager', 'Manager FrontOne', NULL, 'wJF6Utcl15nYVZfBRo195A==', 1
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Usuario WHERE NombreUsuario = 'manager');
GO

-- Asignar rol Administrador a manager
INSERT INTO Seguridad.UsuarioRol (UsuarioId, RolId)
SELECT u.Id, r.Id
FROM Seguridad.Usuario u
CROSS JOIN Seguridad.Rol r
WHERE u.NombreUsuario = 'manager'
  AND r.Nombre = 'Administrador'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.UsuarioRol ur WHERE ur.UsuarioId = u.Id AND ur.RolId = r.Id);
GO
