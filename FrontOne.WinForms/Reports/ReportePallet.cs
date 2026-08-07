using System.IO;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.WinForms.Forms.Pallets;

namespace FrontOne.WinForms.Reports;

// Papeleta de Pallet. A diferencia de ReporteRecepcionFruta (un solo registro, todo a mano), acá
// el detalle sí es multi-línea: el DetailBand se enlaza a la lista de PalletDetalleDto y las
// etiquetas del encabezado se llenan a mano en CargarDatos.
//
// ConectarOrigenDatos sigue el mismo criterio que ReporteRecepcionFruta: agrega un SqlDataSource
// contra el SP del encabezado SOLO para que el Diseñador de Reportes muestre un Field List real.
// Regla dura heredada: nunca debe quedar pegado al reporte al momento de SaveLayoutToXml, o la
// contraseña de conexión terminaría escrita dentro de Configuracion.ReportePlantilla.DefinicionXml.
public partial class ReportePallet : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReportePallet()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int id)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "Pallet",
            "Produccion.sp_Pallet_ObtenerParaReporte",
            new QueryParameter("@Id", typeof(int), id));

        ComponentStorage.Add(_origenDatos);
    }

    public void DesconectarOrigenDatos()
    {
        if (_origenDatos is null)
        {
            return;
        }

        ComponentStorage.Remove(_origenDatos);
        _origenDatos.Dispose();
        _origenDatos = null;
    }

    public void CargarDatos(PalletReporteDto datos, IReadOnlyList<PalletDetalleDto> detalle, EmpresaConfiguracionDto empresa)
    {
        _picLogo.Image = empresa.Logo is { Length: > 0 } ? ImagenDesdeBytes(empresa.Logo) : null;
        _lblRazonSocial.Text = empresa.RazonSocial;
        _lblDomicilio.Text = empresa.Domicilio;
        _lblRfc.Text = string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}";
        _lblTelefonoCorreo.Text = string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v)));

        _lblNoPallet.Text = datos.Folio;
        _lblFecha.Text = datos.FechaCreacion.ToString("dd/MM/yyyy");
        _lblHora.Text = datos.HoraCreacion.ToString(@"hh\:mm");
        _lblStatus.Text = PalletsForm.NombreEstatus(datos.Estatus);
        _lblLineaProduccion.Text = datos.LineaProduccionNombre;
        _lblEsMixto.Text = datos.EsMixto ? "Sí" : "No";
        _lblPorcentajeMateriaSeca.Text = datos.PorcentajeMateriaSeca.ToString("N2");
        _lblPesoReal.Text = datos.PesoReal?.ToString("N2") ?? string.Empty;
        _lblNoReempaque.Text = datos.NoReempaque;

        _lblTotalCajas.Text = datos.TotalCajas.ToString();
        _lblTotalKilogramos.Text = datos.TotalKilogramos.ToString("N2");

        // El DetailBand repite una fila por línea del pallet: el DataSource es la lista, y cada
        // etiqueta se enlaza por nombre de propiedad del DTO.
        DataSource = detalle.ToList();
        DataMember = null;
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
