using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace FrontOne.Web.Security;

/// <summary>
/// AuthenticationStateProvider del sitio: toma el ClaimsPrincipal ya autenticado por el
/// middleware de cookie auth (UseAuthentication, corre antes de que Blazor arme el circuito) y lo
/// expone a los componentes vía CascadingAuthenticationState. El login real (validar
/// usuario/password contra AuthService y firmar la cookie) sucede en el endpoint POST /login
/// (ver Program.cs) — este provider solo refleja el resultado, no lo produce.
/// </summary>
public class FrontOneAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FrontOneAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var usuario = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(usuario));
    }
}
