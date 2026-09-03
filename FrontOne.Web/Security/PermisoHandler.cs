using Microsoft.AspNetCore.Authorization;

namespace FrontOne.Web.Security;

/// <summary>Resuelve PermisoRequirement contra los claims "permisoWeb" firmados en la cookie del usuario.</summary>
public class PermisoHandler : AuthorizationHandler<PermisoRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisoRequirement requirement)
    {
        var valorEsperado = $"{requirement.Pantalla}/{requirement.Accion}";

        var tienePermiso = context.User.Claims.Any(c =>
            c.Type == ClaimsFactory.TipoPermisoWeb &&
            string.Equals(c.Value, valorEsperado, StringComparison.OrdinalIgnoreCase));

        if (tienePermiso)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
