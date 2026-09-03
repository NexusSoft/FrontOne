namespace FrontOne.Domain.DTOs;

// Fila del grid de Salida: toda línea de Produccion.PalletDetalle que nació de este reempaque
// (ReempaqueDetalleId <> NULL), sin importar en qué pallet destino terminó — incluye las del
// Pallet Neutro de ajuste (EsNeutro = true). PalletDetalleId es la línea real en PalletDetalle;
// PalletEstatus deja ver en el grid si el destino ya quedó Completo/Excedido.
public record ReempaqueSalidaFilaDto(
    int PalletDetalleId,
    int PalletId,
    string PalletFolio,
    byte PalletEstatus,
    bool EsNeutro,
    int LoteId,
    string LoteFolio,
    int ProductoTerminadoId,
    string ProductoDescripcion,
    int? Cajas,
    decimal Kilogramos,
    int ReempaqueDetalleId);
