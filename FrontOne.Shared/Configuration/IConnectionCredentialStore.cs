namespace FrontOne.Shared.Configuration;

public interface IConnectionCredentialStore
{
    SqlConnectionCredentials? GetSqlCredentials();
    void SaveSqlCredentials(SqlConnectionCredentials credentials);
    SapConnectionCredentials? GetSapCredentials();
    void SaveSapCredentials(SapConnectionCredentials credentials);
}
