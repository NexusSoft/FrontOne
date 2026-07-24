using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IListaPrecioCorteRepository
{
    Task<IReadOnlyList<ListaPrecioCorte>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(ListaPrecioCorte lista);
    Task ActualizarAsync(ListaPrecioCorte lista);
    Task ActualizarNombreAsync(int id, string cardName);
    Task ActualizarActivoAsync(int id, bool activo);
}
