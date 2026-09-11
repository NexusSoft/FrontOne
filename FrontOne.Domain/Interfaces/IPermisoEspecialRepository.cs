using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IPermisoEspecialRepository
{
    Task<IReadOnlyList<PermisoEspecial>> ObtenerPorRolAsync(int rolId);
    Task SincronizarAsync(int rolId, IReadOnlyList<PermisoEspecial> filas);
    Task<IReadOnlyList<string>> ObtenerCodigosHabilitadosAsync(int usuarioId);
}
