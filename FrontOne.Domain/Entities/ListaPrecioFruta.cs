namespace FrontOne.Domain.Entities;

public class ListaPrecioFruta
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
    public int CalibreApeamId { get; set; }
    public string? CalibreApeamNombre { get; set; }
    public decimal Convencional { get; set; }
    public decimal Organico { get; set; }
    public decimal Nacional { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    // NULL = lista general. Con valor = lista especial (override) solo para ese productor.
    public int? ProductorId { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}
