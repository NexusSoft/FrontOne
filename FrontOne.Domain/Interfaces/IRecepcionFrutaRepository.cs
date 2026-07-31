using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IRecepcionFrutaRepository
{
    Task<IReadOnlyList<RecepcionFruta>> ObtenerAsync(int? id = null);
    Task<(int Id, string Folio)> InsertarAsync(RecepcionFruta recepcion);
    Task ActualizarAsync(RecepcionFruta recepcion);
    Task EliminarAsync(int id);

    Task<IReadOnlyList<RecepcionFrutaOrdenCorte>> ObtenerDetalleAsync(int recepcionFrutaId);
    Task<int> InsertarDetalleAsync(RecepcionFrutaOrdenCorte linea);
    Task ActualizarDetalleAsync(int id, decimal kilogramos);
    Task EliminarDetalleAsync(int id);

    // Proyección ancha para el reporte "Recepción de Fruta" — join directo, no pasa por Entity.
    Task<RecepcionFrutaReporteDto?> ObtenerParaReporteAsync(int id);

    // Alimentan el picker "Seleccionar Recepción" del módulo Lotes — mismo criterio que
    // ObtenerTop100ParaRecepcionAsync/BuscarParaRecepcionAsync de IOrdenCorteRepository.
    Task<IReadOnlyList<RecepcionDisponibleParaLote>> ObtenerTop100ParaLoteAsync(int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode);
    Task<IReadOnlyList<RecepcionDisponibleParaLote>> BuscarParaLoteAsync(string filtro, int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode);
    Task<RecepcionDisponibleParaLote?> ObtenerParaLotePorIdAsync(int recepcionFrutaId);

    // Al agregar/quitar una Recepción de un Lote, LoteService actualiza aquí su "No. de Lote"
    // (Folio del Lote, o NULL al quitarla) sin tener que pasar por ActualizarAsync completo.
    Task ActualizarNoLoteAsync(int id, string? noLote);
}
