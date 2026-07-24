using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acarreo;

partial class ListaPrecioAcarreoForm
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
    private GridColumn _colMunicipio;
    private GridColumn _colEstado;
    private GridColumn _colZona;
    private GridColumn _colPrecio300;
    private GridColumn _colPrecio400;
    private GridColumn _colPrecio500;
    private GridColumn _colTolerancia;
    private RepositoryItemLookUpEdit _repoMunicipio;
    private RepositoryItemLookUpEdit _repoZona;
    private RepositoryItemSpinEdit _repoPrecio;
    private RepositoryItemSpinEdit _repoTolerancia;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaPrecioAcarreoForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _colMunicipio = new GridColumn();
        _colEstado = new GridColumn();
        _colZona = new GridColumn();
        _colPrecio300 = new GridColumn();
        _colPrecio400 = new GridColumn();
        _colPrecio500 = new GridColumn();
        _colTolerancia = new GridColumn();
        _repoMunicipio = new RepositoryItemLookUpEdit();
        _repoZona = new RepositoryItemLookUpEdit();
        _repoPrecio = new RepositoryItemSpinEdit();
        _repoTolerancia = new RepositoryItemSpinEdit();
        _btnGuardar = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoMunicipio).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoZona).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoPrecio).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_repoTolerancia).BeginInit();
        SuspendLayout();
        //
        // _repoMunicipio
        //
        _repoMunicipio.AutoHeight = false;
        _repoMunicipio.Name = "_repoMunicipio";
        _repoMunicipio.NullText = "Seleccionar";
        _repoMunicipio.TextEditStyle = TextEditStyles.DisableTextEditor;
        //
        // _repoZona
        //
        _repoZona.AutoHeight = false;
        _repoZona.Name = "_repoZona";
        _repoZona.NullText = "Seleccionar";
        _repoZona.TextEditStyle = TextEditStyles.DisableTextEditor;
        //
        // _repoPrecio
        //
        _repoPrecio.AutoHeight = false;
        _repoPrecio.Mask.EditMask = "N02";
        _repoPrecio.MinValue = new decimal(new int[] { 0, 0, 0, 0 });
        _repoPrecio.Name = "_repoPrecio";
        //
        // _repoTolerancia
        //
        _repoTolerancia.AutoHeight = false;
        _repoTolerancia.MinValue = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        _repoTolerancia.MaxValue = new decimal(new int[] { 100000, 0, 0, 0 });
        _repoTolerancia.Name = "_repoTolerancia";
        //
        // _colMunicipio
        //
        _colMunicipio.Caption = "Municipio";
        _colMunicipio.ColumnEdit = _repoMunicipio;
        _colMunicipio.FieldName = "MunicipioId";
        _colMunicipio.Name = "_colMunicipio";
        _colMunicipio.Visible = true;
        _colMunicipio.VisibleIndex = 0;
        _colMunicipio.Width = 180;
        //
        // _colEstado
        //
        _colEstado.Caption = "Estado";
        _colEstado.FieldName = "EstadoNombre";
        _colEstado.Name = "_colEstado";
        _colEstado.OptionsColumn.AllowEdit = false;
        _colEstado.OptionsColumn.ReadOnly = true;
        _colEstado.Visible = true;
        _colEstado.VisibleIndex = 1;
        _colEstado.Width = 150;
        //
        // _colZona
        //
        _colZona.Caption = "Zona";
        _colZona.ColumnEdit = _repoZona;
        _colZona.FieldName = "ZonaId";
        _colZona.Name = "_colZona";
        _colZona.Visible = true;
        _colZona.VisibleIndex = 2;
        _colZona.Width = 100;
        //
        // _colPrecio300
        //
        _colPrecio300.Caption = "300 Cajas";
        _colPrecio300.ColumnEdit = _repoPrecio;
        _colPrecio300.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecio300.DisplayFormat.FormatString = "c2";
        _colPrecio300.FieldName = "Precio300";
        _colPrecio300.Name = "_colPrecio300";
        _colPrecio300.Visible = true;
        _colPrecio300.VisibleIndex = 3;
        _colPrecio300.Width = 110;
        //
        // _colPrecio400
        //
        _colPrecio400.Caption = "400 Cajas";
        _colPrecio400.ColumnEdit = _repoPrecio;
        _colPrecio400.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecio400.DisplayFormat.FormatString = "c2";
        _colPrecio400.FieldName = "Precio400";
        _colPrecio400.Name = "_colPrecio400";
        _colPrecio400.Visible = true;
        _colPrecio400.VisibleIndex = 4;
        _colPrecio400.Width = 110;
        //
        // _colPrecio500
        //
        _colPrecio500.Caption = "500 Cajas";
        _colPrecio500.ColumnEdit = _repoPrecio;
        _colPrecio500.DisplayFormat.FormatType = FormatType.Numeric;
        _colPrecio500.DisplayFormat.FormatString = "c2";
        _colPrecio500.FieldName = "Precio500";
        _colPrecio500.Name = "_colPrecio500";
        _colPrecio500.Visible = true;
        _colPrecio500.VisibleIndex = 5;
        _colPrecio500.Width = 110;
        //
        // _colTolerancia
        //
        _colTolerancia.Caption = "Tolerancia kg Faltante";
        _colTolerancia.ColumnEdit = _repoTolerancia;
        _colTolerancia.DisplayFormat.FormatType = FormatType.Numeric;
        _colTolerancia.DisplayFormat.FormatString = "n2";
        _colTolerancia.FieldName = "ToleranciaKgFaltante";
        _colTolerancia.Name = "_colTolerancia";
        _colTolerancia.Visible = true;
        _colTolerancia.VisibleIndex = 6;
        _colTolerancia.Width = 130;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 10);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.RepositoryItems.AddRange(new RepositoryItem[] { _repoMunicipio, _repoZona, _repoPrecio, _repoTolerancia });
        _grid.Size = new Size(900, 430);
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.Columns.AddRange(new GridColumn[] { _colMunicipio, _colEstado, _colZona, _colPrecio300, _colPrecio400, _colPrecio500, _colTolerancia });
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = true;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
        _gridView.OptionsView.ShowGroupPanel = false;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(12, 450);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(120, 23);
        _btnGuardar.Text = "Guardar cambios";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Location = new Point(138, 450);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.Size = new Size(110, 23);
        _btnEliminar.Text = "Eliminar fila";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(798, 450);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // ListaPrecioAcarreoForm
        //
        ClientSize = new Size(920, 485);
        Controls.Add(_grid);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(790, 400);
        Name = "ListaPrecioAcarreoForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FrontOne - Lista de Precios de Acarreo";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoMunicipio).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoZona).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoPrecio).EndInit();
        ((System.ComponentModel.ISupportInitialize)_repoTolerancia).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
