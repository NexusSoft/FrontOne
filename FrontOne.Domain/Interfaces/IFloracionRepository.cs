using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IFloracionRepository
{
    Task<IReadOnlyList<Floracion>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Floracion floracion);
    Task ActualizarAsync(Floracion floracion);
    Task EliminarAsync(int id);
}
