using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IPalletRepository
{
    Task<IReadOnlyList<Pallet>> ObtenerAsync(int? id = null);
    Task<IReadOnlyList<PalletDetalle>> ObtenerDetalleAsync(int palletId);
    Task<IReadOnlyList<LoteEnProcesoParaPalletDto>> ObtenerLotesEnProcesoAsync(int? lineaProduccionId = null);
    Task<PalletReporteDto?> ObtenerParaReporteAsync(int id);
    Task<PalletUltimaModificacionDto> ObtenerUltimaModificacionAsync();
    Task<int> InsertarAsync(int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal);
    Task<int> CrearNeutroAsync(int corridaId, int productoTerminadoId, decimal kilogramos);
    Task ActualizarEncabezadoAsync(int id, int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal);
    Task BloquearAsync(int id);
    Task EliminarAsync(int id);
    Task<int> InsertarDetalleAsync(int palletId, int corridaId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca);
    Task ActualizarDetalleAsync(int id, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca);
    Task EliminarDetalleAsync(int id);
}
