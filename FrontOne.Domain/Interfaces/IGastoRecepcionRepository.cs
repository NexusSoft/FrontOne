using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface IGastoRecepcionRepository
{
    Task<IReadOnlyList<GastoRecepcionBaseDto>> ObtenerBaseAsync(int gastoLoteId, byte tipoGasto);
    Task ActualizarCargoAAsync(int id, byte cargoA);
    Task<(decimal PrecioUnitario, decimal Importe)> ActualizarPrecioCorteAsync(int gastoRecepcionId);
    Task<(decimal PrecioUnitario, decimal Importe)> ActualizarPrecioAcarreoAsync(int gastoRecepcionId);
    Task<IReadOnlyList<RelacionGastoDto>> ObtenerParaReporteAsync(int gastoLoteId);
}
