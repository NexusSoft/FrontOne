using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorResumenHuertaSinKg
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private TopMarginBand _topMarginBand;
    private ReportHeaderBand _reportHeaderBand;
    private DetailReportBand _detailReportBand;
    private DetailBand _detailBand;
    private ReportFooterBand _reportFooterBandDetalle;
    private BottomMarginBand _bottomMarginBand;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand { HeightF = 39 };
        _bottomMarginBand = new BottomMarginBand { HeightF = 39 };

        _reportHeaderBand = new ReportHeaderBand { HeightF = 190 };
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearMembrete("Resumen de Carga de Contenedor por Huerta"));
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearBloqueEncabezadoContenedor(105));
        _reportHeaderBand.Controls.AddRange(new XRControl[]
        {
            ReporteContenedorComun.CrearColumna("Huerta", 0, 172, 220),
            ReporteContenedorComun.CrearColumna("No. de Registro", 225, 172, 150),
            ReporteContenedorComun.CrearColumna("Población", 380, 172, 140),
            ReporteContenedorComun.CrearColumna("Municipio", 525, 172, 140),
            ReporteContenedorComun.CrearColumna("Cant.", 670, 172, 80),
        });

        var lblHuerta = ReporteContenedorComun.CrearCelda(0, 0, 220);
        lblHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaNombre]"));
        var lblRegistro = ReporteContenedorComun.CrearCelda(225, 0, 150);
        lblRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RegistroSagarpa]"));
        var lblPoblacion = ReporteContenedorComun.CrearCelda(380, 0, 140);
        lblPoblacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PoblacionNombre]"));
        var lblMunicipio = ReporteContenedorComun.CrearCelda(525, 0, 140);
        lblMunicipio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Municipio]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(670, 0, 80, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[] { lblHuerta, lblRegistro, lblPoblacion, lblMunicipio, lblCantidad });

        var lblEtqTotal = ReporteContenedorComun.CrearEtiqueta("Total General:");
        lblEtqTotal.LocationFloat = new PointFloat(525, 6);
        lblEtqTotal.SizeF = new System.Drawing.SizeF(140, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(670, 6, 80, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Cajas])"));

        _reportFooterBandDetalle = new ReportFooterBand { HeightF = 28 };
        _reportFooterBandDetalle.Controls.AddRange(new XRControl[] { lblEtqTotal, lblTotalCajas });

        _detailReportBand = new DetailReportBand();
        _detailReportBand.Bands.AddRange(new Band[] { _detailBand, _reportFooterBandDetalle });

        Bands.AddRange(new Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }
}
