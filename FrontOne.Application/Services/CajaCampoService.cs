using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class CajaCampoService
{
    private const string Modulo = "Catalogos";

    private readonly ICajaCampoRepository _cajaCampoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CajaCampoService(ICajaCampoRepository cajaCampoRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _cajaCampoRepository = cajaCampoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<CajaCampoDto>> ObtenerAsync()
    {
        var cajas = await _cajaCampoRepository.ObtenerAsync();
        return cajas.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _cajaCampoRepository.InsertarAsync(new CajaCampo
        {
            Nombre = nombre.Trim(),
            Activo = true,
        });

        var creado = (await _cajaCampoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _cajaCampoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _cajaCampoRepository.ActualizarAsync(new CajaCampo
        {
            Id = id,
            Nombre = nombre.Trim(),
            Activo = activo,
        });

        var nuevo = (await _cajaCampoRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _cajaCampoRepository.ObtenerAsync(id)).FirstOrDefault();

        await _cajaCampoRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, CajaCampo? anterior, CajaCampo? nuevo)
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
            throw new ValidationException("El nombre de la caja de campo es obligatorio");
        }
    }

    private static CajaCampoDto MapearDto(CajaCampo cajaCampo) => new(cajaCampo.Id, cajaCampo.Nombre, cajaCampo.Activo);
}
