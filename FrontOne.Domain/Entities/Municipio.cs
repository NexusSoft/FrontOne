namespace FrontOne.Domain.Entities;

public class Municipio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int EstadoId { get; set; }
    public bool Activo { get; set; }
}
