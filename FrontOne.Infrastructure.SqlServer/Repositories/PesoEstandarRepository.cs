using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class PesoEstandarRepository : SqlRepositoryBase, IPesoEstandarRepository
{
    private record InsertResult(int Id, string Codigo);

    public PesoEstandarRepository(IConnectionFactory connectionFactory, ILogger<PesoEstandarRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<PesoEstandar>> ObtenerAsync(int? id = null)
        => QueryAsync<PesoEstandar>("Catalogos.sp_PesoEstandar_Obtener", new { Id = id });

    public async Task<(int Id, string Codigo)> InsertarAsync(PesoEstandar pesoEstandar)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Catalogos.sp_PesoEstandar_Insertar", new
        {
            pesoEstandar.Descripcion,
            pesoEstandar.PesoNeto,
            pesoEstandar.PesoPromedio,
        });

        return (resultado!.Id, resultado.Codigo);
    }

    public Task ActualizarAsync(PesoEstandar pesoEstandar)
        => ExecuteAsync("Catalogos.sp_PesoEstandar_Actualizar", new
        {
            pesoEstandar.Id,
            pesoEstandar.Descripcion,
            pesoEstandar.PesoNeto,
            pesoEstandar.PesoPromedio,
            pesoEstandar.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Catalogos.sp_PesoEstandar_Eliminar", new { Id = id });
}
