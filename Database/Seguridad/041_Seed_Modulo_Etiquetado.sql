USE FrontOne;
GO

-- Módulo Etiquetado (nuevo) con su pantalla Etiquetas, más la Accion nueva "Recuperar" (para
-- restaurar una etiqueta eliminada, ver Etiquetado.Etiqueta.Activo — soft-delete). Permisos
-- completos para el rol Administrador (mismo rol que usa el usuario manager, ver 003_Seed_Manager.sql)
-- — así "solo manager puede recuperar" se resuelve por rol, sin hardcodear el username, mismo
-- criterio que el resto del sistema de permisos. Mismo patrón exacto que
-- 034_Seed_Modulo_Pallets_Bascula.sql.

INSERT INTO Seguridad.Accion (Nombre)
SELECT 'Recuperar'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Accion WHERE Nombre = 'Recuperar');
GO

INSERT INTO Seguridad.Modulo (Nombre, Descripcion)
SELECT 'Etiquetado', 'Plantillas de etiqueta de trazabilidad para Pallets (Caja, Pallet, Registro Sagarpa)'
WHERE NOT EXISTS (SELECT 1 FROM Seguridad.Modulo WHERE Nombre = 'Etiquetado');
GO

INSERT INTO Seguridad.Pantalla (ModuloId, Nombre, Descripcion)
SELECT m.Id, 'Etiquetas', 'Catálogo de plantillas de etiqueta'
FROM Seguridad.Modulo m
WHERE m.Nombre = 'Etiquetado'
  AND NOT EXISTS (SELECT 1 FROM Seguridad.Pantalla p WHERE p.ModuloId = m.Id AND p.Nombre = 'Etiquetas');
GO

INSERT INTO Seguridad.Permiso (RolId, PantallaId, AccionId)
SELECT r.Id, p.Id, a.Id
FROM Seguridad.Rol r
CROSS JOIN Seguridad.Pantalla p
INNER JOIN Seguridad.Modulo m ON m.Id = p.ModuloId
CROSS JOIN Seguridad.Accion a
WHERE r.Nombre = 'Administrador'
  AND m.Nombre = 'Etiquetado'
  AND p.Nombre = 'Etiquetas'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.Permiso pe
      WHERE pe.RolId = r.Id AND pe.PantallaId = p.Id AND pe.AccionId = a.Id);
GO

-- VisorReporteForm (vista previa/imprimir/exportar de una etiqueta) reutiliza el mecanismo de
-- Seguridad.ReportePermiso ya existente para reportes de Código fijo — cada Tipo de etiqueta se
-- trata como un "reporte" con su propio código pseudo-fijo para efectos de este permiso (la
-- plantilla en sí es dinámica, pero el TIPO de etiqueta no).
INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, v.Codigo, 1, 1, 1, 1
FROM Seguridad.Rol r
CROSS JOIN (VALUES ('EtiquetaCaja'), ('EtiquetaPallet'), ('EtiquetaSagarpa')) AS v(Codigo)
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = v.Codigo);
GO
