using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Sistema;

partial class LoginForm
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

    private LabelControl _lblUsuario;
    private TextEdit _txtUsuario;
    private LabelControl _lblPassword;
    private TextEdit _txtPassword;
    private LabelControl _lblError;
    private SimpleButton _btnEntrar;
    private SimpleButton _btnConfiguracion;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
        _lblUsuario = new LabelControl();
        _txtUsuario = new TextEdit();
        _lblPassword = new LabelControl();
        _txtPassword = new TextEdit();
        _lblError = new LabelControl();
        _btnEntrar = new SimpleButton();
        _btnConfiguracion = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtUsuario.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).BeginInit();
        SuspendLayout();
        // 
        // _lblUsuario
        // 
        _lblUsuario.Location = new Point(20, 50);
        _lblUsuario.Name = "_lblUsuario";
        _lblUsuario.Size = new Size(36, 13);
        _lblUsuario.TabIndex = 0;
        _lblUsuario.Text = "Usuario";
        // 
        // _txtUsuario
        // 
        _txtUsuario.Location = new Point(120, 48);
        _txtUsuario.Name = "_txtUsuario";
        _txtUsuario.Size = new Size(180, 20);
        _txtUsuario.TabIndex = 1;
        // 
        // _lblPassword
        // 
        _lblPassword.Location = new Point(20, 76);
        _lblPassword.Name = "_lblPassword";
        _lblPassword.Size = new Size(56, 13);
        _lblPassword.TabIndex = 2;
        _lblPassword.Text = "Contraseña";
        // 
        // _txtPassword
        // 
        _txtPassword.Location = new Point(120, 74);
        _txtPassword.Name = "_txtPassword";
        _txtPassword.Properties.UseSystemPasswordChar = true;
        _txtPassword.Size = new Size(180, 20);
        _txtPassword.TabIndex = 3;
        // 
        // _lblError
        // 
        _lblError.Appearance.ForeColor = Color.Red;
        _lblError.Appearance.Options.UseForeColor = true;
        _lblError.Location = new Point(20, 90);
        _lblError.Name = "_lblError";
        _lblError.Size = new Size(0, 13);
        _lblError.TabIndex = 4;
        // 
        // _btnEntrar
        // 
        _btnEntrar.ImageOptions.Image = (Image)resources.GetObject("_btnEntrar.ImageOptions.Image");
        _btnEntrar.Location = new Point(210, 113);
        _btnEntrar.Name = "_btnEntrar";
        _btnEntrar.Size = new Size(90, 23);
        _btnEntrar.TabIndex = 5;
        _btnEntrar.Text = "Entrar";
        _btnEntrar.Click += BtnEntrar_Click;
        // 
        // _btnConfiguracion
        // 
        _btnConfiguracion.ImageOptions.Image = (Image)resources.GetObject("_btnConfiguracion.ImageOptions.Image");
        _btnConfiguracion.Location = new Point(12, 2);
        _btnConfiguracion.Name = "_btnConfiguracion";
        _btnConfiguracion.Size = new Size(180, 23);
        _btnConfiguracion.TabIndex = 6;
        _btnConfiguracion.Text = "Configuración de conexiones";
        _btnConfiguracion.Click += BtnConfiguracion_Click;
        // 
        // LoginForm
        // 
        AcceptButton = _btnEntrar;
        ClientSize = new Size(325, 157);
        Controls.Add(_lblUsuario);
        Controls.Add(_txtUsuario);
        Controls.Add(_lblPassword);
        Controls.Add(_txtPassword);
        Controls.Add(_lblError);
        Controls.Add(_btnEntrar);
        Controls.Add(_btnConfiguracion);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Iniciar sesión";
        ((System.ComponentModel.ISupportInitialize)_txtUsuario.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
