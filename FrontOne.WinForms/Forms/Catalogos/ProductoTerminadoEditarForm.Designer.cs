using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class ProductoTerminadoEditarForm
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

    // Encabezado (solo lectura, espejo de SAP)
    private LabelControl _lblCodigoSap;
    private TextEdit _txtCodigoSap;
    private LabelControl _lblDescripcionSap;
    private TextEdit _txtDescripcionSap;
    private LabelControl _lblDescripcionExtranjeraSap;
    private TextEdit _txtDescripcionExtranjeraSap;
    private LabelControl _lblPresentacion;
    private ComboBoxEdit _cmbPresentacion;

    // Tabs
    private XtraTabControl _tabControl;
    private XtraTabPage _tabGeneral;
    private XtraTabPage _tabCarta;

    // Tab General
    private LabelControl _lblCodigoUpc;
    private TextEdit _txtCodigoUpc;
    private LabelControl _lblCodigoPlu;
    private TextEdit _txtCodigoPlu;
    private LabelControl _lblCodigoGtin;
    private TextEdit _txtCodigoGtin;
    private LabelControl _lblMateriaPrima;
    private LookUpEdit _cmbMateriaPrima;
    private LabelControl _lblCategoria;
    private LookUpEdit _cmbCategoria;
    private LabelControl _lblTipoProducto;
    private LookUpEdit _cmbTipoProducto;
    private LabelControl _lblCalibreApeam;
    private LookUpEdit _cmbCalibreApeam;
    private LabelControl _lblCalibreCodigoExterno;
    private TextEdit _txtCalibreCodigoExterno;
    private LabelControl _lblMercadoDestino;
    private LookUpEdit _cmbMercadoDestino;
    private LabelControl _lblMarca;
    private LookUpEdit _cmbMarca;
    private LabelControl _lblVariedad;
    private LookUpEdit _cmbVariedad;
    private LabelControl _lblPesoEstandar;
    private LookUpEdit _cmbPesoEstandar;
    private LabelControl _lblPesoNeto;
    private TextEdit _txtPesoNeto;
    private LabelControl _lblPesoPromedio;
    private TextEdit _txtPesoPromedio;
    private LabelControl _lblCajasPorPallet;
    private SpinEdit _spnCajasPorPallet;

    // Tab Carta
    private GridControl _gridCarta;
    private GridView _gridViewCarta;

    // Botones
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductoTerminadoEditarForm));
        _lblCodigoSap = new LabelControl();
        _txtCodigoSap = new TextEdit();
        _lblDescripcionSap = new LabelControl();
        _txtDescripcionSap = new TextEdit();
        _lblDescripcionExtranjeraSap = new LabelControl();
        _txtDescripcionExtranjeraSap = new TextEdit();
        _lblPresentacion = new LabelControl();
        _cmbPresentacion = new ComboBoxEdit();
        _tabControl = new XtraTabControl();
        _tabGeneral = new XtraTabPage();
        _lblCodigoUpc = new LabelControl();
        _txtCodigoUpc = new TextEdit();
        _lblCodigoPlu = new LabelControl();
        _txtCodigoPlu = new TextEdit();
        _lblCodigoGtin = new LabelControl();
        _txtCodigoGtin = new TextEdit();
        _lblMateriaPrima = new LabelControl();
        _cmbMateriaPrima = new LookUpEdit();
        _lblCategoria = new LabelControl();
        _cmbCategoria = new LookUpEdit();
        _lblTipoProducto = new LabelControl();
        _cmbTipoProducto = new LookUpEdit();
        _lblCalibreApeam = new LabelControl();
        _cmbCalibreApeam = new LookUpEdit();
        _lblCalibreCodigoExterno = new LabelControl();
        _txtCalibreCodigoExterno = new TextEdit();
        _lblMercadoDestino = new LabelControl();
        _cmbMercadoDestino = new LookUpEdit();
        _lblMarca = new LabelControl();
        _cmbMarca = new LookUpEdit();
        _lblVariedad = new LabelControl();
        _cmbVariedad = new LookUpEdit();
        _lblPesoEstandar = new LabelControl();
        _cmbPesoEstandar = new LookUpEdit();
        _lblPesoNeto = new LabelControl();
        _txtPesoNeto = new TextEdit();
        _lblPesoPromedio = new LabelControl();
        _txtPesoPromedio = new TextEdit();
        _lblCajasPorPallet = new LabelControl();
        _spnCajasPorPallet = new SpinEdit();
        _tabCarta = new XtraTabPage();
        _gridCarta = new GridControl();
        _gridViewCarta = new GridView();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoSap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcionSap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcionExtranjeraSap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPresentacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).BeginInit();
        _tabControl.SuspendLayout();
        _tabGeneral.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoUpc.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoPlu.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoGtin.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMateriaPrima.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCategoria.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCalibreApeam.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCalibreCodigoExterno.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMercadoDestino.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMarca.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbVariedad.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPesoEstandar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPesoNeto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPesoPromedio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPorPallet.Properties).BeginInit();
        _tabCarta.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_gridCarta).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewCarta).BeginInit();
        SuspendLayout();
        // 
        // _lblCodigoSap
        // 
        _lblCodigoSap.Location = new Point(20, 15);
        _lblCodigoSap.Name = "_lblCodigoSap";
        _lblCodigoSap.Size = new Size(59, 13);
        _lblCodigoSap.TabIndex = 0;
        _lblCodigoSap.Text = "Código SAP:";
        // 
        // _txtCodigoSap
        // 
        _txtCodigoSap.Location = new Point(140, 12);
        _txtCodigoSap.Name = "_txtCodigoSap";
        _txtCodigoSap.Properties.ReadOnly = true;
        _txtCodigoSap.Size = new Size(120, 20);
        _txtCodigoSap.TabIndex = 0;
        // 
        // _lblDescripcionSap
        // 
        _lblDescripcionSap.Location = new Point(280, 15);
        _lblDescripcionSap.Name = "_lblDescripcionSap";
        _lblDescripcionSap.Size = new Size(58, 13);
        _lblDescripcionSap.TabIndex = 1;
        _lblDescripcionSap.Text = "Descripción:";
        // 
        // _txtDescripcionSap
        // 
        _txtDescripcionSap.Location = new Point(400, 12);
        _txtDescripcionSap.Name = "_txtDescripcionSap";
        _txtDescripcionSap.Properties.ReadOnly = true;
        _txtDescripcionSap.Size = new Size(300, 20);
        _txtDescripcionSap.TabIndex = 1;
        // 
        // _lblDescripcionExtranjeraSap
        // 
        _lblDescripcionExtranjeraSap.Location = new Point(20, 43);
        _lblDescripcionExtranjeraSap.Name = "_lblDescripcionExtranjeraSap";
        _lblDescripcionExtranjeraSap.Size = new Size(112, 13);
        _lblDescripcionExtranjeraSap.TabIndex = 2;
        _lblDescripcionExtranjeraSap.Text = "Descripción Extranjera:";
        // 
        // _txtDescripcionExtranjeraSap
        // 
        _txtDescripcionExtranjeraSap.Location = new Point(140, 40);
        _txtDescripcionExtranjeraSap.Name = "_txtDescripcionExtranjeraSap";
        _txtDescripcionExtranjeraSap.Properties.ReadOnly = true;
        _txtDescripcionExtranjeraSap.Size = new Size(560, 20);
        _txtDescripcionExtranjeraSap.TabIndex = 2;
        //
        // _lblPresentacion
        //
        _lblPresentacion.Location = new Point(20, 71);
        _lblPresentacion.Name = "_lblPresentacion";
        _lblPresentacion.Size = new Size(63, 13);
        _lblPresentacion.TabIndex = 3;
        _lblPresentacion.Text = "Presentación:";
        //
        // _cmbPresentacion
        //
        _cmbPresentacion.Location = new Point(140, 68);
        _cmbPresentacion.Name = "_cmbPresentacion";
        _cmbPresentacion.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbPresentacion.Size = new Size(150, 20);
        _cmbPresentacion.TabIndex = 3;
        _cmbPresentacion.SelectedIndexChanged += CmbPresentacion_SelectedIndexChanged;
        //
        // _tabControl
        //
        _tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _tabControl.Location = new Point(10, 98);
        _tabControl.Name = "_tabControl";
        _tabControl.SelectedTabPage = _tabGeneral;
        _tabControl.Size = new Size(728, 383);
        _tabControl.TabIndex = 3;
        _tabControl.TabPages.AddRange(new XtraTabPage[] { _tabGeneral, _tabCarta });
        // 
        // _tabGeneral
        // 
        _tabGeneral.Controls.Add(_lblCodigoUpc);
        _tabGeneral.Controls.Add(_txtCodigoUpc);
        _tabGeneral.Controls.Add(_lblCodigoPlu);
        _tabGeneral.Controls.Add(_txtCodigoPlu);
        _tabGeneral.Controls.Add(_lblCodigoGtin);
        _tabGeneral.Controls.Add(_txtCodigoGtin);
        _tabGeneral.Controls.Add(_lblMateriaPrima);
        _tabGeneral.Controls.Add(_cmbMateriaPrima);
        _tabGeneral.Controls.Add(_lblCategoria);
        _tabGeneral.Controls.Add(_cmbCategoria);
        _tabGeneral.Controls.Add(_lblTipoProducto);
        _tabGeneral.Controls.Add(_cmbTipoProducto);
        _tabGeneral.Controls.Add(_lblCalibreApeam);
        _tabGeneral.Controls.Add(_cmbCalibreApeam);
        _tabGeneral.Controls.Add(_lblCalibreCodigoExterno);
        _tabGeneral.Controls.Add(_txtCalibreCodigoExterno);
        _tabGeneral.Controls.Add(_lblMercadoDestino);
        _tabGeneral.Controls.Add(_cmbMercadoDestino);
        _tabGeneral.Controls.Add(_lblMarca);
        _tabGeneral.Controls.Add(_cmbMarca);
        _tabGeneral.Controls.Add(_lblVariedad);
        _tabGeneral.Controls.Add(_cmbVariedad);
        _tabGeneral.Controls.Add(_lblPesoEstandar);
        _tabGeneral.Controls.Add(_cmbPesoEstandar);
        _tabGeneral.Controls.Add(_lblPesoNeto);
        _tabGeneral.Controls.Add(_txtPesoNeto);
        _tabGeneral.Controls.Add(_lblPesoPromedio);
        _tabGeneral.Controls.Add(_txtPesoPromedio);
        _tabGeneral.Controls.Add(_lblCajasPorPallet);
        _tabGeneral.Controls.Add(_spnCajasPorPallet);
        _tabGeneral.Name = "_tabGeneral";
        _tabGeneral.Size = new Size(726, 358);
        _tabGeneral.Text = "General";
        // 
        // _lblCodigoUpc
        // 
        _lblCodigoUpc.Location = new Point(10, 12);
        _lblCodigoUpc.Name = "_lblCodigoUpc";
        _lblCodigoUpc.Size = new Size(60, 13);
        _lblCodigoUpc.TabIndex = 0;
        _lblCodigoUpc.Text = "Código UPC:";
        // 
        // _txtCodigoUpc
        // 
        _txtCodigoUpc.Location = new Point(140, 10);
        _txtCodigoUpc.Name = "_txtCodigoUpc";
        _txtCodigoUpc.Properties.MaxLength = 20;
        _txtCodigoUpc.Size = new Size(150, 20);
        _txtCodigoUpc.TabIndex = 0;
        // 
        // _lblCodigoPlu
        // 
        _lblCodigoPlu.Location = new Point(310, 12);
        _lblCodigoPlu.Name = "_lblCodigoPlu";
        _lblCodigoPlu.Size = new Size(58, 13);
        _lblCodigoPlu.TabIndex = 1;
        _lblCodigoPlu.Text = "Código PLU:";
        // 
        // _txtCodigoPlu
        // 
        _txtCodigoPlu.Location = new Point(420, 10);
        _txtCodigoPlu.Name = "_txtCodigoPlu";
        _txtCodigoPlu.Properties.MaxLength = 20;
        _txtCodigoPlu.Size = new Size(150, 20);
        _txtCodigoPlu.TabIndex = 1;
        // 
        // _lblCodigoGtin
        // 
        _lblCodigoGtin.Location = new Point(10, 40);
        _lblCodigoGtin.Name = "_lblCodigoGtin";
        _lblCodigoGtin.Size = new Size(64, 13);
        _lblCodigoGtin.TabIndex = 2;
        _lblCodigoGtin.Text = "Código GTIN:";
        // 
        // _txtCodigoGtin
        // 
        _txtCodigoGtin.Location = new Point(140, 38);
        _txtCodigoGtin.Name = "_txtCodigoGtin";
        _txtCodigoGtin.Properties.MaxLength = 20;
        _txtCodigoGtin.Size = new Size(150, 20);
        _txtCodigoGtin.TabIndex = 2;
        // 
        // _lblMateriaPrima
        // 
        _lblMateriaPrima.Location = new Point(10, 68);
        _lblMateriaPrima.Name = "_lblMateriaPrima";
        _lblMateriaPrima.Size = new Size(69, 13);
        _lblMateriaPrima.TabIndex = 3;
        _lblMateriaPrima.Text = "Materia Prima:";
        // 
        // _cmbMateriaPrima
        // 
        _cmbMateriaPrima.Location = new Point(140, 66);
        _cmbMateriaPrima.Name = "_cmbMateriaPrima";
        _cmbMateriaPrima.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
        _cmbMateriaPrima.Properties.NullText = "Seleccionar";
        _cmbMateriaPrima.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbMateriaPrima.Size = new Size(270, 20);
        _cmbMateriaPrima.TabIndex = 3;
        // 
        // _lblCategoria
        // 
        _lblCategoria.Location = new Point(10, 96);
        _lblCategoria.Name = "_lblCategoria";
        _lblCategoria.Size = new Size(51, 13);
        _lblCategoria.TabIndex = 4;
        _lblCategoria.Text = "Categoría:";
        // 
        // _cmbCategoria
        // 
        _cmbCategoria.Location = new Point(140, 94);
        _cmbCategoria.Name = "_cmbCategoria";
        _cmbCategoria.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbCategoria.Properties.NullText = "Seleccionar";
        _cmbCategoria.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbCategoria.Size = new Size(270, 20);
        _cmbCategoria.TabIndex = 4;
        _cmbCategoria.ButtonClick += CmbCategoria_ButtonClick;
        // 
        // _lblTipoProducto
        // 
        _lblTipoProducto.Location = new Point(10, 124);
        _lblTipoProducto.Name = "_lblTipoProducto";
        _lblTipoProducto.Size = new Size(70, 13);
        _lblTipoProducto.TabIndex = 5;
        _lblTipoProducto.Text = "Tipo Producto:";
        // 
        // _cmbTipoProducto
        // 
        _cmbTipoProducto.Location = new Point(140, 122);
        _cmbTipoProducto.Name = "_cmbTipoProducto";
        _cmbTipoProducto.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbTipoProducto.Properties.NullText = "Seleccionar";
        _cmbTipoProducto.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbTipoProducto.Size = new Size(270, 20);
        _cmbTipoProducto.TabIndex = 5;
        _cmbTipoProducto.ButtonClick += CmbTipoProducto_ButtonClick;
        // 
        // _lblCalibreApeam
        // 
        _lblCalibreApeam.Location = new Point(10, 152);
        _lblCalibreApeam.Name = "_lblCalibreApeam";
        _lblCalibreApeam.Size = new Size(74, 13);
        _lblCalibreApeam.TabIndex = 6;
        _lblCalibreApeam.Text = "Calibre APEAM:";
        // 
        // _cmbCalibreApeam
        // 
        _cmbCalibreApeam.Location = new Point(140, 150);
        _cmbCalibreApeam.Name = "_cmbCalibreApeam";
        _cmbCalibreApeam.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbCalibreApeam.Properties.NullText = "Seleccionar";
        _cmbCalibreApeam.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbCalibreApeam.Size = new Size(270, 20);
        _cmbCalibreApeam.TabIndex = 6;
        _cmbCalibreApeam.ButtonClick += CmbCalibreApeam_ButtonClick;
        // 
        // _lblCalibreCodigoExterno
        // 
        _lblCalibreCodigoExterno.Location = new Point(430, 152);
        _lblCalibreCodigoExterno.Name = "_lblCalibreCodigoExterno";
        _lblCalibreCodigoExterno.Size = new Size(114, 13);
        _lblCalibreCodigoExterno.TabIndex = 7;
        _lblCalibreCodigoExterno.Text = "Código Externo Calibre:";
        // 
        // _txtCalibreCodigoExterno
        // 
        _txtCalibreCodigoExterno.Location = new Point(550, 150);
        _txtCalibreCodigoExterno.Name = "_txtCalibreCodigoExterno";
        _txtCalibreCodigoExterno.Properties.MaxLength = 50;
        _txtCalibreCodigoExterno.Size = new Size(150, 20);
        _txtCalibreCodigoExterno.TabIndex = 7;
        // 
        // _lblMercadoDestino
        // 
        _lblMercadoDestino.Location = new Point(10, 180);
        _lblMercadoDestino.Name = "_lblMercadoDestino";
        _lblMercadoDestino.Size = new Size(84, 13);
        _lblMercadoDestino.TabIndex = 8;
        _lblMercadoDestino.Text = "Mercado Destino:";
        // 
        // _cmbMercadoDestino
        // 
        _cmbMercadoDestino.Location = new Point(140, 178);
        _cmbMercadoDestino.Name = "_cmbMercadoDestino";
        _cmbMercadoDestino.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbMercadoDestino.Properties.NullText = "Seleccionar";
        _cmbMercadoDestino.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbMercadoDestino.Size = new Size(270, 20);
        _cmbMercadoDestino.TabIndex = 8;
        _cmbMercadoDestino.ButtonClick += CmbMercadoDestino_ButtonClick;
        // 
        // _lblMarca
        // 
        _lblMarca.Location = new Point(10, 208);
        _lblMarca.Name = "_lblMarca";
        _lblMarca.Size = new Size(33, 13);
        _lblMarca.TabIndex = 9;
        _lblMarca.Text = "Marca:";
        // 
        // _cmbMarca
        // 
        _cmbMarca.Location = new Point(140, 206);
        _cmbMarca.Name = "_cmbMarca";
        _cmbMarca.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbMarca.Properties.NullText = "Seleccionar";
        _cmbMarca.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbMarca.Size = new Size(270, 20);
        _cmbMarca.TabIndex = 9;
        _cmbMarca.ButtonClick += CmbMarca_ButtonClick;
        // 
        // _lblVariedad
        // 
        _lblVariedad.Location = new Point(10, 236);
        _lblVariedad.Name = "_lblVariedad";
        _lblVariedad.Size = new Size(46, 13);
        _lblVariedad.TabIndex = 10;
        _lblVariedad.Text = "Variedad:";
        // 
        // _cmbVariedad
        // 
        _cmbVariedad.Location = new Point(140, 234);
        _cmbVariedad.Name = "_cmbVariedad";
        _cmbVariedad.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbVariedad.Properties.NullText = "Seleccionar";
        _cmbVariedad.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbVariedad.Size = new Size(270, 20);
        _cmbVariedad.TabIndex = 10;
        _cmbVariedad.ButtonClick += CmbVariedad_ButtonClick;
        // 
        // _lblPesoEstandar
        // 
        _lblPesoEstandar.Location = new Point(10, 264);
        _lblPesoEstandar.Name = "_lblPesoEstandar";
        _lblPesoEstandar.Size = new Size(73, 13);
        _lblPesoEstandar.TabIndex = 11;
        _lblPesoEstandar.Text = "Peso Estándar:";
        // 
        // _cmbPesoEstandar
        // 
        _cmbPesoEstandar.Location = new Point(140, 262);
        _cmbPesoEstandar.Name = "_cmbPesoEstandar";
        _cmbPesoEstandar.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbPesoEstandar.Properties.NullText = "Seleccionar";
        _cmbPesoEstandar.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbPesoEstandar.Size = new Size(270, 20);
        _cmbPesoEstandar.TabIndex = 11;
        _cmbPesoEstandar.ButtonClick += CmbPesoEstandar_ButtonClick;
        // 
        // _lblPesoNeto
        // 
        _lblPesoNeto.Location = new Point(430, 264);
        _lblPesoNeto.Name = "_lblPesoNeto";
        _lblPesoNeto.Size = new Size(53, 13);
        _lblPesoNeto.TabIndex = 12;
        _lblPesoNeto.Text = "Peso Neto:";
        // 
        // _txtPesoNeto
        // 
        _txtPesoNeto.Location = new Point(550, 262);
        _txtPesoNeto.Name = "_txtPesoNeto";
        _txtPesoNeto.Properties.ReadOnly = true;
        _txtPesoNeto.Size = new Size(100, 20);
        _txtPesoNeto.TabIndex = 12;
        // 
        // _lblPesoPromedio
        // 
        _lblPesoPromedio.Location = new Point(430, 292);
        _lblPesoPromedio.Name = "_lblPesoPromedio";
        _lblPesoPromedio.Size = new Size(74, 13);
        _lblPesoPromedio.TabIndex = 13;
        _lblPesoPromedio.Text = "Peso Promedio:";
        // 
        // _txtPesoPromedio
        // 
        _txtPesoPromedio.Location = new Point(550, 290);
        _txtPesoPromedio.Name = "_txtPesoPromedio";
        _txtPesoPromedio.Properties.ReadOnly = true;
        _txtPesoPromedio.Size = new Size(100, 20);
        _txtPesoPromedio.TabIndex = 13;
        // 
        // _lblCajasPorPallet
        // 
        _lblCajasPorPallet.Location = new Point(10, 292);
        _lblCajasPorPallet.Name = "_lblCajasPorPallet";
        _lblCajasPorPallet.Size = new Size(79, 13);
        _lblCajasPorPallet.TabIndex = 14;
        _lblCajasPorPallet.Text = "Cajas por Pallet:";
        // 
        // _spnCajasPorPallet
        // 
        _spnCajasPorPallet.EditValue = new decimal(new int[] { 1, 0, 0, 0 });
        _spnCajasPorPallet.Location = new Point(140, 290);
        _spnCajasPorPallet.Name = "_spnCajasPorPallet";
        _spnCajasPorPallet.Properties.IsFloatValue = false;
        _spnCajasPorPallet.Properties.Mask.EditMask = "N00";
        _spnCajasPorPallet.Properties.MinValue = new decimal(new int[] { 1, 0, 0, 0 });
        _spnCajasPorPallet.Properties.MaxValue = new decimal(new int[] { 9999, 0, 0, 0 });
        _spnCajasPorPallet.Size = new Size(100, 20);
        _spnCajasPorPallet.TabIndex = 14;
        //
        // _tabCarta
        //
        _tabCarta.Controls.Add(_gridCarta);
        _tabCarta.Name = "_tabCarta";
        _tabCarta.Size = new Size(726, 358);
        _tabCarta.Text = "Carta Materiales";
        // 
        // _gridCarta
        // 
        _gridCarta.Dock = DockStyle.Fill;
        _gridCarta.Location = new Point(0, 0);
        _gridCarta.MainView = _gridViewCarta;
        _gridCarta.Name = "_gridCarta";
        _gridCarta.Size = new Size(726, 328);
        _gridCarta.TabIndex = 0;
        _gridCarta.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewCarta });
        // 
        // _gridViewCarta
        // 
        _gridViewCarta.GridControl = _gridCarta;
        _gridViewCarta.Name = "_gridViewCarta";
        _gridViewCarta.OptionsBehavior.Editable = false;
        _gridViewCarta.OptionsFind.AlwaysVisible = true;
        _gridViewCarta.OptionsView.ShowGroupPanel = false;
        // 
        // _btnGuardar
        // 
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(568, 491);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 4;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        // 
        // _btnCancelar
        // 
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(658, 491);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 5;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        // 
        // ProductoTerminadoEditarForm
        // 
        AcceptButton = _btnGuardar;
        ClientSize = new Size(748, 526);
        Controls.Add(_lblCodigoSap);
        Controls.Add(_txtCodigoSap);
        Controls.Add(_lblDescripcionSap);
        Controls.Add(_txtDescripcionSap);
        Controls.Add(_lblDescripcionExtranjeraSap);
        Controls.Add(_txtDescripcionExtranjeraSap);
        Controls.Add(_lblPresentacion);
        Controls.Add(_cmbPresentacion);
        Controls.Add(_tabControl);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(700, 558);
        Name = "ProductoTerminadoEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Producto Terminado";
        ((System.ComponentModel.ISupportInitialize)_txtCodigoSap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcionSap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDescripcionExtranjeraSap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPresentacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).EndInit();
        _tabControl.ResumeLayout(false);
        _tabGeneral.ResumeLayout(false);
        _tabGeneral.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoUpc.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoPlu.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoGtin.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMateriaPrima.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCategoria.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCalibreApeam.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCalibreCodigoExterno.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMercadoDestino.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMarca.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbVariedad.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPesoEstandar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPesoNeto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPesoPromedio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCajasPorPallet.Properties).EndInit();
        _tabCarta.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_gridCarta).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewCarta).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
