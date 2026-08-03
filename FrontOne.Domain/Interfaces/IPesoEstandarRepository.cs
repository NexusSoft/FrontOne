using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IPesoEstandarRepository
{
    Task<IReadOnlyList<PesoEstandar>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(PesoEstandar pesoEstandar);
    Task ActualizarAsync(PesoEstandar pesoEstandar);
    Task EliminarAsync(int id);
}
