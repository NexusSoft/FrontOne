using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace FrontOne.WinForms.Forms.Embarques;

partial class PedidoDetalleForm
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

    private GroupControl _grpEncabezado;
    private LabelControl _lblDocNum;
    private TextEdit _txtDocNum;
    private LabelControl _lblEstatus;
    private TextEdit _txtEstatus;
    private LabelControl _lblCardCode;
    private TextEdit _txtCardCode;
    private LabelControl _lblCardName;
    private TextEdit _txtCardName;
    private LabelControl _lblNumAtCard;
    private TextEdit _txtNumAtCard;
    private LabelControl _lblDocCurrency;
    private TextEdit _txtDocCurrency;
    private LabelControl _lblDocDate;
    private TextEdit _txtDocDate;
    private LabelControl _lblDocDueDate;
    private TextEdit _txtDocDueDate;
    private LabelControl _lblTaxDate;
    private TextEdit _txtTaxDate;
    private LabelControl _lblDocRate;
    private TextEdit _txtDocRate;
    private LabelControl _lblDiscountPercent;
    private TextEdit _txtDiscountPercent;
    private LabelControl _lblVatSum;
    private TextEdit _txtVatSum;
    private LabelControl _lblDocTotal;
    private TextEdit _txtDocTotal;
    private LabelControl _lblVendedor;
    private TextEdit _txtVendedor;
    private LabelControl _lblFolioFronterra;
    private TextEdit _txtFolioFronterra;
    private LabelControl _lblDireccion;
    private TextEdit _txtDireccion;
    private LabelControl _lblComentarios;
    private MemoEdit _memComentarios;
    private GroupControl _grpDetalle;
    private GridControl _grid;
    private GridView _gridView;
    private GridColumn _colCodigo;
    private GridColumn _colDescripcion;
    private GridColumn _colCantidad;
    private GridColumn _colPrecioUnitario;
    private GridColumn _colTotal;
    private GridColumn _colAlmacen;
    private SimpleButton _btnCerrar;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PedidoDetalleForm));
        _grpEncabezado = new GroupControl();
        _lblDocNum = new LabelControl();
        _txtDocNum = new TextEdit();
        _lblEstatus = new LabelControl();
        _txtEstatus = new TextEdit();
        _lblCardCode = new LabelControl();
        _txtCardCode = new TextEdit();
        _lblCardName = new LabelControl();
        _txtCardName = new TextEdit();
        _lblNumAtCard = new LabelControl();
        _txtNumAtCard = new TextEdit();
        _lblDocCurrency = new LabelControl();
        _txtDocCurrency = new TextEdit();
        _lblDocDate = new LabelControl();
        _txtDocDate = new TextEdit();
        _lblDocDueDate = new LabelControl();
        _txtDocDueDate = new TextEdit();
        _lblTaxDate = new LabelControl();
        _txtTaxDate = new TextEdit();
        _lblDocRate = new LabelControl();
        _txtDocRate = new TextEdit();
        _lblDiscountPercent = new LabelControl();
        _txtDiscountPercent = new TextEdit();
        _lblVatSum = new LabelControl();
        _txtVatSum = new TextEdit();
        _lblDocTotal = new LabelControl();
        _txtDocTotal = new TextEdit();
        _lblVendedor = new LabelControl();
        _txtVendedor = new TextEdit();
        _lblFolioFronterra = new LabelControl();
        _txtFolioFronterra = new TextEdit();
        _lblDireccion = new LabelControl();
        _txtDireccion = new TextEdit();
        _lblComentarios = new LabelControl();
        _memComentarios = new MemoEdit();
        _grpDetalle = new GroupControl();
        _grid = new GridControl();
        _gridView = new GridView(_grid);
        _colCodigo = new GridColumn();
        _colDescripcion = new GridColumn();
        _colCantidad = new GridColumn();
        _colPrecioUnitario = new GridColumn();
        _colTotal = new GridColumn();
        _colAlmacen = new GridColumn();
        _btnCerrar = new SimpleButton();
        ((System.ComponentModel.ISupportInitialize)_grpEncabezado).BeginInit();
        _grpEncabezado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_txtDocNum.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEstatus.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCardCode.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCardName.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumAtCard.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocCurrency.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocDate.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocDueDate.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtTaxDate.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocRate.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDiscountPercent.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtVatSum.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocTotal.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtVendedor.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFolioFronterra.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDireccion.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_memComentarios.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpDetalle).BeginInit();
        _grpDetalle.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).BeginInit();
        SuspendLayout();
        //
        // _grpEncabezado
        //
        _grpEncabezado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _grpEncabezado.Controls.Add(_lblDocNum);
        _grpEncabezado.Controls.Add(_txtDocNum);
        _grpEncabezado.Controls.Add(_lblEstatus);
        _grpEncabezado.Controls.Add(_txtEstatus);
        _grpEncabezado.Controls.Add(_lblCardCode);
        _grpEncabezado.Controls.Add(_txtCardCode);
        _grpEncabezado.Controls.Add(_lblCardName);
        _grpEncabezado.Controls.Add(_txtCardName);
        _grpEncabezado.Controls.Add(_lblNumAtCard);
        _grpEncabezado.Controls.Add(_txtNumAtCard);
        _grpEncabezado.Controls.Add(_lblDocCurrency);
        _grpEncabezado.Controls.Add(_txtDocCurrency);
        _grpEncabezado.Controls.Add(_lblDocDate);
        _grpEncabezado.Controls.Add(_txtDocDate);
        _grpEncabezado.Controls.Add(_lblDocDueDate);
        _grpEncabezado.Controls.Add(_txtDocDueDate);
        _grpEncabezado.Controls.Add(_lblTaxDate);
        _grpEncabezado.Controls.Add(_txtTaxDate);
        _grpEncabezado.Controls.Add(_lblDocRate);
        _grpEncabezado.Controls.Add(_txtDocRate);
        _grpEncabezado.Controls.Add(_lblDiscountPercent);
        _grpEncabezado.Controls.Add(_txtDiscountPercent);
        _grpEncabezado.Controls.Add(_lblVatSum);
        _grpEncabezado.Controls.Add(_txtVatSum);
        _grpEncabezado.Controls.Add(_lblDocTotal);
        _grpEncabezado.Controls.Add(_txtDocTotal);
        _grpEncabezado.Controls.Add(_lblVendedor);
        _grpEncabezado.Controls.Add(_txtVendedor);
        _grpEncabezado.Controls.Add(_lblFolioFronterra);
        _grpEncabezado.Controls.Add(_txtFolioFronterra);
        _grpEncabezado.Controls.Add(_lblDireccion);
        _grpEncabezado.Controls.Add(_txtDireccion);
        _grpEncabezado.Controls.Add(_lblComentarios);
        _grpEncabezado.Controls.Add(_memComentarios);
        _grpEncabezado.Location = new Point(10, 10);
        _grpEncabezado.Name = "_grpEncabezado";
        _grpEncabezado.Size = new Size(900, 347);
        _grpEncabezado.TabIndex = 0;
        _grpEncabezado.Text = "Pedido de Venta";
        //
        // _lblDocNum
        //
        _lblDocNum.Location = new Point(15, 30);
        _lblDocNum.Name = "_lblDocNum";
        _lblDocNum.Size = new Size(66, 13);
        _lblDocNum.Text = "No. Pedido:";
        //
        // _txtDocNum
        //
        _txtDocNum.Location = new Point(140, 27);
        _txtDocNum.Name = "_txtDocNum";
        _txtDocNum.Properties.ReadOnly = true;
        _txtDocNum.Size = new Size(160, 20);
        _txtDocNum.TabIndex = 0;
        //
        // _lblEstatus
        //
        _lblEstatus.Location = new Point(350, 30);
        _lblEstatus.Name = "_lblEstatus";
        _lblEstatus.Size = new Size(40, 13);
        _lblEstatus.Text = "Estatus:";
        //
        // _txtEstatus
        //
        _txtEstatus.Location = new Point(465, 27);
        _txtEstatus.Name = "_txtEstatus";
        _txtEstatus.Properties.ReadOnly = true;
        _txtEstatus.Size = new Size(160, 20);
        _txtEstatus.TabIndex = 1;
        //
        // _lblCardCode
        //
        _lblCardCode.Location = new Point(15, 57);
        _lblCardCode.Name = "_lblCardCode";
        _lblCardCode.Size = new Size(80, 13);
        _lblCardCode.Text = "Código Cliente:";
        //
        // _txtCardCode
        //
        _txtCardCode.Location = new Point(140, 54);
        _txtCardCode.Name = "_txtCardCode";
        _txtCardCode.Properties.ReadOnly = true;
        _txtCardCode.Size = new Size(160, 20);
        _txtCardCode.TabIndex = 2;
        //
        // _lblCardName
        //
        _lblCardName.Location = new Point(350, 57);
        _lblCardName.Name = "_lblCardName";
        _lblCardName.Size = new Size(76, 13);
        _lblCardName.Text = "Nombre Cliente:";
        //
        // _txtCardName
        //
        _txtCardName.Location = new Point(465, 54);
        _txtCardName.Name = "_txtCardName";
        _txtCardName.Properties.ReadOnly = true;
        _txtCardName.Size = new Size(420, 20);
        _txtCardName.TabIndex = 3;
        //
        // _lblNumAtCard
        //
        _lblNumAtCard.Location = new Point(15, 84);
        _lblNumAtCard.Name = "_lblNumAtCard";
        _lblNumAtCard.Size = new Size(103, 13);
        _lblNumAtCard.Text = "Referencia Cliente:";
        //
        // _txtNumAtCard
        //
        _txtNumAtCard.Location = new Point(140, 81);
        _txtNumAtCard.Name = "_txtNumAtCard";
        _txtNumAtCard.Properties.ReadOnly = true;
        _txtNumAtCard.Size = new Size(160, 20);
        _txtNumAtCard.TabIndex = 4;
        //
        // _lblDocCurrency
        //
        _lblDocCurrency.Location = new Point(350, 84);
        _lblDocCurrency.Name = "_lblDocCurrency";
        _lblDocCurrency.Size = new Size(43, 13);
        _lblDocCurrency.Text = "Moneda:";
        //
        // _txtDocCurrency
        //
        _txtDocCurrency.Location = new Point(465, 81);
        _txtDocCurrency.Name = "_txtDocCurrency";
        _txtDocCurrency.Properties.ReadOnly = true;
        _txtDocCurrency.Size = new Size(160, 20);
        _txtDocCurrency.TabIndex = 5;
        //
        // _lblDocDate
        //
        _lblDocDate.Location = new Point(15, 111);
        _lblDocDate.Name = "_lblDocDate";
        _lblDocDate.Size = new Size(94, 13);
        _lblDocDate.Text = "Fecha Documento:";
        //
        // _txtDocDate
        //
        _txtDocDate.Location = new Point(140, 108);
        _txtDocDate.Name = "_txtDocDate";
        _txtDocDate.Properties.ReadOnly = true;
        _txtDocDate.Size = new Size(160, 20);
        _txtDocDate.TabIndex = 6;
        //
        // _lblDocDueDate
        //
        _lblDocDueDate.Location = new Point(350, 111);
        _lblDocDueDate.Name = "_lblDocDueDate";
        _lblDocDueDate.Size = new Size(80, 13);
        _lblDocDueDate.Text = "Fecha Entrega:";
        //
        // _txtDocDueDate
        //
        _txtDocDueDate.Location = new Point(465, 108);
        _txtDocDueDate.Name = "_txtDocDueDate";
        _txtDocDueDate.Properties.ReadOnly = true;
        _txtDocDueDate.Size = new Size(160, 20);
        _txtDocDueDate.TabIndex = 7;
        //
        // _lblTaxDate
        //
        _lblTaxDate.Location = new Point(15, 138);
        _lblTaxDate.Name = "_lblTaxDate";
        _lblTaxDate.Size = new Size(114, 13);
        _lblTaxDate.Text = "Fecha Contabilización:";
        //
        // _txtTaxDate
        //
        _txtTaxDate.Location = new Point(140, 135);
        _txtTaxDate.Name = "_txtTaxDate";
        _txtTaxDate.Properties.ReadOnly = true;
        _txtTaxDate.Size = new Size(160, 20);
        _txtTaxDate.TabIndex = 8;
        //
        // _lblDocRate
        //
        _lblDocRate.Location = new Point(350, 138);
        _lblDocRate.Name = "_lblDocRate";
        _lblDocRate.Size = new Size(74, 13);
        _lblDocRate.Text = "Tipo de Cambio:";
        //
        // _txtDocRate
        //
        _txtDocRate.Location = new Point(465, 135);
        _txtDocRate.Name = "_txtDocRate";
        _txtDocRate.Properties.ReadOnly = true;
        _txtDocRate.Size = new Size(160, 20);
        _txtDocRate.TabIndex = 9;
        //
        // _lblDiscountPercent
        //
        _lblDiscountPercent.Location = new Point(15, 165);
        _lblDiscountPercent.Name = "_lblDiscountPercent";
        _lblDiscountPercent.Size = new Size(66, 13);
        _lblDiscountPercent.Text = "% Descuento:";
        //
        // _txtDiscountPercent
        //
        _txtDiscountPercent.Location = new Point(140, 162);
        _txtDiscountPercent.Name = "_txtDiscountPercent";
        _txtDiscountPercent.Properties.ReadOnly = true;
        _txtDiscountPercent.Size = new Size(160, 20);
        _txtDiscountPercent.TabIndex = 10;
        //
        // _lblVatSum
        //
        _lblVatSum.Location = new Point(350, 165);
        _lblVatSum.Name = "_lblVatSum";
        _lblVatSum.Size = new Size(45, 13);
        _lblVatSum.Text = "Impuesto:";
        //
        // _txtVatSum
        //
        _txtVatSum.Location = new Point(465, 162);
        _txtVatSum.Name = "_txtVatSum";
        _txtVatSum.Properties.ReadOnly = true;
        _txtVatSum.Size = new Size(160, 20);
        _txtVatSum.TabIndex = 11;
        //
        // _lblDocTotal
        //
        _lblDocTotal.Location = new Point(15, 192);
        _lblDocTotal.Name = "_lblDocTotal";
        _lblDocTotal.Size = new Size(30, 13);
        _lblDocTotal.Text = "Total:";
        //
        // _txtDocTotal
        //
        _txtDocTotal.Location = new Point(140, 189);
        _txtDocTotal.Name = "_txtDocTotal";
        _txtDocTotal.Properties.ReadOnly = true;
        _txtDocTotal.Size = new Size(160, 20);
        _txtDocTotal.TabIndex = 12;
        //
        // _lblVendedor
        //
        _lblVendedor.Location = new Point(350, 192);
        _lblVendedor.Name = "_lblVendedor";
        _lblVendedor.Size = new Size(48, 13);
        _lblVendedor.Text = "Vendedor:";
        //
        // _txtVendedor
        //
        _txtVendedor.Location = new Point(465, 189);
        _txtVendedor.Name = "_txtVendedor";
        _txtVendedor.Properties.ReadOnly = true;
        _txtVendedor.Size = new Size(160, 20);
        _txtVendedor.TabIndex = 13;
        //
        // _lblFolioFronterra
        //
        _lblFolioFronterra.Location = new Point(15, 219);
        _lblFolioFronterra.Name = "_lblFolioFronterra";
        _lblFolioFronterra.Size = new Size(78, 13);
        _lblFolioFronterra.Text = "Folio Fronterra:";
        //
        // _txtFolioFronterra
        //
        _txtFolioFronterra.Location = new Point(140, 216);
        _txtFolioFronterra.Name = "_txtFolioFronterra";
        _txtFolioFronterra.Properties.ReadOnly = true;
        _txtFolioFronterra.Size = new Size(160, 20);
        _txtFolioFronterra.TabIndex = 14;
        //
        // _lblDireccion
        //
        _lblDireccion.Location = new Point(15, 246);
        _lblDireccion.Name = "_lblDireccion";
        _lblDireccion.Size = new Size(50, 13);
        _lblDireccion.Text = "Dirección:";
        //
        // _txtDireccion
        //
        _txtDireccion.Location = new Point(140, 243);
        _txtDireccion.Name = "_txtDireccion";
        _txtDireccion.Properties.ReadOnly = true;
        _txtDireccion.Size = new Size(745, 20);
        _txtDireccion.TabIndex = 15;
        //
        // _lblComentarios
        //
        _lblComentarios.Location = new Point(15, 273);
        _lblComentarios.Name = "_lblComentarios";
        _lblComentarios.Size = new Size(63, 13);
        _lblComentarios.Text = "Comentarios:";
        //
        // _memComentarios
        //
        _memComentarios.Location = new Point(140, 273);
        _memComentarios.Name = "_memComentarios";
        _memComentarios.Properties.ReadOnly = true;
        _memComentarios.Size = new Size(745, 60);
        _memComentarios.TabIndex = 16;
        //
        // _grpDetalle
        //
        _grpDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grpDetalle.Controls.Add(_grid);
        _grpDetalle.Location = new Point(10, 367);
        _grpDetalle.Name = "_grpDetalle";
        _grpDetalle.Size = new Size(900, 260);
        _grpDetalle.TabIndex = 1;
        _grpDetalle.Text = "Artículos";
        //
        // _grid
        //
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.Location = new Point(10, 25);
        _grid.MainView = _gridView;
        _grid.Name = "_grid";
        _grid.Size = new Size(880, 225);
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
        _gridView.OptionsView.ShowFooter = true;
        _gridView.Columns.AddRange(new GridColumn[] { _colCodigo, _colDescripcion, _colCantidad, _colPrecioUnitario, _colTotal, _colAlmacen });
        //
        // _colCodigo
        //
        _colCodigo.Caption = "Código";
        _colCodigo.FieldName = "Codigo";
        _colCodigo.Name = "_colCodigo";
        _colCodigo.Visible = true;
        _colCodigo.VisibleIndex = 0;
        _colCodigo.Width = 90;
        //
        // _colDescripcion
        //
        _colDescripcion.Caption = "Descripción";
        _colDescripcion.FieldName = "Descripcion";
        _colDescripcion.Name = "_colDescripcion";
        _colDescripcion.Visible = true;
        _colDescripcion.VisibleIndex = 1;
        _colDescripcion.Width = 320;
        //
        // _colCantidad
        //
        _colCantidad.Caption = "Cantidad";
        _colCantidad.DisplayFormat.FormatString = "N2";
        _colCantidad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _colCantidad.FieldName = "Cantidad";
        _colCantidad.Name = "_colCantidad";
        _colCantidad.Visible = true;
        _colCantidad.VisibleIndex = 2;
        _colCantidad.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        _colCantidad.SummaryItem.DisplayFormat = "{0:N2}";
        _colCantidad.Width = 90;
        //
        // _colPrecioUnitario
        //
        _colPrecioUnitario.Caption = "Precio Unitario";
        _colPrecioUnitario.DisplayFormat.FormatString = "N2";
        _colPrecioUnitario.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _colPrecioUnitario.FieldName = "PrecioUnitario";
        _colPrecioUnitario.Name = "_colPrecioUnitario";
        _colPrecioUnitario.Visible = true;
        _colPrecioUnitario.VisibleIndex = 3;
        _colPrecioUnitario.Width = 100;
        //
        // _colTotal
        //
        _colTotal.Caption = "Total";
        _colTotal.DisplayFormat.FormatString = "N2";
        _colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        _colTotal.FieldName = "Total";
        _colTotal.Name = "_colTotal";
        _colTotal.Visible = true;
        _colTotal.VisibleIndex = 4;
        _colTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        _colTotal.SummaryItem.DisplayFormat = "{0:N2}";
        _colTotal.Width = 100;
        //
        // _colAlmacen
        //
        _colAlmacen.Caption = "Almacén";
        _colAlmacen.FieldName = "Almacen";
        _colAlmacen.Name = "_colAlmacen";
        _colAlmacen.Visible = true;
        _colAlmacen.VisibleIndex = 5;
        _colAlmacen.Width = 80;
        //
        // _btnCerrar
        //
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Location = new Point(820, 637);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(90, 23);
        _btnCerrar.TabIndex = 2;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Click += BtnCerrar_Click;
        //
        // PedidoDetalleForm
        //
        ClientSize = new Size(920, 670);
        Controls.Add(_grpEncabezado);
        Controls.Add(_grpDetalle);
        Controls.Add(_btnCerrar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PedidoDetalleForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne - Pedido";
        Shown += PedidoDetalleForm_Load;
        ((System.ComponentModel.ISupportInitialize)_txtDocNum.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEstatus.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCardCode.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCardName.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNumAtCard.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocCurrency.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocDate.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocDueDate.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtTaxDate.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocRate.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDiscountPercent.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtVatSum.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDocTotal.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtVendedor.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFolioFronterra.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDireccion.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_memComentarios.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpEncabezado).EndInit();
        _grpEncabezado.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridView).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpDetalle).EndInit();
        _grpDetalle.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
}
