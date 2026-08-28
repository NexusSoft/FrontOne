namespace FrontOne.Domain.Entities;

// Fila del listado de Producción (FrontOne.Web) — shape de Produccion.sp_Lote_ObtenerParaListado.
public class LoteProduccionResumen
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string? HuertaNombre { get; set; }
    public string? ProductorNombre { get; set; }
    public string? Beneficiario { get; set; }
    public decimal KilosRecibidos { get; set; }
    public decimal KilosProcesados { get; set; }
    public int Recepciones { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
    public byte Estatus { get; set; }
}
