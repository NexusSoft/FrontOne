using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IPoblacionRepository
{
    Task<IReadOnlyList<Poblacion>> ObtenerAsync(int? municipioId = null, int? id = null);
    Task<int> InsertarAsync(Poblacion poblacion);
    Task ActualizarAsync(Poblacion poblacion);
    Task EliminarAsync(int id);
}
