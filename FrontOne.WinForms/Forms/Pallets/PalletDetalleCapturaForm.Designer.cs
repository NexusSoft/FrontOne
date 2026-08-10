using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace FrontOne.WinForms.Forms.Pallets;

partial class PalletDetalleCapturaForm
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

    private LabelControl _lblPallet;
    private TextEdit _txtPallet;
    private LabelControl _lblLote;
    private LookUpEdit _cmbLote;
    private LabelControl _lblKilosDisponibles;
    private SpinEdit _spnKilosDisponibles;
    private LabelControl _lblLineaProduccion;
    private TextEdit _txtLineaProduccion;
    private LabelControl _lblHuerta;
    private TextEdit _txtHuerta;
    private LabelControl _lblRegistroSagarpa;
    private TextEdit _txtRegistroSagarpa;
    private LabelControl _lblProductor;
    private TextEdit _txtProductor;
    private LabelControl _lblProducto;
    private LookUpEdit _cmbProducto;
    private LabelControl _lblPesoEstandar;
    private SpinEdit _spnPesoEstandar;
    private LabelControl _lblCajasPorPallet;
    private SpinEdit _spnCajasPorPallet;
    private LabelControl _lblCajas;
    private SpinEdit _spnCajas;
    private LabelControl _lblKilogramos;
    private SpinEdit _spnKilogramos;
    private LabelControl _lblPorcentajeMateriaSeca;
    private SpinEdit _spnPorcentajeMateriaSeca;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PalletDetalleCapturaForm));
        _lblPallet = new LabelControl();
        _txtPallet = new TextEdit();
        _lblLote = new LabelControl();
        _cmbLote = new LookUpEdit();
        _lblKilosDisponibles = new LabelControl();
        _spnKilosDisponibles = new SpinEdit();
        _lblLineaProduccion = new LabelControl();
        _txtLineaProduccion = new TextEdit();
        _lblHuerta = new LabelControl();
        _txtHuerta = new TextEdit();
        _lblRegistroSagarpa = new LabelControl();
        _txtRegistroSagarpa = new TextEdit();
        _lblProductor = new LabelControl();
        _txtProductor = new TextEdit();
        _lblProducto = new LabelControl();
        _cmbProducto = new LookUpEdit();
        _lblPesoEstandar = new LabelControl();
        _spnPesoEstandar = new SpinEdit();
        _lblCajasPorPallet = new LabelControl();
        _spnCajasPorPallet = new SpinEdit();
        _lblCajas = new LabelControl();
        _spnCajas = new SpinEdit();
        _lblKilogramos = new LabelControl();
        _spnKilogramos = new SpinEdit();
        _lblPorcentajeMateriaSeca = new LabelControl();
        _spnPorcentajeMateriaSeca = new SpinEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtPallet.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbLote.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilosDisponibles.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtLineaProduccion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoEstandar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPorPallet.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajas.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).BeginInit();
        SuspendLayout();
        //
        // _lblPallet
        //
        _lblPallet.Location = new Point(15, 18);
        _lblPallet.Name = "_lblPallet";
        _lblPallet.Size = new Size(66, 13);
        _lblPallet.Text = "No. de Pallet:";
        //
        // _txtPallet
        //
        _txtPallet.Location = new Point(130, 15);
        _txtPallet.Name = "_txtPallet";
        _txtPallet.Properties.ReadOnly = true;
        _txtPallet.Size = new Size(120, 20);
        _txtPallet.TabIndex = 0;
        //
        // _lblLote
        //
        _lblLote.Location = new Point(15, 46);
        _lblLote.Name = "_lblLote";
        _lblLote.Size = new Size(28, 13);
        _lblLote.Text = "Lote:";
        //
        // _cmbLote
        //
        _cmbLote.Location = new Point(130, 43);
        _cmbLote.Name = "_cmbLote";
        _cmbLote.Properties.NullText = "Seleccionar";
        _cmbLote.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbLote.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbLote.Size = new Size(250, 20);
        _cmbLote.TabIndex = 1;
        _cmbLote.EditValueChanged += CmbLote_EditValueChanged;
        //
        // _lblKilosDisponibles
        //
        _lblKilosDisponibles.Location = new Point(395, 46);
        _lblKilosDisponibles.Name = "_lblKilosDisponibles";
        _lblKilosDisponibles.Size = new Size(80, 13);
        _lblKilosDisponibles.Text = "Kg disponibles:";
        //
        // _spnKilosDisponibles
        //
        _spnKilosDisponibles.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnKilosDisponibles.Location = new Point(485, 43);
        _spnKilosDisponibles.Name = "_spnKilosDisponibles";
        _spnKilosDisponibles.Properties.Mask.EditMask = "N02";
        _spnKilosDisponibles.Properties.ReadOnly = true;
        _spnKilosDisponibles.Size = new Size(100, 20);
        _spnKilosDisponibles.TabIndex = 2;
        //
        // _lblLineaProduccion
        //
        _lblLineaProduccion.Location = new Point(15, 74);
        _lblLineaProduccion.Name = "_lblLineaProduccion";
        _lblLineaProduccion.Size = new Size(105, 13);
        _lblLineaProduccion.Text = "Línea de Producción:";
        //
        // _txtLineaProduccion
        //
        _txtLineaProduccion.Location = new Point(130, 71);
        _txtLineaProduccion.Name = "_txtLineaProduccion";
        _txtLineaProduccion.Properties.ReadOnly = true;
        _txtLineaProduccion.Size = new Size(250, 20);
        _txtLineaProduccion.TabIndex = 3;
        //
        // _lblHuerta
        //
        _lblHuerta.Location = new Point(15, 102);
        _lblHuerta.Name = "_lblHuerta";
        _lblHuerta.Size = new Size(38, 13);
        _lblHuerta.Text = "Huerta:";
        //
        // _txtHuerta
        //
        _txtHuerta.Location = new Point(130, 99);
        _txtHuerta.Name = "_txtHuerta";
        _txtHuerta.Properties.ReadOnly = true;
        _txtHuerta.Size = new Size(250, 20);
        _txtHuerta.TabIndex = 4;
        //
        // _lblRegistroSagarpa
        //
        _lblRegistroSagarpa.Location = new Point(395, 102);
        _lblRegistroSagarpa.Name = "_lblRegistroSagarpa";
        _lblRegistroSagarpa.Size = new Size(87, 13);
        _lblRegistroSagarpa.Text = "Registro Sagarpa:";
        //
        // _txtRegistroSagarpa
        //
        _txtRegistroSagarpa.Location = new Point(485, 99);
        _txtRegistroSagarpa.Name = "_txtRegistroSagarpa";
        _txtRegistroSagarpa.Properties.ReadOnly = true;
        _txtRegistroSagarpa.Size = new Size(100, 20);
        _txtRegistroSagarpa.TabIndex = 5;
        //
        // _lblProductor
        //
        _lblProductor.Location = new Point(15, 130);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(50, 13);
        _lblProductor.Text = "Productor:";
        //
        // _txtProductor
        //
        _txtProductor.Location = new Point(130, 127);
        _txtProductor.Name = "_txtProductor";
        _txtProductor.Properties.ReadOnly = true;
        _txtProductor.Size = new Size(250, 20);
        _txtProductor.TabIndex = 6;
        //
        // _lblProducto
        //
        _lblProducto.Location = new Point(15, 166);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(48, 13);
        _lblProducto.Text = "Producto:";
        //
        // _cmbProducto
        //
        _cmbProducto.Location = new Point(130, 163);
        _cmbProducto.Name = "_cmbProducto";
        _cmbProducto.Properties.NullText = "Seleccionar";
        _cmbProducto.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbProducto.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbProducto.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbProducto.Size = new Size(455, 20);
        _cmbProducto.TabIndex = 7;
        _cmbProducto.EditValueChanged += CmbProducto_EditValueChanged;
        _cmbProducto.ButtonClick += CmbProducto_ButtonClick;
        //
        // _lblPesoEstandar
        //
        _lblPesoEstandar.Location = new Point(15, 194);
        _lblPesoEstandar.Name = "_lblPesoEstandar";
        _lblPesoEstandar.Size = new Size(78, 13);
        _lblPesoEstandar.Text = "Peso Estándar:";
        //
        // _spnPesoEstandar
        //
        _spnPesoEstandar.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPesoEstandar.Location = new Point(130, 191);
        _spnPesoEstandar.Name = "_spnPesoEstandar";
        _spnPesoEstandar.Properties.Mask.EditMask = "N02";
        _spnPesoEstandar.Properties.ReadOnly = true;
        _spnPesoEstandar.Size = new Size(100, 20);
        _spnPesoEstandar.TabIndex = 8;
        //
        // _lblCajasPorPallet
        //
        _lblCajasPorPallet.Location = new Point(250, 194);
        _lblCajasPorPallet.Name = "_lblCajasPorPallet";
        _lblCajasPorPallet.Size = new Size(84, 13);
        _lblCajasPorPallet.Text = "Cajas por Pallet:";
        //
        // _spnCajasPorPallet
        //
        _spnCajasPorPallet.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajasPorPallet.Location = new Point(345, 191);
        _spnCajasPorPallet.Name = "_spnCajasPorPallet";
        _spnCajasPorPallet.Properties.Mask.EditMask = "N00";
        _spnCajasPorPallet.Properties.ReadOnly = true;
        _spnCajasPorPallet.Size = new Size(100, 20);
        _spnCajasPorPallet.TabIndex = 9;
        //
        // _lblPorcentajeMateriaSeca
        //
        _lblPorcentajeMateriaSeca.Location = new Point(465, 194);
        _lblPorcentajeMateriaSeca.Name = "_lblPorcentajeMateriaSeca";
        _lblPorcentajeMateriaSeca.Size = new Size(80, 13);
        _lblPorcentajeMateriaSeca.Text = "% Materia Seca:";
        //
        // _spnPorcentajeMateriaSeca
        //
        _spnPorcentajeMateriaSeca.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPorcentajeMateriaSeca.Location = new Point(485, 217);
        _spnPorcentajeMateriaSeca.Name = "_spnPorcentajeMateriaSeca";
        _spnPorcentajeMateriaSeca.Properties.Mask.EditMask = "N02";
        _spnPorcentajeMateriaSeca.Properties.ReadOnly = true;
        _spnPorcentajeMateriaSeca.Size = new Size(100, 20);
        _spnPorcentajeMateriaSeca.TabIndex = 12;
        //
        // _lblCajas
        //
        _lblCajas.Location = new Point(15, 220);
        _lblCajas.Name = "_lblCajas";
        _lblCajas.Size = new Size(32, 13);
        _lblCajas.Text = "Cajas:";
        //
        // _spnCajas
        //
        _spnCajas.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCajas.Location = new Point(130, 217);
        _spnCajas.Name = "_spnCajas";
        _spnCajas.Properties.Mask.EditMask = "N00";
        _spnCajas.Properties.MaxValue = new decimal(new int[] { 999999, 0, 0, 0 });
        _spnCajas.Size = new Size(100, 20);
        _spnCajas.TabIndex = 10;
        _spnCajas.EditValueChanged += SpnCajas_EditValueChanged;
        //
        // _lblKilogramos
        //
        _lblKilogramos.Location = new Point(250, 220);
        _lblKilogramos.Name = "_lblKilogramos";
        _lblKilogramos.Size = new Size(63, 13);
        _lblKilogramos.Text = "Kilogramos:";
        //
        // _spnKilogramos
        //
        _spnKilogramos.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnKilogramos.Location = new Point(345, 217);
        _spnKilogramos.Name = "_spnKilogramos";
        _spnKilogramos.Properties.Mask.EditMask = "N02";
        _spnKilogramos.Properties.ReadOnly = true;
        _spnKilogramos.Size = new Size(100, 20);
        _spnKilogramos.TabIndex = 11;
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(415, 256);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 13;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(505, 256);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 14;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // PalletDetalleCapturaForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(600, 291);
        Controls.Add(_lblPallet);
        Controls.Add(_txtPallet);
        Controls.Add(_lblLote);
        Controls.Add(_cmbLote);
        Controls.Add(_lblKilosDisponibles);
        Controls.Add(_spnKilosDisponibles);
        Controls.Add(_lblLineaProduccion);
        Controls.Add(_txtLineaProduccion);
        Controls.Add(_lblHuerta);
        Controls.Add(_txtHuerta);
        Controls.Add(_lblRegistroSagarpa);
        Controls.Add(_txtRegistroSagarpa);
        Controls.Add(_lblProductor);
        Controls.Add(_txtProductor);
        Controls.Add(_lblProducto);
        Controls.Add(_cmbProducto);
        Controls.Add(_lblPesoEstandar);
        Controls.Add(_spnPesoEstandar);
        Controls.Add(_lblCajasPorPallet);
        Controls.Add(_spnCajasPorPallet);
        Controls.Add(_lblPorcentajeMateriaSeca);
        Controls.Add(_spnPorcentajeMateriaSeca);
        Controls.Add(_lblCajas);
        Controls.Add(_spnCajas);
        Controls.Add(_lblKilogramos);
        Controls.Add(_spnKilogramos);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PalletDetalleCapturaForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Agregar línea al pallet";
        ((System.ComponentModel.ISupportInitialize)_txtPallet.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbLote.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilosDisponibles.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtLineaProduccion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPesoEstandar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPorPallet.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajas.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilogramos.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMateriaSeca.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
