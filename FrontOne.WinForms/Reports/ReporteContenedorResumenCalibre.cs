using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// "Resumen de Carga de Contenedor por Calibre": un renglón por pallet (no por línea de lote) —
// se pre-agrega en CargarDatos con LINQ, no en la SP, porque comparte la misma SP plana que los
// demás reportes de Contenedor (ver Embarques.sp_Contenedor_ObtenerCargaParaReporte).
public partial class ReporteContenedorResumenCalibre : XtraReport
{
    private SqlDataSource? _origenDatos;

    private sealed record FilaResumen(
        int Numero, string Identificador, string PalletFolio, DateTime Fecha,
        string? Calibre, decimal? Temperatura, int Cajas, decimal Kilogramos, string? Marca);

    public ReporteContenedorResumenCalibre()
    {
        InitializeComponent();
    }

    public void ConectarOrigenDatos(SqlOptions sqlOptions, int contenedorId)
    {
        DesconectarOrigenDatos();

        _origenDatos = ReporteConexionSql.CrearOrigenDatos(
            sqlOptions,
            "ContenedorResumenCalibre",
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
            .GroupBy(l => l.Posicion)
            .Select(g => new
            {
                Posicion = g.Key,
                Folio = g.First().PalletFolio,
                Fecha = g.First().PalletFechaCreacion,
                Calibre = g.First().CalibreCodigoExterno,
                Temperatura = g.First().Temperatura,
                Cajas = g.Sum(x => x.Cajas),
                Kilogramos = g.Sum(x => x.Kilogramos),
                Marca = g.First().MarcaNombre,
            })
            .OrderBy(f => f.Calibre)
            .ThenBy(f => f.Posicion)
            .Select((f, i) => new FilaResumen(
                i + 1,
                $"PALLET {f.Posicion:00}",
                f.Folio,
                f.Fecha,
                f.Calibre,
                f.Temperatura,
                f.Cajas,
                f.Kilogramos,
                f.Marca))
            .ToList();

        _detailReportBand.DataSource = filas;
        _detailReportBand.DataMember = null;
    }
}
