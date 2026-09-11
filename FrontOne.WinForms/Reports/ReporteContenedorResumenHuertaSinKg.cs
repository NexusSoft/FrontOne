using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// "Resumen de Carga de Contenedor por Huerta (sin Kg)": mismo agrupado que
// ReporteContenedorResumenHuerta, sin la columna/total de Kilogramos (variante pedida aparte
// por el usuario, ej. para documentación donde no se declara peso).
public partial class ReporteContenedorResumenHuertaSinKg : XtraReport
{
    private SqlDataSource? _origenDatos;

    private sealed record FilaResumen(string? HuertaNombre, string? RegistroSagarpa, string? PoblacionNombre, string? Municipio, int Cajas);

    public ReporteContenedorResumenHuertaSinKg()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int contenedorId)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "ContenedorResumenHuertaSinKg",
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

        var filas = lineas
            .GroupBy(l => l.HuertaNombre)
            .OrderBy(g => g.Key)
            .Select(g => new FilaResumen(g.Key, g.First().RegistroSagarpa, g.First().PoblacionNombre, g.First().Municipio, g.Sum(x => x.Cajas)))
            .ToList();

        _detailReportBand.DataSource = filas;
        _detailReportBand.DataMember = null;
    }
}
