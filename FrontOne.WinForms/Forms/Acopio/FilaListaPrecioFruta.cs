namespace FrontOne.WinForms.Forms.Acopio;

// Fila editable de la grilla: CategoriaId/CalibreApeamId (+ nombres) vienen de la combinación
// de Catalogos.MateriaPrima, Convencional/Organico/Nacional se capturan a mano.
public class FilaListaPrecioFruta
{
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public int CalibreApeamId { get; set; }
    public string CalibreApeamNombre { get; set; } = string.Empty;
    public decimal Convencional { get; set; }
    public decimal Organico { get; set; }
    public decimal Nacional { get; set; }
}
