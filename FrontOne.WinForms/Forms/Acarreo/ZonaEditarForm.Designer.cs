using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Acarreo;

partial class ZonaEditarForm
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

    private LabelControl _lblNombre;
    private TextEdit _txtNombre;
    private LabelControl _lblKgMinimo300;
    private SpinEdit _txtKgMinimo300;
    private LabelControl _lblKgMinimo400;
    private SpinEdit _txtKgMinimo400;
    private LabelControl _lblKgMinimo500;
    private SpinEdit _txtKgMinimo500;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZonaEditarForm));
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _lblKgMinimo300 = new LabelControl();
        _txtKgMinimo300 = new SpinEdit();
        _lblKgMinimo400 = new LabelControl();
        _txtKgMinimo400 = new SpinEdit();
        _lblKgMinimo500 = new LabelControl();
        _txtKgMinimo500 = new SpinEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo300.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo400.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo500.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 20);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(28, 13);
        _lblNombre.TabIndex = 0;
        _lblNombre.Text = "Zona:";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(130, 18);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(220, 20);
        _txtNombre.TabIndex = 1;
        //
        // _lblKgMinimo300
        //
        _lblKgMinimo300.Location = new Point(20, 50);
        _lblKgMinimo300.Name = "_lblKgMinimo300";
        _lblKgMinimo300.Size = new Size(80, 13);
        _lblKgMinimo300.TabIndex = 2;
        _lblKgMinimo300.Text = "Kg MÃ­n. 300 Cajas:";
        //
        // _txtKgMinimo300
        //
        _txtKgMinimo300.Location = new Point(130, 48);
        _txtKgMinimo300.Name = "_txtKgMinimo300";
        _txtKgMinimo300.Properties.Mask.EditMask = "N02";
        _txtKgMinimo300.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _txtKgMinimo300.Size = new Size(120, 20);
        _txtKgMinimo300.TabIndex = 3;
        //
        // _lblKgMinimo400
        //
        _lblKgMinimo400.Location = new Point(20, 80);
        _lblKgMinimo400.Name = "_lblKgMinimo400";
        _lblKgMinimo400.Size = new Size(80, 13);
        _lblKgMinimo400.TabIndex = 4;
        _lblKgMinimo400.Text = "Kg MÃ­n. 400 Cajas:";
        //
        // _txtKgMinimo400
        //
        _txtKgMinimo400.Location = new Point(130, 78);
        _txtKgMinimo400.Name = "_txtKgMinimo400";
        _txtKgMinimo400.Properties.Mask.EditMask = "N02";
        _txtKgMinimo400.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _txtKgMinimo400.Size = new Size(120, 20);
        _txtKgMinimo400.TabIndex = 5;
        //
        // _lblKgMinimo500
        //
        _lblKgMinimo500.Location = new Point(20, 110);
        _lblKgMinimo500.Name = "_lblKgMinimo500";
        _lblKgMinimo500.Size = new Size(80, 13);
        _lblKgMinimo500.TabIndex = 6;
        _lblKgMinimo500.Text = "Kg MÃ­n. 500 Cajas:";
        //
        // _txtKgMinimo500
        //
        _txtKgMinimo500.Location = new Point(130, 108);
        _txtKgMinimo500.Name = "_txtKgMinimo500";
        _txtKgMinimo500.Properties.Mask.EditMask = "N02";
        _txtKgMinimo500.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _txtKgMinimo500.Size = new Size(120, 20);
        _txtKgMinimo500.TabIndex = 7;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(130, 140);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 8;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(190, 175);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 9;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(280, 175);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 10;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // ZonaEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(380, 215);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_lblKgMinimo300);
        Controls.Add(_txtKgMinimo300);
        Controls.Add(_lblKgMinimo400);
        Controls.Add(_txtKgMinimo400);
        Controls.Add(_lblKgMinimo500);
        Controls.Add(_txtKgMinimo500);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ZonaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Zona";
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo300.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo400.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKgMinimo500.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
