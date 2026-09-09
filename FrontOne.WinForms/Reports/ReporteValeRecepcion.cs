using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Todas las etiquetas de valor (encabezado, tabla, totales y membrete) están enlazadas
// declarativamente (ExpressionBindings en el Designer.cs, regla dura de CLAUDE.md) contra un
// DataSource de una fila (VistaEncabezado: Datos + Empresa + Rfc/TelefonoCorreo ya formateados),
// mismo patrón que ReporteRecepcionFruta — CargarDatos solo arma ese wrapper.
// ConectarOrigenDatos usa el mismo SP que ReporteRecepcionFruta (Recepcion.sp_RecepcionFruta_
// ObtenerParaReporte) — ambos reportes son layouts distintos sobre el mismo origen de datos.
public partial class ReporteValeRecepcion : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteValeRecepcion()
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

    // Combina el encabezado del vale + la empresa (con los 2 campos de membrete que ya requerían
    // formato en C#) en un solo objeto de una fila — mismo wrapper que ReporteRecepcionFruta.
    private sealed record VistaEncabezado(RecepcionFrutaReporteDto Datos, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo);

    public void CargarDatos(RecepcionFrutaReporteDto datos, EmpresaConfiguracionDto empresa)
    {
        var vista = new VistaEncabezado(
            datos,
            empresa,
            string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
            string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))));

        DataSource = new List<VistaEncabezado> { vista };
        DataMember = null;
    }
}
