namespace FrontOne.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string PasswordEncriptado { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int IntentosFallidos { get; set; }
    public DateTime? BloqueadoHasta { get; set; }
}
