using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Acopio;

partial class TipoCorteEditarForm
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
    private LabelControl _lblFueraDeNorma;
    private SpinEdit _spnFueraDeNorma;
    private CheckEdit _chkDanioMinimo;
    private LabelControl _lblTipoPago;
    private LookUpEdit _cmbTipoPago;
    private CheckEdit _chkActivo;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipoCorteEditarForm));
        _lblNombre = new LabelControl();
        _txtNombre = new TextEdit();
        _lblFueraDeNorma = new LabelControl();
        _spnFueraDeNorma = new SpinEdit();
        _chkDanioMinimo = new CheckEdit();
        _lblTipoPago = new LabelControl();
        _cmbTipoPago = new LookUpEdit();
        _chkActivo = new CheckEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnFueraDeNorma.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkDanioMinimo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoPago.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblNombre
        //
        _lblNombre.Location = new Point(20, 22);
        _lblNombre.Name = "_lblNombre";
        _lblNombre.Size = new Size(64, 13);
        _lblNombre.TabIndex = 0;
        _lblNombre.Text = "Tipo de corte";
        //
        // _txtNombre
        //
        _txtNombre.Location = new Point(140, 18);
        _txtNombre.Name = "_txtNombre";
        _txtNombre.Properties.MaxLength = 100;
        _txtNombre.Size = new Size(210, 20);
        _txtNombre.TabIndex = 1;
        //
        // _lblFueraDeNorma
        //
        _lblFueraDeNorma.Location = new Point(20, 48);
        _lblFueraDeNorma.Name = "_lblFueraDeNorma";
        _lblFueraDeNorma.Size = new Size(103, 13);
        _lblFueraDeNorma.TabIndex = 2;
        _lblFueraDeNorma.Text = "Fuera de norma (gr)";
        //
        // _spnFueraDeNorma
        //
        _spnFueraDeNorma.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnFueraDeNorma.Location = new Point(140, 44);
        _spnFueraDeNorma.Name = "_spnFueraDeNorma";
        _spnFueraDeNorma.Properties.Mask.EditMask = "N02";
        _spnFueraDeNorma.Size = new Size(100, 20);
        _spnFueraDeNorma.TabIndex = 3;
        //
        // _chkDanioMinimo
        //
        _chkDanioMinimo.Location = new Point(138, 70);
        _chkDanioMinimo.Name = "_chkDanioMinimo";
        _chkDanioMinimo.Properties.Caption = "Daño mínimo";
        _chkDanioMinimo.Size = new Size(120, 20);
        _chkDanioMinimo.TabIndex = 4;
        //
        // _lblTipoPago
        //
        _lblTipoPago.Location = new Point(20, 100);
        _lblTipoPago.Name = "_lblTipoPago";
        _lblTipoPago.Size = new Size(63, 13);
        _lblTipoPago.TabIndex = 5;
        _lblTipoPago.Text = "Tipo de pago";
        //
        // _cmbTipoPago
        //
        _cmbTipoPago.Location = new Point(140, 96);
        _cmbTipoPago.Name = "_cmbTipoPago";
        _cmbTipoPago.Properties.NullText = "Seleccionar";
        _cmbTipoPago.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbTipoPago.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbTipoPago.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbTipoPago.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbTipoPago.Size = new Size(210, 20);
        _cmbTipoPago.TabIndex = 6;
        _cmbTipoPago.ButtonClick += CmbTipoPago_ButtonClick;
        //
        // _chkActivo
        //
        _chkActivo.Location = new Point(138, 122);
        _chkActivo.Name = "_chkActivo";
        _chkActivo.Properties.Caption = "Activo";
        _chkActivo.Size = new Size(100, 20);
        _chkActivo.TabIndex = 7;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(180, 154);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 8;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(270, 154);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 9;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // TipoCorteEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(380, 195);
        Controls.Add(_lblNombre);
        Controls.Add(_txtNombre);
        Controls.Add(_lblFueraDeNorma);
        Controls.Add(_spnFueraDeNorma);
        Controls.Add(_chkDanioMinimo);
        Controls.Add(_lblTipoPago);
        Controls.Add(_cmbTipoPago);
        Controls.Add(_chkActivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TipoCorteEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tipo de corte";
        ((System.ComponentModel.ISupportInitialize)_txtNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnFueraDeNorma.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkDanioMinimo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoPago.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkActivo.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
