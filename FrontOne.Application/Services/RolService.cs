using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class RolService
{
    private const string Modulo = "Seguridad";

    private readonly IRolRepository _rolRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public RolService(IRolRepository rolRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _rolRepository = rolRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<RolDto>> ObtenerAsync()
    {
        var roles = await _rolRepository.ObtenerAsync();
        return roles.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre, string? descripcion)
    {
        ValidarCampos(nombre);

        var id = await _rolRepository.InsertarAsync(new Rol
        {
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            Activo = true,
        });

        var creado = (await _rolRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, string? descripcion, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _rolRepository.ObtenerAsync(id)).FirstOrDefault();

        await _rolRepository.ActualizarAsync(new Rol
        {
            Id = id,
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            Activo = activo,
        });

        var nuevo = (await _rolRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _rolRepository.ObtenerAsync(id)).FirstOrDefault();

        await _rolRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Rol? anterior, Rol? nuevo)
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
            throw new ValidationException("El nombre del rol es obligatorio");
        }
    }

    private static RolDto MapearDto(Rol rol) => new(rol.Id, rol.Nombre, rol.Descripcion, rol.Activo);
}
