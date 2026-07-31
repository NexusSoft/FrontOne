using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Sistema;

partial class ConfiguracionLicenciaTecitForm
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

    private LabelControl _lblLicenciatario;
    private TextEdit _txtLicenciatario;
    private LabelControl _lblClaveLicencia;
    private TextEdit _txtClaveLicencia;
    private LabelControl _lblTipoLicencia;
    private ComboBoxEdit _cmbTipoLicencia;
    private LabelControl _lblNumeroLicencias;
    private TextEdit _txtNumeroLicencias;
    private LabelControl _lblProducto;
    private ComboBoxEdit _cmbProducto;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfiguracionLicenciaTecitForm));
        _lblLicenciatario = new LabelControl();
        _txtLicenciatario = new TextEdit();
        _lblClaveLicencia = new LabelControl();
        _txtClaveLicencia = new TextEdit();
        _lblTipoLicencia = new LabelControl();
        _cmbTipoLicencia = new ComboBoxEdit();
        _lblNumeroLicencias = new LabelControl();
        _txtNumeroLicencias = new TextEdit();
        _lblProducto = new LabelControl();
        _cmbProducto = new ComboBoxEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtLicenciatario.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtClaveLicencia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoLicencia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroLicencias.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblLicenciatario
        //
        _lblLicenciatario.Location = new Point(15, 18);
        _lblLicenciatario.Name = "_lblLicenciatario";
        _lblLicenciatario.Size = new Size(75, 13);
        _lblLicenciatario.Text = "Licenciatario:";
        //
        // _txtLicenciatario
        //
        _txtLicenciatario.Location = new Point(140, 15);
        _txtLicenciatario.Name = "_txtLicenciatario";
        _txtLicenciatario.Size = new Size(300, 20);
        //
        // _lblClaveLicencia
        //
        _lblClaveLicencia.Location = new Point(15, 44);
        _lblClaveLicencia.Name = "_lblClaveLicencia";
        _lblClaveLicencia.Size = new Size(94, 13);
        _lblClaveLicencia.Text = "Clave de licencia:";
        //
        // _txtClaveLicencia
        //
        _txtClaveLicencia.Location = new Point(140, 41);
        _txtClaveLicencia.Name = "_txtClaveLicencia";
        _txtClaveLicencia.Properties.UseSystemPasswordChar = true;
        _txtClaveLicencia.Size = new Size(300, 20);
        //
        // _lblTipoLicencia
        //
        _lblTipoLicencia.Location = new Point(15, 70);
        _lblTipoLicencia.Name = "_lblTipoLicencia";
        _lblTipoLicencia.Size = new Size(70, 13);
        _lblTipoLicencia.Text = "Tipo de licencia:";
        //
        // _cmbTipoLicencia
        //
        _cmbTipoLicencia.Location = new Point(140, 67);
        _cmbTipoLicencia.Name = "_cmbTipoLicencia";
        _cmbTipoLicencia.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        _cmbTipoLicencia.Properties.Items.AddRange(new object[] { "Single", "Site", "DeveloperOrWeb" });
        _cmbTipoLicencia.Size = new Size(200, 20);
        //
        // _lblNumeroLicencias
        //
        _lblNumeroLicencias.Location = new Point(15, 96);
        _lblNumeroLicencias.Name = "_lblNumeroLicencias";
        _lblNumeroLicencias.Size = new Size(95, 13);
        _lblNumeroLicencias.Text = "No. de licencias:";
        //
        // _txtNumeroLicencias
        //
        _txtNumeroLicencias.Location = new Point(140, 93);
        _txtNumeroLicencias.Name = "_txtNumeroLicencias";
        _txtNumeroLicencias.Size = new Size(100, 20);
        //
        // _lblProducto
        //
        _lblProducto.Location = new Point(15, 122);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(48, 13);
        _lblProducto.Text = "Producto:";
        //
        // _cmbProducto
        //
        _cmbProducto.Location = new Point(140, 119);
        _cmbProducto.Name = "_cmbProducto";
        _cmbProducto.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        _cmbProducto.Properties.Items.AddRange(new object[] { "Barcode1D", "Barcode2D" });
        _cmbProducto.Size = new Size(200, 20);
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(280, 160);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(370, 160);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // ConfiguracionLicenciaTecitForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(465, 198);
        Controls.Add(_lblLicenciatario);
        Controls.Add(_txtLicenciatario);
        Controls.Add(_lblClaveLicencia);
        Controls.Add(_txtClaveLicencia);
        Controls.Add(_lblTipoLicencia);
        Controls.Add(_cmbTipoLicencia);
        Controls.Add(_lblNumeroLicencias);
        Controls.Add(_txtNumeroLicencias);
        Controls.Add(_lblProducto);
        Controls.Add(_cmbProducto);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ConfiguracionLicenciaTecitForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Licencia TECIT (Código de Barras)";
        ((System.ComponentModel.ISupportInitialize)_txtLicenciatario.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtClaveLicencia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoLicencia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroLicencias.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
