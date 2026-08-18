using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Gastos;

partial class TipoAjusteEditarForm
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
    private LabelControl _lblTipoGasto;
    private ComboBoxEdit _cmbTipoGasto;
    private LabelControl _lblSigno;
    private ComboBoxEdit _cmbSigno;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipoAjusteEditarForm));
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _lblTipoGasto = new LabelControl();
        _cmbTipoGasto = new ComboBoxEdit();
        _lblSigno = new LabelControl();
        _cmbSigno = new ComboBoxEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoGasto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbSigno.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 20);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(43, 13);
        _lblNombre.TabIndex = 0;
        _lblNombre.Text = "Nombre";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(140, 18);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Size = new Size(200, 20);
        _txtNombre.TabIndex = 1;
        //
        // _lblTipoGasto
        //
        _lblTipoGasto.Location = new Point(20, 48);
        _lblTipoGasto.Name = "_lblTipoGasto";
        _lblTipoGasto.Size = new Size(60, 13);
        _lblTipoGasto.TabIndex = 2;
        _lblTipoGasto.Text = "Tipo de Gasto";
        //
        // _cmbTipoGasto
        //
        _cmbTipoGasto.Location = new Point(140, 44);
        _cmbTipoGasto.Name = "_cmbTipoGasto";
        _cmbTipoGasto.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbTipoGasto.Size = new Size(200, 20);
        _cmbTipoGasto.TabIndex = 3;
        //
        // _lblSigno
        //
        _lblSigno.Location = new Point(20, 76);
        _lblSigno.Name = "_lblSigno";
        _lblSigno.Size = new Size(30, 13);
        _lblSigno.TabIndex = 4;
        _lblSigno.Text = "Signo";
        //
        // _cmbSigno
        //
        _cmbSigno.Location = new Point(140, 72);
        _cmbSigno.Name = "_cmbSigno";
        _cmbSigno.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbSigno.Size = new Size(200, 20);
        _cmbSigno.TabIndex = 5;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(140, 98);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 6;
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(180, 124);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 7;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(270, 124);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 8;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // TipoAjusteEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(366, 166);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_lblTipoGasto);
        Controls.Add(_cmbTipoGasto);
        Controls.Add(_lblSigno);
        Controls.Add(_cmbSigno);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TipoAjusteEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tipo de Ajuste";
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoGasto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbSigno.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
