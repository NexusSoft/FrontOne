namespace FrontOne.Infrastructure.SapB1.Models;

public record SapBatchRequest(string Method, string Endpoint, object? Body = null);

public record SapBatchResult(int StatusCode, string? Content);
