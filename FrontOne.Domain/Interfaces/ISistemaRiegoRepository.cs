using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ISistemaRiegoRepository
{
    Task<IReadOnlyList<SistemaRiego>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(SistemaRiego sistemaRiego);
    Task ActualizarAsync(SistemaRiego sistemaRiego);
    Task EliminarAsync(int id);
}
