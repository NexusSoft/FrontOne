using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Corridas;

partial class PalletNeutroCapturaForm
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

    private LabelControl _lblLote;
    private TextEdit _txtLote;
    private LabelControl _lblKilosRestantes;
    private TextEdit _txtKilosRestantes;
    private LabelControl _lblProducto;
    private ComboBoxEdit _cmbProducto;
    private LabelControl _lblKilogramos;
    private SpinEdit _spnKilogramos;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PalletNeutroCapturaForm));
        _lblLote = new LabelControl();
        _txtLote = new TextEdit();
        _lblKilosRestantes = new LabelControl();
        _txtKilosRestantes = new TextEdit();
        _lblProducto = new LabelControl();
        _cmbProducto = new ComboBoxEdit();
        _lblKilogramos = new LabelControl();
        _spnKilogramos = new SpinEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtLote.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosRestantes.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblLote
        //
        _lblLote.Location = new Point(15, 18);
        _lblLote.Name = "_lblLote";
        _lblLote.Size = new Size(60, 13);
        _lblLote.Text = "No. de Lote:";
        //
        // _txtLote
        //
        _txtLote.Location = new Point(150, 15);
        _txtLote.Name = "_txtLote";
        _txtLote.Properties.ReadOnly = true;
        _txtLote.Size = new Size(150, 20);
        _txtLote.TabIndex = 0;
        //
        // _lblKilosRestantes
        //
        _lblKilosRestantes.Location = new Point(15, 46);
        _lblKilosRestantes.Name = "_lblKilosRestantes";
        _lblKilosRestantes.Size = new Size(80, 13);
        _lblKilosRestantes.Text = "Kilos Restantes:";
        //
        // _txtKilosRestantes
        //
        _txtKilosRestantes.Location = new Point(150, 43);
        _txtKilosRestantes.Name = "_txtKilosRestantes";
        _txtKilosRestantes.Properties.ReadOnly = true;
        _txtKilosRestantes.Size = new Size(150, 20);
        _txtKilosRestantes.TabIndex = 1;
        //
        // _lblProducto
        //
        _lblProducto.Location = new Point(15, 74);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(45, 13);
        _lblProducto.Text = "Producto:";
        //
        // _cmbProducto
        //
        _cmbProducto.Location = new Point(150, 71);
        _cmbProducto.Name = "_cmbProducto";
        _cmbProducto.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbProducto.Size = new Size(200, 20);
        _cmbProducto.TabIndex = 2;
        //
        // _lblKilogramos
        //
        _lblKilogramos.Location = new Point(15, 102);
        _lblKilogramos.Name = "_lblKilogramos";
        _lblKilogramos.Size = new Size(60, 13);
        _lblKilogramos.Text = "Kilogramos:";
        //
        // _spnKilogramos
        //
        _spnKilogramos.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnKilogramos.Location = new Point(150, 99);
        _spnKilogramos.Name = "_spnKilogramos";
        _spnKilogramos.Properties.Mask.EditMask = "N02";
        _spnKilogramos.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnKilogramos.Properties.MaxValue = new decimal(new int[] { 999999, 0, 0, 0 });
        _spnKilogramos.Size = new Size(150, 20);
        _spnKilogramos.TabIndex = 3;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(190, 140);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 4;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(280, 140);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 5;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // PalletNeutroCapturaForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(375, 178);
        Controls.Add(_lblLote);
        Controls.Add(_txtLote);
        Controls.Add(_lblKilosRestantes);
        Controls.Add(_txtKilosRestantes);
        Controls.Add(_lblProducto);
        Controls.Add(_cmbProducto);
        Controls.Add(_lblKilogramos);
        Controls.Add(_spnKilogramos);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PalletNeutroCapturaForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Capturar Diferencia";
        ((System.ComponentModel.ISupportInitialize)_txtLote.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosRestantes.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
