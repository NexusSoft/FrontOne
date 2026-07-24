using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class PoblacionService
{
    private const string Modulo = "Catalogos";

    private readonly IPoblacionRepository _poblacionRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PoblacionService(IPoblacionRepository poblacionRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _poblacionRepository = poblacionRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PoblacionDto>> ObtenerAsync(int? municipioId = null)
    {
        var poblaciones = await _poblacionRepository.ObtenerAsync(municipioId);
        return poblaciones.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre, int municipioId)
    {
        ValidarCampos(nombre);

        var id = await _poblacionRepository.InsertarAsync(new Poblacion { Nombre = nombre.Trim(), MunicipioId = municipioId, Activo = true });

        var creado = (await _poblacionRepository.ObtenerAsync(id: id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, int municipioId, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _poblacionRepository.ObtenerAsync(id: id)).FirstOrDefault();

        await _poblacionRepository.ActualizarAsync(new Poblacion { Id = id, Nombre = nombre.Trim(), MunicipioId = municipioId, Activo = activo });

        var nuevo = (await _poblacionRepository.ObtenerAsync(id: id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _poblacionRepository.ObtenerAsync(id: id)).FirstOrDefault();

        await _poblacionRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Poblacion? anterior, Poblacion? nuevo)
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
            throw new ValidationException("El nombre de la población es obligatorio");
        }
    }

    private static PoblacionDto MapearDto(Poblacion poblacion) => new(poblacion.Id, poblacion.Nombre, poblacion.MunicipioId, poblacion.Activo);
}
