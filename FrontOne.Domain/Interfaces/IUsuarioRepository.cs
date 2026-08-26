using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<IReadOnlyList<PermisoDto>> ObtenerPermisosAsync(int usuarioId);
    Task<IReadOnlyList<ReportePermisoDto>> ObtenerPermisosReporteAsync(int usuarioId);
    Task<IReadOnlyList<PermisoDto>> ObtenerWebPermisosAsync(int usuarioId);
    Task ActualizarPasswordHashAsync(int usuarioId, string passwordHash);
    Task RegistrarIntentoFallidoAsync(string nombreUsuario);
    Task ResetearIntentosFallidosAsync(string nombreUsuario);

    Task<IReadOnlyList<Usuario>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task EliminarAsync(int id);

    Task<IReadOnlyList<int>> ObtenerRolesAsync(int usuarioId);
    Task AsignarRolesAsync(int usuarioId, IReadOnlyList<int> rolIds);
}
