namespace FrontOne.Domain.Entities;

// Captura de campo de una Orden de Corte (llegada a huerta, cajas cosechadas, incidencias,
// ajustes, datos SAP de flete/cosecha). Relación 1 a 1 con Acopio.OrdenCorte — los campos que ya
// existen en la Orden de Corte (Folio, Fecha, Huerta, Productor, etc.) no se duplican aquí.
public class Incidencia
{
    public int Id { get; set; }
    public int OrdenCorteId { get; set; }
    public int? SupervisorHuertaId { get; set; }
    public string? OdcSapCosecha { get; set; }
    public string? NumeroTelefono { get; set; }
    public string? Placas { get; set; }
    public string? OdcSapFlete { get; set; }
    public string? Bascula { get; set; }
    public string? PuntoReunion { get; set; }
    public TimeSpan? HoraLlegadaHuerta { get; set; }
    public int? CajasCosechadas { get; set; }
    public string? CajaPorCuadrilla { get; set; }
    public string? Observaciones { get; set; }
    public string? Incidencias { get; set; }
    public string? Ajuste { get; set; }
    public DateTime FechaCreacion { get; set; }
}
