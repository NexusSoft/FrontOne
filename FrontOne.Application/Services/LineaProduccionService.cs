using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class LineaProduccionService
{
    private const string Modulo = "Catalogos";

    private readonly ILineaProduccionRepository _lineaProduccionRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public LineaProduccionService(ILineaProduccionRepository lineaProduccionRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _lineaProduccionRepository = lineaProduccionRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<LineaProduccionDto>> ObtenerAsync()
    {
        var lineas = await _lineaProduccionRepository.ObtenerAsync();
        return lineas.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(string nombre)
    {
        ValidarCampos(nombre);

        var id = await _lineaProduccionRepository.InsertarAsync(new LineaProduccion { Nombre = nombre.Trim(), Activo = true });

        var creado = (await _lineaProduccionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(int id, string nombre, bool activo)
    {
        ValidarCampos(nombre);

        var anterior = (await _lineaProduccionRepository.ObtenerAsync(id)).FirstOrDefault();

        await _lineaProduccionRepository.ActualizarAsync(new LineaProduccion { Id = id, Nombre = nombre.Trim(), Activo = activo });

        var nuevo = (await _lineaProduccionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _lineaProduccionRepository.ObtenerAsync(id)).FirstOrDefault();

        await _lineaProduccionRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, LineaProduccion? anterior, LineaProduccion? nuevo)
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
            throw new ValidationException("El nombre de la línea de producción es obligatorio");
        }
    }

    private static LineaProduccionDto MapearDto(LineaProduccion l) => new(l.Id, l.Nombre, l.Activo);
}
