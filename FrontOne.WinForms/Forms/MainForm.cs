using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using FrontOne.Application.Services;
using FrontOne.WinForms.Forms.Acarreo;
using FrontOne.WinForms.Forms.Acopio;
using FrontOne.WinForms.Forms.Catalogos;
using FrontOne.WinForms.Forms.Lotes;
using FrontOne.WinForms.Forms.Recepcion;
using FrontOne.WinForms.Forms.Seguridad;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Session;

namespace FrontOne.WinForms.Forms;

public partial class MainForm : RibbonForm
{
    private const string ModuloCatalogos = "Catalogos";
    private const string ModuloAcopio = "Acopio";
    private const string ModuloAcarreo = "Acarreo";
    private const string ModuloRecepcion = "Recepcion";
    private const string ModuloLotes = "Lotes";
    private const string ModuloSeguridad = "Seguridad";
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
    private readonly LineaProduccionService _lineaProduccionService = null!;
    private readonly UsuarioService _usuarioService = null!;
    private readonly RolService _rolService = null!;
    private readonly PermisoService _permisoService = null!;
    private readonly EmpresaConfiguracionService _empresaConfiguracionService = null!;

    private ProductorEditarForm? _productorEditarForm;
    private JefeAcopioEditarForm? _jefeAcopioEditarForm;
    private HuertaEditarForm? _huertaEditarForm;

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
        LineaProduccionService lineaProduccionService,
        UsuarioService usuarioService,
        RolService rolService,
        PermisoService permisoService,
        EmpresaConfiguracionService empresaConfiguracionService)
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
        _lineaProduccionService = lineaProduccionService;
        _usuarioService = usuarioService;
        _rolService = rolService;
        _permisoService = permisoService;
        _empresaConfiguracionService = empresaConfiguracionService;

        _staticUsuario.Caption = $"Usuario: {_sessionContext.UsuarioActual?.NombreCompleto}";

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
        _btnLotes.Enabled = _sessionContext.TienePermiso(ModuloLotes, "Lotes", AccionConsultar);
        _btnLineasProduccion.Enabled = _sessionContext.TienePermiso(ModuloCatalogos, "LineasProduccion", AccionConsultar);
        _btnUsuarios.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Usuarios", AccionConsultar);
        _btnRoles.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Roles", AccionConsultar);
        _btnPermisos.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "Permisos", AccionConsultar);
        _btnConfiguracionEmpresa.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "ConfiguracionEmpresa", AccionConsultar);
        _btnReportes.Enabled = _sessionContext.TienePermiso(ModuloSeguridad, "DisenadorReportes", AccionConsultar);
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
        using var form = new ListaPrecioFrutaForm(_listaPrecioFrutaService, _productorService, _variedadService, _paisService, _estadoService);
        form.ShowDialog(this);
    }

    private void BtnVariedades_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new VariedadesForm(_variedadService);
        form.ShowDialog(this);
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
        using var form = new AcuerdosCorteForm(
            _acuerdoCorteService, _productorService, _paisService, _estadoService, _productoService,
            _variedadService, _tipoComercializacionService, _tipoCorteService, _tipoPagoService,
            _monedaService, _listaPrecioFrutaService);
        form.ShowDialog(this);
    }

    private void BtnOrdenesCorte_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new OrdenesCorteForm(
            _ordenCorteService, _huertaService, _floracionService, _variedadService, _listaPrecioAcarreoService, _zonaService,
            _listaPrecioCorteService, _jefeAcopioService, _tipoCorteService,
            _paisService, _estadoService, _municipioService, _poblacionService);
        form.ShowDialog(this);
    }

    private void BtnZonas_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ZonasForm(_zonaService);
        form.ShowDialog(this);
    }

    private void BtnListaPrecioAcarreo_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ListaPrecioAcarreoForm(_listaPrecioAcarreoService, _municipioService, _paisService, _estadoService, _zonaService);
        form.ShowDialog(this);
    }

    private void BtnListaPrecioCorte_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ListaPrecioCorteForm(_listaPrecioCorteService);
        form.ShowDialog(this);
    }

    private void BtnFloraciones_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new FloracionesForm(_floracionService);
        form.ShowDialog(this);
    }

    private void BtnRecepcionesFruta_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new RecepcionesFrutaForm(_recepcionFrutaService, _reportePlantillaService, _empresaConfiguracionService);
        form.ShowDialog(this);
    }

    private void BtnLotes_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new LotesForm(_loteService, _lineaProduccionService);
        form.ShowDialog(this);
    }

    private void BtnLineasProduccion_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new LineasProduccionForm(_lineaProduccionService);
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

    private void BtnReportes_ItemClick(object? sender, ItemClickEventArgs e)
    {
        using var form = new ReportesForm(_reportePlantillaService);
        form.ShowDialog(this);
    }

    private void BtnSalir_ItemClick(object? sender, ItemClickEventArgs e) => Close();
}
