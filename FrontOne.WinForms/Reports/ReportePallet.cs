using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;
using FrontOne.WinForms.Forms.Pallets;

namespace FrontOne.WinForms.Reports;

// Papeleta de Pallet. El encabezado/membrete/totales están enlazados declarativamente contra un
// DataSource de una fila (VistaEncabezado, ver CargarDatos); el detalle multi-línea vive en un
// DetailBand anidado dentro de _detailReportBand, que tiene su propio DataSource independiente
// (PalletDetalleDto) — ver comentario de ReportePallet.Designer.cs sobre por qué hace falta esa
// banda especial.
//
// ConectarOrigenDatos agrega, aparte, un SqlDataSource contra el SP del encabezado SOLO para que
// el Diseñador de Reportes muestre un Field List real. Regla dura heredada: nunca debe quedar
// pegado al reporte al momento de SaveLayoutToXml, o la contraseña de conexión terminaría escrita
// dentro de Configuracion.ReportePlantilla.DefinicionXml.
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

    // Combina el encabezado del pallet + la empresa (con los 2 campos de membrete que ya
    // requerían formato en C#) en un solo objeto de una fila — DataSource del reporte, para que
    // ReportePallet.Designer.cs enlace declarativamente encabezado/membrete/totales con rutas
    // anidadas ([Pallet.Campo]/[Empresa.Campo]). No aplana los DTOs originales, solo los agrupa.
    private sealed record VistaEncabezado(PalletReporteDto Pallet, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo);

    public void CargarDatos(PalletReporteDto datos, IReadOnlyList<PalletDetalleDto> detalle, EmpresaConfiguracionDto empresa)
    {
        // Lógica de negocio/transformación/nulos — no es mapeo 1:1 de columna, se queda manual.
        _lblStatus.Text = PalletsForm.NombreEstatus(datos.Estatus);
        _lblEsMixto.Text = datos.EsMixto ? "Sí" : "No";
        _lblPesoReal.Text = datos.PesoReal?.ToString("N2") ?? string.Empty;

        var vista = new VistaEncabezado(
            datos,
            empresa,
            string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
            string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))));

        // Resto del encabezado, membrete y totales enlazados declarativamente en el Designer.cs.
        DataSource = new List<VistaEncabezado> { vista };
        DataMember = null;

        // _detailReportBand tiene su propio DataSource, independiente del de arriba — el
        // DetailBand anidado adentro repite una fila por línea del pallet.
        _detailReportBand.DataSource = detalle.ToList();
        _detailReportBand.DataMember = null;
    }
}
