namespace FrontOne.Domain.Entities;

public class ListaPrecioCorte
{
    public int Id { get; set; }
    public string? CardCode { get; set; }
    public string CardName { get; set; } = string.Empty;
    public decimal PrecioKg { get; set; }
    public decimal PrecioDia { get; set; }
    public decimal CuadrillaApoyo { get; set; }
    public bool Activo { get; set; }
}
