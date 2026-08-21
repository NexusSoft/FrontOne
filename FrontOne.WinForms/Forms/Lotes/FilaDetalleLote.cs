namespace FrontOne.WinForms.Forms.Lotes;

// Fila mutable para bindear el grid de detalle de LoteEditarForm — DetalleId es null mientras la
// línea no se ha guardado en base de datos todavía (se guarda junto con el encabezado al hacer
// clic en Guardar). Mismo criterio que FilaDetalleRecepcion.
public class FilaDetalleLote
{
    public int? DetalleId { get; set; }
    public int RecepcionFrutaId { get; set; }
    public string RecepcionFrutaFolio { get; set; } = string.Empty;
    public string? NumeroTicket { get; set; }
    public DateTime Fecha { get; set; }
    public string? CoprefBico { get; set; }
    public decimal PesoNeto { get; set; }
    public decimal PorcentajeMateriaSeca { get; set; }
    public int HuertaId { get; set; }
    public int AcuerdoCorteId { get; set; }
    public string PagarCorteACardCode { get; set; } = string.Empty;
    public string? OrdenCorteFolio { get; set; }
    public string? AcuerdoCorteFolio { get; set; }

    // Solo se usa de la primera fila agregada, para mandarle la Variedad a LoteService.CrearAsync
    // al guardar (Lote.VariedadId es un campo duro del encabezado, ver LoteEditarForm.BtnGuardar_Click).
    public int VariedadId { get; set; }
    public string? VariedadNombre { get; set; }
}
