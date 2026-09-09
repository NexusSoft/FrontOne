using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class BuscarEstimacionForm
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

    private LabelControl _lblBuscar;
    private TextEdit _txtBuscar;
    private SimpleButton _btnBuscar;
    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colFolio;
    private GridColumn _colFecha;
    private GridColumn _colHuerta;
    private GridColumn _colPrecioSugerido;
    private GridColumn _colEstatus;
    private SimpleButton _btnSeleccionar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuscarEstimacionForm));
        _lblBuscar = new LabelControl();
        _txtBuscar = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colFolio = new GridColumn();
        _colFecha = new GridColumn();
        _colHuerta = new GridColumn();
        _colPrecioSugerido = new GridColumn();
        _colEstatus = new GridColumn();
        _btnSeleccionar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtBuscar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblBuscar
        //
        _lblBuscar.Location = new Point(10, 15);
        _lblBuscar.Name = "_lblBuscar";
        _lblBuscar.Size = new Size(36, 13);
        _lblBuscar.Text = "Buscar:";
        //
        // _txtBuscar
        //
        _txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _txtBuscar.Location = new Point(60, 12);
        _txtBuscar.Name = "_txtBuscar";
        _txtBuscar.Properties.NullValuePrompt = "Folio o nombre de huerta";
        _txtBuscar.Size = new Size(454, 20);
        _txtBuscar.TabIndex = 0;
        _txtBuscar.KeyDown += TxtBuscar_KeyDown;
        //
        // _btnBuscar
        //
        _btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnBuscar.Location = new Point(520, 10);
        _btnBuscar.Name = "_btnBuscar";
        _btnBuscar.Size = new Size(90, 23);
        _btnBuscar.TabIndex = 1;
        _btnBuscar.Text = "Buscar";
        _btnBuscar.Click += BtnBuscar_Click;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 40);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(600, 320);
        _grid.TabIndex = 2;
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsSelection.MultiSelect = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.Columns.AddRange(new GridColumn[] { _colFolio, _colFecha, _colHuerta, _colPrecioSugerido, _colEstatus });
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _colFolio
        //
        _colFolio.Caption = "Folio";
        _colFolio.FieldName = "Folio";
        _colFolio.Name = "_colFolio";
        _colFolio.Visible = true;
        _colFolio.VisibleIndex = 0;
        _colFolio.Width = 80;
        //
        // _colFecha
        //
        _colFecha.Caption = "Fecha";
        _colFecha.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        _colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        _colFecha.FieldName = "Fecha";
        _colFecha.Name = "_colFecha";
        _colFecha.Visible = true;
        _colFecha.VisibleIndex = 1;
        _colFecha.Width = 90;
        //
        // _colHuerta
        //
        _colHuerta.Caption = "Huerta";
        _colHuerta.FieldName = "Huerta";
        _colHuerta.Name = "_colHuerta";
        _colHuerta.Visible = true;
        _colHuerta.VisibleIndex = 2;
        _colHuerta.Width = 220;
        //
        // _colPrecioSugerido
        //
        _colPrecioSugerido.Caption = "Precio Sugerido";
        _colPrecioSugerido.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _colPrecioSugerido.DisplayFormat.FormatString = "c2";
        _colPrecioSugerido.FieldName = "PrecioSugerido";
        _colPrecioSugerido.Name = "_colPrecioSugerido";
        _colPrecioSugerido.Visible = true;
        _colPrecioSugerido.VisibleIndex = 3;
        _colPrecioSugerido.Width = 110;
        //
        // _colEstatus
        //
        _colEstatus.Caption = "Estatus";
        _colEstatus.FieldName = "Estatus";
        _colEstatus.Name = "_colEstatus";
        _colEstatus.Visible = true;
        _colEstatus.VisibleIndex = 4;
        _colEstatus.Width = 90;
        //
        // _btnSeleccionar
        //
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSeleccionar.Location = new Point(10, 370);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(110, 23);
        _btnSeleccionar.TabIndex = 3;
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Click += BtnSeleccionar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(520, 370);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 4;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // BuscarEstimacionForm
        //
        ClientSize = new Size(620, 420);
        Controls.Add(_lblBuscar);
        Controls.Add(_txtBuscar);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "BuscarEstimacionForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Buscar estimación";
        Load += BuscarEstimacionForm_Load;
        ((System.ComponentModel.ISupportInitialize)_txtBuscar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
