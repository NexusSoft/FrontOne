using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IZonaRepository
{
    Task<IReadOnlyList<Zona>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(Zona zona);
    Task ActualizarAsync(Zona zona);
    Task EliminarAsync(int id);
}
