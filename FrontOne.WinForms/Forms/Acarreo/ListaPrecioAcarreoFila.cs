namespace FrontOne.WinForms.Forms.Acarreo;

// Fila mutable para la captura inline del grid de ListaPrecioAcarreoForm — el DTO es un record
// inmutable, no sirve como fuente de datos editable en el GridControl.
public class ListaPrecioAcarreoFila
{
    public int Id { get; set; }
    public int? MunicipioId { get; set; }
    public string? EstadoNombre { get; set; }
    public int? ZonaId { get; set; }
    public decimal? Precio300 { get; set; }
    public decimal? Precio400 { get; set; }
    public decimal? Precio500 { get; set; }
    public int? ToleranciaKgFaltante { get; set; }
    public bool Activo { get; set; } = true;
}
