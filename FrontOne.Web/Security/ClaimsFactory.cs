using System.Security.Claims;
using FrontOne.Domain.DTOs;

namespace FrontOne.Web.Security;

/// <summary>
/// Convierte el resultado del login (UsuarioDto + permisos web) en el ClaimsPrincipal que se
/// firma en la cookie de autenticación. Cada permiso web se emite como claim "permisoWeb" con
/// valor "{Pantalla}/{Accion}" — es lo que PermisoHandler evalúa contra la política dinámica
/// "Permiso:{Pantalla}/{Accion}".
/// </summary>
public static class ClaimsFactory
{
    public const string TipoPermisoWeb = "permisoWeb";

    public static ClaimsPrincipal Crear(UsuarioDto usuario, IReadOnlyList<PermisoDto> permisosWeb, string authenticationType)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreUsuario),
            new("NombreCompleto", usuario.NombreCompleto),
        };

        claims.AddRange(permisosWeb.Select(p => new Claim(TipoPermisoWeb, $"{p.Pantalla}/{p.Accion}")));

        var identity = new ClaimsIdentity(claims, authenticationType);
        return new ClaimsPrincipal(identity);
    }
}
