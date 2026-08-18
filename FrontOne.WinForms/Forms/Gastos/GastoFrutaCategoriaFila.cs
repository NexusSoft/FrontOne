namespace FrontOne.WinForms.Forms.Gastos;

// Fila mutable para la captura inline del grid de la pestaña Fruta — el DTO es un record
// inmutable, no sirve como fuente de datos editable en el GridControl.
public class GastoFrutaCategoriaFila
{
    public string MateriaPrimaItemCode { get; set; } = string.Empty;
    public string MateriaPrimaNombre { get; set; } = string.Empty;
    public decimal KilogramosSeleccionados { get; set; }
    public decimal Porcentaje { get; set; }
    public decimal KilogramosComprados { get; set; }
    public decimal? CostoRealUnitario { get; set; }
    public decimal ImporteReal { get; set; }
    public decimal? CostoEstimadoUnitario { get; set; }
    public decimal ImporteEstimado { get; set; }
}
