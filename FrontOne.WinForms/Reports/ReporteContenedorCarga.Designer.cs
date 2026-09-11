using DevExpress.Utils;
using DevExpress.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteContenedorCarga
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
    private GroupHeaderBand _groupHeaderPallet;
    private DetailBand _detailBand;
    private GroupFooterBand _groupFooterPallet;
    private BottomMarginBand _bottomMarginBand;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand { HeightF = 39 };
        _bottomMarginBand = new BottomMarginBand { HeightF = 39 };

        _reportHeaderBand = new ReportHeaderBand { HeightF = 195 };
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearMembrete("Carga de Contenedor"));
        _reportHeaderBand.Controls.AddRange(ReporteContenedorComun.CrearBloqueEncabezadoContenedor(105));
        _reportHeaderBand.Controls.AddRange(new XRControl[]
        {
            ReporteContenedorComun.CrearColumna("Producto", 0, 172, 200),
            ReporteContenedorComun.CrearColumna("SAGARPA", 205, 172, 110),
            ReporteContenedorComun.CrearColumna("GlobalGAP", 320, 172, 80),
            ReporteContenedorComun.CrearColumna("Presentación", 405, 172, 65),
            ReporteContenedorComun.CrearColumna("Fecha", 475, 172, 65),
            ReporteContenedorComun.CrearColumna("Lote", 545, 172, 65),
            ReporteContenedorComun.CrearColumna("Cantidad", 615, 172, 55),
            ReporteContenedorComun.CrearColumna("Kilogramos", 675, 172, 90),
        });

        // Encabezado de grupo: un renglón por pallet (Posicion). TAG no existe en el esquema —
        // se muestra fijo "000000000" en las 20 muestras de referencia, no se inventa una columna.
        var lblGrupoPallet = new XRLabel { LocationFloat = new PointFloat(0, 4), SizeF = new System.Drawing.SizeF(150, 16), Font = new DXFont("Arial", 9, DXFontStyle.Bold), TextFormatString = "PALLET {0:00}" };
        lblGrupoPallet.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Posicion]"));
        var lblGrupoFolio = new XRLabel { LocationFloat = new PointFloat(155, 4), SizeF = new System.Drawing.SizeF(200, 16), Font = new DXFont("Arial", 9, DXFontStyle.Bold), TextFormatString = "Pallet {0}" };
        lblGrupoFolio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PalletFolio]"));
        var lblGrupoTag = new XRLabel { LocationFloat = new PointFloat(360, 4), SizeF = new System.Drawing.SizeF(200, 16), Font = new DXFont("Arial", 9, DXFontStyle.Bold), Text = "TAG 000000000" };

        _groupHeaderPallet = new GroupHeaderBand { HeightF = 24 };
        _groupHeaderPallet.GroupFields.Add(new GroupField("Posicion"));
        _groupHeaderPallet.Controls.AddRange(new XRControl[] { lblGrupoPallet, lblGrupoFolio, lblGrupoTag });

        var lblProducto = ReporteContenedorComun.CrearCelda(0, 0, 200);
        lblProducto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoDescripcion]"));
        var lblSagarpa = ReporteContenedorComun.CrearCelda(205, 0, 110);
        lblSagarpa.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RegistroSagarpa]"));
        var lblGlobalGap = ReporteContenedorComun.CrearCelda(320, 0, 80);
        lblGlobalGap.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Iif([CertificadoGlobalGap], [NumeroGlobalGap], '')"));
        var lblPresentacion = ReporteContenedorComun.CrearCelda(405, 0, 65);
        lblPresentacion.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoPresentacionTexto]"));
        var lblFecha = ReporteContenedorComun.CrearCelda(475, 0, 65);
        lblFecha.TextFormatString = "{0:dd/MM/yyyy}";
        lblFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LoteFecha]"));
        var lblLote = ReporteContenedorComun.CrearCelda(545, 0, 65);
        lblLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LoteFolio]"));
        var lblCantidad = ReporteContenedorComun.CrearCelda(615, 0, 55, alinearDerecha: true);
        lblCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));
        var lblKilogramos = ReporteContenedorComun.CrearCelda(675, 0, 90, alinearDerecha: true);
        lblKilogramos.TextFormatString = "{0:N2}";
        lblKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        _detailBand = new DetailBand { HeightF = 16 };
        _detailBand.Controls.AddRange(new XRControl[] { lblProducto, lblSagarpa, lblGlobalGap, lblPresentacion, lblFecha, lblLote, lblCantidad, lblKilogramos });

        var lblEtqTotalPallet = ReporteContenedorComun.CrearEtiqueta("Total Pallet:");
        lblEtqTotalPallet.LocationFloat = new PointFloat(405, 2);
        lblEtqTotalPallet.SizeF = new System.Drawing.SizeF(110, 16);
        var lblTotalCajas = ReporteContenedorComun.CrearCelda(615, 2, 55, alinearDerecha: true);
        lblTotalCajas.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[][[Posicion] == ^.[Posicion]].Sum([Cajas])"));
        var lblTotalKilogramos = ReporteContenedorComun.CrearCelda(675, 2, 90, alinearDerecha: true);
        lblTotalKilogramos.Font = new DXFont("Arial", 8, DXFontStyle.Bold);
        lblTotalKilogramos.TextFormatString = "{0:N2}";
        lblTotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[][[Posicion] == ^.[Posicion]].Sum([Kilogramos])"));

        _groupFooterPallet = new GroupFooterBand { HeightF = 22 };
        _groupFooterPallet.Controls.AddRange(new XRControl[] { lblEtqTotalPallet, lblTotalCajas, lblTotalKilogramos });

        _detailReportBand = new DetailReportBand();
        _detailReportBand.Bands.AddRange(new Band[] { _groupHeaderPallet, _detailBand, _groupFooterPallet });

        Bands.AddRange(new Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }
}
