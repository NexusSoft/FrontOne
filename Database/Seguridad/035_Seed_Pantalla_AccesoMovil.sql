USE FrontOne;
GO

-- Pantalla AccesoMovil del módulo Seguridad: no tiene UI propia en escritorio, es un
-- permiso puro que FrontOne.Android consulta después de validar la contraseña
-- (sp_Usuario_ObtenerPermisos) para decidir si ese usuario puede iniciar sesión en la
-- app móvil. Se administra desde la pantalla de Permisos ya existente, como cualquier
-- otro permiso — no hace falta pantalla nueva en WinForms. El rol Administrador (y por
-- lo tanto "manager") lo recibe automáticamente por el mismo patrón de seed que ya
-- usa el resto del proyecto.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'AccesoMovil', 'Permiso de acceso a la aplicación móvil FrontOne.Android (sin pantalla propia en escritorio)'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Seguridad'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'AccesoMovil');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Seguridad'
  AND p.Nombre = 'AccesoMovil'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
