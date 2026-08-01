using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Seguridad;

partial class UsuarioEditarForm
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

    private LabelControl _lblNombreUsuario;
    private TextEdit _txtNombreUsuario;
    private LabelControl _lblNombreCompleto;
    private TextEdit _txtNombreCompleto;
    private LabelControl _lblEmail;
    private TextEdit _txtEmail;
    private LabelControl _lblPassword;
    private TextEdit _txtPassword;
    private LabelControl _lblPasswordAyuda;
    private CheckEdit _chkActivo;
    private LabelControl _lblRoles;
    private CheckedListBoxControl _clbRoles;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsuarioEditarForm));
        _lblNombreUsuario = new LabelControl();
        _txtNombreUsuario = new TextEdit();
        _lblNombreCompleto = new LabelControl();
        _txtNombreCompleto = new TextEdit();
        _lblEmail = new LabelControl();
        _txtEmail = new TextEdit();
        _lblPassword = new LabelControl();
        _txtPassword = new TextEdit();
        _lblPasswordAyuda = new LabelControl();
        _chkActivo = new CheckEdit();
        _lblRoles = new LabelControl();
        _clbRoles = new CheckedListBoxControl();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombreUsuario.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombreCompleto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombreUsuario
        //
        _lblNombreUsuario.Location = new Point(20, 20);
        _lblNombreUsuario.Name = "_lblNombreUsuario";
        _lblNombreUsuario.Size = new Size(78, 13);
        _lblNombreUsuario.Text = "Nombre usuario:";
        _txtNombreUsuario.Location = new Point(150, 18);
        _txtNombreUsuario.Name = "_txtNombreUsuario";
        _txtNombreUsuario.Properties.MaxLength = 50;
        _txtNombreUsuario.Size = new Size(220, 20);
        //
        // _lblNombreCompleto
        //
        _lblNombreCompleto.Location = new Point(20, 50);
        _lblNombreCompleto.Name = "_lblNombreCompleto";
        _lblNombreCompleto.Size = new Size(85, 13);
        _lblNombreCompleto.Text = "Nombre completo:";
        _txtNombreCompleto.Location = new Point(150, 48);
        _txtNombreCompleto.Name = "_txtNombreCompleto";
        _txtNombreCompleto.Size = new Size(260, 20);
        //
        // _lblEmail
        //
        _lblEmail.Location = new Point(20, 80);
        _lblEmail.Name = "_lblEmail";
        _lblEmail.Size = new Size(35, 13);
        _lblEmail.Text = "e-mail:";
        _txtEmail.Location = new Point(150, 78);
        _txtEmail.Name = "_txtEmail";
        _txtEmail.Size = new Size(260, 20);
        //
        // _lblPassword
        //
        _lblPassword.Location = new Point(20, 110);
        _lblPassword.Name = "_lblPassword";
        _lblPassword.Size = new Size(51, 13);
        _lblPassword.Text = "ContraseÃ±a:";
        _txtPassword.Location = new Point(150, 108);
        _txtPassword.Name = "_txtPassword";
        _txtPassword.Properties.UseSystemPasswordChar = true;
        _txtPassword.Size = new Size(200, 20);
        //
        // _lblPasswordAyuda
        //
        _lblPasswordAyuda.Location = new Point(150, 132);
        _lblPasswordAyuda.Name = "_lblPasswordAyuda";
        _lblPasswordAyuda.Size = new Size(300, 13);
        _lblPasswordAyuda.Text = "Dejar en blanco para no cambiar la contraseÃ±a actual.";
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(150, 155);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        //
        // _lblRoles
        //
        _lblRoles.Location = new Point(20, 190);
        _lblRoles.Name = "_lblRoles";
        _lblRoles.Size = new Size(31, 13);
        _lblRoles.Text = "Roles:";
        //
        // _clbRoles
        //
        _clbRoles.Location = new Point(150, 188);
        _clbRoles.Name = "_clbRoles";
        _clbRoles.Size = new Size(260, 130);
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(240, 335);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(330, 335);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // UsuarioEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(430, 375);
        Controls.Add(_lblNombreUsuario);
        Controls.Add(_txtNombreUsuario);
        Controls.Add(_lblNombreCompleto);
        Controls.Add(_txtNombreCompleto);
        Controls.Add(_lblEmail);
        Controls.Add(_txtEmail);
        Controls.Add(_lblPassword);
        Controls.Add(_txtPassword);
        Controls.Add(_lblPasswordAyuda);
        Controls.Add(_chkActivo);
        Controls.Add(_lblRoles);
        Controls.Add(_clbRoles);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "UsuarioEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "UsuarioEditarForm";
        ((System.ComponentModel.ISupportInitialize)_txtNombreUsuario.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombreCompleto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
