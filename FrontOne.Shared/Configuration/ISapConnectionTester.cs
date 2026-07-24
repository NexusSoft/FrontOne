namespace FrontOne.Shared.Configuration;

public interface ISapConnectionTester
{
    Task<ConnectionTestResult> TestAsync(SapConnectionCredentials credentials, CancellationToken cancellationToken = default);
}
