namespace FrontOne.Domain.DTOs;

public record GastoRecepcionBaseDto(
    int GastoRecepcionId,
    int LoteRecepcionId,
    byte CargoA,
    int RecepcionFrutaId,
    string RecepcionFolio,
    DateTime Fecha,
    decimal PesoNeto,
    decimal PesoProductor,
    int OrdenCorteId,
    string OrdenCorteFolio,
    string? Proveedor,
    decimal Cantidad,
    decimal PrecioUnitario,
    decimal Importe);
