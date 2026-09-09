namespace FrontOne.Domain.Constants;

// Lista de reportes que existen en el proyecto (Código + Nombre) — la fábrica de cada
// XtraReport concreto vive en FrontOne.WinForms.Reports.CatalogoReportes, que no puede
// referenciarse desde Application (regla dura: Application no depende de WinForms). Agregar
// un reporte nuevo requiere una entrada aquí Y un caso nuevo en el switch de CatalogoReportes.
public static class ReportesDisponibles
{
    public static IReadOnlyList<(string Codigo, string Nombre)> Todos { get; } =
    [
        ("RecepcionFruta", "Recepción de Fruta"),
        ("ValeRecepcion", "Vale de Recepción"),
        ("Pallet", "Papeleta de Pallet"),
        ("Incidencias", "Incidencias de Corte"),
        ("ProcesoLote", "Reporte de Proceso"),
        ("LiquidacionProductor", "Reporte de Proceso y Liquidación para Productor"),
        ("ContenedorCarga", "Carga de Contenedor"),
        ("ContenedorDetalleLote", "Detalle de Carga de Contenedor por Lote"),
        ("ContenedorDetalleHuerta", "Detalle de Carga de Contenedor por Huerta"),
        ("ContenedorResumenCalibre", "Resumen de Carga de Contenedor por Calibre"),
        ("ContenedorResumenLote", "Resumen de Carga de Contenedor por Lote"),
        ("ContenedorResumenHuerta", "Resumen de Carga de Contenedor por Huerta"),
        ("ContenedorResumenHuertaSinKg", "Resumen de Carga de Contenedor por Huerta (sin Kg)"),
    ];
}
