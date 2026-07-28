USE FrontOne;
GO

-- Módulo Recepción (nuevo) y su primera pantalla, Recepciones de Fruta, con permisos completos
-- para el rol Administrador. Mismo patrón exacto que 015_Seed_Modulo_Acarreo_Zona.sql.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Recepcion', 'Recepción de fruta en la empacadora'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Recepcion');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'RecepcionesFruta', 'Recepción de fruta contra Órdenes de Corte'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Recepcion'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'RecepcionesFruta');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Recepcion'
  AND p.Nombre = 'RecepcionesFruta'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO
