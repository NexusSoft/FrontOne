using DevExpress.Drawing;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// Layout default del PDF de Incidencias — punto de partida antes de que alguien lo edite con el
// Diseñador de Reportes (ver Forms/Sistema/DisenadorReporteForm.cs). Apaisado (Tabloide, 17x11")
// porque cada Incidencia trae muchos campos. El membrete/rango de fecha están enlazados
// declarativamente (regla dura, ver CLAUDE.md) contra el DataSource de una fila que arma
// ReporteIncidencias.CargarDatos (VistaEncabezado); las tarjetas de Incidencia viven en un
// DetailBand anidado dentro de _detailReportBand — el único tipo de banda de DevExpress con
// DataSource propio, independiente del DataSource de una fila del reporte — porque la lista de
// Incidencias (IncidenciaReporteDto) tiene una forma distinta a la del encabezado. El usuario
// puede cambiar el tamaño de hoja al vuelo desde el selector de VisorReporteForm (ver
// TamanoPapelReporte.cs).
partial class ReporteIncidencias
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
    private BottomMarginBand _bottomMarginBand;

    private XRPictureBox _picLogo;
    private XRLabel _lblRazonSocial;
    private XRLabel _lblDomicilio;
    private XRLabel _lblRfc;
    private XRLabel _lblTelefonoCorreo;
    private XRLabel _lblTitulo;
    private XRLabel _lblRangoFecha;
    private XRLine _lineDivisor1;

    private void InitializeComponent()
    {
        _topMarginBand = new TopMarginBand();
        _reportHeaderBand = new ReportHeaderBand();
        _detailReportBand = new DetailReportBand();
        _detailBand = new DetailBand();
        _bottomMarginBand = new BottomMarginBand();

        _picLogo = new XRPictureBox();
        _lblRazonSocial = new XRLabel();
        _lblDomicilio = new XRLabel();
        _lblRfc = new XRLabel();
        _lblTelefonoCorreo = new XRLabel();
        _lblTitulo = new XRLabel();
        _lblRangoFecha = new XRLabel();
        _lineDivisor1 = new XRLine();

        //
        // Membrete (logo + datos de la empresa)
        //
        _picLogo.LocationFloat = new PointFloat(0, 0);
        _picLogo.SizeF = new System.Drawing.SizeF(140, 55);
        _picLogo.Sizing = ImageSizeMode.ZoomImage;

        _lblRazonSocial.LocationFloat = new PointFloat(400, 0);
        _lblRazonSocial.SizeF = new System.Drawing.SizeF(1247, 16);
        _lblRazonSocial.Font = new DXFont("Arial", 9, DXFontStyle.Bold);

        _lblDomicilio.LocationFloat = new PointFloat(400, 16);
        _lblDomicilio.SizeF = new System.Drawing.SizeF(1247, 14);

        _lblRfc.LocationFloat = new PointFloat(400, 30);
        _lblRfc.SizeF = new System.Drawing.SizeF(1247, 14);

        _lblTelefonoCorreo.LocationFloat = new PointFloat(400, 44);
        _lblTelefonoCorreo.SizeF = new System.Drawing.SizeF(1247, 14);

        _lblTitulo.LocationFloat = new PointFloat(0, 62);
        _lblTitulo.SizeF = new System.Drawing.SizeF(1647, 20);
        _lblTitulo.Font = new DXFont("Arial", 13, DXFontStyle.Bold);
        _lblTitulo.TextAlignment = TextAlignment.MiddleCenter;
        _lblTitulo.Text = "Incidencias de Corte";

        _lblRangoFecha.LocationFloat = new PointFloat(0, 84);
        _lblRangoFecha.SizeF = new System.Drawing.SizeF(1647, 16);
        _lblRangoFecha.Font = new DXFont("Arial", 9, DXFontStyle.Bold);
        _lblRangoFecha.TextAlignment = TextAlignment.MiddleCenter;

        _lineDivisor1.LocationFloat = new PointFloat(0, 104);
        _lineDivisor1.SizeF = new System.Drawing.SizeF(1647, 4);

        // Binding declarativo (regla dura, ver CLAUDE.md) contra el DataSource de una sola fila
        // que arma ReporteIncidencias.CargarDatos (VistaEncabezado: RangoFecha ya formateado +
        // Empresa + Rfc/TelefonoCorreo ya formateados) — ruta anidada [Empresa.Campo] porque el
        // wrapper no aplana EmpresaConfiguracionDto. El logo se enlaza vía ImageSource con un Iif
        // para no romper si viene vacío.
        _lblRangoFecha.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RangoFecha]"));
        _lblRazonSocial.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.RazonSocial]"));
        _lblDomicilio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Empresa.Domicilio]"));
        _lblRfc.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Rfc]"));
        _lblTelefonoCorreo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TelefonoCorreo]"));
        _picLogo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "ImageSource", "Iif(IsNullOrEmpty([Empresa.Logo]), Null, [Empresa.Logo])"));

        //
        // Encabezados de columna — misma posición X/ancho que la fila del DetailBand. Las 26
        // columnas caben en una sola fila usando el ancho completo de la hoja Tabloide apaisada
        // (~1647 de 1670 puntos de contenido disponibles), con más aire que la versión anterior en
        // Legal (~1349pt) — columnas y fuente más grandes, más legible.
        //
        var yEncabezado = 112f;
        var etiquetas = new (string Texto, float X, float Ancho)[]
        {
            ("Folio", 0, 47), ("Fecha", 47, 53), ("Huerta", 100, 88), ("Productor", 188, 88),
            ("Acopiador", 276, 67), ("Supervisor", 343, 67), ("Reg. Sagarpa", 410, 62),
            ("Tipo Corte", 472, 57), ("Tipo Pago", 529, 57), ("Precio", 586, 42),
            ("Jefe Cuadrilla", 628, 78), ("Transportista", 706, 78), ("Floración", 784, 57),
            ("Municipio", 841, 62), ("Localidad", 903, 62),
            ("Beneficiario", 965, 83), ("Cajas Ent.", 1048, 47), ("ODC SAP Cosecha", 1095, 57),
            ("ODC SAP Flete", 1152, 57), ("Teléfono", 1209, 57), ("Placas", 1266, 53),
            ("Báscula", 1319, 67), ("Punto de Reunión", 1386, 94), ("Hora Llegada", 1480, 47),
            ("Cajas Cosech.", 1527, 53), ("Caja/Cuadr.", 1580, 67),
        };

        var controlesEncabezado = new List<DevExpress.XtraReports.UI.XRControl>
        {
            _picLogo, _lblRazonSocial, _lblDomicilio, _lblRfc, _lblTelefonoCorreo, _lblTitulo, _lblRangoFecha, _lineDivisor1,
        };

        foreach (var (texto, x, ancho) in etiquetas)
        {
            controlesEncabezado.Add(CrearEtiquetaEncabezado(texto, x, yEncabezado, ancho));
        }

        //
        // Fila de detalle: una sola fila de 26 columnas + 3 líneas de texto libre, una "tarjeta"
        // por Incidencia, enlazadas a las propiedades de IncidenciaReporteDto.
        //
        var controlesDetalle = new List<DevExpress.XtraReports.UI.XRControl>();

        AgregarColumnaDetalle(controlesDetalle, "[Folio]", 0, 0, 47);
        AgregarColumnaDetalle(controlesDetalle, "[Fecha]", 47, 0, 53, "{0:dd/MM/yy}");
        AgregarColumnaDetalle(controlesDetalle, "[HuertaNombre]", 100, 0, 88);
        AgregarColumnaDetalle(controlesDetalle, "[ProductorNombre]", 188, 0, 88);
        AgregarColumnaDetalle(controlesDetalle, "[Acopiador]", 276, 0, 67);
        AgregarColumnaDetalle(controlesDetalle, "[SupervisorHuertaNombre]", 343, 0, 67);
        AgregarColumnaDetalle(controlesDetalle, "[RegistroSagarpa]", 410, 0, 62);
        AgregarColumnaDetalle(controlesDetalle, "[TipoCorteNombre]", 472, 0, 57);
        AgregarColumnaDetalle(controlesDetalle, "[TipoPagoNombre]", 529, 0, 57);
        AgregarColumnaDetalle(controlesDetalle, "[CostoKg]", 586, 0, 42, "{0:N2}");
        AgregarColumnaDetalle(controlesDetalle, "[JefeCuadrillaNombre]", 628, 0, 78);
        AgregarColumnaDetalle(controlesDetalle, "[TransportistaNombre]", 706, 0, 78);
        AgregarColumnaDetalle(controlesDetalle, "[FloracionNombre]", 784, 0, 57);
        AgregarColumnaDetalle(controlesDetalle, "[MunicipioNombre]", 841, 0, 62);
        AgregarColumnaDetalle(controlesDetalle, "[PoblacionNombre]", 903, 0, 62);
        AgregarColumnaDetalle(controlesDetalle, "[Beneficiario]", 965, 0, 83);
        AgregarColumnaDetalle(controlesDetalle, "[CajasEntregadas]", 1048, 0, 47);
        AgregarColumnaDetalle(controlesDetalle, "[OdcSapCosecha]", 1095, 0, 57);
        AgregarColumnaDetalle(controlesDetalle, "[OdcSapFlete]", 1152, 0, 57);
        AgregarColumnaDetalle(controlesDetalle, "[NumeroTelefono]", 1209, 0, 57);
        AgregarColumnaDetalle(controlesDetalle, "[Placas]", 1266, 0, 53);
        AgregarColumnaDetalle(controlesDetalle, "[Bascula]", 1319, 0, 67);
        AgregarColumnaDetalle(controlesDetalle, "[PuntoReunion]", 1386, 0, 94);
        AgregarColumnaDetalle(controlesDetalle, "[HoraLlegadaHuerta]", 1480, 0, 47);
        AgregarColumnaDetalle(controlesDetalle, "[CajasCosechadas]", 1527, 0, 53);
        AgregarColumnaDetalle(controlesDetalle, "[CajaPorCuadrilla]", 1580, 0, 67);

        controlesDetalle.Add(CrearLineaTextoLibre("Observaciones: ", "[Observaciones]", 20));
        controlesDetalle.Add(CrearLineaTextoLibre("Incidencias: ", "[Incidencias]", 36));
        controlesDetalle.Add(CrearLineaTextoLibre("Ajuste: ", "[Ajuste]", 52));

        var lineaSeparadora = new XRLine { LocationFloat = new PointFloat(0, 70), SizeF = new System.Drawing.SizeF(1647, 2) };
        controlesDetalle.Add(lineaSeparadora);

        //
        // _topMarginBand / _bottomMarginBand
        //
        _topMarginBand.HeightF = 20;
        _bottomMarginBand.HeightF = 20;

        //
        // _reportHeaderBand
        //
        _reportHeaderBand.HeightF = 148;
        _reportHeaderBand.Controls.AddRange(controlesEncabezado.ToArray());

        //
        // _detailBand (anidado dentro de _detailReportBand — ver comentario de InitializeComponent)
        //
        _detailBand.HeightF = 74;
        _detailBand.Controls.AddRange(controlesDetalle.ToArray());

        //
        // _detailReportBand — DataSource propio (ver ReporteIncidencias.cs), independiente del
        // DataSource de una fila que usa el reporte para el membrete/rango de fecha.
        //
        _detailReportBand.Bands.Add(_detailBand);

        //
        // ReporteIncidencias
        //
        Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { _topMarginBand, _reportHeaderBand, _detailReportBand, _bottomMarginBand });
        Font = new DXFont("Arial", 8);
        Landscape = true;
        PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Tabloid;
        Margins = new System.Drawing.Printing.Margins(15, 15, 20, 20);
    }

    // Cada etiqueta pinta su propio fondo gris (en vez de un panel aparte detrás de todas) — las
    // columnas son contiguas (sin huecos entre ellas), así que se ven como una sola franja gris
    // continua a lo largo de toda la fila, sin depender del orden de dibujo entre controles.
    private static XRLabel CrearEtiquetaEncabezado(string texto, float x, float y, float ancho) => new()
    {
        Text = texto,
        LocationFloat = new PointFloat(x, y),
        SizeF = new System.Drawing.SizeF(ancho, 28),
        Font = new DXFont("Arial", 7.5f, DXFontStyle.Bold),
        BackColor = System.Drawing.Color.LightGray,
    };

    private static void AgregarColumnaDetalle(List<DevExpress.XtraReports.UI.XRControl> controles, string expresion, float x, float y, float ancho, string? formato = null)
    {
        var etiqueta = new XRLabel
        {
            LocationFloat = new PointFloat(x, y),
            SizeF = new System.Drawing.SizeF(ancho, 16),
            CanShrink = true,
        };
        etiqueta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", expresion));
        if (formato is not null)
        {
            etiqueta.TextFormatString = formato;
        }

        controles.Add(etiqueta);
    }

    // Observaciones/Incidencias/Ajuste son texto libre (NVARCHAR(MAX)) — se imprimen a ancho
    // completo, con la etiqueta del campo concatenada al valor en la misma línea.
    private static XRLabel CrearLineaTextoLibre(string prefijo, string expresion, float y)
    {
        var etiqueta = new XRLabel
        {
            LocationFloat = new PointFloat(0, y),
            SizeF = new System.Drawing.SizeF(1647, 16),
            CanShrink = false,
        };
        etiqueta.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", $"'{prefijo}' + Iif(IsNullOrEmpty({expresion}), '', {expresion})"));
        return etiqueta;
    }
}
