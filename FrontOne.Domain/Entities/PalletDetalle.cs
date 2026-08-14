namespace FrontOne.Domain.Entities;

// Línea de detalle del Pallet. Para producto Caja: Kilogramos y CajasPorPallet son snapshot
// calculados en el SP a partir del Producto Terminado (PesoNeto × Cajas y CajasPorPallet) — nunca
// se capturan. Para producto Granel: Cajas y CajasPorPallet quedan NULL (no aplican, no hay cajas
// que empacar) y Kilogramos es el valor que el usuario captura directo.
// LoteEnProceso lo calcula sp_Pallet_ObtenerDetalle: si la Corrida de esta línea ya se finalizó,
// la línea queda de solo lectura aunque el Pallet siga sin bloquear.
public class PalletDetalle
{
    public int Id { get; set; }
    public int PalletId { get; set; }
    public int CorridaId { get; set; }
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
}
