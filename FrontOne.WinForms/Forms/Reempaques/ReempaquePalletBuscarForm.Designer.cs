using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Reempaques;

partial class ReempaquePalletBuscarForm
{
    private IContainer components = null;

    private LabelControl _lblFolio;
    private TextEdit _txtFolio;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReempaquePalletBuscarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnSeleccionar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();

        _lblFolio.Location = new Point(12, 15);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(70, 13);
        _lblFolio.Text = "No. de Pallet:";

        _txtFolio.Location = new Point(90, 12);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Size = new Size(150, 20);
        _txtFolio.TabIndex = 0;
        _txtFolio.KeyDown += TxtFolio_KeyDown;

        _btnBuscar.Location = new Point(250, 11);
        _btnBuscar.Name = "_btnBuscar";
        _btnBuscar.Size = new Size(90, 23);
        _btnBuscar.TabIndex = 1;
        _btnBuscar.Text = "Buscar";
        _btnBuscar.Click += BtnBuscar_Click;

        _grid.Location = new Point(12, 45);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(650, 350);
        _grid.TabIndex = 2;
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });

        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.DoubleClick += GridView_DoubleClick;

        _btnSeleccionar.Location = new Point(12, 405);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(100, 23);
        _btnSeleccionar.TabIndex = 3;
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSeleccionar.Click += BtnSeleccionar_Click;

        _btnCerrar.Location = new Point(572, 405);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 4;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Click += BtnCerrar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(674, 440);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCerrar);
        Name = "ReempaquePalletBuscarForm";
        Text = "Buscar Pallet para Reempaque";
        StartPosition = FormStartPosition.CenterParent;
        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
