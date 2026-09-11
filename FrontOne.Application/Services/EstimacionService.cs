using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class EstimacionService
{
    private const string Modulo = "Acopio";

    private readonly IEstimacionRepository _estimacionRepository;
    private readonly IHuertaRepository _huertaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public EstimacionService(
        IEstimacionRepository estimacionRepository,
        IHuertaRepository huertaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _estimacionRepository = estimacionRepository;
        _huertaRepository = huertaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<EstimacionDto>> ObtenerAsync()
    {
        var estimaciones = await _estimacionRepository.ObtenerAsync();
        return estimaciones.Select(MapearDto).ToList();
    }

    public async Task<EstimacionDto?> ObtenerPorIdAsync(int id)
        => (await _estimacionRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public async Task<EstimacionDto?> ObtenerPorFolioAsync(string folio)
    {
        var estimacion = await _estimacionRepository.ObtenerPorFolioAsync(folio);
        return estimacion is null ? null : MapearDto(estimacion);
    }

    public Task<IReadOnlyList<EstimacionBusquedaDto>> ObtenerTop100Async(int? huertaId = null, bool soloAutorizadas = false)
        => _estimacionRepository.ObtenerTop100Async(huertaId, soloAutorizadas);

    public Task<IReadOnlyList<EstimacionBusquedaDto>> BuscarAsync(string filtro, int? huertaId = null, bool soloAutorizadas = false)
        => _estimacionRepository.BuscarAsync(filtro, huertaId, soloAutorizadas);

    public async Task<(int Id, string Folio)> GuardarAsync(EstimacionDto datos)
    {
        await ValidarHuertaAsync(datos.HuertaId);

        var entidad = MapearEntidad(datos);
        var resultado = await _estimacionRepository.InsertarAsync(entidad);

        var creado = await _estimacionRepository.ObtenerAsync(resultado.Id);
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado.FirstOrDefault());

        return resultado;
    }

    public async Task ActualizarAsync(EstimacionDto datos)
    {
        var anterior = (await _estimacionRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La estimación que intentas actualizar ya no existe.");

        if (anterior.Cerrada)
        {
            throw new ValidationException("No se puede editar una Estimación ya asignada a una Orden de Corte.");
        }

        if (anterior.Autorizada)
        {
            throw new ValidationException("No se puede editar una Estimación autorizada. Desautorízala primero.");
        }

        await ValidarHuertaAsync(datos.HuertaId);

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _estimacionRepository.ActualizarAsync(entidad);

        var nuevo = (await _estimacionRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public Task<IReadOnlyList<EstimacionAutorizacionDto>> ObtenerParaAutorizacionAsync(DateTime? fecha, bool? soloAutorizadas)
        => _estimacionRepository.ObtenerParaAutorizacionAsync(fecha, soloAutorizadas);

    public Task AutorizarAsync(int id) => CambiarAutorizacionAsync(id, true);

    public Task DesautorizarAsync(int id) => CambiarAutorizacionAsync(id, false);

    private async Task CambiarAutorizacionAsync(int id, bool autorizar)
    {
        var anterior = (await _estimacionRepository.ObtenerAsync(id)).FirstOrDefault()
            ?? throw new ValidationException("La estimación que intentas actualizar ya no existe.");

        if (anterior.Cerrada)
        {
            throw new ValidationException("No se puede autorizar o desautorizar una Estimación ya asignada a una Orden de Corte.");
        }

        await _estimacionRepository.MarcarAutorizadaAsync(id, autorizar);

        var nuevo = (await _estimacionRepository.ObtenerAsync(id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    private async Task ValidarHuertaAsync(int huertaId)
    {
        if (huertaId <= 0)
        {
            throw new ValidationException("Selecciona la huerta");
        }

        _ = (await _huertaRepository.ObtenerAsync(huertaId)).FirstOrDefault()
            ?? throw new ValidationException("La huerta seleccionada ya no existe.");
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Estimacion? anterior, Estimacion? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static Estimacion MapearEntidad(EstimacionDto datos) => new()
    {
        Fecha = datos.Fecha.Date,
        HuertaId = datos.HuertaId,
        RegistroSagarpa = datos.RegistroSagarpa,
        Kilos = datos.Kilos,
        AcopiadorId = datos.AcopiadorId,
        AcopiadorNombre = datos.AcopiadorNombre,
        PorcentajeCat1 = datos.PorcentajeCat1,
        PorcentajeCat2 = datos.PorcentajeCat2,
        PorcentajeNal = datos.PorcentajeNal,
        PorcentajeCalibre32 = datos.PorcentajeCalibre32,
        PorcentajeCalibre36 = datos.PorcentajeCalibre36,
        PorcentajeCalibre40 = datos.PorcentajeCalibre40,
        PorcentajeCalibre48 = datos.PorcentajeCalibre48,
        PorcentajeCalibre60 = datos.PorcentajeCalibre60,
        PorcentajeCalibre70 = datos.PorcentajeCalibre70,
        PorcentajeCalibre84 = datos.PorcentajeCalibre84,
        PorcentajeCalibre90 = datos.PorcentajeCalibre90,
        PorcentajeBorona = datos.PorcentajeBorona,
        PorcentajeCanica = datos.PorcentajeCanica,
        PorcentajeCuarta = datos.PorcentajeCuarta,
        PorcentajeDesecho = datos.PorcentajeDesecho,
        PorcentajeProceso = datos.PorcentajeProceso,
        ListaPrecioFecha = datos.ListaPrecioFecha.Date,
        ListaPrecioProductorId = datos.ListaPrecioProductorId,
        TipoLista = datos.TipoLista,
        PrecioSugerido = datos.PrecioSugerido,
        UsarListaMasReciente = datos.UsarListaMasReciente,
    };

    private static EstimacionDto MapearDto(Estimacion e) => new(
        e.Id,
        e.Folio,
        e.Fecha,
        e.HuertaId,
        e.HuertaNombre,
        e.RegistroSagarpa,
        e.Kilos,
        e.AcopiadorId,
        e.AcopiadorNombre,
        e.PorcentajeCat1,
        e.PorcentajeCat2,
        e.PorcentajeNal,
        e.PorcentajeCalibre32,
        e.PorcentajeCalibre36,
        e.PorcentajeCalibre40,
        e.PorcentajeCalibre48,
        e.PorcentajeCalibre60,
        e.PorcentajeCalibre70,
        e.PorcentajeCalibre84,
        e.PorcentajeCalibre90,
        e.PorcentajeBorona,
        e.PorcentajeCanica,
        e.PorcentajeCuarta,
        e.PorcentajeDesecho,
        e.PorcentajeProceso,
        e.ListaPrecioFecha,
        e.ListaPrecioProductorId,
        e.TipoLista,
        e.PrecioSugerido,
        e.Cerrada,
        e.Autorizada,
        e.UsarListaMasReciente);
}
