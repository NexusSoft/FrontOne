using FrontOne.Domain.DTOs;

namespace FrontOne.Domain.Interfaces;

public interface ISapProveedorRepository
{
    // Proveedores (Business Partners tipo Supplier) de SAP Business One que pertenecen al
    // grupo de proveedores indicado (ej. "Cosecha" para las empresas de corte).
    Task<IReadOnlyList<SapProveedorDto>> ObtenerPorGrupoAsync(int grupoCodigo, CancellationToken cancellationToken = default);
}
