namespace FrontOne.Domain.DTOs;

public record UsuarioDto(int Id, string NombreUsuario, string NombreCompleto, string? Email, bool Activo);
