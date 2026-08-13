using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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

    private LabelControl _lblTamanoPapel;
    private ComboBoxEdit _cmbTamanoPapel;
    private LabelControl _lblAvisoTamano;
    private PrintControl _printControl;
    private SimpleButton _btnImprimir;
    private SimpleButton _btnExportarExcel;
    private SimpleButton _btnExportarPdf;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        _lblTamanoPapel = new LabelControl();
        _cmbTamanoPapel = new ComboBoxEdit();
        _lblAvisoTamano = new LabelControl();
        _printControl = new PrintControl();
        _btnImprimir = new SimpleButton();
        _btnExportarExcel = new SimpleButton();
        _btnExportarPdf = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbTamanoPapel.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblTamanoPapel
        //
        _lblTamanoPapel.Location = new Point(10, 14);
        _lblTamanoPapel.Name = "_lblTamanoPapel";
        _lblTamanoPapel.Size = new Size(85, 13);
        _lblTamanoPapel.Text = "Tamaño de hoja:";
        //
        // _cmbTamanoPapel
        //
        _cmbTamanoPapel.Location = new Point(105, 10);
        _cmbTamanoPapel.Name = "_cmbTamanoPapel";
        _cmbTamanoPapel.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbTamanoPapel.Size = new Size(110, 20);
        _cmbTamanoPapel.EditValueChanged += CmbTamanoPapel_EditValueChanged;
        //
        // _lblAvisoTamano
        //
        _lblAvisoTamano.Appearance.ForeColor = Color.DarkRed;
        _lblAvisoTamano.Location = new Point(225, 14);
        _lblAvisoTamano.Name = "_lblAvisoTamano";
        _lblAvisoTamano.Size = new Size(545, 13);
        _lblAvisoTamano.Text = "El reporte fue diseñado para otro tamaño de hoja — el contenido puede recortarse o desalinearse.";
        _lblAvisoTamano.Visible = false;
        //
        // _printControl
        //
        _printControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _printControl.Location = new Point(10, 40);
        _printControl.Name = "_printControl";
        _printControl.Size = new Size(760, 470);
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
        Controls.Add(_lblTamanoPapel);
        Controls.Add(_cmbTamanoPapel);
        Controls.Add(_lblAvisoTamano);
        Controls.Add(_printControl);
        Controls.Add(_btnImprimir);
        Controls.Add(_btnExportarExcel);
        Controls.Add(_btnExportarPdf);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(500, 400);
        Name = "VisorReporteForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Vista previa del reporte";
        ((System.ComponentModel.ISupportInitialize)_cmbTamanoPapel.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
