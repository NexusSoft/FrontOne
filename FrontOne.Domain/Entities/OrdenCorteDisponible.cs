namespace FrontOne.Domain.Entities;

// Proyección ligera de Acopio.OrdenCorte para el picker "Seleccionar Orden de Corte" de
// Recepción de Fruta — solo trae órdenes que todavía no están usadas en ninguna Recepción.
public class OrdenCorteDisponible
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string HuertaNombre { get; set; } = string.Empty;
    public short CajasEntregadas { get; set; }
}
