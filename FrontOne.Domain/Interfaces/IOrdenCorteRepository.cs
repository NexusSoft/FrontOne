using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IOrdenCorteRepository
{
    Task<IReadOnlyList<OrdenCorte>> ObtenerAsync(int? id = null);
    Task<(int Id, string Folio)> InsertarAsync(OrdenCorte orden);
    Task ActualizarAsync(OrdenCorte orden);
    Task EliminarAsync(int id);
}
