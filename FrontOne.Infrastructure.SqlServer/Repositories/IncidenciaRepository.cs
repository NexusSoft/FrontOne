using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class IncidenciaRepository : SqlRepositoryBase, IIncidenciaRepository
{
    private record InsertResult(int Id);

    public IncidenciaRepository(IConnectionFactory connectionFactory, ILogger<IncidenciaRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<IncidenciaListadoDto>> ObtenerOrdenesConEstatusAsync(DateTime fechaDesde, DateTime fechaHasta)
        => QueryAsync<IncidenciaListadoDto>("Acopio.sp_Incidencia_ObtenerOrdenesConEstatus",
            new { FechaDesde = fechaDesde.Date, FechaHasta = fechaHasta.Date });

    public Task<IncidenciaDto?> ObtenerPorOrdenCorteIdAsync(int ordenCorteId)
        => QueryFirstAsync<IncidenciaDto>("Acopio.sp_Incidencia_ObtenerPorOrdenCorteId", new { OrdenCorteId = ordenCorteId });

    public async Task<int> InsertarAsync(Incidencia incidencia)
    {
        var resultado = await QueryFirstAsync<InsertResult>("Acopio.sp_Incidencia_Insertar", new
        {
            incidencia.OrdenCorteId,
            incidencia.SupervisorHuertaId,
            incidencia.OdcSapCosecha,
            incidencia.NumeroTelefono,
            incidencia.Placas,
            incidencia.OdcSapFlete,
            incidencia.Bascula,
            incidencia.PuntoReunion,
            incidencia.HoraLlegadaHuerta,
            incidencia.CajasCosechadas,
            incidencia.CajaPorCuadrilla,
            incidencia.Observaciones,
            incidencia.Incidencias,
            incidencia.Ajuste,
        });

        return resultado!.Id;
    }

    public Task ActualizarAsync(Incidencia incidencia)
        => ExecuteAsync("Acopio.sp_Incidencia_Actualizar", new
        {
            incidencia.Id,
            incidencia.SupervisorHuertaId,
            incidencia.OdcSapCosecha,
            incidencia.NumeroTelefono,
            incidencia.Placas,
            incidencia.OdcSapFlete,
            incidencia.Bascula,
            incidencia.PuntoReunion,
            incidencia.HoraLlegadaHuerta,
            incidencia.CajasCosechadas,
            incidencia.CajaPorCuadrilla,
            incidencia.Observaciones,
            incidencia.Incidencias,
            incidencia.Ajuste,
        });

    public Task<IReadOnlyList<IncidenciaReporteDto>> ObtenerParaReporteAsync(DateTime fechaDesde, DateTime fechaHasta)
        => QueryAsync<IncidenciaReporteDto>("Acopio.sp_Incidencia_ObtenerParaReporte",
            new { FechaDesde = fechaDesde.Date, FechaHasta = fechaHasta.Date });
}
