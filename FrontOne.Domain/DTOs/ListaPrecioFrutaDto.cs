namespace FrontOne.Domain.DTOs;

public record ListaPrecioFrutaDto(
    int Id,
    int CategoriaId,
    int CalibreApeamId,
    decimal Convencional,
    decimal Organico,
    decimal Nacional,
    DateTime FechaInicio,
    DateTime? FechaFin,
    bool Activo,
    int? ProductorId = null,
    string? CategoriaNombre = null,
    string? CalibreApeamNombre = null);
