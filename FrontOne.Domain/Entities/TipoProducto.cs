namespace FrontOne.Domain.Entities;

public class TipoProducto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
