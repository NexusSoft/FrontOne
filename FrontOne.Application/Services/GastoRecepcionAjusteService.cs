using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class GastoRecepcionAjusteService
{
    private const string Modulo = "Gastos";

    private readonly IGastoRecepcionAjusteRepository _gastoRecepcionAjusteRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GastoRecepcionAjusteService(
        IGastoRecepcionAjusteRepository gastoRecepcionAjusteRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _gastoRecepcionAjusteRepository = gastoRecepcionAjusteRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public Task<IReadOnlyList<GastoRecepcionAjusteDto>> ObtenerAsync(int gastoLoteId, byte tipoGasto)
        => _gastoRecepcionAjusteRepository.ObtenerAsync(gastoLoteId, tipoGasto);

    public async Task<int> CrearAsync(int gastoLoteId, int loteRecepcionId, int tipoAjusteId, decimal monto, byte cargoA)
    {
        ValidarCampos(tipoAjusteId, monto, cargoA);

        var ajuste = new GastoRecepcionAjuste
        {
            GastoLoteId = gastoLoteId,
            LoteRecepcionId = loteRecepcionId,
            TipoAjusteId = tipoAjusteId,
            Monto = monto,
            CargoA = cargoA,
        };

        var id = await _gastoRecepcionAjusteRepository.InsertarAsync(ajuste);
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, ajuste);

        return id;
    }

    public async Task ActualizarAsync(int id, int tipoAjusteId, decimal monto, byte cargoA)
    {
        ValidarCampos(tipoAjusteId, monto, cargoA);

        var ajuste = new GastoRecepcionAjuste { Id = id, TipoAjusteId = tipoAjusteId, Monto = monto, CargoA = cargoA };
        await _gastoRecepcionAjusteRepository.ActualizarAsync(ajuste);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, null, ajuste);
    }

    public async Task EliminarAsync(int id)
    {
        await _gastoRecepcionAjusteRepository.EliminarAsync(id);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        await _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Eliminar, Modulo, JsonSerializer.Serialize(new { Id = id }), null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, GastoRecepcionAjuste? anterior, GastoRecepcionAjuste? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(int tipoAjusteId, decimal monto, byte cargoA)
    {
        if (tipoAjusteId <= 0)
        {
            throw new ValidationException("Selecciona el tipo de ajuste");
        }

        if (monto <= 0)
        {
            throw new ValidationException("El monto del ajuste debe ser mayor a cero");
        }

        if (cargoA is not (1 or 2))
        {
            throw new ValidationException("Selecciona si el ajuste es con cargo a Empresa o a Productor");
        }
    }
}
