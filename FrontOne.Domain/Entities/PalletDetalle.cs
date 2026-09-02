namespace FrontOne.Domain.Entities;

// Línea de detalle del Pallet. Para producto Caja: Kilogramos y CajasPorPallet son snapshot
// calculados en el SP a partir del Producto Terminado (PesoNeto × Cajas y CajasPorPallet) — nunca
// se capturan. Para producto Granel: Cajas y CajasPorPallet quedan NULL (no aplican, no hay cajas
// que empacar) y Kilogramos es el valor que el usuario captura directo.
// LoteEnProceso lo calcula sp_Pallet_ObtenerDetalle: si la Corrida de esta línea ya se finalizó,
// la línea queda de solo lectura aunque el Pallet siga sin bloquear.
// Una línea nace de una Corrida (proceso normal, CorridaId con valor) o de un Reempaque
// (ReempaqueDetalleId con valor) — exactamente uno de los dos, nunca ambos (CK_Produccion_
// PalletDetalle_Origen). ReempaqueFolio/OrigenDescripcion son de solo lectura, resueltos por el SP
// para la columna "Origen" + hipervínculo "No. de Reempaque" del detalle en PalletEditarForm.
public class PalletDetalle
{
    public int Id { get; set; }
    public int PalletId { get; set; }
    public int? CorridaId { get; set; }
    public int LoteId { get; set; }
    public string LoteFolio { get; set; } = string.Empty;
    public int ProductoTerminadoId { get; set; }
    public string ProductoCodigoSap { get; set; } = string.Empty;
    public string ProductoDescripcion { get; set; } = string.Empty;
    public int? Cajas { get; set; }
    public decimal Kilogramos { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
    public int? CajasPorPallet { get; set; }
    public bool LoteEnProceso { get; set; }
    public string? CodigoGs1128 { get; set; }
    public string? VoiceCodeLow { get; set; }
    public string? VoiceCodeHigh { get; set; }
    public int? ReempaqueDetalleId { get; set; }
    public string? ReempaqueFolio { get; set; }
    public string OrigenDescripcion { get; set; } = string.Empty;
}
