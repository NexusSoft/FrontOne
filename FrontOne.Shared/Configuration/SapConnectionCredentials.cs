namespace FrontOne.Shared.Configuration;

public record SapConnectionCredentials(string ServiceLayerUrl, string CompanyDb, string UserName, string? Password);
