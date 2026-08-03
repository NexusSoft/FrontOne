using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ICalibreApeamRepository
{
    Task<IReadOnlyList<CalibreApeam>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(CalibreApeam calibreApeam);
    Task ActualizarAsync(CalibreApeam calibreApeam);
    Task EliminarAsync(int id);
}
