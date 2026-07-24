namespace FrontOne.Domain.Entities;

public class Poblacion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int MunicipioId { get; set; }
    public bool Activo { get; set; }
}
