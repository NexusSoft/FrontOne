using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class BuscarListaPrecioFrutaForm
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
    private SimpleButton _btnSeleccionar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuscarListaPrecioFrutaForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _btnSeleccionar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 10);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(618, 360);
        _grid.TabIndex = 0;
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
        // _btnSeleccionar
        //
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSeleccionar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnSeleccionar.Location = new Point(12, 385);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(90, 23);
        _btnSeleccionar.TabIndex = 1;
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Click += BtnSeleccionar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(538, 385);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 2;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // BuscarListaPrecioFrutaForm
        //
        ClientSize = new Size(640, 420);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(500, 300);
        Name = "BuscarListaPrecioFrutaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Buscar Lista de Precio Fruta";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
