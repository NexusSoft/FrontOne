using FrontOne.Shared.Configuration;

namespace FrontOne.Application.Services;

public class ConnectionSettingsService
{
    private readonly IConnectionCredentialStore _credentialStore;
    private readonly ISqlConnectionTester _sqlConnectionTester;
    private readonly ISapConnectionTester _sapConnectionTester;

    public ConnectionSettingsService(
        IConnectionCredentialStore credentialStore,
        ISqlConnectionTester sqlConnectionTester,
        ISapConnectionTester sapConnectionTester)
    {
        _credentialStore = credentialStore;
        _sqlConnectionTester = sqlConnectionTester;
        _sapConnectionTester = sapConnectionTester;
    }

    public SqlConnectionCredentials? GetSqlCredentials() => _credentialStore.GetSqlCredentials();

    public void SaveSqlCredentials(SqlConnectionCredentials credentials) => _credentialStore.SaveSqlCredentials(credentials);

    public SapConnectionCredentials? GetSapCredentials() => _credentialStore.GetSapCredentials();

    public void SaveSapCredentials(SapConnectionCredentials credentials) => _credentialStore.SaveSapCredentials(credentials);

    public Task<ConnectionTestResult> TestSqlConnectionAsync(SqlConnectionCredentials credentials, CancellationToken cancellationToken = default)
        => _sqlConnectionTester.TestAsync(credentials, cancellationToken);

    public Task<ConnectionTestResult> TestSapConnectionAsync(SapConnectionCredentials credentials, CancellationToken cancellationToken = default)
        => _sapConnectionTester.TestAsync(credentials, cancellationToken);
}
