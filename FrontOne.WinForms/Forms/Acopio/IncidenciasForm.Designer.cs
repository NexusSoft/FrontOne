using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class IncidenciasForm
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

    private LabelControl _lblDesde;
    private DateEdit _dtDesde;
    private LabelControl _lblHasta;
    private DateEdit _dtHasta;
    private SimpleButton _btnActualizar;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnEditar;
    private SimpleButton _btnGenerarPdf;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncidenciasForm));
        _lblDesde = new LabelControl();
        _dtDesde = new DateEdit();
        _lblHasta = new LabelControl();
        _dtHasta = new DateEdit();
        _btnActualizar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnEditar = new SimpleButton();
        _btnGenerarPdf = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_dtDesde.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtDesde.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtHasta.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtHasta.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblDesde
        //
        _lblDesde.Location = new Point(10, 14);
        _lblDesde.Name = "_lblDesde";
        _lblDesde.Size = new Size(33, 13);
        _lblDesde.Text = "Desde:";
        //
        // _dtDesde
        //
        _dtDesde.EditValue = null;
        _dtDesde.Location = new Point(55, 11);
        _dtDesde.Name = "_dtDesde";
        _dtDesde.Size = new Size(120, 20);
        //
        // _lblHasta
        //
        _lblHasta.Location = new Point(185, 14);
        _lblHasta.Name = "_lblHasta";
        _lblHasta.Size = new Size(30, 13);
        _lblHasta.Text = "Hasta:";
        //
        // _dtHasta
        //
        _dtHasta.EditValue = null;
        _dtHasta.Location = new Point(225, 11);
        _dtHasta.Name = "_dtHasta";
        _dtHasta.Size = new Size(120, 20);
        //
        // _btnActualizar
        //
        _btnActualizar.Location = new Point(355, 10);
        _btnActualizar.Name = "_btnActualizar";
        _btnActualizar.Size = new Size(90, 23);
        _btnActualizar.Text = "Actualizar";
        _btnActualizar.Click += BtnActualizar_Click;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 40);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(1080, 460);
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _btnEditar
        //
        _btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditar.Location = new Point(12, 515);
        _btnEditar.Name = "_btnEditar";
        _btnEditar.Size = new Size(90, 23);
        _btnEditar.Text = "Editar";
        _btnEditar.Click += BtnEditar_Click;
        //
        // _btnGenerarPdf
        //
        _btnGenerarPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGenerarPdf.Location = new Point(108, 515);
        _btnGenerarPdf.Name = "_btnGenerarPdf";
        _btnGenerarPdf.Size = new Size(140, 23);
        _btnGenerarPdf.Text = "Generar PDF";
        _btnGenerarPdf.Click += BtnGenerarPdf_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(1000, 515);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // IncidenciasForm
        //
        ClientSize = new Size(1100, 550);
        Controls.Add(_lblDesde);
        Controls.Add(_dtDesde);
        Controls.Add(_lblHasta);
        Controls.Add(_dtHasta);
        Controls.Add(_btnActualizar);
        Controls.Add(_grid);
        Controls.Add(_btnEditar);
        Controls.Add(_btnGenerarPdf);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(800, 400);
        Name = "IncidenciasForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Incidencias";
        ((System.ComponentModel.ISupportInitialize)_dtDesde.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtDesde.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtHasta.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtHasta.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
