using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Reempaques;

partial class ReempaqueEditarForm
{
    private IContainer components = null;

    private LabelControl _lblFolio;
    private TextEdit _txtFolio;
    private LabelControl _lblFecha;
    private TextEdit _txtFecha;
    private LabelControl _lblMotivo;
    private TextEdit _txtMotivo;
    private LabelControl _lblEstatus;
    private TextEdit _txtEstatus;
    private SimpleButton _btnGuardar;

    private GroupControl _grpKilos;
    private LabelControl _lblKilosAProcesar;
    private TextEdit _txtKilosAProcesar;
    private LabelControl _lblKilosProcesados;
    private TextEdit _txtKilosProcesados;
    private LabelControl _lblDiferencia;
    private TextEdit _txtDiferencia;

    private XtraTabControl _tabs;
    private XtraTabPage _tabEntrada;
    private XtraTabPage _tabSalida;

    private GridControl _gridEntrada;
    private GridView _gridViewEntrada;
    private SimpleButton _btnAgregarPallet;
    private SimpleButton _btnQuitarPallet;

    private GridControl _gridSalida;
    private GridView _gridViewSalida;
    private SimpleButton _btnAgregarAPallet;
    private SimpleButton _btnNuevoPallet;
    private SimpleButton _btnQuitarLinea;
    private SimpleButton _btnAjusteNeutro;

    private SimpleButton _btnCerrarReempaque;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReempaqueEditarForm));
        _lblFolio = new LabelControl();
        _txtFolio = new TextEdit();
        _lblFecha = new LabelControl();
        _txtFecha = new TextEdit();
        _lblMotivo = new LabelControl();
        _txtMotivo = new TextEdit();
        _lblEstatus = new LabelControl();
        _txtEstatus = new TextEdit();
        _btnGuardar = new SimpleButton();

        _grpKilos = new GroupControl();
        _lblKilosAProcesar = new LabelControl();
        _txtKilosAProcesar = new TextEdit();
        _lblKilosProcesados = new LabelControl();
        _txtKilosProcesados = new TextEdit();
        _lblDiferencia = new LabelControl();
        _txtDiferencia = new TextEdit();

        _tabs = new XtraTabControl();
        _tabEntrada = new XtraTabPage();
        _tabSalida = new XtraTabPage();

        _gridEntrada = new GridControl();
        _gridViewEntrada = new GridView();
        _btnAgregarPallet = new SimpleButton();
        _btnQuitarPallet = new SimpleButton();

        _gridSalida = new GridControl();
        _gridViewSalida = new GridView();
        _btnAgregarAPallet = new SimpleButton();
        _btnNuevoPallet = new SimpleButton();
        _btnQuitarLinea = new SimpleButton();
        _btnAjusteNeutro = new SimpleButton();

        _btnCerrarReempaque = new SimpleButton();
        _btnCerrar = new SimpleButton();

        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtFecha.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtMotivo.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtEstatus.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_grpKilos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosAProcesar.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosProcesados.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_txtDiferencia.Properties).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_tabs).BeginInit();
        _tabs.SuspendLayout();
        _tabEntrada.SuspendLayout();
        _tabSalida.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_gridEntrada).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewEntrada).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridSalida).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewSalida).BeginInit();
        SuspendLayout();

        // Encabezado
        _lblFolio.Location = new Point(12, 15);
        _lblFolio.Name = "_lblFolio";
        _lblFolio.Size = new Size(40, 13);
        _lblFolio.Text = "Folio:";

        _txtFolio.Location = new Point(90, 12);
        _txtFolio.Name = "_txtFolio";
        _txtFolio.Properties.ReadOnly = true;
        _txtFolio.Size = new Size(110, 20);
        _txtFolio.TabIndex = 0;

        _lblFecha.Location = new Point(220, 15);
        _lblFecha.Name = "_lblFecha";
        _lblFecha.Size = new Size(40, 13);
        _lblFecha.Text = "Fecha:";

        _txtFecha.Location = new Point(280, 12);
        _txtFecha.Name = "_txtFecha";
        _txtFecha.Properties.ReadOnly = true;
        _txtFecha.Size = new Size(100, 20);
        _txtFecha.TabIndex = 1;

        _lblEstatus.Location = new Point(400, 15);
        _lblEstatus.Name = "_lblEstatus";
        _lblEstatus.Size = new Size(45, 13);
        _lblEstatus.Text = "Estatus:";

        _txtEstatus.Location = new Point(460, 12);
        _txtEstatus.Name = "_txtEstatus";
        _txtEstatus.Properties.ReadOnly = true;
        _txtEstatus.Size = new Size(120, 20);
        _txtEstatus.TabIndex = 2;

        _lblMotivo.Location = new Point(12, 44);
        _lblMotivo.Name = "_lblMotivo";
        _lblMotivo.Size = new Size(45, 13);
        _lblMotivo.Text = "Motivo:";

        _txtMotivo.Location = new Point(90, 41);
        _txtMotivo.Name = "_txtMotivo";
        _txtMotivo.Size = new Size(650, 20);
        _txtMotivo.TabIndex = 3;

        _btnGuardar.Location = new Point(760, 34);
        _btnGuardar.Name = "_btnGuardar";
        _btnGuardar.Size = new Size(100, 28);
        _btnGuardar.TabIndex = 4;
        _btnGuardar.Text = "Guardar";
        _btnGuardar.ImageOptions.Image = (Image)resources.GetObject("_btnGuardar.ImageOptions.Image");
        _btnGuardar.Click += BtnGuardar_Click;

        // Panel Control de Kilogramos
        _grpKilos.Location = new Point(12, 70);
        _grpKilos.Name = "_grpKilos";
        _grpKilos.Size = new Size(956, 90);
        _grpKilos.Text = "Control de Kilogramos";
        _grpKilos.TabIndex = 5;

        _lblKilosAProcesar.Location = new Point(15, 38);
        _lblKilosAProcesar.Name = "_lblKilosAProcesar";
        _lblKilosAProcesar.Text = "Kilogramos a Procesar:";

        _txtKilosAProcesar.Location = new Point(175, 35);
        _txtKilosAProcesar.Name = "_txtKilosAProcesar";
        _txtKilosAProcesar.Properties.ReadOnly = true;
        _txtKilosAProcesar.Size = new Size(100, 20);
        _txtKilosAProcesar.TabIndex = 0;

        _lblKilosProcesados.Location = new Point(310, 38);
        _lblKilosProcesados.Name = "_lblKilosProcesados";
        _lblKilosProcesados.Text = "Kilogramos Procesados:";

        _txtKilosProcesados.Location = new Point(470, 35);
        _txtKilosProcesados.Name = "_txtKilosProcesados";
        _txtKilosProcesados.Properties.ReadOnly = true;
        _txtKilosProcesados.Size = new Size(100, 20);
        _txtKilosProcesados.TabIndex = 1;

        _lblDiferencia.Location = new Point(605, 38);
        _lblDiferencia.Name = "_lblDiferencia";
        _lblDiferencia.Text = "Diferencia:";

        _txtDiferencia.Location = new Point(680, 35);
        _txtDiferencia.Name = "_txtDiferencia";
        _txtDiferencia.Properties.ReadOnly = true;
        _txtDiferencia.Size = new Size(100, 20);
        _txtDiferencia.TabIndex = 2;

        _grpKilos.Controls.Add(_lblKilosAProcesar);
        _grpKilos.Controls.Add(_txtKilosAProcesar);
        _grpKilos.Controls.Add(_lblKilosProcesados);
        _grpKilos.Controls.Add(_txtKilosProcesados);
        _grpKilos.Controls.Add(_lblDiferencia);
        _grpKilos.Controls.Add(_txtDiferencia);

        // Tabs — el grid llena TODO el tab page con Dock=Fill (nunca depende de medir a mano el
        // alto de la franja de pestañas). Los botones de acción NO viven dentro del tab page: se
        // movieron a una fila fija del formulario, debajo de los tabs, con visibilidad según la
        // pestaña activa (ver Tabs_SelectedPageChanged en el .cs) — evita el bug de botones fuera
        // del área visible cuando el área interna del tab no coincide con lo calculado a mano.
        _tabs.Location = new Point(12, 170);
        _tabs.Name = "_tabs";
        _tabs.SelectedTabPage = _tabEntrada;
        _tabs.Size = new Size(956, 400);
        _tabs.TabIndex = 6;
        _tabs.TabPages.AddRange(new XtraTabPage[] { _tabEntrada, _tabSalida });
        _tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        _tabEntrada.Name = "_tabEntrada";
        _tabEntrada.Text = "Entrada";
        _tabEntrada.Controls.Add(_gridEntrada);

        _gridEntrada.Dock = DockStyle.Fill;
        _gridEntrada.MainView = _gridViewEntrada;
        _gridEntrada.Name = "_gridEntrada";
        _gridEntrada.TabIndex = 0;
        _gridEntrada.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewEntrada });

        _gridViewEntrada.GridControl = _gridEntrada;
        _gridViewEntrada.Name = "_gridViewEntrada";
        _gridViewEntrada.OptionsFind.AlwaysVisible = true;
        _gridViewEntrada.OptionsView.ShowGroupPanel = false;
        _gridViewEntrada.OptionsBehavior.Editable = false;
        _gridViewEntrada.OptionsView.ColumnAutoWidth = false;
        _gridViewEntrada.RowCellClick += GridViewEntrada_RowCellClick;
        _gridViewEntrada.MouseMove += GridViewEntrada_MouseMove;

        _tabSalida.Name = "_tabSalida";
        _tabSalida.Text = "Salida";
        _tabSalida.Controls.Add(_gridSalida);

        _gridSalida.Dock = DockStyle.Fill;
        _gridSalida.MainView = _gridViewSalida;
        _gridSalida.Name = "_gridSalida";
        _gridSalida.TabIndex = 0;
        _gridSalida.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { _gridViewSalida });

        _gridViewSalida.GridControl = _gridSalida;
        _gridViewSalida.Name = "_gridViewSalida";
        _gridViewSalida.OptionsFind.AlwaysVisible = true;
        _gridViewSalida.OptionsView.ShowGroupPanel = false;
        _gridViewSalida.OptionsBehavior.Editable = false;
        _gridViewSalida.OptionsView.ColumnAutoWidth = false;
        _gridViewSalida.RowCellClick += GridViewSalida_RowCellClick;
        _gridViewSalida.MouseMove += GridViewSalida_MouseMove;
        _gridViewSalida.CustomColumnDisplayText += GridViewSalida_CustomColumnDisplayText;

        _tabs.SelectedPageChanged += Tabs_SelectedPageChanged;

        // Fila de botones de acción (fija, fuera de los tabs) — Entrada y Salida comparten el
        // mismo renglón; solo se ve el juego de botones de la pestaña activa.
        _btnAgregarPallet.Location = new Point(12, 576);
        _btnAgregarPallet.Name = "_btnAgregarPallet";
        _btnAgregarPallet.Size = new Size(120, 28);
        _btnAgregarPallet.TabIndex = 7;
        _btnAgregarPallet.Text = "Agregar Pallet";
        _btnAgregarPallet.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnAgregarPallet.Click += BtnAgregarPallet_Click;

        _btnQuitarPallet.Location = new Point(140, 576);
        _btnQuitarPallet.Name = "_btnQuitarPallet";
        _btnQuitarPallet.Size = new Size(120, 28);
        _btnQuitarPallet.TabIndex = 8;
        _btnQuitarPallet.Text = "Quitar Pallet";
        _btnQuitarPallet.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnQuitarPallet.Click += BtnQuitarPallet_Click;

        _btnAgregarAPallet.Location = new Point(12, 576);
        _btnAgregarAPallet.Name = "_btnAgregarAPallet";
        _btnAgregarAPallet.Size = new Size(140, 28);
        _btnAgregarAPallet.TabIndex = 9;
        _btnAgregarAPallet.Text = "Agregar a Pallet";
        _btnAgregarAPallet.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnAgregarAPallet.Visible = false;
        _btnAgregarAPallet.Click += BtnAgregarAPallet_Click;

        _btnNuevoPallet.Location = new Point(160, 576);
        _btnNuevoPallet.Name = "_btnNuevoPallet";
        _btnNuevoPallet.Size = new Size(120, 28);
        _btnNuevoPallet.TabIndex = 10;
        _btnNuevoPallet.Text = "Nuevo Pallet";
        _btnNuevoPallet.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnNuevoPallet.Visible = false;
        _btnNuevoPallet.Click += BtnNuevoPallet_Click;

        _btnQuitarLinea.Location = new Point(288, 576);
        _btnQuitarLinea.Name = "_btnQuitarLinea";
        _btnQuitarLinea.Size = new Size(120, 28);
        _btnQuitarLinea.TabIndex = 11;
        _btnQuitarLinea.Text = "Quitar Línea";
        _btnQuitarLinea.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnQuitarLinea.Visible = false;
        _btnQuitarLinea.Click += BtnQuitarLinea_Click;

        _btnAjusteNeutro.Location = new Point(416, 576);
        _btnAjusteNeutro.Name = "_btnAjusteNeutro";
        _btnAjusteNeutro.Size = new Size(140, 28);
        _btnAjusteNeutro.TabIndex = 12;
        _btnAjusteNeutro.Text = "Ajuste (Neutro)";
        _btnAjusteNeutro.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnAjusteNeutro.Visible = false;
        _btnAjusteNeutro.Click += BtnAjusteNeutro_Click;

        // Botones inferiores
        _btnCerrarReempaque.Location = new Point(12, 614);
        _btnCerrarReempaque.Name = "_btnCerrarReempaque";
        _btnCerrarReempaque.Size = new Size(140, 28);
        _btnCerrarReempaque.TabIndex = 13;
        _btnCerrarReempaque.Text = "Cerrar Reempaque";
        _btnCerrarReempaque.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _btnCerrarReempaque.Click += BtnCerrarReempaque_Click;

        _btnCerrar.Location = new Point(868, 614);
        _btnCerrar.Name = "_btnCerrar";
        _btnCerrar.Size = new Size(100, 28);
        _btnCerrar.TabIndex = 14;
        _btnCerrar.Text = "Cerrar";
        _btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCerrar.ImageOptions.Image = (Image)resources.GetObject("_btnCerrar.ImageOptions.Image");
        _btnCerrar.Click += BtnCerrar_Click;

        // ReempaqueEditarForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(980, 650);
        Controls.Add(_lblFolio);
        Controls.Add(_txtFolio);
        Controls.Add(_lblFecha);
        Controls.Add(_txtFecha);
        Controls.Add(_lblEstatus);
        Controls.Add(_txtEstatus);
        Controls.Add(_lblMotivo);
        Controls.Add(_txtMotivo);
        Controls.Add(_btnGuardar);
        Controls.Add(_grpKilos);
        Controls.Add(_tabs);
        Controls.Add(_btnAgregarPallet);
        Controls.Add(_btnQuitarPallet);
        Controls.Add(_btnAgregarAPallet);
        Controls.Add(_btnNuevoPallet);
        Controls.Add(_btnQuitarLinea);
        Controls.Add(_btnAjusteNeutro);
        Controls.Add(_btnCerrarReempaque);
        Controls.Add(_btnCerrar);
        Name = "ReempaqueEditarForm";
        Text = "Reempaque";
        StartPosition = FormStartPosition.CenterScreen;

        ((System.ComponentModel.ISupportInitialize)_txtFolio.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtFecha.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtMotivo.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtEstatus.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_grpKilos).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosAProcesar.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtKilosProcesados.Properties).EndInit();
        ((System.ComponentModel.ISupportInitialize)_txtDiferencia.Properties).EndInit();
        _tabEntrada.ResumeLayout(false);
        _tabSalida.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_gridEntrada).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewEntrada).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridSalida).EndInit();
        ((System.ComponentModel.ISupportInitialize)_gridViewSalida).EndInit();
        ((System.ComponentModel.ISupportInitialize)_tabs).EndInit();
        _tabs.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
