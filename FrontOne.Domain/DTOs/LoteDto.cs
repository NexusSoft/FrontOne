namespace FrontOne.Domain.DTOs;

public record LoteDto(
    int Id,
    string Folio,
    DateTime Fecha,
    string Referencia,
    string? Observaciones,
    decimal Kilogramos,
    string? Personalizado,
    int LineaProduccionId,
    string LineaProduccionNombre,
    decimal PorcentajeMateriaSeca,
    byte Estatus,
    int Tickets,
    string? HuertaNombre,
    string? ProductorNombre);
