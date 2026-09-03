namespace FrontOne.Domain.Entities;

// Saldo GUARDADO (no derivado) de kilos disponibles por línea de pallet origen, reservado al
// folio del Reempaque. Una fila por cada línea de PalletDetalle del pallet que entra. Puede
// quedar en negativo temporalmente si la salida real pesó más que lo reservado — el Pallet Neutro
// de Reempaque lo regresa a 0 en cualquier dirección.
public class ReempaqueDetalle
{
    public int Id { get; set; }
    public int ReempaqueId { get; set; }
    public int PalletOrigenId { get; set; }
    public string PalletFolio { get; set; } = string.Empty;
    public int LoteId { get; set; }
    public string LoteFolio { get; set; } = string.Empty;
    public decimal PorcentajeMateriaSeca { get; set; }
    public int ProductoTerminadoOrigenId { get; set; }
    public string ProductoDescripcion { get; set; } = string.Empty;
    public int? CajasEntrada { get; set; }
    public decimal KilosEntrada { get; set; }
    public decimal KilosDisponibles { get; set; }
}
