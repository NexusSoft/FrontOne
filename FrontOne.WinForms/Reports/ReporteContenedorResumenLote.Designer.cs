using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorResumenLote
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
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearMembrete("Resumen de Carga de Contenedor por Lote"));
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearBloqueEncabezadoContenedor(105));
        _reportHeaderBand.Controls.AddRange(new XRControl[]
        {
            ReporteContenedorComun.CrearColumna("Lote", 0, 172, 90),
            ReporteContenedorComun.CrearColumna("Huerta", 95, 172, 150),
            ReporteContenedorComun.CrearColumna("No. de Registro", 250, 172, 120),
            ReporteContenedorComun.CrearColumna("Población", 375, 172, 110),
            ReporteContenedorComun.CrearColumna("Municipio", 490, 172, 110),
            ReporteContenedorComun.CrearColumna("Cant.", 605, 172, 60),
            ReporteContenedorComun.CrearColumna("Kilogramos", 670, 172, 90),
        });

        var lblLote = ReporteContenedorComun.CrearCelda(0, 0, 90);
        lblLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LoteFolio]"));
        var lblHuerta = ReporteContenedorComun.CrearCelda(95, 0, 150);
        lblHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaNombre]"));
        var lblRegistro = ReporteContenedorComun.CrearCelda(250, 0, 120);
        lblRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RegistroSagarpa]"));
        var lblPoblacion = ReporteContenedorComun.CrearCelda(375, 0, 110);
        lblPoblacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PoblacionNombre]"));
        var lblMunicipio = ReporteContenedorComun.CrearCelda(490, 0, 110);
        lblMunicipio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Municipio]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(605, 0, 60, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));
        var lblKilogramos = ReporteContenedorComun.CrearCelda(670, 0, 90, alinearDerecha: true);
        lblKilogramos.TextFormatString = "{0:N2}";
        lblKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[] { lblLote, lblHuerta, lblRegistro, lblPoblacion, lblMunicipio, lblCantidad, lblKilogramos });

        var lblEtqTotal = ReporteContenedorComun.CrearEtiqueta("Total General:");
        lblEtqTotal.LocationFloat = new PointFloat(490, 6);
        lblEtqTotal.SizeF = new System.Drawing.SizeF(110, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(605, 6, 60, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Cajas])"));
        var lblTotalKilogramos = ReporteContenedorComun.CrearCelda(670, 6, 90, alinearDerecha: true);
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
