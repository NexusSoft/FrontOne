using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// Layout default del reporte "Recepción de Fruta" — punto de partida antes de que alguien lo
// edite con el Diseñador de Reportes (ver Forms/Sistema/DisenadorReporteForm.cs). No usa
// databinding real: como siempre es un solo registro, los valores se asignan directo en
// ReporteRecepcionFruta.cs (método CargarDatos), no hace falta un DetailBand que repita filas.
partial class ReporteRecepcionFruta
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

    private XRPictureBox _picLogo;
    private XRLabel _lblRazonSocial;
    private XRLabel _lblDomicilio;
    private XRLabel _lblRfc;
    private XRLabel _lblTelefonoCorreo;
    private XRLabel _lblTitulo;
    private XRLine _lineDivisor1;

    private XRLabel _lblEtqNoLote;
    private XRLabel _lblNoLote;
    private XRLabel _lblEtqFecha;
    private XRLabel _lblFecha;
    private XRLabel _lblEtqChofer;
    private XRLabel _lblChofer;
    private XRLabel _lblEtqPlacas;
    private XRLabel _lblPlacas;
    private XRLabel _lblEtqObservaciones;
    private XRLabel _lblObservaciones;
    private XRLabel _lblEtqTicket;
    private XRLabel _lblTicket;
    private XRLabel _lblEtqPesoBruto;
    private XRLabel _lblPesoBruto;
    private XRLabel _lblEtqPesoTara;
    private XRLabel _lblPesoTara;
    private XRLabel _lblEtqPesoMuestra;
    private XRLabel _lblPesoMuestra;
    private XRLabel _lblEtqPesoNeto;
    private XRLabel _lblPesoNeto;

    private XRLine _lineDivisor2;

    private XRLabel _lblEtqHuerta;
    private XRLabel _lblHuerta;
    private XRLabel _lblEtqProductor;
    private XRLabel _lblProductor;
    private XRLabel _lblEtqTipoCorte;
    private XRLabel _lblTipoCorte;
    private XRLabel _lblEtqNoAcuerdo;
    private XRLabel _lblNoAcuerdo;
    private XRLabel _lblEtqTransportista;
    private XRLabel _lblTransportista;
    private XRLabel _lblEtqEmpresaCorte;
    private XRLabel _lblEmpresaCorte;
    private XRLabel _lblEtqNoCandado;
    private XRLabel _lblNoCandado;
    private XRLabel _lblEtqObservacionesOrden;
    private XRLabel _lblObservacionesOrden;
    private XRLabel _lblEtqCajasEntregadas;
    private XRLabel _lblCajasEntregadas;
    private XRLabel _lblEtqCajasCortadas;
    private XRLabel _lblCajasCortadas;
    private XRLabel _lblEtqCajasRecibidasVacias;
    private XRLabel _lblCajasRecibidasVacias;
    private XRLabel _lblEtqDiferencia;
    private XRLabel _lblDiferencia;
    private XRLabel _lblEtqNoCortadores;
    private XRLabel _lblNoCortadores;

    private XRLine _lineDivisor3;

    private XRLabel _lblColProducto;
    private XRLabel _lblColVariedad;
    private XRLabel _lblColCajas;
    private XRLabel _lblColKilogramos;
    private XRLabel _lblProducto;
    private XRLabel _lblVariedad;
    private XRLabel _lblCajasTabla;
    private XRLabel _lblKilogramosTabla;
    private XRLine _lineDivisor4;
    private XRLabel _lblEtqTotales;
    private XRLabel _lblTotalCajas;
    private XRLabel _lblTotalKilogramos;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _reportHeaderBand = new ReportHeaderBand();
        _bottomMarginBand = new BottomMarginBand();

        _picLogo = new XRPictureBox();
        _lblRazonSocial = new XRLabel();
        _lblDomicilio = new XRLabel();
        _lblRfc = new XRLabel();
        _lblTelefonoCorreo = new XRLabel();
        _lblTitulo = new XRLabel();
        _lineDivisor1 = new XRLine();

        _lblEtqNoLote = CrearEtiqueta("No. de Lote:");
        _lblNoLote = new XRLabel();
        _lblEtqFecha = CrearEtiqueta("Fecha:");
        _lblFecha = new XRLabel();
        _lblEtqChofer = CrearEtiqueta("Chofer:");
        _lblChofer = new XRLabel();
        _lblEtqPlacas = CrearEtiqueta("Placas:");
        _lblPlacas = new XRLabel();
        _lblEtqObservaciones = CrearEtiqueta("Observaciones:");
        _lblObservaciones = new XRLabel();
        _lblEtqTicket = CrearEtiqueta("Ticket:");
        _lblTicket = new XRLabel();
        _lblEtqPesoBruto = CrearEtiqueta("Peso Bruto:");
        _lblPesoBruto = new XRLabel();
        _lblEtqPesoTara = CrearEtiqueta("Peso Tara:");
        _lblPesoTara = new XRLabel();
        _lblEtqPesoMuestra = CrearEtiqueta("Peso Muestra:");
        _lblPesoMuestra = new XRLabel();
        _lblEtqPesoNeto = CrearEtiqueta("Peso Neto:");
        _lblPesoNeto = new XRLabel();

        _lineDivisor2 = new XRLine();

        _lblEtqHuerta = CrearEtiqueta("Huerta:");
        _lblHuerta = new XRLabel();
        _lblEtqProductor = CrearEtiqueta("Productor:");
        _lblProductor = new XRLabel();
        _lblEtqTipoCorte = CrearEtiqueta("Tipo de Corte:");
        _lblTipoCorte = new XRLabel();
        _lblEtqNoAcuerdo = CrearEtiqueta("No. de Acuerdo:");
        _lblNoAcuerdo = new XRLabel();
        _lblEtqTransportista = CrearEtiqueta("Transportista:");
        _lblTransportista = new XRLabel();
        _lblEtqEmpresaCorte = CrearEtiqueta("Empresa de Corte:");
        _lblEmpresaCorte = new XRLabel();
        _lblEtqNoCandado = CrearEtiqueta("No. de Candado:");
        _lblNoCandado = new XRLabel();
        _lblEtqObservacionesOrden = CrearEtiqueta("Observaciones:");
        _lblObservacionesOrden = new XRLabel();
        _lblEtqCajasEntregadas = CrearEtiqueta("Cajas Entregadas:");
        _lblCajasEntregadas = new XRLabel();
        _lblEtqCajasCortadas = CrearEtiqueta("Cajas Cortadas:");
        _lblCajasCortadas = new XRLabel();
        _lblEtqCajasRecibidasVacias = CrearEtiqueta("Recibidas Vacías:");
        _lblCajasRecibidasVacias = new XRLabel();
        _lblEtqDiferencia = CrearEtiqueta("Diferencia:");
        _lblDiferencia = new XRLabel();
        _lblEtqNoCortadores = CrearEtiqueta("No. de Cortadores:");
        _lblNoCortadores = new XRLabel();

        _lineDivisor3 = new XRLine();

        _lblColProducto = CrearEtiqueta("Producto");
        _lblColVariedad = CrearEtiqueta("Variedad");
        _lblColCajas = CrearEtiqueta("Cajas");
        _lblColKilogramos = CrearEtiqueta("Kilogramos");
        _lblProducto = new XRLabel();
        _lblVariedad = new XRLabel();
        _lblCajasTabla = new XRLabel();
        _lblKilogramosTabla = new XRLabel();
        _lineDivisor4 = new XRLine();
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

        _lblTitulo.LocationFloat = new PointFloat(0, 70);
        _lblTitulo.SizeF = new System.Drawing.SizeF(772, 20);
        _lblTitulo.Text = "Recepción de Ordenes de Corte";
        _lblTitulo.Font = new DXFont("Arial", 12, DXFontStyle.Bold);

        _lineDivisor1.LocationFloat = new PointFloat(0, 95);
        _lineDivisor1.SizeF = new System.Drawing.SizeF(772, 2);

        //
        // Bloque A (encabezado de Recepción)
        //
        UbicarPar(_lblEtqNoLote, _lblNoLote, 0, 105, 90, 150);
        UbicarPar(_lblEtqTicket, _lblTicket, 400, 105, 90, 150);
        UbicarPar(_lblEtqFecha, _lblFecha, 0, 123, 90, 150);
        UbicarPar(_lblEtqPesoBruto, _lblPesoBruto, 400, 123, 90, 150);
        UbicarPar(_lblEtqChofer, _lblChofer, 0, 141, 90, 150);
        UbicarPar(_lblEtqPesoTara, _lblPesoTara, 400, 141, 90, 150);
        UbicarPar(_lblEtqPlacas, _lblPlacas, 0, 159, 90, 150);
        UbicarPar(_lblEtqPesoMuestra, _lblPesoMuestra, 400, 159, 90, 150);
        UbicarPar(_lblEtqObservaciones, _lblObservaciones, 0, 177, 90, 150);
        UbicarPar(_lblEtqPesoNeto, _lblPesoNeto, 400, 177, 90, 150);

        _lineDivisor2.LocationFloat = new PointFloat(0, 200);
        _lineDivisor2.SizeF = new System.Drawing.SizeF(772, 2);

        //
        // Bloque B (Orden de Corte asociada)
        //
        UbicarPar(_lblEtqHuerta, _lblHuerta, 0, 210, 110, 270);
        UbicarPar(_lblEtqCajasEntregadas, _lblCajasEntregadas, 400, 210, 110, 130);
        UbicarPar(_lblEtqProductor, _lblProductor, 0, 228, 110, 270);
        UbicarPar(_lblEtqCajasCortadas, _lblCajasCortadas, 400, 228, 110, 130);
        UbicarPar(_lblEtqTipoCorte, _lblTipoCorte, 0, 246, 110, 270);
        UbicarPar(_lblEtqCajasRecibidasVacias, _lblCajasRecibidasVacias, 400, 246, 110, 130);
        UbicarPar(_lblEtqNoAcuerdo, _lblNoAcuerdo, 0, 264, 110, 270);
        UbicarPar(_lblEtqDiferencia, _lblDiferencia, 400, 264, 110, 130);
        UbicarPar(_lblEtqTransportista, _lblTransportista, 0, 282, 110, 270);
        UbicarPar(_lblEtqNoCortadores, _lblNoCortadores, 400, 282, 110, 130);
        UbicarPar(_lblEtqEmpresaCorte, _lblEmpresaCorte, 0, 300, 110, 270);
        UbicarPar(_lblEtqNoCandado, _lblNoCandado, 0, 318, 110, 270);
        UbicarPar(_lblEtqObservacionesOrden, _lblObservacionesOrden, 0, 336, 110, 270);

        _lineDivisor3.LocationFloat = new PointFloat(0, 360);
        _lineDivisor3.SizeF = new System.Drawing.SizeF(772, 2);

        //
        // Tabla Producto/Variedad/Cajas/Kilogramos (una sola fila — regla de negocio: 1 Orden de
        // Corte por Recepción)
        //
        _lblColProducto.LocationFloat = new PointFloat(0, 370);
        _lblColProducto.SizeF = new System.Drawing.SizeF(200, 16);
        _lblColVariedad.LocationFloat = new PointFloat(200, 370);
        _lblColVariedad.SizeF = new System.Drawing.SizeF(200, 16);
        _lblColCajas.LocationFloat = new PointFloat(400, 370);
        _lblColCajas.SizeF = new System.Drawing.SizeF(100, 16);
        _lblColKilogramos.LocationFloat = new PointFloat(500, 370);
        _lblColKilogramos.SizeF = new System.Drawing.SizeF(272, 16);

        _lblProducto.LocationFloat = new PointFloat(0, 388);
        _lblProducto.SizeF = new System.Drawing.SizeF(200, 16);
        _lblVariedad.LocationFloat = new PointFloat(200, 388);
        _lblVariedad.SizeF = new System.Drawing.SizeF(200, 16);
        _lblCajasTabla.LocationFloat = new PointFloat(400, 388);
        _lblCajasTabla.SizeF = new System.Drawing.SizeF(100, 16);
        _lblKilogramosTabla.LocationFloat = new PointFloat(500, 388);
        _lblKilogramosTabla.SizeF = new System.Drawing.SizeF(272, 16);

        _lineDivisor4.LocationFloat = new PointFloat(0, 406);
        _lineDivisor4.SizeF = new System.Drawing.SizeF(772, 2);

        _lblEtqTotales.LocationFloat = new PointFloat(200, 412);
        _lblEtqTotales.SizeF = new System.Drawing.SizeF(200, 16);
        _lblTotalCajas.LocationFloat = new PointFloat(400, 412);
        _lblTotalCajas.SizeF = new System.Drawing.SizeF(100, 16);
        _lblTotalKilogramos.LocationFloat = new PointFloat(500, 412);
        _lblTotalKilogramos.SizeF = new System.Drawing.SizeF(272, 16);

        //
        // _reportHeaderBand
        //
        _reportHeaderBand.HeightF = 440;
        _reportHeaderBand.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[]
        {
            _picLogo, _lblRazonSocial, _lblDomicilio, _lblRfc, _lblTelefonoCorreo, _lblTitulo, _lineDivisor1,
            _lblEtqNoLote, _lblNoLote, _lblEtqFecha, _lblFecha, _lblEtqChofer, _lblChofer, _lblEtqPlacas, _lblPlacas,
            _lblEtqObservaciones, _lblObservaciones, _lblEtqTicket, _lblTicket, _lblEtqPesoBruto, _lblPesoBruto,
            _lblEtqPesoTara, _lblPesoTara, _lblEtqPesoMuestra, _lblPesoMuestra, _lblEtqPesoNeto, _lblPesoNeto,
            _lineDivisor2,
            _lblEtqHuerta, _lblHuerta, _lblEtqProductor, _lblProductor, _lblEtqTipoCorte, _lblTipoCorte,
            _lblEtqNoAcuerdo, _lblNoAcuerdo, _lblEtqTransportista, _lblTransportista, _lblEtqEmpresaCorte, _lblEmpresaCorte,
            _lblEtqNoCandado, _lblNoCandado, _lblEtqObservacionesOrden, _lblObservacionesOrden,
            _lblEtqCajasEntregadas, _lblCajasEntregadas, _lblEtqCajasCortadas, _lblCajasCortadas,
            _lblEtqCajasRecibidasVacias, _lblCajasRecibidasVacias, _lblEtqDiferencia, _lblDiferencia,
            _lblEtqNoCortadores, _lblNoCortadores,
            _lineDivisor3,
            _lblColProducto, _lblColVariedad, _lblColCajas, _lblColKilogramos,
            _lblProducto, _lblVariedad, _lblCajasTabla, _lblKilogramosTabla,
            _lineDivisor4, _lblEtqTotales, _lblTotalCajas, _lblTotalKilogramos,
        });

        //
        // ReporteRecepcionFruta
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
