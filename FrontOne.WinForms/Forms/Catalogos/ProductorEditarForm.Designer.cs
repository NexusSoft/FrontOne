using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class ProductorEditarForm
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
    private LabelControl _lblClave;
    private TextEdit _txtClave;
    private LabelControl _lblFechaRegistro;
    private TextEdit _txtFechaRegistro;
    private LabelControl _lblNombreProductor;
    private ButtonEdit _txtNombreProductor;

    // Tabs
    private XtraTabControl _tabControl;
    private XtraTabPage _tabGeneral;
    private XtraTabPage _tabHuertas;
    private GridControl _gridHuertas;
    private GridView _gridViewHuertas;

    // Pestaña General
    private LabelControl _lblDomicilio;
    private TextEdit _txtDomicilio;
    private LabelControl _lblColonia;
    private TextEdit _txtColonia;
    private LabelControl _lblCodigoPostal;
    private TextEdit _txtCodigoPostal;
    private LabelControl _lblPais;
    private LookUpEdit _cmbPais;
    private LabelControl _lblEstado;
    private LookUpEdit _cmbEstado;
    private LabelControl _lblPoblacion;
    private LookUpEdit _cmbPoblacion;
    private LabelControl _lblMunicipio;
    private LookUpEdit _cmbMunicipio;
    private LabelControl _lblRfc;
    private TextEdit _txtRfc;
    private LabelControl _lblTelefono;
    private TextEdit _txtTelefono;
    private LabelControl _lblCelular;
    private TextEdit _txtCelular;
    private LabelControl _lblEmail;
    private TextEdit _txtEmail;
    private LabelControl _lblOrganizacion;
    private TextEdit _txtOrganizacion;
    private LabelControl _lblObservaciones;
    private TextEdit _txtObservaciones;
    private LabelControl _lblUsuario;
    private TextEdit _txtUsuario;
    private LabelControl _lblPassword;
    private TextEdit _txtPassword;
    private LabelControl _lblDiasCredito;
    private SpinEdit _spnDiasCredito;
    private CheckEdit _chkObsoleto;

    // Botones
    private SimpleButton _btnNuevo;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductorEditarForm));
        _btnInicio = new SimpleButton();
        _btnAnterior = new SimpleButton();
        _btnSiguiente = new SimpleButton();
        _btnFin = new SimpleButton();
        _lblClave = new LabelControl();
        _txtClave = new TextEdit();
        _lblFechaRegistro = new LabelControl();
        _txtFechaRegistro = new TextEdit();
        _lblNombreProductor = new LabelControl();
        _txtNombreProductor = new ButtonEdit();

        _tabControl = new XtraTabControl();
        _tabGeneral = new XtraTabPage();
        _tabHuertas = new XtraTabPage();
        _gridHuertas = new GridControl();
        _gridViewHuertas = new GridView(_gridHuertas);

        _lblDomicilio = new LabelControl();
        _txtDomicilio = new TextEdit();
        _lblColonia = new LabelControl();
        _txtColonia = new TextEdit();
        _lblCodigoPostal = new LabelControl();
        _txtCodigoPostal = new TextEdit();
        _lblPais = new LabelControl();
        _cmbPais = new LookUpEdit();
        _lblEstado = new LabelControl();
        _cmbEstado = new LookUpEdit();
        _lblPoblacion = new LabelControl();
        _cmbPoblacion = new LookUpEdit();
        _lblMunicipio = new LabelControl();
        _cmbMunicipio = new LookUpEdit();
        _lblRfc = new LabelControl();
        _txtRfc = new TextEdit();
        _lblTelefono = new LabelControl();
        _txtTelefono = new TextEdit();
        _lblCelular = new LabelControl();
        _txtCelular = new TextEdit();
        _lblEmail = new LabelControl();
        _txtEmail = new TextEdit();
        _lblOrganizacion = new LabelControl();
        _txtOrganizacion = new TextEdit();
        _lblObservaciones = new LabelControl();
        _txtObservaciones = new TextEdit();
        _lblUsuario = new LabelControl();
        _txtUsuario = new TextEdit();
        _lblPassword = new LabelControl();
        _txtPassword = new TextEdit();
        _lblDiasCredito = new LabelControl();
        _spnDiasCredito = new SpinEdit();
        _chkObsoleto = new CheckEdit();

        _btnNuevo = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_txtClave.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFechaRegistro.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombreProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridHuertas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewHuertas).BeginInit();
        _tabControl.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtDomicilio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtColonia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoPostal.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPoblacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRfc.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTelefono.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCelular.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtOrganizacion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtUsuario.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnDiasCredito.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkObsoleto.Properties).BeginInit();
        SuspendLayout();
        //
        // Encabezado: navegación
        //
        _btnInicio.Location = new Point(20, 8);
        _btnInicio.Name = "_btnInicio";
        _btnInicio.Size = new Size(28, 23);
        _btnInicio.Text = "";
        _btnInicio.ToolTip = "Inicio";
        _btnInicio.Click += BtnInicio_Click;
        _btnAnterior.Location = new Point(52, 8);
        _btnAnterior.Name = "_btnAnterior";
        _btnAnterior.Size = new Size(28, 23);
        _btnAnterior.Text = "";
        _btnAnterior.ToolTip = "Anterior";
        _btnAnterior.Click += BtnAnterior_Click;
        _btnSiguiente.Location = new Point(84, 8);
        _btnSiguiente.Name = "_btnSiguiente";
        _btnSiguiente.Size = new Size(28, 23);
        _btnSiguiente.Text = "";
        _btnSiguiente.ToolTip = "Siguiente";
        _btnSiguiente.Click += BtnSiguiente_Click;
        _btnFin.Location = new Point(116, 8);
        _btnFin.Name = "_btnFin";
        _btnFin.Size = new Size(28, 23);
        _btnFin.Text = "";
        _btnFin.ToolTip = "Fin";
        _btnFin.Click += BtnFin_Click;
        //
        // Encabezado: Clave / Fecha de Registro
        //
        _lblClave.Location = new Point(20, 45);
        _lblClave.Name = "_lblClave";
        _lblClave.Size = new Size(31, 13);
        _lblClave.Text = "Clave:";
        _txtClave.Location = new Point(150, 43);
        _txtClave.Name = "_txtClave";
        _txtClave.Properties.ReadOnly = true;
        _txtClave.Size = new Size(90, 20);
        _lblFechaRegistro.Location = new Point(250, 45);
        _lblFechaRegistro.Name = "_lblFechaRegistro";
        _lblFechaRegistro.Size = new Size(88, 13);
        _lblFechaRegistro.Text = "Fecha de Registro:";
        _txtFechaRegistro.Location = new Point(345, 43);
        _txtFechaRegistro.Name = "_txtFechaRegistro";
        _txtFechaRegistro.Properties.ReadOnly = true;
        _txtFechaRegistro.Size = new Size(105, 20);
        //
        // Encabezado: Nombre del Productor
        //
        _lblNombreProductor.Location = new Point(20, 73);
        _lblNombreProductor.Name = "_lblNombreProductor";
        _lblNombreProductor.Size = new Size(108, 13);
        _lblNombreProductor.Text = "Nombre del Productor:";
        _txtNombreProductor.Location = new Point(150, 71);
        _txtNombreProductor.Name = "_txtNombreProductor";
        _txtNombreProductor.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Search));
        _txtNombreProductor.Size = new Size(300, 20);
        _txtNombreProductor.ButtonClick += TxtNombreProductor_ButtonClick;
        //
        // _tabControl
        //
        _tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _tabControl.Location = new Point(10, 103);
        _tabControl.Name = "_tabControl";
        _tabControl.SelectedTabPage = _tabGeneral;
        _tabControl.Size = new Size(800, 520);
        _tabControl.TabPages.AddRange(new XtraTabPage[] { _tabGeneral, _tabHuertas });
        //
        // _tabGeneral
        //
        _tabGeneral.Name = "_tabGeneral";
        _tabGeneral.Text = "General";
        _tabGeneral.Controls.Add(_lblDomicilio);
        _tabGeneral.Controls.Add(_txtDomicilio);
        _tabGeneral.Controls.Add(_lblColonia);
        _tabGeneral.Controls.Add(_txtColonia);
        _tabGeneral.Controls.Add(_lblCodigoPostal);
        _tabGeneral.Controls.Add(_txtCodigoPostal);
        _tabGeneral.Controls.Add(_lblPais);
        _tabGeneral.Controls.Add(_cmbPais);
        _tabGeneral.Controls.Add(_lblEstado);
        _tabGeneral.Controls.Add(_cmbEstado);
        _tabGeneral.Controls.Add(_lblMunicipio);
        _tabGeneral.Controls.Add(_cmbMunicipio);
        _tabGeneral.Controls.Add(_lblPoblacion);
        _tabGeneral.Controls.Add(_cmbPoblacion);
        _tabGeneral.Controls.Add(_lblRfc);
        _tabGeneral.Controls.Add(_txtRfc);
        _tabGeneral.Controls.Add(_lblTelefono);
        _tabGeneral.Controls.Add(_txtTelefono);
        _tabGeneral.Controls.Add(_lblCelular);
        _tabGeneral.Controls.Add(_txtCelular);
        _tabGeneral.Controls.Add(_lblEmail);
        _tabGeneral.Controls.Add(_txtEmail);
        _tabGeneral.Controls.Add(_lblOrganizacion);
        _tabGeneral.Controls.Add(_txtOrganizacion);
        _tabGeneral.Controls.Add(_lblObservaciones);
        _tabGeneral.Controls.Add(_txtObservaciones);
        _tabGeneral.Controls.Add(_lblUsuario);
        _tabGeneral.Controls.Add(_txtUsuario);
        _tabGeneral.Controls.Add(_lblPassword);
        _tabGeneral.Controls.Add(_txtPassword);
        _tabGeneral.Controls.Add(_lblDiasCredito);
        _tabGeneral.Controls.Add(_spnDiasCredito);
        _tabGeneral.Controls.Add(_chkObsoleto);
        //
        // _tabHuertas
        //
        _tabHuertas.Name = "_tabHuertas";
        _tabHuertas.Text = "Huertas";
        _tabHuertas.Controls.Add(_gridHuertas);
        //
        // _gridHuertas
        //
        _gridHuertas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _gridHuertas.Location = new Point(10, 10);
        _gridHuertas.MainView = _gridViewHuertas;
        _gridHuertas.Name = "_gridHuertas";
        _gridHuertas.Size = new Size(780, 500);
        _gridHuertas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewHuertas });
        //
        // _gridViewHuertas
        //
        _gridViewHuertas.GridControl = _gridHuertas;
        _gridViewHuertas.Name = "_gridViewHuertas";
        _gridViewHuertas.OptionsBehavior.Editable = false;
        _gridViewHuertas.OptionsFind.AlwaysVisible = true;
        _gridViewHuertas.OptionsSelection.MultiSelect = false;
        _gridViewHuertas.OptionsView.ShowGroupPanel = false;
        _gridViewHuertas.OptionsBehavior.AutoPopulateColumns = false;
        _gridViewHuertas.OptionsView.ColumnAutoWidth = false;
        _gridViewHuertas.Columns.AddRange(new GridColumn[]
        {
            new GridColumn { FieldName = "Nombre", Caption = "Nombre de la Huerta", Visible = true, VisibleIndex = 0, Width = 160 },
            new GridColumn { FieldName = "RegistroSagarpa", Caption = "Registro SAGARPA", Visible = true, VisibleIndex = 1, Width = 120 },
            new GridColumn { FieldName = "Pais", Caption = "País", Visible = true, VisibleIndex = 2, Width = 90 },
            new GridColumn { FieldName = "Estado", Caption = "Estado", Visible = true, VisibleIndex = 3, Width = 100 },
            new GridColumn { FieldName = "Poblacion", Caption = "Población", Visible = true, VisibleIndex = 4, Width = 120 },
            new GridColumn { FieldName = "Municipio", Caption = "Municipio", Visible = true, VisibleIndex = 5, Width = 110 },
            new GridColumn { FieldName = "EstadoHuerta", Caption = "Estatus de la Huerta", Visible = true, VisibleIndex = 6, Width = 110 },
        });
        //
        // Domicilio
        //
        _lblDomicilio.Location = new Point(10, 10);
        _lblDomicilio.Name = "_lblDomicilio";
        _lblDomicilio.Size = new Size(48, 13);
        _lblDomicilio.Text = "Domicilio:";
        _txtDomicilio.Location = new Point(140, 8);
        _txtDomicilio.Name = "_txtDomicilio";
        _txtDomicilio.Size = new Size(300, 20);
        //
        // Colonia
        //
        _lblColonia.Location = new Point(10, 38);
        _lblColonia.Name = "_lblColonia";
        _lblColonia.Size = new Size(40, 13);
        _lblColonia.Text = "Colonia:";
        _txtColonia.Location = new Point(140, 36);
        _txtColonia.Name = "_txtColonia";
        _txtColonia.Size = new Size(300, 20);
        //
        // Código Postal
        //
        _lblCodigoPostal.Location = new Point(10, 66);
        _lblCodigoPostal.Name = "_lblCodigoPostal";
        _lblCodigoPostal.Size = new Size(72, 13);
        _lblCodigoPostal.Text = "Código Postal:";
        _txtCodigoPostal.Location = new Point(140, 64);
        _txtCodigoPostal.Name = "_txtCodigoPostal";
        _txtCodigoPostal.Properties.MaxLength = 10;
        _txtCodigoPostal.Size = new Size(90, 20);
        //
        // País
        //
        _lblPais.Location = new Point(10, 94);
        _lblPais.Name = "_lblPais";
        _lblPais.Size = new Size(21, 13);
        _lblPais.Text = "País:";
        _cmbPais.Location = new Point(140, 92);
        _cmbPais.Name = "_cmbPais";
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPais.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPais.Properties.NullText = "Seleccionar";
        _cmbPais.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbPais.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbPais.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbPais.Size = new Size(260, 20);
        _cmbPais.ButtonClick += CmbPais_ButtonClick;
        //
        // Estado
        //
        _lblEstado.Location = new Point(10, 122);
        _lblEstado.Name = "_lblEstado";
        _lblEstado.Size = new Size(37, 13);
        _lblEstado.Text = "Estado:";
        _cmbEstado.Location = new Point(140, 120);
        _cmbEstado.Name = "_cmbEstado";
        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbEstado.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbEstado.Properties.NullText = "Seleccionar";
        _cmbEstado.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbEstado.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbEstado.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbEstado.Size = new Size(260, 20);
        _cmbEstado.ButtonClick += CmbEstado_ButtonClick;
        //
        // Municipio
        //
        _lblMunicipio.Location = new Point(10, 150);
        _lblMunicipio.Name = "_lblMunicipio";
        _lblMunicipio.Size = new Size(53, 13);
        _lblMunicipio.Text = "Municipio:";
        _cmbMunicipio.Location = new Point(140, 148);
        _cmbMunicipio.Name = "_cmbMunicipio";
        _cmbMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbMunicipio.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbMunicipio.Properties.NullText = "Seleccionar";
        _cmbMunicipio.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbMunicipio.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbMunicipio.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbMunicipio.Size = new Size(260, 20);
        _cmbMunicipio.ButtonClick += CmbMunicipio_ButtonClick;
        //
        // Población
        //
        _lblPoblacion.Location = new Point(10, 178);
        _lblPoblacion.Name = "_lblPoblacion";
        _lblPoblacion.Size = new Size(55, 13);
        _lblPoblacion.Text = "Población:";
        _cmbPoblacion.Location = new Point(140, 176);
        _cmbPoblacion.Name = "_cmbPoblacion";
        _cmbPoblacion.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
        _cmbPoblacion.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
        _cmbPoblacion.Properties.NullText = "Seleccionar";
        _cmbPoblacion.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbPoblacion.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbPoblacion.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbPoblacion.Size = new Size(260, 20);
        _cmbPoblacion.ButtonClick += CmbPoblacion_ButtonClick;
        //
        // R.F.C.
        //
        _lblRfc.Location = new Point(10, 206);
        _lblRfc.Name = "_lblRfc";
        _lblRfc.Size = new Size(33, 13);
        _lblRfc.Text = "R.F.C.:";
        _txtRfc.Location = new Point(140, 204);
        _txtRfc.Name = "_txtRfc";
        _txtRfc.Properties.MaxLength = 20;
        _txtRfc.Size = new Size(150, 20);
        //
        // Teléfono
        //
        _lblTelefono.Location = new Point(10, 234);
        _lblTelefono.Name = "_lblTelefono";
        _lblTelefono.Size = new Size(44, 13);
        _lblTelefono.Text = "Teléfono:";
        _txtTelefono.Location = new Point(140, 232);
        _txtTelefono.Name = "_txtTelefono";
        _txtTelefono.Size = new Size(300, 20);
        //
        // Celular
        //
        _lblCelular.Location = new Point(10, 262);
        _lblCelular.Name = "_lblCelular";
        _lblCelular.Size = new Size(38, 13);
        _lblCelular.Text = "Celular:";
        _txtCelular.Location = new Point(140, 260);
        _txtCelular.Name = "_txtCelular";
        _txtCelular.Size = new Size(150, 20);
        //
        // e-mail
        //
        _lblEmail.Location = new Point(10, 290);
        _lblEmail.Name = "_lblEmail";
        _lblEmail.Size = new Size(35, 13);
        _lblEmail.Text = "e-mail:";
        _txtEmail.Location = new Point(140, 288);
        _txtEmail.Name = "_txtEmail";
        _txtEmail.Size = new Size(300, 20);
        //
        // Organización
        //
        _lblOrganizacion.Location = new Point(10, 318);
        _lblOrganizacion.Name = "_lblOrganizacion";
        _lblOrganizacion.Size = new Size(66, 13);
        _lblOrganizacion.Text = "Organización:";
        _txtOrganizacion.Location = new Point(140, 316);
        _txtOrganizacion.Name = "_txtOrganizacion";
        _txtOrganizacion.Size = new Size(300, 20);
        //
        // Observaciones
        //
        _lblObservaciones.Location = new Point(10, 346);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(73, 13);
        _lblObservaciones.Text = "Observaciones:";
        _txtObservaciones.Location = new Point(140, 344);
        _txtObservaciones.Name = "_txtObservaciones";
        _txtObservaciones.Size = new Size(300, 20);
        //
        // Usuario
        //
        _lblUsuario.Location = new Point(10, 374);
        _lblUsuario.Name = "_lblUsuario";
        _lblUsuario.Size = new Size(42, 13);
        _lblUsuario.Text = "Usuario:";
        _txtUsuario.Location = new Point(140, 372);
        _txtUsuario.Name = "_txtUsuario";
        _txtUsuario.Size = new Size(200, 20);
        //
        // Contraseña
        //
        _lblPassword.Location = new Point(10, 402);
        _lblPassword.Name = "_lblPassword";
        _lblPassword.Size = new Size(51, 13);
        _lblPassword.Text = "Contraseña:";
        _txtPassword.Location = new Point(140, 400);
        _txtPassword.Name = "_txtPassword";
        _txtPassword.Properties.UseSystemPasswordChar = true;
        _txtPassword.Size = new Size(200, 20);
        //
        // Días de Crédito
        //
        _lblDiasCredito.Location = new Point(10, 430);
        _lblDiasCredito.Name = "_lblDiasCredito";
        _lblDiasCredito.Size = new Size(74, 13);
        _lblDiasCredito.Text = "Días de Crédito:";
        _spnDiasCredito.Location = new Point(140, 428);
        _spnDiasCredito.Name = "_spnDiasCredito";
        _spnDiasCredito.Properties.IsFloatValue = false;
        _spnDiasCredito.Properties.Mask.EditMask = "N00";
        _spnDiasCredito.Properties.MaxValue = new decimal(new int[] { 9999, 0, 0, 0 });
        _spnDiasCredito.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnDiasCredito.Size = new Size(90, 20);
        //
        // Obsoleto
        //
        _chkObsoleto.Location = new Point(140, 458);
        _chkObsoleto.Name = "_chkObsoleto";
        _chkObsoleto.Properties.Caption = "¿El Productor es Obsoleto?";
        _chkObsoleto.Size = new Size(200, 20);
        //
        // _btnNuevo
        //
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.Location = new Point(20, 640);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.Location = new Point(116, 640);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.Location = new Point(202, 640);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.Location = new Point(390, 640);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // ProductorEditarForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(820, 680);
        Controls.Add(_btnInicio);
        Controls.Add(_btnAnterior);
        Controls.Add(_btnSiguiente);
        Controls.Add(_btnFin);
        Controls.Add(_lblClave);
        Controls.Add(_txtClave);
        Controls.Add(_lblFechaRegistro);
        Controls.Add(_txtFechaRegistro);
        Controls.Add(_lblNombreProductor);
        Controls.Add(_txtNombreProductor);
        Controls.Add(_tabControl);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = true;
        Name = "ProductorEditarForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Productor";
        ((System.ComponentModel.ISupportInitialize)_txtClave.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFechaRegistro.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombreProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDomicilio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtColonia.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoPostal.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPoblacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbMunicipio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbEstado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRfc.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTelefono.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCelular.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtOrganizacion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtUsuario.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnDiasCredito.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkObsoleto.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridHuertas).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewHuertas).EndInit();
        ((System.ComponentModel.ISupportInitialize)_tabControl).EndInit();
        _tabControl.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
