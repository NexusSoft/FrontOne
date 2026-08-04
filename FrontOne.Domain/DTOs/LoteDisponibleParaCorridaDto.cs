namespace FrontOne.Domain.DTOs;

public record LoteDisponibleParaCorridaDto(
    int LoteId,
    string LoteFolio,
    string CodigoTrazabilidad,
    DateTime Fecha,
    decimal Kilogramos,
    string? HuertaNombre,
    string? RegistroSagarpa,
    string? ProductorNombre,
    string? Beneficiario);
