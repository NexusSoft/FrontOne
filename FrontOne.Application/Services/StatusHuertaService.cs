using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class StatusHuertaService
{
    private const string Modulo = "Catalogos";

    private readonly IStatusHuertaRepository _statusHuertaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public StatusHuertaService(IStatusHuertaRepository statusHuertaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _statusHuertaRepository = statusHuertaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<StatusHuertaDto>> ObtenerAsync()
    {
        var estatus = await _statusHuertaRepository.ObtenerAsync();
        return estatus.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _statusHuertaRepository.InsertarAsync(new StatusHuerta { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _statusHuertaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _statusHuertaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _statusHuertaRepository.ActualizarAsync(new StatusHuerta { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _statusHuertaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _statusHuertaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _statusHuertaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, StatusHuerta? anterior, StatusHuerta? nuevo)
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
            throw new ValidationException("El nombre del status es obligatorio");
        }
    }

    private static StatusHuertaDto MapearDto(StatusHuerta statusHuerta) => new(statusHuerta.Id, statusHuerta.Nombre, statusHuerta.Activo);
}
