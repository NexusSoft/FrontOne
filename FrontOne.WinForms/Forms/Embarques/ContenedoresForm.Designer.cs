using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Embarques;

partial class ContenedoresForm
{
    private IContainer components = null;

    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnNuevo;
    private SimpleButton _btnEditar;
    private SimpleButton _btnEliminar;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContenedoresForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _btnNuevo = new SimpleButton();
        _btnEditar = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();

        // _grid
        _grid.Location = new Point(12, 12);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(860, 430);
        _grid.TabIndex = 0;
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });

        // _gridView
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.DoubleClick += GridView_DoubleClick;

        // _btnNuevo
        _btnNuevo.Location = new Point(12, 450);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.Size = new Size(100, 28);
        _btnNuevo.TabIndex = 1;
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Click += BtnNuevo_Click;

        // _btnEditar
        _btnEditar.Location = new Point(118, 450);
        _btnEditar.Name = "_btnEditar";
        _btnEditar.Size = new Size(100, 28);
        _btnEditar.TabIndex = 2;
        _btnEditar.Text = "Editar";
        _btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditar.Click += BtnEditar_Click;

        // _btnEliminar
        _btnEliminar.Location = new Point(224, 450);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.Size = new Size(100, 28);
        _btnEliminar.TabIndex = 3;
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Click += BtnEliminar_Click;

        // _btnCerrar
        _btnCerrar.Location = new Point(772, 450);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(100, 28);
        _btnCerrar.TabIndex = 4;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Click += BtnCerrar_Click;

        // ContenedoresForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 498);
        Controls.Add(_grid);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnEditar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCerrar);
        Name = "ContenedoresForm";
        Text = "Contenedores";
        StartPosition = FormStartPosition.CenterParent;
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }
}
