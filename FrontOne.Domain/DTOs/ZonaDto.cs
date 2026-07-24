namespace FrontOne.Domain.DTOs;

public record ZonaDto(int Id, string Nombre, decimal KgMinimo300, decimal KgMinimo400, decimal KgMinimo500, bool Activo);
