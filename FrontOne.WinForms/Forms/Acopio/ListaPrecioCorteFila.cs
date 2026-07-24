namespace FrontOne.WinForms.Forms.Acopio;

// Fila mutable para la captura inline del grid de ListaPrecioCorteForm — el DTO es un record
// inmutable, no sirve como fuente de datos editable en el GridControl.
public class ListaPrecioCorteFila
{
    public int Id { get; set; }
    public string? CardCode { get; set; }
    public string CardName { get; set; } = string.Empty;
    public decimal PrecioKg { get; set; }
    public decimal PrecioDia { get; set; }
    public decimal CuadrillaApoyo { get; set; }
}
