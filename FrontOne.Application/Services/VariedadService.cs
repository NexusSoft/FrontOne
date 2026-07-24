using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class VariedadService
{
    private const string Modulo = "Acopio";

    private readonly IVariedadRepository _variedadRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public VariedadService(IVariedadRepository variedadRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _variedadRepository = variedadRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<VariedadDto>> ObtenerAsync()
    {
        var variedades = await _variedadRepository.ObtenerAsync();
        return variedades.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _variedadRepository.InsertarAsync(new Variedad { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _variedadRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _variedadRepository.ObtenerAsync(id)).FirstOrDefault();

        await _variedadRepository.ActualizarAsync(new Variedad { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _variedadRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _variedadRepository.ObtenerAsync(id)).FirstOrDefault();

        await _variedadRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Variedad? anterior, Variedad? nuevo)
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
            throw new ValidationException("El nombre de la variedad es obligatorio");
        }
    }

    private static VariedadDto MapearDto(Variedad v) => new(v.Id, v.Nombre, v.Activo);
}
