using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraTabbedMdi;

namespace FrontOne.WinForms.Forms;

partial class MainForm
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

    private RibbonControl _ribbon;
    private RibbonPage _pageCatalogos;
    private RibbonPage _pageAcopio;
    private RibbonPage _pageRecepcion;
    private RibbonPage _pageSeguridad;
    private RibbonPage _pageSistema;
    private RibbonPageGroup _grpUbicaciones;
    private RibbonPageGroup _grpSociosNegocio;
    private RibbonPageGroup _grpCatalogosAcopio;
    private RibbonPageGroup _grpCatalogosAcarreo;
    private RibbonPageGroup _grpPreciosFruta;
    private RibbonPageGroup _grpOrdenesCorte;
    private RibbonPageGroup _grpPreciosAcarreo;
    private RibbonPageGroup _grpPreciosCorte;
    private RibbonPageGroup _grpRecepcionFruta;
    private RibbonPageGroup _grpUsuariosRoles;
    private RibbonPageGroup _grpConfiguracion;
    private RibbonPageGroup _grpAplicacion;
    private BarButtonItem _btnPaises;
    private BarButtonItem _btnEstados;
    private BarButtonItem _btnMunicipios;
    private BarButtonItem _btnProductores;
    private BarButtonItem _btnHuertas;
    private BarButtonItem _btnListaPrecioFruta;
    private BarButtonItem _btnVariedades;
    private BarButtonItem _btnTiposComercializacion;
    private BarButtonItem _btnMonedas;
    private BarButtonItem _btnTiposPago;
    private BarButtonItem _btnTiposCorte;
    private BarButtonItem _btnJefesAcopio;
    private BarButtonItem _btnFloraciones;
    private BarButtonItem _btnRecepcionesFruta;
    private BarButtonItem _btnConfiguracionEmpresa;
    private BarButtonItem _btnAcuerdosCorte;
    private BarButtonItem _btnOrdenesCorte;
    private BarButtonItem _btnZonas;
    private BarButtonItem _btnListaPrecioAcarreo;
    private BarButtonItem _btnListaPrecioCorte;
    private BarButtonItem _btnUsuarios;
    private BarButtonItem _btnRoles;
    private BarButtonItem _btnPermisos;
    private BarButtonItem _btnConfiguracionConexiones;
    private BarButtonItem _btnSalir;
    private RibbonStatusBar _statusBar;
    private BarStaticItem _staticUsuario;
    private XtraTabbedMdiManager _tabbedMdiManager;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        _tabbedMdiManager = new XtraTabbedMdiManager(components);
        _ribbon = new RibbonControl();
        _pageCatalogos = new RibbonPage();
        _pageAcopio = new RibbonPage();
        _pageRecepcion = new RibbonPage();
        _pageSeguridad = new RibbonPage();
        _pageSistema = new RibbonPage();
        _grpUbicaciones = new RibbonPageGroup();
        _grpSociosNegocio = new RibbonPageGroup();
        _grpCatalogosAcopio = new RibbonPageGroup();
        _grpCatalogosAcarreo = new RibbonPageGroup();
        _grpPreciosFruta = new RibbonPageGroup();
        _grpOrdenesCorte = new RibbonPageGroup();
        _grpPreciosAcarreo = new RibbonPageGroup();
        _grpPreciosCorte = new RibbonPageGroup();
        _grpRecepcionFruta = new RibbonPageGroup();
        _grpUsuariosRoles = new RibbonPageGroup();
        _grpConfiguracion = new RibbonPageGroup();
        _grpAplicacion = new RibbonPageGroup();
        _btnPaises = new BarButtonItem(_ribbon.Manager, "Países");
        _btnEstados = new BarButtonItem(_ribbon.Manager, "Estados");
        _btnMunicipios = new BarButtonItem(_ribbon.Manager, "Municipios");
        _btnProductores = new BarButtonItem(_ribbon.Manager, "Productores");
        _btnHuertas = new BarButtonItem(_ribbon.Manager, "Huertas");
        _btnListaPrecioFruta = new BarButtonItem(_ribbon.Manager, "Lista de Precio Fruta");
        _btnVariedades = new BarButtonItem(_ribbon.Manager, "Variedades");
        _btnTiposComercializacion = new BarButtonItem(_ribbon.Manager, "Tipos de Comercialización");
        _btnMonedas = new BarButtonItem(_ribbon.Manager, "Monedas");
        _btnTiposPago = new BarButtonItem(_ribbon.Manager, "Tipos de Pago");
        _btnTiposCorte = new BarButtonItem(_ribbon.Manager, "Tipos de Corte");
        _btnJefesAcopio = new BarButtonItem(_ribbon.Manager, "Jefes de Acopio");
        _btnFloraciones = new BarButtonItem(_ribbon.Manager, "Floración");
        _btnRecepcionesFruta = new BarButtonItem(_ribbon.Manager, "Recepciones de Fruta");
        _btnConfiguracionEmpresa = new BarButtonItem(_ribbon.Manager, "Datos de la empresa");
        _btnAcuerdosCorte = new BarButtonItem(_ribbon.Manager, "Acuerdos de Corte");
        _btnOrdenesCorte = new BarButtonItem(_ribbon.Manager, "Órdenes de Corte");
        _btnZonas = new BarButtonItem(_ribbon.Manager, "Zonas");
        _btnListaPrecioAcarreo = new BarButtonItem(_ribbon.Manager, "Lista de Precios de Acarreo");
        _btnListaPrecioCorte = new BarButtonItem(_ribbon.Manager, "Lista de Precios de Corte");
        _btnUsuarios = new BarButtonItem(_ribbon.Manager, "Usuarios");
        _btnRoles = new BarButtonItem(_ribbon.Manager, "Roles");
        _btnPermisos = new BarButtonItem(_ribbon.Manager, "Permisos");
        _btnConfiguracionConexiones = new BarButtonItem(_ribbon.Manager, "Configuración de conexiones");
        _btnSalir = new BarButtonItem(_ribbon.Manager, "Salir");
        _statusBar = new RibbonStatusBar();
        _staticUsuario = new BarStaticItem();
        ((System.ComponentModel.ISupportInitialize)_ribbon).BeginInit();
        SuspendLayout();
        //
        // _btnPaises
        //
        _btnPaises.Id = 1;
        _btnPaises.Name = "_btnPaises";
        _btnPaises.RibbonStyle = RibbonItemStyles.Large;
        _btnPaises.ItemClick += BtnPaises_ItemClick;
        //
        // _btnEstados
        //
        _btnEstados.Id = 2;
        _btnEstados.Name = "_btnEstados";
        _btnEstados.RibbonStyle = RibbonItemStyles.Large;
        _btnEstados.ItemClick += BtnEstados_ItemClick;
        //
        // _btnMunicipios
        //
        _btnMunicipios.Id = 11;
        _btnMunicipios.Name = "_btnMunicipios";
        _btnMunicipios.RibbonStyle = RibbonItemStyles.Large;
        _btnMunicipios.ItemClick += BtnMunicipios_ItemClick;
        //
        // _btnProductores
        //
        _btnProductores.Id = 3;
        _btnProductores.Name = "_btnProductores";
        _btnProductores.RibbonStyle = RibbonItemStyles.Large;
        _btnProductores.ItemClick += BtnProductores_ItemClick;
        //
        // _btnHuertas
        //
        _btnHuertas.Id = 4;
        _btnHuertas.Name = "_btnHuertas";
        _btnHuertas.RibbonStyle = RibbonItemStyles.Large;
        _btnHuertas.ItemClick += BtnHuertas_ItemClick;
        //
        // _btnListaPrecioFruta
        //
        _btnListaPrecioFruta.Id = 12;
        _btnListaPrecioFruta.Name = "_btnListaPrecioFruta";
        _btnListaPrecioFruta.RibbonStyle = RibbonItemStyles.Large;
        _btnListaPrecioFruta.ItemClick += BtnListaPrecioFruta_ItemClick;
        //
        // _btnVariedades
        //
        _btnVariedades.Id = 15;
        _btnVariedades.Name = "_btnVariedades";
        _btnVariedades.RibbonStyle = RibbonItemStyles.Large;
        _btnVariedades.ItemClick += BtnVariedades_ItemClick;
        //
        // _btnTiposComercializacion
        //
        _btnTiposComercializacion.Id = 16;
        _btnTiposComercializacion.Name = "_btnTiposComercializacion";
        _btnTiposComercializacion.RibbonStyle = RibbonItemStyles.Large;
        _btnTiposComercializacion.ItemClick += BtnTiposComercializacion_ItemClick;
        //
        // _btnMonedas
        //
        _btnMonedas.Id = 17;
        _btnMonedas.Name = "_btnMonedas";
        _btnMonedas.RibbonStyle = RibbonItemStyles.Large;
        _btnMonedas.ItemClick += BtnMonedas_ItemClick;
        //
        // _btnTiposPago
        //
        _btnTiposPago.Id = 18;
        _btnTiposPago.Name = "_btnTiposPago";
        _btnTiposPago.RibbonStyle = RibbonItemStyles.Large;
        _btnTiposPago.ItemClick += BtnTiposPago_ItemClick;
        //
        // _btnTiposCorte
        //
        _btnTiposCorte.Id = 13;
        _btnTiposCorte.Name = "_btnTiposCorte";
        _btnTiposCorte.RibbonStyle = RibbonItemStyles.Large;
        _btnTiposCorte.ItemClick += BtnTiposCorte_ItemClick;
        //
        // _btnJefesAcopio
        //
        _btnJefesAcopio.Id = 21;
        _btnJefesAcopio.Name = "_btnJefesAcopio";
        _btnJefesAcopio.RibbonStyle = RibbonItemStyles.Large;
        _btnJefesAcopio.ItemClick += BtnJefesAcopio_ItemClick;
        //
        // _btnFloraciones
        //
        _btnFloraciones.Id = 24;
        _btnFloraciones.Name = "_btnFloraciones";
        _btnFloraciones.RibbonStyle = RibbonItemStyles.Large;
        _btnFloraciones.ItemClick += BtnFloraciones_ItemClick;
        //
        // _btnRecepcionesFruta
        //
        _btnRecepcionesFruta.Id = 26;
        _btnRecepcionesFruta.Name = "_btnRecepcionesFruta";
        _btnRecepcionesFruta.RibbonStyle = RibbonItemStyles.Large;
        _btnRecepcionesFruta.ItemClick += BtnRecepcionesFruta_ItemClick;
        //
        // _btnAcuerdosCorte
        //
        _btnAcuerdosCorte.Id = 14;
        _btnAcuerdosCorte.Name = "_btnAcuerdosCorte";
        _btnAcuerdosCorte.RibbonStyle = RibbonItemStyles.Large;
        _btnAcuerdosCorte.ItemClick += BtnAcuerdosCorte_ItemClick;
        //
        // _btnOrdenesCorte
        //
        _btnOrdenesCorte.Id = 23;
        _btnOrdenesCorte.Name = "_btnOrdenesCorte";
        _btnOrdenesCorte.RibbonStyle = RibbonItemStyles.Large;
        _btnOrdenesCorte.ItemClick += BtnOrdenesCorte_ItemClick;
        //
        // _btnZonas
        //
        _btnZonas.Id = 19;
        _btnZonas.Name = "_btnZonas";
        _btnZonas.RibbonStyle = RibbonItemStyles.Large;
        _btnZonas.ItemClick += BtnZonas_ItemClick;
        //
        // _btnListaPrecioAcarreo
        //
        _btnListaPrecioAcarreo.Id = 20;
        _btnListaPrecioAcarreo.Name = "_btnListaPrecioAcarreo";
        _btnListaPrecioAcarreo.RibbonStyle = RibbonItemStyles.Large;
        _btnListaPrecioAcarreo.ItemClick += BtnListaPrecioAcarreo_ItemClick;
        //
        // _btnListaPrecioCorte
        //
        _btnListaPrecioCorte.Id = 22;
        _btnListaPrecioCorte.Name = "_btnListaPrecioCorte";
        _btnListaPrecioCorte.RibbonStyle = RibbonItemStyles.Large;
        _btnListaPrecioCorte.ItemClick += BtnListaPrecioCorte_ItemClick;
        //
        // _btnUsuarios
        //
        _btnUsuarios.Id = 5;
        _btnUsuarios.Name = "_btnUsuarios";
        _btnUsuarios.RibbonStyle = RibbonItemStyles.Large;
        _btnUsuarios.ItemClick += BtnUsuarios_ItemClick;
        //
        // _btnRoles
        //
        _btnRoles.Id = 6;
        _btnRoles.Name = "_btnRoles";
        _btnRoles.RibbonStyle = RibbonItemStyles.Large;
        _btnRoles.ItemClick += BtnRoles_ItemClick;
        //
        // _btnPermisos
        //
        _btnPermisos.Id = 7;
        _btnPermisos.Name = "_btnPermisos";
        _btnPermisos.RibbonStyle = RibbonItemStyles.Large;
        _btnPermisos.ItemClick += BtnPermisos_ItemClick;
        //
        // _btnConfiguracionConexiones
        //
        _btnConfiguracionConexiones.Id = 8;
        _btnConfiguracionConexiones.Name = "_btnConfiguracionConexiones";
        _btnConfiguracionConexiones.RibbonStyle = RibbonItemStyles.Large;
        _btnConfiguracionConexiones.ItemClick += BtnConfiguracionConexiones_ItemClick;
        //
        // _btnConfiguracionEmpresa
        //
        _btnConfiguracionEmpresa.Id = 25;
        _btnConfiguracionEmpresa.Name = "_btnConfiguracionEmpresa";
        _btnConfiguracionEmpresa.RibbonStyle = RibbonItemStyles.Large;
        _btnConfiguracionEmpresa.ItemClick += BtnConfiguracionEmpresa_ItemClick;
        //
        // _btnSalir
        //
        _btnSalir.Id = 9;
        _btnSalir.Name = "_btnSalir";
        _btnSalir.RibbonStyle = RibbonItemStyles.Large;
        _btnSalir.ItemClick += BtnSalir_ItemClick;
        //
        // _staticUsuario
        //
        _staticUsuario.Id = 10;
        _staticUsuario.Name = "_staticUsuario";
        //
        // _grpUbicaciones
        //
        _grpUbicaciones.ItemLinks.Add(_btnPaises);
        _grpUbicaciones.ItemLinks.Add(_btnEstados);
        _grpUbicaciones.ItemLinks.Add(_btnMunicipios);
        _grpUbicaciones.Name = "_grpUbicaciones";
        _grpUbicaciones.Text = "Ubicaciones";
        //
        // _grpSociosNegocio
        //
        _grpSociosNegocio.ItemLinks.Add(_btnProductores);
        _grpSociosNegocio.ItemLinks.Add(_btnHuertas);
        _grpSociosNegocio.ItemLinks.Add(_btnFloraciones);
        _grpSociosNegocio.Name = "_grpSociosNegocio";
        _grpSociosNegocio.Text = "Huerta - Campo";
        //
        // _grpUsuariosRoles
        //
        _grpUsuariosRoles.ItemLinks.Add(_btnUsuarios);
        _grpUsuariosRoles.ItemLinks.Add(_btnRoles);
        _grpUsuariosRoles.ItemLinks.Add(_btnPermisos);
        _grpUsuariosRoles.Name = "_grpUsuariosRoles";
        _grpUsuariosRoles.Text = "Usuarios y Roles";
        //
        // _grpConfiguracion
        //
        _grpConfiguracion.ItemLinks.Add(_btnConfiguracionConexiones);
        _grpConfiguracion.ItemLinks.Add(_btnConfiguracionEmpresa);
        _grpConfiguracion.Name = "_grpConfiguracion";
        _grpConfiguracion.Text = "Configuración";
        //
        // _grpAplicacion
        //
        _grpAplicacion.ItemLinks.Add(_btnSalir);
        _grpAplicacion.Name = "_grpAplicacion";
        _grpAplicacion.Text = "Aplicación";
        //
        // _grpPreciosFruta
        //
        _grpPreciosFruta.ItemLinks.Add(_btnListaPrecioFruta);
        _grpPreciosFruta.ItemLinks.Add(_btnAcuerdosCorte);
        _grpPreciosFruta.Name = "_grpPreciosFruta";
        _grpPreciosFruta.Text = "Precios de Fruta";
        //
        // _grpOrdenesCorte
        //
        _grpOrdenesCorte.ItemLinks.Add(_btnOrdenesCorte);
        _grpOrdenesCorte.Name = "_grpOrdenesCorte";
        _grpOrdenesCorte.Text = "Órdenes de Corte";
        //
        // _grpCatalogosAcopio
        //
        _grpCatalogosAcopio.ItemLinks.Add(_btnVariedades);
        _grpCatalogosAcopio.ItemLinks.Add(_btnTiposComercializacion);
        _grpCatalogosAcopio.ItemLinks.Add(_btnMonedas);
        _grpCatalogosAcopio.ItemLinks.Add(_btnTiposPago);
        _grpCatalogosAcopio.ItemLinks.Add(_btnTiposCorte);
        _grpCatalogosAcopio.ItemLinks.Add(_btnJefesAcopio);
        _grpCatalogosAcopio.Name = "_grpCatalogosAcopio";
        _grpCatalogosAcopio.Text = "Acopio";
        //
        // _grpCatalogosAcarreo
        //
        _grpCatalogosAcarreo.ItemLinks.Add(_btnZonas);
        _grpCatalogosAcarreo.Name = "_grpCatalogosAcarreo";
        _grpCatalogosAcarreo.Text = "Acarreo";
        //
        // _pageCatalogos
        //
        _pageCatalogos.Groups.AddRange(new RibbonPageGroup[] { _grpUbicaciones, _grpSociosNegocio, _grpCatalogosAcopio, _grpCatalogosAcarreo });
        _pageCatalogos.Name = "_pageCatalogos";
        _pageCatalogos.Text = "Catálogos";
        //
        // _grpPreciosAcarreo
        //
        _grpPreciosAcarreo.ItemLinks.Add(_btnListaPrecioAcarreo);
        _grpPreciosAcarreo.Name = "_grpPreciosAcarreo";
        _grpPreciosAcarreo.Text = "Precios de Acarreo";
        //
        // _grpPreciosCorte
        //
        _grpPreciosCorte.ItemLinks.Add(_btnListaPrecioCorte);
        _grpPreciosCorte.Name = "_grpPreciosCorte";
        _grpPreciosCorte.Text = "Precios de Corte";
        //
        // _pageAcopio
        //
        _pageAcopio.Groups.AddRange(new RibbonPageGroup[] { _grpPreciosFruta, _grpPreciosAcarreo, _grpPreciosCorte, _grpOrdenesCorte });
        _pageAcopio.Name = "_pageAcopio";
        _pageAcopio.Text = "Acopio";
        //
        // _grpRecepcionFruta
        //
        _grpRecepcionFruta.ItemLinks.Add(_btnRecepcionesFruta);
        _grpRecepcionFruta.Name = "_grpRecepcionFruta";
        _grpRecepcionFruta.Text = "Recepción de Fruta";
        //
        // _pageRecepcion
        //
        _pageRecepcion.Groups.AddRange(new RibbonPageGroup[] { _grpRecepcionFruta });
        _pageRecepcion.Name = "_pageRecepcion";
        _pageRecepcion.Text = "Recepción";
        //
        // _pageSeguridad
        //
        _pageSeguridad.Groups.AddRange(new RibbonPageGroup[] { _grpUsuariosRoles });
        _pageSeguridad.Name = "_pageSeguridad";
        _pageSeguridad.Text = "Seguridad";
        //
        // _pageSistema
        //
        _pageSistema.Groups.AddRange(new RibbonPageGroup[] { _grpConfiguracion, _grpAplicacion });
        _pageSistema.Name = "_pageSistema";
        _pageSistema.Text = "Sistema";
        //
        // _ribbon
        //
        _ribbon.Location = new Point(0, 0);
        _ribbon.MaxItemId = 26;
        _ribbon.Name = "_ribbon";
        _ribbon.Pages.AddRange(new RibbonPage[] { _pageCatalogos, _pageAcopio, _pageRecepcion, _pageSeguridad, _pageSistema });
        _ribbon.Size = new Size(900, 158);
        //
        // _statusBar
        //
        _statusBar.ItemLinks.Add(_staticUsuario);
        _statusBar.Location = new Point(0, 553);
        _statusBar.Name = "_statusBar";
        _statusBar.Ribbon = _ribbon;
        _statusBar.Size = new Size(900, 31);
        //
        // _tabbedMdiManager
        //
        _tabbedMdiManager.MdiParent = this;
        //
        // MainForm
        //
        ClientSize = new Size(900, 584);
        Controls.Add(_statusBar);
        Controls.Add(_ribbon);
        IsMdiContainer = true;
        Name = "MainForm";
        Ribbon = _ribbon;
        StatusBar = _statusBar;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FrontOne";
        WindowState = FormWindowState.Maximized;
        ((System.ComponentModel.ISupportInitialize)_ribbon).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
