using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class EstimacionRepository : SqlRepositoryBase, IEstimacionRepository
{
    private record InsertResult(int Id, string Folio);

    public EstimacionRepository(IConnectionFactory connectionFactory, ILogger<EstimacionRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Estimacion>> ObtenerAsync(int? id = null)
        => QueryAsync<Estimacion>("Acopio.sp_Estimacion_Obtener", new { Id = id });

    public Task<Estimacion?> ObtenerPorFolioAsync(string folio)
        => QueryFirstAsync<Estimacion>("Acopio.sp_Estimacion_ObtenerPorFolio", new { Folio = folio });

    public Task<IReadOnlyList<EstimacionBusquedaDto>> ObtenerTop100Async()
        => QueryAsync<EstimacionBusquedaDto>("Acopio.sp_Estimacion_ObtenerTop100");

    public Task<IReadOnlyList<EstimacionBusquedaDto>> BuscarAsync(string filtro)
        => QueryAsync<EstimacionBusquedaDto>("Acopio.sp_Estimacion_Buscar", new { Filtro = filtro });

    public async Task<(int Id, string Folio)> InsertarAsync(Estimacion estimacion)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Acopio.sp_Estimacion_Insertar", new
        {
            estimacion.Fecha,
            estimacion.HuertaId,
            estimacion.RegistroSagarpa,
            estimacion.Kilos,
            estimacion.AcopiadorId,
            estimacion.AcopiadorNombre,
            estimacion.PorcentajeCat1,
            estimacion.PorcentajeCat2,
            estimacion.PorcentajeNal,
            estimacion.PorcentajeCalibre32,
            estimacion.PorcentajeCalibre36,
            estimacion.PorcentajeCalibre40,
            estimacion.PorcentajeCalibre48,
            estimacion.PorcentajeCalibre60,
            estimacion.PorcentajeCalibre70,
            estimacion.PorcentajeCalibre84,
            estimacion.PorcentajeCalibre90,
            estimacion.PorcentajeBorona,
            estimacion.PorcentajeCanica,
            estimacion.PorcentajeCuarta,
            estimacion.PorcentajeDesecho,
            estimacion.PorcentajeProceso,
            estimacion.ListaPrecioFecha,
            estimacion.ListaPrecioProductorId,
            estimacion.TipoLista,
            estimacion.PrecioSugerido,
        });

        return (resultado!.Id, resultado.Folio);
    }

    public Task ActualizarAsync(Estimacion estimacion)
        => ExecuteAsync("Acopio.sp_Estimacion_Actualizar", new
        {
            estimacion.Id,
            estimacion.Fecha,
            estimacion.HuertaId,
            estimacion.RegistroSagarpa,
            estimacion.Kilos,
            estimacion.AcopiadorId,
            estimacion.AcopiadorNombre,
            estimacion.PorcentajeCat1,
            estimacion.PorcentajeCat2,
            estimacion.PorcentajeNal,
            estimacion.PorcentajeCalibre32,
            estimacion.PorcentajeCalibre36,
            estimacion.PorcentajeCalibre40,
            estimacion.PorcentajeCalibre48,
            estimacion.PorcentajeCalibre60,
            estimacion.PorcentajeCalibre70,
            estimacion.PorcentajeCalibre84,
            estimacion.PorcentajeCalibre90,
            estimacion.PorcentajeBorona,
            estimacion.PorcentajeCanica,
            estimacion.PorcentajeCuarta,
            estimacion.PorcentajeDesecho,
            estimacion.PorcentajeProceso,
            estimacion.ListaPrecioFecha,
            estimacion.ListaPrecioProductorId,
            estimacion.TipoLista,
            estimacion.PrecioSugerido,
        });

    public Task MarcarCerradaAsync(int id)
        => ExecuteAsync("Acopio.sp_Estimacion_MarcarCerrada", new { Id = id });
}
