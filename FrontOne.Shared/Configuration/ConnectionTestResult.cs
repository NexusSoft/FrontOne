namespace FrontOne.Shared.Configuration;

public record ConnectionTestResult(bool Success, string? Error)
{
    public static ConnectionTestResult Ok() => new(true, null);
    public static ConnectionTestResult Fail(string error) => new(false, error);
}
