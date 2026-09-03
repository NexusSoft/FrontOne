using FrontOne.Shared.Configuration;
using FrontOne.Shared.Exceptions;
using Microsoft.Extensions.Options;

namespace FrontOne.Web.Configuration;

/// <summary>
/// Implementación de <see cref="IConnectionCredentialStore"/> de solo lectura para el sitio web:
/// la cadena de conexión SQL/SAP se administra desde la configuración del servidor (appsettings +
/// variables de entorno / user secrets), nunca desde una pantalla del sitio. Nunca se expone la
/// pantalla de "Configuración de conexiones" en FrontOne.Web — a diferencia de WinForms
/// (<c>RegistryConnectionStore</c>), aquí no tiene sentido que cada visitante del sitio pueda
/// cambiar a qué base de datos se conecta el servidor.
/// </summary>
public class ConfigurationConnectionCredentialStore : IConnectionCredentialStore
{
    private const string MensajeSoloLectura = "La configuración de conexiones se administra desde el servidor, no desde el sitio web.";

    private readonly SqlOptions _sqlOptions;
    private readonly SapOptions _sapOptions;

    public ConfigurationConnectionCredentialStore(IOptions<SqlOptions> sqlOptions, IOptions<SapOptions> sapOptions)
    {
        _sqlOptions = sqlOptions.Value;
        _sapOptions = sapOptions.Value;
    }

    public SqlConnectionCredentials? GetSqlCredentials()
        => new(_sqlOptions.Server, _sqlOptions.Database, _sqlOptions.IntegratedSecurity, _sqlOptions.UserId, _sqlOptions.Password);

    public void SaveSqlCredentials(SqlConnectionCredentials credentials)
        => throw new BusinessException(MensajeSoloLectura);

    public SapConnectionCredentials? GetSapCredentials()
        => new(_sapOptions.ServiceLayerUrl, _sapOptions.CompanyDb, _sapOptions.UserName, null);

    public void SaveSapCredentials(SapConnectionCredentials credentials)
        => throw new BusinessException(MensajeSoloLectura);
}
