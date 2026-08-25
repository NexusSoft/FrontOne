using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class SimuladorBandasForm
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

    private LabelControl _lblListaPrecio;
    private ComboBoxEdit _cmbListaPrecio;
    private SimpleButton _btnCargarPrecios;
    private LabelControl _lblListaCargada;
    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colCalibreApeamNombre;
    private GridColumn _colCategoriaNombre;
    private GridColumn _colCategoriaId;
    private GridColumn _colCalibreApeamId;
    private GridColumn _colPrecio;
    private GridColumn _colPorcentaje;
    private GridColumn _colBanda;
    private RepositoryItemSpinEdit _repoPrecio;
    private RepositoryItemSpinEdit _repoPorcentaje;
    private LabelControl _lblAvisoSuma;
    private SimpleButton _btnLimpiar;
    private DropDownButton _btnExportar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimuladorBandasForm));
        _lblListaPrecio = new LabelControl();
        _cmbListaPrecio = new ComboBoxEdit();
        _btnCargarPrecios = new SimpleButton();
        _lblListaCargada = new LabelControl();
        _grid = new GridControl();
        _gridView = new GridView();
        _colCalibreApeamNombre = new GridColumn();
        _colCategoriaNombre = new GridColumn();
        _colCategoriaId = new GridColumn();
        _colCalibreApeamId = new GridColumn();
        _colPrecio = new GridColumn();
        _colPorcentaje = new GridColumn();
        _colBanda = new GridColumn();
        _repoPrecio = new RepositoryItemSpinEdit();
        _repoPorcentaje = new RepositoryItemSpinEdit();
        _lblAvisoSuma = new LabelControl();
        _btnLimpiar = new SimpleButton();
        _btnExportar = new DropDownButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbListaPrecio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoPrecio).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoPorcentaje).BeginInit();
        SuspendLayout();
        //
        // _lblListaPrecio
        //
        _lblListaPrecio.Location = new Point(12, 15);
        _lblListaPrecio.Name = "_lblListaPrecio";
        _lblListaPrecio.Size = new Size(75, 13);
        _lblListaPrecio.Text = "Lista a cargar:";
        //
        // _cmbListaPrecio
        //
        _cmbListaPrecio.Location = new Point(95, 12);
        _cmbListaPrecio.Name = "_cmbListaPrecio";
        _cmbListaPrecio.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbListaPrecio.Size = new Size(140, 20);
        _cmbListaPrecio.TabIndex = 0;
        //
        // _btnCargarPrecios
        //
        _btnCargarPrecios.Location = new Point(245, 11);
        _btnCargarPrecios.Name = "_btnCargarPrecios";
        _btnCargarPrecios.Size = new Size(240, 23);
        _btnCargarPrecios.TabIndex = 1;
        _btnCargarPrecios.Text = "Cargar Precios de una Lista...";
        _btnCargarPrecios.Click += BtnCargarPrecios_Click;
        //
        // _lblListaCargada
        //
        _lblListaCargada.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _lblListaCargada.Location = new Point(495, 15);
        _lblListaCargada.Name = "_lblListaCargada";
        _lblListaCargada.Size = new Size(293, 13);
        _lblListaCargada.Visible = false;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(12, 42);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.RepositoryItems.AddRange(new RepositoryItem[] { _repoPrecio, _repoPorcentaje });
        _grid.Size = new Size(776, 650);
        _grid.TabIndex = 2;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.Columns.AddRange(new GridColumn[] { _colCalibreApeamNombre, _colCategoriaNombre, _colCategoriaId, _colCalibreApeamId, _colPrecio, _colPorcentaje, _colBanda });
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = true;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.OptionsView.ShowFooter = true;
        _gridView.CellValueChanged += GridView_CellValueChanged;
        //
        // _colCalibreApeamNombre
        //
        _colCalibreApeamNombre.Caption = "Calibre APEAM";
        _colCalibreApeamNombre.FieldName = "CalibreApeamNombre";
        _colCalibreApeamNombre.Name = "_colCalibreApeamNombre";
        _colCalibreApeamNombre.OptionsColumn.AllowEdit = false;
        _colCalibreApeamNombre.Visible = true;
        _colCalibreApeamNombre.VisibleIndex = 0;
        _colCalibreApeamNombre.Width = 160;
        //
        // _colCategoriaNombre
        //
        _colCategoriaNombre.Caption = "Categoría";
        _colCategoriaNombre.FieldName = "CategoriaNombre";
        _colCategoriaNombre.Name = "_colCategoriaNombre";
        _colCategoriaNombre.OptionsColumn.AllowEdit = false;
        _colCategoriaNombre.Visible = true;
        _colCategoriaNombre.VisibleIndex = 1;
        _colCategoriaNombre.Width = 160;
        //
        // _colCategoriaId
        //
        _colCategoriaId.FieldName = "CategoriaId";
        _colCategoriaId.Name = "_colCategoriaId";
        _colCategoriaId.Visible = false;
        //
        // _colCalibreApeamId
        //
        _colCalibreApeamId.FieldName = "CalibreApeamId";
        _colCalibreApeamId.Name = "_colCalibreApeamId";
        _colCalibreApeamId.Visible = false;
        //
        // _colPrecio
        //
        // Precio/Banda con formato de moneda (c2, "$ 24.00") — mismo criterio que Importe Real/
        // Estimado de GastoLoteForm y Precio x Kg de ListaPrecioCorteForm.
        _colPrecio.Caption = "Precio x Kg";
        _colPrecio.ColumnEdit = _repoPrecio;
        _colPrecio.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecio.DisplayFormat.FormatString = "c2";
        _colPrecio.FieldName = "Precio";
        _colPrecio.Name = "_colPrecio";
        _colPrecio.Visible = true;
        _colPrecio.VisibleIndex = 2;
        _colPrecio.Width = 110;
        _colPrecio.AppearanceCell.BackColor = ColorTranslator.FromHtml("#D6EAF8"); // Azul, mismo tono que ListaPrecioFrutaForm
        _colPrecio.AppearanceCell.Options.UseBackColor = true;
        //
        // _colPorcentaje
        //
        // Formato numérico "0.00'%'" (custom, no FormatType.Percent): el valor ya viene en
        // escala de porcentaje (30.51, no 0.3051) — un formato "p2" lo multiplicaría por 100 de
        // más. El '%' entre comillas es un literal, no dispara esa multiplicación.
        _colPorcentaje.Caption = "% de la Curva";
        _colPorcentaje.ColumnEdit = _repoPorcentaje;
        _colPorcentaje.DisplayFormat.FormatType = FormatType.Numeric;
        _colPorcentaje.DisplayFormat.FormatString = "0.00'%'";
        _colPorcentaje.FieldName = "Porcentaje";
        _colPorcentaje.Name = "_colPorcentaje";
        _colPorcentaje.Visible = true;
        _colPorcentaje.VisibleIndex = 3;
        _colPorcentaje.Width = 110;
        _colPorcentaje.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Porcentaje", "{0:n2}'%'");
        _colPorcentaje.AppearanceCell.BackColor = ColorTranslator.FromHtml("#FDEBD0"); // Naranja
        _colPorcentaje.AppearanceCell.Options.UseBackColor = true;
        //
        // _colBanda
        //
        _colBanda.Caption = "Banda";
        _colBanda.DisplayFormat.FormatType = FormatType.Numeric;
        _colBanda.DisplayFormat.FormatString = "c2";
        _colBanda.FieldName = "Banda";
        _colBanda.Name = "_colBanda";
        _colBanda.OptionsColumn.AllowEdit = false;
        _colBanda.Visible = true;
        _colBanda.VisibleIndex = 4;
        _colBanda.Width = 110;
        _colBanda.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Banda", "{0:c2}");
        _colBanda.AppearanceCell.BackColor = ColorTranslator.FromHtml("#D5F5E3"); // Verde
        _colBanda.AppearanceCell.Options.UseBackColor = true;
        //
        // _repoPrecio
        //
        // Sin MinValue: el precio es siempre positivo en la práctica, pero no hay una regla de
        // negocio que lo exija — se deja libre igual que Porcentaje.
        _repoPrecio.AutoHeight = false;
        _repoPrecio.Mask.EditMask = "n2";
        _repoPrecio.Name = "_repoPrecio";
        //
        // _repoPorcentaje
        //
        // Sin MinValue a propósito: admite negativos (ej. MERMA en -4.18% del caso de
        // referencia del usuario).
        _repoPorcentaje.AutoHeight = false;
        _repoPorcentaje.Mask.EditMask = "n2";
        _repoPorcentaje.Name = "_repoPorcentaje";
        //
        // _lblAvisoSuma
        //
        _lblAvisoSuma.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _lblAvisoSuma.Appearance.ForeColor = Color.FromArgb(179, 38, 30);
        _lblAvisoSuma.Appearance.Options.UseForeColor = true;
        _lblAvisoSuma.Location = new Point(12, 698);
        _lblAvisoSuma.Name = "_lblAvisoSuma";
        _lblAvisoSuma.Size = new Size(776, 13);
        _lblAvisoSuma.Visible = false;
        //
        // _btnLimpiar
        //
        _btnLimpiar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnLimpiar.Location = new Point(12, 724);
        _btnLimpiar.Name = "_btnLimpiar";
        _btnLimpiar.Size = new Size(110, 23);
        _btnLimpiar.TabIndex = 3;
        _btnLimpiar.Text = "Limpiar";
        _btnLimpiar.Click += BtnLimpiar_Click;
        //
        // _btnExportar
        //
        _btnExportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnExportar.Location = new Point(618, 724);
        _btnExportar.Name = "_btnExportar";
        _btnExportar.Size = new Size(90, 23);
        _btnExportar.TabIndex = 4;
        _btnExportar.Text = "Exportar";
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(708, 724);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(80, 23);
        _btnCerrar.TabIndex = 5;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // SimuladorBandasForm
        //
        ClientSize = new Size(800, 759);
        Controls.Add(_lblListaPrecio);
        Controls.Add(_cmbListaPrecio);
        Controls.Add(_btnCargarPrecios);
        Controls.Add(_lblListaCargada);
        Controls.Add(_grid);
        Controls.Add(_lblAvisoSuma);
        Controls.Add(_btnLimpiar);
        Controls.Add(_btnExportar);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(700, 690);
        Name = "SimuladorBandasForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Simulador de Bandas";
        ((System.ComponentModel.ISupportInitialize)_cmbListaPrecio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoPrecio).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoPorcentaje).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
