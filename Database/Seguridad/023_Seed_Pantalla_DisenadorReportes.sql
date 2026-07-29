USE FrontOne;
GO

-- Pantalla del Diseñador de Reportes (editor en vivo, dentro de la app) — mismo módulo Seguridad
-- que usan las demás pantallas de la página "Sistema" del Ribbon (ej. ConfiguracionEmpresa),
-- permisos sembrados SOLO para el rol Administrador (pedido explícito del usuario, para que un
-- usuario operativo no pueda desacomodar el diseño del reporte sin querer).

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'DisenadorReportes', 'Editor en vivo de plantillas de reportes'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Seguridad'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'DisenadorReportes');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Seguridad'
  AND p.Nombre = 'DisenadorReportes'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
