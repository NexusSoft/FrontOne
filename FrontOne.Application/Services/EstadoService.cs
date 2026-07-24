using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class EstadoService
{
    private const string Modulo = "Catalogos";

    private readonly IEstadoRepository _estadoRepository;
    private readonly IPaisRepository _paisRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public EstadoService(
        IEstadoRepository estadoRepository,
        IPaisRepository paisRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _estadoRepository = estadoRepository;
        _paisRepository = paisRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<EstadoDto>> ObtenerAsync(int? paisId = null)
    {
        var estados = await _estadoRepository.ObtenerAsync(paisId);
        return estados.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(int paisId, string clave, string nombre)
    {
        await ValidarPaisExisteAsync(paisId);
        ValidarCampos(clave, nombre);

        var id = await _estadoRepository.InsertarAsync(new Estado
        {
            PaisId = paisId,
            Clave = clave.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _estadoRepository.ObtenerAsync(id: id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, int paisId, string clave, string nombre, bool activo)
    {
        await ValidarPaisExisteAsync(paisId);
        ValidarCampos(clave, nombre);

        var anterior = (await _estadoRepository.ObtenerAsync(id: id)).FirstOrDefault();

        await _estadoRepository.ActualizarAsync(new Estado
        {
            Id = id,
            PaisId = paisId,
            Clave = clave.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _estadoRepository.ObtenerAsync(id: id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _estadoRepository.ObtenerAsync(id: id)).FirstOrDefault();

        await _estadoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Estado? anterior, Estado? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private async Task ValidarPaisExisteAsync(int paisId)
    {
        var paises = await _paisRepository.ObtenerAsync(paisId);
        if (paises.Count == 0)
        {
            throw new ValidationException("El país seleccionado no existe");
        }
    }

    private static void ValidarCampos(string clave, string nombre)
    {
        if (string.IsNullOrWhiteSpace(clave))
        {
            throw new ValidationException("La clave del estado es obligatoria");
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del estado es obligatorio");
        }
    }

    private static EstadoDto MapearDto(Estado estado) => new(estado.Id, estado.PaisId, estado.Clave, estado.Nombre, estado.Activo);
}
