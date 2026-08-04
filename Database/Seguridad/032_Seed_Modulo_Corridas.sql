USE FrontOne;
GO

-- Módulo Corridas (nuevo) y su primera pantalla, Corridas (Inicio/Fin de proceso de Lotes), con
-- permisos completos para el rol Administrador. Mismo patrón exacto que 029_Seed_Modulo_Lotes.sql.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Corridas', 'Inicio y finalización del proceso de producción de Lotes'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Corridas');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Corridas', 'Corridas'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Corridas'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Corridas');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Corridas'
  AND p.Nombre = 'Corridas'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
