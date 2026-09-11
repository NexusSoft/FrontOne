namespace FrontOne.Domain.Entities;

public class PermisoEspecial
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public bool Habilitado { get; set; }
}
