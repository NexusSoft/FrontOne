using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorDetallePorHuerta
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
    private GroupHeaderBand _groupHeaderHuerta;
    private DetailBand _detailBand;
    private GroupFooterBand _groupFooterHuerta;
    private BottomMarginBand _bottomMarginBand;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand { HeightF = 39 };
        _bottomMarginBand = new BottomMarginBand { HeightF = 39 };

        _reportHeaderBand = new ReportHeaderBand { HeightF = 170 };
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearMembrete("Detalle de Carga de Contenedor por Huerta"));
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearBloqueEncabezadoContenedor(105));

        var etqHuerta = ReporteContenedorComun.CrearEtiqueta("Huerta:");
        var valHuerta = new XRLabel();
        var etqRegistro = ReporteContenedorComun.CrearEtiqueta("No. de Registro:");
        var valRegistro = new XRLabel();
        var etqPoblacion = ReporteContenedorComun.CrearEtiqueta("Población:");
        var valPoblacion = new XRLabel();
        var etqMunicipio = ReporteContenedorComun.CrearEtiqueta("Municipio:");
        var valMunicipio = new XRLabel();
        var etqProductor = ReporteContenedorComun.CrearEtiqueta("Productor:");
        var valProductor = new XRLabel();

        ReporteContenedorComun.UbicarPar(etqHuerta, valHuerta, 0, 4, 100, 250);
        ReporteContenedorComun.UbicarPar(etqRegistro, valRegistro, 400, 4, 100, 250);
        ReporteContenedorComun.UbicarPar(etqPoblacion, valPoblacion, 0, 22, 100, 150);
        ReporteContenedorComun.UbicarPar(etqMunicipio, valMunicipio, 400, 22, 100, 150);
        ReporteContenedorComun.UbicarPar(etqProductor, valProductor, 0, 40, 100, 400);

        valHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaNombre]"));
        valRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RegistroSagarpa]"));
        valPoblacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PoblacionNombre]"));
        valMunicipio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Municipio]"));
        valProductor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductorNombre]"));

        var colLote = ReporteContenedorComun.CrearColumna("Lote", 0, 62, 90);
        var colPallet = ReporteContenedorComun.CrearColumna("Pallet", 95, 62, 90);
        var colProducto = ReporteContenedorComun.CrearColumna("Producto", 190, 62, 300);
        var colCantidad = ReporteContenedorComun.CrearColumna("Cant.", 495, 62, 70);
        var colKilogramos = ReporteContenedorComun.CrearColumna("Kilogramos", 570, 62, 90);

        _groupHeaderHuerta = new GroupHeaderBand { HeightF = 80 };
        _groupHeaderHuerta.GroupFields.Add(new GroupField("HuertaNombre"));
        _groupHeaderHuerta.Controls.AddRange(new XRControl[]
        {
            etqHuerta, valHuerta, etqRegistro, valRegistro, etqPoblacion, valPoblacion,
            etqMunicipio, valMunicipio, etqProductor, valProductor,
            colLote, colPallet, colProducto, colCantidad, colKilogramos,
        });

        var lblLote = ReporteContenedorComun.CrearCelda(0, 0, 90);
        lblLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LoteFolio]"));
        var lblPallet = ReporteContenedorComun.CrearCelda(95, 0, 90);
        lblPallet.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PalletFolio]"));
        var lblProducto = ReporteContenedorComun.CrearCelda(190, 0, 300);
        lblProducto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoDescripcion]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(495, 0, 70, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));
        var lblKilogramos = ReporteContenedorComun.CrearCelda(570, 0, 90, alinearDerecha: true);
        lblKilogramos.TextFormatString = "{0:N2}";
        lblKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[] { lblLote, lblPallet, lblProducto, lblCantidad, lblKilogramos });

        var lblEtqTotal = ReporteContenedorComun.CrearEtiqueta("Total:");
        lblEtqTotal.LocationFloat = new PointFloat(400, 2);
        lblEtqTotal.SizeF = new System.Drawing.SizeF(90, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(495, 2, 70, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Cajas])"));
        var lblTotalKilogramos = ReporteContenedorComun.CrearCelda(570, 2, 90, alinearDerecha: true);
        lblTotalKilogramos.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalKilogramos.TextFormatString = "{0:N2}";
        lblTotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Kilogramos])"));

        _groupFooterHuerta = new GroupFooterBand { HeightF = 24 };
        _groupFooterHuerta.Controls.AddRange(new XRControl[] { lblEtqTotal, lblTotalCajas, lblTotalKilogramos });

        _detailReportBand = new DetailReportBand();
        _detailReportBand.Bands.AddRange(new Band[] { _groupHeaderHuerta, _detailBand, _groupFooterHuerta });

        Bands.AddRange(new Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }
}
