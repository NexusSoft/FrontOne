using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// "Detalle de Carga de Contenedor por Lote": agrupado por Lote, con un renglón por pallet que
// contiene ese lote y un total por lote.
public partial class ReporteContenedorDetallePorLote : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteContenedorDetallePorLote()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int contenedorId)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "ContenedorDetalleLote",
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

        // Se ordena por Lote (no por Posición, como trae la SP) para que el agrupado quede
        // contiguo — DevExpress agrupa por cambio de valor consecutivo, no reordena solo.
        _detailReportBand.DataSource = lineas.OrderBy(l => l.LoteFolio).ThenBy(l => l.Posicion).ToList();
        _detailReportBand.DataMember = null;
    }
}
