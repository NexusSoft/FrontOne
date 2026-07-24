using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class PaisEditarForm
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

    private LabelControl _lblClave;
    private TextEdit _txtClave;
    private LabelControl _lblNombre;
    private TextEdit _txtNombre;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaisEditarForm));
        _lblClave = new LabelControl();
        _txtClave = new TextEdit();
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtClave.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        // 
        // _lblClave
        // 
        _lblClave.Location = new Point(20, 20);
        _lblClave.Name = "_lblClave";
        _lblClave.Size = new Size(56, 13);
        _lblClave.TabIndex = 0;
        _lblClave.Text = "Clave (ISO)";
        // 
        // _txtClave
        // 
        _txtClave.Location = new Point(130, 18);
        _txtClave.Name = "_txtClave";
        _txtClave.Properties.CharacterCasing = CharacterCasing.Upper;
        _txtClave.Properties.MaxLength = 3;
        _txtClave.Size = new Size(80, 20);
        _txtClave.TabIndex = 1;
        // 
        // _lblNombre
        // 
        _lblNombre.Location = new Point(20, 48);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(37, 13);
        _lblNombre.TabIndex = 2;
        _lblNombre.Text = "Nombre";
        // 
        // _txtNombre
        // 
        _txtNombre.Location = new Point(130, 44);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(170, 20);
        _txtNombre.TabIndex = 3;
        // 
        // _chkActivo
        // 
        _chkActivo.Location = new Point(130, 70);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 4;
        // 
        // _btnGuardar
        // 
        _btnGuardar.Location = new Point(130, 96);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 5;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        // 
        // _btnCancelar
        // 
        _btnCancelar.Location = new Point(220, 96);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 6;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        // 
        // PaisEditarForm
        // 
        AcceptButton = _btnGuardar;
        ClientSize = new Size(326, 138);
        Controls.Add(_lblClave);
        Controls.Add(_txtClave);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PaisEditarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Pais";
        ((System.ComponentModel.ISupportInitialize)_txtClave.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
