using FrontOne.Shared.Configuration;
using FrontOne.Shared.Security;
using FrontOne.Web.Configuration;
using FrontOne.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace FrontOne.Web.Extensions;

/// <summary>
/// Piezas propias del sitio que completan lo que ya registran AddShared/AddApplication/
/// AddSqlServerInfrastructure/AddSapB1Infrastructure (esos cuatro no se tocan, se llaman tal cual
/// desde Program.cs — cero servicios duplicados respecto a WinForms).
///
/// Nota de ciclo de vida: AddApplication() registra todos los {Entidad}Service como AddScoped. En
/// Blazor Server un scope dura lo que dura el circuito (la conexión SignalR de una pestaña
/// abierta), no una sola petición HTTP — es distinto de una Web API clásica. Es correcto para
/// estos servicios porque son stateless sobre Dapper (abren/cierran su propia conexión SQL por
/// llamada vía IConnectionFactory), así que no hay estado que se filtre entre usuarios distintos
/// ni entre operaciones del mismo usuario.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        // Huecos que en WinForms llena Program.cs con RegistryConnectionStore/SessionContext.
        services.AddSingleton<IConnectionCredentialStore, ConfigurationConnectionCredentialStore>();
        services.AddScoped<ICurrentUserProvider, WebCurrentUserProvider>();

        // Autenticación/autorización propias del sitio.
        services.AddScoped<AuthenticationStateProvider, FrontOneAuthenticationStateProvider>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermisoPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermisoHandler>();

        return services;
    }
}
