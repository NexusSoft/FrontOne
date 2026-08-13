using System.IO;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Las etiquetas de valor del layout default están enlazadas declarativamente (ExpressionBindings
// en el Designer.cs, regla dura de CLAUDE.md) — CargarDatos solo asigna el DataSource real
// (una lista de un elemento, ya que siempre es un solo registro) más las etiquetas de membrete de
// empresa y _lblNoCortadores, que no vienen del SP del reporte.
// ConectarOrigenDatos agrega, aparte, un SqlDataSource (contra el mismo SP) SOLO para que el
// Diseñador de Reportes muestre un Field List real y el usuario pueda arrastrar campos nuevos sin
// depender de un cambio de código cada vez. Regla dura: el SqlDataSource nunca debe quedar
// pegado al reporte al momento de SaveLayoutToXml (ver DesconectarOrigenDatos) — si no se quita
// antes de guardar, la contraseña de conexión quedaría escrita dentro de
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

        // Sin columna real en el SP — se queda hardcodeada, no se puede convertir a binding.
        _lblNoCortadores.Text = "0";

        // El resto de las etiquetas de valor (Bloque A/B, tabla, totales) están enlazadas
        // declarativamente en el Designer.cs (ExpressionBindings) — se resuelven solas contra
        // este DataSource al CreateDocument(), sin asignación manual de .Text.
        DataSource = new List<RecepcionFrutaReporteDto> { datos };
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
