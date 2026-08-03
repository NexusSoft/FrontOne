using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class MarcaService
{
    private const string Modulo = "Catalogos";

    private readonly IMarcaRepository _marcaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public MarcaService(IMarcaRepository marcaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _marcaRepository = marcaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<MarcaDto>> ObtenerAsync()
    {
        var marcas = await _marcaRepository.ObtenerAsync();
        return marcas.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _marcaRepository.InsertarAsync(new Marca
        {
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _marcaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _marcaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _marcaRepository.ActualizarAsync(new Marca
        {
            Id = id,
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _marcaRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _marcaRepository.ObtenerAsync(id)).FirstOrDefault();

        await _marcaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Marca? anterior, Marca? nuevo)
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
            throw new ValidationException("El nombre de la marca es obligatorio");
        }
    }

    private static MarcaDto MapearDto(Marca marca) => new(marca.Id, marca.Nombre, marca.Activo);
}
