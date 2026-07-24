namespace FrontOne.Domain.DTOs;

public record JefeAcopioDto(
    int Id,
    string Clave,
    string Nombre,
    string? Domicilio,
    string? Colonia,
    string? CodigoPostal,
    int? PoblacionId,
    int? MunicipioId,
    int? EstadoId,
    string? Telefono,
    string? Celular,
    string? Email,
    string? Observaciones,
    bool Activo);
