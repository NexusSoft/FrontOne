using System.IO;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// PDF de las Incidencias (captura de campo por Orden de Corte) del rango de fecha elegido en
// IncidenciasForm. A diferencia de ReportePallet (un encabezado + detalle de líneas), acá cada
// Incidencia es "el detalle" — el DataSource es la lista completa de IncidenciaReporteDto y el
// DetailBand repite, por registro, dos filas de columnas (datos de la Orden de Corte y datos
// propios de la Incidencia) más tres líneas de texto libre (Observaciones/Incidencias/Ajuste).
public partial class ReporteIncidencias : XtraReport
{
    public ReporteIncidencias()
    {
        InitializeComponent();
    }

    public void CargarDatos(DateTime fechaDesde, DateTime fechaHasta, IReadOnlyList<IncidenciaReporteDto> datos, EmpresaConfiguracionDto empresa)
    {
        _picLogo.Image = empresa.Logo is { Length: > 0 } ? ImagenDesdeBytes(empresa.Logo) : null;
        _lblRazonSocial.Text = empresa.RazonSocial;
        _lblDomicilio.Text = empresa.Domicilio;
        _lblRfc.Text = string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}";
        _lblTelefonoCorreo.Text = string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v)));
        _lblRangoFecha.Text = $"Del {fechaDesde:dd/MM/yyyy} al {fechaHasta:dd/MM/yyyy}";

        DataSource = datos.ToList();
        DataMember = null;
    }

    // Image.FromStream requiere que el stream permanezca abierto durante toda la vida de la
    // imagen — mismo patrón que ReportePallet.ImagenDesdeBytes.
    private static Image ImagenDesdeBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var original = Image.FromStream(stream);
        return new Bitmap(original);
    }
}
