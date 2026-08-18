using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Reporte de Proceso y Liquidación para Productor: mismo encabezado/membrete que
// ReporteProcesoLote, pero el detalle de Categorías se filtra a solo Exportación+Nacional
// (sin Merma, ya excluida por el llamador) y la "Relación de Gastos" se filtra a solo lo que
// tiene CargoA = Productor (CAP) — el Importe a Pagar al Productor se precalcula en C# porque
// es una resta entre 2 sub-totales, no un mapeo 1:1 de columna.
public partial class ReporteLiquidacionProductor : XtraReport
{
    private SqlDataSource? _origenDatos;

    public ReporteLiquidacionProductor()
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

    private sealed record VistaEncabezado(
        GastoLoteReporteDto Lote, EmpresaConfiguracionDto Empresa, string Rfc, string TelefonoCorreo,
        decimal TotalGastosProductor, decimal ImporteAPagarProductor);

    public void CargarDatos(
        GastoLoteReporteDto datos,
        IReadOnlyList<GastoFrutaCategoriaLineaDto> categorias,
        IReadOnlyList<RelacionGastoDto> relacionGastos,
        EmpresaConfiguracionDto empresa)
    {
        var gastosProductor = relacionGastos.Where(g => g.CAP).ToList();
        var totalGastosProductor = gastosProductor.Sum(g => g.Importe);
        var importeAPagarProductor = categorias.Sum(c => c.ImporteReal) - totalGastosProductor;

        var vista = new VistaEncabezado(
            datos,
            empresa,
            string.IsNullOrWhiteSpace(empresa.Rfc) ? string.Empty : $"RFC: {empresa.Rfc}",
            string.Join(" · ", new[] { empresa.Telefono, empresa.Correo }.Where(v => !string.IsNullOrWhiteSpace(v))),
            totalGastosProductor,
            importeAPagarProductor);

        DataSource = new List<VistaEncabezado> { vista };
        DataMember = null;

        var unaFila = new List<object> { new() };
        _detailReportBandCategoriasEncabezado.DataSource = unaFila;
        _detailReportBandCategoriasEncabezado.DataMember = null;
        _detailReportBandGastosEncabezado.DataSource = unaFila;
        _detailReportBandGastosEncabezado.DataMember = null;

        _detailReportBandCategorias.DataSource = categorias.ToList();
        _detailReportBandCategorias.DataMember = null;

        _detailReportBandGastos.DataSource = gastosProductor;
        _detailReportBandGastos.DataMember = null;
    }
}
