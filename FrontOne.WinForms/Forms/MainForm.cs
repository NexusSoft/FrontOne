using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using FrontOne.Application.Services;
using FrontOne.Shared.Configuration;
using FrontOne.WinForms.Forms.Acarreo;
using FrontOne.WinForms.Forms.Acopio;
using FrontOne.WinForms.Forms.Almacenes;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Forms.Corridas;
using FrontOne.WinForms.Forms.Lotes;
using FrontOne.WinForms.Forms.Pallets;
using FrontOne.WinForms.Forms.Recepcion;
using FrontOne.WinForms.Forms.Seguridad;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Session;
using Microsoft.Extensions.Options;

namespace FrontOne.WinForms.Forms;

public partial class MainForm : RibbonForm
{
    private const string ModuloCatalogos = "Catalogos";
    private const string ModuloAcopio = "Acopio";
    private const string ModuloAcarreo = "Acarreo";
    private const string ModuloRecepcion = "Recepcion";
    private const string ModuloLotes = "Lotes";
    private const string ModuloCorridas = "Corridas";
    private const string ModuloPallets = "Pallets";
    private const string ModuloEtiquetado = "Etiquetado";
    private const string ModuloSeguridad = "Seguridad";
    private const string ModuloAlmacenes = "Almacenes";
    private const string AccionConsultar = "Consultar";

    private readonly SessionContext _sessionContext = null!;
    private readonly ConnectionSettingsService _connectionSettingsService = null!;
    private readonly PaisService _paisService = null!;
    private readonly EstadoService _estadoService = null!;
    private readonly MunicipioService _municipioService = null!;
    private readonly ProductorService _productorService = null!;
    private readonly HuertaService _huertaService = null!;
    private readonly PoblacionService _poblacionService = null!;
    private readonly ProductoService _productoService = null!;
    private readonly SistemaRiegoService _sistemaRiegoService = null!;
    private readonly StatusHuertaService _statusHuertaService = null!;
    private readonly ListaPrecioFrutaService _listaPrecioFrutaService = null!;
    private readonly TipoCorteService _tipoCorteService = null!;
    private readonly TipoPagoService _tipoPagoService = null!;
    private readonly VariedadService _variedadService = null!;
    private readonly TipoComercializacionService _tipoComercializacionService = null!;
    private readonly MonedaService _monedaService = null!;
    private readonly AcuerdoCorteService _acuerdoCorteService = null!;
    private readonly ZonaService _zonaService = null!;
    private readonly ListaPrecioAcarreoService _listaPrecioAcarreoService = null!;
    private readonly ListaPrecioCorteService _listaPrecioCorteService = null!;
    private readonly OrdenCorteService _ordenCorteService = null!;
    private readonly FloracionService _floracionService = null!;
    private readonly JefeAcopioService _jefeAcopioService = null!;
    private readonly RecepcionFrutaService _recepcionFrutaService = null!;
    private readonly ReportePlantillaService _reportePlantillaService = null!;
    private readonly LoteService _loteService = null!;
    private readonly CorridaService _corridaService = null!;
    private readonly PalletService _palletService = null!;
    private readonly ConfiguracionBasculaService _configuracionBasculaService = null!;
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly CajaCampoService _cajaCampoService = null!;
    private readonly ProductoTerminadoService _productoTerminadoService = null!;
    private readonly CategoriaService _categoriaService = null!;
    private readonly TipoProductoService _tipoProductoService = null!;
    private readonly CalibreApeamService _calibreApeamService = null!;
    private readonly MarcaService _marcaService = null!;
    private readonly PesoEstandarService _pesoEstandarService = null!;
    private readonly UsuarioService _usuarioService = null!;
    private readonly RolService _rolService = null!;
    private readonly PermisoService _permisoService = null!;
    private readonly ReportePermisoService _reportePermisoService = null!;
    private readonly MovilPermisoService _movilPermisoService = null!;
    private readonly SqlOptions _sqlOptions = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;
    private readonly LicenciaTecitService _licenciaTecitService = null!;
    private readonly MovimientoAlmacenService _movimientoAlmacenService = null!;
    private readonly SupervisorHuertaService _supervisorHuertaService = null!;
    private readonly IncidenciaService _incidenciaService = null!;
    private readonly EtiquetaService _etiquetaService = null!;

    private ProductorEditarForm? _productorEditarForm;
    private JefeAcopioEditarForm? _jefeAcopioEditarForm;
    private HuertaEditarForm? _huertaEditarForm;
    private ListaPrecioFrutaForm? _listaPrecioFrutaForm;
    private AcuerdosCorteForm? _acuerdosCorteForm;
    private OrdenesCorteForm? _ordenesCorteForm;
    private ListaPrecioAcarreoForm? _listaPrecioAcarreoForm;
    private ListaPrecioCorteForm? _listaPrecioCorteForm;
    private RecepcionesFrutaForm? _recepcionesFrutaForm;
    private LotesForm? _lotesForm;
    private CorridasForm? _corridasForm;
    private PalletsForm? _palletsForm;
    private FrontOne.WinForms.Forms.Etiquetado.EtiquetasForm? _etiquetasForm;
    private ProductosTerminadosForm? _productosTerminadosForm;
    private IncidenciasForm? _incidenciasForm;

    public MainForm()
    {
        InitializeComponent();
    }

    public MainForm(
        SessionContext sessionContext,
        ConnectionSettingsService connectionSettingsService,
        PaisService paisService,
        EstadoService estadoService,
        MunicipioService municipioService,
        ProductorService productorService,
        HuertaService huertaService,
        PoblacionService poblacionService,
        ProductoService productoService,
        SistemaRiegoService sistemaRiegoService,
        StatusHuertaService statusHuertaService,
        ListaPrecioFrutaService listaPrecioFrutaService,
        TipoCorteService tipoCorteService,
        TipoPagoService tipoPagoService,
        VariedadService variedadService,
        TipoComercializacionService tipoComercializacionService,
        MonedaService monedaService,
        AcuerdoCorteService acuerdoCorteService,
        ZonaService zonaService,
        ListaPrecioAcarreoService listaPrecioAcarreoService,
        ListaPrecioCorteService listaPrecioCorteService,
        OrdenCorteService ordenCorteService,
        FloracionService floracionService,
        JefeAcopioService jefeAcopioService,
        RecepcionFrutaService recepcionFrutaService,
        ReportePlantillaService reportePlantillaService,
        LoteService loteService,
        CorridaService corridaService,
        PalletService palletService,
        ConfiguracionBasculaService configuracionBasculaService,
        LineaProduccionService lineaProduccionService,
        CajaCampoService cajaCampoService,
        ProductoTerminadoService productoTerminadoService,
        CategoriaService categoriaService,
        TipoProductoService tipoProductoService,
        CalibreApeamService calibreApeamService,
        MarcaService marcaService,
        PesoEstandarService pesoEstandarService,
        UsuarioService usuarioService,
        RolService rolService,
        PermisoService permisoService,
        EmpresaConfiguracionService empresaConfiguracionService,
        LicenciaTecitService licenciaTecitService,
        ReportePermisoService reportePermisoService,
        MovilPermisoService movilPermisoService,
        MovimientoAlmacenService movimientoAlmacenService,
        SupervisorHuertaService supervisorHuertaService,
        IncidenciaService incidenciaService,
        EtiquetaService etiquetaService,
        IOptions<SqlOptions> sqlOptions)
        : this()
    {
        _sessionContext = sessionContext;
        _connectionSettingsService = connectionSettingsService;
        _paisService = paisService;
        _estadoService = estadoService;
        _municipioService = municipioService;
        _productorService = productorService;
        _huertaService = huertaService;
        _poblacionService = poblacionService;
        _productoService = productoService;
        _sistemaRiegoService = sistemaRiegoService;
        _statusHuertaService = statusHuertaService;
        _listaPrecioFrutaService = listaPrecioFrutaService;
        _tipoCorteService = tipoCorteService;
        _tipoPagoService = tipoPagoService;
        _variedadService = variedadService;
        _tipoComercializacionService = tipoComercializacionService;
        _monedaService = monedaService;
        _acuerdoCorteService = acuerdoCorteService;
        _zonaService = zonaService;
        _listaPrecioAcarreoService = listaPrecioAcarreoService;
        _listaPrecioCorteService = listaPrecioCorteService;
        _ordenCorteService = ordenCorteService;
        _floracionService = floracionService;
        _jefeAcopioService = jefeAcopioService;
        _recepcionFrutaService = recepcionFrutaService;
        _reportePlantillaService = reportePlantillaService;
        _loteService = loteService;
        _corridaService = corridaService;
        _palletService = palletService;
        _configuracionBasculaService = configuracionBasculaService;
        _lineaProduccionService = lineaProduccionService;
        _cajaCampoService = cajaCampoService;
        _productoTerminadoService = productoTerminadoService;
        _categoriaService = categoriaService;
        _tipoProductoService = tipoProductoService;
        _calibreApeamService = calibreApeamService;
        _marcaService = marcaService;
        _pesoEstandarService = pesoEstandarService;
        _usuarioService = usuarioService;
        _rolService = rolService;
        _permisoService = permisoService;
        _empresaConfiguracionService = empresaConfiguracionService;
        _licenciaTecitService = licenciaTecitService;
        _reportePermisoService = reportePermisoService;
        _movilPermisoService = movilPermisoService;
        _movimientoAlmacenService = movimientoAlmacenService;
        _supervisorHuertaService = supervisorHuertaService;
        _incidenciaService = incidenciaService;
        _etiquetaService = etiquetaService;
        _sqlOptions = sqlOptions.Value;

        var dbSql = _connectionSettingsService.GetSqlCredentials()?.Database ?? "(no configurado)";
        var dbHana = _connectionSettingsService.GetSapCredentials()?.CompanyDb ?? "(no configurado)";
        _staticUsuario.Caption = $"[Usuario: {_sessionContext.UsuarioActual?.NombreCompleto}] [DB SQL: {dbSql}] [DB HANA: {dbHana}]";

        AplicarPermisos();
    }

    private void AplicarPermisos()
    {
        _btnPaises.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "Paises", AccionConsultar);
        _btnEstados.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "Estados", AccionConsultar);
        _btnMunicipios.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "Municipios", AccionConsultar);
        _btnProductores.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "Productores", AccionConsultar);
        _btnHuertas.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "Huertas", AccionConsultar);
        _btnListaPrecioFruta.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "ListaPrecioFruta", AccionConsultar);
        _btnVariedades.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "Variedades", AccionConsultar);
        _btnTiposComercializacion.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "TiposComercializacion", AccionConsultar);
        _btnMonedas.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "Monedas", AccionConsultar);
        _btnTiposPago.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "TiposPago", AccionConsultar);
        _btnTiposCorte.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "TiposCorte", AccionConsultar);
        _btnJefesAcopio.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "JefesAcopio", AccionConsultar);
        _btnZonas.Enabled = _sessionContext.TienePermiso(ModuloAcarreo, "Zonas", AccionConsultar);
        _btnListaPrecioAcarreo.Enabled = _sessionContext.TienePermiso(ModuloAcarreo, "ListaPrecioAcarreo", AccionConsultar);
        _btnListaPrecioCorte.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "ListaPrecioCorte", AccionConsultar);
        _btnAcuerdosCorte.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "AcuerdosCorte", AccionConsultar);
        _btnOrdenesCorte.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "OrdenesCorte", AccionConsultar);
        _btnFloraciones.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "Floraciones", AccionConsultar);
        _btnRecepcionesFruta.Enabled = _sessionContext.TienePermiso(ModuloRecepcion, "RecepcionesFruta", AccionConsultar);
        _btnProductosTerminados.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "ProductosTerminados", AccionConsultar);
        _btnLotes.Enabled = _sessionContext.TienePermiso(ModuloLotes, "Lotes", AccionConsultar);
        _btnCorridas.Enabled = _sessionContext.TienePermiso(ModuloCorridas, "Corridas", AccionConsultar);
        _btnPallets.Enabled = _sessionContext.TienePermiso(ModuloPallets, "Pallets", AccionConsultar);
        _btnEtiquetas.Enabled = _sessionContext.TienePermiso(ModuloEtiquetado, "Etiquetas", AccionConsultar);
        _btnConfiguracionBascula.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "ConfiguracionBascula", AccionConsultar);
        _btnLineasProduccion.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "LineasProduccion", AccionConsultar);
        _btnCajasCampo.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "CajasCampo", AccionConsultar);
        _btnUsuarios.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Usuarios", AccionConsultar);
        _btnRoles.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Roles", AccionConsultar);
        _btnPermisos.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Permisos", AccionConsultar);
        _btnReportePermisos.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Permisos", AccionConsultar);
        _btnConfiguracionEmpresa.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "ConfiguracionEmpresa", AccionConsultar);
        _btnLicenciaTecit.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "LicenciaTecit", AccionConsultar);
        _btnReportes.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "DisenadorReportes", AccionConsultar);
        _btnAlmacenCajaCampo.Enabled = _sessionContext.TienePermiso(ModuloAlmacenes, "AlmacenCajaCampo", AccionConsultar);
        _btnSupervisoresHuerta.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "SupervisoresHuerta", AccionConsultar);
        _btnIncidencias.Enabled = _sessionContext.TienePermiso(ModuloAcopio, "Incidencias", AccionConsultar);
    }

    private void BtnPaises_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new PaisesForm(_paisService);
        form.ShowDialog(this);
    }

    private void BtnEstados_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new EstadosForm(_estadoService, _paisService);
        form.ShowDialog(this);
    }

    private void BtnMunicipios_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new MunicipiosForm(_municipioService, _paisService, _estadoService);
        form.ShowDialog(this);
    }

    private void BtnProductores_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_productorEditarForm is { IsDisposed: false })
        {
            _productorEditarForm.Activate();
            return;
        }

        _productorEditarForm = new ProductorEditarForm(_productorService, _paisService, _estadoService, _municipioService, _huertaService, _poblacionService)
        {
            MdiParent = this,
        };
        _productorEditarForm.FormClosed += (_, _) => _productorEditarForm = null;
        _productorEditarForm.Show();
    }

    private void BtnHuertas_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_huertaEditarForm is { IsDisposed: false })
        {
            _huertaEditarForm.Activate();
            return;
        }

        _huertaEditarForm = new HuertaEditarForm(
            _huertaService,
            _productorService,
            _paisService,
            _estadoService,
            _municipioService,
            _poblacionService,
            _productoService,
            _sistemaRiegoService,
            _statusHuertaService,
            _sessionContext)
        {
            MdiParent = this,
        };
        _huertaEditarForm.FormClosed += (_, _) => _huertaEditarForm = null;
        _huertaEditarForm.Show();
    }

    private void BtnListaPrecioFruta_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_listaPrecioFrutaForm is { IsDisposed: false })
        {
            _listaPrecioFrutaForm.Activate();
            return;
        }

        _listaPrecioFrutaForm = new ListaPrecioFrutaForm(_listaPrecioFrutaService, _productorService, _variedadService, _paisService, _estadoService)
        {
            MdiParent = this,
        };
        _listaPrecioFrutaForm.FormClosed += (_, _) => _listaPrecioFrutaForm = null;
        _listaPrecioFrutaForm.Show();
    }

    private void BtnVariedades_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new VariedadesForm(_variedadService);
        form.ShowDialog(this);
    }

    private void BtnProductosTerminados_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_productosTerminadosForm is { IsDisposed: false })
        {
            _productosTerminadosForm.Activate();
            return;
        }

        _productosTerminadosForm = new ProductosTerminadosForm(
            _productoTerminadoService, _categoriaService, _tipoProductoService, _calibreApeamService,
            _marcaService, _pesoEstandarService, _paisService, _variedadService, _sessionContext)
        {
            MdiParent = this,
        };
        _productosTerminadosForm.FormClosed += (_, _) => _productosTerminadosForm = null;
        _productosTerminadosForm.Show();
    }

    private void BtnTiposComercializacion_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new TiposComercializacionForm(_tipoComercializacionService);
        form.ShowDialog(this);
    }

    private void BtnMonedas_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new MonedasForm(_monedaService);
        form.ShowDialog(this);
    }

    private void BtnTiposPago_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new TiposPagoForm(_tipoPagoService);
        form.ShowDialog(this);
    }

    private void BtnSupervisoresHuerta_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new SupervisoresHuertaForm(_supervisorHuertaService);
        form.ShowDialog(this);
    }

    private void BtnTiposCorte_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new TiposCorteForm(_tipoCorteService, _tipoPagoService);
        form.ShowDialog(this);
    }

    private void BtnJefesAcopio_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_jefeAcopioEditarForm is { IsDisposed: false })
        {
            _jefeAcopioEditarForm.Activate();
            return;
        }

        _jefeAcopioEditarForm = new JefeAcopioEditarForm(_jefeAcopioService, _paisService, _estadoService, _municipioService, _poblacionService)
        {
            MdiParent = this,
        };
        _jefeAcopioEditarForm.FormClosed += (_, _) => _jefeAcopioEditarForm = null;
        _jefeAcopioEditarForm.Show();
    }

    private void BtnAcuerdosCorte_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_acuerdosCorteForm is { IsDisposed: false })
        {
            _acuerdosCorteForm.Activate();
            return;
        }

        _acuerdosCorteForm = new AcuerdosCorteForm(
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService)
        {
            MdiParent = this,
        };
        _acuerdosCorteForm.FormClosed += (_, _) => _acuerdosCorteForm = null;
        _acuerdosCorteForm.Show();
    }

    private void BtnOrdenesCorte_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_ordenesCorteForm is { IsDisposed: false })
        {
            _ordenesCorteForm.Activate();
            return;
        }

        _ordenesCorteForm = new OrdenesCorteForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService, _cajaCampoService,
            _acuerdoCorteService, _productorService, _productoService, _tipoComercializacionService,
            _tipoPagoService, _monedaService, _listaPrecioFrutaService, _sessionContext)
        {
            MdiParent = this,
        };
        _ordenesCorteForm.FormClosed += (_, _) => _ordenesCorteForm = null;
        _ordenesCorteForm.Show();
    }

    private void BtnIncidencias_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_incidenciasForm is { IsDisposed: false })
        {
            _incidenciasForm.Activate();
            return;
        }

        _incidenciasForm = new IncidenciasForm(
            _incidenciaService, _supervisorHuertaService, _reportePlantillaService, _empresaConfiguracionService, _sessionContext)
        {
            MdiParent = this,
        };
        _incidenciasForm.FormClosed += (_, _) => _incidenciasForm = null;
        _incidenciasForm.Show();
    }

    private void BtnZonas_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ZonasForm(_zonaService);
        form.ShowDialog(this);
    }

    private void BtnListaPrecioAcarreo_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_listaPrecioAcarreoForm is { IsDisposed: false })
        {
            _listaPrecioAcarreoForm.Activate();
            return;
        }

        _listaPrecioAcarreoForm = new ListaPrecioAcarreoForm(_listaPrecioAcarreoService, _municipioService, _paisService, _estadoService, _zonaService)
        {
            MdiParent = this,
        };
        _listaPrecioAcarreoForm.FormClosed += (_, _) => _listaPrecioAcarreoForm = null;
        _listaPrecioAcarreoForm.Show();
    }

    private void BtnListaPrecioCorte_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_listaPrecioCorteForm is { IsDisposed: false })
        {
            _listaPrecioCorteForm.Activate();
            return;
        }

        _listaPrecioCorteForm = new ListaPrecioCorteForm(_listaPrecioCorteService)
        {
            MdiParent = this,
        };
        _listaPrecioCorteForm.FormClosed += (_, _) => _listaPrecioCorteForm = null;
        _listaPrecioCorteForm.Show();
    }

    private void BtnFloraciones_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new FloracionesForm(_floracionService);
        form.ShowDialog(this);
    }

    private void BtnRecepcionesFruta_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_recepcionesFrutaForm is { IsDisposed: false })
        {
            _recepcionesFrutaForm.Activate();
            return;
        }

        _recepcionesFrutaForm = new RecepcionesFrutaForm(
            _recepcionFrutaService, _reportePlantillaService, _empresaConfiguracionService, _sessionContext, _sqlOptions,
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService,
            _ordenCorteService, _huertaService, _floracionService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _municipioService, _poblacionService,
            _loteService, _lineaProduccionService, _cajaCampoService)
        {
            MdiParent = this,
        };
        _recepcionesFrutaForm.FormClosed += (_, _) => _recepcionesFrutaForm = null;
        _recepcionesFrutaForm.Show();
    }

    private void BtnLotes_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_lotesForm is { IsDisposed: false })
        {
            _lotesForm.Activate();
            return;
        }

        _lotesForm = new LotesForm(
            _loteService, _lineaProduccionService, _sessionContext,
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService,
            _ordenCorteService, _huertaService, _floracionService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _municipioService, _poblacionService,
            _recepcionFrutaService, _cajaCampoService)
        {
            MdiParent = this,
        };
        _lotesForm.FormClosed += (_, _) => _lotesForm = null;
        _lotesForm.Show();
    }

    private void BtnCorridas_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_corridasForm is { IsDisposed: false })
        {
            _corridasForm.Activate();
            return;
        }

        _corridasForm = new CorridasForm(_corridaService, _palletService, _productoTerminadoService, _sessionContext)
        {
            MdiParent = this,
        };
        _corridasForm.FormClosed += (_, _) => _corridasForm = null;
        _corridasForm.Show();
    }

    private void BtnPallets_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_palletsForm is { IsDisposed: false })
        {
            _palletsForm.Activate();
            return;
        }

        _palletsForm = new PalletsForm(
            _palletService, _lineaProduccionService, _productoTerminadoService,
            _categoriaService, _tipoProductoService, _calibreApeamService, _marcaService,
            _pesoEstandarService, _paisService, _variedadService,
            _configuracionBasculaService, _reportePlantillaService, _empresaConfiguracionService,
            _sessionContext, _sqlOptions)
        {
            MdiParent = this,
        };
        _palletsForm.FormClosed += (_, _) => _palletsForm = null;
        _palletsForm.Show();
    }

    private void BtnEtiquetas_ItemClick(object? sender, ItemClickEventArgs e)
    {
        if (_etiquetasForm is { IsDisposed: false })
        {
            _etiquetasForm.Activate();
            return;
        }

        _etiquetasForm = new FrontOne.WinForms.Forms.Etiquetado.EtiquetasForm(
            _etiquetaService, _palletService, _empresaConfiguracionService, _sessionContext, _sqlOptions, _licenciaTecitService)
        {
            MdiParent = this,
        };
        _etiquetasForm.FormClosed += (_, _) => _etiquetasForm = null;
        _etiquetasForm.Show();
    }

    private void BtnConfiguracionBascula_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ConfiguracionBasculaForm(_configuracionBasculaService);
        form.ShowDialog(this);
    }

    private void BtnLineasProduccion_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new LineasProduccionForm(_lineaProduccionService);
        form.ShowDialog(this);
    }

    private void BtnCajasCampo_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new CajasCampoForm(_cajaCampoService);
        form.ShowDialog(this);
    }

    private void BtnAlmacenCajaCampo_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new AlmacenCajaCampoDashboardForm(_movimientoAlmacenService, _cajaCampoService);
        form.ShowDialog(this);
    }

    private void BtnUsuarios_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new UsuariosForm(_usuarioService, _rolService);
        form.ShowDialog(this);
    }

    private void BtnRoles_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new RolesForm(_rolService);
        form.ShowDialog(this);
    }

    private void BtnPermisos_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new PermisosForm(_rolService, _permisoService);
        form.ShowDialog(this);
    }

    private void BtnReportePermisos_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ReportePermisosForm(_rolService, _reportePermisoService);
        form.ShowDialog(this);
    }

    private void BtnPermisosAplicacionMovil_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new PermisosAplicacionMovilForm(_rolService, _movilPermisoService);
        form.ShowDialog(this);
    }

    private void BtnConfiguracionConexiones_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ConfiguracionConexionesForm(_connectionSettingsService);
        form.ShowDialog(this);
    }

    private void BtnConfiguracionEmpresa_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ConfiguracionEmpresaForm(_empresaConfiguracionService);
        form.ShowDialog(this);
    }

    private void BtnLicenciaTecit_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ConfiguracionLicenciaTecitForm(_licenciaTecitService);
        form.ShowDialog(this);
    }

    private void BtnReportes_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ReportesForm(_reportePlantillaService, _sessionContext, _sqlOptions, _licenciaTecitService);
        form.ShowDialog(this);
    }

    private void BtnSalir_ItemClick(object? sender, ItemClickEventArgs e) => Close();
}
