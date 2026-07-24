using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class PermisoService
{
    private const string Modulo = "Seguridad";

    private readonly IPermisoRepository _permisoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PermisoService(IPermisoRepository permisoRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _permisoRepository = permisoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PermisoPantallaDto>> ObtenerMatrizAsync(int rolId)
    {
        var estructura = await _permisoRepository.ObtenerEstructuraAsync();
        var otorgados = (await _permisoRepository.ObtenerPorRolAsync(rolId)).ToHashSet();

        return estructura
            .GroupBy(e => (e.PantallaId, e.ModuloNombre, e.PantallaNombre))
            .Select(g =>
            {
                var accionIdPorNombre = g.ToDictionary(x => x.AccionNombre, x => x.AccionId);

                bool Otorgado(string accion)
                    => accionIdPorNombre.TryGetValue(accion, out var accionId) && otorgados.Contains((g.Key.PantallaId, accionId));

                return new PermisoPantallaDto(
                    g.Key.PantallaId,
                    g.Key.ModuloNombre,
                    g.Key.PantallaNombre,
                    Otorgado("Consultar"),
                    Otorgado("Crear"),
                    Otorgado("Modificar"),
                    Otorgado("Eliminar"));
            })
            .OrderBy(f => f.ModuloNombre)
            .ThenBy(f => f.PantallaNombre)
            .ToList();
    }

    public async Task GuardarAsync(int rolId, IReadOnlyList<PermisoPantallaDto> filas)
    {
        var estructura = await _permisoRepository.ObtenerEstructuraAsync();
        var accionIdPorNombre = estructura
            .Select(e => (e.AccionNombre, e.AccionId))
            .Distinct()
            .ToDictionary(x => x.AccionNombre, x => x.AccionId);

        var anteriores = await _permisoRepository.ObtenerPorRolAsync(rolId);

        var otorgados = new List<(int PantallaId, int AccionId)>();
        foreach (var fila in filas)
        {
            AgregarSiOtorgado(otorgados, fila.PantallaId, fila.Consultar, accionIdPorNombre, "Consultar");
            AgregarSiOtorgado(otorgados, fila.PantallaId, fila.Crear, accionIdPorNombre, "Crear");
            AgregarSiOtorgado(otorgados, fila.PantallaId, fila.Modificar, accionIdPorNombre, "Modificar");
            AgregarSiOtorgado(otorgados, fila.PantallaId, fila.Eliminar, accionIdPorNombre, "Eliminar");
        }

        await _permisoRepository.SincronizarAsync(rolId, otorgados);

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        await _auditService.RegistrarAsync(
            usuario,
            TipoAccionAuditoria.Modificar,
            Modulo,
            JsonSerializer.Serialize(anteriores),
            JsonSerializer.Serialize(otorgados));
    }

    private static void AgregarSiOtorgado(
        List<(int PantallaId, int AccionId)> otorgados,
        int pantallaId,
        bool marcado,
        Dictionary<string, int> accionIdPorNombre,
        string accion)
    {
        if (marcado && accionIdPorNombre.TryGetValue(accion, out var accionId))
        {
            otorgados.Add((pantallaId, accionId));
        }
    }
}
