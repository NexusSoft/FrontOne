namespace FrontOne.Domain.Entities;

public class ListaPrecioFruta
{
    public int Id { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public decimal Lista1 { get; set; }
    public decimal Lista2 { get; set; }
    public decimal Lista3 { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    // NULL = lista general. Con valor = lista especial (override) solo para ese productor.
    public int? ProductorId { get; set; }
    public int? VariedadId { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}
