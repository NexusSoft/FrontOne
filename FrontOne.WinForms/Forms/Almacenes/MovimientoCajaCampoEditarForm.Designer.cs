using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Almacenes;

partial class MovimientoCajaCampoEditarForm
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

    private LabelControl _lblCajaCampo;
    private LookUpEdit _cmbCajaCampo;
    private LabelControl _lblCantidad;
    private SpinEdit _spnCantidad;
    private LabelControl _lblObservaciones;
    private MemoEdit _txtObservaciones;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovimientoCajaCampoEditarForm));
        _lblCajaCampo = new LabelControl();
        _cmbCajaCampo = new LookUpEdit();
        _lblCantidad = new LabelControl();
        _spnCantidad = new SpinEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new MemoEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbCajaCampo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCantidad.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblCajaCampo
        //
        _lblCajaCampo.Location = new Point(20, 20);
        _lblCajaCampo.Name = "_lblCajaCampo";
        _lblCajaCampo.Size = new Size(80, 13);
        _lblCajaCampo.TabIndex = 0;
        _lblCajaCampo.Text = "Color de Caja:";
        //
        // _cmbCajaCampo
        //
        _cmbCajaCampo.Location = new Point(150, 17);
        _cmbCajaCampo.Name = "_cmbCajaCampo";
        _cmbCajaCampo.Properties.NullText = "Seleccionar";
        _cmbCajaCampo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus) });
        _cmbCajaCampo.Size = new Size(220, 20);
        _cmbCajaCampo.TabIndex = 1;
        _cmbCajaCampo.ButtonClick += CmbCajaCampo_ButtonClick;
        //
        // _lblCantidad
        //
        _lblCantidad.Location = new Point(20, 55);
        _lblCantidad.Name = "_lblCantidad";
        _lblCantidad.Size = new Size(46, 13);
        _lblCantidad.TabIndex = 2;
        _lblCantidad.Text = "Cantidad:";
        //
        // _spnCantidad
        //
        _spnCantidad.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCantidad.Location = new Point(150, 52);
        _spnCantidad.Name = "_spnCantidad";
        _spnCantidad.Properties.Mask.EditMask = "N00";
        _spnCantidad.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCantidad.Properties.MaxValue = new decimal(new int[] { 32000, 0, 0, 0 });
        _spnCantidad.Size = new Size(120, 20);
        _spnCantidad.TabIndex = 3;
        //
        // _lblObservaciones
        //
        _lblObservaciones.Location = new Point(20, 90);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(74, 13);
        _lblObservaciones.TabIndex = 4;
        _lblObservaciones.Text = "Observaciones:";
        //
        // _txtObservaciones
        //
        _txtObservaciones.Location = new Point(150, 88);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(220, 60);
        _txtObservaciones.TabIndex = 5;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(200, 165);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 6;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(290, 165);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 7;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // MovimientoCajaCampoEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(390, 205);
        Controls.Add(_lblCajaCampo);
        Controls.Add(_cmbCajaCampo);
        Controls.Add(_lblCantidad);
        Controls.Add(_spnCantidad);
        Controls.Add(_lblObservaciones);
        Controls.Add(_txtObservaciones);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MovimientoCajaCampoEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Movimiento de Caja de Campo";
        ((System.ComponentModel.ISupportInitialize)_cmbCajaCampo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCantidad.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
