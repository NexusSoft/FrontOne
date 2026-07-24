namespace FrontOne.Domain.DTOs;

public record UsuarioAdminDto(
    int Id,
    string NombreUsuario,
    string NombreCompleto,
    string? Email,
    string? Password,
    bool Activo,
    IReadOnlyList<int> RolIds);
