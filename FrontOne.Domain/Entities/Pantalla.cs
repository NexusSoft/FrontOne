namespace FrontOne.Domain.Entities;

public class Pantalla
{
    public int Id { get; set; }
    public int ModuloId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
