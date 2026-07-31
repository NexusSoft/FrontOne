namespace FrontOne.Domain.DTOs;

public record LoteRecepcionDto(
    int Id,
    int LoteId,
    int RecepcionFrutaId,
    string RecepcionFrutaFolio,
    string? NumeroTicket,
    DateTime Fecha,
    string? CoprefBico,
    decimal PesoNeto,
    decimal PorcentajeMateriaSeca);
