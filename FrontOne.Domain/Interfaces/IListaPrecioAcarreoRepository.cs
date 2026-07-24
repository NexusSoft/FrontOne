using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IListaPrecioAcarreoRepository
{
    Task<IReadOnlyList<ListaPrecioAcarreo>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(ListaPrecioAcarreo lista);
    Task ActualizarAsync(ListaPrecioAcarreo lista);
    Task EliminarAsync(int id);
}
