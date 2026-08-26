using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IWebPermisoRepository
{
    Task<IReadOnlyList<WebPermiso>> ObtenerPorRolAsync(int rolId);
    Task SincronizarAsync(int rolId, IReadOnlyList<WebPermiso> filas);
}
