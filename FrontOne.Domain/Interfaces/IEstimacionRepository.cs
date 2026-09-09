using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IEstimacionRepository
{
    Task<IReadOnlyList<Estimacion>> ObtenerAsync(int? id = null);
    Task<Estimacion?> ObtenerPorFolioAsync(string folio);
    Task<IReadOnlyList<EstimacionBusquedaDto>> ObtenerTop100Async();
    Task<IReadOnlyList<EstimacionBusquedaDto>> BuscarAsync(string filtro);
    Task<(int Id, string Folio)> InsertarAsync(Estimacion estimacion);
    Task ActualizarAsync(Estimacion estimacion);
    Task MarcarCerradaAsync(int id);
}
