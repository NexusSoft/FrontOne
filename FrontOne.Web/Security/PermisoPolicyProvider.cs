using Microsoft.AspNetCore.Authorization;

namespace FrontOne.Web.Security;

/// <summary>
/// Fabrica al vuelo la política "Permiso:{Pantalla}/{Accion}" — no hace falta registrar cada
/// combinación de antemano con AddPolicy; cualquier página nueva puede usar
/// [Authorize(Policy = "Permiso:Paises/Consultar")] y la política se construye sola la primera
/// vez que se le pide. Todo lo que no matchea ese prefijo cae al provider por defecto
/// (DefaultAuthorizationPolicyProvider), que sigue resolviendo la política "fallback"
/// (RequireAuthenticatedUser) configurada en Program.cs.
/// </summary>
public class PermisoPolicyProvider : IAuthorizationPolicyProvider
{
    private const string Prefijo = "Permiso:";

    private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

    public PermisoPolicyProvider(Microsoft.Extensions.Options.IOptions<AuthorizationOptions> options)
    {
        _fallbackProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(Prefijo, StringComparison.OrdinalIgnoreCase))
        {
            var partes = policyName[Prefijo.Length..].Split('/', 2);
            if (partes.Length == 2)
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new PermisoRequirement(partes[0], partes[1]))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }
        }

        return _fallbackProvider.GetPolicyAsync(policyName);
    }
}
