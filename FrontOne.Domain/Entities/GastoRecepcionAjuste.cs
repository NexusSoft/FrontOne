namespace FrontOne.Domain.Entities;

public class GastoRecepcionAjuste
{
    public int Id { get; set; }
    public int GastoLoteId { get; set; }
    public int LoteRecepcionId { get; set; }
    public int TipoAjusteId { get; set; }
    public decimal Monto { get; set; }
    public byte CargoA { get; set; }
    public DateTime FechaCreacion { get; set; }
}
