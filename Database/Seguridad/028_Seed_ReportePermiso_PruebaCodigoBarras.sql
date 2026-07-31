USE FrontOne;
GO

-- Reporte piloto nuevo (PruebaCodigoBarras) — se le dan las 4 acciones a Administrador, mismo
-- criterio que el seed original de RecepcionFruta (026), para que quede usable de inmediato.

INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, 'PruebaCodigoBarras', 1, 1, 1, 1
FROM Seguridad.Rol r
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = 'PruebaCodigoBarras');
GO
