using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Seguridad;

partial class RolEditarForm
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
    private LabelControl _lblDescripcion;
    private TextEdit _txtDescripcion;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RolEditarForm));
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _lblDescripcion = new LabelControl();
        _txtDescripcion = new TextEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 20);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(40, 13);
        _lblNombre.Text = "Nombre:";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(130, 18);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(200, 20);
        //
        // _lblDescripcion
        //
        _lblDescripcion.Location = new Point(20, 55);
        _lblDescripcion.Name = "_lblDescripcion";
        _lblDescripcion.Size = new Size(58, 13);
        _lblDescripcion.Text = "DescripciÃ³n:";
        //
        // _txtDescripcion
        //
        _txtDescripcion.Location = new Point(130, 53);
        _txtDescripcion.Name = "_txtDescripcion";
        _txtDescripcion.Size = new Size(250, 20);
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(130, 85);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(220, 130);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(310, 130);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // RolEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(410, 170);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_lblDescripcion);
        Controls.Add(_txtDescripcion);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "RolEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "RolEditarForm";
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
