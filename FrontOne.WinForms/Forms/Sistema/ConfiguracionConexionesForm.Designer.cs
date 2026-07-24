using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Sistema;

partial class ConfiguracionConexionesForm
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

    private GroupControl _grpSql;
    private LabelControl _lblSqlServidor;
    private TextEdit _txtSqlServer;
    private LabelControl _lblSqlBaseDatos;
    private TextEdit _txtSqlDatabase;
    private CheckEdit _chkSqlIntegratedSecurity;
    private LabelControl _lblSqlUsuario;
    private TextEdit _txtSqlUserId;
    private LabelControl _lblSqlPassword;
    private TextEdit _txtSqlPassword;
    private SimpleButton _btnSqlProbar;
    private SimpleButton _btnSqlGuardar;

    private GroupControl _grpSap;
    private LabelControl _lblSapUrl;
    private TextEdit _txtSapServiceLayerUrl;
    private LabelControl _lblSapCompanyDb;
    private TextEdit _txtSapCompanyDb;
    private LabelControl _lblSapUsuario;
    private TextEdit _txtSapUserName;
    private LabelControl _lblSapPassword;
    private TextEdit _txtSapPassword;
    private SimpleButton _btnSapProbar;
    private SimpleButton _btnSapGuardar;

    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfiguracionConexionesForm));
        _grpSql = new GroupControl();
        _lblSqlServidor = new LabelControl();
        _txtSqlServer = new TextEdit();
        _lblSqlBaseDatos = new LabelControl();
        _txtSqlDatabase = new TextEdit();
        _chkSqlIntegratedSecurity = new CheckEdit();
        _lblSqlUsuario = new LabelControl();
        _txtSqlUserId = new TextEdit();
        _lblSqlPassword = new LabelControl();
        _txtSqlPassword = new TextEdit();
        _btnSqlProbar = new SimpleButton();
        _btnSqlGuardar = new SimpleButton();

        _grpSap = new GroupControl();
        _lblSapUrl = new LabelControl();
        _txtSapServiceLayerUrl = new TextEdit();
        _lblSapCompanyDb = new LabelControl();
        _txtSapCompanyDb = new TextEdit();
        _lblSapUsuario = new LabelControl();
        _txtSapUserName = new TextEdit();
        _lblSapPassword = new LabelControl();
        _txtSapPassword = new TextEdit();
        _btnSapProbar = new SimpleButton();
        _btnSapGuardar = new SimpleButton();

        _btnCerrar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_grpSql).BeginInit();
        _grpSql.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtSqlServer.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtSqlDatabase.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkSqlIntegratedSecurity.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtSqlUserId.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtSqlPassword.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpSap).BeginInit();
        _grpSap.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtSapServiceLayerUrl.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtSapCompanyDb.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtSapUserName.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtSapPassword.Properties).BeginInit();
        SuspendLayout();
        //
        // _grpSql
        //
        _grpSql.Location = new Point(15, 15);
        _grpSql.Size = new Size(415, 165);
        _grpSql.Text = "SQL Server";
        _grpSql.Controls.Add(_lblSqlServidor);
        _grpSql.Controls.Add(_txtSqlServer);
        _grpSql.Controls.Add(_lblSqlBaseDatos);
        _grpSql.Controls.Add(_txtSqlDatabase);
        _grpSql.Controls.Add(_chkSqlIntegratedSecurity);
        _grpSql.Controls.Add(_lblSqlUsuario);
        _grpSql.Controls.Add(_txtSqlUserId);
        _grpSql.Controls.Add(_lblSqlPassword);
        _grpSql.Controls.Add(_txtSqlPassword);
        _grpSql.Controls.Add(_btnSqlProbar);
        _grpSql.Controls.Add(_btnSqlGuardar);
        //
        // _lblSqlServidor
        //
        _lblSqlServidor.Location = new Point(10, 28);
        _lblSqlServidor.Name = "_lblSqlServidor";
        _lblSqlServidor.Size = new Size(43, 13);
        _lblSqlServidor.Text = "Servidor";
        //
        // _txtSqlServer
        //
        _txtSqlServer.Location = new Point(110, 25);
        _txtSqlServer.Name = "_txtSqlServer";
        _txtSqlServer.Size = new Size(280, 20);
        //
        // _lblSqlBaseDatos
        //
        _lblSqlBaseDatos.Location = new Point(10, 58);
        _lblSqlBaseDatos.Name = "_lblSqlBaseDatos";
        _lblSqlBaseDatos.Size = new Size(70, 13);
        _lblSqlBaseDatos.Text = "Base de datos";
        //
        // _txtSqlDatabase
        //
        _txtSqlDatabase.Location = new Point(110, 55);
        _txtSqlDatabase.Name = "_txtSqlDatabase";
        _txtSqlDatabase.Size = new Size(280, 20);
        //
        // _chkSqlIntegratedSecurity
        //
        _chkSqlIntegratedSecurity.Location = new Point(110, 85);
        _chkSqlIntegratedSecurity.Name = "_chkSqlIntegratedSecurity";
        _chkSqlIntegratedSecurity.Properties.Caption = "Autenticación de Windows";
        _chkSqlIntegratedSecurity.Size = new Size(220, 20);
        //
        // _lblSqlUsuario
        //
        _lblSqlUsuario.Location = new Point(10, 113);
        _lblSqlUsuario.Name = "_lblSqlUsuario";
        _lblSqlUsuario.Size = new Size(42, 13);
        _lblSqlUsuario.Text = "Usuario";
        //
        // _txtSqlUserId
        //
        _txtSqlUserId.Location = new Point(110, 110);
        _txtSqlUserId.Name = "_txtSqlUserId";
        _txtSqlUserId.Size = new Size(280, 20);
        //
        // _lblSqlPassword
        //
        _lblSqlPassword.Location = new Point(10, 138);
        _lblSqlPassword.Name = "_lblSqlPassword";
        _lblSqlPassword.Size = new Size(56, 13);
        _lblSqlPassword.Text = "Contraseña";
        //
        // _txtSqlPassword
        //
        _txtSqlPassword.Location = new Point(110, 135);
        _txtSqlPassword.Name = "_txtSqlPassword";
        _txtSqlPassword.Properties.UseSystemPasswordChar = true;
        _txtSqlPassword.Size = new Size(180, 20);
        //
        // _btnSqlProbar
        //
        _btnSqlProbar.Location = new Point(300, 133);
        _btnSqlProbar.Name = "_btnSqlProbar";
        _btnSqlProbar.Size = new Size(45, 23);
        _btnSqlProbar.Text = "Probar";
        _btnSqlProbar.Click += BtnSqlProbar_Click;
        //
        // _btnSqlGuardar
        //
        _btnSqlGuardar.Location = new Point(350, 133);
        _btnSqlGuardar.Name = "_btnSqlGuardar";
        _btnSqlGuardar.Size = new Size(55, 23);
        _btnSqlGuardar.Text = "Guardar";
        _btnSqlGuardar.Click += BtnSqlGuardar_Click;
        //
        // _grpSap
        //
        _grpSap.Location = new Point(15, 190);
        _grpSap.Size = new Size(415, 165);
        _grpSap.Text = "SAP Business One (Service Layer)";
        _grpSap.Controls.Add(_lblSapUrl);
        _grpSap.Controls.Add(_txtSapServiceLayerUrl);
        _grpSap.Controls.Add(_lblSapCompanyDb);
        _grpSap.Controls.Add(_txtSapCompanyDb);
        _grpSap.Controls.Add(_lblSapUsuario);
        _grpSap.Controls.Add(_txtSapUserName);
        _grpSap.Controls.Add(_lblSapPassword);
        _grpSap.Controls.Add(_txtSapPassword);
        _grpSap.Controls.Add(_btnSapProbar);
        _grpSap.Controls.Add(_btnSapGuardar);
        //
        // _lblSapUrl
        //
        _lblSapUrl.Location = new Point(10, 28);
        _lblSapUrl.Name = "_lblSapUrl";
        _lblSapUrl.Size = new Size(88, 13);
        _lblSapUrl.Text = "URL Service Layer";
        //
        // _txtSapServiceLayerUrl
        //
        _txtSapServiceLayerUrl.Location = new Point(110, 25);
        _txtSapServiceLayerUrl.Name = "_txtSapServiceLayerUrl";
        _txtSapServiceLayerUrl.Size = new Size(280, 20);
        //
        // _lblSapCompanyDb
        //
        _lblSapCompanyDb.Location = new Point(10, 58);
        _lblSapCompanyDb.Name = "_lblSapCompanyDb";
        _lblSapCompanyDb.Size = new Size(59, 13);
        _lblSapCompanyDb.Text = "CompanyDB";
        //
        // _txtSapCompanyDb
        //
        _txtSapCompanyDb.Location = new Point(110, 55);
        _txtSapCompanyDb.Name = "_txtSapCompanyDb";
        _txtSapCompanyDb.Size = new Size(280, 20);
        //
        // _lblSapUsuario
        //
        _lblSapUsuario.Location = new Point(10, 88);
        _lblSapUsuario.Name = "_lblSapUsuario";
        _lblSapUsuario.Size = new Size(42, 13);
        _lblSapUsuario.Text = "Usuario";
        //
        // _txtSapUserName
        //
        _txtSapUserName.Location = new Point(110, 85);
        _txtSapUserName.Name = "_txtSapUserName";
        _txtSapUserName.Size = new Size(280, 20);
        //
        // _lblSapPassword
        //
        _lblSapPassword.Location = new Point(10, 113);
        _lblSapPassword.Name = "_lblSapPassword";
        _lblSapPassword.Size = new Size(56, 13);
        _lblSapPassword.Text = "Contraseña";
        //
        // _txtSapPassword
        //
        _txtSapPassword.Location = new Point(110, 110);
        _txtSapPassword.Name = "_txtSapPassword";
        _txtSapPassword.Properties.UseSystemPasswordChar = true;
        _txtSapPassword.Size = new Size(180, 20);
        //
        // _btnSapProbar
        //
        _btnSapProbar.Location = new Point(300, 108);
        _btnSapProbar.Name = "_btnSapProbar";
        _btnSapProbar.Size = new Size(45, 23);
        _btnSapProbar.Text = "Probar";
        _btnSapProbar.Click += BtnSapProbar_Click;
        //
        // _btnSapGuardar
        //
        _btnSapGuardar.Location = new Point(350, 108);
        _btnSapGuardar.Name = "_btnSapGuardar";
        _btnSapGuardar.Size = new Size(55, 23);
        _btnSapGuardar.Text = "Guardar";
        _btnSapGuardar.Click += BtnSapGuardar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Location = new Point(355, 365);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(75, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // ConfiguracionConexionesForm
        //
        ClientSize = new Size(460, 420);
        Controls.Add(_grpSql);
        Controls.Add(_grpSap);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ConfiguracionConexionesForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Configuración de conexiones";
        ((System.ComponentModel.ISupportInitialize)_txtSqlServer.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtSqlDatabase.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkSqlIntegratedSecurity.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtSqlUserId.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtSqlPassword.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpSql).EndInit();
        _grpSql.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_txtSapServiceLayerUrl.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtSapCompanyDb.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtSapUserName.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtSapPassword.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpSap).EndInit();
        _grpSap.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
}
