using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class PoblacionEditarForm
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

    private LabelControl _lblPais;
    private LookUpEdit _cmbPais;
    private LabelControl _lblEstado;
    private LookUpEdit _cmbEstado;
    private LabelControl _lblMunicipio;
    private LookUpEdit _cmbMunicipio;
    private LabelControl _lblNombre;
    private TextEdit _txtNombre;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PoblacionEditarForm));
        _lblPais = new LabelControl();
        _cmbPais = new LookUpEdit();
        _lblEstado = new LabelControl();
        _cmbEstado = new LookUpEdit();
        _lblMunicipio = new LabelControl();
        _cmbMunicipio = new LookUpEdit();
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblPais
        //
        _lblPais.Location = new Point(19, 14);
        _lblPais.Name = "_lblPais";
        _lblPais.Size = new Size(23, 13);
        _lblPais.TabIndex = 0;
        _lblPais.Text = "País:";
        //
        // _cmbPais
        //
        _cmbPais.Location = new Point(129, 12);
        _cmbPais.Name = "_cmbPais";
        _cmbPais.Properties.NullText = "Seleccionar";
        _cmbPais.Size = new Size(220, 20);
        _cmbPais.TabIndex = 1;
        _cmbPais.EditValueChanged += CmbPais_EditValueChanged;
        //
        // _lblEstado
        //
        _lblEstado.Location = new Point(19, 42);
        _lblEstado.Name = "_lblEstado";
        _lblEstado.Size = new Size(37, 13);
        _lblEstado.TabIndex = 2;
        _lblEstado.Text = "Estado:";
        //
        // _cmbEstado
        //
        _cmbEstado.Location = new Point(129, 40);
        _cmbEstado.Name = "_cmbEstado";
        _cmbEstado.Properties.NullText = "Seleccionar";
        _cmbEstado.Size = new Size(220, 20);
        _cmbEstado.TabIndex = 3;
        _cmbEstado.EditValueChanged += CmbEstado_EditValueChanged;
        //
        // _lblMunicipio
        //
        _lblMunicipio.Location = new Point(19, 70);
        _lblMunicipio.Name = "_lblMunicipio";
        _lblMunicipio.Size = new Size(52, 13);
        _lblMunicipio.TabIndex = 4;
        _lblMunicipio.Text = "Municipio:";
        //
        // _cmbMunicipio
        //
        _cmbMunicipio.Location = new Point(129, 68);
        _cmbMunicipio.Name = "_cmbMunicipio";
        _cmbMunicipio.Properties.NullText = "Seleccionar";
        _cmbMunicipio.Size = new Size(220, 20);
        _cmbMunicipio.TabIndex = 5;
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(19, 96);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(41, 13);
        _lblNombre.TabIndex = 6;
        _lblNombre.Text = "Nombre:";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(129, 94);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(220, 20);
        _txtNombre.TabIndex = 7;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(129, 120);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 8;
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(176, 158);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 9;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(266, 158);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 10;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // PoblacionEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(380, 196);
        Controls.Add(_lblPais);
        Controls.Add(_cmbPais);
        Controls.Add(_lblEstado);
        Controls.Add(_cmbEstado);
        Controls.Add(_lblMunicipio);
        Controls.Add(_cmbMunicipio);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PoblacionEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PoblacionEditarForm";
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
