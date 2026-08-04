using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ICorridaRepository
{
    Task<IReadOnlyList<Corrida>> ObtenerAsync(int? id = null);
    Task<IReadOnlyList<LoteDisponibleParaCorridaDto>> ObtenerLotesDisponiblesAsync();
    Task<int> IniciarAsync(int loteId);
    Task FinalizarAsync(int id);
    Task EliminarAsync(int id);
}
