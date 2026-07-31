using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ILineaProduccionRepository
{
    Task<IReadOnlyList<LineaProduccion>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(LineaProduccion lineaProduccion);
    Task ActualizarAsync(LineaProduccion lineaProduccion);
    Task EliminarAsync(int id);
}
