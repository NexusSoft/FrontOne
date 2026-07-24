using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IPaisRepository
{
    Task<IReadOnlyList<Pais>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Pais pais);
    Task ActualizarAsync(Pais pais);
    Task EliminarAsync(int id);
}
