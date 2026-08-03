using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IReadOnlyList<Categoria>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Categoria categoria);
    Task ActualizarAsync(Categoria categoria);
    Task EliminarAsync(int id);
}
