USE FrontOne;
GO

-- Módulo Catálogos y sus pantallas, con permisos completos para el rol Administrador.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Catalogos', 'Catálogos generales del sistema'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Catalogos');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, v.Nombre, v.Descripcion
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Paises', 'Catálogo de países'),
    ('Estados', 'Catálogo de estados/provincias'),
    ('Productores', 'Catálogo de productores')
) AS v(Nombre, Descripcion)
WHERE m.Nombre = 'Catalogos'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = v.Nombre);
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Catalogos'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
