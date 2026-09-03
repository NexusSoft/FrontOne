namespace FrontOne.Domain.DTOs;

public record ReempaqueDto(
    int Id,
    string Folio,
    DateTime FechaCreacion,
    TimeSpan HoraCreacion,
    string Motivo,
    byte Estatus,
    decimal KilosAProcesar,
    decimal KilosProcesados,
    decimal Diferencia,
    DateTime? FechaCierre,
    DateTime FechaCreacionRegistro);
