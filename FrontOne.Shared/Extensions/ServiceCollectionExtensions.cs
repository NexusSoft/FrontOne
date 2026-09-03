using FrontOne.Shared.Security;
using Microsoft.Extensions.DependencyInjection;

namespace FrontOne.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddSingleton<ICryptoService, CryptoService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
