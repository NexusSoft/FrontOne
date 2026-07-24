using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IVariedadRepository
{
    Task<IReadOnlyList<Variedad>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Variedad variedad);
    Task ActualizarAsync(Variedad variedad);
    Task EliminarAsync(int id);
}
