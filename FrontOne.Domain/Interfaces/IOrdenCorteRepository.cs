using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IOrdenCorteRepository
{
    Task<IReadOnlyList<OrdenCorte>> ObtenerAsync(int? id = null);
    Task<(int Id, string Folio)> InsertarAsync(OrdenCorte orden);
    Task ActualizarAsync(OrdenCorte orden);
    Task EliminarAsync(int id);

    // Usados por el picker "Seleccionar Orden de Corte" de Recepción de Fruta — solo regresan
    // órdenes que todavía no están ligadas a ninguna Recepción.
    Task<IReadOnlyList<OrdenCorteDisponible>> ObtenerTop100ParaRecepcionAsync();
    Task<IReadOnlyList<OrdenCorteDisponible>> BuscarParaRecepcionAsync(string filtro);
    Task<bool> EstaDisponibleParaRecepcionAsync(int ordenCorteId);
}
