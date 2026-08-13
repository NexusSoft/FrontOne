namespace FrontOne.Domain.DTOs;

// Fila del grid principal del módulo Incidencias: TODAS las Órdenes de Corte del rango de fecha
// filtrado, tengan o no Incidencia ya capturada (Revisada distingue ambos casos).
public record IncidenciaListadoDto(
    int OrdenCorteId,
    int? IncidenciaId,
    bool Revisada,
    string Folio,
    DateTime Fecha,
    string HuertaNombre,
    string ProductorNombre,
    string Acopiador,
    string? SupervisorNombre,
    short Cajas,
    string? Incidencias,
    string? Ajuste);
