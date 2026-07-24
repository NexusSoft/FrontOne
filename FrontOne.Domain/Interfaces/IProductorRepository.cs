using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IProductorRepository
{
    Task<IReadOnlyList<Productor>> ObtenerAsync(int? id = null);
    Task<IReadOnlyList<Productor>> BuscarAsync(string filtro);
    Task<IReadOnlyList<Productor>> ObtenerTop100Async();
    Task<Productor?> ObtenerPrimeroAsync();
    Task<Productor?> ObtenerUltimoAsync();
    Task<Productor?> ObtenerSiguienteAsync(int id);
    Task<Productor?> ObtenerAnteriorAsync(int id);
    Task<(int Id, string Clave)> InsertarAsync(Productor productor);
    Task ActualizarAsync(Productor productor);
    Task EliminarAsync(int id);
}
