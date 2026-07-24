using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class AcuerdoCorteRepository : SqlRepositoryBase, IAcuerdoCorteRepository
{
    private record InsertResult(int Id, string Folio);

    public AcuerdoCorteRepository(IConnectionFactory connectionFactory, ILogger<AcuerdoCorteRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<AcuerdoCorte>> ObtenerAsync(int? id = null)
        => QueryAsync<AcuerdoCorte>("Acopio.sp_AcuerdoCorte_Obtener", new { Id = id });

    public async Task<(int Id, string Folio)> InsertarAsync(AcuerdoCorte acuerdo)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Acopio.sp_AcuerdoCorte_Insertar", new
        {
            acuerdo.ProductorId,
            acuerdo.FechaInicio,
            acuerdo.FechaFin,
            acuerdo.ProductoId,
            acuerdo.VariedadId,
            acuerdo.TipoComercializacionId,
            acuerdo.TipoCorteId,
            acuerdo.Precio,
            acuerdo.ListaPrecioFecha,
            acuerdo.ListaPrecioProductorId,
            acuerdo.ListaPrecioNumero,
            acuerdo.MonedaId,
            acuerdo.Observaciones,
            acuerdo.Activo,
        });

        return (resultado!.Id, resultado.Folio);
    }

    public Task ActualizarAsync(AcuerdoCorte acuerdo)
        => ExecuteAsync("Acopio.sp_AcuerdoCorte_Actualizar", new
        {
            acuerdo.Id,
            acuerdo.ProductorId,
            acuerdo.FechaInicio,
            acuerdo.FechaFin,
            acuerdo.ProductoId,
            acuerdo.VariedadId,
            acuerdo.TipoComercializacionId,
            acuerdo.TipoCorteId,
            acuerdo.Precio,
            acuerdo.ListaPrecioFecha,
            acuerdo.ListaPrecioProductorId,
            acuerdo.ListaPrecioNumero,
            acuerdo.MonedaId,
            acuerdo.Observaciones,
            acuerdo.Activo,
        });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Acopio.sp_AcuerdoCorte_Eliminar", new { Id = id });
}
