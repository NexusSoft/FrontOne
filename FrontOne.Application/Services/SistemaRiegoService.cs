using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class SistemaRiegoService
{
    private const string Modulo = "Catalogos";

    private readonly ISistemaRiegoRepository _sistemaRiegoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public SistemaRiegoService(ISistemaRiegoRepository sistemaRiegoRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _sistemaRiegoRepository = sistemaRiegoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<SistemaRiegoDto>> ObtenerAsync()
    {
        var sistemas = await _sistemaRiegoRepository.ObtenerAsync();
        return sistemas.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _sistemaRiegoRepository.InsertarAsync(new SistemaRiego { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _sistemaRiegoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _sistemaRiegoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _sistemaRiegoRepository.ActualizarAsync(new SistemaRiego { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _sistemaRiegoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _sistemaRiegoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _sistemaRiegoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, SistemaRiego? anterior, SistemaRiego? nuevo)
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
            throw new ValidationException("El nombre del sistema de riego es obligatorio");
        }
    }

    private static SistemaRiegoDto MapearDto(SistemaRiego sistemaRiego) => new(sistemaRiego.Id, sistemaRiego.Nombre, sistemaRiego.Activo);
}
