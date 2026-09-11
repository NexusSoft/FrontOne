using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorResumenCalibre
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
    private GroupHeaderBand _groupHeaderCalibre;
    private DetailBand _detailBand;
    private GroupFooterBand _groupFooterCalibre;
    private ReportFooterBand _reportFooterBandDetalle;
    private BottomMarginBand _bottomMarginBand;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand { HeightF = 39 };
        _bottomMarginBand = new BottomMarginBand { HeightF = 39 };

        _reportHeaderBand = new ReportHeaderBand { HeightF = 190 };
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearMembrete("Resumen de Carga de Contenedor por Calibre"));
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearBloqueEncabezadoContenedor(105));
        _reportHeaderBand.Controls.AddRange(new XRControl[]
        {
            ReporteContenedorComun.CrearColumna("#", 0, 172, 30),
            ReporteContenedorComun.CrearColumna("Identificador", 35, 172, 90),
            ReporteContenedorComun.CrearColumna("Pallet", 130, 172, 70),
            ReporteContenedorComun.CrearColumna("TAG", 205, 172, 85),
            ReporteContenedorComun.CrearColumna("Fecha", 295, 172, 60),
            ReporteContenedorComun.CrearColumna("Calibre", 360, 172, 45),
            ReporteContenedorComun.CrearColumna("°F", 410, 172, 40),
            ReporteContenedorComun.CrearColumna("Cantidad", 455, 172, 55),
            ReporteContenedorComun.CrearColumna("Kilogramos", 515, 172, 80),
            ReporteContenedorComun.CrearColumna("Marca", 600, 172, 170),
        });

        var lblNumero = ReporteContenedorComun.CrearCelda(0, 0, 30);
        lblNumero.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Numero]"));
        var lblIdentificador = ReporteContenedorComun.CrearCelda(35, 0, 90);
        lblIdentificador.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Identificador]"));
        var lblPallet = ReporteContenedorComun.CrearCelda(130, 0, 70);
        lblPallet.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PalletFolio]"));
        var lblTag = ReporteContenedorComun.CrearCelda(205, 0, 85);
        lblTag.Text = "000000000";
        var lblFecha = ReporteContenedorComun.CrearCelda(295, 0, 60);
        lblFecha.TextFormatString = "{0:dd/MM/yyyy}";
        lblFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Fecha]"));
        var lblCalibre = ReporteContenedorComun.CrearCelda(360, 0, 45);
        lblCalibre.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Calibre]"));
        var lblTemperatura = ReporteContenedorComun.CrearCelda(410, 0, 40);
        lblTemperatura.TextFormatString = "{0:N1}";
        lblTemperatura.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Temperatura]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(455, 0, 55, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));
        var lblKilogramos = ReporteContenedorComun.CrearCelda(515, 0, 80, alinearDerecha: true);
        lblKilogramos.TextFormatString = "{0:N2}";
        lblKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));
        var lblMarca = ReporteContenedorComun.CrearCelda(600, 0, 170);
        lblMarca.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Marca]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[]
        {
            lblNumero, lblIdentificador, lblPallet, lblTag, lblFecha, lblCalibre, lblTemperatura, lblCantidad, lblKilogramos, lblMarca,
        });

        var lblEtqCalibre = ReporteContenedorComun.CrearEtiqueta("Calibre:");
        var valCalibre = new XRLabel();
        ReporteContenedorComun.UbicarPar(lblEtqCalibre, valCalibre, 0, 4, 60, 100);
        valCalibre.Font = new DXFont("Arial", 9, DXFontStyle.Bold);
        valCalibre.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Calibre]"));

        _groupHeaderCalibre = new GroupHeaderBand { HeightF = 24 };
        _groupHeaderCalibre.GroupFields.Add(new GroupField("Calibre"));
        _groupHeaderCalibre.Controls.AddRange(new XRControl[] { lblEtqCalibre, valCalibre });

        var lblEtqSubtotal = ReporteContenedorComun.CrearEtiqueta("Subtotal Calibre:");
        lblEtqSubtotal.LocationFloat = new PointFloat(360, 2);
        lblEtqSubtotal.SizeF = new System.Drawing.SizeF(90, 16);
        var lblSubtotalCajas = ReporteContenedorComun.CrearCelda(455, 2, 55, alinearDerecha: true);
        lblSubtotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblSubtotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[][[Calibre] == ^.[Calibre]].Sum([Cajas])"));
        var lblSubtotalKilogramos = ReporteContenedorComun.CrearCelda(515, 2, 80, alinearDerecha: true);
        lblSubtotalKilogramos.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblSubtotalKilogramos.TextFormatString = "{0:N2}";
        lblSubtotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[][[Calibre] == ^.[Calibre]].Sum([Kilogramos])"));

        _groupFooterCalibre = new GroupFooterBand { HeightF = 20 };
        _groupFooterCalibre.Controls.AddRange(new XRControl[] { lblEtqSubtotal, lblSubtotalCajas, lblSubtotalKilogramos });

        var lblEtqTotal = ReporteContenedorComun.CrearEtiqueta("Total General:");
        lblEtqTotal.LocationFloat = new PointFloat(360, 6);
        lblEtqTotal.SizeF = new System.Drawing.SizeF(90, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(455, 6, 55, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Cajas])"));
        var lblTotalKilogramos = ReporteContenedorComun.CrearCelda(515, 6, 80, alinearDerecha: true);
        lblTotalKilogramos.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalKilogramos.TextFormatString = "{0:N2}";
        lblTotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Kilogramos])"));

        _reportFooterBandDetalle = new ReportFooterBand { HeightF = 28 };
        _reportFooterBandDetalle.Controls.AddRange(new XRControl[] { lblEtqTotal, lblTotalCajas, lblTotalKilogramos });

        _detailReportBand = new DetailReportBand();
        _detailReportBand.Bands.AddRange(new Band[] { _groupHeaderCalibre, _detailBand, _groupFooterCalibre, _reportFooterBandDetalle });

        Bands.AddRange(new Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }
}
