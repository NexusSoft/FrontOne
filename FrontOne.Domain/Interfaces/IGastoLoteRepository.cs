using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IGastoLoteRepository
{
    Task<IReadOnlyList<GastoLoteListadoDto>> ObtenerLotesCosteablesAsync();
    Task<GastoLoteEncabezado?> ObtenerEncabezadoAsync(int loteId);
    Task<int> ObtenerOCrearAsync(int loteId);
    Task<GastoLote?> ObtenerPorIdAsync(int id);
    Task ActualizarVigenciaEstimadoAsync(GastoLote gastoLote);
    Task<GastoLoteReporteDto?> ObtenerParaReporteAsync(int gastoLoteId);
}
