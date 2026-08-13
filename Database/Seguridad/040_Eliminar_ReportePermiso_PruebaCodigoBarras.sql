USE FrontOne;
GO

-- ReportePruebaCodigoBarras se quitó del código (era el reporte piloto sin CargarDatos/pantalla
-- real detrás, "base para las futuras pantallas de etiquetas", ya no hace falta) — estas filas
-- quedan huérfanas (ReportePermiso.ReporteCodigo no lleva FK a ningún catálogo, el código ya no
-- existe en FrontOne.Domain.Constants.ReportesDisponibles).
DELETE FROM Seguridad.ReportePermiso WHERE ReporteCodigo = 'PruebaCodigoBarras';
GO
