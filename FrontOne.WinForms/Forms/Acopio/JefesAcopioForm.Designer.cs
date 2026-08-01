using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class JefesAcopioForm
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
    private GridColumn _colClave;
    private GridColumn _colNombre;
    private GridColumn _colEstado;
    private GridColumn _colEstatus;
    private SimpleButton _btnSeleccionar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JefesAcopioForm));
        _lblBuscar = new LabelControl();
        _txtBuscar = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colClave = new GridColumn();
        _colNombre = new GridColumn();
        _colEstado = new GridColumn();
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
        _txtBuscar.Properties.NullValuePrompt = "Clave o nombre del jefe de acopio";
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
        _gridView.Columns.AddRange(new GridColumn[] { _colClave, _colNombre, _colEstado, _colEstatus });
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _colClave
        //
        _colClave.Caption = "Clave";
        _colClave.FieldName = "Clave";
        _colClave.Name = "_colClave";
        _colClave.Visible = true;
        _colClave.VisibleIndex = 0;
        _colClave.Width = 60;
        //
        // _colNombre
        //
        _colNombre.Caption = "Nombre";
        _colNombre.FieldName = "Nombre";
        _colNombre.Name = "_colNombre";
        _colNombre.Visible = true;
        _colNombre.VisibleIndex = 1;
        _colNombre.Width = 380;
        //
        // _colEstado
        //
        _colEstado.Caption = "Estado";
        _colEstado.FieldName = "Estado";
        _colEstado.Name = "_colEstado";
        _colEstado.Visible = true;
        _colEstado.VisibleIndex = 2;
        _colEstado.Width = 150;
        //
        // _colEstatus
        //
        _colEstatus.Caption = "Estatus";
        _colEstatus.FieldName = "Estatus";
        _colEstatus.Name = "_colEstatus";
        _colEstatus.Visible = true;
        _colEstatus.VisibleIndex = 3;
        _colEstatus.Width = 60;
        //
        // _btnSeleccionar
        //
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSeleccionar.Location = new Point(10, 420);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(110, 23);
        _btnSeleccionar.TabIndex = 3;
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Click += BtnSeleccionar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(620, 420);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 4;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // JefesAcopioForm
        //
        ClientSize = new Size(720, 460);
        Controls.Add(_lblBuscar);
        Controls.Add(_txtBuscar);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "JefesAcopioForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Buscar jefe de acopio";
        Load += JefesAcopioForm_Load;
        ((System.ComponentModel.ISupportInitialize)_txtBuscar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
