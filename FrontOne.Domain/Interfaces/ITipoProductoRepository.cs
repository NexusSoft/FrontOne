using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ITipoProductoRepository
{
    Task<IReadOnlyList<TipoProducto>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(TipoProducto tipoProducto);
    Task ActualizarAsync(TipoProducto tipoProducto);
    Task EliminarAsync(int id);
}
