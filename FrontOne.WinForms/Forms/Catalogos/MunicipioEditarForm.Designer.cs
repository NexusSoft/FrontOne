using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class MunicipioEditarForm
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
    private LabelControl _lblNombre;
    private TextEdit _txtNombre;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MunicipioEditarForm));
        _lblPais = new LabelControl();
        _cmbPais = new LookUpEdit();
        _lblEstado = new LabelControl();
        _cmbEstado = new LookUpEdit();
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblPais
        //
        _lblPais.Location = new Point(20, 20);
        _lblPais.Name = "_lblPais";
        _lblPais.Size = new Size(19, 13);
        _lblPais.TabIndex = 0;
        _lblPais.Text = "País";
        //
        // _cmbPais
        //
        _cmbPais.Location = new Point(130, 18);
        _cmbPais.Name = "_cmbPais";
        _cmbPais.Properties.NullText = "Seleccionar";
        _cmbPais.Size = new Size(190, 20);
        _cmbPais.TabIndex = 1;
        //
        // _lblEstado
        //
        _lblEstado.Location = new Point(20, 48);
        _lblEstado.Name = "_lblEstado";
        _lblEstado.Size = new Size(33, 13);
        _lblEstado.TabIndex = 2;
        _lblEstado.Text = "Estado";
        //
        // _cmbEstado
        //
        _cmbEstado.Location = new Point(130, 44);
        _cmbEstado.Name = "_cmbEstado";
        _cmbEstado.Properties.NullText = "Seleccionar";
        _cmbEstado.Size = new Size(190, 20);
        _cmbEstado.TabIndex = 3;
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 74);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(37, 13);
        _lblNombre.TabIndex = 4;
        _lblNombre.Text = "Nombre";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(130, 70);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(190, 20);
        _txtNombre.TabIndex = 5;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(130, 96);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 6;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(150, 122);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 7;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(236, 122);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 8;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // MunicipioEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(350, 170);
        Controls.Add(_lblPais);
        Controls.Add(_cmbPais);
        Controls.Add(_lblEstado);
        Controls.Add(_cmbEstado);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MunicipioEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Municipio";
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
