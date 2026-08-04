using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Almacenes;

partial class AlmacenCajaCampoDashboardForm
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
    private SimpleButton _btnCompra;
    private SimpleButton _btnAjuste;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlmacenCajaCampoDashboardForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _btnCompra = new SimpleButton();
        _btnAjuste = new SimpleButton();
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
        _grid.Size = new Size(698, 360);
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
        // _btnCompra
        //
        _btnCompra.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnCompra.Location = new Point(12, 385);
        _btnCompra.Name = "_btnCompra";
        _btnCompra.Size = new Size(130, 23);
        _btnCompra.Text = "Registrar Compra";
        _btnCompra.Click += BtnCompra_Click;
        //
        // _btnAjuste
        //
        _btnAjuste.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnAjuste.Location = new Point(148, 385);
        _btnAjuste.Name = "_btnAjuste";
        _btnAjuste.Size = new Size(90, 23);
        _btnAjuste.Text = "Ajuste";
        _btnAjuste.Click += BtnAjuste_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(618, 385);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // AlmacenCajaCampoDashboardForm
        //
        ClientSize = new Size(720, 420);
        Controls.Add(_grid);
        Controls.Add(_btnCompra);
        Controls.Add(_btnAjuste);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AlmacenCajaCampoDashboardForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Almacén de Caja de Campo";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
