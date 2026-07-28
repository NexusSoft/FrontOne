namespace FrontOne.Domain.DTOs;

public record RecepcionFrutaOrdenCorteDto(
    int Id,
    int RecepcionFrutaId,
    int OrdenCorteId,
    string OrdenCorteFolio,
    string HuertaNombre,
    short CajasCortadas,
    decimal Kilogramos);
