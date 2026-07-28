namespace FrontOne.Domain.DTOs;

public record OrdenCorteDisponibleDto(
    int Id,
    string Folio,
    DateTime Fecha,
    string HuertaNombre,
    short CajasEntregadas);
