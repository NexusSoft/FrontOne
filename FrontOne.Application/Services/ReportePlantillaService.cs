using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ReportePlantillaService
{
    private const string Modulo = "Seguridad";

    private readonly IReportePlantillaRepository _reportePlantillaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ReportePlantillaService(
        IReportePlantillaRepository reportePlantillaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _reportePlantillaRepository = reportePlantillaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<ReportePlantillaDto?> ObtenerPorCodigoAsync(string codigo)
    {
        var plantilla = await _reportePlantillaRepository.ObtenerPorCodigoAsync(codigo);
        return plantilla is null
            ? null
            : new ReportePlantillaDto(plantilla.Id, plantilla.Codigo, plantilla.Nombre, plantilla.DefinicionXml, plantilla.FechaModificacion);
    }

    // Guarda el layout editado en el Diseñador de Reportes — nunca se pierde el reporte si algo
    // falla aquí, porque el default embebido en el proyecto sigue funcionando sin plantilla.
    public async Task GuardarAsync(string codigo, string nombre, string definicionXml)
    {
        var existente = await _reportePlantillaRepository.ObtenerPorCodigoAsync(codigo);

        await _reportePlantillaRepository.GuardarAsync(codigo, nombre, definicionXml);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var accion = existente is null ? TipoAccionAuditoria.Crear : TipoAccionAuditoria.Modificar;
        var valoresAnteriores = existente is null ? null : JsonSerializer.Serialize(new { existente.Codigo, existente.Nombre });
        var valoresNuevos = JsonSerializer.Serialize(new { Codigo = codigo, Nombre = nombre });

        await _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    // "Restablecer" — borra el diseño personalizado, el reporte vuelve al layout default.
    public async Task EliminarAsync(string codigo)
    {
        var existente = await _reportePlantillaRepository.ObtenerPorCodigoAsync(codigo);
        if (existente is null)
        {
            return;
        }

        await _reportePlantillaRepository.EliminarAsync(codigo);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = JsonSerializer.Serialize(new { existente.Codigo, existente.Nombre });
        await _auditService.RegistrarAsync(usuario, TipoAccionAuditoria.Eliminar, Modulo, valoresAnteriores, null);
    }
}
