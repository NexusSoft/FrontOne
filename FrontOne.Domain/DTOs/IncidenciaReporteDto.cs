namespace FrontOne.Domain.DTOs;

// Una fila del PDF de Incidencias del rango de fecha elegido — solo Órdenes de Corte que ya
// tienen Incidencia capturada.
public record IncidenciaReporteDto(
    string Folio,
    DateTime Fecha,
    string Acopiador,
    string? RegistroSagarpa,
    string HuertaNombre,
    short CajasEntregadas,
    string ProductorNombre,
    string Beneficiario,
    string TipoCorteNombre,
    string TipoPagoNombre,
    decimal CostoKg,
    string JefeCuadrillaNombre,
    string TransportistaNombre,
    string FloracionNombre,
    string? MunicipioNombre,
    string? PoblacionNombre,
    string? SupervisorHuertaNombre,
    string? OdcSapCosecha,
    string? NumeroTelefono,
    string? Placas,
    string? OdcSapFlete,
    string? Bascula,
    string? PuntoReunion,
    TimeSpan? HoraLlegadaHuerta,
    int? CajasCosechadas,
    string? CajaPorCuadrilla,
    string? Observaciones,
    string? Incidencias,
    string? Ajuste);
