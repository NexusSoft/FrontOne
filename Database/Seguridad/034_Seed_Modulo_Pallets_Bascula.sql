USE FrontOne;
GO

-- Módulo Pallets (nuevo) con su pantalla Pallets, pantalla ConfiguracionBascula dentro del
-- módulo Seguridad ya existente (mismo criterio que ConfiguracionEmpresa/LicenciaTecit, que
-- también viven en Seguridad por ser configuración del sistema), y el permiso del reporte nuevo
-- de Papeleta de Pallet — los tres con permisos completos para el rol Administrador.
-- Mismo patrón exacto que 032_Seed_Modulo_Corridas.sql y 028_Seed_ReportePermiso_*.sql.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Pallets', 'Armado de pallets con el producto terminado que sale de un lote en proceso'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Pallets');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Pallets', 'Pallets'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Pallets'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Pallets');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Pallets'
  AND p.Nombre = 'Pallets'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'ConfiguracionBascula', 'Configuración de báscula (puerto serie)'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Seguridad'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'ConfiguracionBascula');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Seguridad'
  AND p.Nombre = 'ConfiguracionBascula'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO

INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, 'Pallet', 1, 1, 1, 1
FROM Seguridad.Rol r
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = 'Pallet');
GO
