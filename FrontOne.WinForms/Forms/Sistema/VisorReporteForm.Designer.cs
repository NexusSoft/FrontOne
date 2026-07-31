using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Control;

namespace FrontOne.WinForms.Forms.Sistema;

partial class VisorReporteForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private PrintControl _printControl;
    private SimpleButton _btnImprimir;
    private SimpleButton _btnExportarExcel;
    private SimpleButton _btnExportarPdf;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        _printControl = new PrintControl();
        _btnImprimir = new SimpleButton();
        _btnExportarExcel = new SimpleButton();
        _btnExportarPdf = new SimpleButton();
        _btnCerrar = new SimpleButton();
        SuspendLayout();
        //
        // _printControl
        //
        _printControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _printControl.Location = new Point(10, 10);
        _printControl.Name = "_printControl";
        _printControl.Size = new Size(760, 500);
        //
        // _btnImprimir
        //
        _btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnImprimir.Location = new Point(10, 520);
        _btnImprimir.Name = "_btnImprimir";
        _btnImprimir.Size = new Size(110, 23);
        _btnImprimir.Text = "Imprimir";
        _btnImprimir.Click += BtnImprimir_Click;
        //
        // _btnExportarExcel
        //
        _btnExportarExcel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnExportarExcel.Location = new Point(126, 520);
        _btnExportarExcel.Name = "_btnExportarExcel";
        _btnExportarExcel.Size = new Size(140, 23);
        _btnExportarExcel.Text = "Exportar a Excel";
        _btnExportarExcel.Click += BtnExportarExcel_Click;
        //
        // _btnExportarPdf
        //
        _btnExportarPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnExportarPdf.Location = new Point(272, 520);
        _btnExportarPdf.Name = "_btnExportarPdf";
        _btnExportarPdf.Size = new Size(140, 23);
        _btnExportarPdf.Text = "Exportar a PDF";
        _btnExportarPdf.Click += BtnExportarPdf_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(680, 520);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // VisorReporteForm
        //
        ClientSize = new Size(780, 553);
        Controls.Add(_printControl);
        Controls.Add(_btnImprimir);
        Controls.Add(_btnExportarExcel);
        Controls.Add(_btnExportarPdf);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(500, 400);
        Name = "VisorReporteForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Vista previa del reporte";
        ResumeLayout(false);
    }

    #endregion
}
