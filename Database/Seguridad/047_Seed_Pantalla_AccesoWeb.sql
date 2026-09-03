USE FrontOne;
GO

-- Pantalla AccesoWeb del módulo Seguridad: llave maestra del sitio FrontOne.Web — un usuario sin
-- este permiso no puede iniciar sesión en el sitio aunque su usuario/contraseña sean válidos
-- (FrontOne.Web valida "AccesoWeb/Consultar" justo después de autenticar, antes de emitir la
-- cookie). Se administra desde WinForms con el botón [Permisos de Aplicación Web]
-- (PermisosAplicacionWebForm), mismo criterio que AccesoMovil para la app móvil.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'AccesoWeb', 'Permiso de acceso al sitio web FrontOne.Web (sin pantalla propia en escritorio)'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Seguridad'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'AccesoWeb');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Seguridad'
  AND p.Nombre = 'AccesoWeb'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
