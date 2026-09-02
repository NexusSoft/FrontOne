namespace FrontOne.Domain.DTOs;

// Fila del grid de Entrada: un pallet origen desglosado por lote (ver ejemplo del pallet 14259,
// 3 lotes -> 3 filas). KilosDisponibles es el saldo reservado a este folio que todavía no se ha
// consumido en la salida.
public record ReempaqueDetalleDto(
    int Id,
    int ReempaqueId,
    int PalletOrigenId,
    string PalletFolio,
    int LoteId,
    string LoteFolio,
    decimal PorcentajeMateriaSeca,
    int ProductoTerminadoOrigenId,
    string ProductoDescripcion,
    int? CajasEntrada,
    decimal KilosEntrada,
    decimal KilosDisponibles);
