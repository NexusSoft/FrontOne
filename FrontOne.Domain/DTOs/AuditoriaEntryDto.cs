namespace FrontOne.Domain.DTOs;

public record AuditoriaEntryDto(
    string Usuario,
    DateTime Fecha,
    string Equipo,
    string Ip,
    string Accion,
    string Modulo,
    string? ValoresAnteriores,
    string? ValoresNuevos);
