using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ITipoComercializacionRepository
{
    Task<IReadOnlyList<TipoComercializacion>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(TipoComercializacion tipoComercializacion);
    Task ActualizarAsync(TipoComercializacion tipoComercializacion);
    Task EliminarAsync(int id);
}
