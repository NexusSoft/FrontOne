using FrontOne.Shared.Security;

namespace FrontOne.Web.Security;

/// <summary>
/// Equivalente web de <c>SessionContext</c> (WinForms) para <see cref="ICurrentUserProvider"/>.
/// Se registra scoped (no singleton): en Blazor Server el scope dura lo que dura el circuito, así
/// que cada usuario conectado tiene su propia instancia con su propio <see cref="IHttpContextAccessor"/>
/// resuelto al nombre del usuario autenticado en la cookie. Con esto, AuditService sigue
/// funcionando igual que en escritorio sin que Application tenga que saber nada de HTTP.
/// </summary>
public class WebCurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebCurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? NombreUsuario => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
}
