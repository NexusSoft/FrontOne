namespace FrontOne.Domain.Entities;

public class Lote
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public decimal Kilogramos { get; set; }
    public string? Personalizado { get; set; }
    public int LineaProduccionId { get; set; }
    public string LineaProduccionNombre { get; set; } = string.Empty;
    public decimal PorcentajeMateriaSeca { get; set; }
    public byte Estatus { get; set; }
    public int Tickets { get; set; }
    public string? HuertaNombre { get; set; }
    public string? ProductorNombre { get; set; }
    public DateTime FechaCreacion { get; set; }
}
