namespace FrontOne.Domain.Entities;

// Proyección cruda de Gastos.sp_GastoLote_ObtenerEncabezado — entidad mutable a propósito
// (no record): Dapper falla el constructor-matching de records con muchos parámetros nullable
// de tipo valor (DateTime?/int?/bool?/byte?) en resultados con OUTER APPLY/LEFT JOIN. Mismo
// criterio que Corrida/CorridaDto — GastoLoteService.MapearEncabezadoDto arma el DTO a mano.
public class GastoLoteEncabezado
{
    public int LoteId { get; set; }
    public string LoteFolio { get; set; } = string.Empty;
    public string CodigoTrazabilidad { get; set; } = string.Empty;
    public DateTime? FechaCorrida { get; set; }
    public decimal Kilogramos { get; set; }
    public string? HuertaNombre { get; set; }
    public string? RegistroSagarpa { get; set; }
    public int? VariedadId { get; set; }
    public string? VariedadNombre { get; set; }
    public string? TipoCorteNombre { get; set; }
    public string? TipoPagoNombre { get; set; }
    public bool? NecesitaListaPrecios { get; set; }
    public decimal? Precio { get; set; }
    public DateTime? ListaPrecioFecha { get; set; }
    public string? ListaPrecioProductorNombre { get; set; }
    public int? ListaPrecioNumero { get; set; }
    public int GastoLoteId { get; set; }
    public DateTime? CostoEstimadoListaPrecioFecha { get; set; }
    public int? CostoEstimadoListaPrecioProductorId { get; set; }
    public string? CostoEstimadoListaPrecioProductorNombre { get; set; }
    public byte? CostoEstimadoListaPrecioNumero { get; set; }
}
