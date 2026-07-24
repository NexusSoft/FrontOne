namespace FrontOne.Domain.DTOs;

public record ProductorDto(
    int Id,
    string Clave,
    DateTime FechaRegistro,
    string NombreProductor,
    string? Domicilio,
    string? Colonia,
    string? CodigoPostal,
    int? PoblacionId,
    int? MunicipioId,
    int? EstadoId,
    string? Rfc,
    string? Telefono,
    string? Celular,
    string? Email,
    string? Organizacion,
    string? Observaciones,
    string? Usuario,
    string? Password,
    int DiasCredito,
    bool Activo);
