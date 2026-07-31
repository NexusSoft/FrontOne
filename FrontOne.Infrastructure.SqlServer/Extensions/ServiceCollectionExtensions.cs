using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using FrontOne.Infrastructure.SqlServer.Repositories;
using FrontOne.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrontOne.Infrastructure.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlServerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlOptions>(configuration.GetSection(SqlOptions.SectionName));
        services.AddSingleton<IConnectionFactory, ConnectionFactory>();
        services.AddSingleton<ISqlConnectionTester, SqlConnectionTester>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
        services.AddScoped<IPaisRepository, PaisRepository>();
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<IMunicipioRepository, MunicipioRepository>();
        services.AddScoped<IProductorRepository, ProductorRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IPermisoRepository, PermisoRepository>();
        services.AddScoped<IPoblacionRepository, PoblacionRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<IStatusHuertaRepository, StatusHuertaRepository>();
        services.AddScoped<ISistemaRiegoRepository, SistemaRiegoRepository>();
        services.AddScoped<IHuertaRepository, HuertaRepository>();
        services.AddScoped<IListaPrecioFrutaRepository, ListaPrecioFrutaRepository>();
        services.AddScoped<ITipoPagoRepository, TipoPagoRepository>();
        services.AddScoped<ITipoCorteRepository, TipoCorteRepository>();
        services.AddScoped<IVariedadRepository, VariedadRepository>();
        services.AddScoped<ITipoComercializacionRepository, TipoComercializacionRepository>();
        services.AddScoped<IMonedaRepository, MonedaRepository>();
        services.AddScoped<IAcuerdoCorteRepository, AcuerdoCorteRepository>();
        services.AddScoped<IZonaRepository, ZonaRepository>();
        services.AddScoped<IListaPrecioAcarreoRepository, ListaPrecioAcarreoRepository>();
        services.AddScoped<IJefeAcopioRepository, JefeAcopioRepository>();
        services.AddScoped<IListaPrecioCorteRepository, ListaPrecioCorteRepository>();
        services.AddScoped<IOrdenCorteRepository, OrdenCorteRepository>();
        services.AddScoped<IFloracionRepository, FloracionRepository>();
        services.AddScoped<IEmpresaConfiguracionRepository, EmpresaConfiguracionRepository>();
        services.AddScoped<ILicenciaTecitRepository, LicenciaTecitRepository>();
        services.AddScoped<IReportePermisoRepository, ReportePermisoRepository>();
        services.AddScoped<IRecepcionFrutaRepository, RecepcionFrutaRepository>();
        services.AddScoped<IReportePlantillaRepository, ReportePlantillaRepository>();

        return services;
    }
}
