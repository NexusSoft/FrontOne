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
        ("PruebaCodigoBarras", "Prueba Código de Barras"),
        ("Pallet", "Papeleta de Pallet"),
    ];
}
