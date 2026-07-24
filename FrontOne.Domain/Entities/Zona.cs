namespace FrontOne.Domain.Entities;

public class Zona
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal KgMinimo300 { get; set; }
    public decimal KgMinimo400 { get; set; }
    public decimal KgMinimo500 { get; set; }
    public bool Activo { get; set; }
}
