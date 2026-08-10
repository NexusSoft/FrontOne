using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class PalletRepository : SqlRepositoryBase, IPalletRepository
{
    public PalletRepository(IConnectionFactory connectionFactory, ILogger<PalletRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Pallet>> ObtenerAsync(int? id = null)
        => QueryAsync<Pallet>("Produccion.sp_Pallet_Obtener", new { Id = id });

    public Task<IReadOnlyList<PalletDetalle>> ObtenerDetalleAsync(int palletId)
        => QueryAsync<PalletDetalle>("Produccion.sp_Pallet_ObtenerDetalle", new { PalletId = palletId });

    public Task<IReadOnlyList<LoteEnProcesoParaPalletDto>> ObtenerLotesEnProcesoAsync(int? lineaProduccionId = null)
        => QueryAsync<LoteEnProcesoParaPalletDto>("Produccion.sp_Pallet_ObtenerLotesEnProceso",
            new { LineaProduccionId = lineaProduccionId });

    public Task<PalletReporteDto?> ObtenerParaReporteAsync(int id)
        => QueryFirstAsync<PalletReporteDto>("Produccion.sp_Pallet_ObtenerParaReporte", new { Id = id });

    public Task<int> InsertarAsync(int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal)
        => ExecuteScalarAsync<int>("Produccion.sp_Pallet_Insertar", new
        {
            LineaProduccionId = lineaProduccionId,
            EsMixto = esMixto,
            ProductoTerminadoId = productoTerminadoId,
            PesoReal = pesoReal,
        })!;

    public Task ActualizarEncabezadoAsync(int id, int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal)
        => ExecuteAsync("Produccion.sp_Pallet_ActualizarEncabezado", new
        {
            Id = id,
            LineaProduccionId = lineaProduccionId,
            EsMixto = esMixto,
            ProductoTerminadoId = productoTerminadoId,
            PesoReal = pesoReal,
        });

    public Task BloquearAsync(int id)
        => ExecuteAsync("Produccion.sp_Pallet_Bloquear", new { Id = id });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Produccion.sp_Pallet_Eliminar", new { Id = id });

    public Task<int> InsertarDetalleAsync(int palletId, int corridaId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
        => ExecuteScalarAsync<int>("Produccion.sp_PalletDetalle_Insertar", new
        {
            PalletId = palletId,
            CorridaId = corridaId,
            ProductoTerminadoId = productoTerminadoId,
            Cajas = cajas,
            Kilogramos = kilogramos,
            PorcentajeMateriaSeca = porcentajeMateriaSeca,
        })!;

    public Task ActualizarDetalleAsync(int id, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
        => ExecuteAsync("Produccion.sp_PalletDetalle_Actualizar", new
        {
            Id = id,
            ProductoTerminadoId = productoTerminadoId,
            Cajas = cajas,
            Kilogramos = kilogramos,
            PorcentajeMateriaSeca = porcentajeMateriaSeca,
        });

    public Task EliminarDetalleAsync(int id)
        => ExecuteAsync("Produccion.sp_PalletDetalle_Eliminar", new { Id = id });
}
