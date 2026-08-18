namespace FrontOne.Domain.Entities;

public class GastoRecepcion
{
    public int Id { get; set; }
    public int GastoLoteId { get; set; }
    public int LoteRecepcionId { get; set; }
    public byte TipoGasto { get; set; }
    public byte CargoA { get; set; }
}
