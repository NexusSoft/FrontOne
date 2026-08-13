using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IMovilPermisoRepository
{
    Task<IReadOnlyList<MovilPermiso>> ObtenerPorRolAsync(int rolId);
    Task SincronizarAsync(int rolId, IReadOnlyList<MovilPermiso> filas);
}
