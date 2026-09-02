namespace FrontOne.Domain.DTOs;

public record PalletDetalleDto(
    int Id,
    int PalletId,
    int? CorridaId,
    int LoteId,
    string LoteFolio,
    int ProductoTerminadoId,
    string ProductoCodigoSap,
    string ProductoDescripcion,
    int? Cajas,
    decimal Kilogramos,
    decimal PorcentajeMateriaSeca,
    int? CajasPorPallet,
    bool LoteEnProceso,
    string? CodigoGs1128,
    string? VoiceCodeLow,
    string? VoiceCodeHigh,
    int? ReempaqueDetalleId,
    string? ReempaqueFolio,
    string OrigenDescripcion);
