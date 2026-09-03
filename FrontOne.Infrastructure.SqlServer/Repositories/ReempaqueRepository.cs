using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SqlServer.Factories;
using Microsoft.Extensions.Logging;

namespace FrontOne.Infrastructure.SqlServer.Repositories;

public class ReempaqueRepository : SqlRepositoryBase, IReempaqueRepository
{
    public ReempaqueRepository(IConnectionFactory connectionFactory, ILogger<ReempaqueRepository> logger)
        : base(connectionFactory, logger)
    {
    }

    public Task<IReadOnlyList<Reempaque>> ObtenerAsync(int? id = null)
        => QueryAsync<Reempaque>("Produccion.sp_Reempaque_Obtener", new { Id = id });

    public Task<Reempaque?> ObtenerPorFolioAsync(string folio)
        => QueryFirstAsync<Reempaque>("Produccion.sp_Reempaque_Obtener", new { Id = (int?)null, Folio = folio });

    public Task EliminarAsync(int id)
        => ExecuteAsync("Produccion.sp_Reempaque_Eliminar", new { Id = id });

    public Task<IReadOnlyList<ReempaquePalletDisponibleDto>> ObtenerPalletsOrigenDisponiblesAsync(string? folio = null)
        => QueryAsync<ReempaquePalletDisponibleDto>("Produccion.sp_Reempaque_ObtenerPalletsOrigenDisponibles", new { Folio = folio });

    public Task<IReadOnlyList<ReempaquePalletDisponibleDto>> ObtenerPalletsDestinoDisponiblesAsync(int reempaqueId, string? folio = null)
        => QueryAsync<ReempaquePalletDisponibleDto>("Produccion.sp_Reempaque_ObtenerPalletsDestinoDisponibles", new { ReempaqueId = reempaqueId, Folio = folio });

    public Task<IReadOnlyList<ReempaqueDetalle>> ObtenerDetalleEntradaAsync(int reempaqueId)
        => QueryAsync<ReempaqueDetalle>("Produccion.sp_Reempaque_ObtenerDetalleEntrada", new { ReempaqueId = reempaqueId });

    public Task AgregarPalletOrigenAsync(int reempaqueId, int palletId)
        => ExecuteAsync("Produccion.sp_Reempaque_AgregarPalletOrigen", new { ReempaqueId = reempaqueId, PalletId = palletId });

    public Task QuitarPalletOrigenAsync(int reempaqueId, int palletId)
        => ExecuteAsync("Produccion.sp_Reempaque_QuitarPalletOrigen", new { ReempaqueId = reempaqueId, PalletId = palletId });

    public Task<IReadOnlyList<ReempaqueSalidaFilaDto>> ObtenerDetalleSalidaAsync(int reempaqueId)
        => QueryAsync<ReempaqueSalidaFilaDto>("Produccion.sp_Reempaque_ObtenerDetalleSalida", new { ReempaqueId = reempaqueId });

    public Task<int> InsertarAsync(string motivo)
        => ExecuteScalarAsync<int>("Produccion.sp_Reempaque_Insertar", new { Motivo = motivo })!;

    public Task<PalletDetalleInsertadoDto> InsertarLineaEnPalletAsync(int palletId, int reempaqueDetalleId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
        => QueryFirstAsync<PalletDetalleInsertadoDto>("Produccion.sp_PalletDetalle_InsertarDesdeReempaque", new
        {
            PalletId = palletId,
            ReempaqueDetalleId = reempaqueDetalleId,
            ProductoTerminadoId = productoTerminadoId,
            Cajas = cajas,
            Kilogramos = kilogramos,
            PorcentajeMateriaSeca = porcentajeMateriaSeca,
        })!;

    public Task<int> CrearNeutroAsync(int reempaqueId, int reempaqueDetalleId, int productoTerminadoId, decimal kilogramos)
        => ExecuteScalarAsync<int>("Produccion.sp_Reempaque_CrearNeutro", new
        {
            ReempaqueId = reempaqueId,
            ReempaqueDetalleId = reempaqueDetalleId,
            ProductoTerminadoId = productoTerminadoId,
            Kilogramos = kilogramos,
        })!;

    public Task CerrarAsync(int id)
        => ExecuteAsync("Produccion.sp_Reempaque_Cerrar", new { Id = id });
}
