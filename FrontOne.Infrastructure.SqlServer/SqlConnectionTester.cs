using FrontOne.Shared.Configuration;
using Microsoft.Data.SqlClient;

namespace FrontOne.Infrastructure.SqlServer;

public class SqlConnectionTester : ISqlConnectionTester
{
    public async Task<ConnectionTestResult> TestAsync(SqlConnectionCredentials credentials, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(credentials.Server) || string.IsNullOrWhiteSpace(credentials.Database))
        {
            return ConnectionTestResult.Fail("Servidor y base de datos son obligatorios");
        }

        if (!credentials.IntegratedSecurity && string.IsNullOrWhiteSpace(credentials.UserId))
        {
            return ConnectionTestResult.Fail("Usuario es obligatorio cuando no se usa autenticación de Windows");
        }

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = credentials.Server,
            InitialCatalog = credentials.Database,
            IntegratedSecurity = credentials.IntegratedSecurity,
            TrustServerCertificate = true,
            ConnectTimeout = 5,
        };

        if (!credentials.IntegratedSecurity)
        {
            builder.UserID = credentials.UserId ?? string.Empty;
            builder.Password = credentials.Password ?? string.Empty;
        }

        try
        {
            using var connection = new SqlConnection(builder.ConnectionString);
            await connection.OpenAsync(cancellationToken);
            return ConnectionTestResult.Ok();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return ConnectionTestResult.Fail(ex.Message);
        }
    }
}
