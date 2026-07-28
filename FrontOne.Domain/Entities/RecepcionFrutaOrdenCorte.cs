namespace FrontOne.Domain.Entities;

public class RecepcionFrutaOrdenCorte
{
    public int Id { get; set; }
    public int RecepcionFrutaId { get; set; }
    public int OrdenCorteId { get; set; }
    public string OrdenCorteFolio { get; set; } = string.Empty;
    public string HuertaNombre { get; set; } = string.Empty;
    public short CajasCortadas { get; set; }
    public decimal Kilogramos { get; set; }
}
