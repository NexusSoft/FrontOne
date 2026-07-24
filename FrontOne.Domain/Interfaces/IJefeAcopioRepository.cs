using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IJefeAcopioRepository
{
    Task<IReadOnlyList<JefeAcopio>> ObtenerAsync(int? id = null);
    Task<IReadOnlyList<JefeAcopio>> BuscarAsync(string filtro);
    Task<IReadOnlyList<JefeAcopio>> ObtenerTop100Async();
    Task<JefeAcopio?> ObtenerPrimeroAsync();
    Task<JefeAcopio?> ObtenerUltimoAsync();
    Task<JefeAcopio?> ObtenerSiguienteAsync(int id);
    Task<JefeAcopio?> ObtenerAnteriorAsync(int id);
    Task<(int Id, string Clave)> InsertarAsync(JefeAcopio jefeAcopio);
    Task ActualizarAsync(JefeAcopio jefeAcopio);
    Task EliminarAsync(int id);
}
