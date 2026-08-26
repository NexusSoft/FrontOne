using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Seguridad;

partial class PermisosAplicacionWebForm
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

    private LabelControl _lblRol;
    private LookUpEdit _cmbRol;
    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colPantallaCodigo;
    private GridColumn _colModulo;
    private GridColumn _colConsultar;
    private GridColumn _colCrear;
    private GridColumn _colModificar;
    private GridColumn _colEliminar;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermisosAplicacionWebForm));
        _lblRol = new LabelControl();
        _cmbRol = new LookUpEdit();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colPantallaCodigo = new GridColumn();
        _colModulo = new GridColumn();
        _colConsultar = new GridColumn();
        _colCrear = new GridColumn();
        _colModificar = new GridColumn();
        _colEliminar = new GridColumn();
        _btnGuardar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbRol.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblRol
        //
        _lblRol.Location = new Point(10, 18);
        _lblRol.Name = "_lblRol";
        _lblRol.Size = new Size(23, 13);
        _lblRol.Text = "Rol:";
        //
        // _cmbRol
        //
        _cmbRol.Location = new Point(60, 15);
        _cmbRol.Name = "_cmbRol";
        _cmbRol.Properties.NullText = "Seleccionar";
        _cmbRol.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
        _cmbRol.Properties.PopupFilterMode = PopupFilterMode.Contains;
        _cmbRol.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        _cmbRol.Size = new Size(280, 20);
        _cmbRol.EditValueChanged += CmbRol_EditValueChanged;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 45);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(660, 350);
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.OptionsBehavior.Editable = true;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsSelection.MultiSelect = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.Columns.AddRange(new GridColumn[] { _colPantallaCodigo, _colModulo, _colConsultar, _colCrear, _colModificar, _colEliminar });
        //
        // _colPantallaCodigo
        //
        _colPantallaCodigo.Caption = "Pantalla";
        _colPantallaCodigo.FieldName = "PantallaCodigo";
        _colPantallaCodigo.Name = "_colPantallaCodigo";
        _colPantallaCodigo.OptionsColumn.AllowEdit = false;
        _colPantallaCodigo.Visible = true;
        _colPantallaCodigo.VisibleIndex = 0;
        _colPantallaCodigo.Width = 200;
        //
        // _colModulo
        //
        _colModulo.Caption = "Módulo";
        _colModulo.FieldName = "Modulo";
        _colModulo.Name = "_colModulo";
        _colModulo.OptionsColumn.AllowEdit = false;
        _colModulo.Visible = true;
        _colModulo.VisibleIndex = 1;
        _colModulo.Width = 140;
        //
        // _colConsultar
        //
        _colConsultar.Caption = "Consultar";
        _colConsultar.FieldName = "Consultar";
        _colConsultar.Name = "_colConsultar";
        _colConsultar.Visible = true;
        _colConsultar.VisibleIndex = 2;
        _colConsultar.Width = 80;
        //
        // _colCrear
        //
        _colCrear.Caption = "Crear";
        _colCrear.FieldName = "Crear";
        _colCrear.Name = "_colCrear";
        _colCrear.Visible = true;
        _colCrear.VisibleIndex = 3;
        _colCrear.Width = 80;
        //
        // _colModificar
        //
        _colModificar.Caption = "Modificar";
        _colModificar.FieldName = "Modificar";
        _colModificar.Name = "_colModificar";
        _colModificar.Visible = true;
        _colModificar.VisibleIndex = 4;
        _colModificar.Width = 80;
        //
        // _colEliminar
        //
        _colEliminar.Caption = "Eliminar";
        _colEliminar.FieldName = "Eliminar";
        _colEliminar.Name = "_colEliminar";
        _colEliminar.Visible = true;
        _colEliminar.VisibleIndex = 5;
        _colEliminar.Width = 80;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.Location = new Point(10, 405);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(90, 23);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(580, 405);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // PermisosAplicacionWebForm
        //
        ClientSize = new Size(680, 440);
        Controls.Add(_lblRol);
        Controls.Add(_cmbRol);
        Controls.Add(_grid);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PermisosAplicacionWebForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Permisos de Aplicación Web por rol";
        ((System.ComponentModel.ISupportInitialize)_cmbRol.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
