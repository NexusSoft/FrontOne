using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;

namespace FrontOne.Application.Services;

// Solo lectura — los Pedidos se capturan en SAP, aquí solo se consultan (ver contexto/embarques.md).
public class PedidoService
{
    private readonly ISapPedidoRepository _sapPedidoRepository;

    public PedidoService(ISapPedidoRepository sapPedidoRepository)
    {
        _sapPedidoRepository = sapPedidoRepository;
    }

    public Task<IReadOnlyList<SapPedidoDto>> ObtenerTop500Async(CancellationToken cancellationToken = default)
        => _sapPedidoRepository.ObtenerTop500Async(cancellationToken);

    public Task<IReadOnlyList<SapPedidoDto>> ObtenerAbiertosAsync(CancellationToken cancellationToken = default)
        => _sapPedidoRepository.ObtenerAbiertosAsync(cancellationToken);

    public Task<SapPedidoDetalleDto?> ObtenerDetalleAsync(int docEntry, CancellationToken cancellationToken = default)
        => _sapPedidoRepository.ObtenerPorDocEntryAsync(docEntry, cancellationToken);
}
