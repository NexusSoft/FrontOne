namespace FrontOne.Domain.Entities;

public class GastoFrutaCategoria
{
    public int Id { get; set; }
    public int GastoLoteId { get; set; }
    public string MateriaPrimaItemCode { get; set; } = string.Empty;
    public decimal? CostoRealUnitario { get; set; }
    public decimal? CostoEstimadoUnitario { get; set; }
}
