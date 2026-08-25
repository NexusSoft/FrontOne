using System.ComponentModel;

namespace FrontOne.WinForms.Forms.Acopio;

// Fila editable del Simulador de Bandas: CategoriaId/CalibreApeamId (+ nombres) vienen de la
// misma combinación de Catalogos.MateriaPrima que usa Lista de Precios de Fruta; Precio y
// Porcentaje se capturan a mano (o se precargan desde una lista ya guardada). Implementa
// INotifyPropertyChanged para que el grid recalcule Banda y las sumas del footer en vivo al
// editar una celda, sin código adicional en el form.
public class FilaSimuladorBanda : INotifyPropertyChanged
{
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public int CalibreApeamId { get; set; }
    public string CalibreApeamNombre { get; set; } = string.Empty;

    private decimal _precio;
    public decimal Precio
    {
        get => _precio;
        set
        {
            _precio = value;
            Notificar(nameof(Precio));
            Notificar(nameof(Banda));
        }
    }

    private decimal _porcentaje;
    public decimal Porcentaje
    {
        get => _porcentaje;
        set
        {
            _porcentaje = value;
            Notificar(nameof(Porcentaje));
            Notificar(nameof(Banda));
        }
    }

    // Sin redondear a propósito: el footer suma estos valores con precisión completa. Sumar los
    // valores ya redondeados a 2 decimales desviaría el total (18.94 en vez del 18.93 real del
    // caso de referencia) — solo el DisplayFormat de la columna redondea, no el dato.
    public decimal Banda => Precio * Porcentaje / 100m;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void Notificar(string propiedad)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
}
