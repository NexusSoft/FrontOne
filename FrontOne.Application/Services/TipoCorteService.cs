using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class TipoCorteService
{
    private const string Modulo = "Acopio";

    private readonly ITipoCorteRepository _tipoCorteRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public TipoCorteService(ITipoCorteRepository tipoCorteRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _tipoCorteRepository = tipoCorteRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<TipoCorteDto>> ObtenerAsync()
    {
        var tiposCorte = await _tipoCorteRepository.ObtenerAsync();
        return tiposCorte.Select(MapearDto).ToList();
    }

    public async Task<int> CrearAsync(TipoCorteDto datos)
    {
        ValidarCampos(datos);

        var id = await _tipoCorteRepository.InsertarAsync(MapearEntidad(datos));

        var creado = (await _tipoCorteRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return id;
    }

    public async Task ActualizarAsync(TipoCorteDto datos)
    {
        ValidarCampos(datos);

        var anterior = (await _tipoCorteRepository.ObtenerAsync(datos.Id)).FirstOrDefault();

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _tipoCorteRepository.ActualizarAsync(entidad);

        var nuevo = (await _tipoCorteRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _tipoCorteRepository.ObtenerAsync(id)).FirstOrDefault();

        await _tipoCorteRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, TipoCorte? anterior, TipoCorte? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarCampos(TipoCorteDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            throw new ValidationException("El nombre del tipo de corte es obligatorio");
        }

        if (datos.FueraDeNormaGr < 0)
        {
            throw new ValidationException("El gramaje fuera de norma no puede ser negativo");
        }

        if (datos.TipoPagoId <= 0)
        {
            throw new ValidationException("Selecciona el tipo de pago");
        }
    }

    private static TipoCorte MapearEntidad(TipoCorteDto datos) => new()
    {
        Nombre = datos.Nombre.Trim(),
        FueraDeNormaGr = datos.FueraDeNormaGr,
        DanioMinimo = datos.DanioMinimo,
        TipoPagoId = datos.TipoPagoId,
        Activo = datos.Activo,
    };

    private static TipoCorteDto MapearDto(TipoCorte tc) => new(
        tc.Id,
        tc.Nombre,
        tc.FueraDeNormaGr,
        tc.DanioMinimo,
        tc.TipoPagoId,
        tc.Activo,
        tc.TipoPagoNombre);
}
