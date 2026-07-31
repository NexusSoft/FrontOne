using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface ILoteRepository
{
    Task<IReadOnlyList<Lote>> ObtenerAsync(int? id = null);
    Task<(int Id, string Folio)> InsertarAsync(Lote lote);
    Task ActualizarAsync(Lote lote);
    Task EliminarAsync(int id);

    Task<IReadOnlyList<LoteRecepcion>> ObtenerDetalleAsync(int loteId);
    Task<int> InsertarDetalleAsync(int loteId, int recepcionFrutaId);

    // Devuelve el RecepcionFrutaId de la línea borrada (o null si no existía) para que
    // LoteService pueda limpiar el "No. de Lote" de esa Recepción.
    Task<int?> EliminarDetalleAsync(int id);

    // Usado por RecepcionFrutaService.ActualizarAsync para bloquear la edición de una Recepción
    // que ya forma parte de un Lote.
    Task<bool> RecepcionEstaEnLoteAsync(int recepcionFrutaId);
}
