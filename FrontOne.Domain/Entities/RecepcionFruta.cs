namespace FrontOne.Domain.Entities;

public class RecepcionFruta
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public string? NoLote { get; set; }
    public DateTime Fecha { get; set; }
    public string Chofer { get; set; } = string.Empty;
    public string? Placas { get; set; }
    public string? Observaciones { get; set; }
    public string? NumeroTicket { get; set; }
    public string? CoprefBico { get; set; }
    public decimal PesoBruto { get; set; }
    public decimal PesoTara { get; set; }
    public decimal TaraCajas { get; set; }
    public decimal PesoMuestra { get; set; }
    public decimal PesoNeto { get; set; }
    public decimal PesoProductor { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
    public short CajasEntregadas { get; set; }
    public short CajasCortadas { get; set; }
    public short CajasRecibidasVacias { get; set; }
    public short CajasDiferencia { get; set; }
    public bool CamionDestarado { get; set; }
    public byte[]? TicketPesadaArchivo { get; set; }
    public string? TicketPesadaNombreArchivo { get; set; }
    public string? Huertas { get; set; }
    public DateTime FechaCreacion { get; set; }
}
