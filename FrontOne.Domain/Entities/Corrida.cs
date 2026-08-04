namespace FrontOne.Domain.Entities;

public class Corrida
{
    public int Id { get; set; }
    public int LoteId { get; set; }
    public string LoteFolio { get; set; } = string.Empty;
    public string CodigoTrazabilidad { get; set; } = string.Empty;
    public decimal Kilogramos { get; set; }
    public string? HuertaNombre { get; set; }
    public string? RegistroSagarpa { get; set; }
    public string? ProductorNombre { get; set; }
    public string? Beneficiario { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public byte Estatus { get; set; }
    public decimal KilosAProcesar { get; set; }
    public decimal KilosProcesados { get; set; }
    public DateTime FechaCreacion { get; set; }
}
