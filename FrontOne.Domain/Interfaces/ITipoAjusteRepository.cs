using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ITipoAjusteRepository
{
    Task<IReadOnlyList<TipoAjuste>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(TipoAjuste tipoAjuste);
    Task ActualizarAsync(TipoAjuste tipoAjuste);
    Task EliminarAsync(int id);
}
