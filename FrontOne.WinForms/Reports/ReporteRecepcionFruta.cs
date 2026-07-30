using System.IO;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Las ~30 etiquetas del layout default siguen llenándose "a mano" (CargarDatos) — sin cambios.
// En paralelo, ConectarOrigenDatos agrega un SqlDataSource (contra el mismo SP que ya arma este
// reporte) SOLO para que el Diseñador de Reportes muestre un Field List real y el usuario pueda
// arrastrar campos nuevos sin depender de un cambio de código cada vez; cualquier etiqueta nueva
// que se arrastre así queda data-bound y se resuelve sola. Regla dura: el SqlDataSource nunca
// debe quedar pegado al reporte al momento de SaveLayoutToXml (ver DesconectarOrigenDatos) —
// si no se quita antes de guardar, la contraseña de conexión quedaría escrita dentro de
// Configuracion.ReportePlantilla.DefinicionXml.
public partial class ReporteRecepcionFruta : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteRecepcionFruta()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int id)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "RecepcionFruta",
            "Recepcion.sp_RecepcionFruta_ObtenerParaReporte",
            new QueryParameter("@Id", typeof(int), id));

        ComponentStorage.Add(_origenDatos);
        DataSource = _origenDatos;
        DataMember = "RecepcionFruta";
    }

    public void DesconectarOrigenDatos()
    {
        if (_origenDatos is null)
        {
            return;
        }

        DataSource = null;
        DataMember = null;
        ComponentStorage.Remove(_origenDatos);
        _origenDatos.Dispose();
        _origenDatos = null;
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
