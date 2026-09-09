using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using FrontOne.Domain.DTOs;
using FrontOne.Shared.Configuration;

namespace FrontOne.WinForms.Reports;

// Todas las etiquetas de valor están enlazadas declarativamente (ExpressionBindings en el
// Designer.cs, regla dura de CLAUDE.md) directo contra RecepcionFrutaReporteDto — sin membrete de
// empresa no hace falta el wrapper VistaEncabezado que sí usa ReporteRecepcionFruta.
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

    public void CargarDatos(RecepcionFrutaReporteDto datos)
    {
        DataSource = new List<RecepcionFrutaReporteDto> { datos };
        DataMember = null;
    }
}
