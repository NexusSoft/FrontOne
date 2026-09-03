namespace FrontOne.Domain.Entities;

public class WebPermiso
{
    public int Id { get; set; }
    public int RolId { get; set; }
    public string PantallaCodigo { get; set; } = string.Empty;
    public bool Consultar { get; set; }
    public bool Crear { get; set; }
    public bool Modificar { get; set; }
    public bool Eliminar { get; set; }
}
