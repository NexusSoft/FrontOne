using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class CalibreApeamService
{
    private const string Modulo = "Catalogos";

    private readonly ICalibreApeamRepository _calibreApeamRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CalibreApeamService(ICalibreApeamRepository calibreApeamRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _calibreApeamRepository = calibreApeamRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<CalibreApeamDto>> ObtenerAsync()
    {
        var calibres = await _calibreApeamRepository.ObtenerAsync();
        return calibres.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _calibreApeamRepository.InsertarAsync(new CalibreApeam
        {
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _calibreApeamRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _calibreApeamRepository.ObtenerAsync(id)).FirstOrDefault();

        await _calibreApeamRepository.ActualizarAsync(new CalibreApeam
        {
            Id = id,
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _calibreApeamRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _calibreApeamRepository.ObtenerAsync(id)).FirstOrDefault();

        await _calibreApeamRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, CalibreApeam? anterior, CalibreApeam? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del calibre APEAM es obligatorio");
        }
    }

    private static CalibreApeamDto MapearDto(CalibreApeam calibreApeam) => new(calibreApeam.Id, calibreApeam.Nombre, calibreApeam.Activo);
}
