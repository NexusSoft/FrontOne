using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class MunicipioService
{
    private const string Modulo = "Catalogos";

    private readonly IMunicipioRepository _municipioRepository;
    private readonly IEstadoRepository _estadoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public MunicipioService(
        IMunicipioRepository municipioRepository,
        IEstadoRepository estadoRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _municipioRepository = municipioRepository;
        _estadoRepository = estadoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<MunicipioDto>> ObtenerAsync(int? estadoId = null)
    {
        var municipios = await _municipioRepository.ObtenerAsync(estadoId);
        return municipios.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(int estadoId, string nombre)
    {
        await ValidarEstadoExisteAsync(estadoId);
        ValidarCampos(nombre);

        var id = await _municipioRepository.InsertarAsync(new Municipio
        {
            EstadoId = estadoId,
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _municipioRepository.ObtenerAsync(id: id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, int estadoId, string nombre, bool activo)
    {
        await ValidarEstadoExisteAsync(estadoId);
        ValidarCampos(nombre);

        var anterior = (await _municipioRepository.ObtenerAsync(id: id)).FirstOrDefault();

        await _municipioRepository.ActualizarAsync(new Municipio
        {
            Id = id,
            EstadoId = estadoId,
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _municipioRepository.ObtenerAsync(id: id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _municipioRepository.ObtenerAsync(id: id)).FirstOrDefault();

        await _municipioRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Municipio? anterior, Municipio? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private async Task ValidarEstadoExisteAsync(int estadoId)
    {
        var estados = await _estadoRepository.ObtenerAsync(id: estadoId);
        if (estados.Count == 0)
        {
            throw new ValidationException("El estado seleccionado no existe");
        }
    }

    private static void ValidarCampos(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del municipio es obligatorio");
        }
    }

    private static MunicipioDto MapearDto(Municipio municipio) => new(municipio.Id, municipio.Nombre, municipio.EstadoId, municipio.Activo);
}
