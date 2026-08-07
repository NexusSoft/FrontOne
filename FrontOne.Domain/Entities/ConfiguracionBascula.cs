namespace FrontOne.Domain.Entities;

// Singleton de configuración (Id = 1), mismo criterio que EmpresaConfiguracion. Parity y
// StopBits guardan el valor numérico de los enums System.IO.Ports.Parity/StopBits — el enum en
// sí vive en WinForms, que es la capa que abre el puerto.
public class ConfiguracionBascula
{
    public int Id { get; set; }
    public string Puerto { get; set; } = string.Empty;
    public int BaudRate { get; set; }
    public byte Parity { get; set; }
    public byte DataBits { get; set; }
    public byte StopBits { get; set; }
    public string? PatronLectura { get; set; }
    public DateTime FechaModificacion { get; set; }
}
