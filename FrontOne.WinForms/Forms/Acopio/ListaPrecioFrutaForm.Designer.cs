using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Acopio;

partial class ListaPrecioFrutaForm
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

    private CheckEdit _chkPreciosPorRango;
    private LabelControl _lblFechaInicio;
    private DateEdit _dtFechaInicio;
    private LabelControl _lblFechaFin;
    private DateEdit _dtFechaFin;
    private SimpleButton _btnCargarCombinaciones;
    private SimpleButton _btnBuscarListaPrecios;
    private LabelControl _lblProductor;
    private ButtonEdit _cmbProductor;
    private GridControl _grid;
    private GridView _gridView;
    private SimpleButton _btnGuardar;
    private SimpleButton _btnEliminar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaPrecioFrutaForm));
        _chkPreciosPorRango = new CheckEdit();
        _lblFechaInicio = new LabelControl();
        _dtFechaInicio = new DateEdit();
        _lblFechaFin = new LabelControl();
        _dtFechaFin = new DateEdit();
        _btnCargarCombinaciones = new SimpleButton();
        _btnBuscarListaPrecios = new SimpleButton();
        _lblProductor = new LabelControl();
        _cmbProductor = new ButtonEdit();
        _grid = new GridControl();
        _gridView = new GridView();
        _btnGuardar = new SimpleButton();
        _btnEliminar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_chkPreciosPorRango.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProductor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _chkPreciosPorRango
        //
        _chkPreciosPorRango.Location = new Point(12, 12);
        _chkPreciosPorRango.Name = "_chkPreciosPorRango";
        _chkPreciosPorRango.Properties.Caption = "Precios por rango";
        _chkPreciosPorRango.Size = new Size(150, 20);
        _chkPreciosPorRango.TabIndex = 0;
        _chkPreciosPorRango.CheckedChanged += ChkPreciosPorRango_CheckedChanged;
        //
        // _lblFechaInicio
        //
        _lblFechaInicio.Location = new Point(12, 40);
        _lblFechaInicio.Name = "_lblFechaInicio";
        _lblFechaInicio.Size = new Size(63, 13);
        _lblFechaInicio.TabIndex = 1;
        _lblFechaInicio.Text = "Fecha inicio:";
        //
        // _dtFechaInicio
        //
        _dtFechaInicio.EditValue = null;
        _dtFechaInicio.Location = new Point(90, 37);
        _dtFechaInicio.Name = "_dtFechaInicio";
        _dtFechaInicio.Size = new Size(120, 20);
        _dtFechaInicio.TabIndex = 2;
        //
        // _lblFechaFin
        //
        _lblFechaFin.Location = new Point(230, 40);
        _lblFechaFin.Name = "_lblFechaFin";
        _lblFechaFin.Size = new Size(52, 13);
        _lblFechaFin.TabIndex = 3;
        _lblFechaFin.Text = "Fecha fin:";
        //
        // _dtFechaFin
        //
        _dtFechaFin.EditValue = null;
        _dtFechaFin.Enabled = false;
        _dtFechaFin.Location = new Point(290, 37);
        _dtFechaFin.Name = "_dtFechaFin";
        _dtFechaFin.Size = new Size(120, 20);
        _dtFechaFin.TabIndex = 4;
        //
        // _btnCargarCombinaciones
        //
        _btnCargarCombinaciones.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnCargarCombinaciones.Location = new Point(628, 36);
        _btnCargarCombinaciones.Name = "_btnCargarCombinaciones";
        _btnCargarCombinaciones.Size = new Size(160, 23);
        _btnCargarCombinaciones.TabIndex = 5;
        _btnCargarCombinaciones.Text = "Cargar Combinaciones";
        _btnCargarCombinaciones.Click += BtnCargarCombinaciones_Click;
        //
        // _btnBuscarListaPrecios
        //
        _btnBuscarListaPrecios.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnBuscarListaPrecios.Location = new Point(548, 8);
        _btnBuscarListaPrecios.Name = "_btnBuscarListaPrecios";
        _btnBuscarListaPrecios.Size = new Size(240, 23);
        _btnBuscarListaPrecios.TabIndex = 9;
        _btnBuscarListaPrecios.Text = "Buscar Lista de Precios";
        _btnBuscarListaPrecios.Click += BtnBuscarListaPrecios_Click;
        //
        // _lblProductor
        //
        _lblProductor.Location = new Point(12, 68);
        _lblProductor.Name = "_lblProductor";
        _lblProductor.Size = new Size(50, 13);
        _lblProductor.TabIndex = 11;
        _lblProductor.Text = "Productor:";
        //
        // _cmbProductor
        //
        _cmbProductor.Location = new Point(90, 65);
        _cmbProductor.Name = "_cmbProductor";
        _cmbProductor.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Search), new EditorButton(ButtonPredefines.Delete) });
        _cmbProductor.Properties.NullValuePrompt = "Buscar productor (opcional)...";
        _cmbProductor.Properties.ReadOnly = true;
        _cmbProductor.Size = new Size(320, 20);
        _cmbProductor.TabIndex = 12;
        _cmbProductor.ButtonClick += CmbProductor_ButtonClick;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(12, 124);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(776, 314);
        _grid.TabIndex = 6;
        _grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridView });
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.Name = "_gridView";
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        //
        // _btnGuardar
        //
        _btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Location = new Point(12, 474);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(80, 23);
        _btnGuardar.TabIndex = 7;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.Click += BtnGuardar_Click;
        //
        // _btnEliminar
        //
        _btnEliminar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnEliminar.ImageOptions.Image = (Image)resources.GetObject("_btnEliminar.ImageOptions.Image");
        _btnEliminar.Location = new Point(98, 474);
        _btnEliminar.Name = "_btnEliminar";
        _btnEliminar.Size = new Size(90, 23);
        _btnEliminar.TabIndex = 10;
        _btnEliminar.Text = "Eliminar";
        _btnEliminar.Click += BtnEliminar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(708, 474);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(80, 23);
        _btnCerrar.TabIndex = 8;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // ListaPrecioFrutaForm
        //
        ClientSize = new Size(800, 509);
        Controls.Add(_chkPreciosPorRango);
        Controls.Add(_lblFechaInicio);
        Controls.Add(_dtFechaInicio);
        Controls.Add(_lblFechaFin);
        Controls.Add(_dtFechaFin);
        Controls.Add(_btnCargarCombinaciones);
        Controls.Add(_btnBuscarListaPrecios);
        Controls.Add(_lblProductor);
        Controls.Add(_cmbProductor);
        Controls.Add(_grid);
        Controls.Add(_btnGuardar);
        Controls.Add(_btnEliminar);
        Controls.Add(_btnCerrar);
        MinimumSize = new Size(700, 406);
        Name = "ListaPrecioFrutaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Lista de Precio Fruta";
        ((System.ComponentModel.ISupportInitialize)_chkPreciosPorRango.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaInicio.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFechaFin.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_cmbProductor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
