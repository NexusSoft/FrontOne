USE FrontOne;
GO

-- Antes de este cambio, Administrador ya podía previsualizar/diseñar cualquier reporte vía el
-- permiso genérico "DisenadorReportes" (retirado del código). Para no romper en frío lo que ya
-- funcionaba, se le da a Administrador las 4 acciones sobre el único reporte que existe hoy
-- (RecepcionFruta). Reportes nuevos que se agreguen después no quedan pre-otorgados — hay que
-- concederlos desde la pantalla "Permisos de Reportes".

INSERT INTO Seguridad.ReportePermiso (RolId, ReporteCodigo, VistaPrevia, Impresion, Exportacion, Diseno)
SELECT r.Id, 'RecepcionFruta', 1, 1, 1, 1
FROM Seguridad.Rol r
WHERE r.Nombre = 'Administrador'
  AND NOT EXISTS (
      SELECT 1 FROM Seguridad.ReportePermiso rp
      WHERE rp.RolId = r.Id AND rp.ReporteCodigo = 'RecepcionFruta');
GO
