using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;

namespace FrontOne.Application.Services;

// Regla dura (ver CLAUDE.md): el rol Administrador siempre tiene TODOS los permisos — de
// escritorio, de reportes y del sitio web — sin importar si esa pantalla/reporte/página tiene
// una fila otorgada a mano en Permiso/ReportePermiso/WebPermiso. Los 3 métodos de "obtener
// permisos" (consumidos igual por WinForms y por FrontOne.Web, ver LoginEndpoints.cs) revisan
// primero EsAdministradorAsync y, si es Administrador, regresan el universo completo en vez de
// consultar la tabla correspondiente — así un módulo/reporte/página nuevo nunca deja a
// Administrador sin acceso, aunque nadie se acuerde de otorgárselo (bug real que motivó esto:
// el reporte "ValeRecepcion" se agregó sin permiso para nadie, ni siquiera Administrador).
public class PermissionService
{
    private readonly IUsuarioRepository _usuarioRepository;

    private readonly PermisoEspecialService _permisoEspecialService;

    public PermissionService(IUsuarioRepository usuarioRepository, PermisoEspecialService permisoEspecialService)
    {
        _usuarioRepository = usuarioRepository;
        _permisoEspecialService = permisoEspecialService;
    }

    public async Task<IReadOnlyList<PermisoDto>> ObtenerPermisosAsync(int usuarioId)
    {
        if (await _usuarioRepository.EsAdministradorAsync(usuarioId))
        {
            return await _usuarioRepository.ObtenerTodosLosPermisosPosiblesAsync();
        }

        return await _usuarioRepository.ObtenerPermisosAsync(usuarioId);
    }

    public async Task<IReadOnlyList<ReportePermisoDto>> ObtenerPermisosReporteAsync(int usuarioId)
    {
        if (await _usuarioRepository.EsAdministradorAsync(usuarioId))
        {
            return ReportesDisponibles.Todos
                .Select(r => new ReportePermisoDto(r.Codigo, true, true, true, true))
                .ToList();
        }

        return await _usuarioRepository.ObtenerPermisosReporteAsync(usuarioId);
    }

    public async Task<IReadOnlyList<PermisoDto>> ObtenerWebPermisosAsync(int usuarioId)
    {
        if (await _usuarioRepository.EsAdministradorAsync(usuarioId))
        {
            return PantallasWebDisponibles.Todas
                .SelectMany(p => new[] { "Consultar", "Crear", "Modificar", "Eliminar" }
                    .Select(accion => new PermisoDto(p.Modulo, p.Codigo, accion)))
                .ToList();
        }

        return await _usuarioRepository.ObtenerWebPermisosAsync(usuarioId);
    }

    public async Task<IReadOnlyList<PermisoDto>> ObtenerPermisosEspecialesWebAsync(int usuarioId)
    {
        if (await _usuarioRepository.EsAdministradorAsync(usuarioId))
        {
            return PermisosEspecialesDisponibles.Todas
                .Select(d => new PermisoDto(d.Modulo, d.Pantalla, d.Accion))
                .ToList();
        }

        return await _permisoEspecialService.ObtenerPermisosWebAsync(usuarioId);
    }

    public async Task<bool> TienePermisoAsync(int usuarioId, string modulo, string pantalla, string accion)
    {
        if (await _usuarioRepository.EsAdministradorAsync(usuarioId))
        {
            return true;
        }

        var permisos = await _usuarioRepository.ObtenerPermisosAsync(usuarioId);
        return permisos.Any(p =>
            string.Equals(p.Modulo, modulo, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Pantalla, pantalla, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(p.Accion, accion, StringComparison.OrdinalIgnoreCase));
    }
}
