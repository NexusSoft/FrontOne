namespace FrontOne.Infrastructure.SapB1.Models;

internal class SapSession
{
    public required string B1Session { get; init; }
    public string? RouteId { get; init; }
    public DateTimeOffset ExpiresAtUtc { get; init; }

    public bool IsValid => DateTimeOffset.UtcNow < ExpiresAtUtc;

    public string ToCookieHeader()
        => RouteId is null ? $"B1SESSION={B1Session}" : $"B1SESSION={B1Session}; ROUTEID={RouteId}";
}
