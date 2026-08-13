using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Todas las etiquetas de valor (Bloque A/B, tabla, totales y membrete) están enlazadas
// declarativamente (ExpressionBindings en el Designer.cs, regla dura de CLAUDE.md) contra un
// DataSource de una fila (VistaEncabezado: Datos + Empresa + Rfc/TelefonoCorreo ya formateados,
// mismo patrón que ReportePallet/ReporteIncidencias) — CargarDatos solo arma ese wrapper y
// _lblNoCortadores, que no viene del SP del reporte.
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

    // Combina el encabezado de recepción + la empresa (con los 2 campos de membrete que ya
    // requerían formato en C#) en un solo objeto de una fila — DataSource del reporte, para que
    // ReporteRecepcionFruta.Designer.cs enlace declarativamente encabezado/membrete con rutas
    // anidadas ([Datos.Campo]/[Empresa.Campo]). No aplana los DTOs originales, solo los agrupa.
    private sealed record VistaEncabezado(RecepcionFrutaReporteDto Datos, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo);

    public void CargarDatos(RecepcionFrutaReporteDto datos, EmpresaConfiguracionDto empresa)
    {
        // Sin columna real en el SP — se queda hardcodeada, no se puede convertir a binding.
        _lblNoCortadores.Text = "0";

        var vista = new VistaEncabezado(
            datos,
            empresa,
            string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
            string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))));

        // Resto de las etiquetas de valor (Bloque A/B, tabla, totales, membrete, logo) están
        // enlazadas declarativamente en el Designer.cs (ExpressionBindings) — se resuelven solas
        // contra este DataSource al CreateDocument(), sin asignación manual de .Text/.Image.
        DataSource = new List<VistaEncabezado> { vista };
        DataMember = null;
    }
}
