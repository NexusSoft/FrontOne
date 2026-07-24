USE FrontOne;
GO

-- Módulo Acarreo (nuevo) y su primera pantalla, Zonas, con permisos completos para el rol Administrador.
-- Zona es el catálogo base del futuro módulo de lista de precios de acarreo (Zona + Municipio + 3 precios
-- por cantidad de cajas).

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Acarreo', 'Fletes y lista de precios de acarreo'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Acarreo');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Zonas', 'Catálogo de zonas de acarreo'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Acarreo'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Zonas');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Acarreo'
  AND p.Nombre = 'Zonas'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
