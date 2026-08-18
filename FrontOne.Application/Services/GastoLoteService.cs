using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class GastoLoteService
{
    private const string Modulo = "Gastos";

    private readonly IGastoLoteRepository _gastoLoteRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GastoLoteService(IGastoLoteRepository gastoLoteRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _gastoLoteRepository = gastoLoteRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public Task<IReadOnlyList<GastoLoteListadoDto>> ObtenerLotesCosteablesAsync()
        => _gastoLoteRepository.ObtenerLotesCosteablesAsync();

    public async Task<GastoLoteEncabezadoDto?> ObtenerEncabezadoAsync(int loteId)
    {
        var e = await _gastoLoteRepository.ObtenerEncabezadoAsync(loteId);
        return e is null ? null : MapearEncabezadoDto(e);
    }

    public Task<int> ObtenerOCrearAsync(int loteId)
        => _gastoLoteRepository.ObtenerOCrearAsync(loteId);

    public Task<GastoLoteReporteDto?> ObtenerParaReporteAsync(int gastoLoteId)
        => _gastoLoteRepository.ObtenerParaReporteAsync(gastoLoteId);

    public async Task ActualizarVigenciaEstimadoAsync(int gastoLoteId, DateTime? fecha, int? productorId, byte? numero)
    {
        var anterior = await _gastoLoteRepository.ObtenerPorIdAsync(gastoLoteId);

        await _gastoLoteRepository.ActualizarVigenciaEstimadoAsync(new GastoLote
        {
            Id = gastoLoteId,
            CostoEstimadoListaPrecioFecha = fecha,
            CostoEstimadoListaPrecioProductorId = productorId,
            CostoEstimadoListaPrecioNumero = numero,
        });

        var nuevo = await _gastoLoteRepository.ObtenerPorIdAsync(gastoLoteId);
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, GastoLote? anterior, GastoLote? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static GastoLoteEncabezadoDto MapearEncabezadoDto(GastoLoteEncabezado e) => new(
        e.LoteId,
        e.LoteFolio,
        e.CodigoTrazabilidad,
        e.FechaCorrida,
        e.Kilogramos,
        e.HuertaNombre,
        e.RegistroSagarpa,
        e.VariedadId,
        e.VariedadNombre,
        e.TipoCorteNombre,
        e.TipoPagoNombre,
        e.NecesitaListaPrecios,
        e.Precio,
        e.ListaPrecioFecha,
        e.ListaPrecioProductorNombre,
        e.ListaPrecioNumero,
        e.GastoLoteId,
        e.CostoEstimadoListaPrecioFecha,
        e.CostoEstimadoListaPrecioProductorId,
        e.CostoEstimadoListaPrecioProductorNombre,
        e.CostoEstimadoListaPrecioNumero);
}
