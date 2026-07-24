namespace FrontOne.Shared.Configuration;

public interface ISqlConnectionTester
{
    Task<ConnectionTestResult> TestAsync(SqlConnectionCredentials credentials, CancellationToken cancellationToken = default);
}
