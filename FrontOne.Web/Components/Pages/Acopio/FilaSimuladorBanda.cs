namespace FrontOne.Web.Components.Pages.Acopio;

// Fila editable del Simulador de Bandas: CategoriaId/CalibreApeamId (+ nombres) vienen de la misma
// combinación de Catalogos.MateriaPrima que usa Lista de Precios de Fruta; Precio y Porcentaje se
// capturan a mano (o se precargan desde una lista ya guardada).
//
// Gemelo consciente de FrontOne.WinForms/Forms/Acopio/FilaSimuladorBanda.cs — no se movió a Domain
// para no tocar código WinForms ya probado. Ahí la clase implementa INotifyPropertyChanged porque
// GridView (WinForms) necesita ese evento para repintar la celda Banda y las sumas del footer; en
// Blazor el re-render lo dispara el propio ciclo de eventos del componente, así que no hace falta.
public class FilaSimuladorBanda
{
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public int CalibreApeamId { get; set; }
    public string CalibreApeamNombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public decimal Porcentaje { get; set; }

    // Sin redondear a propósito: el footer suma estos valores con precisión completa. Sumar los
    // valores ya redondeados a 2 decimales desviaría el total (18.94 en vez del 18.93 real del
    // caso de referencia) — solo el formato de la columna redondea, no el dato.
    public decimal Banda => Precio * Porcentaje / 100m;
}
