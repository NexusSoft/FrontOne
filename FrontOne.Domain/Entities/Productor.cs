namespace FrontOne.Domain.Entities;

public class Productor
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public string NombreProductor { get; set; } = string.Empty;
    public string? Domicilio { get; set; }
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public int? PoblacionId { get; set; }
    public int? MunicipioId { get; set; }
    public int? EstadoId { get; set; }
    public string? Rfc { get; set; }
    public string? Telefono { get; set; }
    public string? Celular { get; set; }
    public string? Email { get; set; }
    public string? Organizacion { get; set; }
    public string? Observaciones { get; set; }
    public string? Usuario { get; set; }
    public string? PasswordEncriptado { get; set; }
    public int DiasCredito { get; set; }
    public bool Activo { get; set; }
}
