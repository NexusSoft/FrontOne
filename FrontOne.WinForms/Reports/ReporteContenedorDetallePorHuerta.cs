using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// "Detalle de Carga de Contenedor por Huerta": agrupado por Huerta, con un renglón por línea de
// lote/pallet dentro de esa huerta y un total por huerta.
public partial class ReporteContenedorDetallePorHuerta : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteContenedorDetallePorHuerta()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int contenedorId)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "ContenedorDetalleHuerta",
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

        _detailReportBand.DataSource = lineas.OrderBy(l => l.HuertaNombre).ThenBy(l => l.LoteFolio).ToList();
        _detailReportBand.DataMember = null;
    }
}
