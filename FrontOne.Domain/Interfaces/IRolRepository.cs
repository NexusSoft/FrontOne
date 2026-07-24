using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IRolRepository
{
    Task<IReadOnlyList<Rol>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Rol rol);
    Task ActualizarAsync(Rol rol);
    Task EliminarAsync(int id);
}
