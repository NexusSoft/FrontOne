namespace FrontOne.Domain.DTOs;

// DTO de captura de una Incidencia — trae siempre los campos derivados de la Orden de Corte (de
// solo lectura en la UI, resueltos por JOIN a Huerta/Productor/Municipio/Población/etc.) más los
// campos propios de Incidencia (captura libre). Id es null mientras la Orden de Corte todavía no
// tiene Incidencia capturada (primera vez que se abre el formulario para esa orden).
public record IncidenciaDto(
    int? Id,
    int OrdenCorteId,
    // Solo lectura — derivados de Orden de Corte / Huerta.
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
    // Captura propia de Incidencia.
    int? SupervisorHuertaId,
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
