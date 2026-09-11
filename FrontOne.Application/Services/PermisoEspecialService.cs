using System.Text.Json;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class PermisoEspecialService
{
    private const string Modulo = "Seguridad";

    private readonly IPermisoEspecialRepository _permisoEspecialRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PermisoEspecialService(
        IPermisoEspecialRepository permisoEspecialRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _permisoEspecialRepository = permisoEspecialRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PermisoEspecialFilaDto>> ObtenerMatrizAsync(int rolId)
    {
        var otorgados = await _permisoEspecialRepository.ObtenerPorRolAsync(rolId);
        var porCodigo = otorgados.ToDictionary(o => o.Codigo, StringComparer.OrdinalIgnoreCase);

        return PermisosEspecialesDisponibles.Todas
            .Select(p =>
            {
                porCodigo.TryGetValue(p.Codigo, out var fila);
                return new PermisoEspecialFilaDto(
                    p.Codigo,
                    p.Descripcion,
                    fila?.Habilitado ?? false);
            })
            .ToList();
    }

    public async Task GuardarAsync(int rolId, IReadOnlyList<PermisoEspecialFilaDto> filas)
    {
        var anteriores = await _permisoEspecialRepository.ObtenerPorRolAsync(rolId);

        var nuevos = filas
            .Select(f => new PermisoEspecial
            {
                RolId = rolId,
                Codigo = f.Codigo,
                Habilitado = f.Habilitado,
            })
            .ToList();

        await _permisoEspecialRepository.SincronizarAsync(rolId, nuevos);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        await _auditService.RegistrarAsync(
            usuario,
            TipoAccionAuditoria.Modificar,
            Modulo,
            JsonSerializer.Serialize(anteriores),
            JsonSerializer.Serialize(nuevos));
    }

    public async Task<IReadOnlyList<PermisoDto>> ObtenerPermisosWebAsync(int usuarioId)
    {
        var codigos = await _permisoEspecialRepository.ObtenerCodigosHabilitadosAsync(usuarioId);
        var porCodigo = PermisosEspecialesDisponibles.Todas.ToDictionary(d => d.Codigo, StringComparer.OrdinalIgnoreCase);

        return codigos
            .Where(porCodigo.ContainsKey)
            .Select(codigo =>
            {
                var def = porCodigo[codigo];
                return new PermisoDto(def.Modulo, def.Pantalla, def.Accion);
            })
            .ToList();
    }
}
