namespace FrontOne.Domain.Entities;

public class EmpresaConfiguracion
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string? Domicilio { get; set; }
    public string? Rfc { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public byte[]? Logo { get; set; }
    public string? NumeroEmpaque { get; set; }
    public byte[]? LogoUsdaOrganic { get; set; }
    public DateTime FechaModificacion { get; set; }
}
