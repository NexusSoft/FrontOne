using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class ProductosTerminadosForm
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
    private GridColumn _colCodigoSap;
    private GridColumn _colGrupoSap;
    private GridColumn _colDescripcionSap;
    private GridColumn _colDescripcionExtranjeraSap;
    private GridColumn _colActivo;
    private GridColumn _colDatosCapturados;
    private SimpleButton _btnEditar;
    private SimpleButton _btnSincronizar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductosTerminadosForm));
        _lblBuscar = new LabelControl();
        _txtBuscar = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colCodigoSap = new GridColumn();
        _colGrupoSap = new GridColumn();
        _colDescripcionSap = new GridColumn();
        _colDescripcionExtranjeraSap = new GridColumn();
        _colActivo = new GridColumn();
        _colDatosCapturados = new GridColumn();
        _btnEditar = new SimpleButton();
        _btnSincronizar = new SimpleButton();
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
        _txtBuscar.Properties.NullValuePrompt = "Código o descripción SAP";
        _txtBuscar.Size = new Size(554, 20);
        _txtBuscar.TabIndex = 0;
        _txtBuscar.KeyDown += TxtBuscar_KeyDown;
        //
        // _btnBuscar
        //
        _btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnBuscar.Location = new Point(620, 10);
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
        _grid.Size = new Size(700, 370);
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
        _gridView.Columns.AddRange(new GridColumn[] { _colCodigoSap, _colGrupoSap, _colDescripcionSap, _colDescripcionExtranjeraSap, _colActivo, _colDatosCapturados });
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _colCodigoSap
        //
        _colCodigoSap.Caption = "Código SAP";
        _colCodigoSap.FieldName = "CodigoSap";
        _colCodigoSap.Name = "_colCodigoSap";
        _colCodigoSap.Visible = true;
        _colCodigoSap.VisibleIndex = 0;
        _colCodigoSap.Width = 90;
        //
        // _colGrupoSap
        //
        _colGrupoSap.Caption = "Grupo";
        _colGrupoSap.FieldName = "GrupoSap";
        _colGrupoSap.Name = "_colGrupoSap";
        _colGrupoSap.Visible = true;
        _colGrupoSap.VisibleIndex = 1;
        _colGrupoSap.Width = 55;
        //
        // _colDescripcionSap
        //
        _colDescripcionSap.Caption = "Descripción";
        _colDescripcionSap.FieldName = "DescripcionSap";
        _colDescripcionSap.Name = "_colDescripcionSap";
        _colDescripcionSap.Visible = true;
        _colDescripcionSap.VisibleIndex = 2;
        _colDescripcionSap.Width = 320;
        //
        // _colDescripcionExtranjeraSap
        //
        _colDescripcionExtranjeraSap.Caption = "Descripción Extranjera";
        _colDescripcionExtranjeraSap.FieldName = "DescripcionExtranjeraSap";
        _colDescripcionExtranjeraSap.Name = "_colDescripcionExtranjeraSap";
        _colDescripcionExtranjeraSap.Visible = true;
        _colDescripcionExtranjeraSap.VisibleIndex = 3;
        _colDescripcionExtranjeraSap.Width = 220;
        //
        // _colActivo
        //
        _colActivo.Caption = "Activo";
        _colActivo.FieldName = "Activo";
        _colActivo.Name = "_colActivo";
        _colActivo.Visible = true;
        _colActivo.VisibleIndex = 4;
        _colActivo.Width = 60;
        //
        // _colDatosCapturados
        //
        _colDatosCapturados.Caption = "Datos Capturados";
        _colDatosCapturados.FieldName = "DatosCapturados";
        _colDatosCapturados.Name = "_colDatosCapturados";
        _colDatosCapturados.Visible = true;
        _colDatosCapturados.VisibleIndex = 5;
        _colDatosCapturados.Width = 110;
        //
        // _btnEditar
        //
        _btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditar.Location = new Point(10, 420);
        _btnEditar.Name = "_btnEditar";
        _btnEditar.Size = new Size(90, 23);
        _btnEditar.TabIndex = 3;
        _btnEditar.Text = "Editar";
        _btnEditar.Click += BtnEditar_Click;
        //
        // _btnSincronizar
        //
        _btnSincronizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSincronizar.Location = new Point(106, 420);
        _btnSincronizar.Name = "_btnSincronizar";
        _btnSincronizar.Size = new Size(120, 23);
        _btnSincronizar.TabIndex = 4;
        _btnSincronizar.Text = "Sincronizar";
        _btnSincronizar.Click += BtnSincronizar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(620, 420);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 5;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // ProductosTerminadosForm
        //
        ClientSize = new Size(720, 460);
        Controls.Add(_lblBuscar);
        Controls.Add(_txtBuscar);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_btnEditar);
        Controls.Add(_btnSincronizar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ProductosTerminadosForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Productos Terminados";
        Shown += ProductosTerminadosForm_Load;
        ((System.ComponentModel.ISupportInitialize)_txtBuscar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
