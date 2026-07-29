using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Sistema;

partial class ReportesForm
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
    private SimpleButton _btnDisenar;
    private SimpleButton _btnRestablecer;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportesForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _btnDisenar = new SimpleButton();
        _btnRestablecer = new SimpleButton();
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
        _grid.Size = new Size(700, 400);
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        //
        // _btnDisenar
        //
        _btnDisenar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnDisenar.Location = new Point(12, 425);
        _btnDisenar.Name = "_btnDisenar";
        _btnDisenar.Size = new Size(100, 23);
        _btnDisenar.Text = "Diseñar";
        _btnDisenar.Click += BtnDisenar_Click;
        //
        // _btnRestablecer
        //
        _btnRestablecer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnRestablecer.Location = new Point(118, 425);
        _btnRestablecer.Name = "_btnRestablecer";
        _btnRestablecer.Size = new Size(100, 23);
        _btnRestablecer.Text = "Restablecer";
        _btnRestablecer.Click += BtnRestablecer_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(620, 425);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // ReportesForm
        //
        ClientSize = new Size(720, 460);
        Controls.Add(_grid);
        Controls.Add(_btnDisenar);
        Controls.Add(_btnRestablecer);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(500, 350);
        Name = "ReportesForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Reportes";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
