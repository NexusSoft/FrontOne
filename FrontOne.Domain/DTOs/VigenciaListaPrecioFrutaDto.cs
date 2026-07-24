namespace FrontOne.Domain.DTOs;

public record VigenciaListaPrecioFrutaDto(
    DateTime Fecha, int? ProductorId, string? ProductorNombre, int? VariedadId, string? VariedadNombre);
