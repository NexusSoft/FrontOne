namespace FrontOne.Domain.DTOs;

public record CorridaDto(
    int Id,
    int LoteId,
    string LoteFolio,
    string CodigoTrazabilidad,
    decimal Kilogramos,
    string? HuertaNombre,
    string? RegistroSagarpa,
    string? ProductorNombre,
    string? Beneficiario,
    DateTime FechaHoraInicio,
    DateTime? FechaHoraFin,
    byte Estatus,
    decimal KilosAProcesar,
    decimal KilosProcesados,
    decimal? PesoFactor,
    DateTime FechaCreacion)
{
    public decimal KilosRestantes => KilosAProcesar - KilosProcesados;
}
