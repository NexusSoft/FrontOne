using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Seguridad;

partial class PermisosEspecialesForm
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

    #region Código generado por el diseñador de formularios

    private LabelControl _lblRol;
    private LookUpEdit _cmbRol;
    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colDescripcion;
    private GridColumn _colHabilitado;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermisosEspecialesForm));
        _lblRol = new LabelControl();
        _cmbRol = new LookUpEdit();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colDescripcion = new GridColumn();
        _colHabilitado = new GridColumn();
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
        _gridView.Columns.AddRange(new GridColumn[] { _colDescripcion, _colHabilitado });
        //
        // _colDescripcion
        //
        _colDescripcion.Caption = "Permiso";
        _colDescripcion.FieldName = "Descripcion";
        _colDescripcion.Name = "_colDescripcion";
        _colDescripcion.OptionsColumn.AllowEdit = false;
        _colDescripcion.Visible = true;
        _colDescripcion.VisibleIndex = 0;
        _colDescripcion.Width = 200;
        //
        // _colHabilitado
        //
        _colHabilitado.Caption = "Habilitado";
        _colHabilitado.FieldName = "Habilitado";
        _colHabilitado.Name = "_colHabilitado";
        _colHabilitado.Visible = true;
        _colHabilitado.VisibleIndex = 1;
        _colHabilitado.Width = 80;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.Location = new Point(10, 405);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Size = new Size(80, 28);
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(590, 405);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(80, 28);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // PermisosEspecialesForm
        //
        AcceptButton = _btnGuardar;
        ClientSize = new Size(680, 440);
        Controls.Add(_lblRol);
        Controls.Add(_cmbRol);
        Controls.Add(_grid);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PermisosEspecialesForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Permisos Especiales por rol";
        ((System.ComponentModel.ISupportInitialize)_cmbRol.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
