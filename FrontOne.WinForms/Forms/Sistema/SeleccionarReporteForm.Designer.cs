using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Sistema;

partial class SeleccionarReporteForm
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

    private LabelControl _lblInstruccion;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnSeleccionar;
    private SimpleButton _btnCancelar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeleccionarReporteForm));
        _lblInstruccion = new LabelControl();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnSeleccionar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblInstruccion
        //
        _lblInstruccion.Location = new Point(10, 12);
        _lblInstruccion.Name = "_lblInstruccion";
        _lblInstruccion.Size = new Size(170, 13);
        _lblInstruccion.Text = "Selecciona el reporte a imprimir:";
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 32);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(460, 220);
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsSelection.MultiSelect = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _btnSeleccionar
        //
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnSeleccionar.Location = new Point(300, 262);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(80, 23);
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Click += BtnSeleccionar_Click;
        //
        // _btnCancelar
        //
        _btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Location = new Point(390, 262);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 23);
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.Click += BtnCancelar_Click;
        //
        // SeleccionarReporteForm
        //
        AcceptButton = _btnSeleccionar;
        CancelButton = _btnCancelar;
        ClientSize = new Size(480, 295);
        Controls.Add(_lblInstruccion);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SeleccionarReporteForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FrontOne - Seleccionar Reporte";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
