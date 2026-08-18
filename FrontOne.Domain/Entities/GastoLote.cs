namespace FrontOne.Domain.Entities;

public class GastoLote
{
    public int Id { get; set; }
    public int LoteId { get; set; }
    public DateTime? CostoEstimadoListaPrecioFecha { get; set; }
    public int? CostoEstimadoListaPrecioProductorId { get; set; }
    public byte? CostoEstimadoListaPrecioNumero { get; set; }
    public DateTime FechaCreacion { get; set; }
}
