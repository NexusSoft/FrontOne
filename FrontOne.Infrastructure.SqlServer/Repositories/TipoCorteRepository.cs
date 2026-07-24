using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class TipoCorteRepository : SqlRepositoryBase, ITipoCorteRepository
{
    public TipoCorteRepository(IConnectionFactory connectionFactory, ILogger<TipoCorteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<TipoCorte>> ObtenerAsync(int? id = null)
        => QueryAsync<TipoCorte>("Acopio.sp_TipoCorte_Obtener", new { Id = id });

    public async Task<int> InsertarAsync(TipoCorte tipoCorte)
        => await QueryFirstAsync<int>("Acopio.sp_TipoCorte_Insertar", new
        {
            tipoCorte.Nombre,
            tipoCorte.FueraDeNormaGr,
            tipoCorte.DanioMinimo,
            tipoCorte.TipoPagoId,
            tipoCorte.Activo,
        });

    public Task ActualizarAsync(TipoCorte tipoCorte)
        => ExecuteAsync("Acopio.sp_TipoCorte_Actualizar", new
        {
            tipoCorte.Id,
            tipoCorte.Nombre,
            tipoCorte.FueraDeNormaGr,
            tipoCorte.DanioMinimo,
            tipoCorte.TipoPagoId,
            tipoCorte.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_TipoCorte_Eliminar", new { Id = id });
}
