using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FrontOne.Infrastructure.SapB1.Models;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrontOne.Infrastructure.SapB1;

public class SapServiceLayerClient : ISapServiceLayerClient
{
    // Lectura: nombres de SAP (PascalCase) se emparejan sin importar mayúsculas/minúsculas.
    private static readonly JsonSerializerOptions ReadOptions = new(JsonSerializerDefaults.Web);

    // Escritura: SAP Service Layer exige los nombres de campo exactos en PascalCase
    // (CompanyDB, UserName, ItemCode...) — sin política de camelCase, si no el login y
    // cualquier POST/PATCH fallan con "Invalid login credential" o error de parseo.
    private static readonly JsonSerializerOptions WriteOptions = new(JsonSerializerDefaults.Web) { PropertyNamingPolicy = null };

    private readonly HttpClient _httpClient;
    private readonly SapOptions _options;
    private readonly ICryptoService _cryptoService;
    private readonly ILogger<SapServiceLayerClient> _logger;
    private readonly SemaphoreSlim _sessionLock = new(1, 1);

    private SapSession? _session;

    public SapServiceLayerClient(
        HttpClient httpClient,
        IOptions<SapOptions> options,
        ICryptoService cryptoService,
        ILogger<SapServiceLayerClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _cryptoService = cryptoService;
        _logger = logger;
        _httpClient.BaseAddress ??= new Uri(_options.ServiceLayerUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        using var response = await SendWithRetryAsync(HttpMethod.Get, endpoint, null, cancellationToken);
        return await ReadContentAsync<T>(response, cancellationToken);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object body, CancellationToken cancellationToken = default)
    {
        using var response = await SendWithRetryAsync(HttpMethod.Post, endpoint, body, cancellationToken);
        return await ReadContentAsync<T>(response, cancellationToken);
    }

    public async Task PatchAsync(string endpoint, object body, CancellationToken cancellationToken = default)
    {
        using var response = await SendWithRetryAsync(HttpMethod.Patch, endpoint, body, cancellationToken);
        response.Dispose();
    }

    public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        using var response = await SendWithRetryAsync(HttpMethod.Delete, endpoint, null, cancellationToken);
        response.Dispose();
    }

    public async Task<IReadOnlyList<SapBatchResult>> BatchAsync(IEnumerable<SapBatchRequest> requests, CancellationToken cancellationToken = default)
    {
        await EnsureSessionAsync(cancellationToken);

        var boundary = $"batch_{Guid.NewGuid():N}";
        var body = BuildBatchBody(requests, boundary);

        using var request = new HttpRequestMessage(HttpMethod.Post, "$batch")
        {
            Content = new StringContent(body, Encoding.UTF8),
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/mixed") { Parameters = { new NameValueHeaderValue("boundary", boundary) } };
        request.Headers.Add("Cookie", _session!.ToCookieHeader());

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new SapException($"Error ejecutando batch SAP B1: {response.StatusCode}", httpStatusCode: (int)response.StatusCode);
        }

        return ParseBatchResponse(responseText);
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(HttpMethod method, string endpoint, object? body, CancellationToken cancellationToken)
    {
        await EnsureSessionAsync(cancellationToken);

        Exception? lastException = null;

        for (var attempt = 1; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                var response = await SendOnceAsync(method, endpoint, body, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Dispose();
                    await LoginAsync(cancellationToken, forceRenew: true);
                    response = await SendOnceAsync(method, endpoint, body, cancellationToken);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    var statusCode = (int)response.StatusCode;
                    response.Dispose();
                    throw new SapException($"Error SAP B1 en {method} {endpoint}: {content}", httpStatusCode: statusCode);
                }

                return response;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                lastException = ex;
                _logger.LogWarning(ex, "Reintento {Attempt}/{MaxRetries} en {Method} {Endpoint}", attempt, _options.MaxRetries, method, endpoint);

                if (attempt < _options.MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(attempt), cancellationToken);
                }
            }
        }

        throw new SapException($"No se pudo completar {method} {endpoint} tras {_options.MaxRetries} intentos", lastException!);
    }

    private async Task<HttpResponseMessage> SendOnceAsync(HttpMethod method, string endpoint, object? body, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, endpoint);
        request.Headers.Add("Cookie", _session!.ToCookieHeader());

        if (body is not null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(body, WriteOptions), Encoding.UTF8, "application/json");
        }

        return await _httpClient.SendAsync(request, cancellationToken);
    }

    private async Task EnsureSessionAsync(CancellationToken cancellationToken)
    {
        if (_session is { IsValid: true })
        {
            return;
        }

        await LoginAsync(cancellationToken, forceRenew: false);
    }

    private async Task LoginAsync(CancellationToken cancellationToken, bool forceRenew)
    {
        await _sessionLock.WaitAsync(cancellationToken);

        try
        {
            if (!forceRenew && _session is { IsValid: true })
            {
                return;
            }

            var payload = new
            {
                CompanyDB = _options.CompanyDb,
                UserName = _options.UserName,
                Password = string.IsNullOrEmpty(_options.PasswordEncrypted)
                    ? null
                    : _cryptoService.Decrypt(_options.PasswordEncrypted),
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "Login")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload, WriteOptions), Encoding.UTF8, "application/json"),
            };

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new SapException($"Error de login SAP B1: {responseText}", httpStatusCode: (int)response.StatusCode);
            }

            using var document = JsonDocument.Parse(responseText);
            var sessionTimeoutMinutes = document.RootElement.TryGetProperty("SessionTimeout", out var timeoutElement)
                ? timeoutElement.GetDouble()
                : 30;

            var b1Session = ExtractCookieValue(response, "B1SESSION")
                ?? throw new SapException("SAP B1 no devolvió cookie B1SESSION en el login");
            var routeId = ExtractCookieValue(response, "ROUTEID");

            _session = new SapSession
            {
                B1Session = b1Session,
                RouteId = routeId,
                ExpiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(sessionTimeoutMinutes - 1),
            };

            _logger.LogInformation("Sesión SAP B1 renovada, expira en {Minutes} minutos", sessionTimeoutMinutes);
        }
        finally
        {
            _sessionLock.Release();
        }
    }

    private static string? ExtractCookieValue(HttpResponseMessage response, string cookieName)
    {
        if (!response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            return null;
        }

        foreach (var cookie in cookies)
        {
            var prefix = $"{cookieName}=";
            if (cookie.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var end = cookie.IndexOf(';');
                var value = end < 0 ? cookie[prefix.Length..] : cookie[prefix.Length..end];
                return value;
            }
        }

        return null;
    }

    private static async Task<T?> ReadContentAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrWhiteSpace(content) ? default : JsonSerializer.Deserialize<T>(content, ReadOptions);
    }

    private static string BuildBatchBody(IEnumerable<SapBatchRequest> requests, string boundary)
    {
        var sb = new StringBuilder();

        foreach (var item in requests)
        {
            sb.AppendLine($"--{boundary}");
            sb.AppendLine("Content-Type: application/http");
            sb.AppendLine("Content-Transfer-Encoding: binary");
            sb.AppendLine();
            sb.AppendLine($"{item.Method} {item.Endpoint} HTTP/1.1");

            if (item.Body is not null)
            {
                var json = JsonSerializer.Serialize(item.Body, WriteOptions);
                sb.AppendLine("Content-Type: application/json");
                sb.AppendLine();
                sb.AppendLine(json);
            }
            else
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine($"--{boundary}--");
        return sb.ToString();
    }

    private static IReadOnlyList<SapBatchResult> ParseBatchResponse(string responseText)
    {
        var results = new List<SapBatchResult>();
        var parts = responseText.Split("HTTP/1.1 ", StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts.Skip(1))
        {
            var statusText = part[..part.IndexOf(' ')];
            var statusCode = int.TryParse(statusText, out var code) ? code : 0;

            var bodyStart = part.IndexOf("\r\n\r\n", StringComparison.Ordinal);
            var content = bodyStart >= 0 ? part[(bodyStart + 4)..].Trim() : null;

            results.Add(new SapBatchResult(statusCode, string.IsNullOrWhiteSpace(content) ? null : content));
        }

        return results;
    }
}
