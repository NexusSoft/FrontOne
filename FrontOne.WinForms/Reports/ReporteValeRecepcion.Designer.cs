using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// Layout default del reporte "Vale de Recepción" — punto de partida antes de que alguien lo edite
// con el Diseñador de Reportes. Mismo criterio que ReporteRecepcionFruta (siempre una sola fila,
// sin DetailBand, ExpressionBindings declarativos contra el DataSource que arma CargarDatos), pero
// sin membrete de empresa — el vale no lleva logo/razón social, ver captura de referencia.
partial class ReporteValeRecepcion
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
    private BottomMarginBand _bottomMarginBand;

    private XRLabel _lblTitulo;
    private XRLine _lineDivisor1;

    private XRLabel _lblEtqFolio;
    private XRLabel _lblFolio;
    private XRLabel _lblEtqCoprefBico;
    private XRLabel _lblCoprefBico;
    private XRLabel _lblEtqFecha;
    private XRLabel _lblFecha;
    private XRLabel _lblEtqTicket;
    private XRLabel _lblTicket;
    private XRLabel _lblEtqProductor;
    private XRLabel _lblProductor;
    private XRLabel _lblEtqPesoBruto;
    private XRLabel _lblPesoBruto;
    private XRLabel _lblEtqHuerta;
    private XRLabel _lblHuerta;
    private XRLabel _lblEtqPesoTara;
    private XRLabel _lblPesoTara;
    private XRLabel _lblEtqRegSagarpa;
    private XRLabel _lblRegSagarpa;
    private XRLabel _lblEtqRegGgn;
    private XRLabel _lblRegGgn;
    private XRLabel _lblEtqTaraCajas;
    private XRLabel _lblTaraCajas;
    private XRLabel _lblEtqMunicipio;
    private XRLabel _lblMunicipio;
    private XRLabel _lblEtqPesoMuestra;
    private XRLabel _lblPesoMuestra;
    private XRLabel _lblEtqNoAcuerdo;
    private XRLabel _lblNoAcuerdo;
    private XRLabel _lblEtqPesoNeto;
    private XRLabel _lblPesoNeto;
    private XRLabel _lblEtqTransportista;
    private XRLabel _lblTransportista;
    private XRLabel _lblEtqChofer;
    private XRLabel _lblChofer;
    private XRLabel _lblEtqPlacas;
    private XRLabel _lblPlacas;
    private XRLabel _lblEtqObservaciones;
    private XRLabel _lblObservaciones;

    private XRLine _lineDivisor2;

    private XRLabel _lblColOrdenCorte;
    private XRLabel _lblColEmpresaCorte;
    private XRLabel _lblColProducto;
    private XRLabel _lblColCajas;
    private XRLabel _lblColPromedio;
    private XRLabel _lblColKilogramos;
    private XRLabel _lblOrdenCorte;
    private XRLabel _lblEmpresaCorte;
    private XRLabel _lblProducto;
    private XRLabel _lblCajasTabla;
    private XRLabel _lblPromedio;
    private XRLabel _lblKilogramosTabla;
    private XRLine _lineDivisor3;
    private XRLabel _lblEtqTotales;
    private XRLabel _lblTotalCajas;
    private XRLabel _lblTotalKilogramos;

    private XRLine _lineDivisor4;
    private XRLabel _lblEtqRecibio;
    private XRLine _lineRecibio;
    private XRLabel _lblEtqEntrego;
    private XRLine _lineEntrego;
    private XRLabel _lblEtqTarimas;
    private XRLine _lineTarimas;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _reportHeaderBand = new ReportHeaderBand();
        _bottomMarginBand = new BottomMarginBand();

        _lblTitulo = new XRLabel();
        _lineDivisor1 = new XRLine();

        _lblEtqFolio = CrearEtiqueta("Folio:");
        _lblFolio = new XRLabel();
        _lblEtqCoprefBico = CrearEtiqueta("BICO/RECO");
        _lblCoprefBico = new XRLabel();
        _lblEtqFecha = CrearEtiqueta("Fecha:");
        _lblFecha = new XRLabel();
        _lblEtqTicket = CrearEtiqueta("Ticket:");
        _lblTicket = new XRLabel();
        _lblEtqProductor = CrearEtiqueta("Productor:");
        _lblProductor = new XRLabel();
        _lblEtqPesoBruto = CrearEtiqueta("Peso Bruto:");
        _lblPesoBruto = new XRLabel();
        _lblEtqHuerta = CrearEtiqueta("Huerta:");
        _lblHuerta = new XRLabel();
        _lblEtqPesoTara = CrearEtiqueta("Peso Tara:");
        _lblPesoTara = new XRLabel();
        _lblEtqRegSagarpa = CrearEtiqueta("Reg. SAGARPA:");
        _lblRegSagarpa = new XRLabel();
        _lblEtqRegGgn = CrearEtiqueta("Reg. GGN:");
        _lblRegGgn = new XRLabel();
        _lblEtqTaraCajas = CrearEtiqueta("Tara Cajas:");
        _lblTaraCajas = new XRLabel();
        _lblEtqMunicipio = CrearEtiqueta("Municipio:");
        _lblMunicipio = new XRLabel();
        _lblEtqPesoMuestra = CrearEtiqueta("Peso Muestra:");
        _lblPesoMuestra = new XRLabel();
        _lblEtqNoAcuerdo = CrearEtiqueta("No. de Acuerdo:");
        _lblNoAcuerdo = new XRLabel();
        _lblEtqPesoNeto = CrearEtiqueta("Peso Neto:");
        _lblPesoNeto = new XRLabel { Font = new DXFont("Arial", 10, DXFontStyle.Bold) };
        _lblEtqTransportista = CrearEtiqueta("Transportista:");
        _lblTransportista = new XRLabel();
        _lblEtqChofer = CrearEtiqueta("Chofer:");
        _lblChofer = new XRLabel();
        _lblEtqPlacas = CrearEtiqueta("Placas:");
        _lblPlacas = new XRLabel();
        _lblEtqObservaciones = CrearEtiqueta("Observaciones:");
        _lblObservaciones = new XRLabel();

        _lineDivisor2 = new XRLine();

        _lblColOrdenCorte = CrearEtiqueta("Orden de Corte");
        _lblColEmpresaCorte = CrearEtiqueta("Empresa de Corte");
        _lblColProducto = CrearEtiqueta("Producto");
        _lblColCajas = CrearEtiqueta("Cajas");
        _lblColPromedio = CrearEtiqueta("P. Promedio");
        _lblColKilogramos = CrearEtiqueta("Kilogramos");
        _lblOrdenCorte = new XRLabel();
        _lblEmpresaCorte = new XRLabel();
        _lblProducto = new XRLabel();
        _lblCajasTabla = new XRLabel();
        _lblPromedio = new XRLabel();
        _lblKilogramosTabla = new XRLabel();
        _lineDivisor3 = new XRLine();
        _lblEtqTotales = CrearEtiqueta("Totales:");
        _lblTotalCajas = new XRLabel();
        _lblTotalKilogramos = new XRLabel();

        _lineDivisor4 = new XRLine();
        _lblEtqRecibio = CrearEtiqueta("Recibió:");
        _lineRecibio = new XRLine();
        _lblEtqEntrego = CrearEtiqueta("Entregó:");
        _lineEntrego = new XRLine();
        _lblEtqTarimas = CrearEtiqueta("Tarimas:");
        _lineTarimas = new XRLine();

        //
        // Título
        //
        _lblTitulo.LocationFloat = new PointFloat(0, 0);
        _lblTitulo.SizeF = new System.Drawing.SizeF(772, 24);
        _lblTitulo.Text = "Vale de Recepción";
        _lblTitulo.Font = new DXFont("Arial", 14, DXFontStyle.Bold);
        _lblTitulo.TextAlignment = TextAlignment.TopCenter;

        _lineDivisor1.LocationFloat = new PointFloat(0, 28);
        _lineDivisor1.SizeF = new System.Drawing.SizeF(772, 2);

        //
        // Bloque A (izquierda: datos de Recepción/Productor/Huerta) + Bloque B (derecha: báscula)
        //
        UbicarPar(_lblEtqFolio, _lblFolio, 0, 38, 90, 150);
        UbicarPar(_lblEtqCoprefBico, _lblCoprefBico, 400, 38, 90, 150);
        UbicarPar(_lblEtqFecha, _lblFecha, 0, 56, 90, 150);
        UbicarPar(_lblEtqTicket, _lblTicket, 400, 56, 90, 150);
        UbicarPar(_lblEtqProductor, _lblProductor, 0, 74, 90, 270);
        UbicarPar(_lblEtqPesoBruto, _lblPesoBruto, 400, 74, 90, 150);
        UbicarPar(_lblEtqHuerta, _lblHuerta, 0, 92, 90, 270);
        UbicarPar(_lblEtqPesoTara, _lblPesoTara, 400, 92, 90, 150);
        UbicarPar(_lblEtqRegSagarpa, _lblRegSagarpa, 0, 110, 90, 150);
        UbicarPar(_lblEtqTaraCajas, _lblTaraCajas, 400, 110, 90, 150);
        UbicarPar(_lblEtqRegGgn, _lblRegGgn, 250, 110, 65, 130);
        UbicarPar(_lblEtqMunicipio, _lblMunicipio, 0, 128, 90, 270);
        UbicarPar(_lblEtqPesoMuestra, _lblPesoMuestra, 400, 128, 90, 150);
        UbicarPar(_lblEtqNoAcuerdo, _lblNoAcuerdo, 0, 146, 90, 370);
        UbicarPar(_lblEtqPesoNeto, _lblPesoNeto, 400, 146, 90, 150);
        UbicarPar(_lblEtqTransportista, _lblTransportista, 0, 164, 90, 370);
        UbicarPar(_lblEtqChofer, _lblChofer, 0, 182, 90, 150);
        UbicarPar(_lblEtqPlacas, _lblPlacas, 260, 182, 55, 150);
        UbicarPar(_lblEtqObservaciones, _lblObservaciones, 0, 200, 90, 480);

        _lineDivisor2.LocationFloat = new PointFloat(0, 224);
        _lineDivisor2.SizeF = new System.Drawing.SizeF(772, 2);

        //
        // Tabla Orden de Corte/Empresa de Corte/Producto/Cajas/P. Promedio/Kilogramos (una sola
        // fila — regla de negocio: 1 Orden de Corte por Recepción)
        //
        _lblColOrdenCorte.LocationFloat = new PointFloat(0, 232);
        _lblColOrdenCorte.SizeF = new System.Drawing.SizeF(130, 16);
        _lblColEmpresaCorte.LocationFloat = new PointFloat(130, 232);
        _lblColEmpresaCorte.SizeF = new System.Drawing.SizeF(160, 16);
        _lblColProducto.LocationFloat = new PointFloat(290, 232);
        _lblColProducto.SizeF = new System.Drawing.SizeF(180, 16);
        _lblColCajas.LocationFloat = new PointFloat(470, 232);
        _lblColCajas.SizeF = new System.Drawing.SizeF(80, 16);
        _lblColPromedio.LocationFloat = new PointFloat(550, 232);
        _lblColPromedio.SizeF = new System.Drawing.SizeF(100, 16);
        _lblColKilogramos.LocationFloat = new PointFloat(650, 232);
        _lblColKilogramos.SizeF = new System.Drawing.SizeF(122, 16);

        _lblOrdenCorte.LocationFloat = new PointFloat(0, 250);
        _lblOrdenCorte.SizeF = new System.Drawing.SizeF(130, 16);
        _lblEmpresaCorte.LocationFloat = new PointFloat(130, 250);
        _lblEmpresaCorte.SizeF = new System.Drawing.SizeF(160, 16);
        _lblProducto.LocationFloat = new PointFloat(290, 250);
        _lblProducto.SizeF = new System.Drawing.SizeF(180, 16);
        _lblCajasTabla.LocationFloat = new PointFloat(470, 250);
        _lblCajasTabla.SizeF = new System.Drawing.SizeF(80, 16);
        _lblPromedio.LocationFloat = new PointFloat(550, 250);
        _lblPromedio.SizeF = new System.Drawing.SizeF(100, 16);
        _lblKilogramosTabla.LocationFloat = new PointFloat(650, 250);
        _lblKilogramosTabla.SizeF = new System.Drawing.SizeF(122, 16);

        _lineDivisor3.LocationFloat = new PointFloat(0, 268);
        _lineDivisor3.SizeF = new System.Drawing.SizeF(772, 2);

        _lblEtqTotales.LocationFloat = new PointFloat(290, 274);
        _lblEtqTotales.SizeF = new System.Drawing.SizeF(180, 16);
        _lblTotalCajas.LocationFloat = new PointFloat(470, 274);
        _lblTotalCajas.SizeF = new System.Drawing.SizeF(80, 16);
        _lblTotalKilogramos.LocationFloat = new PointFloat(650, 274);
        _lblTotalKilogramos.SizeF = new System.Drawing.SizeF(122, 16);

        //
        // Pie de firmas (Recibió / Entregó / Tarimas) — sin binding, son líneas para firmar
        //
        _lineDivisor4.LocationFloat = new PointFloat(0, 310);
        _lineDivisor4.SizeF = new System.Drawing.SizeF(772, 2);

        _lblEtqRecibio.LocationFloat = new PointFloat(0, 380);
        _lblEtqRecibio.SizeF = new System.Drawing.SizeF(55, 16);
        _lineRecibio.LocationFloat = new PointFloat(60, 392);
        _lineRecibio.SizeF = new System.Drawing.SizeF(220, 2);

        _lblEtqEntrego.LocationFloat = new PointFloat(310, 380);
        _lblEtqEntrego.SizeF = new System.Drawing.SizeF(60, 16);
        _lineEntrego.LocationFloat = new PointFloat(375, 392);
        _lineEntrego.SizeF = new System.Drawing.SizeF(220, 2);

        _lblEtqTarimas.LocationFloat = new PointFloat(630, 380);
        _lblEtqTarimas.SizeF = new System.Drawing.SizeF(55, 16);
        _lineTarimas.LocationFloat = new PointFloat(690, 392);
        _lineTarimas.SizeF = new System.Drawing.SizeF(82, 2);

        // Binding declarativo (regla dura, ver CLAUDE.md) contra el DataSource de una sola fila
        // (RecepcionFrutaReporteDto directo — sin membrete de empresa no hace falta wrapper) que
        // arma ReporteValeRecepcion.CargarDatos.
        _lblFolio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Folio]"));
        _lblCoprefBico.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CoprefBico]"));
        _lblFecha.TextFormatString = "{0:dd/MM/yyyy}";
        _lblFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Fecha]"));
        _lblTicket.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[NumeroTicket]"));
        _lblProductor.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductorNombre]"));
        _lblPesoBruto.TextFormatString = "{0:N2}";
        _lblPesoBruto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PesoBruto]"));
        _lblHuerta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaNombre]"));
        _lblPesoTara.TextFormatString = "{0:N2}";
        _lblPesoTara.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PesoTara]"));
        _lblRegSagarpa.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaRegistroSagarpa]"));
        _lblTaraCajas.TextFormatString = "{0:N2}";
        _lblTaraCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TaraCajas]"));
        _lblRegGgn.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaRegistroGgn]"));
        _lblMunicipio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[HuertaMunicipioNombre]"));
        _lblPesoMuestra.TextFormatString = "{0:N2}";
        _lblPesoMuestra.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PesoMuestra]"));
        _lblNoAcuerdo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "Concat([AcuerdoCorteFolio], '   ', [TipoCorteNombre])"));
        _lblPesoNeto.TextFormatString = "{0:N2}";
        _lblPesoNeto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PesoNeto]"));
        _lblTransportista.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TransportistaNombre]"));
        _lblChofer.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Chofer]"));
        _lblPlacas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Placas]"));
        _lblObservaciones.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Observaciones]"));

        _lblOrdenCorte.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[OrdenCorteFolio]"));
        _lblEmpresaCorte.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[EmpresaCorteNombre]"));
        _lblProducto.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductoNombre]"));
        _lblCajasTabla.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CajasCortadas]"));
        _lblPromedio.TextFormatString = "{0:N2}";
        _lblPromedio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text",
            "Iif([CajasCortadas] = 0, 0, [Kilogramos] / [CajasCortadas])"));
        _lblKilogramosTabla.TextFormatString = "{0:N2}";
        _lblKilogramosTabla.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));
        _lblTotalCajas.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CajasCortadas]"));
        _lblTotalKilogramos.TextFormatString = "{0:N2}";
        _lblTotalKilogramos.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Kilogramos]"));

        //
        // _reportHeaderBand
        //
        _reportHeaderBand.HeightF = 440;
        _reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[]
        {
            _lblTitulo, _lineDivisor1,
            _lblEtqFolio, _lblFolio, _lblEtqCoprefBico, _lblCoprefBico,
            _lblEtqFecha, _lblFecha, _lblEtqTicket, _lblTicket,
            _lblEtqProductor, _lblProductor, _lblEtqPesoBruto, _lblPesoBruto,
            _lblEtqHuerta, _lblHuerta, _lblEtqPesoTara, _lblPesoTara,
            _lblEtqRegSagarpa, _lblRegSagarpa, _lblEtqRegGgn, _lblRegGgn, _lblEtqTaraCajas, _lblTaraCajas,
            _lblEtqMunicipio, _lblMunicipio, _lblEtqPesoMuestra, _lblPesoMuestra,
            _lblEtqNoAcuerdo, _lblNoAcuerdo, _lblEtqPesoNeto, _lblPesoNeto,
            _lblEtqTransportista, _lblTransportista,
            _lblEtqChofer, _lblChofer, _lblEtqPlacas, _lblPlacas,
            _lblEtqObservaciones, _lblObservaciones,
            _lineDivisor2,
            _lblColOrdenCorte, _lblColEmpresaCorte, _lblColProducto, _lblColCajas, _lblColPromedio, _lblColKilogramos,
            _lblOrdenCorte, _lblEmpresaCorte, _lblProducto, _lblCajasTabla, _lblPromedio, _lblKilogramosTabla,
            _lineDivisor3, _lblEtqTotales, _lblTotalCajas, _lblTotalKilogramos,
            _lineDivisor4,
            _lblEtqRecibio, _lineRecibio, _lblEtqEntrego, _lineEntrego, _lblEtqTarimas, _lineTarimas,
        });

        //
        // ReporteValeRecepcion
        //
        Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { _topMarginBand, _reportHeaderBand, _bottomMarginBand });
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
}
