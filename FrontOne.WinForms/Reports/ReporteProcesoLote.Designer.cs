using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

partial class ReporteProcesoLote
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

    // Cada sección repetitiva es un PAR de DetailReportBand: uno de encabezado (título + columnas,
    // 1 fila fija) y uno de datos — nunca GroupHeaderBand, que exige agrupación real y con Kg/CXP/CAP
    // sin GroupFields DevExpress rechaza el documento entero ("no contiene banda Detail"). Mismo
    // patrón exacto que ReportePallet/ReporteIncidencias (DetailReportBand con un DetailBand
    // anidado), solo que aquí se usan 2 pares por sección en vez de 1.
    private DetailReportBand _detailReportBandCategoriasEncabezado;
    private DetailBand _detailBandCategoriasEncabezado;
    private DetailReportBand _detailReportBandCategorias;
    private DetailBand _detailBandCategorias;
    private DetailReportBand _detailReportBandMercadoEncabezado;
    private DetailBand _detailBandMercadoEncabezado;
    private DetailReportBand _detailReportBandMercado;
    private DetailBand _detailBandMercado;
    private DetailReportBand _detailReportBandGastosEncabezado;
    private DetailBand _detailBandGastosEncabezado;
    private DetailReportBand _detailReportBandGastos;
    private DetailBand _detailBandGastos;
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
    private XRLabel _lblEtqTipoCorte, _lblTipoCorte;
    private XRLabel _lblEtqPeso, _lblPeso;
    private XRLine _lineDivisor2;

    // Categorías
    private XRLabel _lblTituloCategorias;
    private XRLabel _lblColCategoria, _lblColKgSel, _lblColPct, _lblColKgComp, _lblColCostoReal, _lblColImpReal, _lblColCostoEst, _lblColImpEst;
    private XRLabel _lblFilaCategoria, _lblFilaKgSel, _lblFilaPct, _lblFilaKgComp, _lblFilaCostoReal, _lblFilaImpReal, _lblFilaCostoEst, _lblFilaImpEst;

    // Resumen por Mercado
    private XRLabel _lblTituloMercado;
    private XRLabel _lblColMercado, _lblColKgMercado, _lblColPctMercado, _lblColImpRealMercado, _lblColImpEstMercado;
    private XRLabel _lblFilaMercado, _lblFilaKgMercado, _lblFilaPctMercado, _lblFilaImpRealMercado, _lblFilaImpEstMercado;

    // Relación de Gastos
    private XRLabel _lblTituloGastos;
    private XRLabel _lblColTipoGasto, _lblColProveedor, _lblColCantidad, _lblColPUnitario, _lblColImporteGasto, _lblColCxp, _lblColCap;
    private XRLabel _lblFilaTipoGasto, _lblFilaProveedor, _lblFilaCantidad, _lblFilaPUnitario, _lblFilaImporteGasto;
    private XRCheckBox _chkCxp, _chkCap;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _reportHeaderBand = new ReportHeaderBand();
        _detailReportBandCategoriasEncabezado = new DetailReportBand();
        _detailBandCategoriasEncabezado = new DetailBand();
        _detailReportBandCategorias = new DetailReportBand();
        _detailBandCategorias = new DetailBand();
        _detailReportBandMercadoEncabezado = new DetailReportBand();
        _detailBandMercadoEncabezado = new DetailBand();
        _detailReportBandMercado = new DetailReportBand();
        _detailBandMercado = new DetailBand();
        _detailReportBandGastosEncabezado = new DetailReportBand();
        _detailBandGastosEncabezado = new DetailBand();
        _detailReportBandGastos = new DetailReportBand();
        _detailBandGastos = new DetailBand();
        _bottomMarginBand = new BottomMarginBand();

        _picLogo = new XRPictureBox();
        _lblRazonSocial = new XRLabel();
        _lblDomicilio = new XRLabel();
        _lblRfc = new XRLabel();
        _lblTelefonoCorreo = new XRLabel();
        _lblTitulo = new XRLabel();
        _lineDivisor1 = new XRLine();

        _lblEtqLote = CrearEtiqueta("No. de Lote:"); _lblLote = new XRLabel();
        _lblEtqFecha = CrearEtiqueta("Fecha:"); _lblFecha = new XRLabel();
        _lblEtqProductor = CrearEtiqueta("Productor:"); _lblProductor = new XRLabel();
        _lblEtqHuerta = CrearEtiqueta("Huerta:"); _lblHuerta = new XRLabel();
        _lblEtqRegistro = CrearEtiqueta("Registro:"); _lblRegistro = new XRLabel();
        _lblEtqTipoCorte = CrearEtiqueta("Tipo de Corte:"); _lblTipoCorte = new XRLabel();
        _lblEtqPeso = CrearEtiqueta("Peso Neto:"); _lblPeso = new XRLabel();
        _lineDivisor2 = new XRLine();

        _lblTituloCategorias = CrearTituloSeccion("Reporte del Proceso");
        _lblColCategoria = CrearEtiqueta("Categoría");
        _lblColKgSel = CrearEtiqueta("Kilogramos");
        _lblColPct = CrearEtiqueta("%");
        _lblColKgComp = CrearEtiqueta("Kg Comprados");
        _lblColCostoReal = CrearEtiqueta("Costo a Pagar");
        _lblColImpReal = CrearEtiqueta("Importe");
        _lblColCostoEst = CrearEtiqueta("Costo Estimado");
        _lblColImpEst = CrearEtiqueta("Importe Est.");
        _lblFilaCategoria = new XRLabel(); _lblFilaKgSel = new XRLabel(); _lblFilaPct = new XRLabel(); _lblFilaKgComp = new XRLabel();
        _lblFilaCostoReal = new XRLabel(); _lblFilaImpReal = new XRLabel(); _lblFilaCostoEst = new XRLabel(); _lblFilaImpEst = new XRLabel();

        _lblTituloMercado = CrearTituloSeccion("Resumen por Mercado");
        _lblColMercado = CrearEtiqueta("Tipo"); _lblColKgMercado = CrearEtiqueta("Kilogramos"); _lblColPctMercado = CrearEtiqueta("Porcentaje");
        _lblColImpRealMercado = CrearEtiqueta("Importe"); _lblColImpEstMercado = CrearEtiqueta("Importe Estimado");
        _lblFilaMercado = new XRLabel(); _lblFilaKgMercado = new XRLabel(); _lblFilaPctMercado = new XRLabel();
        _lblFilaImpRealMercado = new XRLabel(); _lblFilaImpEstMercado = new XRLabel();

        _lblTituloGastos = CrearTituloSeccion("Relación de Gastos");
        _lblColTipoGasto = CrearEtiqueta("Tipo de Gasto"); _lblColProveedor = CrearEtiqueta("Proveedor"); _lblColCantidad = CrearEtiqueta("Cantidad");
        _lblColPUnitario = CrearEtiqueta("P. Unitario"); _lblColImporteGasto = CrearEtiqueta("Importe"); _lblColCxp = CrearEtiqueta("CXP"); _lblColCap = CrearEtiqueta("CAP");
        _lblFilaTipoGasto = new XRLabel(); _lblFilaProveedor = new XRLabel(); _lblFilaCantidad = new XRLabel();
        _lblFilaPUnitario = new XRLabel(); _lblFilaImporteGasto = new XRLabel();
        _chkCxp = new XRCheckBox(); _chkCap = new XRCheckBox();

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
        _lblTitulo.Text = "Reporte de Proceso";

        _lineDivisor1.LocationFloat = new PointFloat(0, 92);
        _lineDivisor1.SizeF = new System.Drawing.SizeF(772, 6);

        //
        // Encabezado del Lote (2 columnas de pares etiqueta/valor)
        //
        UbicarPar(_lblEtqLote, _lblLote, 0, 105, 90, 150);
        UbicarPar(_lblEtqFecha, _lblFecha, 400, 105, 90, 150);
        UbicarPar(_lblEtqProductor, _lblProductor, 0, 125, 90, 260);
        UbicarPar(_lblEtqHuerta, _lblHuerta, 0, 145, 90, 260);
        UbicarPar(_lblEtqRegistro, _lblRegistro, 400, 145, 90, 150);
        UbicarPar(_lblEtqTipoCorte, _lblTipoCorte, 0, 165, 90, 260);
        UbicarPar(_lblEtqPeso, _lblPeso, 400, 165, 90, 150);

        _lineDivisor2.LocationFloat = new PointFloat(0, 190);
        _lineDivisor2.SizeF = new System.Drawing.SizeF(772, 6);

        //
        // Sección Categorías
        //
        _lblTituloCategorias.LocationFloat = new PointFloat(0, 0);
        UbicarColumna(_lblColCategoria, 0, 20, 180);
        UbicarColumna(_lblColKgSel, 180, 20, 80);
        UbicarColumna(_lblColPct, 260, 20, 50);
        UbicarColumna(_lblColKgComp, 310, 20, 80);
        UbicarColumna(_lblColCostoReal, 390, 20, 80);
        UbicarColumna(_lblColImpReal, 470, 20, 80);
        UbicarColumna(_lblColCostoEst, 550, 20, 80);
        UbicarColumna(_lblColImpEst, 630, 20, 90);

        UbicarColumna(_lblFilaCategoria, 0, 0, 180);
        _lblFilaCategoria.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[MateriaPrimaNombre]"));

        UbicarColumna(_lblFilaKgSel, 180, 0, 80);
        _lblFilaKgSel.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaKgSel.TextFormatString = "{0:N2}";
        _lblFilaKgSel.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[KilogramosSeleccionados]"));

        UbicarColumna(_lblFilaPct, 260, 0, 50);
        _lblFilaPct.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaPct.TextFormatString = "{0:N2}";
        _lblFilaPct.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Porcentaje]"));

        UbicarColumna(_lblFilaKgComp, 310, 0, 80);
        _lblFilaKgComp.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaKgComp.TextFormatString = "{0:N2}";
        _lblFilaKgComp.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[KilogramosComprados]"));

        UbicarColumna(_lblFilaCostoReal, 390, 0, 80);
        _lblFilaCostoReal.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCostoReal.TextFormatString = "{0:N4}";
        _lblFilaCostoReal.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CostoRealUnitario]"));

        UbicarColumna(_lblFilaImpReal, 470, 0, 80);
        _lblFilaImpReal.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImpReal.TextFormatString = "{0:C2}";
        _lblFilaImpReal.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ImporteReal]"));

        UbicarColumna(_lblFilaCostoEst, 550, 0, 80);
        _lblFilaCostoEst.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCostoEst.TextFormatString = "{0:N4}";
        _lblFilaCostoEst.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CostoEstimadoUnitario]"));

        UbicarColumna(_lblFilaImpEst, 630, 0, 90);
        _lblFilaImpEst.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImpEst.TextFormatString = "{0:C2}";
        _lblFilaImpEst.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ImporteEstimado]"));

        //
        // Sección Resumen por Mercado
        //
        _lblTituloMercado.LocationFloat = new PointFloat(0, 0);
        UbicarColumna(_lblColMercado, 0, 20, 150);
        UbicarColumna(_lblColKgMercado, 150, 20, 100);
        UbicarColumna(_lblColPctMercado, 250, 20, 100);
        UbicarColumna(_lblColImpRealMercado, 350, 20, 120);
        UbicarColumna(_lblColImpEstMercado, 470, 20, 130);

        UbicarColumna(_lblFilaMercado, 0, 0, 150);
        _lblFilaMercado.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Mercado]"));

        UbicarColumna(_lblFilaKgMercado, 150, 0, 100);
        _lblFilaKgMercado.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaKgMercado.TextFormatString = "{0:N2}";
        _lblFilaKgMercado.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        UbicarColumna(_lblFilaPctMercado, 250, 0, 100);
        _lblFilaPctMercado.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaPctMercado.TextFormatString = "{0:N2}";
        _lblFilaPctMercado.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Porcentaje]"));

        UbicarColumna(_lblFilaImpRealMercado, 350, 0, 120);
        _lblFilaImpRealMercado.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImpRealMercado.TextFormatString = "{0:C2}";
        _lblFilaImpRealMercado.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ImporteReal]"));

        UbicarColumna(_lblFilaImpEstMercado, 470, 0, 130);
        _lblFilaImpEstMercado.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImpEstMercado.TextFormatString = "{0:C2}";
        _lblFilaImpEstMercado.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ImporteEstimado]"));

        //
        // Sección Relación de Gastos
        //
        _lblTituloGastos.LocationFloat = new PointFloat(0, 0);
        UbicarColumna(_lblColTipoGasto, 0, 20, 90);
        UbicarColumna(_lblColProveedor, 90, 20, 220);
        UbicarColumna(_lblColCantidad, 310, 20, 80);
        UbicarColumna(_lblColPUnitario, 390, 20, 90);
        UbicarColumna(_lblColImporteGasto, 480, 20, 100);
        UbicarColumna(_lblColCxp, 580, 20, 50);
        UbicarColumna(_lblColCap, 630, 20, 50);

        UbicarColumna(_lblFilaTipoGasto, 0, 0, 90);
        _lblFilaTipoGasto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TipoGasto]"));

        UbicarColumna(_lblFilaProveedor, 90, 0, 220);
        _lblFilaProveedor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Proveedor]"));

        UbicarColumna(_lblFilaCantidad, 310, 0, 80);
        _lblFilaCantidad.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaCantidad.TextFormatString = "{0:N2}";
        _lblFilaCantidad.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Cantidad]"));

        UbicarColumna(_lblFilaPUnitario, 390, 0, 90);
        _lblFilaPUnitario.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaPUnitario.TextFormatString = "{0:C2}";
        _lblFilaPUnitario.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PrecioUnitario]"));

        UbicarColumna(_lblFilaImporteGasto, 480, 0, 100);
        _lblFilaImporteGasto.TextAlignment = TextAlignment.MiddleRight;
        _lblFilaImporteGasto.TextFormatString = "{0:C2}";
        _lblFilaImporteGasto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Importe]"));

        _chkCxp.LocationFloat = new PointFloat(580, 0);
        _chkCxp.SizeF = new System.Drawing.SizeF(50, 16);
        _chkCxp.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Checked", "[CXP]"));

        _chkCap.LocationFloat = new PointFloat(630, 0);
        _chkCap.SizeF = new System.Drawing.SizeF(50, 16);
        _chkCap.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Checked", "[CAP]"));

        //
        // Encabezado (membrete + datos del Lote)
        //
        _reportHeaderBand.HeightF = 200;
        _reportHeaderBand.Controls.AddRange(new XRControl[]
        {
            _picLogo, _lblRazonSocial, _lblDomicilio, _lblRfc, _lblTelefonoCorreo, _lblTitulo, _lineDivisor1,
            _lblEtqLote, _lblLote, _lblEtqFecha, _lblFecha, _lblEtqProductor, _lblProductor,
            _lblEtqHuerta, _lblHuerta, _lblEtqRegistro, _lblRegistro, _lblEtqTipoCorte, _lblTipoCorte,
            _lblEtqPeso, _lblPeso, _lineDivisor2,
        });

        _lblLote.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.LoteFolio]"));
        _lblFecha.TextFormatString = "{0:dd/MM/yyyy}";
        _lblFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.FechaCorrida]"));
        _lblProductor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.ProductorNombre]"));
        _lblHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.HuertaNombre]"));
        _lblRegistro.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.RegistroSagarpa]"));
        _lblTipoCorte.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.TipoCorteNombre]"));
        _lblPeso.TextFormatString = "{0:N2}";
        _lblPeso.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Lote.Kilogramos]"));

        _lblRazonSocial.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.RazonSocial]"));
        _lblDomicilio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.Domicilio]"));
        _lblRfc.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Rfc]"));
        _lblTelefonoCorreo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TelefonoCorreo]"));
        _picLogo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "ImageSource", "Iif(IsNullOrEmpty([Empresa.Logo]), Null, [Empresa.Logo])"));

        //
        // Cada sección: un DetailReportBand de encabezado (título + columnas, 1 fila fija) seguido
        // de un DetailReportBand de datos — ambos con un DetailBand simple anidado, sin
        // GroupHeaderBand (ver comentario junto a los campos arriba).
        //
        _detailBandCategoriasEncabezado.HeightF = 40;
        _detailBandCategoriasEncabezado.Controls.AddRange(new XRControl[]
        {
            _lblTituloCategorias, _lblColCategoria, _lblColKgSel, _lblColPct, _lblColKgComp, _lblColCostoReal, _lblColImpReal, _lblColCostoEst, _lblColImpEst,
        });
        _detailReportBandCategoriasEncabezado.Bands.Add(_detailBandCategoriasEncabezado);

        _detailBandCategorias.HeightF = 18;
        _detailBandCategorias.Controls.AddRange(new XRControl[]
        {
            _lblFilaCategoria, _lblFilaKgSel, _lblFilaPct, _lblFilaKgComp, _lblFilaCostoReal, _lblFilaImpReal, _lblFilaCostoEst, _lblFilaImpEst,
        });
        _detailReportBandCategorias.Bands.Add(_detailBandCategorias);

        _detailBandMercadoEncabezado.HeightF = 40;
        _detailBandMercadoEncabezado.Controls.AddRange(new XRControl[]
        {
            _lblTituloMercado, _lblColMercado, _lblColKgMercado, _lblColPctMercado, _lblColImpRealMercado, _lblColImpEstMercado,
        });
        _detailReportBandMercadoEncabezado.Bands.Add(_detailBandMercadoEncabezado);

        _detailBandMercado.HeightF = 18;
        _detailBandMercado.Controls.AddRange(new XRControl[]
        {
            _lblFilaMercado, _lblFilaKgMercado, _lblFilaPctMercado, _lblFilaImpRealMercado, _lblFilaImpEstMercado,
        });
        _detailReportBandMercado.Bands.Add(_detailBandMercado);

        _detailBandGastosEncabezado.HeightF = 40;
        _detailBandGastosEncabezado.Controls.AddRange(new XRControl[]
        {
            _lblTituloGastos, _lblColTipoGasto, _lblColProveedor, _lblColCantidad, _lblColPUnitario, _lblColImporteGasto, _lblColCxp, _lblColCap,
        });
        _detailReportBandGastosEncabezado.Bands.Add(_detailBandGastosEncabezado);

        _detailBandGastos.HeightF = 18;
        _detailBandGastos.Controls.AddRange(new XRControl[]
        {
            _lblFilaTipoGasto, _lblFilaProveedor, _lblFilaCantidad, _lblFilaPUnitario, _lblFilaImporteGasto, _chkCxp, _chkCap,
        });
        _detailReportBandGastos.Bands.Add(_detailBandGastos);

        _topMarginBand.HeightF = 39;
        _bottomMarginBand.HeightF = 39;

        // Todos los DetailReportBand son hermanos secuenciales (mismo Level = 0, el default) —
        // el orden de impresión lo da su posición en Bands.AddRange, no el Level (eso es solo
        // para reordenar hermanos del mismo nivel entre sí).
        //
        // Un reporte armado por código necesita, ADEMÁS de los DetailReportBand, una banda
        // DetailBand plana de nivel superior (puede ir vacía) — su sola presencia satisface el
        // requisito interno de DevExpress ("el reporte no contiene banda Detail"); con varios
        // DetailReportBand hermanos y ninguna DetailBand plana en el nivel superior,
        // CreateDocument() truena aunque cada DetailReportBand tenga su propio DetailBand anidado.
        _detailBandVacio = new DetailBand { HeightF = 0 };

        Bands.AddRange(new Band[]
        {
            _topMarginBand, _reportHeaderBand, _detailBandVacio,
            _detailReportBandCategoriasEncabezado, _detailReportBandCategorias,
            _detailReportBandMercadoEncabezado, _detailReportBandMercado,
            _detailReportBandGastosEncabezado, _detailReportBandGastos,
            _bottomMarginBand,
        });
        Font = new DXFont("Arial", 9);
        Margins = new System.Drawing.Printing.Margins(39, 39, 39, 39);
    }

    private static XRLabel CrearEtiqueta(string texto) => new() { Text = texto, Font = new DXFont("Arial", 9, DXFontStyle.Bold) };

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
