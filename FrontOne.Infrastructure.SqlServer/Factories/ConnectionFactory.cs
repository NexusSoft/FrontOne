using FrontOne.Shared.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace FrontOne.Infrastructure.SqlServer.Factories;

public class ConnectionFactory : IConnectionFactory
{
    private readonly SqlOptions _options;

    public ConnectionFactory(IOptions<SqlOptions> options)
    {
        _options = options.Value;
    }

    public SqlConnection CreateConnection()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = _options.Server,
            InitialCatalog = _options.Database,
            IntegratedSecurity = _options.IntegratedSecurity,
            TrustServerCertificate = _options.TrustServerCertificate,
            ConnectTimeout = _options.CommandTimeoutSeconds,
        };

        if (!_options.IntegratedSecurity)
        {
            builder.UserID = _options.UserId ?? string.Empty;
            builder.Password = _options.Password ?? string.Empty;
        }

        return new SqlConnection(builder.ConnectionString);
    }
}
