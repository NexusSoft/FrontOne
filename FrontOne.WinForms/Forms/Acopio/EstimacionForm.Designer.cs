using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class EstimacionForm
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

    private GroupControl _grpHuerta;
    private LabelControl _lblFolio;
    private LabelControl _lblAutorizacion;
    private SimpleButton _btnBuscar;
    private LabelControl _lblFecha;
    private DateEdit _dtFecha;
    private LabelControl _lblAcopiador;
    private LookUpEdit _cmbAcopiador;
    private LabelControl _lblHuerta;
    private ButtonEdit _beHuerta;
    private LabelControl _lblRegistroSagarpa;
    private TextEdit _txtRegistroSagarpa;
    private LabelControl _lblKilos;
    private SpinEdit _spnKilos;

    private GroupControl _grpListaPrecios;
    private LabelControl _lblListaPrecio;
    private ButtonEdit _beListaPrecio;
    private CheckEdit _chkListaMasReciente;
    private ComboBoxEdit _cmbTipoLista;
    private LabelControl _lblListaCargada;

    private GroupControl _grpPrecios;
    private GridControl _gridPrecios;
    private GridView _gridViewPrecios;
    private GridColumn _colPrecioCalibre;
    private GridColumn _colPrecioCategoria;
    private GridColumn _colPrecioConvencional;
    private GridColumn _colPrecioOrganico;
    private GridColumn _colPrecioNacional;

    private GroupControl _grpCategorias;
    private LabelControl _lblCat1;
    private SpinEdit _spnCat1;
    private LabelControl _lblCat2;
    private SpinEdit _spnCat2;
    private LabelControl _lblNal;
    private SpinEdit _spnNal;
    private LabelControl _lblAvisoCategorias;

    private GroupControl _grpCalibresExport;
    private LabelControl _lblCalibre32;
    private SpinEdit _spnCalibre32;
    private LabelControl _lblCalibre36;
    private SpinEdit _spnCalibre36;
    private LabelControl _lblCalibre40;
    private SpinEdit _spnCalibre40;
    private LabelControl _lblCalibre48;
    private SpinEdit _spnCalibre48;
    private LabelControl _lblCalibre60;
    private SpinEdit _spnCalibre60;
    private LabelControl _lblCalibre70;
    private SpinEdit _spnCalibre70;
    private LabelControl _lblCalibre84;
    private SpinEdit _spnCalibre84;
    private LabelControl _lblCalibre90;
    private SpinEdit _spnCalibre90;
    private LabelControl _lblAvisoCalibresExport;

    private GroupControl _grpCalibresNacional;
    private LabelControl _lblBorona;
    private SpinEdit _spnBorona;
    private LabelControl _lblCanica;
    private SpinEdit _spnCanica;
    private LabelControl _lblCuarta;
    private SpinEdit _spnCuarta;
    private LabelControl _lblDesecho;
    private SpinEdit _spnDesecho;
    private LabelControl _lblProceso;
    private SpinEdit _spnProceso;
    private LabelControl _lblAvisoCalibresNacional;

    private LabelControl _lblPrecioSugerido;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstimacionForm));
        _grpHuerta = new GroupControl();
        _lblFolio = new LabelControl();
        _lblAutorizacion = new LabelControl();
        _btnBuscar = new SimpleButton();
        _lblFecha = new LabelControl();
        _dtFecha = new DateEdit();
        _lblAcopiador = new LabelControl();
        _cmbAcopiador = new LookUpEdit();
        _lblHuerta = new LabelControl();
        _beHuerta = new ButtonEdit();
        _lblRegistroSagarpa = new LabelControl();
        _txtRegistroSagarpa = new TextEdit();
        _lblKilos = new LabelControl();
        _spnKilos = new SpinEdit();
        _grpListaPrecios = new GroupControl();
        _lblListaPrecio = new LabelControl();
        _beListaPrecio = new ButtonEdit();
        _chkListaMasReciente = new CheckEdit();
        _cmbTipoLista = new ComboBoxEdit();
        _lblListaCargada = new LabelControl();
        _grpPrecios = new GroupControl();
        _gridPrecios = new GridControl();
        _gridViewPrecios = new GridView(_gridPrecios);
        _colPrecioCalibre = new GridColumn();
        _colPrecioCategoria = new GridColumn();
        _colPrecioConvencional = new GridColumn();
        _colPrecioOrganico = new GridColumn();
        _colPrecioNacional = new GridColumn();
        _grpCategorias = new GroupControl();
        _lblCat1 = new LabelControl();
        _spnCat1 = new SpinEdit();
        _lblCat2 = new LabelControl();
        _spnCat2 = new SpinEdit();
        _lblNal = new LabelControl();
        _spnNal = new SpinEdit();
        _lblAvisoCategorias = new LabelControl();
        _grpCalibresExport = new GroupControl();
        _lblCalibre32 = new LabelControl();
        _spnCalibre32 = new SpinEdit();
        _lblCalibre36 = new LabelControl();
        _spnCalibre36 = new SpinEdit();
        _lblCalibre40 = new LabelControl();
        _spnCalibre40 = new SpinEdit();
        _lblCalibre48 = new LabelControl();
        _spnCalibre48 = new SpinEdit();
        _lblCalibre60 = new LabelControl();
        _spnCalibre60 = new SpinEdit();
        _lblCalibre70 = new LabelControl();
        _spnCalibre70 = new SpinEdit();
        _lblCalibre84 = new LabelControl();
        _spnCalibre84 = new SpinEdit();
        _lblCalibre90 = new LabelControl();
        _spnCalibre90 = new SpinEdit();
        _lblAvisoCalibresExport = new LabelControl();
        _grpCalibresNacional = new GroupControl();
        _lblBorona = new LabelControl();
        _spnBorona = new SpinEdit();
        _lblCanica = new LabelControl();
        _spnCanica = new SpinEdit();
        _lblCuarta = new LabelControl();
        _spnCuarta = new SpinEdit();
        _lblDesecho = new LabelControl();
        _spnDesecho = new SpinEdit();
        _lblProceso = new LabelControl();
        _spnProceso = new SpinEdit();
        _lblAvisoCalibresNacional = new LabelControl();
        _lblPrecioSugerido = new LabelControl();
        _btnGuardar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grpHuerta).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbAcopiador.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_beHuerta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilos.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpListaPrecios).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_beListaPrecio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chkListaMasReciente.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoLista.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpPrecios).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridPrecios).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPrecios).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpCategorias).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCat1.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCat2.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnNal.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpCalibresExport).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre32.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre36.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre40.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre48.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre60.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre70.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre84.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre90.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpCalibresNacional).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnBorona.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCanica.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnCuarta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnDesecho.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnProceso.Properties).BeginInit();
        SuspendLayout();
        //
        // _grpHuerta
        //
        _grpHuerta.Controls.Add(_lblFolio);
        _grpHuerta.Controls.Add(_lblAutorizacion);
        _grpHuerta.Controls.Add(_btnBuscar);
        _grpHuerta.Controls.Add(_lblFecha);
        _grpHuerta.Controls.Add(_dtFecha);
        _grpHuerta.Controls.Add(_lblAcopiador);
        _grpHuerta.Controls.Add(_cmbAcopiador);
        _grpHuerta.Controls.Add(_lblHuerta);
        _grpHuerta.Controls.Add(_beHuerta);
        _grpHuerta.Controls.Add(_lblRegistroSagarpa);
        _grpHuerta.Controls.Add(_txtRegistroSagarpa);
        _grpHuerta.Controls.Add(_lblKilos);
        _grpHuerta.Controls.Add(_spnKilos);
        _grpHuerta.Location = new Point(12, 12);
        _grpHuerta.Name = "_grpHuerta";
        _grpHuerta.Size = new Size(871, 160);
        _grpHuerta.TabIndex = 0;
        _grpHuerta.Text = "Datos de la Huerta";
        //
        // _lblFolio
        //
        _lblFolio.Location = new Point(15, 35);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(84, 13);
        _lblFolio.Text = "Folio: (nuevo)";
        //
        // _lblAutorizacion
        //
        _lblAutorizacion.Location = new Point(200, 35);
        _lblAutorizacion.Name = "_lblAutorizacion";
        _lblAutorizacion.Size = new Size(200, 13);
        _lblAutorizacion.Text = "Autorización: Pendiente";
        //
        // _btnBuscar
        //
        _btnBuscar.Location = new Point(725, 32);
        _btnBuscar.Name = "_btnBuscar";
        _btnBuscar.Size = new Size(140, 23);
        _btnBuscar.TabIndex = 0;
        _btnBuscar.Text = "Buscar existente...";
        _btnBuscar.Click += BtnBuscar_Click;
        //
        // _lblFecha
        //
        _lblFecha.Location = new Point(15, 65);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(33, 13);
        _lblFecha.Text = "Fecha:";
        //
        // _dtFecha
        //
        _dtFecha.EditValue = null;
        _dtFecha.Location = new Point(115, 62);
        _dtFecha.Name = "_dtFecha";
        _dtFecha.Size = new Size(140, 20);
        _dtFecha.TabIndex = 1;
        //
        // _lblAcopiador
        //
        _lblAcopiador.Location = new Point(420, 65);
        _lblAcopiador.Name = "_lblAcopiador";
        _lblAcopiador.Size = new Size(52, 13);
        _lblAcopiador.Text = "Acopiador:";
        //
        // _cmbAcopiador
        //
        _cmbAcopiador.Location = new Point(490, 62);
        _cmbAcopiador.Name = "_cmbAcopiador";
        _cmbAcopiador.Properties.NullText = "Seleccionar";
        _cmbAcopiador.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbAcopiador.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbAcopiador.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Plus) });
        _cmbAcopiador.Size = new Size(290, 20);
        _cmbAcopiador.TabIndex = 2;
        _cmbAcopiador.ButtonClick += CmbAcopiador_ButtonClick;
        //
        // _lblHuerta
        //
        _lblHuerta.Location = new Point(15, 95);
        _lblHuerta.Name = "_lblHuerta";
        _lblHuerta.Size = new Size(37, 13);
        _lblHuerta.Text = "Huerta:";
        //
        // _beHuerta
        //
        _beHuerta.Location = new Point(115, 92);
        _beHuerta.Name = "_beHuerta";
        _beHuerta.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search) });
        _beHuerta.Properties.NullValuePrompt = "Buscar huerta...";
        _beHuerta.Properties.ReadOnly = true;
        _beHuerta.Size = new Size(665, 20);
        _beHuerta.TabIndex = 3;
        _beHuerta.ButtonClick += BeHuerta_ButtonClick;
        //
        // _lblRegistroSagarpa
        //
        _lblRegistroSagarpa.Location = new Point(15, 125);
        _lblRegistroSagarpa.Name = "_lblRegistroSagarpa";
        _lblRegistroSagarpa.Size = new Size(103, 13);
        _lblRegistroSagarpa.Text = "Registro SAGARPA:";
        //
        // _txtRegistroSagarpa
        //
        _txtRegistroSagarpa.Location = new Point(160, 122);
        _txtRegistroSagarpa.Name = "_txtRegistroSagarpa";
        _txtRegistroSagarpa.Properties.ReadOnly = true;
        _txtRegistroSagarpa.Size = new Size(200, 20);
        _txtRegistroSagarpa.TabIndex = 4;
        //
        // _lblKilos
        //
        _lblKilos.Location = new Point(420, 125);
        _lblKilos.Name = "_lblKilos";
        _lblKilos.Size = new Size(113, 13);
        _lblKilos.Text = "Kilogramos a Cortar:";
        //
        // _spnKilos
        //
        _spnKilos.Location = new Point(560, 122);
        _spnKilos.Name = "_spnKilos";
        _spnKilos.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnKilos.Properties.DisplayFormat.FormatString = "n2";
        _spnKilos.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnKilos.Properties.EditFormat.FormatString = "n2";
        _spnKilos.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnKilos.Size = new Size(120, 20);
        _spnKilos.TabIndex = 5;
        //
        // _grpListaPrecios
        //
        _grpListaPrecios.Controls.Add(_lblListaPrecio);
        _grpListaPrecios.Controls.Add(_beListaPrecio);
        _grpListaPrecios.Controls.Add(_chkListaMasReciente);
        _grpListaPrecios.Controls.Add(_cmbTipoLista);
        _grpListaPrecios.Controls.Add(_lblListaCargada);
        _grpListaPrecios.Location = new Point(12, 178);
        _grpListaPrecios.Name = "_grpListaPrecios";
        _grpListaPrecios.Size = new Size(871, 90);
        _grpListaPrecios.TabIndex = 1;
        _grpListaPrecios.Text = "Lista de Precios";
        //
        // _lblListaPrecio
        //
        _lblListaPrecio.Location = new Point(15, 35);
        _lblListaPrecio.Name = "_lblListaPrecio";
        _lblListaPrecio.Size = new Size(89, 13);
        _lblListaPrecio.Text = "Lista de Precios:";
        //
        // _beListaPrecio
        //
        _beListaPrecio.Location = new Point(115, 32);
        _beListaPrecio.Name = "_beListaPrecio";
        _beListaPrecio.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search) });
        _beListaPrecio.Properties.NullValuePrompt = "Buscar lista de precios...";
        _beListaPrecio.Properties.ReadOnly = true;
        _beListaPrecio.Size = new Size(280, 20);
        _beListaPrecio.TabIndex = 0;
        _beListaPrecio.ButtonClick += BeListaPrecio_ButtonClick;
        //
        // _chkListaMasReciente
        //
        _chkListaMasReciente.Location = new Point(115, 58);
        _chkListaMasReciente.Name = "_chkListaMasReciente";
        _chkListaMasReciente.Properties.Caption = "Usar lista más reciente";
        _chkListaMasReciente.Size = new Size(180, 20);
        _chkListaMasReciente.TabIndex = 2;
        _chkListaMasReciente.CheckedChanged += ChkListaMasReciente_CheckedChanged;
        //
        // _cmbTipoLista
        //
        _cmbTipoLista.Location = new Point(405, 32);
        _cmbTipoLista.Name = "_cmbTipoLista";
        _cmbTipoLista.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbTipoLista.Size = new Size(140, 20);
        _cmbTipoLista.TabIndex = 1;
        _cmbTipoLista.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblListaCargada
        //
        _lblListaCargada.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _lblListaCargada.Location = new Point(555, 35);
        _lblListaCargada.Name = "_lblListaCargada";
        _lblListaCargada.Size = new Size(300, 13);
        _lblListaCargada.Visible = false;
        //
        // _grpPrecios
        //
        _grpPrecios.Controls.Add(_gridPrecios);
        _grpPrecios.Location = new Point(12, 276);
        _grpPrecios.Name = "_grpPrecios";
        _grpPrecios.Size = new Size(555, 631);
        _grpPrecios.TabIndex = 2;
        _grpPrecios.Text = "Precios de la Vigencia";
        //
        // _gridPrecios
        //
        _gridPrecios.Location = new Point(15, 32);
        _gridPrecios.MainView = _gridViewPrecios;
        _gridPrecios.Name = "_gridPrecios";
        _gridPrecios.Size = new Size(525, 584);
        _gridPrecios.TabIndex = 0;
        _gridPrecios.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewPrecios });
        //
        // _gridViewPrecios
        //
        _gridViewPrecios.Columns.AddRange(new GridColumn[] { _colPrecioCalibre, _colPrecioCategoria, _colPrecioConvencional, _colPrecioOrganico, _colPrecioNacional });
        _gridViewPrecios.GridControl = _gridPrecios;
        _gridViewPrecios.Name = "_gridViewPrecios";
        _gridViewPrecios.OptionsBehavior.Editable = false;
        _gridViewPrecios.OptionsView.ShowGroupPanel = false;
        _gridViewPrecios.OptionsView.ColumnAutoWidth = false;
        _gridViewPrecios.RowCellStyle += GridViewPrecios_RowCellStyle;
        //
        // _colPrecioCalibre
        //
        _colPrecioCalibre.Caption = "Calibre APEAM";
        _colPrecioCalibre.FieldName = "CalibreApeamNombre";
        _colPrecioCalibre.Name = "_colPrecioCalibre";
        _colPrecioCalibre.Visible = true;
        _colPrecioCalibre.VisibleIndex = 0;
        _colPrecioCalibre.Width = 110;
        //
        // _colPrecioCategoria
        //
        _colPrecioCategoria.Caption = "Categoría";
        _colPrecioCategoria.FieldName = "CategoriaNombre";
        _colPrecioCategoria.Name = "_colPrecioCategoria";
        _colPrecioCategoria.Visible = true;
        _colPrecioCategoria.VisibleIndex = 1;
        _colPrecioCategoria.Width = 90;
        //
        // _colPrecioConvencional
        //
        _colPrecioConvencional.Caption = "Convencional";
        _colPrecioConvencional.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecioConvencional.DisplayFormat.FormatString = "c2";
        _colPrecioConvencional.FieldName = "Convencional";
        _colPrecioConvencional.Name = "_colPrecioConvencional";
        _colPrecioConvencional.Visible = true;
        _colPrecioConvencional.VisibleIndex = 2;
        _colPrecioConvencional.Width = 95;
        _colPrecioConvencional.AppearanceCell.BackColor = ColorTranslator.FromHtml("#D6EAF8");
        _colPrecioConvencional.AppearanceCell.Options.UseBackColor = true;
        //
        // _colPrecioOrganico
        //
        _colPrecioOrganico.Caption = "Orgánica";
        _colPrecioOrganico.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecioOrganico.DisplayFormat.FormatString = "c2";
        _colPrecioOrganico.FieldName = "Organico";
        _colPrecioOrganico.Name = "_colPrecioOrganico";
        _colPrecioOrganico.Visible = true;
        _colPrecioOrganico.VisibleIndex = 3;
        _colPrecioOrganico.Width = 95;
        _colPrecioOrganico.AppearanceCell.BackColor = ColorTranslator.FromHtml("#FDEBD0");
        _colPrecioOrganico.AppearanceCell.Options.UseBackColor = true;
        //
        // _colPrecioNacional
        //
        _colPrecioNacional.Caption = "Nacional";
        _colPrecioNacional.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecioNacional.DisplayFormat.FormatString = "c2";
        _colPrecioNacional.FieldName = "Nacional";
        _colPrecioNacional.Name = "_colPrecioNacional";
        _colPrecioNacional.Visible = true;
        _colPrecioNacional.VisibleIndex = 4;
        _colPrecioNacional.Width = 100;
        _colPrecioNacional.AppearanceCell.BackColor = ColorTranslator.FromHtml("#D5F5E3");
        _colPrecioNacional.AppearanceCell.Options.UseBackColor = true;
        //
        // _grpCategorias
        //
        _grpCategorias.Controls.Add(_lblCat1);
        _grpCategorias.Controls.Add(_spnCat1);
        _grpCategorias.Controls.Add(_lblCat2);
        _grpCategorias.Controls.Add(_spnCat2);
        _grpCategorias.Controls.Add(_lblNal);
        _grpCategorias.Controls.Add(_spnNal);
        _grpCategorias.Controls.Add(_lblAvisoCategorias);
        _grpCategorias.Location = new Point(575, 276);
        _grpCategorias.Name = "_grpCategorias";
        _grpCategorias.Size = new Size(308, 150);
        _grpCategorias.TabIndex = 3;
        _grpCategorias.Text = "Categorías";
        //
        // _lblCat1
        //
        _lblCat1.Location = new Point(15, 35);
        _lblCat1.Name = "_lblCat1";
        _lblCat1.Size = new Size(29, 13);
        _lblCat1.Text = "Cat 1:";
        //
        // _spnCat1
        //
        _spnCat1.Location = new Point(150, 32);
        _spnCat1.Name = "_spnCat1";
        _spnCat1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCat1.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCat1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCat1.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCat1.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCat1.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCat1.Size = new Size(140, 20);
        _spnCat1.TabIndex = 0;
        _spnCat1.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCat2
        //
        _lblCat2.Location = new Point(15, 63);
        _lblCat2.Name = "_lblCat2";
        _lblCat2.Size = new Size(29, 13);
        _lblCat2.Text = "Cat 2:";
        //
        // _spnCat2
        //
        _spnCat2.Location = new Point(150, 60);
        _spnCat2.Name = "_spnCat2";
        _spnCat2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCat2.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCat2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCat2.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCat2.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCat2.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCat2.Size = new Size(140, 20);
        _spnCat2.TabIndex = 1;
        _spnCat2.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblNal
        //
        _lblNal.Location = new Point(15, 91);
        _lblNal.Name = "_lblNal";
        _lblNal.Size = new Size(48, 13);
        _lblNal.Text = "Nacional:";
        //
        // _spnNal
        //
        _spnNal.Location = new Point(150, 88);
        _spnNal.Name = "_spnNal";
        _spnNal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnNal.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnNal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnNal.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnNal.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnNal.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnNal.Size = new Size(140, 20);
        _spnNal.TabIndex = 2;
        _spnNal.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblAvisoCategorias
        //
        _lblAvisoCategorias.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _lblAvisoCategorias.Appearance.ForeColor = Color.FromArgb(179, 38, 30);
        _lblAvisoCategorias.Appearance.Options.UseForeColor = true;
        _lblAvisoCategorias.Location = new Point(15, 118);
        _lblAvisoCategorias.Name = "_lblAvisoCategorias";
        _lblAvisoCategorias.Size = new Size(278, 26);
        _lblAvisoCategorias.Visible = false;
        //
        // _grpCalibresExport
        //
        _grpCalibresExport.Controls.Add(_lblCalibre32);
        _grpCalibresExport.Controls.Add(_spnCalibre32);
        _grpCalibresExport.Controls.Add(_lblCalibre36);
        _grpCalibresExport.Controls.Add(_spnCalibre36);
        _grpCalibresExport.Controls.Add(_lblCalibre40);
        _grpCalibresExport.Controls.Add(_spnCalibre40);
        _grpCalibresExport.Controls.Add(_lblCalibre48);
        _grpCalibresExport.Controls.Add(_spnCalibre48);
        _grpCalibresExport.Controls.Add(_lblCalibre60);
        _grpCalibresExport.Controls.Add(_spnCalibre60);
        _grpCalibresExport.Controls.Add(_lblCalibre70);
        _grpCalibresExport.Controls.Add(_spnCalibre70);
        _grpCalibresExport.Controls.Add(_lblCalibre84);
        _grpCalibresExport.Controls.Add(_spnCalibre84);
        _grpCalibresExport.Controls.Add(_lblCalibre90);
        _grpCalibresExport.Controls.Add(_spnCalibre90);
        _grpCalibresExport.Controls.Add(_lblAvisoCalibresExport);
        _grpCalibresExport.Location = new Point(575, 434);
        _grpCalibresExport.Name = "_grpCalibresExport";
        _grpCalibresExport.Size = new Size(308, 270);
        _grpCalibresExport.TabIndex = 4;
        _grpCalibresExport.Text = "Calibres Exportación";
        //
        // _lblCalibre32
        //
        _lblCalibre32.Location = new Point(15, 35);
        _lblCalibre32.Name = "_lblCalibre32";
        _lblCalibre32.Size = new Size(21, 13);
        _lblCalibre32.Text = "32:";
        //
        // _spnCalibre32
        //
        _spnCalibre32.Location = new Point(150, 32);
        _spnCalibre32.Name = "_spnCalibre32";
        _spnCalibre32.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre32.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre32.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre32.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre32.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre32.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre32.Size = new Size(140, 20);
        _spnCalibre32.TabIndex = 0;
        _spnCalibre32.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre36
        //
        _lblCalibre36.Location = new Point(15, 63);
        _lblCalibre36.Name = "_lblCalibre36";
        _lblCalibre36.Size = new Size(21, 13);
        _lblCalibre36.Text = "36:";
        //
        // _spnCalibre36
        //
        _spnCalibre36.Location = new Point(150, 60);
        _spnCalibre36.Name = "_spnCalibre36";
        _spnCalibre36.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre36.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre36.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre36.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre36.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre36.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre36.Size = new Size(140, 20);
        _spnCalibre36.TabIndex = 1;
        _spnCalibre36.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre40
        //
        _lblCalibre40.Location = new Point(15, 91);
        _lblCalibre40.Name = "_lblCalibre40";
        _lblCalibre40.Size = new Size(21, 13);
        _lblCalibre40.Text = "40:";
        //
        // _spnCalibre40
        //
        _spnCalibre40.Location = new Point(150, 88);
        _spnCalibre40.Name = "_spnCalibre40";
        _spnCalibre40.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre40.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre40.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre40.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre40.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre40.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre40.Size = new Size(140, 20);
        _spnCalibre40.TabIndex = 2;
        _spnCalibre40.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre48
        //
        _lblCalibre48.Location = new Point(15, 119);
        _lblCalibre48.Name = "_lblCalibre48";
        _lblCalibre48.Size = new Size(21, 13);
        _lblCalibre48.Text = "48:";
        //
        // _spnCalibre48
        //
        _spnCalibre48.Location = new Point(150, 116);
        _spnCalibre48.Name = "_spnCalibre48";
        _spnCalibre48.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre48.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre48.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre48.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre48.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre48.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre48.Size = new Size(140, 20);
        _spnCalibre48.TabIndex = 3;
        _spnCalibre48.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre60
        //
        _lblCalibre60.Location = new Point(15, 147);
        _lblCalibre60.Name = "_lblCalibre60";
        _lblCalibre60.Size = new Size(21, 13);
        _lblCalibre60.Text = "60:";
        //
        // _spnCalibre60
        //
        _spnCalibre60.Location = new Point(150, 144);
        _spnCalibre60.Name = "_spnCalibre60";
        _spnCalibre60.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre60.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre60.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre60.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre60.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre60.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre60.Size = new Size(140, 20);
        _spnCalibre60.TabIndex = 4;
        _spnCalibre60.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre70
        //
        _lblCalibre70.Location = new Point(15, 175);
        _lblCalibre70.Name = "_lblCalibre70";
        _lblCalibre70.Size = new Size(21, 13);
        _lblCalibre70.Text = "70:";
        //
        // _spnCalibre70
        //
        _spnCalibre70.Location = new Point(150, 172);
        _spnCalibre70.Name = "_spnCalibre70";
        _spnCalibre70.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre70.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre70.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre70.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre70.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre70.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre70.Size = new Size(140, 20);
        _spnCalibre70.TabIndex = 5;
        _spnCalibre70.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre84
        //
        _lblCalibre84.Location = new Point(15, 203);
        _lblCalibre84.Name = "_lblCalibre84";
        _lblCalibre84.Size = new Size(21, 13);
        _lblCalibre84.Text = "84:";
        //
        // _spnCalibre84
        //
        _spnCalibre84.Location = new Point(150, 200);
        _spnCalibre84.Name = "_spnCalibre84";
        _spnCalibre84.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre84.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre84.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre84.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre84.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre84.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre84.Size = new Size(140, 20);
        _spnCalibre84.TabIndex = 6;
        _spnCalibre84.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCalibre90
        //
        _lblCalibre90.Location = new Point(15, 231);
        _lblCalibre90.Name = "_lblCalibre90";
        _lblCalibre90.Size = new Size(21, 13);
        _lblCalibre90.Text = "90:";
        //
        // _spnCalibre90
        //
        _spnCalibre90.Location = new Point(150, 228);
        _spnCalibre90.Name = "_spnCalibre90";
        _spnCalibre90.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre90.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCalibre90.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCalibre90.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCalibre90.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCalibre90.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCalibre90.Size = new Size(140, 20);
        _spnCalibre90.TabIndex = 7;
        _spnCalibre90.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblAvisoCalibresExport
        //
        _lblAvisoCalibresExport.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _lblAvisoCalibresExport.Appearance.ForeColor = Color.FromArgb(179, 38, 30);
        _lblAvisoCalibresExport.Appearance.Options.UseForeColor = true;
        _lblAvisoCalibresExport.Location = new Point(15, 254);
        _lblAvisoCalibresExport.Name = "_lblAvisoCalibresExport";
        _lblAvisoCalibresExport.Size = new Size(278, 13);
        _lblAvisoCalibresExport.Visible = false;
        //
        // _grpCalibresNacional
        //
        _grpCalibresNacional.Controls.Add(_lblBorona);
        _grpCalibresNacional.Controls.Add(_spnBorona);
        _grpCalibresNacional.Controls.Add(_lblCanica);
        _grpCalibresNacional.Controls.Add(_spnCanica);
        _grpCalibresNacional.Controls.Add(_lblCuarta);
        _grpCalibresNacional.Controls.Add(_spnCuarta);
        _grpCalibresNacional.Controls.Add(_lblDesecho);
        _grpCalibresNacional.Controls.Add(_spnDesecho);
        _grpCalibresNacional.Controls.Add(_lblProceso);
        _grpCalibresNacional.Controls.Add(_spnProceso);
        _grpCalibresNacional.Controls.Add(_lblAvisoCalibresNacional);
        _grpCalibresNacional.Location = new Point(575, 712);
        _grpCalibresNacional.Name = "_grpCalibresNacional";
        _grpCalibresNacional.Size = new Size(308, 195);
        _grpCalibresNacional.TabIndex = 5;
        _grpCalibresNacional.Text = "Calibres Nacional";
        //
        // _lblBorona
        //
        _lblBorona.Location = new Point(15, 35);
        _lblBorona.Name = "_lblBorona";
        _lblBorona.Size = new Size(38, 13);
        _lblBorona.Text = "Borona:";
        //
        // _spnBorona
        //
        _spnBorona.Location = new Point(150, 32);
        _spnBorona.Name = "_spnBorona";
        _spnBorona.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnBorona.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnBorona.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnBorona.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnBorona.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnBorona.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnBorona.Size = new Size(140, 20);
        _spnBorona.TabIndex = 0;
        _spnBorona.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCanica
        //
        _lblCanica.Location = new Point(15, 63);
        _lblCanica.Name = "_lblCanica";
        _lblCanica.Size = new Size(37, 13);
        _lblCanica.Text = "Canica:";
        //
        // _spnCanica
        //
        _spnCanica.Location = new Point(150, 60);
        _spnCanica.Name = "_spnCanica";
        _spnCanica.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCanica.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCanica.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCanica.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCanica.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCanica.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCanica.Size = new Size(140, 20);
        _spnCanica.TabIndex = 1;
        _spnCanica.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblCuarta
        //
        _lblCuarta.Location = new Point(15, 91);
        _lblCuarta.Name = "_lblCuarta";
        _lblCuarta.Size = new Size(36, 13);
        _lblCuarta.Text = "Cuarta:";
        //
        // _spnCuarta
        //
        _spnCuarta.Location = new Point(150, 88);
        _spnCuarta.Name = "_spnCuarta";
        _spnCuarta.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCuarta.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnCuarta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnCuarta.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnCuarta.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnCuarta.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnCuarta.Size = new Size(140, 20);
        _spnCuarta.TabIndex = 2;
        _spnCuarta.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblDesecho
        //
        _lblDesecho.Location = new Point(15, 119);
        _lblDesecho.Name = "_lblDesecho";
        _lblDesecho.Size = new Size(46, 13);
        _lblDesecho.Text = "Desecho:";
        //
        // _spnDesecho
        //
        _spnDesecho.Location = new Point(150, 116);
        _spnDesecho.Name = "_spnDesecho";
        _spnDesecho.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnDesecho.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnDesecho.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnDesecho.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnDesecho.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnDesecho.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnDesecho.Size = new Size(140, 20);
        _spnDesecho.TabIndex = 3;
        _spnDesecho.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblProceso
        //
        _lblProceso.Location = new Point(15, 147);
        _lblProceso.Name = "_lblProceso";
        _lblProceso.Size = new Size(44, 13);
        _lblProceso.Text = "Proceso:";
        //
        // _spnProceso
        //
        _spnProceso.Location = new Point(150, 144);
        _spnProceso.Name = "_spnProceso";
        _spnProceso.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnProceso.Properties.DisplayFormat.FormatString = "#,##0.00'%'";
        _spnProceso.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _spnProceso.Properties.EditFormat.FormatString = "#,##0.00'%'";
        _spnProceso.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
        _spnProceso.Properties.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _spnProceso.Size = new Size(140, 20);
        _spnProceso.TabIndex = 4;
        _spnProceso.EditValueChanged += Recalcular_EditValueChanged;
        //
        // _lblAvisoCalibresNacional
        //
        _lblAvisoCalibresNacional.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _lblAvisoCalibresNacional.Appearance.ForeColor = Color.FromArgb(179, 38, 30);
        _lblAvisoCalibresNacional.Appearance.Options.UseForeColor = true;
        _lblAvisoCalibresNacional.Location = new Point(15, 174);
        _lblAvisoCalibresNacional.Name = "_lblAvisoCalibresNacional";
        _lblAvisoCalibresNacional.Size = new Size(278, 13);
        _lblAvisoCalibresNacional.Visible = false;
        //
        // _lblPrecioSugerido
        //
        _lblPrecioSugerido.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        _lblPrecioSugerido.Appearance.Options.UseFont = true;
        _lblPrecioSugerido.Location = new Point(12, 919);
        _lblPrecioSugerido.Name = "_lblPrecioSugerido";
        _lblPrecioSugerido.Size = new Size(400, 21);
        _lblPrecioSugerido.Text = "Precio Sugerido: —";
        //
        // _btnGuardar
        //
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(713, 915);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 28);
        _btnGuardar.TabIndex = 6;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(797, 915);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(80, 28);
        _btnCerrar.TabIndex = 7;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // EstimacionForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(895, 951);
        Controls.Add(_grpHuerta);
        Controls.Add(_grpListaPrecios);
        Controls.Add(_grpPrecios);
        Controls.Add(_grpCategorias);
        Controls.Add(_grpCalibresExport);
        Controls.Add(_grpCalibresNacional);
        Controls.Add(_lblPrecioSugerido);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EstimacionForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Estimación";
        ((System.ComponentModel.ISupportInitialize)_grpHuerta).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbAcopiador.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_beHuerta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtRegistroSagarpa.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnKilos.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpListaPrecios).EndInit();
        ((System.ComponentModel.ISupportInitialize)_beListaPrecio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chkListaMasReciente.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoLista.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpPrecios).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridPrecios).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPrecios).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpCategorias).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCat1.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCat2.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnNal.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpCalibresExport).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre32.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre36.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre40.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre48.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre60.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre70.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre84.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCalibre90.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpCalibresNacional).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnBorona.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCanica.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnCuarta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnDesecho.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnProceso.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
