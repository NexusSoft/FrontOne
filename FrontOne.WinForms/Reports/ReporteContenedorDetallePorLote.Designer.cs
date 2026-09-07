using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorDetallePorLote
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
    private GroupHeaderBand _groupHeaderLote;
    private DetailBand _detailBand;
    private GroupFooterBand _groupFooterLote;
    private BottomMarginBand _bottomMarginBand;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand { HeightF = 39 };
        _bottomMarginBand = new BottomMarginBand { HeightF = 39 };

        _reportHeaderBand = new ReportHeaderBand { HeightF = 170 };
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearMembrete("Detalle de Carga de Contenedor por Lote"));
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearBloqueEncabezadoContenedor(105));

        // Encabezado de grupo: datos del Lote/Huerta, dos columnas de pares etiqueta/valor.
        var etqLote = ReporteContenedorComun.CrearEtiqueta("Lote:");
        var valLote = new XRLabel();
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

        ReporteContenedorComun.UbicarPar(etqLote, valLote, 0, 4, 100, 150);
        ReporteContenedorComun.UbicarPar(etqHuerta, valHuerta, 400, 4, 100, 250);
        ReporteContenedorComun.UbicarPar(etqRegistro, valRegistro, 0, 22, 100, 150);
        ReporteContenedorComun.UbicarPar(etqPoblacion, valPoblacion, 400, 22, 100, 250);
        ReporteContenedorComun.UbicarPar(etqMunicipio, valMunicipio, 0, 40, 100, 150);
        ReporteContenedorComun.UbicarPar(etqProductor, valProductor, 400, 40, 100, 250);

        valLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LoteFolio]"));
        valHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaNombre]"));
        valRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RegistroSagarpa]"));
        valPoblacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PoblacionNombre]"));
        valMunicipio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Municipio]"));
        valProductor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductorNombre]"));

        var colPallet = ReporteContenedorComun.CrearColumna("Pallet", 0, 62, 100);
        var colProducto = ReporteContenedorComun.CrearColumna("Producto", 105, 62, 300);
        var colPresentacion = ReporteContenedorComun.CrearColumna("Presentación", 410, 62, 90);
        var colCantidad = ReporteContenedorComun.CrearColumna("Cant.", 505, 62, 70);
        var colKilogramos = ReporteContenedorComun.CrearColumna("Kilogramos", 580, 62, 90);

        _groupHeaderLote = new GroupHeaderBand { HeightF = 80 };
        _groupHeaderLote.GroupFields.Add(new GroupField("LoteFolio"));
        _groupHeaderLote.Controls.AddRange(new XRControl[]
        {
            etqLote, valLote, etqHuerta, valHuerta, etqRegistro, valRegistro,
            etqPoblacion, valPoblacion, etqMunicipio, valMunicipio, etqProductor, valProductor,
            colPallet, colProducto, colPresentacion, colCantidad, colKilogramos,
        });

        var lblPallet = ReporteContenedorComun.CrearCelda(0, 0, 100);
        lblPallet.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PalletFolio]"));
        var lblProducto = ReporteContenedorComun.CrearCelda(105, 0, 300);
        lblProducto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoDescripcion]"));
        var lblPresentacion = ReporteContenedorComun.CrearCelda(410, 0, 90);
        lblPresentacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoPresentacionTexto]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(505, 0, 70, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));
        var lblKilogramos = ReporteContenedorComun.CrearCelda(580, 0, 90, alinearDerecha: true);
        lblKilogramos.TextFormatString = "{0:N2}";
        lblKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[] { lblPallet, lblProducto, lblPresentacion, lblCantidad, lblKilogramos });

        var lblEtqTotal = ReporteContenedorComun.CrearEtiqueta("Total:");
        lblEtqTotal.LocationFloat = new PointFloat(410, 2);
        lblEtqTotal.SizeF = new System.Drawing.SizeF(90, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(505, 2, 70, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Cajas])"));
        var lblTotalKilogramos = ReporteContenedorComun.CrearCelda(580, 2, 90, alinearDerecha: true);
        lblTotalKilogramos.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalKilogramos.TextFormatString = "{0:N2}";
        lblTotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Sum([Kilogramos])"));

        _groupFooterLote = new GroupFooterBand { HeightF = 24 };
        _groupFooterLote.Controls.AddRange(new XRControl[] { lblEtqTotal, lblTotalCajas, lblTotalKilogramos });

        _detailReportBand = new DetailReportBand();
        _detailReportBand.Bands.AddRange(new Band[] { _groupHeaderLote, _detailBand, _groupFooterLote });

        Bands.AddRange(new Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }
}
