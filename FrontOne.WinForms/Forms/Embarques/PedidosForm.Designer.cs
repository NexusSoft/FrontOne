using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Embarques;

partial class PedidosForm
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
    private GridColumn _colDocNum;
    private GridColumn _colCardCode;
    private GridColumn _colCardName;
    private GridColumn _colDocDate;
    private GridColumn _colDocDueDate;
    private GridColumn _colDocCurrency;
    private GridColumn _colDocTotal;
    private GridColumn _colEstatus;
    private GridColumn _colFolioFronterra;
    private SimpleButton _btnActualizar;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PedidosForm));
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colDocNum = new GridColumn();
        _colCardCode = new GridColumn();
        _colCardName = new GridColumn();
        _colDocDate = new GridColumn();
        _colDocDueDate = new GridColumn();
        _colDocCurrency = new GridColumn();
        _colDocTotal = new GridColumn();
        _colEstatus = new GridColumn();
        _colFolioFronterra = new GridColumn();
        _btnActualizar = new SimpleButton();
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
        _grid.Size = new Size(900, 460);
        _grid.TabIndex = 0;
        //
        // _gridView
        //
        _gridView.GridControl = _grid;
        _gridView.OptionsBehavior.Editable = false;
        _gridView.OptionsFind.AlwaysVisible = true;
        _gridView.OptionsSelection.MultiSelect = false;
        _gridView.OptionsView.ShowGroupPanel = false;
        _gridView.OptionsView.ColumnAutoWidth = false;
        _gridView.Columns.AddRange(new GridColumn[] { _colDocNum, _colFolioFronterra, _colCardCode, _colCardName, _colDocDate, _colDocDueDate, _colDocCurrency, _colDocTotal, _colEstatus });
        _gridView.DoubleClick += GridView_DoubleClick;
        //
        // _colDocNum
        //
        _colDocNum.Caption = "No. Pedido";
        _colDocNum.FieldName = "DocNum";
        _colDocNum.Name = "_colDocNum";
        _colDocNum.Visible = true;
        _colDocNum.VisibleIndex = 0;
        _colDocNum.Width = 80;
        //
        // _colFolioFronterra
        //
        _colFolioFronterra.Caption = "Folio Fronterra";
        _colFolioFronterra.FieldName = "FolioFronterra";
        _colFolioFronterra.Name = "_colFolioFronterra";
        _colFolioFronterra.Visible = true;
        _colFolioFronterra.VisibleIndex = 1;
        _colFolioFronterra.Width = 100;
        //
        // _colCardCode
        //
        _colCardCode.Caption = "Código Cliente";
        _colCardCode.FieldName = "CardCode";
        _colCardCode.Name = "_colCardCode";
        _colCardCode.Visible = true;
        _colCardCode.VisibleIndex = 2;
        _colCardCode.Width = 90;
        //
        // _colCardName
        //
        _colCardName.Caption = "Cliente";
        _colCardName.FieldName = "CardName";
        _colCardName.Name = "_colCardName";
        _colCardName.Visible = true;
        _colCardName.VisibleIndex = 3;
        _colCardName.Width = 260;
        //
        // _colDocDate
        //
        _colDocDate.Caption = "Fecha";
        _colDocDate.DisplayFormat.FormatString = "dd/MM/yyyy";
        _colDocDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        _colDocDate.FieldName = "DocDate";
        _colDocDate.Name = "_colDocDate";
        _colDocDate.Visible = true;
        _colDocDate.VisibleIndex = 4;
        _colDocDate.Width = 85;
        //
        // _colDocDueDate
        //
        _colDocDueDate.Caption = "Fecha de Entrega";
        _colDocDueDate.DisplayFormat.FormatString = "dd/MM/yyyy";
        _colDocDueDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        _colDocDueDate.FieldName = "DocDueDate";
        _colDocDueDate.Name = "_colDocDueDate";
        _colDocDueDate.Visible = true;
        _colDocDueDate.VisibleIndex = 5;
        _colDocDueDate.Width = 90;
        //
        // _colDocCurrency
        //
        _colDocCurrency.Caption = "Moneda";
        _colDocCurrency.FieldName = "DocCurrency";
        _colDocCurrency.Name = "_colDocCurrency";
        _colDocCurrency.Visible = true;
        _colDocCurrency.VisibleIndex = 6;
        _colDocCurrency.Width = 60;
        //
        // _colDocTotal
        //
        _colDocTotal.Caption = "Total";
        _colDocTotal.DisplayFormat.FormatString = "N2";
        _colDocTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _colDocTotal.FieldName = "DocTotal";
        _colDocTotal.Name = "_colDocTotal";
        _colDocTotal.Visible = true;
        _colDocTotal.VisibleIndex = 7;
        _colDocTotal.Width = 100;
        //
        // _colEstatus
        //
        _colEstatus.Caption = "Estatus";
        _colEstatus.FieldName = "Estatus";
        _colEstatus.Name = "_colEstatus";
        _colEstatus.Visible = true;
        _colEstatus.VisibleIndex = 8;
        _colEstatus.Width = 80;
        //
        // _btnActualizar
        //
        _btnActualizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnActualizar.Location = new Point(10, 480);
        _btnActualizar.Name = "_btnActualizar";
        _btnActualizar.Size = new Size(100, 23);
        _btnActualizar.TabIndex = 1;
        _btnActualizar.Text = "Actualizar";
        _btnActualizar.Click += BtnActualizar_Click;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(820, 480);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 2;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // PedidosForm
        //
        ClientSize = new Size(920, 513);
        Controls.Add(_grid);
        Controls.Add(_btnActualizar);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PedidosForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Pedidos";
        Shown += PedidosForm_Load;
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
