using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class IncidenciaService
{
    private const string Modulo = "Acopio";

    private readonly IIncidenciaRepository _incidenciaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public IncidenciaService(IIncidenciaRepository incidenciaRepository, AuditService auditService, ICurrentUserProvider currentUserProvider)
    {
        _incidenciaRepository = incidenciaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    // Grid principal: TODAS las Órdenes de Corte del rango de fecha, tengan o no Incidencia.
    public Task<IReadOnlyList<IncidenciaListadoDto>> ObtenerOrdenesConEstatusAsync(DateTime fechaDesde, DateTime fechaHasta)
        => _incidenciaRepository.ObtenerOrdenesConEstatusAsync(fechaDesde, fechaHasta);

    // Usado al abrir el formulario de captura — siempre regresa datos (los derivados de Orden de
    // Corte vienen resueltos aunque la Incidencia todavía no exista para esa orden).
    public Task<IncidenciaDto?> ObtenerPorOrdenCorteIdAsync(int ordenCorteId)
        => _incidenciaRepository.ObtenerPorOrdenCorteIdAsync(ordenCorteId);

    public Task<IReadOnlyList<IncidenciaReporteDto>> ObtenerParaReporteAsync(DateTime fechaDesde, DateTime fechaHasta)
        => _incidenciaRepository.ObtenerParaReporteAsync(fechaDesde, fechaHasta);

    public async Task<int> CrearAsync(IncidenciaDto datos)
    {
        var entidad = MapearEntidad(datos);

        var id = await _incidenciaRepository.InsertarAsync(entidad);

        var creada = (await ObtenerEntidadPorOrdenCorteIdAsync(datos.OrdenCorteId));
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creada);

        return id;
    }

    public async Task ActualizarAsync(IncidenciaDto datos)
    {
        if (datos.Id is null)
        {
            throw new ValidationException("No se puede actualizar una Incidencia que todavía no ha sido capturada.");
        }

        var anterior = await ObtenerEntidadPorOrdenCorteIdAsync(datos.OrdenCorteId);

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id.Value;
        await _incidenciaRepository.ActualizarAsync(entidad);

        var nueva = await ObtenerEntidadPorOrdenCorteIdAsync(datos.OrdenCorteId);
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nueva);
    }

    // La auditoría guarda el JSON completo de la entidad Incidencia (no del DTO, que trae también
    // los campos de solo lectura de Orden de Corte) — mismo criterio que el resto de los servicios.
    private async Task<Incidencia?> ObtenerEntidadPorOrdenCorteIdAsync(int ordenCorteId)
    {
        var dto = await _incidenciaRepository.ObtenerPorOrdenCorteIdAsync(ordenCorteId);
        if (dto?.Id is null)
        {
            return null;
        }

        return MapearEntidad(dto);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Incidencia? anterior, Incidencia? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static Incidencia MapearEntidad(IncidenciaDto datos) => new()
    {
        Id = datos.Id ?? 0,
        OrdenCorteId = datos.OrdenCorteId,
        SupervisorHuertaId = datos.SupervisorHuertaId,
        OdcSapCosecha = datos.OdcSapCosecha,
        NumeroTelefono = datos.NumeroTelefono,
        Placas = datos.Placas,
        OdcSapFlete = datos.OdcSapFlete,
        Bascula = datos.Bascula,
        PuntoReunion = datos.PuntoReunion,
        HoraLlegadaHuerta = datos.HoraLlegadaHuerta,
        CajasCosechadas = datos.CajasCosechadas,
        CajaPorCuadrilla = datos.CajaPorCuadrilla,
        Observaciones = datos.Observaciones,
        Incidencias = datos.Incidencias,
        Ajuste = datos.Ajuste,
    };
}
