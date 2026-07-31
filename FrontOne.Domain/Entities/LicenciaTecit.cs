namespace FrontOne.Domain.Entities;

public class LicenciaTecit
{
    public int Id { get; set; }
    public string Licenciatario { get; set; } = string.Empty;
    public string? ClaveLicencia { get; set; }
    public string? TipoLicencia { get; set; }
    public int? NumeroLicencias { get; set; }
    public string? Producto { get; set; }
    public DateTime FechaModificacion { get; set; }
}
