namespace FrontOne.Domain.Entities;

public class LoteRecepcion
{
    public int Id { get; set; }
    public int LoteId { get; set; }
    public int RecepcionFrutaId { get; set; }
    public string RecepcionFrutaFolio { get; set; } = string.Empty;
    public string? NumeroTicket { get; set; }
    public DateTime Fecha { get; set; }
    public string? CoprefBico { get; set; }
    public decimal PesoNeto { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
}
