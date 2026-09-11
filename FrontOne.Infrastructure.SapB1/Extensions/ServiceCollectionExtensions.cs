using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SapB1.Repositories;
using FrontOne.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrontOne.Infrastructure.SapB1.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSapB1Infrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SapOptions>(configuration.GetSection(SapOptions.SectionName));
        services.AddHttpClient<ISapServiceLayerClient, SapServiceLayerClient>();
        services.AddHttpClient();
        services.AddSingleton<ISapConnectionTester, SapConnectionTester>();
        services.AddScoped<ISapItemRepository, SapItemRepository>();
        services.AddScoped<ISapProveedorRepository, SapProveedorRepository>();
        services.AddScoped<ISapPedidoRepository, SapPedidoRepository>();

        return services;
    }
}
