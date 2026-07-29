using System.IO;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Reports;

// No usa databinding real (report.DataSource) — siempre es un solo registro (regla de negocio:
// una Orden de Corte por Recepción), así que CargarDatos asigna los valores directo a los
// labels. El layout de este archivo es el default; si alguien lo edita con el Diseñador de
// Reportes, ese layout guardado en Configuracion.ReportePlantilla se aplica encima con
// LoadLayoutFromXml antes de llamar a CargarDatos — ver RecepcionesFrutaForm.BtnVistaPrevia_Click.
public partial class ReporteRecepcionFruta : XtraReport
{
    public ReporteRecepcionFruta()
    {
        InitializeComponent();
    }

    public void CargarDatos(RecepcionFrutaReporteDto datos, EmpresaConfiguracionDto empresa)
    {
        _picLogo.Image = empresa.Logo is { Length: > 0 } ? ImagenDesdeBytes(empresa.Logo) : null;
        _lblRazonSocial.Text = empresa.RazonSocial;
        _lblDomicilio.Text = empresa.Domicilio;
        _lblRfc.Text = string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}";
        _lblTelefonoCorreo.Text = string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v)));

        _lblNoLote.Text = datos.NoLote;
        _lblFecha.Text = datos.Fecha.ToString("dd/MM/yyyy");
        _lblChofer.Text = datos.Chofer;
        _lblPlacas.Text = datos.Placas;
        _lblObservaciones.Text = datos.Observaciones;
        _lblTicket.Text = datos.NumeroTicket;
        _lblPesoBruto.Text = datos.PesoBruto.ToString("N2");
        _lblPesoTara.Text = datos.PesoTara.ToString("N2");
        _lblPesoMuestra.Text = datos.PesoMuestra.ToString("N2");
        _lblPesoNeto.Text = datos.PesoNeto.ToString("N2");

        _lblHuerta.Text = datos.HuertaNombre;
        _lblProductor.Text = datos.ProductorNombre;
        _lblTipoCorte.Text = datos.TipoCorteNombre;
        _lblNoAcuerdo.Text = datos.AcuerdoCorteFolio;
        _lblTransportista.Text = datos.TransportistaNombre;
        _lblEmpresaCorte.Text = datos.EmpresaCorteNombre;
        _lblNoCandado.Text = datos.NoCandado;
        _lblObservacionesOrden.Text = datos.OrdenObservaciones;
        _lblCajasEntregadas.Text = datos.CajasPorEntregar.ToString();
        _lblCajasCortadas.Text = datos.CajasCortadas.ToString();
        _lblCajasRecibidasVacias.Text = datos.CajasRecibidasVacias.ToString();
        _lblDiferencia.Text = datos.CajasDiferencia.ToString();
        _lblNoCortadores.Text = "0";

        _lblProducto.Text = datos.ProductoNombre;
        _lblVariedad.Text = datos.VariedadNombre;
        _lblCajasTabla.Text = datos.CajasCortadas.ToString();
        _lblKilogramosTabla.Text = datos.Kilogramos.ToString("N2");
        _lblTotalCajas.Text = datos.CajasCortadas.ToString();
        _lblTotalKilogramos.Text = datos.Kilogramos.ToString("N2");
    }

    // Image.FromStream requiere que el stream permanezca abierto durante toda la vida de la
    // imagen — mismo patrón que ConfiguracionEmpresaForm.ImagenDesdeBytes.
    private static Image ImagenDesdeBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var original = Image.FromStream(stream);
        return new Bitmap(original);
    }
}
