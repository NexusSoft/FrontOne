namespace FrontOne.Domain.DTOs;

public record EtiquetaPalletDetalleDto(
    string NoLote,
    string? RegistroSagarpa,
    string? Huerta,
    int Cajas,
    decimal Kilogramos);
