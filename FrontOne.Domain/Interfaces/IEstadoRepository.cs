using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IEstadoRepository
{
    Task<IReadOnlyList<Estado>> ObtenerAsync(int? paisId = null, int? id = null);
    Task<int> InsertarAsync(Estado estado);
    Task ActualizarAsync(Estado estado);
    Task EliminarAsync(int id);
}
