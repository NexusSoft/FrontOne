using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Acopio;

partial class MonedaEditarForm
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
    private LabelControl _lblNomenclatura;
    private TextEdit _txtNomenclatura;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonedaEditarForm));
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _lblNomenclatura = new LabelControl();
        _txtNomenclatura = new TextEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNomenclatura.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 20);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(41, 13);
        _lblNombre.TabIndex = 0;
        _lblNombre.Text = "Nombre:";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(130, 18);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(220, 20);
        _txtNombre.TabIndex = 1;
        //
        // _lblNomenclatura
        //
        _lblNomenclatura.Location = new Point(20, 50);
        _lblNomenclatura.Name = "_lblNomenclatura";
        _lblNomenclatura.Size = new Size(72, 13);
        _lblNomenclatura.TabIndex = 2;
        _lblNomenclatura.Text = "Nomenclatura:";
        //
        // _txtNomenclatura
        //
        _txtNomenclatura.Location = new Point(130, 47);
        _txtNomenclatura.Name = "_txtNomenclatura";
        _txtNomenclatura.Properties.CharacterCasing = CharacterCasing.Upper;
        _txtNomenclatura.Properties.MaxLength = 10;
        _txtNomenclatura.Size = new Size(80, 20);
        _txtNomenclatura.TabIndex = 3;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(130, 79);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 4;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(190, 124);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 5;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(280, 124);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 6;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // MonedaEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(380, 164);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_lblNomenclatura);
        Controls.Add(_txtNomenclatura);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MonedaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Moneda";
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNomenclatura.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
