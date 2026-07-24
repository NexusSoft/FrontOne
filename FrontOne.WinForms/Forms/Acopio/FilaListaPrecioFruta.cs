namespace FrontOne.WinForms.Forms.Acopio;

// Fila editable de la grilla: ItemCode/ItemName vienen de SAP, Lista1/2/3 se capturan a mano.
public class FilaListaPrecioFruta
{
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public decimal Lista1 { get; set; }
    public decimal Lista2 { get; set; }
    public decimal Lista3 { get; set; }
}
