using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface IGastoFrutaCategoriaRepository
{
    Task<IReadOnlyList<GastoFrutaCategoriaLineaDto>> ObtenerAsync(int gastoLoteId);
    Task<IReadOnlyList<ResumenMercadoDto>> ObtenerResumenMercadoAsync(int gastoLoteId);
    Task GuardarCostoRealAsync(int gastoLoteId, string materiaPrimaItemCode, decimal? costoRealUnitario);
    Task GuardarCostoEstimadoAsync(int gastoLoteId, string materiaPrimaItemCode, decimal? costoEstimadoUnitario);
}
