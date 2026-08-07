namespace FrontOne.Domain.Entities;

// Encabezado del Pallet. Estatus, PorcentajeMateriaSeca, TotalCajas y TotalKilogramos nunca se
// capturan: los recalcula Produccion.sp_Pallet_Recalcular con cada movimiento del detalle.
// NoReempaque queda vacío hasta que el futuro módulo de Reempaques lo llene.
public class Pallet
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public TimeSpan HoraCreacion { get; set; }
    public byte Estatus { get; set; }
    public int LineaProduccionId { get; set; }
    public string LineaProduccionNombre { get; set; } = string.Empty;
    public bool EsMixto { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
    public decimal? PesoReal { get; set; }
    public bool Bloqueado { get; set; }
    public DateTime? FechaBloqueo { get; set; }
    public string? NoReempaque { get; set; }
    public bool PrimeraCorrida { get; set; }
    public int TotalCajas { get; set; }
    public decimal TotalKilogramos { get; set; }
    public string ProductoDescripcion { get; set; } = string.Empty;
    public DateTime FechaCreacionRegistro { get; set; }
}
