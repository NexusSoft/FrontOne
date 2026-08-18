using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Reporte de Proceso del módulo Gastos: encabezado/membrete de una fila (VistaEncabezado) +
// TRES secciones repetitivas independientes (Categorías de Fruta, Resumen por Mercado, Relación
// de Gastos de Cosecha/Acarreo), cada una en su propio DetailReportBand con su propio
// DataSource — mismo criterio que ReportePallet/ReporteIncidencias, extendido a 3 bandas en vez
// de 1 porque son 3 listas de forma distinta (Categoria/Mercado/Gasto), no una sola tabla.
public partial class ReporteProcesoLote : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteProcesoLote()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int gastoLoteId)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "GastoLote",
            "Gastos.sp_GastoLote_ObtenerParaReporte",
            new QueryParameter("@GastoLoteId", typeof(int), gastoLoteId));

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

    private sealed record VistaEncabezado(GastoLoteReporteDto Lote, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo);

    public void CargarDatos(
        GastoLoteReporteDto datos,
        IReadOnlyList<GastoFrutaCategoriaLineaDto> categorias,
        IReadOnlyList<ResumenMercadoDto> resumenMercado,
        IReadOnlyList<RelacionGastoDto> relacionGastos,
        EmpresaConfiguracionDto empresa)
    {
        var vista = new VistaEncabezado(
            datos,
            empresa,
            string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
            string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))));

        DataSource = new List<VistaEncabezado> { vista };
        DataMember = null;

        // Bandas de encabezado (título + columnas): 1 sola fila fija, solo para que impriman
        // exactamente una vez antes de la banda de datos correspondiente.
        var unaFila = new List<object> { new() };
        _detailReportBandCategoriasEncabezado.DataSource = unaFila;
        _detailReportBandCategoriasEncabezado.DataMember = null;
        _detailReportBandMercadoEncabezado.DataSource = unaFila;
        _detailReportBandMercadoEncabezado.DataMember = null;
        _detailReportBandGastosEncabezado.DataSource = unaFila;
        _detailReportBandGastosEncabezado.DataMember = null;

        _detailReportBandCategorias.DataSource = categorias.ToList();
        _detailReportBandCategorias.DataMember = null;

        _detailReportBandMercado.DataSource = resumenMercado.ToList();
        _detailReportBandMercado.DataMember = null;

        _detailReportBandGastos.DataSource = relacionGastos.ToList();
        _detailReportBandGastos.DataMember = null;
    }
}
