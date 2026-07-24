using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class TipoComercializacionService
{
    private const string Modulo = "Acopio";

    private readonly ITipoComercializacionRepository _tipoComercializacionRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public TipoComercializacionService(
        ITipoComercializacionRepository tipoComercializacionRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _tipoComercializacionRepository = tipoComercializacionRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<TipoComercializacionDto>> ObtenerAsync()
    {
        var tipos = await _tipoComercializacionRepository.ObtenerAsync();
        return tipos.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _tipoComercializacionRepository.InsertarAsync(new TipoComercializacion { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _tipoComercializacionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _tipoComercializacionRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoComercializacionRepository.ActualizarAsync(new TipoComercializacion { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _tipoComercializacionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _tipoComercializacionRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoComercializacionRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, TipoComercializacion? anterior, TipoComercializacion? nuevo)
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
            throw new ValidationException("El nombre del tipo de comercialización es obligatorio");
        }
    }

    private static TipoComercializacionDto MapearDto(TipoComercializacion t) => new(t.Id, t.Nombre, t.Activo);
}
