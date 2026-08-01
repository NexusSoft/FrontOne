using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class VerListaPrecioFrutaForm
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

    private LabelControl _lblTitulo;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerListaPrecioFrutaForm));
        _lblTitulo = new LabelControl();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblTitulo
        //
        _lblTitulo.Location = new Point(12, 12);
        _lblTitulo.Name = "_lblTitulo";
        _lblTitulo.Size = new Size(10, 13);
        _lblTitulo.TabIndex = 0;
        _lblTitulo.Text = "-";
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(12, 32);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(576, 360);
        _grid.TabIndex = 1;
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
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(498, 402);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 2;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // VerListaPrecioFrutaForm
        //
        ClientSize = new Size(600, 437);
        Controls.Add(_lblTitulo);
        Controls.Add(_grid);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(480, 320);
        Name = "VerListaPrecioFrutaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Ver Lista de Precios";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
