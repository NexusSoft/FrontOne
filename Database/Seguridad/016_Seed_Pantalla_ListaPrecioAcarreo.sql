USE FrontOne;
GO

-- Pantalla ListaPrecioAcarreo del módulo Acarreo, con permisos completos para el rol Administrador.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'ListaPrecioAcarreo', 'Lista de precios de acarreo por municipio/zona'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Acarreo'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'ListaPrecioAcarreo');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Acarreo'
  AND p.Nombre = 'ListaPrecioAcarreo'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
