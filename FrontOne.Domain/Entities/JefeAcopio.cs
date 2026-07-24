namespace FrontOne.Domain.Entities;

public class JefeAcopio
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Domicilio { get; set; }
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public int? PoblacionId { get; set; }
    public int? MunicipioId { get; set; }
    public int? EstadoId { get; set; }
    public string? Telefono { get; set; }
    public string? Celular { get; set; }
    public string? Email { get; set; }
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
}
