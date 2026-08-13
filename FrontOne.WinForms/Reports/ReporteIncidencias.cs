using DevExpress.DataAccess.Sql;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using DevExpress.XtraReports.UI;

namespace FrontOne.WinForms.Reports;

// PDF de las Incidencias (captura de campo por Orden de Corte) del rango de fecha elegido en
// IncidenciasForm. El membrete/rango de fecha están enlazados declarativamente contra el
// DataSource de una fila (VistaEncabezado, ver CargarDatos); las tarjetas de Incidencia (una fila
// de columnas + 3 líneas de texto libre por registro) viven en un DetailBand anidado dentro de
// _detailReportBand, que tiene su propio DataSource independiente (IncidenciaReporteDto) — ver
// comentario de ReporteIncidencias.Designer.cs sobre por qué hace falta esa banda especial.
//
// ConectarOrigenDatos agrega, aparte, un SqlDataSource contra el SP que llena este reporte SOLO
// para que el Diseñador de Reportes muestre un Field List real (regla dura, ver CLAUDE.md).
// Nunca debe quedar pegado al reporte al momento de SaveLayoutToXml, o la contraseña de conexión
// terminaría escrita dentro de Configuracion.ReportePlantilla.DefinicionXml.
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

    // Combina el rango de fecha ya formateado + la empresa (con los 2 campos de membrete que ya
    // requerían formato en C#) en un solo objeto de una fila — DataSource del reporte, para que
    // ReporteIncidencias.Designer.cs enlace declarativamente el membrete con ruta anidada
    // ([Empresa.Campo]). No aplana EmpresaConfiguracionDto, solo lo agrupa.
    private sealed record VistaEncabezado(string RangoFecha, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo);

    public void CargarDatos(DateTime fechaDesde, DateTime fechaHasta, IReadOnlyList<IncidenciaReporteDto> datos, EmpresaConfiguracionDto empresa)
    {
        var vista = new VistaEncabezado(
            $"Del {fechaDesde:dd/MM/yyyy} al {fechaHasta:dd/MM/yyyy}",
            empresa,
            string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
            string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))));

        // Membrete y rango de fecha enlazados declarativamente en el Designer.cs.
        DataSource = new List<VistaEncabezado> { vista };
        DataMember = null;

        // _detailReportBand tiene su propio DataSource, independiente del de arriba — el
        // DetailBand anidado adentro repite una tarjeta por Incidencia.
        _detailReportBand.DataSource = datos.ToList();
        _detailReportBand.DataMember = null;
    }
}
