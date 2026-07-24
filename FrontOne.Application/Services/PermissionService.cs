using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;

namespace FrontOne.Application.Services;

public class PermissionService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public PermissionService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public Task<IReadOnlyList<PermisoDto>> ObtenerPermisosAsync(int usuarioId)
        => _usuarioRepository.ObtenerPermisosAsync(usuarioId);

    public async Task<bool> TienePermisoAsync(int usuarioId, string modulo, string pantalla, string accion)
    {
        var permisos = await _usuarioRepository.ObtenerPermisosAsync(usuarioId);
        return permisos.Any(p =>
            string.Equals(p.Modulo, modulo, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Pantalla, pantalla, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Accion, accion, StringComparison.OrdinalIgnoreCase));
    }
}
