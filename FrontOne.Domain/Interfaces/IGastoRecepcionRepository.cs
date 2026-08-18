using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface IGastoRecepcionRepository
{
    Task<IReadOnlyList<GastoRecepcionBaseDto>> ObtenerBaseAsync(int gastoLoteId, byte tipoGasto);
    Task ActualizarCargoAAsync(int id, byte cargoA);
    Task<IReadOnlyList<RelacionGastoDto>> ObtenerParaReporteAsync(int gastoLoteId);
}
