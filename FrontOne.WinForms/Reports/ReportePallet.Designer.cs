using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// Layout default de la "Papeleta de Pallet" — punto de partida antes de que alguien lo edite con
// el Diseñador de Reportes (ver Forms/Sistema/DisenadorReporteForm.cs). El encabezado se llena a
// mano en ReportePallet.CargarDatos; el DetailBand sí usa databinding real contra la lista de
// PalletDetalleDto, porque un pallet puede traer varias líneas.
partial class ReportePallet
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
    private DetailBand _detailBand;
    private ReportFooterBand _reportFooterBand;
    private BottomMarginBand _bottomMarginBand;

    private XRPictureBox _picLogo;
    private XRLabel _lblRazonSocial;
    private XRLabel _lblDomicilio;
    private XRLabel _lblRfc;
    private XRLabel _lblTelefonoCorreo;
    private XRLabel _lblTitulo;
    private XRLine _lineDivisor1;

    private XRLabel _lblEtqNoPallet;
    private XRLabel _lblNoPallet;
    private XRLabel _lblEtqFecha;
    private XRLabel _lblFecha;
    private XRLabel _lblEtqHora;
    private XRLabel _lblHora;
    private XRLabel _lblEtqStatus;
    private XRLabel _lblStatus;
    private XRLabel _lblEtqLineaProduccion;
    private XRLabel _lblLineaProduccion;
    private XRLabel _lblEtqEsMixto;
    private XRLabel _lblEsMixto;
    private XRLabel _lblEtqPorcentajeMateriaSeca;
    private XRLabel _lblPorcentajeMateriaSeca;
    private XRLabel _lblEtqPesoReal;
    private XRLabel _lblPesoReal;
    private XRLabel _lblEtqNoReempaque;
    private XRLabel _lblNoReempaque;

    private XRLine _lineDivisor2;

    private XRLabel _lblColProducto;
    private XRLabel _lblColLote;
    private XRLabel _lblColCajas;
    private XRLabel _lblColKilogramos;
    private XRLabel _lblColMateriaSeca;
    private XRLabel _lblColCajasPorPallet;

    private XRLabel _lblFilaProducto;
    private XRLabel _lblFilaLote;
    private XRLabel _lblFilaCajas;
    private XRLabel _lblFilaKilogramos;
    private XRLabel _lblFilaMateriaSeca;
    private XRLabel _lblFilaCajasPorPallet;

    private XRLine _lineDivisor3;
    private XRLabel _lblEtqTotales;
    private XRLabel _lblTotalCajas;
    private XRLabel _lblTotalKilogramos;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _reportHeaderBand = new ReportHeaderBand();
        _detailBand = new DetailBand();
        _reportFooterBand = new ReportFooterBand();
        _bottomMarginBand = new BottomMarginBand();

        _picLogo = new XRPictureBox();
        _lblRazonSocial = new XRLabel();
        _lblDomicilio = new XRLabel();
        _lblRfc = new XRLabel();
        _lblTelefonoCorreo = new XRLabel();
        _lblTitulo = new XRLabel();
        _lineDivisor1 = new XRLine();

        _lblEtqNoPallet = CrearEtiqueta("No. de Pallet:");
        _lblNoPallet = new XRLabel();
        _lblEtqFecha = CrearEtiqueta("Fecha:");
        _lblFecha = new XRLabel();
        _lblEtqHora = CrearEtiqueta("Hora:");
        _lblHora = new XRLabel();
        _lblEtqStatus = CrearEtiqueta("Status:");
        _lblStatus = new XRLabel();
        _lblEtqLineaProduccion = CrearEtiqueta("Línea de Producción:");
        _lblLineaProduccion = new XRLabel();
        _lblEtqEsMixto = CrearEtiqueta("Es Mixto:");
        _lblEsMixto = new XRLabel();
        _lblEtqPorcentajeMateriaSeca = CrearEtiqueta("% Materia Seca:");
        _lblPorcentajeMateriaSeca = new XRLabel();
        _lblEtqPesoReal = CrearEtiqueta("Peso Real:");
        _lblPesoReal = new XRLabel();
        _lblEtqNoReempaque = CrearEtiqueta("No. de Reempaque:");
        _lblNoReempaque = new XRLabel();

        _lineDivisor2 = new XRLine();

        _lblColProducto = CrearEtiqueta("Producto");
        _lblColLote = CrearEtiqueta("Lote");
        _lblColCajas = CrearEtiqueta("Cajas");
        _lblColKilogramos = CrearEtiqueta("Kilogramos");
        _lblColMateriaSeca = CrearEtiqueta("% M. Seca");
        _lblColCajasPorPallet = CrearEtiqueta("Cajas/Pallet");

        _lblFilaProducto = new XRLabel();
        _lblFilaLote = new XRLabel();
        _lblFilaCajas = new XRLabel();
        _lblFilaKilogramos = new XRLabel();
        _lblFilaMateriaSeca = new XRLabel();
        _lblFilaCajasPorPallet = new XRLabel();

        _lineDivisor3 = new XRLine();
        _lblEtqTotales = CrearEtiqueta("Totales:");
        _lblTotalCajas = new XRLabel();
        _lblTotalKilogramos = new XRLabel();

        //
        // Membrete (logo + datos de la empresa)
        //
        _picLogo.LocationFloat = new PointFloat(0, 0);
        _picLogo.SizeF = new System.Drawing.SizeF(140, 60);
        _picLogo.Sizing = ImageSizeMode.ZoomImage;

        _lblRazonSocial.LocationFloat = new PointFloat(400, 0);
        _lblRazonSocial.SizeF = new System.Drawing.SizeF(372, 16);
        _lblRazonSocial.Font = new DXFont("Arial", 9, DXFontStyle.Bold);

        _lblDomicilio.LocationFloat = new PointFloat(400, 16);
        _lblDomicilio.SizeF = new System.Drawing.SizeF(372, 14);

        _lblRfc.LocationFloat = new PointFloat(400, 30);
        _lblRfc.SizeF = new System.Drawing.SizeF(372, 14);

        _lblTelefonoCorreo.LocationFloat = new PointFloat(400, 44);
        _lblTelefonoCorreo.SizeF = new System.Drawing.SizeF(372, 14);

        _lblTitulo.LocationFloat = new PointFloat(0, 68);
        _lblTitulo.SizeF = new System.Drawing.SizeF(772, 20);
        _lblTitulo.Font = new DXFont("Arial", 13, DXFontStyle.Bold);
        _lblTitulo.TextAlignment = TextAlignment.MiddleCenter;
        _lblTitulo.Text = "Papeleta de Pallet";

        _lineDivisor1.LocationFloat = new PointFloat(0, 92);
        _lineDivisor1.SizeF = new System.Drawing.SizeF(772, 6);

        //
        // Datos del pallet (dos columnas de pares etiqueta/valor)
        //
        UbicarPar(_lblEtqNoPallet, _lblNoPallet, 0, 105, 110, 150);
        UbicarPar(_lblEtqFecha, _lblFecha, 400, 105, 110, 150);
        UbicarPar(_lblEtqHora, _lblHora, 0, 125, 110, 150);
        UbicarPar(_lblEtqStatus, _lblStatus, 400, 125, 110, 150);
        UbicarPar(_lblEtqLineaProduccion, _lblLineaProduccion, 0, 145, 110, 150);
        UbicarPar(_lblEtqEsMixto, _lblEsMixto, 400, 145, 110, 150);
        UbicarPar(_lblEtqPorcentajeMateriaSeca, _lblPorcentajeMateriaSeca, 0, 165, 110, 150);
        UbicarPar(_lblEtqPesoReal, _lblPesoReal, 400, 165, 110, 150);
        UbicarPar(_lblEtqNoReempaque, _lblNoReempaque, 0, 185, 110, 150);

        _lineDivisor2.LocationFloat = new PointFloat(0, 210);
        _lineDivisor2.SizeF = new System.Drawing.SizeF(772, 6);

        //
        // Encabezados de la tabla de detalle
        //
        UbicarColumna(_lblColProducto, 0, 220, 260);
        UbicarColumna(_lblColLote, 265, 220, 90);
        UbicarColumna(_lblColCajas, 360, 220, 70);
        UbicarColumna(_lblColKilogramos, 435, 220, 100);
        UbicarColumna(_lblColMateriaSeca, 540, 220, 90);
        UbicarColumna(_lblColCajasPorPallet, 635, 220, 100);

        //
        // Fila de detalle: una por línea del pallet, enlazada a las propiedades de PalletDetalleDto.
        //
        UbicarColumna(_lblFilaProducto, 0, 0, 260);
        _lblFilaProducto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoDescripcion]"));

        UbicarColumna(_lblFilaLote, 265, 0, 90);
        _lblFilaLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LoteFolio]"));

        UbicarColumna(_lblFilaCajas, 360, 0, 70);
        _lblFilaCajas.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cajas]"));

        UbicarColumna(_lblFilaKilogramos, 435, 0, 100);
        _lblFilaKilogramos.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaKilogramos.TextFormatString = "{0:N2}";
        _lblFilaKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        UbicarColumna(_lblFilaMateriaSeca, 540, 0, 90);
        _lblFilaMateriaSeca.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaMateriaSeca.TextFormatString = "{0:N2}";
        _lblFilaMateriaSeca.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PorcentajeMateriaSeca]"));

        UbicarColumna(_lblFilaCajasPorPallet, 635, 0, 100);
        _lblFilaCajasPorPallet.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCajasPorPallet.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CajasPorPallet]"));

        //
        // Totales
        //
        _lineDivisor3.LocationFloat = new PointFloat(0, 0);
        _lineDivisor3.SizeF = new System.Drawing.SizeF(772, 6);

        _lblEtqTotales.LocationFloat = new PointFloat(265, 10);
        _lblEtqTotales.SizeF = new System.Drawing.SizeF(90, 16);

        _lblTotalCajas.LocationFloat = new PointFloat(360, 10);
        _lblTotalCajas.SizeF = new System.Drawing.SizeF(70, 16);
        _lblTotalCajas.TextAlignment = TextAlignment.MiddleRight;
        _lblTotalCajas.Font = new DXFont("Arial", 9, DXFontStyle.Bold);

        _lblTotalKilogramos.LocationFloat = new PointFloat(435, 10);
        _lblTotalKilogramos.SizeF = new System.Drawing.SizeF(100, 16);
        _lblTotalKilogramos.TextAlignment = TextAlignment.MiddleRight;
        _lblTotalKilogramos.Font = new DXFont("Arial", 9, DXFontStyle.Bold);

        //
        // _topMarginBand / _bottomMarginBand
        //
        _topMarginBand.HeightF = 39;
        _bottomMarginBand.HeightF = 39;

        //
        // _reportHeaderBand
        //
        _reportHeaderBand.HeightF = 240;
        _reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[]
        {
            _picLogo, _lblRazonSocial, _lblDomicilio, _lblRfc, _lblTelefonoCorreo, _lblTitulo, _lineDivisor1,
            _lblEtqNoPallet, _lblNoPallet, _lblEtqFecha, _lblFecha, _lblEtqHora, _lblHora,
            _lblEtqStatus, _lblStatus, _lblEtqLineaProduccion, _lblLineaProduccion,
            _lblEtqEsMixto, _lblEsMixto, _lblEtqPorcentajeMateriaSeca, _lblPorcentajeMateriaSeca,
            _lblEtqPesoReal, _lblPesoReal, _lblEtqNoReempaque, _lblNoReempaque,
            _lineDivisor2,
            _lblColProducto, _lblColLote, _lblColCajas, _lblColKilogramos, _lblColMateriaSeca, _lblColCajasPorPallet,
        });

        //
        // _detailBand
        //
        _detailBand.HeightF = 18;
        _detailBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[]
        {
            _lblFilaProducto, _lblFilaLote, _lblFilaCajas, _lblFilaKilogramos, _lblFilaMateriaSeca, _lblFilaCajasPorPallet,
        });

        //
        // _reportFooterBand
        //
        _reportFooterBand.HeightF = 40;
        _reportFooterBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[]
        {
            _lineDivisor3, _lblEtqTotales, _lblTotalCajas, _lblTotalKilogramos,
        });

        //
        // ReportePallet
        //
        Bands.AddRange(new DevExpress.XtraReports.UI.Band[]
        {
            _topMarginBand, _reportHeaderBand, _detailBand, _reportFooterBand, _bottomMarginBand,
        });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }

    private static XRLabel CrearEtiqueta(string texto) => new() { Text = texto, Font = new DXFont("Arial", 9, DXFontStyle.Bold) };

    private static void UbicarPar(XRLabel etiqueta, XRLabel valor, float x, float y, float anchoEtiqueta, float anchoValor)
    {
        etiqueta.LocationFloat = new PointFloat(x, y);
        etiqueta.SizeF = new System.Drawing.SizeF(anchoEtiqueta, 16);
        valor.LocationFloat = new PointFloat(x + anchoEtiqueta + 5, y);
        valor.SizeF = new System.Drawing.SizeF(anchoValor, 16);
    }

    private static void UbicarColumna(XRLabel etiqueta, float x, float y, float ancho)
    {
        etiqueta.LocationFloat = new PointFloat(x, y);
        etiqueta.SizeF = new System.Drawing.SizeF(ancho, 16);
    }
}
