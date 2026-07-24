namespace FrontOne.Domain.Entities;

public class TipoCorte
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    // Gramaje permitido fuera de norma (en gramos).
    public decimal FueraDeNormaGr { get; set; }
    public bool DanioMinimo { get; set; }
    public int TipoPagoId { get; set; }
    public bool Activo { get; set; }
    // Nombre del tipo de pago (join en sp_TipoCorte_Obtener, solo lectura para el grid).
    public string TipoPagoNombre { get; set; } = string.Empty;
}
