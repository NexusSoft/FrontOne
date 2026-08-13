using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class SupervisorHuertaService
{
    private const string Modulo = "Acopio";

    private readonly ISupervisorHuertaRepository _supervisorHuertaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public SupervisorHuertaService(ISupervisorHuertaRepository supervisorHuertaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _supervisorHuertaRepository = supervisorHuertaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<SupervisorHuertaDto>> ObtenerAsync()
    {
        var supervisores = await _supervisorHuertaRepository.ObtenerAsync();
        return supervisores.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _supervisorHuertaRepository.InsertarAsync(new SupervisorHuerta { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _supervisorHuertaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _supervisorHuertaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _supervisorHuertaRepository.ActualizarAsync(new SupervisorHuerta { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _supervisorHuertaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _supervisorHuertaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _supervisorHuertaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, SupervisorHuerta? anterior, SupervisorHuerta? nuevo)
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
            throw new ValidationException("El nombre del supervisor de huerta es obligatorio");
        }
    }

    private static SupervisorHuertaDto MapearDto(SupervisorHuerta supervisorHuerta) => new(supervisorHuerta.Id, supervisorHuerta.Nombre, supervisorHuerta.Activo);
}
