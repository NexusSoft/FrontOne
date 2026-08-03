using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IMarcaRepository
{
    Task<IReadOnlyList<Marca>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Marca marca);
    Task ActualizarAsync(Marca marca);
    Task EliminarAsync(int id);
}
