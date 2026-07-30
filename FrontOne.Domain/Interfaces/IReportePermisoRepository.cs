using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IReportePermisoRepository
{
    Task<IReadOnlyList<ReportePermiso>> ObtenerPorRolAsync(int rolId);
    Task SincronizarAsync(int rolId, IReadOnlyList<ReportePermiso> filas);
}
