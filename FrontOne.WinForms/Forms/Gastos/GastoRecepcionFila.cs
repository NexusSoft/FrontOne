using FrontOne.Domain.DTOs;

namespace FrontOne.WinForms.Forms.Gastos;

// Fila mutable que unifica la fila base (no borrable) y las filas de ajuste del grid de
// Cosecha/Acarreo. AjusteOrigen solo va lleno cuando EsBase = false, para reenviar
// TipoAjusteId/Monto al actualizar CargoA sin perder los demás datos del ajuste.
public class GastoRecepcionFila
{
    public bool EsBase { get; set; }
    public int Id { get; set; }
    public int LoteRecepcionId { get; set; }
    public int RecepcionFrutaId { get; set; }
    public string RecepcionFolio { get; set; } = string.Empty;
    public decimal PesoNeto { get; set; }
    public decimal PesoProductor { get; set; }
    public int OrdenCorteId { get; set; }
    public string OrdenCorteFolio { get; set; } = string.Empty;
    public string Concepto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Importe { get; set; }
    public bool CargoEmpresa { get; set; }
    public bool CargoProductor { get; set; }
    public GastoRecepcionAjusteDto? AjusteOrigen { get; set; }
}
