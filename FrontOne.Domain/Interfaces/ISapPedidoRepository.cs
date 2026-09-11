using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface ISapPedidoRepository
{
    Task<IReadOnlyList<SapPedidoDto>> ObtenerTop500Async(CancellationToken cancellationToken = default);

    // Solo pedidos con DocumentStatus = bost_Open — usado por Contenedor para elegir qué surtir.
    Task<IReadOnlyList<SapPedidoDto>> ObtenerAbiertosAsync(CancellationToken cancellationToken = default);

    Task<SapPedidoDetalleDto?> ObtenerPorDocEntryAsync(int docEntry, CancellationToken cancellationToken = default);
}
