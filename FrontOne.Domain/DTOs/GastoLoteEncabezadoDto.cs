namespace FrontOne.Domain.DTOs;

// Encabezado de la pantalla Gastos: todo lo que no es CostoEstimadoListaPrecio* se deriva en
// vivo del Lote/Corrida/OrdenCorte/AcuerdoCorte (ver Gastos.sp_GastoLote_ObtenerEncabezado).
public record GastoLoteEncabezadoDto(
    int LoteId,
    string LoteFolio,
    string CodigoTrazabilidad,
    DateTime? FechaCorrida,
    decimal Kilogramos,
    string? HuertaNombre,
    string? RegistroSagarpa,
    int? VariedadId,
    string? VariedadNombre,
    string? TipoCorteNombre,
    string? TipoPagoNombre,
    bool? NecesitaListaPrecios,
    decimal? Precio,
    DateTime? ListaPrecioFecha,
    string? ListaPrecioProductorNombre,
    int? ListaPrecioNumero,
    int GastoLoteId,
    DateTime? CostoEstimadoListaPrecioFecha,
    int? CostoEstimadoListaPrecioProductorId,
    string? CostoEstimadoListaPrecioProductorNombre,
    byte? CostoEstimadoListaPrecioNumero)
{
    // Fijo: precio directo del Acuerdo. Banda: nombre de la vigencia elegida (fecha + productor,
    // o "General" si es la lista general sin productor específico).
    public string PrecioAcordadoTexto => NecesitaListaPrecios == false
        ? Precio?.ToString("C2") ?? string.Empty
        : ListaPrecioFecha is { } fecha
            ? $"{fecha:dd/MM/yyyy} - {ListaPrecioProductorNombre ?? "General"}"
            : string.Empty;
}
