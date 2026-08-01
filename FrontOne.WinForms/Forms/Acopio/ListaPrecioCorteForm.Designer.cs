using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class ListaPrecioCorteForm
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

    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colCardCode;
    private GridColumn _colCardName;
    private GridColumn _colPrecioKg;
    private GridColumn _colPrecioDia;
    private GridColumn _colCuadrillaApoyo;
    private RepositoryItemSpinEdit _repoPrecio;
    private SimpleButton _btnActualizar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaPrecioCorteForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _colCardCode = new GridColumn();
        _colCardName = new GridColumn();
        _colPrecioKg = new GridColumn();
        _colPrecioDia = new GridColumn();
        _colCuadrillaApoyo = new GridColumn();
        _repoPrecio = new RepositoryItemSpinEdit();
        _btnActualizar = new SimpleButton();
        _btnGuardar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoPrecio).BeginInit();
        SuspendLayout();
        //
        // _repoPrecio
        //
        _repoPrecio.AutoHeight = false;
        _repoPrecio.Mask.EditMask = "N02";
        _repoPrecio.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _repoPrecio.Name = "_repoPrecio";
        //
        // _colCardCode
        //
        _colCardCode.Caption = "Id";
        _colCardCode.FieldName = "CardCode";
        _colCardCode.Name = "_colCardCode";
        _colCardCode.OptionsColumn.AllowEdit = false;
        _colCardCode.OptionsColumn.ReadOnly = true;
        _colCardCode.Visible = true;
        _colCardCode.VisibleIndex = 0;
        _colCardCode.Width = 100;
        //
        // _colCardName
        //
        _colCardName.Caption = "Proveedor";
        _colCardName.FieldName = "CardName";
        _colCardName.Name = "_colCardName";
        _colCardName.OptionsColumn.AllowEdit = false;
        _colCardName.OptionsColumn.ReadOnly = true;
        _colCardName.Visible = true;
        _colCardName.VisibleIndex = 1;
        _colCardName.Width = 260;
        //
        // _colPrecioKg
        //
        _colPrecioKg.Caption = "Precio kg";
        _colPrecioKg.ColumnEdit = _repoPrecio;
        _colPrecioKg.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecioKg.DisplayFormat.FormatString = "c2";
        _colPrecioKg.FieldName = "PrecioKg";
        _colPrecioKg.Name = "_colPrecioKg";
        _colPrecioKg.Visible = true;
        _colPrecioKg.VisibleIndex = 2;
        _colPrecioKg.Width = 110;
        //
        // _colPrecioDia
        //
        _colPrecioDia.Caption = "Precio Día";
        _colPrecioDia.ColumnEdit = _repoPrecio;
        _colPrecioDia.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecioDia.DisplayFormat.FormatString = "c2";
        _colPrecioDia.FieldName = "PrecioDia";
        _colPrecioDia.Name = "_colPrecioDia";
        _colPrecioDia.Visible = true;
        _colPrecioDia.VisibleIndex = 3;
        _colPrecioDia.Width = 110;
        //
        // _colCuadrillaApoyo
        //
        _colCuadrillaApoyo.Caption = "Cuadrilla Apoyo";
        _colCuadrillaApoyo.ColumnEdit = _repoPrecio;
        _colCuadrillaApoyo.DisplayFormat.FormatType = FormatType.Numeric;
        _colCuadrillaApoyo.DisplayFormat.FormatString = "c2";
        _colCuadrillaApoyo.FieldName = "CuadrillaApoyo";
        _colCuadrillaApoyo.Name = "_colCuadrillaApoyo";
        _colCuadrillaApoyo.Visible = true;
        _colCuadrillaApoyo.VisibleIndex = 4;
        _colCuadrillaApoyo.Width = 130;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 10);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.RepositoryItems.AddRange(new RepositoryItem[] { _repoPrecio });
        _grid.Size = new Size(780, 430);
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.Columns.AddRange(new GridColumn[] { _colCardCode, _colCardName, _colPrecioKg, _colPrecioDia, _colCuadrillaApoyo });
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = true;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        _gridView.OptionsView.ShowGroupPanel = false;
        //
        // _btnActualizar
        //
        _btnActualizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnActualizar.Location = new Point(12, 450);
        _btnActualizar.Name = "_btnActualizar";
        _btnActualizar.Size = new Size(140, 23);
        _btnActualizar.Text = "Actualizar de SAP";
        _btnActualizar.Click += BtnActualizar_Click;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(158, 450);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(120, 23);
        _btnGuardar.Text = "Guardar cambios";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(698, 450);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // ListaPrecioCorteForm
        //
        ClientSize = new Size(800, 485);
        Controls.Add(_grid);
        Controls.Add(_btnActualizar);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(700, 400);
        Name = "ListaPrecioCorteForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Lista de Precios de Corte";
        Load += ListaPrecioCorteForm_Load;
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoPrecio).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
