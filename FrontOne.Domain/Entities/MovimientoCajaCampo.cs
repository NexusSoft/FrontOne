namespace FrontOne.Domain.Entities;

public class MovimientoCajaCampo
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int CajaCampoId { get; set; }
    public string Cuenta { get; set; } = string.Empty;
    public string TipoMovimiento { get; set; } = string.Empty;
    public short Cantidad { get; set; }
    public string OrigenModulo { get; set; } = string.Empty;
    public int? OrigenId { get; set; }
    public string? Observaciones { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
