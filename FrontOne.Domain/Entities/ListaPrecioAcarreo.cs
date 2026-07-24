namespace FrontOne.Domain.Entities;

public class ListaPrecioAcarreo
{
    public int Id { get; set; }
    public int MunicipioId { get; set; }
    public string MunicipioNombre { get; set; } = string.Empty;
    public string EstadoNombre { get; set; } = string.Empty;
    public int ZonaId { get; set; }
    public string ZonaNombre { get; set; } = string.Empty;
    public decimal Precio300 { get; set; }
    public decimal Precio400 { get; set; }
    public decimal Precio500 { get; set; }
    public int ToleranciaKgFaltante { get; set; }
    public bool Activo { get; set; }
}
