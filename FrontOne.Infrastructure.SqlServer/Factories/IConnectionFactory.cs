using Microsoft.Data.SqlClient;

namespace FrontOne.Infrastructure.SqlServer.Factories;

public interface IConnectionFactory
{
    SqlConnection CreateConnection();
}
