using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);
}
