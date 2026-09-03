using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteLiquidacionProductor
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
    private DetailBand _detailBandVacio;

    // Cada sección repetitiva es un PAR de DetailReportBand: uno de encabezado (título +
    // columnas, 1 fila fija) y uno de datos — nunca GroupHeaderBand, que exige agrupación real y
    // sin GroupFields DevExpress rechaza el documento entero ("no contiene banda Detail"). Mismo
    // patrón exacto que ReportePallet/ReporteIncidencias.
    private DetailReportBand _detailReportBandCategoriasEncabezado;
    private DetailBand _detailBandCategoriasEncabezado;
    private DetailReportBand _detailReportBandCategorias;
    private DetailBand _detailBandCategorias;
    private DetailReportBand _detailReportBandGastosEncabezado;
    private DetailBand _detailBandGastosEncabezado;
    private DetailReportBand _detailReportBandGastos;
    private DetailBand _detailBandGastos;
    private ReportFooterBand _reportFooterBand;
    private BottomMarginBand _bottomMarginBand;

    // Membrete
    private XRPictureBox _picLogo;
    private XRLabel _lblRazonSocial;
    private XRLabel _lblDomicilio;
    private XRLabel _lblRfc;
    private XRLabel _lblTelefonoCorreo;
    private XRLabel _lblTitulo;
    private XRLine _lineDivisor1;

    // Encabezado del Lote
    private XRLabel _lblEtqLote, _lblLote;
    private XRLabel _lblEtqFecha, _lblFecha;
    private XRLabel _lblEtqProductor, _lblProductor;
    private XRLabel _lblEtqHuerta, _lblHuerta;
    private XRLabel _lblEtqRegistro, _lblRegistro;
    private XRLine _lineDivisor2;

    // Categorías (Exportación + Nacional)
    private XRLabel _lblTituloCategorias;
    private XRLabel _lblColCategoria, _lblColKgSel, _lblColPct, _lblColCostoReal, _lblColImpReal;
    private XRLabel _lblFilaCategoria, _lblFilaKgSel, _lblFilaPct, _lblFilaCostoReal, _lblFilaImpReal;

    // Gastos a Cargo del Productor
    private XRLabel _lblTituloGastos;
    private XRLabel _lblColProveedor, _lblColCantidad, _lblColPUnitario, _lblColImporteGasto;
    private XRLabel _lblFilaProveedor, _lblFilaCantidad, _lblFilaPUnitario, _lblFilaImporteGasto;

    // Totales
    private XRLine _lineDivisor3;
    private XRLabel _lblEtqTotalGastos, _lblTotalGastos;
    private XRLabel _lblEtqImporteAPagar, _lblImporteAPagar;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _reportHeaderBand = new ReportHeaderBand();
        _detailReportBandCategoriasEncabezado = new DetailReportBand();
        _detailBandCategoriasEncabezado = new DetailBand();
        _detailReportBandCategorias = new DetailReportBand();
        _detailBandCategorias = new DetailBand();
        _detailReportBandGastosEncabezado = new DetailReportBand();
        _detailBandGastosEncabezado = new DetailBand();
        _detailReportBandGastos = new DetailReportBand();
        _detailBandGastos = new DetailBand();
        _reportFooterBand = new ReportFooterBand();
        _bottomMarginBand = new BottomMarginBand();

        _picLogo = new XRPictureBox();
        _lblRazonSocial = new XRLabel();
        _lblDomicilio = new XRLabel();
        _lblRfc = new XRLabel();
        _lblTelefonoCorreo = new XRLabel();
        _lblTitulo = new XRLabel();
        _lineDivisor1 = new XRLine();

        _lblEtqLote = CrearEtiqueta("Lote:"); _lblLote = new XRLabel();
        _lblEtqFecha = CrearEtiqueta("Fecha:"); _lblFecha = new XRLabel();
        _lblEtqProductor = CrearEtiqueta("Productor / Beneficiario:"); _lblProductor = new XRLabel();
        _lblEtqHuerta = CrearEtiqueta("Huerta:"); _lblHuerta = new XRLabel();
        _lblEtqRegistro = CrearEtiqueta("Registro Sagarpa:"); _lblRegistro = new XRLabel();
        _lineDivisor2 = new XRLine();

        _lblTituloCategorias = CrearTituloSeccion("Resultados de Fruta (Exportación + Nacional)");
        _lblColCategoria = CrearEncabezadoColumna("Categoría");
        _lblColKgSel = CrearEncabezadoColumna("Kilogramos");
        _lblColPct = CrearEncabezadoColumna("%");
        _lblColCostoReal = CrearEncabezadoColumna("P. Unitario");
        _lblColImpReal = CrearEncabezadoColumna("Importe");
        _lblFilaCategoria = new XRLabel(); _lblFilaKgSel = new XRLabel(); _lblFilaPct = new XRLabel();
        _lblFilaCostoReal = new XRLabel(); _lblFilaImpReal = new XRLabel();

        _lblTituloGastos = CrearTituloSeccion("Gastos a Cargo del Productor");
        _lblColProveedor = CrearEncabezadoColumna("Tipo de Gasto");
        _lblColCantidad = CrearEncabezadoColumna("Cantidad");
        _lblColPUnitario = CrearEncabezadoColumna("P. Unitario");
        _lblColImporteGasto = CrearEncabezadoColumna("Importe");
        _lblFilaProveedor = new XRLabel(); _lblFilaCantidad = new XRLabel(); _lblFilaPUnitario = new XRLabel(); _lblFilaImporteGasto = new XRLabel();

        _lineDivisor3 = new XRLine();
        _lblEtqTotalGastos = CrearEtiqueta("Total de Gastos:"); _lblTotalGastos = new XRLabel();
        _lblEtqImporteAPagar = CrearEtiqueta("Importe a Pagar al Productor:"); _lblImporteAPagar = new XRLabel();

        //
        // Membrete
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
        _lblTitulo.Text = "Reporte de Proceso y Liquidación para Productor";

        _lineDivisor1.LocationFloat = new PointFloat(0, 92);
        _lineDivisor1.SizeF = new System.Drawing.SizeF(772, 6);

        //
        // Encabezado del Lote
        //
        UbicarPar(_lblEtqLote, _lblLote, 0, 105, 90, 150);
        UbicarPar(_lblEtqFecha, _lblFecha, 400, 105, 90, 150);
        UbicarPar(_lblEtqProductor, _lblProductor, 0, 125, 150, 300);
        UbicarPar(_lblEtqHuerta, _lblHuerta, 0, 145, 150, 300);
        UbicarPar(_lblEtqRegistro, _lblRegistro, 0, 165, 150, 300);

        _lineDivisor2.LocationFloat = new PointFloat(0, 190);
        _lineDivisor2.SizeF = new System.Drawing.SizeF(772, 6);

        //
        // Categorías
        //
        _lblTituloCategorias.LocationFloat = new PointFloat(0, 0);
        UbicarColumna(_lblColCategoria, 0, 20, 250);
        UbicarColumna(_lblColKgSel, 250, 20, 100);
        UbicarColumna(_lblColPct, 350, 20, 80);
        UbicarColumna(_lblColCostoReal, 430, 20, 100);
        UbicarColumna(_lblColImpReal, 530, 20, 120);

        UbicarColumna(_lblFilaCategoria, 0, 0, 250);
        _lblFilaCategoria.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[MateriaPrimaNombre]"));

        UbicarColumna(_lblFilaKgSel, 250, 0, 100);
        _lblFilaKgSel.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaKgSel.TextFormatString = "{0:N2}";
        _lblFilaKgSel.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[KilogramosSeleccionados]"));

        UbicarColumna(_lblFilaPct, 350, 0, 80);
        _lblFilaPct.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaPct.TextFormatString = "{0:N2}";
        _lblFilaPct.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Porcentaje]"));

        UbicarColumna(_lblFilaCostoReal, 430, 0, 100);
        _lblFilaCostoReal.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCostoReal.TextFormatString = "{0:N4}";
        _lblFilaCostoReal.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CostoRealUnitario]"));

        UbicarColumna(_lblFilaImpReal, 530, 0, 120);
        _lblFilaImpReal.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImpReal.TextFormatString = "{0:C2}";
        _lblFilaImpReal.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ImporteReal]"));

        //
        // Gastos a Cargo del Productor
        //
        _lblTituloGastos.LocationFloat = new PointFloat(0, 0);
        UbicarColumna(_lblColProveedor, 0, 20, 300);
        UbicarColumna(_lblColCantidad, 300, 20, 100);
        UbicarColumna(_lblColPUnitario, 400, 20, 120);
        UbicarColumna(_lblColImporteGasto, 520, 20, 130);

        UbicarColumna(_lblFilaProveedor, 0, 0, 300);
        _lblFilaProveedor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Proveedor]"));

        UbicarColumna(_lblFilaCantidad, 300, 0, 100);
        _lblFilaCantidad.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCantidad.TextFormatString = "{0:N2}";
        _lblFilaCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cantidad]"));

        UbicarColumna(_lblFilaPUnitario, 400, 0, 120);
        _lblFilaPUnitario.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaPUnitario.TextFormatString = "{0:C2}";
        _lblFilaPUnitario.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PrecioUnitario]"));

        UbicarColumna(_lblFilaImporteGasto, 520, 0, 130);
        _lblFilaImporteGasto.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImporteGasto.TextFormatString = "{0:C2}";
        _lblFilaImporteGasto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Importe]"));

        //
        // Totales
        //
        _lineDivisor3.LocationFloat = new PointFloat(0, 0);
        _lineDivisor3.SizeF = new System.Drawing.SizeF(772, 6);

        _lblEtqTotalGastos.LocationFloat = new PointFloat(400, 12);
        _lblEtqTotalGastos.SizeF = new System.Drawing.SizeF(180, 16);
        _lblTotalGastos.LocationFloat = new PointFloat(580, 12);
        _lblTotalGastos.SizeF = new System.Drawing.SizeF(120, 16);
        _lblTotalGastos.TextAlignment = TextAlignment.MiddleRight;

        _lblEtqImporteAPagar.LocationFloat = new PointFloat(400, 32);
        _lblEtqImporteAPagar.SizeF = new System.Drawing.SizeF(180, 18);
        _lblEtqImporteAPagar.Font = new DXFont("Arial", 10, DXFontStyle.Bold);
        _lblImporteAPagar.LocationFloat = new PointFloat(580, 32);
        _lblImporteAPagar.SizeF = new System.Drawing.SizeF(120, 18);
        _lblImporteAPagar.TextAlignment = TextAlignment.MiddleRight;
        _lblImporteAPagar.Font = new DXFont("Arial", 10, DXFontStyle.Bold);

        //
        // Encabezado (membrete + datos del Lote)
        //
        _reportHeaderBand.HeightF = 200;
        _reportHeaderBand.Controls.AddRange(new XRControl[]
        {
            _picLogo, _lblRazonSocial, _lblDomicilio, _lblRfc, _lblTelefonoCorreo, _lblTitulo, _lineDivisor1,
            _lblEtqLote, _lblLote, _lblEtqFecha, _lblFecha, _lblEtqProductor, _lblProductor,
            _lblEtqHuerta, _lblHuerta, _lblEtqRegistro, _lblRegistro, _lineDivisor2,
        });

        _lblLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.LoteFolio]"));
        _lblFecha.TextFormatString = "{0:dd/MM/yyyy}";
        _lblFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.FechaCorrida]"));
        _lblProductor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.ProductorNombre]"));
        _lblHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.HuertaNombre]"));
        _lblRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.RegistroSagarpa]"));

        _lblRazonSocial.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.RazonSocial]"));
        _lblDomicilio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.Domicilio]"));
        _lblRfc.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Rfc]"));
        _lblTelefonoCorreo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TelefonoCorreo]"));
        _picLogo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "ImageSource", "Iif(IsNullOrEmpty([Empresa.Logo]), Null, [Empresa.Logo])"));

        _lblTotalGastos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TotalGastosProductor]"));
        _lblTotalGastos.TextFormatString = "{0:C2}";
        _lblImporteAPagar.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ImporteAPagarProductor]"));
        _lblImporteAPagar.TextFormatString = "{0:C2}";

        //
        // Cada sección: un DetailReportBand de encabezado (título + columnas, 1 fila fija)
        // seguido de un DetailReportBand de datos — sin GroupHeaderBand (ver comentario junto a
        // los campos arriba).
        //
        _detailBandCategoriasEncabezado.HeightF = 40;
        _detailBandCategoriasEncabezado.Controls.AddRange(new XRControl[] { _lblTituloCategorias, _lblColCategoria, _lblColKgSel, _lblColPct, _lblColCostoReal, _lblColImpReal });
        _detailReportBandCategoriasEncabezado.Bands.Add(_detailBandCategoriasEncabezado);

        _detailBandCategorias.HeightF = 18;
        _detailBandCategorias.Controls.AddRange(new XRControl[] { _lblFilaCategoria, _lblFilaKgSel, _lblFilaPct, _lblFilaCostoReal, _lblFilaImpReal });
        _detailReportBandCategorias.Bands.Add(_detailBandCategorias);

        _detailBandGastosEncabezado.HeightF = 40;
        _detailBandGastosEncabezado.Controls.AddRange(new XRControl[] { _lblTituloGastos, _lblColProveedor, _lblColCantidad, _lblColPUnitario, _lblColImporteGasto });
        _detailReportBandGastosEncabezado.Bands.Add(_detailBandGastosEncabezado);

        _detailBandGastos.HeightF = 18;
        _detailBandGastos.Controls.AddRange(new XRControl[] { _lblFilaProveedor, _lblFilaCantidad, _lblFilaPUnitario, _lblFilaImporteGasto });
        _detailReportBandGastos.Bands.Add(_detailBandGastos);

        _reportFooterBand.HeightF = 60;
        _reportFooterBand.Controls.AddRange(new XRControl[] { _lineDivisor3, _lblEtqTotalGastos, _lblTotalGastos, _lblEtqImporteAPagar, _lblImporteAPagar });

        _topMarginBand.HeightF = 39;
        _bottomMarginBand.HeightF = 39;

        // Un reporte armado por código necesita, además de los DetailReportBand, una banda
        // DetailBand plana de nivel superior (puede ir vacía) — ver comentario junto a los
        // campos arriba.
        _detailBandVacio = new DetailBand { HeightF = 0 };

        Bands.AddRange(new Band[]
        {
            _topMarginBand, _reportHeaderBand, _detailBandVacio,
            _detailReportBandCategoriasEncabezado, _detailReportBandCategorias,
            _detailReportBandGastosEncabezado, _detailReportBandGastos,
            _reportFooterBand, _bottomMarginBand,
        });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }

    private static XRLabel CrearEtiqueta(string texto) => new() { Text = texto, Font = new DXFont("Arial", 9, DXFontStyle.Bold) };

    private static XRLabel CrearEncabezadoColumna(string texto) => new() { Text = texto, Font = new DXFont("Arial", 9, DXFontStyle.Bold), TextAlignment = TextAlignment.MiddleCenter };

    private static XRLabel CrearTituloSeccion(string texto) => new()
    {
        Text = texto,
        Font = new DXFont("Arial", 11, DXFontStyle.Bold),
        SizeF = new System.Drawing.SizeF(772, 18),
    };

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
