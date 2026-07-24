using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ITipoCorteRepository
{
    Task<IReadOnlyList<TipoCorte>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(TipoCorte tipoCorte);
    Task ActualizarAsync(TipoCorte tipoCorte);
    Task EliminarAsync(int id);
}
