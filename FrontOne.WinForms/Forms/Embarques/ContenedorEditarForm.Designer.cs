using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Embarques;

partial class ContenedorEditarForm
{
    private IContainer components = null;

    private LabelControl _lblFolio;
    private TextEdit _txtFolio;
    private LabelControl _lblFecha;
    private DateEdit _dtFecha;
    private LabelControl _lblPedidoSap;
    private TextEdit _txtPedidoSap;
    private SimpleButton _btnBuscarPedido;
    private LabelControl _lblFolioFronterra;
    private TextEdit _txtFolioFronterra;
    private LabelControl _lblCodigoCliente;
    private TextEdit _txtCodigoCliente;
    private LabelControl _lblNombreCliente;
    private TextEdit _txtNombreCliente;
    private LabelControl _lblObservaciones;
    private MemoEdit _memoObservaciones;
    private SimpleButton _btnGuardar;

    private GridControl _gridPedido;
    private GridView _gridViewPedido;

    private XtraTabControl _tabs;
    private XtraTabPage _tabPedido;
    private XtraTabPage _tabEmbarque;

    private SplitContainerControl _splitPrincipal;
    private SplitContainerControl _splitDerecho;
    private GridControl _gridPallets;
    private GridView _gridViewPallets;
    private GridControl _gridPalletDetalle;
    private GridView _gridViewPalletDetalle;
    private GridControl _gridResumen;
    private GridView _gridViewResumen;
    private Panel _pnlBotonesPallets;
    private SimpleButton _btnAgregarPallet;
    private SimpleButton _btnEliminarPallet;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContenedorEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _dtFecha = new DateEdit();
        _lblPedidoSap = new LabelControl();
        _txtPedidoSap = new TextEdit();
        _btnBuscarPedido = new SimpleButton();
        _lblFolioFronterra = new LabelControl();
        _txtFolioFronterra = new TextEdit();
        _lblCodigoCliente = new LabelControl();
        _txtCodigoCliente = new TextEdit();
        _lblNombreCliente = new LabelControl();
        _txtNombreCliente = new TextEdit();
        _lblObservaciones = new LabelControl();
        _memoObservaciones = new MemoEdit();
        _btnGuardar = new SimpleButton();
        _gridPedido = new GridControl();
        _gridViewPedido = new GridView();

        _tabs = new XtraTabControl();
        _tabPedido = new XtraTabPage();
        _tabEmbarque = new XtraTabPage();

        _splitPrincipal = new SplitContainerControl();
        _splitDerecho = new SplitContainerControl();
        _gridPallets = new GridControl();
        _gridViewPallets = new GridView();
        _gridPalletDetalle = new GridControl();
        _gridViewPalletDetalle = new GridView();
        _gridResumen = new GridControl();
        _gridViewResumen = new GridView();
        _pnlBotonesPallets = new Panel();
        _btnAgregarPallet = new SimpleButton();
        _btnEliminarPallet = new SimpleButton();

        _btnCerrar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtPedidoSap.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFolioFronterra.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoCliente.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombreCliente.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_memoObservaciones.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridPedido).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPedido).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_tabs).BeginInit();
        _tabs.SuspendLayout();
        _tabPedido.SuspendLayout();
        _tabEmbarque.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_splitPrincipal).BeginInit();
        _splitPrincipal.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_splitDerecho).BeginInit();
        _splitDerecho.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_gridPallets).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPallets).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridPalletDetalle).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPalletDetalle).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridResumen).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewResumen).BeginInit();
        SuspendLayout();

        // ===== Tab Pedido: encabezado =====
        _lblFolio.Location = new Point(12, 15);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(40, 13);
        _lblFolio.Text = "Folio:";

        _txtFolio.Location = new Point(65, 12);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Properties.ReadOnly = true;
        _txtFolio.Size = new Size(110, 20);
        _txtFolio.TabIndex = 0;

        _lblFecha.Location = new Point(195, 15);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(45, 13);
        _lblFecha.Text = "Fecha:";

        _dtFecha.EditValue = null;
        _dtFecha.Location = new Point(245, 12);
        _dtFecha.Name = "_dtFecha";
        _dtFecha.Size = new Size(110, 20);
        _dtFecha.TabIndex = 1;

        _lblPedidoSap.Location = new Point(375, 15);
        _lblPedidoSap.Name = "_lblPedidoSap";
        _lblPedidoSap.Size = new Size(75, 13);
        _lblPedidoSap.Text = "Pedido SAP:";

        _txtPedidoSap.Location = new Point(455, 12);
        _txtPedidoSap.Name = "_txtPedidoSap";
        _txtPedidoSap.Properties.ReadOnly = true;
        _txtPedidoSap.Size = new Size(160, 20);
        _txtPedidoSap.TabIndex = 2;

        _btnBuscarPedido.Location = new Point(620, 11);
        _btnBuscarPedido.Name = "_btnBuscarPedido";
        _btnBuscarPedido.Size = new Size(30, 28);
        _btnBuscarPedido.TabIndex = 3;
        _btnBuscarPedido.Text = "...";
        _btnBuscarPedido.Click += BtnBuscarPedido_Click;

        _lblFolioFronterra.Location = new Point(12, 50);
        _lblFolioFronterra.Name = "_lblFolioFronterra";
        _lblFolioFronterra.Size = new Size(100, 13);
        _lblFolioFronterra.Text = "Folio Fronterra:";

        _txtFolioFronterra.Location = new Point(115, 47);
        _txtFolioFronterra.Name = "_txtFolioFronterra";
        _txtFolioFronterra.Properties.ReadOnly = true;
        _txtFolioFronterra.Size = new Size(150, 20);
        _txtFolioFronterra.TabIndex = 4;

        _lblCodigoCliente.Location = new Point(285, 50);
        _lblCodigoCliente.Name = "_lblCodigoCliente";
        _lblCodigoCliente.Size = new Size(95, 13);
        _lblCodigoCliente.Text = "Código Cliente:";

        _txtCodigoCliente.Location = new Point(385, 47);
        _txtCodigoCliente.Name = "_txtCodigoCliente";
        _txtCodigoCliente.Properties.ReadOnly = true;
        _txtCodigoCliente.Size = new Size(120, 20);
        _txtCodigoCliente.TabIndex = 5;

        _lblNombreCliente.Location = new Point(525, 50);
        _lblNombreCliente.Name = "_lblNombreCliente";
        _lblNombreCliente.Size = new Size(55, 13);
        _lblNombreCliente.Text = "Cliente:";

        _txtNombreCliente.Location = new Point(585, 47);
        _txtNombreCliente.Name = "_txtNombreCliente";
        _txtNombreCliente.Properties.ReadOnly = true;
        _txtNombreCliente.Size = new Size(400, 20);
        _txtNombreCliente.TabIndex = 6;

        _lblObservaciones.Location = new Point(12, 92);
        _lblObservaciones.Name = "_lblObservaciones";
        _lblObservaciones.Size = new Size(90, 13);
        _lblObservaciones.Text = "Observaciones:";

        _memoObservaciones.Location = new Point(115, 86);
        _memoObservaciones.Name = "_memoObservaciones";
        _memoObservaciones.Size = new Size(1046, 55);
        _memoObservaciones.TabIndex = 7;

        _btnGuardar.Location = new Point(1174, 100);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(90, 28);
        _btnGuardar.TabIndex = 8;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Click += BtnGuardar_Click;

        // Grid de detalle del pedido — footer con totales, sin panel de búsqueda (excepción
        // documentada en CLAUDE.md: grids de resumen/totales del módulo Contenedor).
        _gridPedido.Location = new Point(12, 155);
        _gridPedido.MainView = _gridViewPedido;
        _gridPedido.Name = "_gridPedido";
        _gridPedido.Size = new Size(1252, 480);
        _gridPedido.TabIndex = 9;
        _gridPedido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _gridPedido.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewPedido });

        _gridViewPedido.GridControl = _gridPedido;
        _gridViewPedido.Name = "_gridViewPedido";
        _gridViewPedido.OptionsView.ShowGroupPanel = false;
        _gridViewPedido.OptionsBehavior.Editable = false;
        _gridViewPedido.OptionsView.ColumnAutoWidth = false;
        _gridViewPedido.OptionsView.ShowFooter = true;

        _tabPedido.Name = "_tabPedido";
        _tabPedido.Text = "Pedido";
        _tabPedido.Controls.Add(_gridPedido);
        _tabPedido.Controls.Add(_lblFolio);
        _tabPedido.Controls.Add(_txtFolio);
        _tabPedido.Controls.Add(_lblFecha);
        _tabPedido.Controls.Add(_dtFecha);
        _tabPedido.Controls.Add(_lblPedidoSap);
        _tabPedido.Controls.Add(_txtPedidoSap);
        _tabPedido.Controls.Add(_btnBuscarPedido);
        _tabPedido.Controls.Add(_lblFolioFronterra);
        _tabPedido.Controls.Add(_txtFolioFronterra);
        _tabPedido.Controls.Add(_lblCodigoCliente);
        _tabPedido.Controls.Add(_txtCodigoCliente);
        _tabPedido.Controls.Add(_lblNombreCliente);
        _tabPedido.Controls.Add(_txtNombreCliente);
        _tabPedido.Controls.Add(_lblObservaciones);
        _tabPedido.Controls.Add(_memoObservaciones);
        _tabPedido.Controls.Add(_btnGuardar);

        // ===== Tab Embarque: 3 secciones (izquierda total / derecha arriba / derecha abajo) =====
        _gridPallets.MainView = _gridViewPallets;
        _gridPallets.Name = "_gridPallets";
        _gridPallets.Dock = DockStyle.Fill;
        _gridPallets.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewPallets });

        _gridViewPallets.GridControl = _gridPallets;
        _gridViewPallets.Name = "_gridViewPallets";
        _gridViewPallets.OptionsView.ShowGroupPanel = false;
        _gridViewPallets.OptionsBehavior.Editable = false;
        _gridViewPallets.OptionsView.ColumnAutoWidth = false;
        _gridViewPallets.OptionsView.ShowFooter = true;
        _gridViewPallets.FocusedRowChanged += GridViewPallets_FocusedRowChanged;

        // Panel fijo acoplado abajo (Dock=Bottom) para los botones chicos — reserva su espacio
        // sin importar la altura real del panel en tiempo de ejecución, a diferencia de
        // posicionarlos a mano con Anchor=Bottom (esos quedaban fuera del área visible).
        _btnAgregarPallet.Location = new Point(4, 6);
        _btnAgregarPallet.Name = "_btnAgregarPallet";
        _btnAgregarPallet.Size = new Size(90, 28);
        _btnAgregarPallet.TabIndex = 0;
        _btnAgregarPallet.Text = "Agregar Pallet";
        _btnAgregarPallet.Click += BtnAgregarPallet_Click;

        _btnEliminarPallet.Location = new Point(100, 6);
        _btnEliminarPallet.Name = "_btnEliminarPallet";
        _btnEliminarPallet.Size = new Size(90, 28);
        _btnEliminarPallet.TabIndex = 1;
        _btnEliminarPallet.Text = "Eliminar Pallet";
        _btnEliminarPallet.Click += BtnEliminarPallet_Click;

        _pnlBotonesPallets.Name = "_pnlBotonesPallets";
        _pnlBotonesPallets.Dock = DockStyle.Bottom;
        _pnlBotonesPallets.Size = new Size(400, 40);
        _pnlBotonesPallets.Controls.Add(_btnAgregarPallet);
        _pnlBotonesPallets.Controls.Add(_btnEliminarPallet);

        // Orden de alta importa para el docking: el grid (Fill) primero, el panel de botones
        // (Bottom) después, así el Bottom reserva su franja y el Fill ocupa el resto.
        _splitPrincipal.Panel1.Controls.Add(_gridPallets);
        _splitPrincipal.Panel1.Controls.Add(_pnlBotonesPallets);
        _splitPrincipal.Panel1.Text = "Pallets";
        // Horizontal=true en DevExpress SplitContainerControl = paneles lado a lado (izq/der).
        _splitPrincipal.Horizontal = true;
        _splitPrincipal.Dock = DockStyle.Fill;
        _splitPrincipal.Name = "_splitPrincipal";
        _splitPrincipal.SplitterPosition = 400;

        _gridPalletDetalle.MainView = _gridViewPalletDetalle;
        _gridPalletDetalle.Name = "_gridPalletDetalle";
        _gridPalletDetalle.Dock = DockStyle.Fill;
        _gridPalletDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewPalletDetalle });

        _gridViewPalletDetalle.GridControl = _gridPalletDetalle;
        _gridViewPalletDetalle.Name = "_gridViewPalletDetalle";
        _gridViewPalletDetalle.OptionsView.ShowGroupPanel = false;
        _gridViewPalletDetalle.OptionsBehavior.Editable = false;
        _gridViewPalletDetalle.OptionsView.ColumnAutoWidth = false;
        _gridViewPalletDetalle.OptionsView.ShowFooter = true;

        _gridResumen.MainView = _gridViewResumen;
        _gridResumen.Name = "_gridResumen";
        _gridResumen.Dock = DockStyle.Fill;
        _gridResumen.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewResumen });

        _gridViewResumen.GridControl = _gridResumen;
        _gridViewResumen.Name = "_gridViewResumen";
        _gridViewResumen.OptionsView.ShowGroupPanel = false;
        _gridViewResumen.OptionsBehavior.Editable = false;
        _gridViewResumen.OptionsView.ColumnAutoWidth = false;
        _gridViewResumen.OptionsView.ShowFooter = true;

        _splitDerecho.Panel1.Controls.Add(_gridPalletDetalle);
        _splitDerecho.Panel1.Text = "Detalle del Pallet Seleccionado";
        _splitDerecho.Panel2.Controls.Add(_gridResumen);
        _splitDerecho.Panel2.Text = "Resumen por Calibre de Exportación";
        // Horizontal=false = paneles apilados (arriba/abajo): Detalle arriba, Resumen abajo.
        _splitDerecho.Horizontal = false;
        _splitDerecho.Dock = DockStyle.Fill;
        _splitDerecho.Name = "_splitDerecho";
        _splitDerecho.SplitterPosition = 300;

        _splitPrincipal.Panel2.Controls.Add(_splitDerecho);

        _tabEmbarque.Name = "_tabEmbarque";
        _tabEmbarque.Text = "Embarque";
        _tabEmbarque.Controls.Add(_splitPrincipal);

        // ===== Tabs =====
        _tabs.Location = new Point(12, 12);
        _tabs.Name = "_tabs";
        _tabs.SelectedTabPage = _tabPedido;
        _tabs.Size = new Size(1276, 656);
        _tabs.TabIndex = 0;
        _tabs.TabPages.AddRange(new XtraTabPage[] { _tabPedido, _tabEmbarque });
        _tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _tabs.SelectedPageChanged += Tabs_SelectedPageChanged;

        _btnCerrar.Location = new Point(1188, 680);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(100, 28);
        _btnCerrar.TabIndex = 1;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Click += BtnCerrar_Click;

        // ContenedorEditarForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1300, 720);
        Controls.Add(_tabs);
        Controls.Add(_btnCerrar);
        Name = "ContenedorEditarForm";
        Text = "Contenedor";
        StartPosition = FormStartPosition.CenterScreen;

        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_dtFecha.Properties.CalendarTimeProperties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtPedidoSap.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFolioFronterra.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtCodigoCliente.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtNombreCliente.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_memoObservaciones.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridPedido).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPedido).EndInit();
        _tabPedido.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_gridPallets).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPallets).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridPalletDetalle).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewPalletDetalle).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridResumen).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewResumen).EndInit();
        ((System.ComponentModel.ISupportInitialize)_splitDerecho).EndInit();
        _splitDerecho.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_splitPrincipal).EndInit();
        _splitPrincipal.ResumeLayout(false);
        _tabEmbarque.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_tabs).EndInit();
        _tabs.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
