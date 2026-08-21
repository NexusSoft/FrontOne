namespace FrontOne.Domain.DTOs;

public record LoteDto(
    int Id,
    string Folio,
    DateTime Fecha,
    string CodigoTrazabilidad,
    string? Observaciones,
    decimal Kilogramos,
    string? Personalizado,
    int LineaProduccionId,
    string LineaProduccionNombre,
    decimal PorcentajeMateriaSeca,
    byte Estatus,
    int Recepciones,
    string? HuertaNombre,
    string? ProductorNombre,
    int? VariedadId,
    string? VariedadNombre);
