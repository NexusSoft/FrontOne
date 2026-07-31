using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Lotes;

partial class SeleccionarRecepcionForm
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

    private LabelControl _lblBuscar;
    private TextEdit _txtBuscar;
    private SimpleButton _btnBuscar;
    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colFolio;
    private GridColumn _colTicket;
    private GridColumn _colFecha;
    private GridColumn _colHuerta;
    private GridColumn _colPesoNeto;
    private SimpleButton _btnSeleccionar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeleccionarRecepcionForm));
        _lblBuscar = new LabelControl();
        _txtBuscar = new TextEdit();
        _btnBuscar = new SimpleButton();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colFolio = new GridColumn();
        _colTicket = new GridColumn();
        _colFecha = new GridColumn();
        _colHuerta = new GridColumn();
        _colPesoNeto = new GridColumn();
        _btnSeleccionar = new SimpleButton();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_txtBuscar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _lblBuscar
        //
        _lblBuscar.Location = new Point(10, 15);
        _lblBuscar.Name = "_lblBuscar";
        _lblBuscar.Size = new Size(36, 13);
        _lblBuscar.Text = "Buscar:";
        //
        // _txtBuscar
        //
        _txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _txtBuscar.Location = new Point(60, 12);
        _txtBuscar.Name = "_txtBuscar";
        _txtBuscar.Properties.NullValuePrompt = "Folio, ticket o huerta de la recepción";
        _txtBuscar.Size = new Size(454, 20);
        _txtBuscar.TabIndex = 0;
        _txtBuscar.KeyDown += TxtBuscar_KeyDown;
        //
        // _btnBuscar
        //
        _btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnBuscar.Location = new Point(520, 10);
        _btnBuscar.Name = "_btnBuscar";
        _btnBuscar.Size = new Size(90, 23);
        _btnBuscar.TabIndex = 1;
        _btnBuscar.Text = "Buscar";
        _btnBuscar.Click += BtnBuscar_Click;
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 40);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(600, 370);
        _grid.TabIndex = 2;
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsSelection.MultiSelect = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.Columns.AddRange(new GridColumn[] { _colFolio, _colTicket, _colFecha, _colHuerta, _colPesoNeto });
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _colFolio
        //
        _colFolio.Caption = "Folio";
        _colFolio.FieldName = "Folio";
        _colFolio.Name = "_colFolio";
        _colFolio.Visible = true;
        _colFolio.VisibleIndex = 0;
        _colFolio.Width = 90;
        //
        // _colTicket
        //
        _colTicket.Caption = "Ticket";
        _colTicket.FieldName = "NumeroTicket";
        _colTicket.Name = "_colTicket";
        _colTicket.Visible = true;
        _colTicket.VisibleIndex = 1;
        _colTicket.Width = 90;
        //
        // _colFecha
        //
        _colFecha.Caption = "Fecha";
        _colFecha.FieldName = "Fecha";
        _colFecha.Name = "_colFecha";
        _colFecha.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        _colFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
        _colFecha.Visible = true;
        _colFecha.VisibleIndex = 2;
        _colFecha.Width = 90;
        //
        // _colHuerta
        //
        _colHuerta.Caption = "Huerta";
        _colHuerta.FieldName = "HuertaNombre";
        _colHuerta.Name = "_colHuerta";
        _colHuerta.Visible = true;
        _colHuerta.VisibleIndex = 3;
        _colHuerta.Width = 260;
        //
        // _colPesoNeto
        //
        _colPesoNeto.Caption = "Peso Neto";
        _colPesoNeto.FieldName = "PesoNeto";
        _colPesoNeto.Name = "_colPesoNeto";
        _colPesoNeto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _colPesoNeto.DisplayFormat.FormatString = "n2";
        _colPesoNeto.Visible = true;
        _colPesoNeto.VisibleIndex = 4;
        _colPesoNeto.Width = 100;
        //
        // _btnSeleccionar
        //
        _btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnSeleccionar.Location = new Point(10, 420);
        _btnSeleccionar.Name = "_btnSeleccionar";
        _btnSeleccionar.Size = new Size(110, 23);
        _btnSeleccionar.TabIndex = 3;
        _btnSeleccionar.Text = "Seleccionar";
        _btnSeleccionar.Click += BtnSeleccionar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.Location = new Point(520, 420);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 4;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // SeleccionarRecepcionForm
        //
        ClientSize = new Size(620, 460);
        Controls.Add(_lblBuscar);
        Controls.Add(_txtBuscar);
        Controls.Add(_btnBuscar);
        Controls.Add(_grid);
        Controls.Add(_btnSeleccionar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SeleccionarRecepcionForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "FrontOne - Seleccionar Recepción";
        Load += SeleccionarRecepcionForm_Load;
        ((System.ComponentModel.ISupportInitialize)_txtBuscar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
