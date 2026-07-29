using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Recepcion;

partial class RecepcionesFrutaForm
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
    private SimpleButton _btnNuevo;
    private SimpleButton _btnEditar;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnVistaPrevia;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecepcionesFrutaForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _btnNuevo = new SimpleButton();
        _btnEditar = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnVistaPrevia = new SimpleButton();
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
        _grid.Size = new Size(1000, 460);
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
        // _btnNuevo
        //
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.Location = new Point(12, 485);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        //
        // _btnEditar
        //
        _btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditar.Location = new Point(108, 485);
        _btnEditar.Name = "_btnEditar";
        _btnEditar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditar.Size = new Size(90, 23);
        _btnEditar.Text = "Editar";
        _btnEditar.Click += BtnEditar_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.Location = new Point(204, 485);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnVistaPrevia
        //
        _btnVistaPrevia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnVistaPrevia.Location = new Point(300, 485);
        _btnVistaPrevia.Name = "_btnVistaPrevia";
        _btnVistaPrevia.Size = new Size(100, 23);
        _btnVistaPrevia.Text = "Vista Previa";
        _btnVistaPrevia.Click += BtnVistaPrevia_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(920, 485);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // RecepcionesFrutaForm
        //
        ClientSize = new Size(1020, 520);
        Controls.Add(_grid);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnEditar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnVistaPrevia);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(800, 400);
        Name = "RecepcionesFrutaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Recepciones de Fruta";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
