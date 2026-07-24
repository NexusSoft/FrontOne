using FrontOne.Domain.DTOs;
using FrontOne.Shared.Security;

namespace FrontOne.WinForms.Session;

public class SessionContext : ICurrentUserProvider
{
    private readonly List<PermisoDto> _permisos = [];

    public UsuarioDto? UsuarioActual { get; private set; }
    public bool EstaAutenticado => UsuarioActual is not null;
    public string? NombreUsuario => UsuarioActual?.NombreUsuario;

    public void IniciarSesion(UsuarioDto usuario, IReadOnlyList<PermisoDto> permisos)
    {
        UsuarioActual = usuario;
        _permisos.Clear();
        _permisos.AddRange(permisos);
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
        _permisos.Clear();
    }

    public bool TienePermiso(string modulo, string pantalla, string accion)
        => _permisos.Any(p =>
            string.Equals(p.Modulo, modulo, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Pantalla, pantalla, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Accion, accion, StringComparison.OrdinalIgnoreCase));
}
