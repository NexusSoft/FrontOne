using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class TipoProductoService
{
    private const string Modulo = "Catalogos";

    private readonly ITipoProductoRepository _tipoProductoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public TipoProductoService(ITipoProductoRepository tipoProductoRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _tipoProductoRepository = tipoProductoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<TipoProductoDto>> ObtenerAsync()
    {
        var tiposProducto = await _tipoProductoRepository.ObtenerAsync();
        return tiposProducto.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _tipoProductoRepository.InsertarAsync(new TipoProducto
        {
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _tipoProductoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _tipoProductoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoProductoRepository.ActualizarAsync(new TipoProducto
        {
            Id = id,
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _tipoProductoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _tipoProductoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoProductoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, TipoProducto? anterior, TipoProducto? nuevo)
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
            throw new ValidationException("El nombre del tipo de producto es obligatorio");
        }
    }

    private static TipoProductoDto MapearDto(TipoProducto tipoProducto) => new(tipoProducto.Id, tipoProducto.Nombre, tipoProducto.Activo);
}
