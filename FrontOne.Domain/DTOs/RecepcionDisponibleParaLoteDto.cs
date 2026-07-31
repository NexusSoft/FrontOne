namespace FrontOne.Domain.DTOs;

public record RecepcionDisponibleParaLoteDto(
    int Id,
    string Folio,
    string? NumeroTicket,
    DateTime Fecha,
    decimal PesoNeto,
    decimal PorcentajeMateriaSeca,
    string? CoprefBico,
    int HuertaId,
    string HuertaNombre,
    int AcuerdoCorteId,
    string PagarCorteACardCode,
    string PagarCorteANombre);
