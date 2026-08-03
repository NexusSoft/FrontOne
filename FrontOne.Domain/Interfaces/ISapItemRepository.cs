using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface ISapItemRepository
{
    // Items de SAP Business One cuyo ItemCode inicia con el prefijo indicado (ej. "MP" para fruta).
    Task<IReadOnlyList<SapItemDto>> ObtenerPorPrefijoAsync(string prefijo, CancellationToken cancellationToken = default);

    // Productos terminados de SAP Business One que pertenecen al grupo de artículos indicado
    // por nombre (ej. "Producto Terminado"). Resuelve primero el código de grupo en ItemGroups.
    Task<IReadOnlyList<SapProductoTerminadoDto>> ObtenerPorGrupoAsync(string nombreGrupo, CancellationToken cancellationToken = default);

    // Lista de materiales (ProductTrees) del ItemCode indicado, con sus componentes.
    Task<IReadOnlyList<SapListaMaterialesDto>> ObtenerListaMaterialesAsync(string itemCode, CancellationToken cancellationToken = default);
}
