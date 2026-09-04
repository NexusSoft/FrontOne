using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Embarques;

partial class ContenedorPedidoBuscarForm
{
    private IContainer components = null;

    private LabelControl _lblFiltro;
    private TextEdit _txtFiltro;
    private SimpleButton _btnBuscar;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnSeleccionar;
    private SimpleButton _btnCerrar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContenedorPedidoBuscarForm));
        _lblFiltro = new LabelControl();
        _txtFiltro = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnSeleccionar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFiltro.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();

        _lblFiltro.Location = new Point(12, 15);
        _lblFiltro.Name = "_lblFiltro";
        _lblFiltro.Size = new Size(90, 13);
        _lblFiltro.Text = "No. Pedido / Cliente:";

        _txtFiltro.Location = new Point(140, 12);
        _txtFiltro.Name = "_txtFiltro";
        _txtFiltro.Size = new Size(200, 20);
        _txtFiltro.TabIndex = 0;
        _txtFiltro.KeyDown += TxtFiltro_KeyDown;

        _btnBuscar.Location = new Point(350, 11);
        _btnBuscar.Name = "_btnBuscar";
        _btnBuscar.Size = new Size(90, 28);
        _btnBuscar.TabIndex = 1;
        _btnBuscar.Text = "Buscar";
        _btnBuscar.Click += BtnBuscar_Click;

        _grid.Location = new Point(12, 45);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(760, 400);
        _grid.TabIndex = 2;
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });

        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.DoubleClick += GridView_DoubleClick;

        _btnSeleccionar.Location = new Point(12, 455);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(100, 28);
        _btnSeleccionar.TabIndex = 3;
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSeleccionar.Click += BtnSeleccionar_Click;

        _btnCerrar.Location = new Point(682, 455);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 28);
        _btnCerrar.TabIndex = 4;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Click += BtnCerrar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 490);
        Controls.Add(_lblFiltro);
        Controls.Add(_txtFiltro);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCerrar);
        Name = "ContenedorPedidoBuscarForm";
        Text = "Buscar Pedido SAP (Abiertos)";
        StartPosition = FormStartPosition.CenterParent;
        ((System.ComponentModel.ISupportInitialize)_txtFiltro.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
