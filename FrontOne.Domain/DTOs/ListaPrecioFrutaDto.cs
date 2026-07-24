namespace FrontOne.Domain.DTOs;

public record ListaPrecioFrutaDto(
    int Id,
    string ItemCode,
    string ItemName,
    decimal Lista1,
    decimal Lista2,
    decimal Lista3,
    DateTime FechaInicio,
    DateTime? FechaFin,
    bool Activo,
    int? ProductorId = null,
    int? VariedadId = null);
