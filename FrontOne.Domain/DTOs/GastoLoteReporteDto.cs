namespace FrontOne.Domain.DTOs;

public record GastoLoteReporteDto(
    int LoteId,
    string LoteFolio,
    string CodigoTrazabilidad,
    DateTime? FechaCorrida,
    decimal Kilogramos,
    string? HuertaNombre,
    string? RegistroSagarpa,
    string? ProductorNombre,
    string? VariedadNombre,
    string? TipoCorteNombre,
    string? TipoPagoNombre);
