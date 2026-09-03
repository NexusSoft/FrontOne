using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class GastoRecepcionService
{
    private const string Modulo = "Gastos";

    private readonly IGastoRecepcionRepository _gastoRecepcionRepository;
    private readonly IGastoRecepcionAjusteRepository _gastoRecepcionAjusteRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GastoRecepcionService(
        IGastoRecepcionRepository gastoRecepcionRepository,
        IGastoRecepcionAjusteRepository gastoRecepcionAjusteRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _gastoRecepcionRepository = gastoRecepcionRepository;
        _gastoRecepcionAjusteRepository = gastoRecepcionAjusteRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<GastoRecepcionResumenDto> ObtenerResumenAsync(int gastoLoteId, byte tipoGasto)
    {
        var baseFilas = await _gastoRecepcionRepository.ObtenerBaseAsync(gastoLoteId, tipoGasto);
        var ajustes = await _gastoRecepcionAjusteRepository.ObtenerAsync(gastoLoteId, tipoGasto);

        var totalEmpresa = baseFilas.Where(b => b.CargoA == (byte)CargoAGasto.Empresa).Sum(b => b.Importe)
                          + ajustes.Where(a => a.CargoA == (byte)CargoAGasto.Empresa).Sum(a => a.Importe);
        var totalProductor = baseFilas.Where(b => b.CargoA == (byte)CargoAGasto.Productor).Sum(b => b.Importe)
                            + ajustes.Where(a => a.CargoA == (byte)CargoAGasto.Productor).Sum(a => a.Importe);

        return new GastoRecepcionResumenDto(baseFilas, ajustes, totalEmpresa, totalProductor);
    }

    public async Task ActualizarCargoAAsync(int id, byte cargoA)
    {
        await _gastoRecepcionRepository.ActualizarCargoAAsync(id, cargoA);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresNuevos = JsonSerializer.Serialize(new { GastoRecepcionId = id, CargoA = cargoA });
        await _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Modificar, Modulo, null, valoresNuevos);
    }

    // Jala el precio vigente de Acopio.ListaPrecioCorte para la Empresa de Corte de esa recepción
    // y lo deja fijo (Gastos.GastoRecepcion.PrecioUnitarioManual) hasta la próxima actualización.
    public async Task<(decimal PrecioUnitario, decimal Importe)> ActualizarPrecioCorteAsync(int gastoRecepcionId)
    {
        var resultado = await _gastoRecepcionRepository.ActualizarPrecioCorteAsync(gastoRecepcionId);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresNuevos = JsonSerializer.Serialize(new { GastoRecepcionId = gastoRecepcionId, resultado.PrecioUnitario, resultado.Importe });
        await _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Modificar, Modulo, null, valoresNuevos);

        return resultado;
    }

    // Jala el precio vigente de Acopio.ListaPrecioAcarreo (Municipio de la Huerta + tramo por
    // Cajas Entregadas) y lo deja fijo (Gastos.GastoRecepcion.PrecioUnitarioManual) hasta la
    // próxima actualización.
    public async Task<(decimal PrecioUnitario, decimal Importe)> ActualizarPrecioAcarreoAsync(int gastoRecepcionId)
    {
        var resultado = await _gastoRecepcionRepository.ActualizarPrecioAcarreoAsync(gastoRecepcionId);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresNuevos = JsonSerializer.Serialize(new { GastoRecepcionId = gastoRecepcionId, resultado.PrecioUnitario, resultado.Importe });
        await _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Modificar, Modulo, null, valoresNuevos);

        return resultado;
    }

    public Task<IReadOnlyList<RelacionGastoDto>> ObtenerParaReporteAsync(int gastoLoteId)
        => _gastoRecepcionRepository.ObtenerParaReporteAsync(gastoLoteId);
}
