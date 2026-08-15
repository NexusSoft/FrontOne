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

    public Task<PalletUltimaModificacionDto> ObtenerUltimaModificacionAsync()
        => QueryFirstAsync<PalletUltimaModificacionDto>("Produccion.sp_Pallet_ObtenerUltimaModificacion", new { })!;

    public Task<EtiquetaCajaDatosDto?> ObtenerDatosEtiquetaCajaAsync(int palletId)
        => QueryFirstAsync<EtiquetaCajaDatosDto>("Produccion.sp_Pallet_ObtenerEtiquetaCaja", new { PalletId = palletId });

    public Task<EtiquetaCajaDatosDto?> ObtenerDatosEtiquetaCajaPorDetalleAsync(int palletDetalleId)
        => QueryFirstAsync<EtiquetaCajaDatosDto>("Produccion.sp_Pallet_ObtenerEtiquetaCajaPorDetalle", new { PalletDetalleId = palletDetalleId });

    public Task<EtiquetaPalletEncabezadoDto?> ObtenerDatosEtiquetaPalletEncabezadoAsync(int palletId)
        => QueryFirstAsync<EtiquetaPalletEncabezadoDto>("Produccion.sp_Pallet_ObtenerEtiquetaPalletEncabezado", new { PalletId = palletId });

    public Task<IReadOnlyList<EtiquetaPalletDetalleDto>> ObtenerDatosEtiquetaPalletDetalleAsync(int palletId)
        => QueryAsync<EtiquetaPalletDetalleDto>("Produccion.sp_Pallet_ObtenerEtiquetaPalletDetalle", new { PalletId = palletId });

    public Task<EtiquetaSagarpaDatosDto?> ObtenerDatosEtiquetaSagarpaAsync(int palletId)
        => QueryFirstAsync<EtiquetaSagarpaDatosDto>("Produccion.sp_Pallet_ObtenerEtiquetaSagarpa", new { PalletId = palletId });

    public Task<EtiquetaSagarpaDatosDto?> ObtenerDatosEtiquetaSagarpaPorDetalleAsync(int palletDetalleId)
        => QueryFirstAsync<EtiquetaSagarpaDatosDto>("Produccion.sp_Pallet_ObtenerEtiquetaSagarpaPorDetalle", new { PalletDetalleId = palletDetalleId });

    public Task<int> InsertarAsync(int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal)
        => ExecuteScalarAsync<int>("Produccion.sp_Pallet_Insertar", new
        {
            LineaProduccionId = lineaProduccionId,
            EsMixto = esMixto,
            ProductoTerminadoId = productoTerminadoId,
            PesoReal = pesoReal,
        })!;

    public Task<int> CrearNeutroAsync(int corridaId, int productoTerminadoId, decimal kilogramos)
        => ExecuteScalarAsync<int>("Produccion.sp_Pallet_CrearNeutro", new
        {
            CorridaId = corridaId,
            ProductoTerminadoId = productoTerminadoId,
            Kilogramos = kilogramos,
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

    public async Task<PalletDetalleInsertadoDto> InsertarDetalleAsync(int palletId, int corridaId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
        => (await QueryFirstAsync<PalletDetalleInsertadoDto>("Produccion.sp_PalletDetalle_Insertar", new
        {
            PalletId = palletId,
            CorridaId = corridaId,
            ProductoTerminadoId = productoTerminadoId,
            Cajas = cajas,
            Kilogramos = kilogramos,
            PorcentajeMateriaSeca = porcentajeMateriaSeca,
        }))!;

    public async Task<PalletDetalleCodigosDto> ActualizarDetalleAsync(int id, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
        => (await QueryFirstAsync<PalletDetalleCodigosDto>("Produccion.sp_PalletDetalle_Actualizar", new
        {
            Id = id,
            ProductoTerminadoId = productoTerminadoId,
            Cajas = cajas,
            Kilogramos = kilogramos,
            PorcentajeMateriaSeca = porcentajeMateriaSeca,
        }))!;

    public Task ActualizarVoiceCodeDetalleAsync(int id, string? voiceCodeLow, string? voiceCodeHigh)
        => ExecuteAsync("Produccion.sp_PalletDetalle_ActualizarVoiceCode", new
        {
            Id = id,
            VoiceCodeLow = voiceCodeLow,
            VoiceCodeHigh = voiceCodeHigh,
        });

    public Task EliminarDetalleAsync(int id)
        => ExecuteAsync("Produccion.sp_PalletDetalle_Eliminar", new { Id = id });

    public Task RecalcularGs1128PalletAsync(int palletId)
        => ExecuteAsync("Produccion.sp_PalletDetalle_RecalcularGs1128Masivo", new { PalletId = palletId });

    public Task<IReadOnlyList<PalletDetalleParaVoiceCodeDto>> ObtenerParaRecalcularVoiceCodeAsync(int palletId)
        => QueryAsync<PalletDetalleParaVoiceCodeDto>("Produccion.sp_PalletDetalle_ObtenerParaRecalcularVoiceCode", new { PalletId = palletId });
}
