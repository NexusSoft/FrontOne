namespace FrontOne.Domain.DTOs;

public record EvaluacionAcarreoDto(
    int Id,
    DateTime Fecha,
    string HuertaNombre,
    string? RegistroSagarpa,
    string? PoblacionNombre,
    string? NombreBascula,
    string TransportistaNombre,
    string? Placas,
    string Chofer,
    short CajasEntregadas,
    short CajasCortadas,
    short CajasRecibidasVacias,
    decimal PesoNeto,
    decimal PesoProductor,
    decimal PesoDiferencia);
