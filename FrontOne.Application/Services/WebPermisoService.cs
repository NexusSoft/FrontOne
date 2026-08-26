using System.Text.Json;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class WebPermisoService
{
    private const string Modulo = "Seguridad";

    private readonly IWebPermisoRepository _webPermisoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public WebPermisoService(
        IWebPermisoRepository webPermisoRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _webPermisoRepository = webPermisoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<WebPermisoPantallaDto>> ObtenerMatrizAsync(int rolId)
    {
        var otorgados = await _webPermisoRepository.ObtenerPorRolAsync(rolId);
        var porCodigo = otorgados.ToDictionary(o => o.PantallaCodigo, StringComparer.OrdinalIgnoreCase);

        return PantallasWebDisponibles.Todas
            .Select(p =>
            {
                porCodigo.TryGetValue(p.Codigo, out var fila);
                return new WebPermisoPantallaDto(
                    p.Codigo,
                    p.Modulo,
                    fila?.Consultar ?? false,
                    fila?.Crear ?? false,
                    fila?.Modificar ?? false,
                    fila?.Eliminar ?? false);
            })
            .ToList();
    }

    public async Task GuardarAsync(int rolId, IReadOnlyList<WebPermisoPantallaDto> filas)
    {
        var anteriores = await _webPermisoRepository.ObtenerPorRolAsync(rolId);

        var nuevos = filas
            .Select(f => new WebPermiso
            {
                RolId = rolId,
                PantallaCodigo = f.PantallaCodigo,
                Consultar = f.Consultar,
                Crear = f.Crear,
                Modificar = f.Modificar,
                Eliminar = f.Eliminar,
            })
            .ToList();

        await _webPermisoRepository.SincronizarAsync(rolId, nuevos);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        await _auditService.RegistrarAsync(
            usuario,
            TipoAccionAuditoria.Modificar,
            Modulo,
            JsonSerializer.Serialize(anteriores),
            JsonSerializer.Serialize(nuevos));
    }
}
