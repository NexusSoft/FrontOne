namespace FrontOne.Domain.DTOs;

public record EmpresaConfiguracionDto(
    string RazonSocial,
    string? Domicilio,
    string? Rfc,
    string? Telefono,
    string? Correo,
    byte[]? Logo,
    string? NumeroEmpaque);
