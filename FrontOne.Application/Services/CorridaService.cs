using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class CorridaService
{
    private const string Modulo = "Corridas";

    private readonly ICorridaRepository _corridaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CorridaService(
        ICorridaRepository corridaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _corridaRepository = corridaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<CorridaDto>> ObtenerAsync()
    {
        var corridas = await _corridaRepository.ObtenerAsync();
        return corridas.Select(MapearDto).ToList();
    }

    public async Task<CorridaDto?> ObtenerPorIdAsync(int id)
        => (await _corridaRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public Task<IReadOnlyList<LoteDisponibleParaCorridaDto>> ObtenerLotesDisponiblesAsync()
        => _corridaRepository.ObtenerLotesDisponiblesAsync();

    public async Task<int> IniciarAsync(int loteId)
    {
        var id = await _corridaRepository.IniciarAsync(loteId);

        var creada = (await _corridaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creada);

        return id;
    }

    public async Task FinalizarAsync(int id)
    {
        var anterior = (await _corridaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _corridaRepository.FinalizarAsync(id);

        var nueva = (await _corridaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nueva);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _corridaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _corridaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Corrida? anterior, Corrida? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static CorridaDto MapearDto(Corrida c) => new(
        c.Id,
        c.LoteId,
        c.LoteFolio,
        c.CodigoTrazabilidad,
        c.Kilogramos,
        c.HuertaNombre,
        c.RegistroSagarpa,
        c.ProductorNombre,
        c.Beneficiario,
        c.FechaHoraInicio,
        c.FechaHoraFin,
        c.Estatus,
        c.KilosAProcesar,
        c.KilosProcesados,
        c.PesoFactor,
        c.FechaCreacion);
}
