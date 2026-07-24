using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IMonedaRepository
{
    Task<IReadOnlyList<Moneda>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Moneda moneda);
    Task ActualizarAsync(Moneda moneda);
    Task EliminarAsync(int id);
}
