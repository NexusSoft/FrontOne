using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace FrontOne.WinForms.Forms.Gastos;

partial class GastoLoteForm
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

    // Encabezado (solo lectura)
    private LabelControl _lblFolioCap, _lblFolio;
    private LabelControl _lblHuertaCap, _lblHuerta;
    private LabelControl _lblRegistroSagarpaCap, _lblRegistroSagarpa;
    private LabelControl _lblFechaCap, _lblFecha;
    private LabelControl _lblPesoCap, _lblPeso;
    private LabelControl _lblTipoPagoCap, _lblTipoPago;
    private LabelControl _lblTipoCorteCap, _lblTipoCorte;
    private LabelControl _lblVariedadCap, _lblVariedad;
    private LabelControl _lblPrecioAcordadoCap, _lblPrecioAcordado;

    private XtraTabControl _tabControl;
    private XtraTabPage _tabFruta;
    private XtraTabPage _tabCosecha;
    private XtraTabPage _tabAcarreo;

    // Tab Fruta
    private GridControl _gridFruta;
    private GridView _gridViewFruta;
    private GridColumn _colCategoriaNombre, _colKgSeleccionados, _colPorcentaje, _colKgComprados,
        _colCostoRealUnitario, _colImporteReal, _colCostoEstimadoUnitario, _colImporteEstimado;
    private RepositoryItemSpinEdit _repoCosto;
    private SimpleButton _btnVigenciaEstimado;
    private LabelControl _lblVigenciaEstimado;
    private ComboBoxEdit _cmbCostoEstimadoListaPrecioNumero;
    private SimpleButton _btnImprimir;
    private ComboBoxEdit _cmbTipoReporte;

    // Tab Cosecha
    private GridControl _gridCosecha;
    private GridView _gridViewCosecha;
    private SimpleButton _btnAgregarAjusteCosecha;
    private SimpleButton _btnEliminarAjusteCosecha;
    private LabelControl _lblTotalEmpresaCosechaCap, _lblTotalEmpresaCosecha;
    private LabelControl _lblTotalProductorCosechaCap, _lblTotalProductorCosecha;

    // Tab Acarreo
    private GridControl _gridAcarreo;
    private GridView _gridViewAcarreo;
    private SimpleButton _btnAgregarAjusteAcarreo;
    private SimpleButton _btnEliminarAjusteAcarreo;
    private LabelControl _lblTotalEmpresaAcarreoCap, _lblTotalEmpresaAcarreo;
    private LabelControl _lblTotalProductorAcarreoCap, _lblTotalProductorAcarreo;

    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GastoLoteForm));

        _lblFolioCap = new LabelControl(); _lblFolio = new LabelControl();
        _lblHuertaCap = new LabelControl(); _lblHuerta = new LabelControl();
        _lblRegistroSagarpaCap = new LabelControl(); _lblRegistroSagarpa = new LabelControl();
        _lblFechaCap = new LabelControl(); _lblFecha = new LabelControl();
        _lblPesoCap = new LabelControl(); _lblPeso = new LabelControl();
        _lblTipoPagoCap = new LabelControl(); _lblTipoPago = new LabelControl();
        _lblTipoCorteCap = new LabelControl(); _lblTipoCorte = new LabelControl();
        _lblVariedadCap = new LabelControl(); _lblVariedad = new LabelControl();
        _lblPrecioAcordadoCap = new LabelControl(); _lblPrecioAcordado = new LabelControl();

        _tabControl = new XtraTabControl();
        _tabFruta = new XtraTabPage();
        _tabCosecha = new XtraTabPage();
        _tabAcarreo = new XtraTabPage();

        _gridFruta = new GridControl();
        _gridViewFruta = new GridView();
        _colCategoriaNombre = new GridColumn();
        _colKgSeleccionados = new GridColumn();
        _colPorcentaje = new GridColumn();
        _colKgComprados = new GridColumn();
        _colCostoRealUnitario = new GridColumn();
        _colImporteReal = new GridColumn();
        _colCostoEstimadoUnitario = new GridColumn();
        _colImporteEstimado = new GridColumn();
        _repoCosto = new RepositoryItemSpinEdit();
        _btnVigenciaEstimado = new SimpleButton();
        _lblVigenciaEstimado = new LabelControl();
        _cmbCostoEstimadoListaPrecioNumero = new ComboBoxEdit();
        _btnImprimir = new SimpleButton();
        _cmbTipoReporte = new ComboBoxEdit();

        _gridCosecha = new GridControl();
        _gridViewCosecha = new GridView();
        _btnAgregarAjusteCosecha = new SimpleButton();
        _btnEliminarAjusteCosecha = new SimpleButton();
        _lblTotalEmpresaCosechaCap = new LabelControl(); _lblTotalEmpresaCosecha = new LabelControl();
        _lblTotalProductorCosechaCap = new LabelControl(); _lblTotalProductorCosecha = new LabelControl();

        _gridAcarreo = new GridControl();
        _gridViewAcarreo = new GridView();
        _btnAgregarAjusteAcarreo = new SimpleButton();
        _btnEliminarAjusteAcarreo = new SimpleButton();
        _lblTotalEmpresaAcarreoCap = new LabelControl(); _lblTotalEmpresaAcarreo = new LabelControl();
        _lblTotalProductorAcarreoCap = new LabelControl(); _lblTotalProductorAcarreo = new LabelControl();

        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_tabControl).BeginInit();
        _tabControl.SuspendLayout();
        _tabFruta.SuspendLayout();
        _tabCosecha.SuspendLayout();
        _tabAcarreo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_gridFruta).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewFruta).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoCosto).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCostoEstimadoListaPrecioNumero.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoReporte.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridCosecha).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewCosecha).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridAcarreo).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewAcarreo).BeginInit();
        SuspendLayout();

        // ---- Encabezado ----
        _lblFolioCap.Location = new Point(15, 15); _lblFolioCap.Text = "No. de Lote"; _lblFolioCap.Size = new Size(90, 13);
        _lblFolio.Location = new Point(110, 15); _lblFolio.Size = new Size(180, 13);
        _lblHuertaCap.Location = new Point(310, 15); _lblHuertaCap.Text = "Huerta"; _lblHuertaCap.Size = new Size(90, 13);
        _lblHuerta.Location = new Point(400, 15); _lblHuerta.Size = new Size(260, 13);
        _lblRegistroSagarpaCap.Location = new Point(680, 15); _lblRegistroSagarpaCap.Text = "Registro Sagarpa"; _lblRegistroSagarpaCap.Size = new Size(100, 13);
        _lblRegistroSagarpa.Location = new Point(785, 15); _lblRegistroSagarpa.Size = new Size(160, 13);

        _lblFechaCap.Location = new Point(15, 40); _lblFechaCap.Text = "Fecha de la Corrida"; _lblFechaCap.Size = new Size(90, 13);
        _lblFecha.Location = new Point(110, 40); _lblFecha.Size = new Size(180, 13);
        _lblPesoCap.Location = new Point(310, 40); _lblPesoCap.Text = "Peso (Kg)"; _lblPesoCap.Size = new Size(90, 13);
        _lblPeso.Location = new Point(400, 40); _lblPeso.Size = new Size(260, 13);
        _lblTipoPagoCap.Location = new Point(680, 40); _lblTipoPagoCap.Text = "Tipo de Pago"; _lblTipoPagoCap.Size = new Size(100, 13);
        _lblTipoPago.Location = new Point(785, 40); _lblTipoPago.Size = new Size(160, 13);

        _lblTipoCorteCap.Location = new Point(15, 65); _lblTipoCorteCap.Text = "Tipo de Corte"; _lblTipoCorteCap.Size = new Size(90, 13);
        _lblTipoCorte.Location = new Point(110, 65); _lblTipoCorte.Size = new Size(180, 13);
        _lblVariedadCap.Location = new Point(310, 65); _lblVariedadCap.Text = "Variedad"; _lblVariedadCap.Size = new Size(90, 13);
        _lblVariedad.Location = new Point(400, 65); _lblVariedad.Size = new Size(260, 13);
        _lblPrecioAcordadoCap.Location = new Point(680, 65); _lblPrecioAcordadoCap.Text = "Precio Acordado"; _lblPrecioAcordadoCap.Size = new Size(100, 13);
        _lblPrecioAcordado.Location = new Point(785, 65); _lblPrecioAcordado.Size = new Size(260, 13);

        // ---- TabControl ----
        _tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _tabControl.Location = new Point(15, 95);
        _tabControl.Name = "_tabControl";
        _tabControl.SelectedTabPage = _tabFruta;
        _tabControl.Size = new Size(1030, 480);
        _tabControl.TabPages.AddRange(new XtraTabPage[] { _tabFruta, _tabCosecha, _tabAcarreo });

        // ---- Tab Fruta ----
        _tabFruta.Text = "Fruta";
        _tabFruta.Name = "_tabFruta";

        _repoCosto.AutoHeight = false;
        _repoCosto.Mask.EditMask = "n4";
        _repoCosto.MinValue = 0m;
        _repoCosto.Name = "_repoCosto";

        _colCategoriaNombre.Caption = "Categoría"; _colCategoriaNombre.FieldName = "MateriaPrimaNombre"; _colCategoriaNombre.Name = "_colCategoriaNombre";
        _colCategoriaNombre.OptionsColumn.AllowEdit = false; _colCategoriaNombre.Visible = true; _colCategoriaNombre.VisibleIndex = 0; _colCategoriaNombre.Width = 260;

        _colKgSeleccionados.Caption = "Kg Seleccionados"; _colKgSeleccionados.FieldName = "KilogramosSeleccionados"; _colKgSeleccionados.Name = "_colKgSeleccionados";
        _colKgSeleccionados.OptionsColumn.AllowEdit = false; _colKgSeleccionados.DisplayFormat.FormatType = FormatType.Numeric; _colKgSeleccionados.DisplayFormat.FormatString = "n2";
        _colKgSeleccionados.Visible = true; _colKgSeleccionados.VisibleIndex = 1; _colKgSeleccionados.Width = 110;
        _colKgSeleccionados.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "KilogramosSeleccionados", "{0:n2}");

        _colPorcentaje.Caption = "%"; _colPorcentaje.FieldName = "Porcentaje"; _colPorcentaje.Name = "_colPorcentaje";
        _colPorcentaje.OptionsColumn.AllowEdit = false; _colPorcentaje.DisplayFormat.FormatType = FormatType.Numeric; _colPorcentaje.DisplayFormat.FormatString = "n2";
        _colPorcentaje.Visible = true; _colPorcentaje.VisibleIndex = 2; _colPorcentaje.Width = 70;
        _colPorcentaje.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Porcentaje", "{0:n2}");

        _colKgComprados.Caption = "Kg Comprados"; _colKgComprados.FieldName = "KilogramosComprados"; _colKgComprados.Name = "_colKgComprados";
        _colKgComprados.OptionsColumn.AllowEdit = false; _colKgComprados.DisplayFormat.FormatType = FormatType.Numeric; _colKgComprados.DisplayFormat.FormatString = "n2";
        _colKgComprados.Visible = true; _colKgComprados.VisibleIndex = 3; _colKgComprados.Width = 110;

        _colCostoRealUnitario.Caption = "Costo Real"; _colCostoRealUnitario.FieldName = "CostoRealUnitario"; _colCostoRealUnitario.Name = "_colCostoRealUnitario";
        _colCostoRealUnitario.ColumnEdit = _repoCosto; _colCostoRealUnitario.DisplayFormat.FormatType = FormatType.Numeric; _colCostoRealUnitario.DisplayFormat.FormatString = "n4";
        _colCostoRealUnitario.Visible = true; _colCostoRealUnitario.VisibleIndex = 4; _colCostoRealUnitario.Width = 100;
        _colCostoRealUnitario.Summary.Add(DevExpress.Data.SummaryItemType.Average, "CostoRealUnitario", "{0:n4}");

        _colImporteReal.Caption = "Importe Real"; _colImporteReal.FieldName = "ImporteReal"; _colImporteReal.Name = "_colImporteReal";
        _colImporteReal.OptionsColumn.AllowEdit = false; _colImporteReal.DisplayFormat.FormatType = FormatType.Numeric; _colImporteReal.DisplayFormat.FormatString = "c2";
        _colImporteReal.Visible = true; _colImporteReal.VisibleIndex = 5; _colImporteReal.Width = 110;
        _colImporteReal.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "ImporteReal", "{0:c2}");

        _colCostoEstimadoUnitario.Caption = "Costo Estimado"; _colCostoEstimadoUnitario.FieldName = "CostoEstimadoUnitario"; _colCostoEstimadoUnitario.Name = "_colCostoEstimadoUnitario";
        _colCostoEstimadoUnitario.ColumnEdit = _repoCosto; _colCostoEstimadoUnitario.DisplayFormat.FormatType = FormatType.Numeric; _colCostoEstimadoUnitario.DisplayFormat.FormatString = "n4";
        _colCostoEstimadoUnitario.Visible = true; _colCostoEstimadoUnitario.VisibleIndex = 6; _colCostoEstimadoUnitario.Width = 110;
        _colCostoEstimadoUnitario.Summary.Add(DevExpress.Data.SummaryItemType.Average, "CostoEstimadoUnitario", "{0:n4}");

        _colImporteEstimado.Caption = "Importe Estimado"; _colImporteEstimado.FieldName = "ImporteEstimado"; _colImporteEstimado.Name = "_colImporteEstimado";
        _colImporteEstimado.OptionsColumn.AllowEdit = false; _colImporteEstimado.DisplayFormat.FormatType = FormatType.Numeric; _colImporteEstimado.DisplayFormat.FormatString = "c2";
        _colImporteEstimado.Visible = true; _colImporteEstimado.VisibleIndex = 7; _colImporteEstimado.Width = 120;
        _colImporteEstimado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "ImporteEstimado", "{0:c2}");

        _gridViewFruta.Columns.AddRange(new GridColumn[] { _colCategoriaNombre, _colKgSeleccionados, _colPorcentaje, _colKgComprados, _colCostoRealUnitario, _colImporteReal, _colCostoEstimadoUnitario, _colImporteEstimado });
        _gridViewFruta.GridControl = _gridFruta;
        _gridViewFruta.Name = "_gridViewFruta";
        _gridViewFruta.OptionsBehavior.Editable = true;
        _gridViewFruta.OptionsFind.AlwaysVisible = true;
        _gridViewFruta.OptionsView.ShowGroupPanel = false;
        _gridViewFruta.OptionsView.ColumnAutoWidth = false;
        _gridViewFruta.OptionsView.ShowFooter = true;
        _gridViewFruta.CellValueChanged += GridViewFruta_CellValueChanged;

        _gridFruta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _gridFruta.Location = new Point(10, 10);
        _gridFruta.MainView = _gridViewFruta;
        _gridFruta.Name = "_gridFruta";
        _gridFruta.RepositoryItems.AddRange(new RepositoryItem[] { _repoCosto });
        _gridFruta.Size = new Size(1000, 260);
        _gridFruta.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewFruta });

        _cmbCostoEstimadoListaPrecioNumero.Location = new Point(10, 290);
        _cmbCostoEstimadoListaPrecioNumero.Name = "_cmbCostoEstimadoListaPrecioNumero";
        _cmbCostoEstimadoListaPrecioNumero.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbCostoEstimadoListaPrecioNumero.Size = new Size(110, 20);

        _btnVigenciaEstimado.Location = new Point(130, 290);
        _btnVigenciaEstimado.Name = "_btnVigenciaEstimado";
        _btnVigenciaEstimado.Size = new Size(190, 23);
        _btnVigenciaEstimado.Text = "Vigencia de Costo Estimado...";
        _btnVigenciaEstimado.Click += BtnVigenciaEstimado_Click;

        _lblVigenciaEstimado.Location = new Point(330, 295); _lblVigenciaEstimado.Size = new Size(400, 13);

        _cmbTipoReporte.Location = new Point(10, 320);
        _cmbTipoReporte.Name = "_cmbTipoReporte";
        _cmbTipoReporte.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbTipoReporte.Size = new Size(340, 20);

        _btnImprimir.Location = new Point(360, 320);
        _btnImprimir.Name = "_btnImprimir";
        _btnImprimir.Size = new Size(100, 23);
        _btnImprimir.Text = "Imprimir";
        _btnImprimir.Click += BtnImprimir_Click;

        _tabFruta.Controls.Add(_gridFruta);
        _tabFruta.Controls.Add(_cmbCostoEstimadoListaPrecioNumero);
        _tabFruta.Controls.Add(_btnVigenciaEstimado);
        _tabFruta.Controls.Add(_lblVigenciaEstimado);
        _tabFruta.Controls.Add(_cmbTipoReporte);
        _tabFruta.Controls.Add(_btnImprimir);

        // ---- Tab Cosecha ----
        _tabCosecha.Text = "Cosecha";
        _tabCosecha.Name = "_tabCosecha";

        ConfigurarGridRecepcion(_gridCosecha, _gridViewCosecha, incluirPesoProductor: false);
        _gridViewCosecha.MouseMove += GridViewRecepcion_MouseMove;
        _gridViewCosecha.RowCellClick += GridViewRecepcion_RowCellClick;

        _btnAgregarAjusteCosecha.Location = new Point(10, 282); _btnAgregarAjusteCosecha.Size = new Size(140, 23);
        _btnAgregarAjusteCosecha.Text = "Agregar Ajuste"; _btnAgregarAjusteCosecha.Click += BtnAgregarAjusteCosecha_Click;
        _btnEliminarAjusteCosecha.Location = new Point(160, 282); _btnEliminarAjusteCosecha.Size = new Size(140, 23);
        _btnEliminarAjusteCosecha.Text = "Eliminar Ajuste"; _btnEliminarAjusteCosecha.Click += BtnEliminarAjusteCosecha_Click;

        _lblTotalEmpresaCosechaCap.Location = new Point(10, 316); _lblTotalEmpresaCosechaCap.Text = "Total con cargo a Empresa:"; _lblTotalEmpresaCosechaCap.Size = new Size(170, 13);
        _lblTotalEmpresaCosecha.Location = new Point(190, 316); _lblTotalEmpresaCosecha.Size = new Size(120, 13);
        _lblTotalProductorCosechaCap.Location = new Point(10, 336); _lblTotalProductorCosechaCap.Text = "Total con cargo a Productor:"; _lblTotalProductorCosechaCap.Size = new Size(170, 13);
        _lblTotalProductorCosecha.Location = new Point(190, 336); _lblTotalProductorCosecha.Size = new Size(120, 13);

        _tabCosecha.Controls.Add(_gridCosecha);
        _tabCosecha.Controls.Add(_btnAgregarAjusteCosecha);
        _tabCosecha.Controls.Add(_btnEliminarAjusteCosecha);
        _tabCosecha.Controls.Add(_lblTotalEmpresaCosechaCap); _tabCosecha.Controls.Add(_lblTotalEmpresaCosecha);
        _tabCosecha.Controls.Add(_lblTotalProductorCosechaCap); _tabCosecha.Controls.Add(_lblTotalProductorCosecha);

        // ---- Tab Acarreo ----
        _tabAcarreo.Text = "Acarreo";
        _tabAcarreo.Name = "_tabAcarreo";

        ConfigurarGridRecepcion(_gridAcarreo, _gridViewAcarreo, incluirPesoProductor: true);
        _gridViewAcarreo.MouseMove += GridViewRecepcion_MouseMove;
        _gridViewAcarreo.RowCellClick += GridViewRecepcion_RowCellClick;

        _btnAgregarAjusteAcarreo.Location = new Point(10, 282); _btnAgregarAjusteAcarreo.Size = new Size(140, 23);
        _btnAgregarAjusteAcarreo.Text = "Agregar Ajuste"; _btnAgregarAjusteAcarreo.Click += BtnAgregarAjusteAcarreo_Click;
        _btnEliminarAjusteAcarreo.Location = new Point(160, 282); _btnEliminarAjusteAcarreo.Size = new Size(140, 23);
        _btnEliminarAjusteAcarreo.Text = "Eliminar Ajuste"; _btnEliminarAjusteAcarreo.Click += BtnEliminarAjusteAcarreo_Click;

        _lblTotalEmpresaAcarreoCap.Location = new Point(10, 316); _lblTotalEmpresaAcarreoCap.Text = "Total con cargo a Empresa:"; _lblTotalEmpresaAcarreoCap.Size = new Size(170, 13);
        _lblTotalEmpresaAcarreo.Location = new Point(190, 316); _lblTotalEmpresaAcarreo.Size = new Size(120, 13);
        _lblTotalProductorAcarreoCap.Location = new Point(10, 336); _lblTotalProductorAcarreoCap.Text = "Total con cargo a Productor:"; _lblTotalProductorAcarreoCap.Size = new Size(170, 13);
        _lblTotalProductorAcarreo.Location = new Point(190, 336); _lblTotalProductorAcarreo.Size = new Size(120, 13);

        _tabAcarreo.Controls.Add(_gridAcarreo);
        _tabAcarreo.Controls.Add(_btnAgregarAjusteAcarreo);
        _tabAcarreo.Controls.Add(_btnEliminarAjusteAcarreo);
        _tabAcarreo.Controls.Add(_lblTotalEmpresaAcarreoCap); _tabAcarreo.Controls.Add(_lblTotalEmpresaAcarreo);
        _tabAcarreo.Controls.Add(_lblTotalProductorAcarreoCap); _tabAcarreo.Controls.Add(_lblTotalProductorAcarreo);

        // ---- Botones ----
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(15, 585);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;

        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(965, 585);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;

        // ---- Form ----
        AcceptButton = null;
        ClientSize = new Size(1060, 620);
        Controls.Add(_lblFolioCap); Controls.Add(_lblFolio);
        Controls.Add(_lblHuertaCap); Controls.Add(_lblHuerta);
        Controls.Add(_lblRegistroSagarpaCap); Controls.Add(_lblRegistroSagarpa);
        Controls.Add(_lblFechaCap); Controls.Add(_lblFecha);
        Controls.Add(_lblPesoCap); Controls.Add(_lblPeso);
        Controls.Add(_lblTipoPagoCap); Controls.Add(_lblTipoPago);
        Controls.Add(_lblTipoCorteCap); Controls.Add(_lblTipoCorte);
        Controls.Add(_lblVariedadCap); Controls.Add(_lblVariedad);
        Controls.Add(_lblPrecioAcordadoCap); Controls.Add(_lblPrecioAcordado);
        Controls.Add(_tabControl);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        MinimumSize = new Size(900, 500);
        Name = "GastoLoteForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Gastos";

        _tabFruta.ResumeLayout(false);
        _tabCosecha.ResumeLayout(false);
        _tabAcarreo.ResumeLayout(false);
        _tabControl.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_tabControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridFruta).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewFruta).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoCosto).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbCostoEstimadoListaPrecioNumero.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbTipoReporte.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridCosecha).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewCosecha).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridAcarreo).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewAcarreo).EndInit();
        ResumeLayout(false);
    }

    // Cosecha y Acarreo comparten exactamente el mismo layout de grid (Recepción/Orden de Corte/
    // Peso Neto[/Peso Productor]/Concepto/Cantidad/Precio Unitario/Importe/Empresa/Productor) —
    // se arma una sola vez por grid. Recepción y Orden de Corte se ven como folio clickeable
    // (azul + negritas), mismo criterio que RecepcionesFrutaForm — el clic lo maneja
    // GastoLoteForm.cs (RowCellClick/MouseMove sobre estas 2 columnas).
    private static void ConfigurarGridRecepcion(GridControl grid, GridView view, bool incluirPesoProductor)
    {
        var indice = 0;

        var colFolio = new GridColumn { Caption = "Recepción", FieldName = "RecepcionFolio", Name = "colFolio", Visible = true, VisibleIndex = indice++, Width = 100 };
        colFolio.OptionsColumn.AllowEdit = false;
        AplicarEstiloFolioClickeable(colFolio);

        var colOrdenCorte = new GridColumn { Caption = "Orden de Corte", FieldName = "OrdenCorteFolio", Name = "colOrdenCorte", Visible = true, VisibleIndex = indice++, Width = 110 };
        colOrdenCorte.OptionsColumn.AllowEdit = false;
        AplicarEstiloFolioClickeable(colOrdenCorte);

        var colPesoNeto = new GridColumn { Caption = "Peso Neto", FieldName = "PesoNeto", Name = "colPesoNeto", Visible = true, VisibleIndex = indice++, Width = 100 };
        colPesoNeto.OptionsColumn.AllowEdit = false;
        colPesoNeto.DisplayFormat.FormatType = FormatType.Numeric;
        colPesoNeto.DisplayFormat.FormatString = "n2";

        GridColumn? colPesoProductor = null;
        if (incluirPesoProductor)
        {
            colPesoProductor = new GridColumn { Caption = "Peso Productor", FieldName = "PesoProductor", Name = "colPesoProductor", Visible = true, VisibleIndex = indice++, Width = 100 };
            colPesoProductor.OptionsColumn.AllowEdit = false;
            colPesoProductor.DisplayFormat.FormatType = FormatType.Numeric;
            colPesoProductor.DisplayFormat.FormatString = "n2";
        }

        var colConcepto = new GridColumn { Caption = "Concepto", FieldName = "Concepto", Name = "colConcepto", Visible = true, VisibleIndex = indice++, Width = 220 };
        colConcepto.OptionsColumn.AllowEdit = false;

        var colCantidad = new GridColumn { Caption = "Cantidad", FieldName = "Cantidad", Name = "colCantidad", Visible = true, VisibleIndex = indice++, Width = 90 };
        colCantidad.OptionsColumn.AllowEdit = false;
        colCantidad.DisplayFormat.FormatType = FormatType.Numeric;
        colCantidad.DisplayFormat.FormatString = "n2";

        var colPUnitario = new GridColumn { Caption = "P. Unitario", FieldName = "PrecioUnitario", Name = "colPUnitario", Visible = true, VisibleIndex = indice++, Width = 100 };
        colPUnitario.OptionsColumn.AllowEdit = false;
        colPUnitario.DisplayFormat.FormatType = FormatType.Numeric;
        colPUnitario.DisplayFormat.FormatString = "c2";

        var colImporte = new GridColumn { Caption = "Importe", FieldName = "Importe", Name = "colImporte", Visible = true, VisibleIndex = indice++, Width = 110 };
        colImporte.OptionsColumn.AllowEdit = false;
        colImporte.DisplayFormat.FormatType = FormatType.Numeric;
        colImporte.DisplayFormat.FormatString = "c2";

        // Con cargo a Empresa/Productor: AllowEdit=false a propósito — el checkbox nativo de
        // DevExpress (clic entra en modo edición y solo postea/repinta la celda hermana al
        // perder el foco) es lo que dejaba ambas columnas marcadas a la vez. Se deja como celda
        // de solo lectura (sigue mostrando el glyph de check/no-check) y el toggle real se
        // maneja a mano en GastoLoteForm.cs (GridViewRecepcion_RowCellClick), sin editor de por
        // medio y sin depender del ciclo de commit del grid.
        var colEmpresa = new GridColumn { Caption = "Con cargo a Empresa", FieldName = "CargoEmpresa", Name = "colEmpresa", Visible = true, VisibleIndex = indice++, Width = 130 };
        colEmpresa.OptionsColumn.AllowEdit = false;
        var colProductor = new GridColumn { Caption = "Con cargo a Productor", FieldName = "CargoProductor", Name = "colProductor", Visible = true, VisibleIndex = indice++, Width = 130 };
        colProductor.OptionsColumn.AllowEdit = false;

        var columnas = new List<GridColumn> { colFolio, colOrdenCorte, colPesoNeto };
        if (colPesoProductor is not null)
        {
            columnas.Add(colPesoProductor);
        }
        columnas.AddRange(new[] { colConcepto, colCantidad, colPUnitario, colImporte, colEmpresa, colProductor });

        view.Columns.AddRange(columnas.ToArray());
        view.GridControl = grid;
        view.OptionsBehavior.Editable = true;
        view.OptionsFind.AlwaysVisible = true;
        view.OptionsView.ShowGroupPanel = false;
        view.OptionsView.ColumnAutoWidth = false;

        grid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grid.Location = new Point(10, 10);
        grid.MainView = view;
        grid.Size = new Size(1000, 260);
        grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { view });
    }

    // Mismo criterio visual que RecepcionesFrutaForm.AplicarEstiloFolioClickeable.
    private static void AplicarEstiloFolioClickeable(GridColumn columna)
    {
        columna.AppearanceCell.Font = new Font(columna.AppearanceCell.Font, FontStyle.Bold | FontStyle.Underline);
        columna.AppearanceCell.ForeColor = ColorTranslator.FromHtml("#0563C1");
        columna.AppearanceCell.Options.UseFont = true;
        columna.AppearanceCell.Options.UseForeColor = true;
    }

    #endregion
}
