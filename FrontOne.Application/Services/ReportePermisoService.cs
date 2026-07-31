using System.Text.Json;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class ReportePermisoService
{
    private const string Modulo = "Seguridad";

    private readonly IReportePermisoRepository _reportePermisoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ReportePermisoService(
        IReportePermisoRepository reportePermisoRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _reportePermisoRepository = reportePermisoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<ReportePermisoFilaDto>> ObtenerMatrizAsync(int rolId)
    {
        var otorgados = await _reportePermisoRepository.ObtenerPorRolAsync(rolId);
        var porCodigo = otorgados.ToDictionary(o => o.ReporteCodigo, StringComparer.OrdinalIgnoreCase);

        return ReportesDisponibles.Todos
            .Select(r =>
            {
                porCodigo.TryGetValue(r.Codigo, out var fila);
                return new ReportePermisoFilaDto(
                    r.Codigo,
                    r.Nombre,
                    fila?.VistaPrevia ?? false,
                    fila?.Impresion ?? false,
                    fila?.Exportacion ?? false,
                    fila?.Diseno ?? false);
            })
            .ToList();
    }

    public async Task GuardarAsync(int rolId, IReadOnlyList<ReportePermisoFilaDto> filas)
    {
        var anteriores = await _reportePermisoRepository.ObtenerPorRolAsync(rolId);

        var nuevos = filas
            .Select(f => new ReportePermiso
            {
                RolId = rolId,
                ReporteCodigo = f.Codigo,
                VistaPrevia = f.VistaPrevia,
                Impresion = f.Impresion,
                Exportacion = f.Exportacion,
                Diseno = f.Diseno,
            })
            .ToList();

        await _reportePermisoRepository.SincronizarAsync(rolId, nuevos);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        await _auditService.RegistrarAsync(
            usuario,
            TipoAccionAuditoria.Modificar,
            Modulo,
            JsonSerializer.Serialize(anteriores),
            JsonSerializer.Serialize(nuevos));
    }
}
