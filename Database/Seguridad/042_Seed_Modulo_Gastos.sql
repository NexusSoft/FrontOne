USE FrontOne;
GO

-- Módulo Gastos (nuevo): liquidación de costos por Lote (Fruta/Cosecha/Acarreo). 2 pantallas —
-- Gastos (listado + maestro-detalle) y TiposAjuste (catálogo de ajustes de Cosecha/Acarreo).
-- Permisos completos para el rol Administrador, mismo patrón que 041_Seed_Modulo_Etiquetado.sql.

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Gastos', 'Liquidación de costos por Lote (Fruta, Cosecha, Acarreo)'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Gastos');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Gastos', 'Listado de Lotes costeables y captura de Gastos'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Gastos'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Gastos');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'TiposAjuste', 'Catálogo de Tipos de Ajuste de Cosecha/Acarreo'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Gastos'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'TiposAjuste');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Gastos'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO

INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, v.Codigo, 1, 1, 1, 1
FROM Seguridad.Rol r
CROSS JOIN (VALUES ('ProcesoLote'), ('LiquidacionProductor')) AS v(Codigo)
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = v.Codigo);
GO
