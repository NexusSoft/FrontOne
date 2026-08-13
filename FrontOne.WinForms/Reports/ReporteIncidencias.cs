using System.IO;
using DevExpress.DataAccess.Sql;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// PDF de las Incidencias (captura de campo por Orden de Corte) del rango de fecha elegido en
// IncidenciasForm. A diferencia de ReportePallet (un encabezado + detalle de líneas), acá cada
// Incidencia es "el detalle" — el DataSource es la lista completa de IncidenciaReporteDto y el
// DetailBand repite, por registro, dos filas de columnas (datos de la Orden de Corte y datos
// propios de la Incidencia) más tres líneas de texto libre (Observaciones/Incidencias/Ajuste).
//
// ConectarOrigenDatos sigue el mismo criterio que ReportePallet: agrega un SqlDataSource contra
// el SP que llena este reporte SOLO para que el Diseñador de Reportes muestre un Field List real
// (regla dura, ver CLAUDE.md). Como CargarDatos ya asigna DataSource = lista.ToList() directo
// (igual que Pallet, no como RecepcionFruta), acá NO se toca DataSource/DataMember — el
// SqlDataSource solo se agrega a ComponentStorage. Nunca debe quedar pegado al reporte al momento
// de SaveLayoutToXml, o la contraseña de conexión terminaría escrita dentro de
// Configuracion.ReportePlantilla.DefinicionXml.
public partial class ReporteIncidencias : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteIncidencias()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, DateTime fechaDesde, DateTime fechaHasta)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "Incidencias",
            "Acopio.sp_Incidencia_ObtenerParaReporte",
            new QueryParameter("@FechaDesde", typeof(DateTime), fechaDesde),
            new QueryParameter("@FechaHasta", typeof(DateTime), fechaHasta));

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
