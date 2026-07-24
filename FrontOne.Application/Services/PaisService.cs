using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class PaisService
{
    private const string Modulo = "Catalogos";

    private readonly IPaisRepository _paisRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PaisService(IPaisRepository paisRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _paisRepository = paisRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PaisDto>> ObtenerAsync()
    {
        var paises = await _paisRepository.ObtenerAsync();
        return paises.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string clave, string nombre)
    {
        ValidarCampos(clave, nombre);

        var id = await _paisRepository.InsertarAsync(new Pais
        {
            Clave = clave.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _paisRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string clave, string nombre, bool activo)
    {
        ValidarCampos(clave, nombre);

        var anterior = (await _paisRepository.ObtenerAsync(id)).FirstOrDefault();

        await _paisRepository.ActualizarAsync(new Pais
        {
            Id = id,
            Clave = clave.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _paisRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _paisRepository.ObtenerAsync(id)).FirstOrDefault();

        await _paisRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Pais? anterior, Pais? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(string clave, string nombre)
    {
        if (string.IsNullOrWhiteSpace(clave))
        {
            throw new ValidationException("La clave del país es obligatoria");
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre del país es obligatorio");
        }
    }

    private static PaisDto MapearDto(Pais pais) => new(pais.Id, pais.Clave, pais.Nombre, pais.Activo);
}
