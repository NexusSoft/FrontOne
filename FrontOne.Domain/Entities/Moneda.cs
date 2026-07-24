namespace FrontOne.Domain.Entities;

public class Moneda
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    // Abreviatura ISO-ish (MXN, USD...).
    public string Nomenclatura { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
