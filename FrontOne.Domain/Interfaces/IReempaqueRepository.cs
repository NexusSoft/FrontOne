using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;

namespace FrontOne.Domain.Interfaces;

public interface IReempaqueRepository
{
    Task<IReadOnlyList<Reempaque>> ObtenerAsync(int? id = null);
    Task<Reempaque?> ObtenerPorFolioAsync(string folio);
    Task EliminarAsync(int id);

    Task<IReadOnlyList<ReempaquePalletDisponibleDto>> ObtenerPalletsOrigenDisponiblesAsync(string? folio = null);
    Task<IReadOnlyList<ReempaquePalletDisponibleDto>> ObtenerPalletsDestinoDisponiblesAsync(int reempaqueId, string? folio = null);
    Task<IReadOnlyList<ReempaqueDetalle>> ObtenerDetalleEntradaAsync(int reempaqueId);
    Task AgregarPalletOrigenAsync(int reempaqueId, int palletId);
    Task QuitarPalletOrigenAsync(int reempaqueId, int palletId);

    Task<IReadOnlyList<ReempaqueSalidaFilaDto>> ObtenerDetalleSalidaAsync(int reempaqueId);

    Task<int> InsertarAsync(string motivo);

    // Escriben directo en Produccion.Pallet/PalletDetalle — la salida del reempaque es un pallet
    // más del módulo de Pallets, nunca una tabla propia. El contrato de retorno de
    // InsertarLineaEnPalletAsync es el mismo que IPalletRepository.InsertarDetalleAsync
    // (PalletDetalleInsertadoDto) para que PalletService.RecalcularGs1VoiceCodePalletAsync se
    // reuse sin ramificar por origen.
    Task<PalletDetalleInsertadoDto> InsertarLineaEnPalletAsync(int palletId, int reempaqueDetalleId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca);
    Task<int> CrearNeutroAsync(int reempaqueId, int reempaqueDetalleId, int productoTerminadoId, decimal kilogramos);

    Task CerrarAsync(int id);
}
