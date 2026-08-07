using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class PalletService
{
    private const string Modulo = "Pallets";

    private readonly IPalletRepository _palletRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PalletService(
        IPalletRepository palletRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _palletRepository = palletRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PalletDto>> ObtenerAsync()
        => (await _palletRepository.ObtenerAsync()).Select(MapearDto).ToList();

    public async Task<PalletDto?> ObtenerPorIdAsync(int id)
        => (await _palletRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public async Task<IReadOnlyList<PalletDetalleDto>> ObtenerDetalleAsync(int palletId)
        => (await _palletRepository.ObtenerDetalleAsync(palletId)).Select(MapearDetalleDto).ToList();

    public Task<IReadOnlyList<LoteEnProcesoParaPalletDto>> ObtenerLotesEnProcesoAsync(int? lineaProduccionId = null)
        => _palletRepository.ObtenerLotesEnProcesoAsync(lineaProduccionId);

    public Task<PalletReporteDto?> ObtenerParaReporteAsync(int id)
        => _palletRepository.ObtenerParaReporteAsync(id);

    public async Task<int> CrearAsync(int lineaProduccionId, bool esMixto, decimal? pesoReal)
    {
        ValidarLineaProduccion(lineaProduccionId);
        ValidarPesoReal(pesoReal);

        var id = await _palletRepository.InsertarAsync(lineaProduccionId, esMixto, pesoReal);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await LeerSnapshotAsync(id));

        return id;
    }

    public async Task ActualizarEncabezadoAsync(int id, int lineaProduccionId, bool esMixto, decimal? pesoReal)
    {
        ValidarLineaProduccion(lineaProduccionId);
        ValidarPesoReal(pesoReal);

        var anterior = await LeerSnapshotAsync(id);

        await _palletRepository.ActualizarEncabezadoAsync(id, lineaProduccionId, esMixto, pesoReal);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    // Bloquear es una modificación definitiva del encabezado (pasa a Estatus Empacado y congela
    // todo) — se audita como Modificar, no como una acción propia: la auditoría del proyecto solo
    // maneja Crear/Modificar/Eliminar y el snapshot antes/después ya deja ver qué cambió.
    public async Task BloquearAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _palletRepository.BloquearAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _palletRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    public async Task<int> AgregarLineaAsync(int palletId, int corridaId, int productoTerminadoId, int cajas, decimal porcentajeMateriaSeca)
    {
        ValidarLinea(corridaId, productoTerminadoId, cajas);

        var anterior = await LeerSnapshotAsync(palletId);

        var id = await _palletRepository.InsertarDetalleAsync(palletId, corridaId, productoTerminadoId, cajas, porcentajeMateriaSeca);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(palletId));

        return id;
    }

    public async Task ActualizarLineaAsync(int palletId, int id, int productoTerminadoId, int cajas, decimal porcentajeMateriaSeca)
    {
        if (productoTerminadoId <= 0)
        {
            throw new ValidationException("Selecciona un producto terminado");
        }

        if (cajas <= 0)
        {
            throw new ValidationException("Las cajas deben ser mayores a cero");
        }

        var anterior = await LeerSnapshotAsync(palletId);

        await _palletRepository.ActualizarDetalleAsync(id, productoTerminadoId, cajas, porcentajeMateriaSeca);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(palletId));
    }

    public async Task EliminarLineaAsync(int palletId, int id)
    {
        var anterior = await LeerSnapshotAsync(palletId);

        await _palletRepository.EliminarDetalleAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(palletId));
    }

    // El snapshot de auditoría de un Pallet es siempre encabezado + detalle completo: un cambio
    // en una línea altera Estatus/% Materia Seca/kilos del encabezado, así que registrar solo una
    // de las dos partes dejaría el registro incompleto.
    private async Task<SnapshotPallet?> LeerSnapshotAsync(int id)
    {
        var encabezado = (await _palletRepository.ObtenerAsync(id)).FirstOrDefault();
        if (encabezado is null)
        {
            return null;
        }

        var detalle = await _palletRepository.ObtenerDetalleAsync(id);
        return new SnapshotPallet(encabezado, detalle);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, SnapshotPallet? anterior, SnapshotPallet? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarLineaProduccion(int lineaProduccionId)
    {
        if (lineaProduccionId <= 0)
        {
            throw new ValidationException("Selecciona una línea de producción");
        }
    }

    private static void ValidarPesoReal(decimal? pesoReal)
    {
        if (pesoReal is < 0)
        {
            throw new ValidationException("El peso real no puede ser negativo");
        }
    }

    private static void ValidarLinea(int corridaId, int productoTerminadoId, int cajas)
    {
        if (corridaId <= 0)
        {
            throw new ValidationException("Selecciona un lote en proceso");
        }

        if (productoTerminadoId <= 0)
        {
            throw new ValidationException("Selecciona un producto terminado");
        }

        if (cajas <= 0)
        {
            throw new ValidationException("Las cajas deben ser mayores a cero");
        }
    }

    private static PalletDto MapearDto(Pallet p) => new(
        p.Id,
        p.Folio,
        p.FechaCreacion,
        p.HoraCreacion,
        p.Estatus,
        p.LineaProduccionId,
        p.LineaProduccionNombre,
        p.EsMixto,
        p.PorcentajeMateriaSeca,
        p.PesoReal,
        p.Bloqueado,
        p.FechaBloqueo,
        p.NoReempaque,
        p.PrimeraCorrida,
        p.TotalCajas,
        p.TotalKilogramos,
        p.ProductoDescripcion,
        p.FechaCreacionRegistro);

    private static PalletDetalleDto MapearDetalleDto(PalletDetalle d) => new(
        d.Id,
        d.PalletId,
        d.CorridaId,
        d.LoteId,
        d.LoteFolio,
        d.ProductoTerminadoId,
        d.ProductoCodigoSap,
        d.ProductoDescripcion,
        d.Cajas,
        d.Kilogramos,
        d.PorcentajeMateriaSeca,
        d.CajasPorPallet,
        d.LoteEnProceso);

    private sealed record SnapshotPallet(Pallet Encabezado, IReadOnlyList<PalletDetalle> Detalle);
}
