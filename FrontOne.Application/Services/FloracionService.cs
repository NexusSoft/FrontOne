using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class FloracionService
{
    private const string Modulo = "Acopio";

    private readonly IFloracionRepository _floracionRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public FloracionService(IFloracionRepository floracionRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _floracionRepository = floracionRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<FloracionDto>> ObtenerAsync()
    {
        var floraciones = await _floracionRepository.ObtenerAsync();
        return floraciones.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _floracionRepository.InsertarAsync(new Floracion { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _floracionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _floracionRepository.ObtenerAsync(id)).FirstOrDefault();

        await _floracionRepository.ActualizarAsync(new Floracion { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _floracionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _floracionRepository.ObtenerAsync(id)).FirstOrDefault();

        await _floracionRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Floracion? anterior, Floracion? nuevo)
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
            throw new ValidationException("El nombre de la floración es obligatorio");
        }
    }

    private static FloracionDto MapearDto(Floracion f) => new(f.Id, f.Nombre, f.Activo);
}
