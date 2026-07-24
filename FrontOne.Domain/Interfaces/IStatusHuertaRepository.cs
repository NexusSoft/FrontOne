using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IStatusHuertaRepository
{
    Task<IReadOnlyList<StatusHuerta>> ObtenerAsync(int? id = null);
    Task<int> InsertarAsync(StatusHuerta statusHuerta);
    Task ActualizarAsync(StatusHuerta statusHuerta);
    Task EliminarAsync(int id);
}
