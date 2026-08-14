using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class EtiquetaService
{
    private const string Modulo = "Etiquetado";

    private readonly IEtiquetaRepository _etiquetaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public EtiquetaService(
        IEtiquetaRepository etiquetaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _etiquetaRepository = etiquetaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<EtiquetaDto>> ObtenerTodosAsync()
        => (await _etiquetaRepository.ObtenerTodosAsync()).Select(MapearDto).ToList();

    public async Task<IReadOnlyList<EtiquetaDto>> ObtenerEliminadosAsync()
        => (await _etiquetaRepository.ObtenerEliminadosAsync()).Select(MapearDto).ToList();

    public async Task<EtiquetaDto?> ObtenerPorIdAsync(int id)
    {
        var etiqueta = await _etiquetaRepository.ObtenerAsync(id);
        return etiqueta is null ? null : MapearDto(etiqueta);
    }

    public async Task<bool> ValidarNombreDisponibleAsync(string nombre, int? idExcluir = null)
        => !await _etiquetaRepository.ExisteNombreAsync(nombre, idExcluir);

    public async Task<int> CrearAsync(string nombre, decimal anchoPulgadas, decimal altoPulgadas, TipoEtiqueta tipo)
    {
        await ValidarCamposAsync(nombre, anchoPulgadas, altoPulgadas, idExcluir: null);

        var id = await _etiquetaRepository.InsertarAsync(nombre.Trim(), anchoPulgadas, altoPulgadas, (byte)tipo, definicionXml: null);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await LeerSnapshotAsync(id));

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, decimal anchoPulgadas, decimal altoPulgadas, TipoEtiqueta tipo)
    {
        await ValidarCamposAsync(nombre, anchoPulgadas, altoPulgadas, idExcluir: id);

        var anterior = await LeerSnapshotAsync(id);

        await _etiquetaRepository.ActualizarAsync(id, nombre.Trim(), anchoPulgadas, altoPulgadas, (byte)tipo);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    // Copia el layout completo de la etiqueta origen (mismo Tipo/Ancho/Alto/DefinicionXml), solo
    // pide el nombre nuevo — así lo pidió el usuario.
    public async Task<int> DuplicarAsync(int idOrigen, string nombreNuevo)
    {
        var origen = await _etiquetaRepository.ObtenerAsync(idOrigen)
            ?? throw new ValidationException("La etiqueta que intentas duplicar ya no existe");

        await ValidarCamposAsync(nombreNuevo, origen.AnchoPulgadas, origen.AltoPulgadas, idExcluir: null);

        var id = await _etiquetaRepository.InsertarAsync(
            nombreNuevo.Trim(), origen.AnchoPulgadas, origen.AltoPulgadas, (byte)origen.Tipo, origen.DefinicionXml);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await LeerSnapshotAsync(id));

        return id;
    }

    // Usado exclusivamente por el Diseñador al guardar el layout — no audita cambio de
    // Nombre/Ancho/Alto/Tipo (esos van por ActualizarAsync), solo registra que el layout cambió.
    public async Task GuardarLayoutAsync(int id, string definicionXml)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _etiquetaRepository.GuardarLayoutAsync(id, definicionXml);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    // Soft-delete: nunca borra el registro, solo lo marca inactivo — se puede recuperar después
    // con el permiso especial (ver RecuperarAsync).
    public async Task EliminarAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _etiquetaRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // La UI debe validar el permiso especial (Etiquetado/Etiquetas/Recuperar) ANTES de llamar
    // este método — el Service no vuelve a validar el permiso porque no tiene acceso a
    // ICurrentUserProvider.TienePermiso (eso vive en SessionContext, capa WinForms).
    public async Task RecuperarAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _etiquetaRepository.RecuperarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    private async Task ValidarCamposAsync(string nombre, decimal anchoPulgadas, decimal altoPulgadas, int? idExcluir)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationException("El nombre de la etiqueta es obligatorio");
        }

        if (anchoPulgadas <= 0 || altoPulgadas <= 0)
        {
            throw new ValidationException("El ancho y el alto de la etiqueta deben ser mayores a cero");
        }

        if (await _etiquetaRepository.ExisteNombreAsync(nombre.Trim(), idExcluir))
        {
            throw new ValidationException($"Ya existe una etiqueta activa con el nombre '{nombre.Trim()}'");
        }
    }

    private async Task<Etiqueta?> LeerSnapshotAsync(int id)
        => await _etiquetaRepository.ObtenerAsync(id);

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Etiqueta? anterior, Etiqueta? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static EtiquetaDto MapearDto(Etiqueta e) => new(
        e.Id,
        e.Nombre,
        e.AnchoPulgadas,
        e.AltoPulgadas,
        e.Tipo,
        e.DefinicionXml,
        e.Activo,
        e.FechaCreacion,
        e.FechaModificacion);
}
