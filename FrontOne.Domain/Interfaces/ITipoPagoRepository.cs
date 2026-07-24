using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ITipoPagoRepository
{
    Task<IReadOnlyList<TipoPago>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(TipoPago tipoPago);
    Task ActualizarAsync(TipoPago tipoPago);
    Task EliminarAsync(int id);
}
