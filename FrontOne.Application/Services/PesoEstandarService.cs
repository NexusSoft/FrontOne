using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class PesoEstandarService
{
    private const string Modulo = "Catalogos";

    private readonly IPesoEstandarRepository _pesoEstandarRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PesoEstandarService(IPesoEstandarRepository pesoEstandarRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _pesoEstandarRepository = pesoEstandarRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PesoEstandarDto>> ObtenerAsync()
    {
        var pesosEstandar = await _pesoEstandarRepository.ObtenerAsync();
        return pesosEstandar.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(PesoEstandarDto datos)
    {
        ValidarCampos(datos);

        var id = await _pesoEstandarRepository.InsertarAsync(MapearEntidad(datos));

        var creado = (await _pesoEstandarRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(PesoEstandarDto datos)
    {
        ValidarCampos(datos);

        var anterior = (await _pesoEstandarRepository.ObtenerAsync(datos.Id)).FirstOrDefault();

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _pesoEstandarRepository.ActualizarAsync(entidad);

        var nuevo = (await _pesoEstandarRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _pesoEstandarRepository.ObtenerAsync(id)).FirstOrDefault();

        await _pesoEstandarRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, PesoEstandar? anterior, PesoEstandar? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(PesoEstandarDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Codigo))
        {
            throw new ValidationException("El código del peso estándar es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(datos.Descripcion))
        {
            throw new ValidationException("La descripción del peso estándar es obligatoria");
        }

        if (datos.PesoNeto <= 0)
        {
            throw new ValidationException("El peso neto debe ser mayor a cero");
        }

        if (datos.PesoPromedio <= 0)
        {
            throw new ValidationException("El peso promedio debe ser mayor a cero");
        }
    }

    private static PesoEstandar MapearEntidad(PesoEstandarDto datos) => new()
    {
        Codigo = datos.Codigo.Trim(),
        Descripcion = datos.Descripcion.Trim(),
        PesoNeto = datos.PesoNeto,
        PesoPromedio = datos.PesoPromedio,
        Activo = datos.Activo,
    };

    private static PesoEstandarDto MapearDto(PesoEstandar pesoEstandar) => new(
        pesoEstandar.Id,
        pesoEstandar.Codigo,
        pesoEstandar.Descripcion,
        pesoEstandar.PesoNeto,
        pesoEstandar.PesoPromedio,
        pesoEstandar.Activo);
}
