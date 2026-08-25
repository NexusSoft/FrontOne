namespace FrontOne.Domain.Entities;

public class Lote
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string CodigoTrazabilidad { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public decimal Kilogramos { get; set; }
    public string? Personalizado { get; set; }
    public int LineaProduccionId { get; set; }
    public string LineaProduccionNombre { get; set; } = string.Empty;
    public decimal PorcentajeMateriaSeca { get; set; }
    public byte Estatus { get; set; }
    public int Recepciones { get; set; }
    public string? HuertaNombre { get; set; }
    public string? ProductorNombre { get; set; }
    // Campo duro (persistido), a diferencia de HuertaNombre/ProductorNombre que se derivan al
    // vuelo — se fija una sola vez al crear el Lote y nunca se recalcula (ver LoteService).
    public int? VariedadId { get; set; }
    public string? VariedadNombre { get; set; }
    public DateTime FechaCreacion { get; set; }
}
