using FrontOne.Domain.DTOs;
using FrontOne.Shared.Constants;
using FrontOne.Shared.Security;

namespace FrontOne.WinForms.Session;

public class SessionContext : ICurrentUserProvider
{
    private readonly List<PermisoDto> _permisos = [];
    private readonly List<ReportePermisoDto> _permisosReporte = [];

    public UsuarioDto? UsuarioActual { get; private set; }
    public bool EstaAutenticado => UsuarioActual is not null;
    public string? NombreUsuario => UsuarioActual?.NombreUsuario;

    public void IniciarSesion(UsuarioDto usuario, IReadOnlyList<PermisoDto> permisos, IReadOnlyList<ReportePermisoDto> permisosReporte)
    {
        UsuarioActual = usuario;
        _permisos.Clear();
        _permisos.AddRange(permisos);
        _permisosReporte.Clear();
        _permisosReporte.AddRange(permisosReporte);
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
        _permisos.Clear();
        _permisosReporte.Clear();
    }

    public bool TienePermiso(string modulo, string pantalla, string accion)
        => _permisos.Any(p =>
            string.Equals(p.Modulo, modulo, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Pantalla, pantalla, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Accion, accion, StringComparison.OrdinalIgnoreCase));

    public bool TienePermisoReporte(string reporteCodigo, string accion)
    {
        var fila = _permisosReporte.FirstOrDefault(p => string.Equals(p.ReporteCodigo, reporteCodigo, StringComparison.OrdinalIgnoreCase));
        if (fila is null)
        {
            return false;
        }

        return accion switch
        {
            AccionReporte.VistaPrevia => fila.VistaPrevia,
            AccionReporte.Impresion => fila.Impresion,
            AccionReporte.Exportacion => fila.Exportacion,
            AccionReporte.Diseno => fila.Diseno,
            _ => false,
        };
    }
}
