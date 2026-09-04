using FrontOne.Application.Services;
using FrontOne.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace FrontOne.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuditService>();
        services.AddScoped<PermissionService>();
        services.AddScoped<AuthService>();
        services.AddScoped<ConnectionSettingsService>();
        services.AddScoped<PaisService>();
        services.AddScoped<EstadoService>();
        services.AddScoped<MunicipioService>();
        services.AddScoped<ProductorService>();
        services.AddScoped<UsuarioService>();
        services.AddScoped<RolService>();
        services.AddScoped<PermisoService>();
        services.AddScoped<PoblacionService>();
        services.AddScoped<ProductoService>();
        services.AddScoped<StatusHuertaService>();
        services.AddScoped<SistemaRiegoService>();
        services.AddScoped<HuertaService>();
        services.AddScoped<ListaPrecioFrutaService>();
        services.AddScoped<TipoPagoService>();
        services.AddScoped<TipoCorteService>();
        services.AddScoped<VariedadService>();
        services.AddScoped<TipoComercializacionService>();
        services.AddScoped<MonedaService>();
        services.AddScoped<AcuerdoCorteService>();
        services.AddScoped<ZonaService>();
        services.AddScoped<ListaPrecioAcarreoService>();
        services.AddScoped<JefeAcopioService>();
        services.AddScoped<ListaPrecioCorteService>();
        services.AddScoped<OrdenCorteService>();
        services.AddScoped<FloracionService>();
        services.AddScoped<EmpresaConfiguracionService>();
        services.AddScoped<LicenciaTecitService>();
        services.AddScoped<ReportePermisoService>();
        services.AddScoped<MovilPermisoService>();
        services.AddScoped<WebPermisoService>();
        services.AddScoped<RecepcionFrutaService>();
        services.AddScoped<ReportePlantillaService>();
        services.AddScoped<LineaProduccionService>();
        services.AddScoped<LoteService>();
        services.AddScoped<CorridaService>();
        services.AddScoped<CategoriaService>();
        services.AddScoped<CajaCampoService>();
        services.AddScoped<TipoProductoService>();
        services.AddScoped<CalibreApeamService>();
        services.AddScoped<MarcaService>();
        services.AddScoped<PesoEstandarService>();
        services.AddScoped<ProductoTerminadoValidator>();
        services.AddScoped<ProductoTerminadoService>();
        services.AddScoped<MateriaPrimaValidator>();
        services.AddScoped<MateriaPrimaService>();
        services.AddScoped<MovimientoAlmacenService>();
        services.AddScoped<PalletService>();
        services.AddScoped<ReempaqueService>();
        services.AddScoped<ConfiguracionBasculaService>();
        services.AddScoped<SupervisorHuertaService>();
        services.AddScoped<IncidenciaService>();
        services.AddScoped<EtiquetaService>();
        services.AddScoped<GastoLoteService>();
        services.AddScoped<TipoAjusteService>();
        services.AddScoped<GastoFrutaCategoriaService>();
        services.AddScoped<GastoRecepcionService>();
        services.AddScoped<GastoRecepcionAjusteService>();
        services.AddScoped<PedidoService>();
        services.AddScoped<ContenedorService>();

        return services;
    }
}
