using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class EstadosForm
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

    private LabelControl _lblFiltro;
    private LookUpEdit _cmbFiltroPais;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnNuevo;
    private SimpleButton _btnEditar;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstadosForm));
        _lblFiltro = new LabelControl();
        _cmbFiltroPais = new LookUpEdit();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnNuevo = new SimpleButton();
        _btnEditar = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        // 
        // _lblFiltro
        // 
        _lblFiltro.Location = new Point(10, 15);
        _lblFiltro.Name = "_lblFiltro";
        _lblFiltro.Size = new Size(19, 13);
        _lblFiltro.TabIndex = 0;
        _lblFiltro.Text = "País";
        // 
        // _cmbFiltroPais
        // 
        _cmbFiltroPais.Location = new Point(55, 12);
        _cmbFiltroPais.Name = "_cmbFiltroPais";
        _cmbFiltroPais.Properties.NullText = "Seleccionar";
        _cmbFiltroPais.Size = new Size(250, 20);
        _cmbFiltroPais.TabIndex = 1;
        // 
        // _grid
        // 
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 45);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(741, 456);
        _grid.TabIndex = 2;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        // 
        // _gridView
        // 
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.ShowGroupPanel = false;
        // 
        // _btnNuevo
        // 
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Location = new Point(10, 517);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.TabIndex = 3;
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        // 
        // _btnEditar
        // 
        _btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditar.Location = new Point(106, 517);
        _btnEditar.Name = "_btnEditar";
        _btnEditar.Size = new Size(90, 23);
        _btnEditar.TabIndex = 4;
        _btnEditar.Text = "Editar";
        _btnEditar.Click += BtnEditar_Click;
        // 
        // _btnEliminar
        // 
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Location = new Point(202, 517);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.TabIndex = 5;
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        // 
        // _btnCerrar
        // 
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(661, 517);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 6;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        // 
        // EstadosForm
        // 
        ClientSize = new Size(763, 552);
        Controls.Add(_lblFiltro);
        Controls.Add(_cmbFiltroPais);
        Controls.Add(_grid);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnEditar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EstadosForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Estados";
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
