namespace FrontOne.Domain.DTOs;

public record ListaPrecioAcarreoDto(
    int Id,
    int MunicipioId,
    string MunicipioNombre,
    string EstadoNombre,
    int ZonaId,
    string ZonaNombre,
    decimal Precio300,
    decimal Precio400,
    decimal Precio500,
    int ToleranciaKgFaltante,
    bool Activo);
