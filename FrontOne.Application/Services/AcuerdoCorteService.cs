using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class AcuerdoCorteService
{
    private const string Modulo = "Acopio";

    private readonly IAcuerdoCorteRepository _acuerdoCorteRepository;
    private readonly ITipoCorteRepository _tipoCorteRepository;
    private readonly ITipoPagoRepository _tipoPagoRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public AcuerdoCorteService(
        IAcuerdoCorteRepository acuerdoCorteRepository,
        ITipoCorteRepository tipoCorteRepository,
        ITipoPagoRepository tipoPagoRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _acuerdoCorteRepository = acuerdoCorteRepository;
        _tipoCorteRepository = tipoCorteRepository;
        _tipoPagoRepository = tipoPagoRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<AcuerdoCorteDto>> ObtenerAsync()
    {
        var acuerdos = await _acuerdoCorteRepository.ObtenerAsync();
        return acuerdos.Select(MapearDto).ToList();
    }

    public async Task<AcuerdoCorteDto?> ObtenerPorFolioAsync(string folio)
        => (await ObtenerAsync()).FirstOrDefault(a => a.Folio == folio);

    public async Task<(int Id, string Folio)> CrearAsync(AcuerdoCorteDto datos)
    {
        await ValidarAsync(datos);

        var resultado = await _acuerdoCorteRepository.InsertarAsync(MapearEntidad(datos));

        var creado = (await _acuerdoCorteRepository.ObtenerAsync(resultado.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return resultado;
    }

    public async Task ActualizarAsync(AcuerdoCorteDto datos)
    {
        var anterior = (await _acuerdoCorteRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("El acuerdo de corte que intentas actualizar ya no existe.");

        await ValidarAsync(datos);

        var entidad = MapearEntidad(datos);
        entidad.Id = datos.Id;
        await _acuerdoCorteRepository.ActualizarAsync(entidad);

        var nuevo = (await _acuerdoCorteRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _acuerdoCorteRepository.ObtenerAsync(id)).FirstOrDefault();

        await _acuerdoCorteRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // El TipoCorte elegido determina si el acuerdo se liquida con lista de precios (no se
    // captura Precio) o con precio fijo (no se referencia lista) — nunca ambos ni ninguno.
    public async Task<bool> NecesitaListaDePreciosAsync(int tipoCorteId)
    {
        var tipoCorte = (await _tipoCorteRepository.ObtenerAsync(tipoCorteId)).FirstOrDefault()
            ?? throw new ValidationException("El tipo de corte seleccionado ya no existe.");

        var tipoPago = (await _tipoPagoRepository.ObtenerAsync(tipoCorte.TipoPagoId)).FirstOrDefault()
            ?? throw new ValidationException("El tipo de pago del corte seleccionado ya no existe.");

        return tipoPago.NecesitaListaPrecios;
    }

    private async Task ValidarAsync(AcuerdoCorteDto datos)
    {
        if (datos.ProductorId <= 0)
        {
            throw new ValidationException("Selecciona el productor");
        }

        if (datos.FechaFin.Date < datos.FechaInicio.Date)
        {
            throw new ValidationException("La caducidad del acuerdo no puede ser anterior a la fecha de vigencia");
        }

        if (datos.ProductoId <= 0 || datos.VariedadId <= 0 || datos.TipoComercializacionId <= 0 || datos.TipoCorteId <= 0 || datos.MonedaId <= 0)
        {
            throw new ValidationException("Completa Producto, Variedad, Tipo de comercialización, Tipo de corte y Moneda");
        }

        var necesitaLista = await NecesitaListaDePreciosAsync(datos.TipoCorteId);
        if (necesitaLista)
        {
            if (datos.ListaPrecioFecha is null)
            {
                throw new ValidationException("El tipo de corte elegido requiere seleccionar una lista de precios");
            }

            if (datos.ListaPrecioFecha.Value.Date < datos.FechaInicio.Date || datos.ListaPrecioFecha.Value.Date > datos.FechaFin.Date)
            {
                throw new ValidationException("La lista de precios elegida debe estar dentro de la vigencia del acuerdo");
            }

            if (datos.ListaPrecioNumero is not (1 or 2 or 3))
            {
                throw new ValidationException("Selecciona cuál de las 3 listas de precios (1, 2 o 3) aplica a este acuerdo");
            }
        }
        else if (datos.Precio is null || datos.Precio < 0)
        {
            throw new ValidationException("El tipo de corte elegido requiere capturar un precio");
        }
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, AcuerdoCorte? anterior, AcuerdoCorte? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static AcuerdoCorte MapearEntidad(AcuerdoCorteDto datos) => new()
    {
        ProductorId = datos.ProductorId,
        FechaInicio = datos.FechaInicio.Date,
        FechaFin = datos.FechaFin.Date,
        ProductoId = datos.ProductoId,
        VariedadId = datos.VariedadId,
        TipoComercializacionId = datos.TipoComercializacionId,
        TipoCorteId = datos.TipoCorteId,
        Precio = datos.Precio,
        ListaPrecioFecha = datos.ListaPrecioFecha?.Date,
        ListaPrecioProductorId = datos.ListaPrecioProductorId,
        ListaPrecioNumero = datos.ListaPrecioNumero,
        MonedaId = datos.MonedaId,
        Observaciones = datos.Observaciones,
        Activo = datos.Activo,
    };

    private static AcuerdoCorteDto MapearDto(AcuerdoCorte a) => new(
        a.Id,
        a.Folio,
        a.ProductorId,
        a.ProductorNombre,
        a.FechaInicio,
        a.FechaFin,
        a.ProductoId,
        a.ProductoNombre,
        a.VariedadId,
        a.VariedadNombre,
        a.TipoComercializacionId,
        a.TipoComercializacionNombre,
        a.TipoCorteId,
        a.TipoCorteNombre,
        a.Precio,
        a.ListaPrecioFecha,
        a.ListaPrecioProductorId,
        a.ListaPrecioProductorNombre,
        a.ListaPrecioNumero,
        a.MonedaId,
        a.MonedaNombre,
        a.Observaciones,
        a.Activo);
}
