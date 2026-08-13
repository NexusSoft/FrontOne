using DevExpress.Drawing.Printing;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// Tamaños de hoja ofrecidos en el selector de VisorReporteForm, compartido por los reportes que
// pasan por ahí (Incidencias, Pallet, Recepción de Fruta). Oficio no existe como valor nombrado en
// DXPaperKind (el más parecido, Folio, difiere ~4mm de alto) — se define a mano con Custom.
public enum TamanoPapelReporte
{
    Carta,
    Legal,
    Tabloide,
    Oficio,
}

public static class TamanoPapelReporteExtensions
{
    public static void AplicarA(this TamanoPapelReporte tamano, XtraReport reporte)
    {
        switch (tamano)
        {
            case TamanoPapelReporte.Carta:
                reporte.PaperKind = DXPaperKind.Letter;
                break;
            case TamanoPapelReporte.Legal:
                reporte.PaperKind = DXPaperKind.Legal;
                break;
            case TamanoPapelReporte.Tabloide:
                reporte.PaperKind = DXPaperKind.Tabloid;
                break;
            case TamanoPapelReporte.Oficio:
                // A diferencia de los tamaños con nombre (Letter/Legal/Tabloid), Custom no se
                // auto-rota con la propiedad Landscape del reporte — PageWidth/PageHeight son el
                // ancho/alto final tal cual. Si el reporte es apaisado hay que invertir a mano.
                reporte.PaperKind = DXPaperKind.Custom;
                if (reporte.Landscape)
                {
                    reporte.PageWidth = 1339;
                    reporte.PageHeight = 850;
                }
                else
                {
                    reporte.PageWidth = 850;
                    reporte.PageHeight = 1339;
                }

                break;
        }
    }

    // Mapeo inverso para preseleccionar el combo con el tamaño que ya trae el reporte al abrirse.
    public static TamanoPapelReporte? DesdeReporte(XtraReport reporte) => reporte.PaperKind switch
    {
        DXPaperKind.Letter => TamanoPapelReporte.Carta,
        DXPaperKind.Legal => TamanoPapelReporte.Legal,
        DXPaperKind.Tabloid => TamanoPapelReporte.Tabloide,
        DXPaperKind.Custom when reporte.PageWidth is 850 or 1339 && reporte.PageHeight is 850 or 1339
            => TamanoPapelReporte.Oficio,
        _ => null,
    };
}
