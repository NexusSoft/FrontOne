using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class JefeAcopioService
{
    private const string Modulo = "Acopio";

    private readonly IJefeAcopioRepository _jefeAcopioRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public JefeAcopioService(IJefeAcopioRepository jefeAcopioRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _jefeAcopioRepository = jefeAcopioRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<JefeAcopioDto>> ObtenerAsync()
    {
        var jefes = await _jefeAcopioRepository.ObtenerAsync();
        return jefes.Select(MapearDto).ToList();
    }

    public async Task<JefeAcopioDto?> ObtenerPorIdAsync(int id)
    {
        var jefe = (await _jefeAcopioRepository.ObtenerAsync(id)).FirstOrDefault();
        return jefe is null ? null : MapearDto(jefe);
    }

    public async Task<IReadOnlyList<JefeAcopioDto>> BuscarAsync(string filtro)
    {
        var jefes = await _jefeAcopioRepository.BuscarAsync(filtro);
        return jefes.Select(MapearDto).ToList();
    }

    // Carga inicial del picker (sin filtro) para que el grid no se vea vacío al abrir.
    public async Task<IReadOnlyList<JefeAcopioDto>> ObtenerTop100Async()
    {
        var jefes = await _jefeAcopioRepository.ObtenerTop100Async();
        return jefes.Select(MapearDto).ToList();
    }

    public async Task<JefeAcopioDto?> ObtenerPrimeroAsync()
    {
        var jefe = await _jefeAcopioRepository.ObtenerPrimeroAsync();
        return jefe is null ? null : MapearDto(jefe);
    }

    public async Task<JefeAcopioDto?> ObtenerUltimoAsync()
    {
        var jefe = await _jefeAcopioRepository.ObtenerUltimoAsync();
        return jefe is null ? null : MapearDto(jefe);
    }

    public async Task<JefeAcopioDto?> ObtenerSiguienteAsync(int id)
    {
        var jefe = await _jefeAcopioRepository.ObtenerSiguienteAsync(id);
        return jefe is null ? null : MapearDto(jefe);
    }

    public async Task<JefeAcopioDto?> ObtenerAnteriorAsync(int id)
    {
        var jefe = await _jefeAcopioRepository.ObtenerAnteriorAsync(id);
        return jefe is null ? null : MapearDto(jefe);
    }

    public async Task<(int Id, string Clave)> CrearAsync(JefeAcopioDto datos)
    {
        ValidarCampos(datos);

        var resultado = await _jefeAcopioRepository.InsertarAsync(MapearEntidad(datos));

        var creado = (await _jefeAcopioRepository.ObtenerAsync(resultado.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return resultado;
    }

    public async Task ActualizarAsync(JefeAcopioDto datos)
    {
        ValidarCampos(datos);

        var anterior = (await _jefeAcopioRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("El jefe de acopio que intentas actualizar ya no existe.");

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _jefeAcopioRepository.ActualizarAsync(entidad);

        var nuevo = (await _jefeAcopioRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _jefeAcopioRepository.ObtenerAsync(id)).FirstOrDefault();

        await _jefeAcopioRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    private static void ValidarCampos(JefeAcopioDto datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            throw new ValidationException("El nombre del jefe de acopio es obligatorio");
        }
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, JefeAcopio? anterior, JefeAcopio? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static JefeAcopio MapearEntidad(JefeAcopioDto datos) => new()
    {
        Nombre = datos.Nombre.Trim(),
        Domicilio = datos.Domicilio,
        Colonia = datos.Colonia,
        CodigoPostal = datos.CodigoPostal,
        PoblacionId = datos.PoblacionId,
        MunicipioId = datos.MunicipioId,
        EstadoId = datos.EstadoId,
        Telefono = datos.Telefono,
        Celular = datos.Celular,
        Email = datos.Email,
        Observaciones = datos.Observaciones,
        Activo = datos.Activo,
    };

    private static JefeAcopioDto MapearDto(JefeAcopio j) => new(
        j.Id,
        j.Clave,
        j.Nombre,
        j.Domicilio,
        j.Colonia,
        j.CodigoPostal,
        j.PoblacionId,
        j.MunicipioId,
        j.EstadoId,
        j.Telefono,
        j.Celular,
        j.Email,
        j.Observaciones,
        j.Activo);
}
