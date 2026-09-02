USE FrontOne;
GO

-- Módulo Reempaques (nuevo) y su primera pantalla, Reempaques, con permisos completos para el rol
-- Administrador. Mismo patrón exacto que 032_Seed_Modulo_Corridas.sql. El botón _btnReempaques ya
-- vivía en el Ribbon (grupo "Fabricación") sin permiso detrás; este script lo habilita.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Reempaques', 'Desarma pallets ya armados y reconstruye pallets nuevos sin perder trazabilidad de lote'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Reempaques');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Reempaques', 'Reempaques'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Reempaques'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Reempaques');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Reempaques'
  AND p.Nombre = 'Reempaques'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
