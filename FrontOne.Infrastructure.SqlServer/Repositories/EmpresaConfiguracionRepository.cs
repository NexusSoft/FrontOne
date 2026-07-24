using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class EmpresaConfiguracionRepository : SqlRepositoryBase, IEmpresaConfiguracionRepository
{
    public EmpresaConfiguracionRepository(IConnectionFactory connectionFactory, ILogger<EmpresaConfiguracionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public async Task<EmpresaConfiguracion> ObtenerAsync()
        => (await QueryFirstAsync<EmpresaConfiguracion>("Configuracion.sp_Empresa_Obtener"))!;

    public Task ActualizarAsync(EmpresaConfiguracion empresa)
        => ExecuteAsync("Configuracion.sp_Empresa_Actualizar", new
        {
            empresa.RazonSocial,
            empresa.Domicilio,
            empresa.Rfc,
            empresa.Telefono,
            empresa.Correo,
            empresa.Logo,
        });
}
