namespace FrontOne.Domain.DTOs;

public record SapPedidoDto(
    int DocEntry,
    int DocNum,
    string CardCode,
    string CardName,
    DateTime DocDate,
    DateTime DocDueDate,
    decimal DocTotal,
    string DocCurrency,
    string Estatus,
    string? Comentarios,
    string? FolioFronterra);

public record SapPedidoLineaDto(
    string Codigo,
    string Descripcion,
    decimal Cantidad,
    decimal PrecioUnitario,
    decimal Total,
    string? Almacen);

public record SapPedidoDetalleDto(
    int DocEntry,
    int DocNum,
    string CardCode,
    string CardName,
    string? NumAtCard,
    DateTime DocDate,
    DateTime DocDueDate,
    DateTime? TaxDate,
    string DocCurrency,
    decimal DocRate,
    decimal DocTotal,
    decimal VatSum,
    decimal DiscountPercent,
    string Estatus,
    string? Comentarios,
    string? Direccion,
    string? VendedorCodigo,
    string? FolioFronterra,
    IReadOnlyList<SapPedidoLineaDto> Lineas);
