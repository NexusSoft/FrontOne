USE FrontOne;
GO

-- Pantallas Incidencias y SupervisoresHuerta del módulo Acopio, y el permiso del reporte nuevo
-- "Incidencias" — los tres con permisos completos para el rol Administrador. Mismo patrón exacto
-- que 034_Seed_Modulo_Pallets_Bascula.sql.

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, v.Nombre, v.Descripcion
FROM Seguridad.Modulo m
CROSS JOIN (VALUES
    ('Incidencias', 'Captura de campo por Orden de Corte (llegada a huerta, cajas cosechadas, incidencias, ajustes)'),
    ('SupervisoresHuerta', 'Catálogo de supervisores de huerta')
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
  AND p.Nombre IN ('Incidencias', 'SupervisoresHuerta')
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO

INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, 'Incidencias', 1, 1, 1, 1
FROM Seguridad.Rol r
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = 'Incidencias');
GO
