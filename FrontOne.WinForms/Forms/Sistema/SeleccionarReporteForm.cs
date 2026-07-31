using DevExpress.XtraEditors;
using FrontOne.Application.Services;
using FrontOne.WinForms.Reports;

namespace FrontOne.WinForms.Forms.Sistema;

// Ventana estilo "Selección de diseño de impresión" (SAP) — se muestra al hacer clic en Vista
// Previa cuando la pantalla tiene más de un reporte disponible (ver CatalogoReportes.Todos).
public partial class SeleccionarReporteForm : XtraForm
{
    private readonly IReadOnlyList<ReporteDisponible> _reportes = [];

    private record FilaReporte(string Codigo, string Nombre, bool Predeterminado);

    public string? CodigoSeleccionado { get; private set; }

    public SeleccionarReporteForm()
    {
        InitializeComponent();
    }

    public SeleccionarReporteForm(ReportePlantillaService reportePlantillaService, IReadOnlyList<ReporteDisponible> reportes)
        : this()
    {
        _reportes = reportes;

        Load += async (_, _) => await CargarDatosAsync(reportePlantillaService);
    }

    private async Task CargarDatosAsync(ReportePlantillaService reportePlantillaService)
    {
        var filas = new List<FilaReporte>();
        foreach (var reporte in _reportes)
        {
            var plantilla = await reportePlantillaService.ObtenerPorCodigoAsync(reporte.Codigo);
            filas.Add(new FilaReporte(reporte.Codigo, reporte.Nombre, plantilla?.EsPredeterminado ?? false));
        }

        _grid.DataSource = filas;
        _gridView.Columns["Codigo"].Visible = false;
        _gridView.Columns["Nombre"].Caption = "Reporte";
        _gridView.Columns["Predeterminado"].Caption = "Predeterminado";
        _gridView.BestFitColumns();

        var indicePredeterminado = filas.FindIndex(f => f.Predeterminado);
        _gridView.FocusedRowHandle = indicePredeterminado >= 0 ? indicePredeterminado : 0;
    }

    private void GridView_DoubleClick(object? sender, EventArgs e) => Seleccionar();

    private void BtnSeleccionar_Click(object? sender, EventArgs e) => Seleccionar();

    private void Seleccionar()
    {
        if (_gridView.GetFocusedRow() is not FilaReporte fila)
        {
            XtraMessageBox.Show(this, "Selecciona un reporte.", "FrontOne", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        CodigoSeleccionado = fila.Codigo;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
