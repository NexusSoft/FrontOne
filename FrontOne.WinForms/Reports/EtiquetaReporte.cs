using System.IO;
using System.Text;
using DevExpress.Drawing.Printing;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Reports;

// Arma el XtraReport "en blanco" de una plantilla de Etiquetado: tamaño de página = Ancho/Alto en
// pulgadas de la etiqueta (regla dura: margen fijo de 5 mm en los 4 lados, no editable), y carga
// el DefinicionXml guardado si ya existe (si no, el reporte queda en blanco para que el usuario lo
// diseñe desde cero por primera vez). A diferencia de ReportePallet/ReporteIncidencias/
// ReporteRecepcionFruta, una Etiqueta no tiene una clase C# propia con Designer.cs — el layout es
// 100% definido por el usuario en el Diseñador, partiendo de este lienzo en blanco.
public static class EtiquetaReporte
{
    private const decimal MargenMilimetros = 5m;
    private const decimal CentesimasPulgadaPorMilimetro = 100m / 25.4m;

    public static XtraReport Crear(EtiquetaDto etiqueta)
    {
        var margen = (int)Math.Round(MargenMilimetros * CentesimasPulgadaPorMilimetro);

        var reporte = new XtraReport
        {
            PaperKind = DXPaperKind.Custom,
            PageWidth = (int)Math.Round(etiqueta.AnchoPulgadas * 100),
            PageHeight = (int)Math.Round(etiqueta.AltoPulgadas * 100),
            Margins = new System.Drawing.Printing.Margins(margen, margen, margen, margen),
        };

        if (!string.IsNullOrWhiteSpace(etiqueta.DefinicionXml))
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(etiqueta.DefinicionXml));
            reporte.LoadLayoutFromXml(stream);
        }

        return reporte;
    }
}
