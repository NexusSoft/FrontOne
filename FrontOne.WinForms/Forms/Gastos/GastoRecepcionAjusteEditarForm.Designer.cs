using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Gastos;

partial class GastoRecepcionAjusteEditarForm
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

    private LabelControl _lblTipoAjuste;
    private LookUpEdit _cmbTipoAjuste;
    private LabelControl _lblMonto;
    private SpinEdit _spnMonto;
    private LabelControl _lblCargoA;
    private RadioGroup _rdgCargoA;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GastoRecepcionAjusteEditarForm));
        _lblTipoAjuste = new LabelControl();
        _cmbTipoAjuste = new LookUpEdit();
        _lblMonto = new LabelControl();
        _spnMonto = new SpinEdit();
        _lblCargoA = new LabelControl();
        _rdgCargoA = new RadioGroup();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoAjuste.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnMonto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_rdgCargoA.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblTipoAjuste
        //
        _lblTipoAjuste.Location = new Point(20, 20);
        _lblTipoAjuste.Name = "_lblTipoAjuste";
        _lblTipoAjuste.Size = new Size(70, 13);
        _lblTipoAjuste.TabIndex = 0;
        _lblTipoAjuste.Text = "Tipo de Ajuste";
        //
        // _cmbTipoAjuste
        //
        _cmbTipoAjuste.Location = new Point(140, 18);
        _cmbTipoAjuste.Name = "_cmbTipoAjuste";
        _cmbTipoAjuste.Properties.NullText = "Seleccionar";
        _cmbTipoAjuste.Properties.SearchMode = SearchMode.AutoFilter;
        _cmbTipoAjuste.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbTipoAjuste.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbTipoAjuste.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus) { ToolTip = "Agregar tipo de ajuste" });
        _cmbTipoAjuste.Properties.ButtonClick += CmbTipoAjuste_ButtonClick;
        _cmbTipoAjuste.Size = new Size(220, 20);
        _cmbTipoAjuste.TabIndex = 1;
        //
        // _lblMonto
        //
        _lblMonto.Location = new Point(20, 48);
        _lblMonto.Name = "_lblMonto";
        _lblMonto.Size = new Size(33, 13);
        _lblMonto.TabIndex = 2;
        _lblMonto.Text = "Monto";
        //
        // _spnMonto
        //
        _spnMonto.Location = new Point(140, 44);
        _spnMonto.Name = "_spnMonto";
        _spnMonto.Properties.Mask.EditMask = "n2";
        _spnMonto.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnMonto.Properties.MaxValue = new decimal(new int[] { 100000000, 0, 0, 0 });
        _spnMonto.Size = new Size(150, 20);
        _spnMonto.TabIndex = 3;
        //
        // _lblCargoA
        //
        _lblCargoA.Location = new Point(20, 76);
        _lblCargoA.Name = "_lblCargoA";
        _lblCargoA.Size = new Size(56, 13);
        _lblCargoA.TabIndex = 4;
        _lblCargoA.Text = "Con cargo a";
        //
        // _rdgCargoA
        //
        _rdgCargoA.Location = new Point(140, 72);
        _rdgCargoA.Name = "_rdgCargoA";
        _rdgCargoA.Size = new Size(220, 44);
        _rdgCargoA.TabIndex = 5;
        //
        // _btnGuardar
        //
        _btnGuardar.Location = new Point(140, 124);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 6;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Location = new Point(230, 124);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 7;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // GastoRecepcionAjusteEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(386, 166);
        Controls.Add(_lblTipoAjuste);
        Controls.Add(_cmbTipoAjuste);
        Controls.Add(_lblMonto);
        Controls.Add(_spnMonto);
        Controls.Add(_lblCargoA);
        Controls.Add(_rdgCargoA);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "GastoRecepcionAjusteEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Agregar Ajuste";
        ((System.ComponentModel.ISupportInitialize)_cmbTipoAjuste.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnMonto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_rdgCargoA.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
