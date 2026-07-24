using FrontOne.Shared.Configuration;
using FrontOne.Shared.Security;
using Microsoft.Win32;

namespace FrontOne.WinForms.Configuration;

public class RegistryConnectionStore : IConnectionCredentialStore
{
    private const string RootPath = @"SOFTWARE\FrontOne\ConnectionSettings";
    private readonly ICryptoService _cryptoService;

    public RegistryConnectionStore(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    public SqlConnectionCredentials? GetSqlCredentials()
    {
        using var key = Registry.CurrentUser.OpenSubKey($@"{RootPath}\SqlServer");
        if (key is null)
        {
            return null;
        }

        var server = key.GetValue("Server") as string;
        var database = key.GetValue("Database") as string;
        if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(database))
        {
            return null;
        }

        var integratedSecurity = string.Equals(key.GetValue("IntegratedSecurity") as string, "true", StringComparison.OrdinalIgnoreCase);
        var userId = key.GetValue("UserId") as string;
        var encryptedPassword = key.GetValue("PasswordEncrypted") as string;
        var password = string.IsNullOrEmpty(encryptedPassword) ? null : _cryptoService.Decrypt(encryptedPassword);

        return new SqlConnectionCredentials(server, database, integratedSecurity, string.IsNullOrEmpty(userId) ? null : userId, password);
    }

    public void SaveSqlCredentials(SqlConnectionCredentials credentials)
    {
        using var key = Registry.CurrentUser.CreateSubKey($@"{RootPath}\SqlServer", writable: true)!;
        key.SetValue("Server", credentials.Server, RegistryValueKind.String);
        key.SetValue("Database", credentials.Database, RegistryValueKind.String);
        key.SetValue("IntegratedSecurity", credentials.IntegratedSecurity.ToString(), RegistryValueKind.String);
        key.SetValue("UserId", credentials.UserId ?? string.Empty, RegistryValueKind.String);
        key.SetValue(
            "PasswordEncrypted",
            string.IsNullOrEmpty(credentials.Password) ? string.Empty : _cryptoService.Encrypt(credentials.Password),
            RegistryValueKind.String);
    }

    public SapConnectionCredentials? GetSapCredentials()
    {
        using var key = Registry.CurrentUser.OpenSubKey($@"{RootPath}\Sap");
        if (key is null)
        {
            return null;
        }

        var serviceLayerUrl = key.GetValue("ServiceLayerUrl") as string;
        var companyDb = key.GetValue("CompanyDb") as string;
        if (string.IsNullOrWhiteSpace(serviceLayerUrl) || string.IsNullOrWhiteSpace(companyDb))
        {
            return null;
        }

        var userName = key.GetValue("UserName") as string ?? string.Empty;
        var encryptedPassword = key.GetValue("PasswordEncrypted") as string;
        var password = string.IsNullOrEmpty(encryptedPassword) ? null : _cryptoService.Decrypt(encryptedPassword);

        return new SapConnectionCredentials(serviceLayerUrl, companyDb, userName, password);
    }

    public void SaveSapCredentials(SapConnectionCredentials credentials)
    {
        using var key = Registry.CurrentUser.CreateSubKey($@"{RootPath}\Sap", writable: true)!;
        key.SetValue("ServiceLayerUrl", credentials.ServiceLayerUrl, RegistryValueKind.String);
        key.SetValue("CompanyDb", credentials.CompanyDb, RegistryValueKind.String);
        key.SetValue("UserName", credentials.UserName, RegistryValueKind.String);
        key.SetValue(
            "PasswordEncrypted",
            string.IsNullOrEmpty(credentials.Password) ? string.Empty : _cryptoService.Encrypt(credentials.Password),
            RegistryValueKind.String);
    }
}
