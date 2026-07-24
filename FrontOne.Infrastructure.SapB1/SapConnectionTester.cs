using System.Text;
using System.Text.Json;
using FrontOne.Shared.Configuration;

namespace FrontOne.Infrastructure.SapB1;

public class SapConnectionTester : ISapConnectionTester
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SapConnectionTester(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ConnectionTestResult> TestAsync(SapConnectionCredentials credentials, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(credentials.ServiceLayerUrl) || string.IsNullOrWhiteSpace(credentials.CompanyDb))
        {
            return ConnectionTestResult.Fail("URL del Service Layer y CompanyDB son obligatorios");
        }

        using var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(15);

        var baseUrl = credentials.ServiceLayerUrl.TrimEnd('/') + "/";
        var payload = JsonSerializer.Serialize(new
        {
            CompanyDB = credentials.CompanyDb,
            UserName = credentials.UserName,
            Password = credentials.Password,
        });

        try
        {
            using var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}Login")
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json"),
            };

            using var response = await httpClient.SendAsync(loginRequest, cancellationToken);
            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var mensaje = ExtraerMensajeError(responseText) ?? $"HTTP {(int)response.StatusCode}";
                return ConnectionTestResult.Fail(mensaje);
            }

            var b1Session = ExtraerCookie(response, "B1SESSION");
            if (b1Session is not null)
            {
                using var logoutRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}Logout");
                logoutRequest.Headers.Add("Cookie", $"B1SESSION={b1Session}");
                using var logoutResponse = await httpClient.SendAsync(logoutRequest, cancellationToken);
            }

            return ConnectionTestResult.Ok();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return ConnectionTestResult.Fail(ex.Message);
        }
    }

    private static string? ExtraerMensajeError(string responseText)
    {
        try
        {
            using var document = JsonDocument.Parse(responseText);
            if (document.RootElement.TryGetProperty("error", out var error) &&
                error.TryGetProperty("message", out var message) &&
                message.TryGetProperty("value", out var value))
            {
                return value.GetString();
            }
        }
        catch (JsonException)
        {
        }

        return string.IsNullOrWhiteSpace(responseText) ? null : responseText;
    }

    private static string? ExtraerCookie(HttpResponseMessage response, string cookieName)
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
                return end < 0 ? cookie[prefix.Length..] : cookie[prefix.Length..end];
            }
        }

        return null;
    }
}
