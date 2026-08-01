using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Catalogos;

partial class PoblacionesForm
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

    private LabelControl _lblFiltroPais;
    private LookUpEdit _cmbFiltroPais;
    private LabelControl _lblFiltroEstado;
    private LookUpEdit _cmbFiltroEstado;
    private LabelControl _lblFiltroMunicipio;
    private LookUpEdit _cmbFiltroMunicipio;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnNuevo;
    private SimpleButton _btnEditar;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PoblacionesForm));
        _lblFiltroPais = new LabelControl();
        _cmbFiltroPais = new LookUpEdit();
        _lblFiltroEstado = new LabelControl();
        _cmbFiltroEstado = new LookUpEdit();
        _lblFiltroMunicipio = new LabelControl();
        _cmbFiltroMunicipio = new LookUpEdit();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnNuevo = new SimpleButton();
        _btnEditar = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroPais.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroEstado.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroMunicipio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblFiltroPais
        //
        _lblFiltroPais.Location = new Point(10, 15);
        _lblFiltroPais.Name = "_lblFiltroPais";
        _lblFiltroPais.Size = new Size(19, 13);
        _lblFiltroPais.TabIndex = 0;
        _lblFiltroPais.Text = "País";
        //
        // _cmbFiltroPais
        //
        _cmbFiltroPais.Location = new Point(45, 12);
        _cmbFiltroPais.Name = "_cmbFiltroPais";
        _cmbFiltroPais.Properties.NullText = "Seleccionar";
        _cmbFiltroPais.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbFiltroPais.Size = new Size(170, 20);
        _cmbFiltroPais.TabIndex = 1;
        //
        // _lblFiltroEstado
        //
        _lblFiltroEstado.Location = new Point(230, 15);
        _lblFiltroEstado.Name = "_lblFiltroEstado";
        _lblFiltroEstado.Size = new Size(33, 13);
        _lblFiltroEstado.TabIndex = 2;
        _lblFiltroEstado.Text = "Estado";
        //
        // _cmbFiltroEstado
        //
        _cmbFiltroEstado.Location = new Point(275, 12);
        _cmbFiltroEstado.Name = "_cmbFiltroEstado";
        _cmbFiltroEstado.Properties.NullText = "Seleccionar";
        _cmbFiltroEstado.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbFiltroEstado.Size = new Size(170, 20);
        _cmbFiltroEstado.TabIndex = 3;
        //
        // _lblFiltroMunicipio
        //
        _lblFiltroMunicipio.Location = new Point(460, 15);
        _lblFiltroMunicipio.Name = "_lblFiltroMunicipio";
        _lblFiltroMunicipio.Size = new Size(52, 13);
        _lblFiltroMunicipio.TabIndex = 4;
        _lblFiltroMunicipio.Text = "Municipio";
        //
        // _cmbFiltroMunicipio
        //
        _cmbFiltroMunicipio.Location = new Point(520, 12);
        _cmbFiltroMunicipio.Name = "_cmbFiltroMunicipio";
        _cmbFiltroMunicipio.Properties.NullText = "Seleccionar";
        _cmbFiltroMunicipio.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _cmbFiltroMunicipio.Size = new Size(170, 20);
        _cmbFiltroMunicipio.TabIndex = 5;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 45);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(680, 396);
        _grid.TabIndex = 6;
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
        // _btnNuevo
        //
        _btnNuevo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevo.Location = new Point(10, 447);
        _btnNuevo.Name = "_btnNuevo";
        _btnNuevo.ImageOptions.Image = (Image)resources.GetObject("_btnNuevo.ImageOptions.Image");
        _btnNuevo.Size = new Size(90, 23);
        _btnNuevo.TabIndex = 7;
        _btnNuevo.Text = "Nuevo";
        _btnNuevo.Click += BtnNuevo_Click;
        //
        // _btnEditar
        //
        _btnEditar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEditar.Location = new Point(106, 447);
        _btnEditar.Name = "_btnEditar";
        _btnEditar.ImageOptions.Image = (Image)resources.GetObject("_btnEditar.ImageOptions.Image");
        _btnEditar.Size = new Size(90, 23);
        _btnEditar.TabIndex = 8;
        _btnEditar.Text = "Editar";
        _btnEditar.Click += BtnEditar_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.Location = new Point(202, 447);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.TabIndex = 9;
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(600, 447);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 10;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // PoblacionesForm
        //
        ClientSize = new Size(702, 482);
        Controls.Add(_lblFiltroPais);
        Controls.Add(_cmbFiltroPais);
        Controls.Add(_lblFiltroEstado);
        Controls.Add(_cmbFiltroEstado);
        Controls.Add(_lblFiltroMunicipio);
        Controls.Add(_cmbFiltroMunicipio);
        Controls.Add(_grid);
        Controls.Add(_btnNuevo);
        Controls.Add(_btnEditar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PoblacionesForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Poblaciones";
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroPais.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroEstado.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbFiltroMunicipio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
