using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class PesoEstandarEditarForm
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

    private LabelControl _lblCodigo;
    private TextEdit _txtCodigo;
    private LabelControl _lblDescripcion;
    private TextEdit _txtDescripcion;
    private LabelControl _lblPesoNeto;
    private SpinEdit _spnPesoNeto;
    private LabelControl _lblPesoPromedio;
    private SpinEdit _spnPesoPromedio;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PesoEstandarEditarForm));
        _lblCodigo = new LabelControl();
        _txtCodigo = new TextEdit();
        _lblDescripcion = new LabelControl();
        _txtDescripcion = new TextEdit();
        _lblPesoNeto = new LabelControl();
        _spnPesoNeto = new SpinEdit();
        _lblPesoPromedio = new LabelControl();
        _spnPesoPromedio = new SpinEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtCodigo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoNeto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoPromedio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblCodigo
        //
        _lblCodigo.Location = new Point(20, 22);
        _lblCodigo.Name = "_lblCodigo";
        _lblCodigo.Size = new Size(37, 13);
        _lblCodigo.TabIndex = 0;
        _lblCodigo.Text = "Código";
        //
        // _txtCodigo
        //
        _txtCodigo.Location = new Point(140, 18);
        _txtCodigo.Name = "_txtCodigo";
        _txtCodigo.Properties.MaxLength = 50;
        _txtCodigo.Size = new Size(210, 20);
        _txtCodigo.TabIndex = 1;
        //
        // _lblDescripcion
        //
        _lblDescripcion.Location = new Point(20, 48);
        _lblDescripcion.Name = "_lblDescripcion";
        _lblDescripcion.Size = new Size(59, 13);
        _lblDescripcion.TabIndex = 2;
        _lblDescripcion.Text = "Descripción";
        //
        // _txtDescripcion
        //
        _txtDescripcion.Location = new Point(140, 44);
        _txtDescripcion.Name = "_txtDescripcion";
        _txtDescripcion.Properties.MaxLength = 200;
        _txtDescripcion.Size = new Size(210, 20);
        _txtDescripcion.TabIndex = 3;
        //
        // _lblPesoNeto
        //
        _lblPesoNeto.Location = new Point(20, 74);
        _lblPesoNeto.Name = "_lblPesoNeto";
        _lblPesoNeto.Size = new Size(56, 13);
        _lblPesoNeto.TabIndex = 4;
        _lblPesoNeto.Text = "Peso neto";
        //
        // _spnPesoNeto
        //
        _spnPesoNeto.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoNeto.Location = new Point(140, 70);
        _spnPesoNeto.Name = "_spnPesoNeto";
        _spnPesoNeto.Properties.Mask.EditMask = "N03";
        _spnPesoNeto.Size = new Size(100, 20);
        _spnPesoNeto.TabIndex = 5;
        //
        // _lblPesoPromedio
        //
        _lblPesoPromedio.Location = new Point(20, 100);
        _lblPesoPromedio.Name = "_lblPesoPromedio";
        _lblPesoPromedio.Size = new Size(79, 13);
        _lblPesoPromedio.TabIndex = 6;
        _lblPesoPromedio.Text = "Peso promedio";
        //
        // _spnPesoPromedio
        //
        _spnPesoPromedio.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoPromedio.Location = new Point(140, 96);
        _spnPesoPromedio.Name = "_spnPesoPromedio";
        _spnPesoPromedio.Properties.Mask.EditMask = "N03";
        _spnPesoPromedio.Size = new Size(100, 20);
        _spnPesoPromedio.TabIndex = 7;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(140, 122);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 8;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(180, 154);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 9;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(270, 154);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 10;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // PesoEstandarEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(380, 195);
        Controls.Add(_lblCodigo);
        Controls.Add(_txtCodigo);
        Controls.Add(_lblDescripcion);
        Controls.Add(_txtDescripcion);
        Controls.Add(_lblPesoNeto);
        Controls.Add(_spnPesoNeto);
        Controls.Add(_lblPesoPromedio);
        Controls.Add(_spnPesoPromedio);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PesoEstandarEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Peso estándar";
        ((System.ComponentModel.ISupportInitialize)_txtCodigo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoNeto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoPromedio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
