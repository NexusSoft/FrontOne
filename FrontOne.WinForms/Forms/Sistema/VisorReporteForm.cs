using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using FrontOne.Shared.Constants;
using FrontOne.WinForms.Reports;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms.Sistema;

// Visor propio en vez de ReportPrintTool.ShowPreviewDialog() — ese diálogo trae Imprimir y
// Exportar ya integrados en un solo ribbon, sin forma documentada de ocultar botones
// individuales sin depender de personalización interna de DevExpress (mismo criterio ya usado
// en DisenadorReporteForm). Acá los 3 botones se muestran/ocultan según el permiso de reporte
// del usuario en sesión — si no tiene ni Impresión ni Exportación, solo queda el visor.
public partial class VisorReporteForm : XtraForm
{
    private readonly XtraReport _reporte = null!;
    private TamanoPapelReporte _tamanoInicial;

    public VisorReporteForm()
    {
        InitializeComponent();
    }

    public VisorReporteForm(XtraReport reporte, string reporteCodigo, SessionContext sessionContext)
        : this()
    {
        _reporte = reporte;

        _reporte.CreateDocument();
        _printControl.PrintingSystem = _reporte.PrintingSystem;

        _tamanoInicial = TamanoPapelReporteExtensions.DesdeReporte(_reporte) ?? TamanoPapelReporte.Carta;
        _cmbTamanoPapel.Properties.Items.AddRange(Enum.GetValues<TamanoPapelReporte>());
        _cmbTamanoPapel.EditValue = _tamanoInicial;

        _btnImprimir.Visible = sessionContext.TienePermisoReporte(reporteCodigo, AccionReporte.Impresion);
        _btnExportarExcel.Visible = sessionContext.TienePermisoReporte(reporteCodigo, AccionReporte.Exportacion);
        _btnExportarPdf.Visible = sessionContext.TienePermisoReporte(reporteCodigo, AccionReporte.Exportacion);
    }

    private void CmbTamanoPapel_EditValueChanged(object? sender, EventArgs e)
    {
        if (_cmbTamanoPapel.EditValue is not TamanoPapelReporte tamano)
        {
            return;
        }

        tamano.AplicarA(_reporte);
        _reporte.CreateDocument();
        _printControl.PrintingSystem = _reporte.PrintingSystem;

        _lblAvisoTamano.Visible = tamano != _tamanoInicial;
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
        => new DevExpress.XtraReports.UI.ReportPrintTool(_reporte).PrintDialog();

    private void BtnExportarExcel_Click(object? sender, EventArgs e)
    {
        using var dialogo = new SaveFileDialog { Title = "Exportar a Excel", Filter = "Excel (*.xlsx)|*.xlsx" };
        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _reporte.ExportToXlsx(dialogo.FileName);
    }

    private void BtnExportarPdf_Click(object? sender, EventArgs e)
    {
        using var dialogo = new SaveFileDialog { Title = "Exportar a PDF", Filter = "PDF (*.pdf)|*.pdf" };
        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _reporte.ExportToPdf(dialogo.FileName);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
