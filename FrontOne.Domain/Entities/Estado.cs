namespace FrontOne.Domain.Entities;

public class Estado
{
    public int Id { get; set; }
    public int PaisId { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
