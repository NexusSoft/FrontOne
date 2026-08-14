using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Sistema;

partial class ConfiguracionEmpresaForm
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

    private LabelControl _lblRazonSocial;
    private TextEdit _txtRazonSocial;
    private LabelControl _lblDomicilio;
    private MemoEdit _txtDomicilio;
    private LabelControl _lblRfc;
    private TextEdit _txtRfc;
    private LabelControl _lblTelefono;
    private TextEdit _txtTelefono;
    private LabelControl _lblCorreo;
    private TextEdit _txtCorreo;
    private LabelControl _lblNumeroEmpaque;
    private TextEdit _txtNumeroEmpaque;
    private LabelControl _lblLogo;
    private PictureEdit _picLogo;
    private SimpleButton _btnCargarLogo;
    private SimpleButton _btnQuitarLogo;
    private LabelControl _lblLogoUsdaOrganic;
    private PictureEdit _picLogoUsdaOrganic;
    private SimpleButton _btnCargarLogoUsdaOrganic;
    private SimpleButton _btnQuitarLogoUsdaOrganic;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfiguracionEmpresaForm));
        _lblRazonSocial = new LabelControl();
        _txtRazonSocial = new TextEdit();
        _lblDomicilio = new LabelControl();
        _txtDomicilio = new MemoEdit();
        _lblRfc = new LabelControl();
        _txtRfc = new TextEdit();
        _lblTelefono = new LabelControl();
        _txtTelefono = new TextEdit();
        _lblCorreo = new LabelControl();
        _txtCorreo = new TextEdit();
        _lblNumeroEmpaque = new LabelControl();
        _txtNumeroEmpaque = new TextEdit();
        _lblLogo = new LabelControl();
        _picLogo = new PictureEdit();
        _btnCargarLogo = new SimpleButton();
        _btnQuitarLogo = new SimpleButton();
        _lblLogoUsdaOrganic = new LabelControl();
        _picLogoUsdaOrganic = new PictureEdit();
        _btnCargarLogoUsdaOrganic = new SimpleButton();
        _btnQuitarLogoUsdaOrganic = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtRazonSocial.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDomicilio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRfc.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTelefono.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCorreo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroEmpaque.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_picLogo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_picLogoUsdaOrganic.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblRazonSocial
        //
        _lblRazonSocial.Location = new Point(15, 18);
        _lblRazonSocial.Name = "_lblRazonSocial";
        _lblRazonSocial.Size = new Size(66, 13);
        _lblRazonSocial.Text = "Razón Social:";
        //
        // _txtRazonSocial
        //
        _txtRazonSocial.Location = new Point(110, 15);
        _txtRazonSocial.Name = "_txtRazonSocial";
        _txtRazonSocial.Size = new Size(320, 20);
        //
        // _lblDomicilio
        //
        _lblDomicilio.Location = new Point(15, 44);
        _lblDomicilio.Name = "_lblDomicilio";
        _lblDomicilio.Size = new Size(51, 13);
        _lblDomicilio.Text = "Domicilio:";
        //
        // _txtDomicilio
        //
        _txtDomicilio.Location = new Point(110, 41);
        _txtDomicilio.Name = "_txtDomicilio";
        _txtDomicilio.Size = new Size(320, 50);
        //
        // _lblRfc
        //
        _lblRfc.Location = new Point(15, 99);
        _lblRfc.Name = "_lblRfc";
        _lblRfc.Size = new Size(24, 13);
        _lblRfc.Text = "RFC:";
        //
        // _txtRfc
        //
        _txtRfc.Location = new Point(110, 96);
        _txtRfc.Name = "_txtRfc";
        _txtRfc.Size = new Size(150, 20);
        //
        // _lblTelefono
        //
        _lblTelefono.Location = new Point(15, 125);
        _lblTelefono.Name = "_lblTelefono";
        _lblTelefono.Size = new Size(48, 13);
        _lblTelefono.Text = "Teléfono:";
        //
        // _txtTelefono
        //
        _txtTelefono.Location = new Point(110, 122);
        _txtTelefono.Name = "_txtTelefono";
        _txtTelefono.Size = new Size(150, 20);
        //
        // _lblCorreo
        //
        _lblCorreo.Location = new Point(15, 151);
        _lblCorreo.Name = "_lblCorreo";
        _lblCorreo.Size = new Size(38, 13);
        _lblCorreo.Text = "Correo:";
        //
        // _txtCorreo
        //
        _txtCorreo.Location = new Point(110, 148);
        _txtCorreo.Name = "_txtCorreo";
        _txtCorreo.Size = new Size(250, 20);
        //
        // _lblNumeroEmpaque
        //
        _lblNumeroEmpaque.Location = new Point(15, 180);
        _lblNumeroEmpaque.Name = "_lblNumeroEmpaque";
        _lblNumeroEmpaque.Size = new Size(93, 13);
        _lblNumeroEmpaque.Text = "No. de Empaque:";
        //
        // _txtNumeroEmpaque
        //
        _txtNumeroEmpaque.Location = new Point(110, 177);
        _txtNumeroEmpaque.Name = "_txtNumeroEmpaque";
        _txtNumeroEmpaque.Properties.MaxLength = 3;
        _txtNumeroEmpaque.Size = new Size(60, 20);
        //
        // _lblLogo
        //
        _lblLogo.Location = new Point(15, 210);
        _lblLogo.Name = "_lblLogo";
        _lblLogo.Size = new Size(29, 13);
        _lblLogo.Text = "Logo:";
        //
        // _picLogo
        //
        _picLogo.Location = new Point(110, 207);
        _picLogo.Name = "_picLogo";
        _picLogo.Properties.ShowMenu = false;
        _picLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
        _picLogo.Size = new Size(140, 140);
        //
        // _btnCargarLogo
        //
        _btnCargarLogo.Location = new Point(260, 207);
        _btnCargarLogo.Name = "_btnCargarLogo";
        _btnCargarLogo.Size = new Size(170, 23);
        _btnCargarLogo.Text = "Cargar imagen...";
        _btnCargarLogo.Click += BtnCargarLogo_Click;
        //
        // _btnQuitarLogo
        //
        _btnQuitarLogo.Location = new Point(260, 235);
        _btnQuitarLogo.Name = "_btnQuitarLogo";
        _btnQuitarLogo.Size = new Size(170, 23);
        _btnQuitarLogo.Text = "Quitar logo";
        _btnQuitarLogo.Click += BtnQuitarLogo_Click;
        //
        // _lblLogoUsdaOrganic
        //
        _lblLogoUsdaOrganic.Location = new Point(15, 362);
        _lblLogoUsdaOrganic.Name = "_lblLogoUsdaOrganic";
        _lblLogoUsdaOrganic.Size = new Size(103, 13);
        _lblLogoUsdaOrganic.Text = "Logo USDA Organic:";
        //
        // _picLogoUsdaOrganic
        //
        _picLogoUsdaOrganic.Location = new Point(110, 359);
        _picLogoUsdaOrganic.Name = "_picLogoUsdaOrganic";
        _picLogoUsdaOrganic.Properties.ShowMenu = false;
        _picLogoUsdaOrganic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
        _picLogoUsdaOrganic.Size = new Size(100, 100);
        //
        // _btnCargarLogoUsdaOrganic
        //
        _btnCargarLogoUsdaOrganic.Location = new Point(260, 359);
        _btnCargarLogoUsdaOrganic.Name = "_btnCargarLogoUsdaOrganic";
        _btnCargarLogoUsdaOrganic.Size = new Size(170, 23);
        _btnCargarLogoUsdaOrganic.Text = "Cargar imagen...";
        _btnCargarLogoUsdaOrganic.Click += BtnCargarLogoUsdaOrganic_Click;
        //
        // _btnQuitarLogoUsdaOrganic
        //
        _btnQuitarLogoUsdaOrganic.Location = new Point(260, 387);
        _btnQuitarLogoUsdaOrganic.Name = "_btnQuitarLogoUsdaOrganic";
        _btnQuitarLogoUsdaOrganic.Size = new Size(170, 23);
        _btnQuitarLogoUsdaOrganic.Text = "Quitar logo";
        _btnQuitarLogoUsdaOrganic.Click += BtnQuitarLogoUsdaOrganic_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(295, 474);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(385, 474);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // ConfiguracionEmpresaForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(480, 512);
        Controls.Add(_lblRazonSocial);
        Controls.Add(_txtRazonSocial);
        Controls.Add(_lblDomicilio);
        Controls.Add(_txtDomicilio);
        Controls.Add(_lblRfc);
        Controls.Add(_txtRfc);
        Controls.Add(_lblTelefono);
        Controls.Add(_txtTelefono);
        Controls.Add(_lblCorreo);
        Controls.Add(_txtCorreo);
        Controls.Add(_lblNumeroEmpaque);
        Controls.Add(_txtNumeroEmpaque);
        Controls.Add(_lblLogo);
        Controls.Add(_picLogo);
        Controls.Add(_btnCargarLogo);
        Controls.Add(_btnQuitarLogo);
        Controls.Add(_lblLogoUsdaOrganic);
        Controls.Add(_picLogoUsdaOrganic);
        Controls.Add(_btnCargarLogoUsdaOrganic);
        Controls.Add(_btnQuitarLogoUsdaOrganic);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ConfiguracionEmpresaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Configuración de la empresa";
        ((System.ComponentModel.ISupportInitialize)_txtRazonSocial.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDomicilio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRfc.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTelefono.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCorreo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroEmpaque.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_picLogo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_picLogoUsdaOrganic.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
