namespace FrontOne.Domain.Entities;

// Proyección ligera de Recepcion.RecepcionFruta (+ su primera Orden de Corte) para el picker
// "Seleccionar Recepción" del módulo Lotes — solo trae Recepciones que todavía no están en
// ningún Lote, y opcionalmente filtradas a Huerta/Acuerdo/Proveedor compatibles.
public class RecepcionDisponibleParaLote
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public string? NumeroTicket { get; set; }
    public DateTime Fecha { get; set; }
    public decimal PesoNeto { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
    public string? CoprefBico { get; set; }

    // Los SPs del picker ya filtran a solo las que ya se descargaron por completo, pero
    // sp_RecepcionFruta_ObtenerParaLote (búsqueda por Id) sí regresa las incompletas para que
    // LoteService pueda distinguir entre "no existe" y "existe pero falta descargar" al validar.
    public bool DescargaCompleta { get; set; }
    public int HuertaId { get; set; }
    public string HuertaNombre { get; set; } = string.Empty;
    public int AcuerdoCorteId { get; set; }
    public string PagarCorteACardCode { get; set; } = string.Empty;
    public string PagarCorteANombre { get; set; } = string.Empty;
    public int OrdenCorteId { get; set; }
    public string OrdenCorteFolio { get; set; } = string.Empty;
    public string AcuerdoCorteFolio { get; set; } = string.Empty;
}
