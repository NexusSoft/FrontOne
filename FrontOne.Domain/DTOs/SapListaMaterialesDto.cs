namespace FrontOne.Domain.DTOs;

public record SapListaMaterialesDto(string CodigoComponente, string? Descripcion, decimal Cantidad, string? UnidadMedida, string? Almacen);
