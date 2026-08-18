namespace FrontOne.Domain.Constants;

// Nombres visibles de las 3 columnas de precio de Acopio.ListaPrecioFruta (Lista1/Lista2/Lista3)
// — el número (1/2/3) sigue siendo lo que se persiste en AcuerdoCorte.ListaPrecioNumero y
// Gastos.GastoLote.CostoEstimadoListaPrecioNumero, esto es solo la etiqueta que ve el usuario en
// combos/columnas (AcuerdoCorteEditarForm, ListaPrecioFrutaForm, GastoLoteForm).
public static class ListasPrecioFruta
{
    public static IReadOnlyList<string> Nombres { get; } = ["Convencional", "Orgánica", "Nacional"];
}
