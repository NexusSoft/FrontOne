using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorResumenHuerta
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
            ReporteContenedorComun.CrearColumna("Huerta", 0, 172, 180),
            ReporteContenedorComun.CrearColumna("No. de Registro", 185, 172, 130),
            ReporteContenedorComun.CrearColumna("Población", 320, 172, 120),
            ReporteContenedorComun.CrearColumna("Municipio", 445, 172, 120),
            ReporteContenedorComun.CrearColumna("Cant.", 570, 172, 70),
            ReporteContenedorComun.CrearColumna("Kilogramos", 645, 172, 90),
        });

        var lblHuerta = ReporteContenedorComun.CrearCelda(0, 0, 180);
        lblHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaNombre]"));
        var lblRegistro = ReporteContenedorComun.CrearCelda(185, 0, 130);
        lblRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RegistroSagarpa]"));
        var lblPoblacion = ReporteContenedorComun.CrearCelda(320, 0, 120);
        lblPoblacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PoblacionNombre]"));
        var lblMunicipio = ReporteContenedorComun.CrearCelda(445, 0, 120);
        lblMunicipio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Municipio]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(570, 0, 70, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));
        var lblKilogramos = ReporteContenedorComun.CrearCelda(645, 0, 90, alinearDerecha: true);
        lblKilogramos.TextFormatString = "{0:N2}";
        lblKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[] { lblHuerta, lblRegistro, lblPoblacion, lblMunicipio, lblCantidad, lblKilogramos });

        var lblEtqTotal = ReporteContenedorComun.CrearEtiqueta("Total General:");
        lblEtqTotal.LocationFloat = new PointFloat(445, 6);
        lblEtqTotal.SizeF = new System.Drawing.SizeF(120, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(570, 6, 70, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Cajas])"));
        var lblTotalKilogramos = ReporteContenedorComun.CrearCelda(645, 6, 90, alinearDerecha: true);
        lblTotalKilogramos.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalKilogramos.TextFormatString = "{0:N2}";
        lblTotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Kilogramos])"));

        _reportFooterBandDetalle = new ReportFooterBand { HeightF = 28 };
        _reportFooterBandDetalle.Controls.AddRange(new XRControl[] { lblEtqTotal, lblTotalCajas, lblTotalKilogramos });

        _detailReportBand = new DetailReportBand();
        _detailReportBand.Bands.AddRange(new Band[] { _detailBand, _reportFooterBandDetalle });

        Bands.AddRange(new Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }
}
