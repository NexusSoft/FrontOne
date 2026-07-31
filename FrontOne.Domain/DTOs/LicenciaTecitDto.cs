namespace FrontOne.Domain.DTOs;

public record LicenciaTecitDto(
    string Licenciatario,
    string? ClaveLicencia,
    string? TipoLicencia,
    int? NumeroLicencias,
    string? Producto);
