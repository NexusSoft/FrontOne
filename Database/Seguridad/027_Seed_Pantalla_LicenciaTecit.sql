USE FrontOne;
GO

-- Pantalla LicenciaTecit del módulo Seguridad, con permisos completos para el rol Administrador.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'LicenciaTecit', 'Datos de licencia de TECIT TBarCode.NET para generar códigos de barras en reportes'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Seguridad'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'LicenciaTecit');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Seguridad'
  AND p.Nombre = 'LicenciaTecit'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
