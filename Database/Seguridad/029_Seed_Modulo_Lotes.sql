USE FrontOne;
GO

-- Módulo Lotes (nuevo) y su primera pantalla, Lotes (Conformación de Lotes), con permisos
-- completos para el rol Administrador. Mismo patrón exacto que 022_Seed_Modulo_Recepcion.sql.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Lotes', 'Conformación de Lotes a partir de Recepciones de Fruta'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Lotes');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Lotes', 'Conformación de Lotes'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Lotes'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Lotes');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Lotes'
  AND p.Nombre = 'Lotes'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
