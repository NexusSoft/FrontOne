using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Embarques;

partial class ContenedorPalletAgregarForm
{
    private IContainer components = null;

    private LabelControl _lblFiltro;
    private TextEdit _txtFiltro;
    private SimpleButton _btnBuscar;
    private GridControl _grid;
    private GridView _gridView;

    private LabelControl _lblPosicion;
    private SpinEdit _spnPosicion;
    private LabelControl _lblTemperatura;
    private SpinEdit _spnTemperatura;

    private SimpleButton _btnGuardar;
    private SimpleButton _btnCancelar;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContenedorPalletAgregarForm));
        _lblFiltro = new LabelControl();
        _txtFiltro = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView();
        _lblPosicion = new LabelControl();
        _spnPosicion = new SpinEdit();
        _lblTemperatura = new LabelControl();
        _spnTemperatura = new SpinEdit();
        _btnGuardar = new SimpleButton();
        _btnCancelar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtFiltro.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnPosicion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_spnTemperatura.Properties).BeginInit();
        SuspendLayout();

        _lblFiltro.Location = new Point(12, 15);
        _lblFiltro.Name = "_lblFiltro";
        _lblFiltro.Size = new Size(70, 13);
        _lblFiltro.Text = "No. de Pallet:";

        _txtFiltro.Location = new Point(90, 12);
        _txtFiltro.Name = "_txtFiltro";
        _txtFiltro.Size = new Size(150, 20);
        _txtFiltro.TabIndex = 0;
        _txtFiltro.KeyDown += TxtFiltro_KeyDown;

        _btnBuscar.Location = new Point(250, 11);
        _btnBuscar.Name = "_btnBuscar";
        _btnBuscar.Size = new Size(90, 28);
        _btnBuscar.TabIndex = 1;
        _btnBuscar.Text = "Buscar";
        _btnBuscar.Click += BtnBuscar_Click;

        _grid.Location = new Point(12, 45);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(650, 320);
        _grid.TabIndex = 2;
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });

        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.OptionsView.ShowGroupPanel = false;

        _lblPosicion.Location = new Point(12, 380);
        _lblPosicion.Name = "_lblPosicion";
        _lblPosicion.Size = new Size(90, 13);
        _lblPosicion.Text = "Posición:";

        _spnPosicion.EditValue = 1;
        _spnPosicion.Location = new Point(110, 377);
        _spnPosicion.Name = "_spnPosicion";
        _spnPosicion.Properties.IsFloatValue = false;
        _spnPosicion.Properties.Mask.EditMask = "N00";
        _spnPosicion.Properties.MaxValue = 9999;
        _spnPosicion.Properties.MinValue = 1;
        _spnPosicion.Size = new Size(100, 20);
        _spnPosicion.TabIndex = 3;

        _lblTemperatura.Location = new Point(250, 380);
        _lblTemperatura.Name = "_lblTemperatura";
        _lblTemperatura.Size = new Size(90, 13);
        _lblTemperatura.Text = "Temperatura (°F):";

        _spnTemperatura.EditValue = null;
        _spnTemperatura.Location = new Point(360, 377);
        _spnTemperatura.Name = "_spnTemperatura";
        _spnTemperatura.Properties.IsFloatValue = true;
        _spnTemperatura.Properties.Mask.EditMask = "n2";
        _spnTemperatura.Properties.MaxValue = 140;
        _spnTemperatura.Properties.MinValue = -80;
        _spnTemperatura.Size = new Size(100, 20);
        _spnTemperatura.TabIndex = 4;

        _btnGuardar.Location = new Point(482, 412);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 28);
        _btnGuardar.TabIndex = 5;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Click += BtnGuardar_Click;

        _btnCancelar.Location = new Point(572, 412);
        _btnCancelar.Name = "_btnCancelar";
        _btnCancelar.Size = new Size(80, 28);
        _btnCancelar.TabIndex = 6;
        _btnCancelar.Text = "Cancelar";
        _btnCancelar.ImageOptions.Image = (Image)resources.GetObject("_btnCancelar.ImageOptions.Image");
        _btnCancelar.Click += BtnCancelar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(674, 446);
        Controls.Add(_lblFiltro);
        Controls.Add(_txtFiltro);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_lblPosicion);
        Controls.Add(_spnPosicion);
        Controls.Add(_lblTemperatura);
        Controls.Add(_spnTemperatura);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnCancelar);
        Name = "ContenedorPalletAgregarForm";
        Text = "Agregar Pallet al Contenedor";
        StartPosition = FormStartPosition.CenterParent;
        ((System.ComponentModel.ISupportInitialize)_txtFiltro.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnPosicion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_spnTemperatura.Properties).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
