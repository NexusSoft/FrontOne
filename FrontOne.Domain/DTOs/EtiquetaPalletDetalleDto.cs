namespace FrontOne.Domain.DTOs;

public record EtiquetaPalletDetalleDto(
    string NoLote,
    string? RegistroSagarpa,
    string? Huerta,
    string? Productor,
    DateTime FechaLote,
    DateTime? FechaOrdenCorteMax,
    int Cajas,
    decimal Kilogramos);
