using DevExpress.XtraEditors;

namespace FrontOne.WinForms.Forms.Etiquetado;

partial class EtiquetaDuplicarForm
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

    private LabelControl _lblNombreNuevo;
    private TextEdit _txtNombreNuevo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EtiquetaDuplicarForm));
        _lblNombreNuevo = new LabelControl();
        _txtNombreNuevo = new TextEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombreNuevo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombreNuevo
        //
        _lblNombreNuevo.Location = new Point(15, 18);
        _lblNombreNuevo.Name = "_lblNombreNuevo";
        _lblNombreNuevo.Size = new Size(130, 13);
        _lblNombreNuevo.Text = "Nombre de la nueva etiqueta:";
        //
        // _txtNombreNuevo
        //
        _txtNombreNuevo.Location = new Point(15, 38);
        _txtNombreNuevo.Name = "_txtNombreNuevo";
        _txtNombreNuevo.Size = new Size(330, 20);
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(180, 75);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(265, 75);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // EtiquetaDuplicarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(360, 110);
        Controls.Add(_lblNombreNuevo);
        Controls.Add(_txtNombreNuevo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EtiquetaDuplicarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Duplicar etiqueta";
        ((System.ComponentModel.ISupportInitialize)_txtNombreNuevo.Properties).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
