using DevExpress.XtraReports.UI;
using FrontOne.Domain.Constants;

namespace FrontOne.WinForms.Reports;

// Un reporte necesita código (una clase XtraReport que sabe qué datos traer y cómo se ve por
// default) — no se puede dar de alta uno nuevo solo desde la app. Este catálogo es la lista de
// reportes ya programados; ReportesForm (Forms/Sistema/) lo recorre para mostrar el listado y
// abrir el Diseñador de cada uno. La lista de Código/Nombre vive en
// FrontOne.Domain.Constants.ReportesDisponibles (Application también la necesita, para armar
// la matriz de permisos de reportes, y no puede depender de WinForms) — agregar un reporte
// nuevo son dos toques: una entrada en ReportesDisponibles y un caso nuevo en el switch de acá.
public record ReporteDisponible(string Codigo, string Nombre, Func<XtraReport> CrearReporteDefault);

public static class CatalogoReportes
{
    public static IReadOnlyList<ReporteDisponible> Todos { get; } =
        ReportesDisponibles.Todos
            .Select(r => new ReporteDisponible(r.Codigo, r.Nombre, ObtenerFactory(r.Codigo)))
            .ToList();

    private static Func<XtraReport> ObtenerFactory(string codigo) => codigo switch
    {
        "RecepcionFruta" => () => new ReporteRecepcionFruta(),
        "PruebaCodigoBarras" => () => new ReportePruebaCodigoBarras(),
        "Pallet" => () => new ReportePallet(),
        _ => throw new InvalidOperationException($"No hay reporte registrado en CatalogoReportes para el código '{codigo}'."),
    };
}
