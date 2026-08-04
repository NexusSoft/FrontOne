namespace FrontOne.Domain.Entities;

// Suma de Recepcion.RecepcionFruta.CajasPerdidas del mes, agrupada por el color de Caja de
// Campo de la Orden de Corte de cada Recepción — alimenta la columna "Pérdida del Mes" del
// dashboard del Almacén.
public class PerdidaCajaCampoMes
{
    public int CajaCampoId { get; set; }
    public int CajasPerdidas { get; set; }
}
