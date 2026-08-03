namespace FrontOne.Domain.Entities;

public class PesoEstandar
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal PesoNeto { get; set; }
    public decimal PesoPromedio { get; set; }
    public bool Activo { get; set; }
}
