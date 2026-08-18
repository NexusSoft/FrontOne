namespace FrontOne.Domain.DTOs;

public record MateriaPrimaDto(
    int Id,
    string CodigoSap,
    string DescripcionSap,
    bool Activo,
    int? CategoriaId,
    int? CalibreApeamId,
    DateTime FechaCreacion,
    string? CategoriaNombre = null,
    string? CalibreApeamNombre = null)
{
    // Basta con revisar Categoría para saber si la materia prima ya se completó tras la
    // sincronización con SAP (mismo criterio que ProductoTerminadoDto.DatosCapturados).
    public bool DatosCapturados => CategoriaId is not null;
}
