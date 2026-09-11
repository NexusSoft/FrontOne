using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// "Carga de Contenedor": agrupado por pallet (Posicion), con una línea por lote dentro de cada
// pallet y un total por pallet — mismo criterio de DetailReportBand con DataSource propio que
// ReportePallet (el encabezado/membrete usan el DataSource de una fila del reporte; el detalle
// agrupado usa el DataSource propio de _detailReportBand, independiente).
public partial class ReporteContenedorCarga : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteContenedorCarga()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int contenedorId)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "ContenedorCarga",
            "Embarques.sp_Contenedor_ObtenerCargaParaReporte",
            new QueryParameter("@ContenedorId", typeof(int), contenedorId));

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

    public void CargarDatos(ContenedorDto encabezado, IReadOnlyList<ContenedorCargaReporteLineaDto> lineas, EmpresaConfiguracionDto empresa)
    {
        DataSource = new List<VistaEncabezadoContenedor> { ReporteContenedorComun.ArmarVista(encabezado, empresa) };
        DataMember = null;

        _detailReportBand.DataSource = lineas.ToList();
        _detailReportBand.DataMember = null;
    }
}
