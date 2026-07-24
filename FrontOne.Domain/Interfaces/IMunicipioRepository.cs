using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IMunicipioRepository
{
    Task<IReadOnlyList<Municipio>> ObtenerAsync(int? estadoId = null, int? id = null);
    Task<int> InsertarAsync(Municipio municipio);
    Task ActualizarAsync(Municipio municipio);
    Task EliminarAsync(int id);
}
