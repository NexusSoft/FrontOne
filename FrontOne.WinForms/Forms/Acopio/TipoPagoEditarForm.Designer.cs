using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Acopio;

partial class TipoPagoEditarForm
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
    private CheckEdit _chkNecesitaListaPrecios;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipoPagoEditarForm));
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _chkNecesitaListaPrecios = new CheckEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkNecesitaListaPrecios.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 22);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(37, 13);
        _lblNombre.TabIndex = 0;
        _lblNombre.Text = "Nombre";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(100, 18);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Properties.MaxLength = 100;
        _txtNombre.Size = new Size(220, 20);
        _txtNombre.TabIndex = 1;
        //
        // _chkNecesitaListaPrecios
        //
        _chkNecesitaListaPrecios.Location = new Point(100, 44);
        _chkNecesitaListaPrecios.Name = "_chkNecesitaListaPrecios";
        _chkNecesitaListaPrecios.Properties.Caption = "Necesita lista de precios";
        _chkNecesitaListaPrecios.Size = new Size(180, 20);
        _chkNecesitaListaPrecios.TabIndex = 2;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(100, 70);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 3;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(150, 102);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 4;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(240, 102);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 5;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // TipoPagoEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(350, 141);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_chkNecesitaListaPrecios);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TipoPagoEditarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Tipo de pago";
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkNecesitaListaPrecios.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
