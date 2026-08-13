using System.Text.Json;
using FrontOne.Domain.Constants;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class MovilPermisoService
{
    private const string Modulo = "Seguridad";

    private readonly IMovilPermisoRepository _movilPermisoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public MovilPermisoService(
        IMovilPermisoRepository movilPermisoRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _movilPermisoRepository = movilPermisoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<MovilPermisoPantallaDto>> ObtenerMatrizAsync(int rolId)
    {
        var otorgados = await _movilPermisoRepository.ObtenerPorRolAsync(rolId);
        var porCodigo = otorgados.ToDictionary(o => o.PantallaCodigo, StringComparer.OrdinalIgnoreCase);

        return PantallasMovilDisponibles.Todas
            .Select(p =>
            {
                porCodigo.TryGetValue(p.Codigo, out var fila);
                return new MovilPermisoPantallaDto(
                    p.Codigo,
                    p.Modulo,
                    fila?.Consultar ?? false,
                    fila?.Crear ?? false,
                    fila?.Modificar ?? false,
                    fila?.Eliminar ?? false);
            })
            .ToList();
    }

    public async Task GuardarAsync(int rolId, IReadOnlyList<MovilPermisoPantallaDto> filas)
    {
        var anteriores = await _movilPermisoRepository.ObtenerPorRolAsync(rolId);

        var nuevos = filas
            .Select(f => new MovilPermiso
            {
                RolId = rolId,
                PantallaCodigo = f.PantallaCodigo,
                Consultar = f.Consultar,
                Crear = f.Crear,
                Modificar = f.Modificar,
                Eliminar = f.Eliminar,
            })
            .ToList();

        await _movilPermisoRepository.SincronizarAsync(rolId, nuevos);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        await _auditService.RegistrarAsync(
            usuario,
            TipoAccionAuditoria.Modificar,
            Modulo,
            JsonSerializer.Serialize(anteriores),
            JsonSerializer.Serialize(nuevos));
    }
}
