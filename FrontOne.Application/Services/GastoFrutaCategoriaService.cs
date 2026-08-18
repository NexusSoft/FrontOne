using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class GastoFrutaCategoriaService
{
    private const string Modulo = "Gastos";

    private readonly IGastoFrutaCategoriaRepository _gastoFrutaCategoriaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GastoFrutaCategoriaService(
        IGastoFrutaCategoriaRepository gastoFrutaCategoriaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _gastoFrutaCategoriaRepository = gastoFrutaCategoriaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public Task<IReadOnlyList<GastoFrutaCategoriaLineaDto>> ObtenerAsync(int gastoLoteId)
        => _gastoFrutaCategoriaRepository.ObtenerAsync(gastoLoteId);

    public async Task<IReadOnlyList<ResumenMercadoDto>> ObtenerResumenMercadoAsync(int gastoLoteId)
    {
        var resumen = await _gastoFrutaCategoriaRepository.ObtenerResumenMercadoAsync(gastoLoteId);
        return resumen.OrderBy(r => OrdenMercado(r.Mercado)).ToList();
    }

    private static int OrdenMercado(string mercado) => mercado switch
    {
        "Exportación" => 1,
        "Nacional" => 2,
        "Merma" => 3,
        "Total" => 4,
        _ => 5,
    };

    public async Task GuardarCostoRealAsync(int gastoLoteId, string materiaPrimaItemCode, decimal? costoRealUnitario)
    {
        await _gastoFrutaCategoriaRepository.GuardarCostoRealAsync(gastoLoteId, materiaPrimaItemCode, costoRealUnitario);
        await RegistrarAuditoriaAsync(gastoLoteId, materiaPrimaItemCode, "CostoRealUnitario", costoRealUnitario);
    }

    public async Task GuardarCostoEstimadoAsync(int gastoLoteId, string materiaPrimaItemCode, decimal? costoEstimadoUnitario)
    {
        await _gastoFrutaCategoriaRepository.GuardarCostoEstimadoAsync(gastoLoteId, materiaPrimaItemCode, costoEstimadoUnitario);
        await RegistrarAuditoriaAsync(gastoLoteId, materiaPrimaItemCode, "CostoEstimadoUnitario", costoEstimadoUnitario);
    }

    private Task RegistrarAuditoriaAsync(int gastoLoteId, string materiaPrimaItemCode, string campo, decimal? valor)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresNuevos = JsonSerializer.Serialize(new { GastoLoteId = gastoLoteId, MateriaPrimaItemCode = materiaPrimaItemCode, Campo = campo, Valor = valor });

        return _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Modificar, Modulo, null, valoresNuevos);
    }
}
