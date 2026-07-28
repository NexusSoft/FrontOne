namespace FrontOne.WinForms.Forms.Recepcion;

// Fila mutable para bindear el grid de detalle de RecepcionFrutaEditarForm — el DTO real
// (RecepcionFrutaOrdenCorteDto) es un record inmutable y no sirve para esto. DetalleId es null
// mientras la línea no se ha guardado en base de datos todavía (se guarda junto con el
// encabezado al hacer clic en Guardar).
public class FilaDetalleRecepcion
{
    public int? DetalleId { get; set; }
    public int OrdenCorteId { get; set; }
    public string OrdenCorteFolio { get; set; } = string.Empty;
    public string HuertaNombre { get; set; } = string.Empty;
    public short CajasCortadas { get; set; }
    public decimal Kilogramos { get; set; }
}
