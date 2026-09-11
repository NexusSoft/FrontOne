USE FrontOne;
GO

-- Pantalla Contenedores dentro del módulo Embarques (ya creado por 050_Seed_Modulo_Embarques.sql).
-- Permisos completos al rol Administrador, mismo patrón que 050.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Contenedores', 'Surtido de pedidos de venta (SAP) con pallets físicos para embarque'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Embarques'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Contenedores');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Embarques'
  AND p.Nombre = 'Contenedores'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
