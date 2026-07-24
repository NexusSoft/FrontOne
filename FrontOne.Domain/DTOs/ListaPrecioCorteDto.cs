namespace FrontOne.Domain.DTOs;

public record ListaPrecioCorteDto(
    int Id,
    string? CardCode,
    string CardName,
    decimal PrecioKg,
    decimal PrecioDia,
    decimal CuadrillaApoyo,
    bool Activo);
