namespace FrontOne.Shared.Configuration;

public record SqlConnectionCredentials(string Server, string Database, bool IntegratedSecurity, string? UserId, string? Password);
