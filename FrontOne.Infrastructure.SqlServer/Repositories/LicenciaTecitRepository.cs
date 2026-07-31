using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class LicenciaTecitRepository : SqlRepositoryBase, ILicenciaTecitRepository
{
    public LicenciaTecitRepository(IConnectionFactory connectionFactory, ILogger<LicenciaTecitRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public async Task<LicenciaTecit> ObtenerAsync()
        => (await QueryFirstAsync<LicenciaTecit>("Configuracion.sp_LicenciaTecit_Obtener"))!;

    public Task ActualizarAsync(LicenciaTecit licencia)
        => ExecuteAsync("Configuracion.sp_LicenciaTecit_Actualizar", new
        {
            licencia.Licenciatario,
            licencia.ClaveLicencia,
            licencia.TipoLicencia,
            licencia.NumeroLicencias,
            licencia.Producto,
        });
}
