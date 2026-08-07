using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ConfiguracionBasculaRepository : SqlRepositoryBase, IConfiguracionBasculaRepository
{
    public ConfiguracionBasculaRepository(IConnectionFactory connectionFactory, ILogger<ConfiguracionBasculaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public async Task<ConfiguracionBascula> ObtenerAsync()
        => (await QueryFirstAsync<ConfiguracionBascula>("Configuracion.sp_Bascula_Obtener"))!;

    public Task ActualizarAsync(ConfiguracionBascula bascula)
        => ExecuteAsync("Configuracion.sp_Bascula_Actualizar", new
        {
            bascula.Puerto,
            bascula.BaudRate,
            bascula.Parity,
            bascula.DataBits,
            bascula.StopBits,
            bascula.PatronLectura,
        });
}
