using FrontOne.Application.Services;
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
        services.AddScoped<RecepcionFrutaService>();
        services.AddScoped<ReportePlantillaService>();
        services.AddScoped<LineaProduccionService>();
        services.AddScoped<LoteService>();

        return services;
    }
}
