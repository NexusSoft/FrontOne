using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Etiquetado;

partial class EtiquetasForm
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
    private SimpleButton _btnDuplicar;
    private SimpleButton _btnEditarFormato;
    private SimpleButton _btnVistaPrevia;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnCerrar;
    private SimpleButton _btnRecuperarEliminadas;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EtiquetasForm));
        _grid = new GridControl();
        _gridView = new GridView();
        _btnNuevo = new SimpleButton();
        _btnDuplicar = new SimpleButton();
        _btnEditarFormato = new SimpleButton();
        _btnVistaPrevia = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        _btnRecuperarEliminadas = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 40);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(620, 360);
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
        // _btnRecuperarEliminadas
        //
        _btnRecuperarEliminadas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnRecuperarEliminadas.Location = new Point(460, 10);
        _btnRecuperarEliminadas.Name = "_btnRecuperarEliminadas";
        _btnRecuperarEliminadas.Size = new Size(170, 23);
        _btnRecuperarEliminadas.TabIndex = 1;
        _btnRecuperarEliminadas.Text = "Recuperar eliminadas...";
        _btnRecuperarEliminadas.Click += BtnRecuperarEliminadas_Click;
        //
        // _btnNuevo
        //
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Location = new Point(12, 410);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.TabIndex = 2;
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        //
        // _btnDuplicar
        //
        _btnDuplicar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnDuplicar.Location = new Point(108, 410);
        _btnDuplicar.Name = "_btnDuplicar";
        _btnDuplicar.Size = new Size(90, 23);
        _btnDuplicar.TabIndex = 3;
        _btnDuplicar.Text = "Duplicar";
        _btnDuplicar.Click += BtnDuplicar_Click;
        //
        // _btnEditarFormato
        //
        _btnEditarFormato.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditarFormato.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditarFormato.Location = new Point(204, 410);
        _btnEditarFormato.Name = "_btnEditarFormato";
        _btnEditarFormato.Size = new Size(110, 23);
        _btnEditarFormato.TabIndex = 4;
        _btnEditarFormato.Text = "Editar formato";
        _btnEditarFormato.Click += BtnEditarFormato_Click;
        //
        // _btnVistaPrevia
        //
        _btnVistaPrevia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnVistaPrevia.Location = new Point(320, 410);
        _btnVistaPrevia.Name = "_btnVistaPrevia";
        _btnVistaPrevia.Size = new Size(100, 23);
        _btnVistaPrevia.TabIndex = 5;
        _btnVistaPrevia.Text = "Vista previa";
        _btnVistaPrevia.Click += BtnVistaPrevia_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Location = new Point(426, 410);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.TabIndex = 6;
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(540, 410);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 7;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // EtiquetasForm
        //
        ClientSize = new Size(640, 460);
        Controls.Add(_grid);
        Controls.Add(_btnRecuperarEliminadas);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnDuplicar);
        Controls.Add(_btnEditarFormato);
        Controls.Add(_btnVistaPrevia);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EtiquetasForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Etiquetas";
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
