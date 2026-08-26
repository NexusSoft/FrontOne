using System.Security.Claims;
using FrontOne.Application.Services;
using FrontOne.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FrontOne.Web.Security;

/// <summary>
/// Endpoints minimal API para login/logout. Van fuera del circuito de Blazor a propósito: firmar
/// la cookie de autenticación requiere una respuesta HTTP real (Set-Cookie), algo que un
/// componente InteractiveServer no puede hacer directo porque corre sobre WebSocket/SignalR, no
/// sobre el ciclo request/response normal. Login.razor postea aquí con un &lt;form method="post"&gt;
/// estático.
/// </summary>
public static class LoginEndpoints
{
    public static void Map(WebApplication app)
    {
        // Rutas fuera del árbol de páginas Razor a propósito: si coinciden con la ruta de una
        // página (p. ej. "/login", que también es @page "/login" en Login.razor), ASP.NET Core
        // registra dos endpoints POST distintos para la misma ruta (uno de Blazor para el
        // manejo de formularios con navegación mejorada, otro el minimal API de aquí) y el
        // enrutador lanza AmbiguousMatchException en cada submit.
        app.MapPost("/account/login", HandleLoginAsync)
            .RequireRateLimiting("login")
            .DisableAntiforgery()
            .AllowAnonymous();

        app.MapPost("/account/logout", HandleLogoutAsync)
            .DisableAntiforgery()
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleLoginAsync(
        HttpContext httpContext,
        AuthService authService,
        PermissionService permissionService,
        IUsuarioRepository usuarioRepository)
    {
        var form = await httpContext.Request.ReadFormAsync();
        var nombreUsuario = form["nombreUsuario"].ToString().Trim();
        var password = form["password"].ToString();
        var returnUrl = form["returnUrl"].ToString();

        string RedirigirConError(string mensaje)
        {
            var destino = string.IsNullOrEmpty(returnUrl) ? "/login" : "/login";
            return $"{destino}?error={Uri.EscapeDataString(mensaje)}&nombreUsuario={Uri.EscapeDataString(nombreUsuario)}";
        }

        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
        {
            return Results.Redirect(RedirigirConError("Usuario y contraseña son obligatorios."));
        }

        // Bloqueo de cuenta: 5 intentos fallidos consecutivos bloquean 15 minutos
        // (Seguridad.Usuario.IntentosFallidos/BloqueadoHasta), además del rate limiter por IP.
        var usuarioExistente = (await usuarioRepository.ObtenerAsync()).FirstOrDefault(u =>
            string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase));

        if (usuarioExistente is { BloqueadoHasta: not null } && usuarioExistente.BloqueadoHasta > DateTime.UtcNow)
        {
            return Results.Redirect(RedirigirConError("Cuenta bloqueada temporalmente por demasiados intentos fallidos. Intenta de nuevo más tarde."));
        }

        var resultado = await authService.LoginAsync(nombreUsuario, password);

        if (resultado.IsFailure || resultado.Value is null)
        {
            await usuarioRepository.RegistrarIntentoFallidoAsync(nombreUsuario);
            return Results.Redirect(RedirigirConError("Usuario o contraseña incorrectos."));
        }

        var usuario = resultado.Value;

        // AccesoWeb es la llave maestra: sin este permiso, ni con contraseña correcta se puede
        // entrar al sitio (aunque el usuario sea válido en escritorio/móvil).
        var permisosWeb = await permissionService.ObtenerWebPermisosAsync(usuario.Id);
        var tieneAccesoWeb = permisosWeb.Any(p =>
            string.Equals(p.Pantalla, "AccesoWeb", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Accion, "Consultar", StringComparison.OrdinalIgnoreCase));

        if (!tieneAccesoWeb)
        {
            return Results.Redirect(RedirigirConError("Su usuario no tiene acceso a la aplicación web."));
        }

        await usuarioRepository.ResetearIntentosFallidosAsync(nombreUsuario);

        var principal = ClaimsFactory.Crear(usuario, permisosWeb, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = false,
        });

        var destinoFinal = !string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith('/') ? returnUrl : "/";
        return Results.Redirect(destinoFinal);
    }

    private static async Task<IResult> HandleLogoutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.Redirect("/login");
    }
}
