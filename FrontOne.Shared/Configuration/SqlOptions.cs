namespace FrontOne.Shared.Configuration;

public class SqlOptions
{
    public const string SectionName = "Sql";

    public string Server { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public bool IntegratedSecurity { get; set; } = true;
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public bool TrustServerCertificate { get; set; } = true;
    public int CommandTimeoutSeconds { get; set; } = 30;
}
