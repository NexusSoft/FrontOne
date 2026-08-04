using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using GMap.NET.WindowsForms;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class HuertaEditarForm
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

    // Encabezado
    private SimpleButton _btnInicio;
    private SimpleButton _btnAnterior;
    private SimpleButton _btnSiguiente;
    private SimpleButton _btnFin;
    private LabelControl _lblIdHuerta;
    private LabelControl _lblNombreHuerta;
    private ButtonEdit _txtNombreHuerta;

    // Tabs
    private XtraTabControl _tabControl;
    private XtraTabPage _tabGeneral;
    private XtraTabPage _tabAdicional;
    private XtraTabPage _tabStatus;

    // Tab General
    private LabelControl _lblProductor;
    private ButtonEdit _cmbProductor;
    private LabelControl _lblUbicacion;
    private TextEdit _txtUbicacion;
    private LabelControl _lblPoblacion;
    private LookUpEdit _cmbPoblacion;
    private LabelControl _lblMunicipio;
    private LookUpEdit _cmbMunicipio;
    private LabelControl _lblPais;
    private LookUpEdit _cmbPais;
    private LabelControl _lblEstado;
    private LookUpEdit _cmbEstado;
    private LabelControl _lblProducto;
    private LookUpEdit _cmbProducto;
    private GroupControl _grpEncargado;
    private LabelControl _lblEncargadoNombre;
    private TextEdit _txtEncargadoNombre;
    private LabelControl _lblEncargadoTelefono;
    private TextEdit _txtEncargadoTelefono;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private LabelControl _lblRegistroSagarpa;
    private TextEdit _txtRegistroSagarpa;
    private CheckEdit _chkGlobalGap;
    private LabelControl _lblRegistroFda;
    private TextEdit _txtRegistroFda;
    private LabelControl _lblNumeroGlobalGap;
    private TextEdit _txtNumeroGlobalGap;

    // Tab Información Adicional
    private LabelControl _lblSuperficie;
    private SpinEdit _spnSuperficie;
    private LabelControl _lblSuperficieUnidad;
    private LabelControl _lblAltura;
    private SpinEdit _spnAltura;
    private LabelControl _lblAlturaUnidad;
    private LabelControl _lblNumeroArboles;
    private SpinEdit _spnNumeroArboles;
    private LabelControl _lblEdadArboles;
    private SpinEdit _spnEdadArboles;
    private LabelControl _lblSistemaRiego;
    private LookUpEdit _cmbSistemaRiego;
    private LabelControl _lblPorcentajeMecanizacion;
    private SpinEdit _spnPorcentajeMecanizacion;
    private LabelControl _lblPorcentajeUnidad;
    private LabelControl _lblLatitud;
    private SpinEdit _spnLatitud;
    private LabelControl _lblLongitud;
    private SpinEdit _spnLongitud;
    private GMapControl _mapHuerta;

    // Tab Status
    private LabelControl _lblStatusHuerta;
    private LookUpEdit _cmbStatusHuerta;
    private LabelControl _lblFechaCambioStatus;
    private DateEdit _dtFechaCambioStatus;
    private LabelControl _lblAniosEnStatus;
    private TextEdit _txtAniosEnStatus;
    private LabelControl _lblEstatusActivo;
    private ComboBoxEdit _cmbEstatusActivo;
    private LabelControl _lblFechaVencimiento;
    private DateEdit _dtFechaVencimiento;
    private LabelControl _lblDiasVencimiento;
    private TextEdit _txtDiasVencimiento;

    // Botones
    private SimpleButton _btnNuevo;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HuertaEditarForm));
        _btnInicio = new SimpleButton();
        _btnAnterior = new SimpleButton();
        _btnSiguiente = new SimpleButton();
        _btnFin = new SimpleButton();
        _lblIdHuerta = new LabelControl();
        _lblNombreHuerta = new LabelControl();
        _txtNombreHuerta = new ButtonEdit();
        _tabControl = new XtraTabControl();
        _tabGeneral = new XtraTabPage();
        _lblProductor = new LabelControl();
        _cmbProductor = new ButtonEdit();
        _lblUbicacion = new LabelControl();
        _txtUbicacion = new TextEdit();
        _lblPais = new LabelControl();
        _cmbPais = new LookUpEdit();
        _lblEstado = new LabelControl();
        _cmbEstado = new LookUpEdit();
        _lblPoblacion = new LabelControl();
        _cmbPoblacion = new LookUpEdit();
        _lblMunicipio = new LabelControl();
        _cmbMunicipio = new LookUpEdit();
        _lblProducto = new LabelControl();
        _cmbProducto = new LookUpEdit();
        _grpEncargado = new GroupControl();
        _lblEncargadoNombre = new LabelControl();
        _txtEncargadoNombre = new TextEdit();
        _lblEncargadoTelefono = new LabelControl();
        _txtEncargadoTelefono = new TextEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
        _lblRegistroSagarpa = new LabelControl();
        _txtRegistroSagarpa = new TextEdit();
        _chkGlobalGap = new CheckEdit();
        _lblRegistroFda = new LabelControl();
        _txtRegistroFda = new TextEdit();
        _lblNumeroGlobalGap = new LabelControl();
        _txtNumeroGlobalGap = new TextEdit();
        _tabAdicional = new XtraTabPage();
        panelControl1 = new PanelControl();
        _lblSuperficie = new LabelControl();
        _spnLongitud = new SpinEdit();
        _spnSuperficie = new SpinEdit();
        _lblLongitud = new LabelControl();
        _lblSuperficieUnidad = new LabelControl();
        _spnLatitud = new SpinEdit();
        _lblAltura = new LabelControl();
        _lblLatitud = new LabelControl();
        _spnAltura = new SpinEdit();
        _lblPorcentajeUnidad = new LabelControl();
        _lblAlturaUnidad = new LabelControl();
        _spnPorcentajeMecanizacion = new SpinEdit();
        _lblNumeroArboles = new LabelControl();
        _lblPorcentajeMecanizacion = new LabelControl();
        _spnNumeroArboles = new SpinEdit();
        _cmbSistemaRiego = new LookUpEdit();
        _lblEdadArboles = new LabelControl();
        _lblSistemaRiego = new LabelControl();
        _spnEdadArboles = new SpinEdit();
        _tabStatus = new XtraTabPage();
        _lblStatusHuerta = new LabelControl();
        _cmbStatusHuerta = new LookUpEdit();
        _lblFechaCambioStatus = new LabelControl();
        _dtFechaCambioStatus = new DateEdit();
        _lblAniosEnStatus = new LabelControl();
        _txtAniosEnStatus = new TextEdit();
        _lblEstatusActivo = new LabelControl();
        _cmbEstatusActivo = new ComboBoxEdit();
        _lblFechaVencimiento = new LabelControl();
        _dtFechaVencimiento = new DateEdit();
        _lblDiasVencimiento = new LabelControl();
        _txtDiasVencimiento = new TextEdit();
        _btnNuevo = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        panelControl3 = new PanelControl();
        panelControl2 = new PanelControl();
        _mapHuerta = new GMapControl();
        chk_mostrar = new CheckEdit();
        comboBoxEdit1 = new ComboBoxEdit();
        btRecargarMapa = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtNombreHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).BeginInit();
        _tabControl.SuspendLayout();
        _tabGeneral.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_cmbProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtUbicacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPoblacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpEncargado).BeginInit();
        _grpEncargado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtEncargadoNombre.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEncargadoTelefono.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkGlobalGap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroFda.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroGlobalGap.Properties).BeginInit();
        _tabAdicional.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
        panelControl1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_spnLongitud.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnSuperficie.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnLatitud.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnAltura.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMecanizacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnNumeroArboles.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbSistemaRiego.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnEdadArboles.Properties).BeginInit();
        _tabStatus.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_cmbStatusHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaCambioStatus.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaCambioStatus.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtAniosEnStatus.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstatusActivo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaVencimiento.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaVencimiento.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDiasVencimiento.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)panelControl3).BeginInit();
        panelControl3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)panelControl2).BeginInit();
        ((System.ComponentModel.ISupportInitialize)chk_mostrar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)comboBoxEdit1.Properties).BeginInit();
        SuspendLayout();
        //
        // _btnInicio
        //
        _btnInicio.Location = new Point(20, 8);
        _btnInicio.Name = "_btnInicio";
        _btnInicio.Size = new Size(28, 23);
        _btnInicio.TabIndex = 0;
        _btnInicio.Text = "";
        _btnInicio.ToolTip = "Inicio";
        _btnInicio.Click += BtnInicio_Click;
        //
        // _btnAnterior
        //
        _btnAnterior.Location = new Point(52, 8);
        _btnAnterior.Name = "_btnAnterior";
        _btnAnterior.Size = new Size(28, 23);
        _btnAnterior.TabIndex = 1;
        _btnAnterior.Text = "";
        _btnAnterior.ToolTip = "Anterior";
        _btnAnterior.Click += BtnAnterior_Click;
        //
        // _btnSiguiente
        //
        _btnSiguiente.Location = new Point(84, 8);
        _btnSiguiente.Name = "_btnSiguiente";
        _btnSiguiente.Size = new Size(28, 23);
        _btnSiguiente.TabIndex = 2;
        _btnSiguiente.Text = "";
        _btnSiguiente.ToolTip = "Siguiente";
        _btnSiguiente.Click += BtnSiguiente_Click;
        //
        // _btnFin
        //
        _btnFin.Location = new Point(116, 8);
        _btnFin.Name = "_btnFin";
        _btnFin.Size = new Size(28, 23);
        _btnFin.TabIndex = 3;
        _btnFin.Text = "";
        _btnFin.ToolTip = "Fin";
        _btnFin.Click += BtnFin_Click;
        //
        // _lblIdHuerta
        //
        _lblIdHuerta.Location = new Point(154, 14);
        _lblIdHuerta.Name = "_lblIdHuerta";
        _lblIdHuerta.Size = new Size(46, 13);
        _lblIdHuerta.TabIndex = 20;
        _lblIdHuerta.Text = "Id: (nueva)";
        //
        // _lblNombreHuerta
        // 
        _lblNombreHuerta.Location = new Point(20, 45);
        _lblNombreHuerta.Name = "_lblNombreHuerta";
        _lblNombreHuerta.Size = new Size(37, 13);
        _lblNombreHuerta.TabIndex = 1;
        _lblNombreHuerta.Text = "Huerta:";
        // 
        // _txtNombreHuerta
        // 
        _txtNombreHuerta.Location = new Point(150, 43);
        _txtNombreHuerta.Name = "_txtNombreHuerta";
        _txtNombreHuerta.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search) });
        _txtNombreHuerta.Size = new Size(300, 20);
        _txtNombreHuerta.TabIndex = 2;
        _txtNombreHuerta.ButtonClick += TxtNombreHuerta_ButtonClick;
        // 
        // _tabControl
        // 
        _tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _tabControl.Location = new Point(10, 73);
        _tabControl.Name = "_tabControl";
        _tabControl.SelectedTabPage = _tabGeneral;
        _tabControl.Size = new Size(1087, 480);
        _tabControl.TabIndex = 3;
        _tabControl.TabPages.AddRange(new XtraTabPage[] { _tabGeneral, _tabAdicional, _tabStatus });
        // 
        // _tabGeneral
        // 
        _tabGeneral.Controls.Add(_lblProductor);
        _tabGeneral.Controls.Add(_cmbProductor);
        _tabGeneral.Controls.Add(_lblUbicacion);
        _tabGeneral.Controls.Add(_txtUbicacion);
        _tabGeneral.Controls.Add(_lblPais);
        _tabGeneral.Controls.Add(_cmbPais);
        _tabGeneral.Controls.Add(_lblEstado);
        _tabGeneral.Controls.Add(_cmbEstado);
        _tabGeneral.Controls.Add(_lblMunicipio);
        _tabGeneral.Controls.Add(_cmbMunicipio);
        _tabGeneral.Controls.Add(_lblPoblacion);
        _tabGeneral.Controls.Add(_cmbPoblacion);
        _tabGeneral.Controls.Add(_lblProducto);
        _tabGeneral.Controls.Add(_cmbProducto);
        _tabGeneral.Controls.Add(_grpEncargado);
        _tabGeneral.Controls.Add(_lblObservaciones);
        _tabGeneral.Controls.Add(_txtObservaciones);
        _tabGeneral.Controls.Add(_lblRegistroSagarpa);
        _tabGeneral.Controls.Add(_txtRegistroSagarpa);
        _tabGeneral.Controls.Add(_chkGlobalGap);
        _tabGeneral.Controls.Add(_lblRegistroFda);
        _tabGeneral.Controls.Add(_txtRegistroFda);
        _tabGeneral.Controls.Add(_lblNumeroGlobalGap);
        _tabGeneral.Controls.Add(_txtNumeroGlobalGap);
        _tabGeneral.Name = "_tabGeneral";
        _tabGeneral.Size = new Size(1085, 455);
        _tabGeneral.Text = "Datos Generales";
        // 
        // _lblProductor
        // 
        _lblProductor.Location = new Point(10, 12);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(51, 13);
        _lblProductor.TabIndex = 0;
        _lblProductor.Text = "Productor:";
        // 
        // _cmbProductor
        // 
        _cmbProductor.Location = new Point(140, 10);
        _cmbProductor.Name = "_cmbProductor";
        _cmbProductor.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search), new EditorButton(ButtonPredefines.Plus) });
        _cmbProductor.Properties.NullValuePrompt = "Buscar productor...";
        _cmbProductor.Properties.ReadOnly = true;
        _cmbProductor.Size = new Size(300, 20);
        _cmbProductor.TabIndex = 1;
        _cmbProductor.ButtonClick += CmbProductor_ButtonClick;
        // 
        // _lblUbicacion
        // 
        _lblUbicacion.Location = new Point(10, 40);
        _lblUbicacion.Name = "_lblUbicacion";
        _lblUbicacion.Size = new Size(49, 13);
        _lblUbicacion.TabIndex = 2;
        _lblUbicacion.Text = "Ubicación:";
        // 
        // _txtUbicacion
        // 
        _txtUbicacion.Location = new Point(140, 38);
        _txtUbicacion.Name = "_txtUbicacion";
        _txtUbicacion.Size = new Size(300, 20);
        _txtUbicacion.TabIndex = 3;
        // 
        // _lblPais
        // 
        _lblPais.Location = new Point(10, 68);
        _lblPais.Name = "_lblPais";
        _lblPais.Size = new Size(23, 13);
        _lblPais.TabIndex = 4;
        _lblPais.Text = "País:";
        // 
        // _cmbPais
        // 
        _cmbPais.Location = new Point(140, 66);
        _cmbPais.Name = "_cmbPais";
        _cmbPais.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbPais.Properties.NullText = "Seleccionar";
        _cmbPais.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbPais.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbPais.Size = new Size(170, 20);
        _cmbPais.TabIndex = 5;
        _cmbPais.ButtonClick += CmbPais_ButtonClick;
        // 
        // _lblEstado
        // 
        _lblEstado.Location = new Point(10, 96);
        _lblEstado.Name = "_lblEstado";
        _lblEstado.Size = new Size(37, 13);
        _lblEstado.TabIndex = 6;
        _lblEstado.Text = "Estado:";
        // 
        // _cmbEstado
        // 
        _cmbEstado.Location = new Point(140, 94);
        _cmbEstado.Name = "_cmbEstado";
        _cmbEstado.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbEstado.Properties.NullText = "Seleccionar";
        _cmbEstado.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbEstado.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbEstado.Size = new Size(170, 20);
        _cmbEstado.TabIndex = 7;
        _cmbEstado.ButtonClick += CmbEstado_ButtonClick;
        // 
        // _lblMunicipio
        //
        _lblMunicipio.Location = new Point(10, 124);
        _lblMunicipio.Name = "_lblMunicipio";
        _lblMunicipio.Size = new Size(47, 13);
        _lblMunicipio.TabIndex = 8;
        _lblMunicipio.Text = "Municipio:";
        //
        // _cmbMunicipio
        //
        _cmbMunicipio.Location = new Point(140, 122);
        _cmbMunicipio.Name = "_cmbMunicipio";
        _cmbMunicipio.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbMunicipio.Properties.NullText = "Seleccionar";
        _cmbMunicipio.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbMunicipio.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbMunicipio.Size = new Size(170, 20);
        _cmbMunicipio.TabIndex = 9;
        _cmbMunicipio.ButtonClick += CmbMunicipio_ButtonClick;
        //
        // _lblPoblacion
        //
        _lblPoblacion.Location = new Point(10, 152);
        _lblPoblacion.Name = "_lblPoblacion";
        _lblPoblacion.Size = new Size(49, 13);
        _lblPoblacion.TabIndex = 10;
        _lblPoblacion.Text = "Población:";
        //
        // _cmbPoblacion
        //
        _cmbPoblacion.Location = new Point(140, 150);
        _cmbPoblacion.Name = "_cmbPoblacion";
        _cmbPoblacion.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbPoblacion.Properties.NullText = "Seleccionar";
        _cmbPoblacion.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbPoblacion.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbPoblacion.Size = new Size(170, 20);
        _cmbPoblacion.TabIndex = 11;
        _cmbPoblacion.ButtonClick += CmbPoblacion_ButtonClick;
        // 
        // _lblProducto
        // 
        _lblProducto.Location = new Point(10, 180);
        _lblProducto.Name = "_lblProducto";
        _lblProducto.Size = new Size(47, 13);
        _lblProducto.TabIndex = 12;
        _lblProducto.Text = "Producto:";
        // 
        // _cmbProducto
        // 
        _cmbProducto.Location = new Point(140, 178);
        _cmbProducto.Name = "_cmbProducto";
        _cmbProducto.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbProducto.Properties.NullText = "Seleccionar";
        _cmbProducto.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbProducto.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbProducto.Size = new Size(170, 20);
        _cmbProducto.TabIndex = 13;
        _cmbProducto.ButtonClick += CmbProducto_ButtonClick;
        // 
        // _grpEncargado
        // 
        _grpEncargado.Controls.Add(_lblEncargadoNombre);
        _grpEncargado.Controls.Add(_txtEncargadoNombre);
        _grpEncargado.Controls.Add(_lblEncargadoTelefono);
        _grpEncargado.Controls.Add(_txtEncargadoTelefono);
        _grpEncargado.Location = new Point(10, 208);
        _grpEncargado.Name = "_grpEncargado";
        _grpEncargado.Size = new Size(430, 91);
        _grpEncargado.TabIndex = 14;
        _grpEncargado.Text = "Encargado de la Huerta";
        // 
        // _lblEncargadoNombre
        // 
        _lblEncargadoNombre.Location = new Point(10, 34);
        _lblEncargadoNombre.Name = "_lblEncargadoNombre";
        _lblEncargadoNombre.Size = new Size(41, 13);
        _lblEncargadoNombre.TabIndex = 0;
        _lblEncargadoNombre.Text = "Nombre:";
        // 
        // _txtEncargadoNombre
        // 
        _txtEncargadoNombre.Location = new Point(100, 32);
        _txtEncargadoNombre.Name = "_txtEncargadoNombre";
        _txtEncargadoNombre.Size = new Size(300, 20);
        _txtEncargadoNombre.TabIndex = 1;
        // 
        // _lblEncargadoTelefono
        // 
        _lblEncargadoTelefono.Location = new Point(10, 60);
        _lblEncargadoTelefono.Name = "_lblEncargadoTelefono";
        _lblEncargadoTelefono.Size = new Size(46, 13);
        _lblEncargadoTelefono.TabIndex = 2;
        _lblEncargadoTelefono.Text = "Teléfono:";
        // 
        // _txtEncargadoTelefono
        // 
        _txtEncargadoTelefono.Location = new Point(100, 58);
        _txtEncargadoTelefono.Name = "_txtEncargadoTelefono";
        _txtEncargadoTelefono.Size = new Size(150, 20);
        _txtEncargadoTelefono.TabIndex = 3;
        // 
        // _lblObservaciones
        // 
        _lblObservaciones.Location = new Point(10, 307);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(75, 13);
        _lblObservaciones.TabIndex = 15;
        _lblObservaciones.Text = "Observaciones:";
        // 
        // _txtObservaciones
        // 
        _txtObservaciones.Location = new Point(140, 305);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(300, 20);
        _txtObservaciones.TabIndex = 16;
        // 
        // _lblRegistroSagarpa
        // 
        _lblRegistroSagarpa.Location = new Point(10, 335);
        _lblRegistroSagarpa.Name = "_lblRegistroSagarpa";
        _lblRegistroSagarpa.Size = new Size(94, 13);
        _lblRegistroSagarpa.TabIndex = 17;
        _lblRegistroSagarpa.Text = "Registro SAGARPA:";
        // 
        // _txtRegistroSagarpa
        // 
        _txtRegistroSagarpa.Location = new Point(140, 333);
        _txtRegistroSagarpa.Name = "_txtRegistroSagarpa";
        _txtRegistroSagarpa.Properties.MaxLength = 50;
        _txtRegistroSagarpa.Size = new Size(180, 20);
        _txtRegistroSagarpa.TabIndex = 18;
        // 
        // _chkGlobalGap
        // 
        _chkGlobalGap.Location = new Point(140, 363);
        _chkGlobalGap.Name = "_chkGlobalGap";
        _chkGlobalGap.Properties.Caption = "¿El productor está certificado en GlobalGap?";
        _chkGlobalGap.Size = new Size(300, 20);
        _chkGlobalGap.TabIndex = 19;
        _chkGlobalGap.CheckedChanged += ChkGlobalGap_CheckedChanged;
        // 
        // _lblRegistroFda
        // 
        _lblRegistroFda.Location = new Point(10, 391);
        _lblRegistroFda.Name = "_lblRegistroFda";
        _lblRegistroFda.Size = new Size(67, 13);
        _lblRegistroFda.TabIndex = 20;
        _lblRegistroFda.Text = "Registro FDA:";
        // 
        // _txtRegistroFda
        // 
        _txtRegistroFda.Location = new Point(140, 389);
        _txtRegistroFda.Name = "_txtRegistroFda";
        _txtRegistroFda.Properties.MaxLength = 50;
        _txtRegistroFda.Size = new Size(180, 20);
        _txtRegistroFda.TabIndex = 21;
        // 
        // _lblNumeroGlobalGap
        // 
        _lblNumeroGlobalGap.Location = new Point(10, 419);
        _lblNumeroGlobalGap.Name = "_lblNumeroGlobalGap";
        _lblNumeroGlobalGap.Size = new Size(104, 13);
        _lblNumeroGlobalGap.TabIndex = 22;
        _lblNumeroGlobalGap.Text = "No. GlobalGap (GGN):";
        // 
        // _txtNumeroGlobalGap
        // 
        _txtNumeroGlobalGap.Location = new Point(140, 417);
        _txtNumeroGlobalGap.Name = "_txtNumeroGlobalGap";
        _txtNumeroGlobalGap.Properties.MaxLength = 50;
        _txtNumeroGlobalGap.Size = new Size(180, 20);
        _txtNumeroGlobalGap.TabIndex = 23;
        // 
        // _tabAdicional
        // 
        _tabAdicional.Controls.Add(panelControl2);
        _tabAdicional.Controls.Add(panelControl3);
        _tabAdicional.Controls.Add(panelControl1);
        _tabAdicional.Name = "_tabAdicional";
        _tabAdicional.Padding = new Padding(5);
        _tabAdicional.Size = new Size(1085, 455);
        _tabAdicional.Text = "Información Adicional";
        // 
        // panelControl1
        // 
        panelControl1.Controls.Add(_lblSuperficie);
        panelControl1.Controls.Add(_spnLongitud);
        panelControl1.Controls.Add(_spnSuperficie);
        panelControl1.Controls.Add(_lblLongitud);
        panelControl1.Controls.Add(_lblSuperficieUnidad);
        panelControl1.Controls.Add(_spnLatitud);
        panelControl1.Controls.Add(_lblAltura);
        panelControl1.Controls.Add(_lblLatitud);
        panelControl1.Controls.Add(_spnAltura);
        panelControl1.Controls.Add(_lblPorcentajeUnidad);
        panelControl1.Controls.Add(_lblAlturaUnidad);
        panelControl1.Controls.Add(_spnPorcentajeMecanizacion);
        panelControl1.Controls.Add(_lblNumeroArboles);
        panelControl1.Controls.Add(_lblPorcentajeMecanizacion);
        panelControl1.Controls.Add(_spnNumeroArboles);
        panelControl1.Controls.Add(_cmbSistemaRiego);
        panelControl1.Controls.Add(_lblEdadArboles);
        panelControl1.Controls.Add(_lblSistemaRiego);
        panelControl1.Controls.Add(_spnEdadArboles);
        panelControl1.Dock = DockStyle.Left;
        panelControl1.Location = new Point(5, 5);
        panelControl1.Name = "panelControl1";
        panelControl1.Size = new Size(434, 445);
        panelControl1.TabIndex = 19;
        // 
        // _lblSuperficie
        // 
        _lblSuperficie.Location = new Point(25, 25);
        _lblSuperficie.Name = "_lblSuperficie";
        _lblSuperficie.Size = new Size(51, 13);
        _lblSuperficie.TabIndex = 0;
        _lblSuperficie.Text = "Superficie:";
        // 
        // _spnLongitud
        // 
        _spnLongitud.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnLongitud.Location = new Point(150, 226);
        _spnLongitud.Name = "_spnLongitud";
        _spnLongitud.Properties.Mask.EditMask = "n06";
        _spnLongitud.Properties.MaxValue = new decimal(new int[] { 180, 0, 0, 0 });
        _spnLongitud.Properties.MinValue = new decimal(new int[] { 180, 0, 0, int.MinValue });
        _spnLongitud.Size = new Size(120, 20);
        _spnLongitud.TabIndex = 18;
        // 
        // _spnSuperficie
        // 
        _spnSuperficie.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnSuperficie.Location = new Point(150, 18);
        _spnSuperficie.Name = "_spnSuperficie";
        _spnSuperficie.Properties.Mask.EditMask = "N02";
        _spnSuperficie.Size = new Size(100, 20);
        _spnSuperficie.TabIndex = 1;
        // 
        // _lblLongitud
        // 
        _lblLongitud.Location = new Point(25, 233);
        _lblLongitud.Name = "_lblLongitud";
        _lblLongitud.Size = new Size(45, 13);
        _lblLongitud.TabIndex = 17;
        _lblLongitud.Text = "Longitud:";
        // 
        // _lblSuperficieUnidad
        // 
        _lblSuperficieUnidad.Location = new Point(263, 25);
        _lblSuperficieUnidad.Name = "_lblSuperficieUnidad";
        _lblSuperficieUnidad.Size = new Size(14, 13);
        _lblSuperficieUnidad.TabIndex = 2;
        _lblSuperficieUnidad.Text = "HA";
        // 
        // _spnLatitud
        // 
        _spnLatitud.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnLatitud.Location = new Point(150, 198);
        _spnLatitud.Name = "_spnLatitud";
        _spnLatitud.Properties.Mask.EditMask = "n06";
        _spnLatitud.Properties.MaxValue = new decimal(new int[] { 90, 0, 0, 0 });
        _spnLatitud.Properties.MinValue = new decimal(new int[] { 90, 0, 0, int.MinValue });
        _spnLatitud.Size = new Size(120, 20);
        _spnLatitud.TabIndex = 16;
        // 
        // _lblAltura
        // 
        _lblAltura.Location = new Point(25, 55);
        _lblAltura.Name = "_lblAltura";
        _lblAltura.Size = new Size(33, 13);
        _lblAltura.TabIndex = 3;
        _lblAltura.Text = "Altura:";
        // 
        // _lblLatitud
        // 
        _lblLatitud.Location = new Point(25, 205);
        _lblLatitud.Name = "_lblLatitud";
        _lblLatitud.Size = new Size(37, 13);
        _lblLatitud.TabIndex = 15;
        _lblLatitud.Text = "Latitud:";
        // 
        // _spnAltura
        // 
        _spnAltura.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnAltura.Location = new Point(150, 48);
        _spnAltura.Name = "_spnAltura";
        _spnAltura.Properties.Mask.EditMask = "N02";
        _spnAltura.Size = new Size(100, 20);
        _spnAltura.TabIndex = 4;
        // 
        // _lblPorcentajeUnidad
        // 
        _lblPorcentajeUnidad.Location = new Point(263, 175);
        _lblPorcentajeUnidad.Name = "_lblPorcentajeUnidad";
        _lblPorcentajeUnidad.Size = new Size(11, 13);
        _lblPorcentajeUnidad.TabIndex = 14;
        _lblPorcentajeUnidad.Text = "%";
        // 
        // _lblAlturaUnidad
        // 
        _lblAlturaUnidad.Location = new Point(263, 55);
        _lblAlturaUnidad.Name = "_lblAlturaUnidad";
        _lblAlturaUnidad.Size = new Size(21, 13);
        _lblAlturaUnidad.TabIndex = 5;
        _lblAlturaUnidad.Text = "mts.";
        // 
        // _spnPorcentajeMecanizacion
        // 
        _spnPorcentajeMecanizacion.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnPorcentajeMecanizacion.Location = new Point(150, 168);
        _spnPorcentajeMecanizacion.Name = "_spnPorcentajeMecanizacion";
        _spnPorcentajeMecanizacion.Properties.Mask.EditMask = "N02";
        _spnPorcentajeMecanizacion.Size = new Size(100, 20);
        _spnPorcentajeMecanizacion.TabIndex = 13;
        // 
        // _lblNumeroArboles
        // 
        _lblNumeroArboles.Location = new Point(25, 85);
        _lblNumeroArboles.Name = "_lblNumeroArboles";
        _lblNumeroArboles.Size = new Size(95, 13);
        _lblNumeroArboles.TabIndex = 6;
        _lblNumeroArboles.Text = "Número de Árboles:";
        // 
        // _lblPorcentajeMecanizacion
        // 
        _lblPorcentajeMecanizacion.Location = new Point(25, 175);
        _lblPorcentajeMecanizacion.Name = "_lblPorcentajeMecanizacion";
        _lblPorcentajeMecanizacion.Size = new Size(96, 13);
        _lblPorcentajeMecanizacion.TabIndex = 12;
        _lblPorcentajeMecanizacion.Text = "% de Mecanización:";
        // 
        // _spnNumeroArboles
        // 
        _spnNumeroArboles.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnNumeroArboles.Location = new Point(150, 78);
        _spnNumeroArboles.Name = "_spnNumeroArboles";
        _spnNumeroArboles.Properties.IsFloatValue = false;
        _spnNumeroArboles.Properties.Mask.EditMask = "N00";
        _spnNumeroArboles.Size = new Size(100, 20);
        _spnNumeroArboles.TabIndex = 7;
        // 
        // _cmbSistemaRiego
        // 
        _cmbSistemaRiego.Location = new Point(150, 138);
        _cmbSistemaRiego.Name = "_cmbSistemaRiego";
        _cmbSistemaRiego.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbSistemaRiego.Properties.NullText = "Seleccionar";
        _cmbSistemaRiego.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbSistemaRiego.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbSistemaRiego.Size = new Size(270, 20);
        _cmbSistemaRiego.TabIndex = 11;
        _cmbSistemaRiego.ButtonClick += CmbSistemaRiego_ButtonClick;
        // 
        // _lblEdadArboles
        // 
        _lblEdadArboles.Location = new Point(25, 115);
        _lblEdadArboles.Name = "_lblEdadArboles";
        _lblEdadArboles.Size = new Size(98, 13);
        _lblEdadArboles.TabIndex = 8;
        _lblEdadArboles.Text = "Edad de los Árboles:";
        // 
        // _lblSistemaRiego
        // 
        _lblSistemaRiego.Location = new Point(25, 145);
        _lblSistemaRiego.Name = "_lblSistemaRiego";
        _lblSistemaRiego.Size = new Size(86, 13);
        _lblSistemaRiego.TabIndex = 10;
        _lblSistemaRiego.Text = "Sistema de Riego:";
        // 
        // _spnEdadArboles
        // 
        _spnEdadArboles.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnEdadArboles.Location = new Point(150, 108);
        _spnEdadArboles.Name = "_spnEdadArboles";
        _spnEdadArboles.Properties.IsFloatValue = false;
        _spnEdadArboles.Properties.Mask.EditMask = "N00";
        _spnEdadArboles.Size = new Size(100, 20);
        _spnEdadArboles.TabIndex = 9;
        // 
        // _tabStatus
        // 
        _tabStatus.Controls.Add(_lblStatusHuerta);
        _tabStatus.Controls.Add(_cmbStatusHuerta);
        _tabStatus.Controls.Add(_lblFechaCambioStatus);
        _tabStatus.Controls.Add(_dtFechaCambioStatus);
        _tabStatus.Controls.Add(_lblAniosEnStatus);
        _tabStatus.Controls.Add(_txtAniosEnStatus);
        _tabStatus.Controls.Add(_lblEstatusActivo);
        _tabStatus.Controls.Add(_cmbEstatusActivo);
        _tabStatus.Controls.Add(_lblFechaVencimiento);
        _tabStatus.Controls.Add(_dtFechaVencimiento);
        _tabStatus.Controls.Add(_lblDiasVencimiento);
        _tabStatus.Controls.Add(_txtDiasVencimiento);
        _tabStatus.Name = "_tabStatus";
        _tabStatus.Size = new Size(1085, 455);
        _tabStatus.Text = "Estatus de la Huerta";
        // 
        // _lblStatusHuerta
        // 
        _lblStatusHuerta.Location = new Point(10, 12);
        _lblStatusHuerta.Name = "_lblStatusHuerta";
        _lblStatusHuerta.Size = new Size(40, 13);
        _lblStatusHuerta.TabIndex = 0;
        _lblStatusHuerta.Text = "Estatus:";
        // 
        // _cmbStatusHuerta
        // 
        _cmbStatusHuerta.Location = new Point(140, 10);
        _cmbStatusHuerta.Name = "_cmbStatusHuerta";
        _cmbStatusHuerta.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbStatusHuerta.Properties.NullText = "Seleccionar";
        _cmbStatusHuerta.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbStatusHuerta.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbStatusHuerta.Size = new Size(270, 20);
        _cmbStatusHuerta.TabIndex = 1;
        _cmbStatusHuerta.ButtonClick += CmbStatusHuerta_ButtonClick;
        // 
        // _lblFechaCambioStatus
        // 
        _lblFechaCambioStatus.Location = new Point(10, 42);
        _lblFechaCambioStatus.Name = "_lblFechaCambioStatus";
        _lblFechaCambioStatus.Size = new Size(108, 13);
        _lblFechaCambioStatus.TabIndex = 2;
        _lblFechaCambioStatus.Text = "Fecha cambio Estatus:";
        // 
        // _dtFechaCambioStatus
        // 
        _dtFechaCambioStatus.EditValue = null;
        _dtFechaCambioStatus.Location = new Point(140, 40);
        _dtFechaCambioStatus.Name = "_dtFechaCambioStatus";
        _dtFechaCambioStatus.Size = new Size(120, 20);
        _dtFechaCambioStatus.TabIndex = 3;
        // 
        // _lblAniosEnStatus
        // 
        _lblAniosEnStatus.Location = new Point(10, 72);
        _lblAniosEnStatus.Name = "_lblAniosEnStatus";
        _lblAniosEnStatus.Size = new Size(115, 13);
        _lblAniosEnStatus.TabIndex = 4;
        _lblAniosEnStatus.Text = "Años en Estatus Actual:";
        // 
        // _txtAniosEnStatus
        // 
        _txtAniosEnStatus.Location = new Point(140, 70);
        _txtAniosEnStatus.Name = "_txtAniosEnStatus";
        _txtAniosEnStatus.Properties.ReadOnly = true;
        _txtAniosEnStatus.Size = new Size(120, 20);
        _txtAniosEnStatus.TabIndex = 5;
        // 
        // _lblEstatusActivo
        // 
        _lblEstatusActivo.Location = new Point(10, 102);
        _lblEstatusActivo.Name = "_lblEstatusActivo";
        _lblEstatusActivo.Size = new Size(34, 13);
        _lblEstatusActivo.TabIndex = 6;
        _lblEstatusActivo.Text = "Activo:";
        // 
        // _cmbEstatusActivo
        // 
        _cmbEstatusActivo.Location = new Point(140, 100);
        _cmbEstatusActivo.Name = "_cmbEstatusActivo";
        _cmbEstatusActivo.Properties.Items.AddRange(new object[] { "Activa", "Baja" });
        _cmbEstatusActivo.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbEstatusActivo.Size = new Size(120, 20);
        _cmbEstatusActivo.TabIndex = 7;
        // 
        // _lblFechaVencimiento
        // 
        _lblFechaVencimiento.Location = new Point(10, 132);
        _lblFechaVencimiento.Name = "_lblFechaVencimiento";
        _lblFechaVencimiento.Size = new Size(93, 13);
        _lblFechaVencimiento.TabIndex = 8;
        _lblFechaVencimiento.Text = "Fecha Vencimiento:";
        // 
        // _dtFechaVencimiento
        // 
        _dtFechaVencimiento.EditValue = null;
        _dtFechaVencimiento.Location = new Point(140, 130);
        _dtFechaVencimiento.Name = "_dtFechaVencimiento";
        _dtFechaVencimiento.Size = new Size(120, 20);
        _dtFechaVencimiento.TabIndex = 9;
        // 
        // _lblDiasVencimiento
        // 
        _lblDiasVencimiento.Location = new Point(10, 162);
        _lblDiasVencimiento.Name = "_lblDiasVencimiento";
        _lblDiasVencimiento.Size = new Size(109, 13);
        _lblDiasVencimiento.TabIndex = 10;
        _lblDiasVencimiento.Text = "Días para Vencimiento:";
        // 
        // _txtDiasVencimiento
        // 
        _txtDiasVencimiento.Location = new Point(140, 160);
        _txtDiasVencimiento.Name = "_txtDiasVencimiento";
        _txtDiasVencimiento.Properties.ReadOnly = true;
        _txtDiasVencimiento.Size = new Size(120, 20);
        _txtDiasVencimiento.TabIndex = 11;
        // 
        // _btnNuevo
        // 
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Location = new Point(20, 560);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.TabIndex = 4;
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        // 
        // _btnEliminar
        // 
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Location = new Point(202, 560);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.TabIndex = 6;
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        // 
        // _btnGuardar
        // 
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(116, 560);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 5;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        // 
        // _btnCancelar
        // 
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(1007, 560);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.TabIndex = 7;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        // 
        // panelControl3
        // 
        panelControl3.Controls.Add(btRecargarMapa);
        panelControl3.Controls.Add(comboBoxEdit1);
        panelControl3.Controls.Add(chk_mostrar);
        panelControl3.Dock = DockStyle.Top;
        panelControl3.Location = new Point(439, 5);
        panelControl3.Name = "panelControl3";
        panelControl3.Size = new Size(641, 49);
        panelControl3.TabIndex = 21;
        // 
        // panelControl2
        //
        panelControl2.Controls.Add(_mapHuerta);
        panelControl2.Dock = DockStyle.Fill;
        panelControl2.Location = new Point(439, 54);
        panelControl2.Name = "panelControl2";
        panelControl2.Size = new Size(641, 396);
        panelControl2.TabIndex = 22;
        //
        // _mapHuerta
        //
        _mapHuerta.Dock = DockStyle.Fill;
        _mapHuerta.Location = new Point(2, 2);
        _mapHuerta.Name = "_mapHuerta";
        _mapHuerta.Size = new Size(637, 392);
        _mapHuerta.TabIndex = 0;
        //
        // chk_mostrar
        //
        chk_mostrar.EditValue = true;
        chk_mostrar.Location = new Point(17, 14);
        chk_mostrar.Name = "chk_mostrar";
        chk_mostrar.Properties.Caption = "Mostrar Coordenada";
        chk_mostrar.Size = new Size(127, 20);
        chk_mostrar.TabIndex = 0;
        chk_mostrar.CheckedChanged += ChkMostrar_CheckedChanged;
        //
        // comboBoxEdit1
        //
        comboBoxEdit1.Location = new Point(160, 14);
        comboBoxEdit1.Name = "comboBoxEdit1";
        comboBoxEdit1.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
        comboBoxEdit1.Properties.Items.AddRange(new object[] { "Google Maps Satélite", "Google Maps Callejero", "Google Maps Híbrido", "OpenClycleMap" });
        comboBoxEdit1.Size = new Size(182, 20);
        comboBoxEdit1.TabIndex = 1;
        comboBoxEdit1.SelectedIndex = 2;
        //
        // btRecargarMapa
        //
        btRecargarMapa.ImageOptions.Image = (Image)resources.GetObject("btRecargarMapa.ImageOptions.Image");
        btRecargarMapa.Location = new Point(369, 12);
        btRecargarMapa.Name = "btRecargarMapa";
        btRecargarMapa.Size = new Size(130, 23);
        btRecargarMapa.TabIndex = 2;
        btRecargarMapa.Text = "Recargar Mapas";
        btRecargarMapa.Click += BtRecargarMapa_Click;
        //
        // HuertaEditarForm
        // 
        AcceptButton = _btnGuardar;
        ClientSize = new Size(1107, 595);
        Controls.Add(_btnInicio);
        Controls.Add(_btnAnterior);
        Controls.Add(_btnSiguiente);
        Controls.Add(_btnFin);
        Controls.Add(_lblIdHuerta);
        Controls.Add(_lblNombreHuerta);
        Controls.Add(_txtNombreHuerta);
        Controls.Add(_tabControl);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCancelar);
        MinimumSize = new Size(492, 500);
        Name = "HuertaEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Huerta";
        ((System.ComponentModel.ISupportInitialize)_txtNombreHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).EndInit();
        _tabControl.ResumeLayout(false);
        _tabGeneral.ResumeLayout(false);
        _tabGeneral.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_cmbProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtUbicacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPoblacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProducto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpEncargado).EndInit();
        _grpEncargado.ResumeLayout(false);
        _grpEncargado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_txtEncargadoNombre.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEncargadoTelefono.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkGlobalGap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroFda.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumeroGlobalGap.Properties).EndInit();
        _tabAdicional.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
        panelControl1.ResumeLayout(false);
        panelControl1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_spnLongitud.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnSuperficie.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnLatitud.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnAltura.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPorcentajeMecanizacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnNumeroArboles.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbSistemaRiego.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnEdadArboles.Properties).EndInit();
        _tabStatus.ResumeLayout(false);
        _tabStatus.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_cmbStatusHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaCambioStatus.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaCambioStatus.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtAniosEnStatus.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstatusActivo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaVencimiento.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaVencimiento.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDiasVencimiento.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)panelControl3).EndInit();
        panelControl3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)panelControl2).EndInit();
        ((System.ComponentModel.ISupportInitialize)chk_mostrar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)comboBoxEdit1.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private PanelControl panelControl1;
    private PanelControl panelControl2;
    private PanelControl panelControl3;
    private SimpleButton btRecargarMapa;
    private ComboBoxEdit comboBoxEdit1;
    private CheckEdit chk_mostrar;
}
