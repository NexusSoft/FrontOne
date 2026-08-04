USE FrontOne;
GO

-- Módulo Almacenes (nuevo) y su primera pantalla, AlmacenCajaCampo (dashboard de existencias/
-- pérdidas de Caja de Campo), con permisos completos para el rol Administrador. Mismo patrón
-- exacto que 029_Seed_Modulo_Lotes.sql.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Almacenes', 'Control de inventario de almacenes (Caja de Campo)'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Almacenes');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'AlmacenCajaCampo', 'Dashboard de existencias y pérdidas de Caja de Campo'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Almacenes'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'AlmacenCajaCampo');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Almacenes'
  AND p.Nombre = 'AlmacenCajaCampo'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
