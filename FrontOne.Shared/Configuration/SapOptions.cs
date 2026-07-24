namespace FrontOne.Shared.Configuration;

public class SapOptions
{
    public const string SectionName = "Sap";

    public string ServiceLayerUrl { get; set; } = string.Empty;
    public string CompanyDb { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    /// <summary>Contraseña cifrada con ICryptoService. Nunca guardar texto plano.</summary>
    public string? PasswordEncrypted { get; set; }

    public int TimeoutSeconds { get; set; } = 30;
    public int MaxRetries { get; set; } = 3;
}
