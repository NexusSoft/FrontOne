using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IGastoRecepcionAjusteRepository
{
    Task<IReadOnlyList<GastoRecepcionAjusteDto>> ObtenerAsync(int gastoLoteId, byte tipoGasto);
    Task<int> InsertarAsync(GastoRecepcionAjuste ajuste);
    Task ActualizarAsync(GastoRecepcionAjuste ajuste);
    Task EliminarAsync(int id);
}
