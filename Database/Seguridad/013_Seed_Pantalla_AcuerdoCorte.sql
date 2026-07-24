USE FrontOne;
GO

-- 4 pantallas nuevas del módulo Acopio (3 catálogos chicos + Acuerdo de Corte), con permisos
-- completos para el rol Administrador.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, v.Nombre, v.Descripcion
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Variedades', 'Catálogo de variedades'),
    ('TiposComercializacion', 'Catálogo de tipos de comercialización'),
    ('Monedas', 'Catálogo de monedas'),
    ('AcuerdosCorte', 'Acuerdos de corte')
) AS v(Nombre, Descripcion)
WHERE m.Nombre = 'Acopio'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = v.Nombre);
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Acopio'
  AND p.Nombre IN ('Variedades', 'TiposComercializacion', 'Monedas', 'AcuerdosCorte')
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
