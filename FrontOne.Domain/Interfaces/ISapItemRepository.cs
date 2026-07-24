using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface ISapItemRepository
{
    // Items de SAP Business One cuyo ItemCode inicia con el prefijo indicado (ej. "MP" para fruta).
    Task<IReadOnlyList<SapItemDto>> ObtenerPorPrefijoAsync(string prefijo, CancellationToken cancellationToken = default);
}
