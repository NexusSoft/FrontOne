using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IHuertaRepository
{
    Task<IReadOnlyList<Huerta>> ObtenerAsync(int? id = null, int? productorId = null);
    Task<IReadOnlyList<HuertaBusquedaDto>> BuscarAsync(string filtro);
    Task<IReadOnlyList<HuertaBusquedaDto>> ObtenerTop100Async();
    Task<Huerta?> ObtenerPrimeroAsync();
    Task<Huerta?> ObtenerUltimoAsync();
    Task<Huerta?> ObtenerSiguienteAsync(int id);
    Task<Huerta?> ObtenerAnteriorAsync(int id);
    Task<int> InsertarAsync(Huerta huerta);
    Task ActualizarAsync(Huerta huerta);
    Task EliminarAsync(int id);
    Task<bool> ExisteRegistroSagarpaAsync(string registroSagarpa, int? idExcluir);
}
