namespace FrontOne.Domain.DTOs;

public record TipoCorteDto(
    int Id,
    string Nombre,
    decimal FueraDeNormaGr,
    bool DanioMinimo,
    int TipoPagoId,
    bool Activo,
    string TipoPagoNombre);
