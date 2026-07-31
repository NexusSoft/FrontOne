using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// Un reporte necesita código (una clase XtraReport que sabe qué datos traer y cómo se ve por
// default) — no se puede dar de alta uno nuevo solo desde la app. Este catálogo es la lista de
// reportes ya programados; ReportesForm (Forms/Sistema/) lo recorre para mostrar el listado y
// abrir el Diseñador de cada uno. Agregar un reporte nuevo al proyecto = agregar una entrada
// aquí, nada más. Pantalla identifica a qué pantalla del sistema pertenece (mismo nombre que la
// pantalla sembrada en Seguridad, ej. "RecepcionesFruta") — cuando una pantalla tiene más de un
// reporte, Vista Previa muestra un selector (ver SeleccionarReporteForm).
public record ReporteDisponible(string Codigo, string Nombre, string Pantalla, Func<XtraReport> CrearReporteDefault);

public static class CatalogoReportes
{
    public static IReadOnlyList<ReporteDisponible> Todos { get; } =
    [
        new ReporteDisponible("RecepcionFruta", "Recepción de Fruta", "RecepcionesFruta", () => new ReporteRecepcionFruta()),
    ];

    public static IReadOnlyList<ReporteDisponible> ObtenerPorPantalla(string pantalla)
        => Todos.Where(r => r.Pantalla == pantalla).ToList();
}
