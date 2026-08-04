using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ICajaCampoRepository
{
    Task<IReadOnlyList<CajaCampo>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(CajaCampo cajaCampo);
    Task ActualizarAsync(CajaCampo cajaCampo);
    Task EliminarAsync(int id);
}
