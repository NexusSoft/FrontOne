using DevExpress.XtraEditors;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Reempaques;

partial class ReempaqueNeutroCapturaForm
{
    private IContainer components = null;

    private LabelControl _lblLote;
    private LookUpEdit _cmbLote;
    private LabelControl _lblKilosDisponibles;
    private TextEdit _txtKilosDisponibles;
    private LabelControl _lblProducto;
    private ComboBoxEdit _cmbProducto;
    private LabelControl _lblKilogramos;
    private SpinEdit _spnKilogramos;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReempaqueNeutroCapturaForm));
        _lblLote = new LabelControl();
        _cmbLote = new LookUpEdit();
        _lblKilosDisponibles = new LabelControl();
        _txtKilosDisponibles = new TextEdit();
        _lblProducto = new LabelControl();
        _cmbProducto = new ComboBoxEdit();
        _lblKilogramos = new LabelControl();
        _spnKilogramos = new SpinEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbLote.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosDisponibles.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).BeginInit();
        SuspendLayout();

        _lblLote.Location = new Point(12, 15);
        _lblLote.Name = "_lblLote";
        _lblLote.Size = new Size(90, 13);
        _lblLote.Text = "Lote a ajustar:";

        _cmbLote.Location = new Point(140, 12);
        _cmbLote.Name = "_cmbLote";
        _cmbLote.Size = new Size(220, 20);
        _cmbLote.TabIndex = 0;
        _cmbLote.Properties.NullText = "Seleccionar";
        _cmbLote.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbLote.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbLote.EditValueChanged += CmbLote_EditValueChanged;

        _lblKilosDisponibles.Location = new Point(12, 44);
        _lblKilosDisponibles.Name = "_lblKilosDisponibles";
        _lblKilosDisponibles.Size = new Size(90, 13);
        _lblKilosDisponibles.Text = "Kg pendientes:";

        _txtKilosDisponibles.Location = new Point(140, 41);
        _txtKilosDisponibles.Name = "_txtKilosDisponibles";
        _txtKilosDisponibles.Properties.ReadOnly = true;
        _txtKilosDisponibles.Size = new Size(120, 20);
        _txtKilosDisponibles.TabIndex = 1;

        _lblProducto.Location = new Point(12, 73);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(60, 13);
        _lblProducto.Text = "Producto:";

        _cmbProducto.Location = new Point(140, 70);
        _cmbProducto.Name = "_cmbProducto";
        _cmbProducto.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        _cmbProducto.Size = new Size(180, 20);
        _cmbProducto.TabIndex = 2;

        _lblKilogramos.Location = new Point(12, 102);
        _lblKilogramos.Name = "_lblKilogramos";
        _lblKilogramos.Size = new Size(60, 13);
        _lblKilogramos.Text = "Kilogramos:";

        _spnKilogramos.Location = new Point(140, 99);
        _spnKilogramos.Name = "_spnKilogramos";
        _spnKilogramos.Properties.Mask.EditMask = "n2";
        _spnKilogramos.Size = new Size(120, 20);
        _spnKilogramos.TabIndex = 3;

        _btnGuardar.Location = new Point(140, 140);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 4;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Click += BtnGuardar_Click;

        _btnCancelar.Location = new Point(230, 140);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 5;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Click += BtnCancelar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(384, 180);
        Controls.Add(_lblLote);
        Controls.Add(_cmbLote);
        Controls.Add(_lblKilosDisponibles);
        Controls.Add(_txtKilosDisponibles);
        Controls.Add(_lblProducto);
        Controls.Add(_cmbProducto);
        Controls.Add(_lblKilogramos);
        Controls.Add(_spnKilogramos);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ReempaqueNeutroCapturaForm";
        Text = "Ajuste (Merma / Diferencia a Favor)";
        StartPosition = FormStartPosition.CenterParent;
        ((System.ComponentModel.ISupportInitialize)_cmbLote.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosDisponibles.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
