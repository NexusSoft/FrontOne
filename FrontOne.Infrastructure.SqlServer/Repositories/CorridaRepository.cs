using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class CorridaRepository : SqlRepositoryBase, ICorridaRepository
{
    public CorridaRepository(IConnectionFactory connectionFactory, ILogger<CorridaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Corrida>> ObtenerAsync(int? id = null)
        => QueryAsync<Corrida>("Produccion.sp_Corrida_Obtener", new { Id = id });

    public Task<IReadOnlyList<LoteDisponibleParaCorridaDto>> ObtenerLotesDisponiblesAsync()
        => QueryAsync<LoteDisponibleParaCorridaDto>("Produccion.sp_Corrida_ObtenerLotesDisponibles");

    public Task<int> IniciarAsync(int loteId)
        => ExecuteScalarAsync<int>("Produccion.sp_Corrida_Insertar", new { LoteId = loteId })!;

    public Task FinalizarAsync(int id)
        => ExecuteAsync("Produccion.sp_Corrida_Finalizar", new { Id = id });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Produccion.sp_Corrida_Eliminar", new { Id = id });
}
