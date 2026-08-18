namespace FrontOne.Domain.DTOs;

public record GastoLoteListadoDto(
    int LoteId,
    string Folio,
    DateTime Fecha,
    decimal Kilogramos,
    string? HuertaNombre,
    string? ProductorNombre);
