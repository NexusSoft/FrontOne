using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting.Control;

namespace FrontOne.WinForms.Forms.Pallets;

partial class PalletImprimirEtiquetaForm
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

    private LabelControl _lblEtiqueta;
    private LookUpEdit _cmbEtiqueta;
    private LabelControl _lblImpresora;
    private LookUpEdit _cmbImpresora;
    private LabelControl _lblCantidad;
    private SpinEdit _txtCantidad;
    private PrintControl _printControl;
    private SimpleButton _btnImprimir;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        _lblEtiqueta = new LabelControl();
        _cmbEtiqueta = new LookUpEdit();
        _lblImpresora = new LabelControl();
        _cmbImpresora = new LookUpEdit();
        _lblCantidad = new LabelControl();
        _txtCantidad = new SpinEdit();
        _printControl = new PrintControl();
        _btnImprimir = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbEtiqueta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbImpresora.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCantidad.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblEtiqueta
        //
        _lblEtiqueta.Location = new Point(10, 15);
        _lblEtiqueta.Name = "_lblEtiqueta";
        _lblEtiqueta.Size = new Size(120, 13);
        _lblEtiqueta.Text = "Etiqueta:";
        //
        // _cmbEtiqueta
        //
        _cmbEtiqueta.Location = new Point(140, 11);
        _cmbEtiqueta.Name = "_cmbEtiqueta";
        _cmbEtiqueta.Properties.NullText = "Seleccionar";
        _cmbEtiqueta.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbEtiqueta.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbEtiqueta.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbEtiqueta.Size = new Size(320, 20);
        _cmbEtiqueta.EditValueChanged += CmbEtiqueta_EditValueChanged;
        //
        // _lblImpresora
        //
        _lblImpresora.Location = new Point(10, 44);
        _lblImpresora.Name = "_lblImpresora";
        _lblImpresora.Size = new Size(120, 13);
        _lblImpresora.Text = "Impresora:";
        //
        // _cmbImpresora
        //
        _cmbImpresora.Location = new Point(140, 40);
        _cmbImpresora.Name = "_cmbImpresora";
        _cmbImpresora.Properties.NullText = "Seleccionar";
        _cmbImpresora.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbImpresora.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbImpresora.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbImpresora.Size = new Size(320, 20);
        _cmbImpresora.EditValueChanged += CmbImpresora_EditValueChanged;
        //
        // _lblCantidad
        //
        _lblCantidad.Location = new Point(10, 72);
        _lblCantidad.Name = "_lblCantidad";
        _lblCantidad.Size = new Size(120, 13);
        _lblCantidad.Text = "No. de etiquetas:";
        //
        // _txtCantidad
        //
        _txtCantidad.Location = new Point(140, 68);
        _txtCantidad.Name = "_txtCantidad";
        _txtCantidad.Properties.MinValue = 1;
        _txtCantidad.Properties.IsFloatValue = false;
        _txtCantidad.Properties.Mask.EditMask = "N00";
        _txtCantidad.Size = new Size(80, 20);
        _txtCantidad.EditValueChanged += TxtCantidad_EditValueChanged;
        //
        // _printControl
        //
        _printControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _printControl.Location = new Point(10, 100);
        _printControl.Name = "_printControl";
        _printControl.Size = new Size(660, 440);
        //
        // _btnImprimir
        //
        _btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnImprimir.Enabled = false;
        _btnImprimir.Location = new Point(10, 550);
        _btnImprimir.Name = "_btnImprimir";
        _btnImprimir.Size = new Size(110, 23);
        _btnImprimir.Text = "Imprimir";
        _btnImprimir.Click += BtnImprimir_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(560, 550);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(110, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // PalletImprimirEtiquetaForm
        //
        ClientSize = new Size(680, 585);
        Controls.Add(_lblEtiqueta);
        Controls.Add(_cmbEtiqueta);
        Controls.Add(_lblImpresora);
        Controls.Add(_cmbImpresora);
        Controls.Add(_lblCantidad);
        Controls.Add(_txtCantidad);
        Controls.Add(_printControl);
        Controls.Add(_btnImprimir);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(500, 400);
        Name = "PalletImprimirEtiquetaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Imprimir etiqueta de trazabilidad";
        ((System.ComponentModel.ISupportInitialize)_cmbEtiqueta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbImpresora.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCantidad.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
