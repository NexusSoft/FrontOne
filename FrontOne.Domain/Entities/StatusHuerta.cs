namespace FrontOne.Domain.Entities;

public class StatusHuerta
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
