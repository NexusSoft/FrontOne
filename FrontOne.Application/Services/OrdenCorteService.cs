using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

// Acuerdo de Corte vigente para una fecha dada — usado para poblar el LookUpEdit de
// "No. de Acuerdo" en OrdenCorteEditarForm (se recarga cada vez que cambia la Fecha).
public record AcuerdoVigenteDto(
    int Id,
    string Folio,
    int ProductorId,
    string ProductorNombre,
    int ProductoId,
    string ProductoNombre,
    int VariedadId,
    string VariedadNombre,
    int TipoCorteId,
    string TipoCorteNombre);

public class OrdenCorteService
{
    private const string Modulo = "Acopio";

    // Grupos de proveedores en SAP Business One (BusinessPartnerGroups.Code, verificados en el
    // ambiente de pruebas FRONTERRA_PROTO) — "Pagar el Corte a" usa el grupo Mercancia,
    // "Transportista" usa el grupo Acarreo. A diferencia de Cosecha (ListaPrecioCorte), estos
    // dos no tienen tabla local: se consultan en vivo cada vez que se abre el form, solo para
    // elegir CardCode/CardName, sin editar precios propios.
    private const int GrupoMercanciaCodigo = 108;
    private const int GrupoAcarreoCodigo = 101;

    private readonly IOrdenCorteRepository _ordenCorteRepository;
    private readonly IAcuerdoCorteRepository _acuerdoCorteRepository;
    private readonly IHuertaRepository _huertaRepository;
    private readonly IFloracionRepository _floracionRepository;
    private readonly IVariedadRepository _variedadRepository;
    private readonly IListaPrecioAcarreoRepository _listaPrecioAcarreoRepository;
    private readonly IZonaRepository _zonaRepository;
    private readonly IListaPrecioCorteRepository _listaPrecioCorteRepository;
    private readonly IJefeAcopioRepository _jefeAcopioRepository;
    private readonly ISapProveedorRepository _sapProveedorRepository;
    private readonly IMovimientoAlmacenRepository _movimientoAlmacenRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public OrdenCorteService(
        IOrdenCorteRepository ordenCorteRepository,
        IAcuerdoCorteRepository acuerdoCorteRepository,
        IHuertaRepository huertaRepository,
        IFloracionRepository floracionRepository,
        IVariedadRepository variedadRepository,
        IListaPrecioAcarreoRepository listaPrecioAcarreoRepository,
        IZonaRepository zonaRepository,
        IListaPrecioCorteRepository listaPrecioCorteRepository,
        IJefeAcopioRepository jefeAcopioRepository,
        ISapProveedorRepository sapProveedorRepository,
        IMovimientoAlmacenRepository movimientoAlmacenRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _ordenCorteRepository = ordenCorteRepository;
        _acuerdoCorteRepository = acuerdoCorteRepository;
        _huertaRepository = huertaRepository;
        _floracionRepository = floracionRepository;
        _variedadRepository = variedadRepository;
        _listaPrecioAcarreoRepository = listaPrecioAcarreoRepository;
        _zonaRepository = zonaRepository;
        _listaPrecioCorteRepository = listaPrecioCorteRepository;
        _jefeAcopioRepository = jefeAcopioRepository;
        _sapProveedorRepository = sapProveedorRepository;
        _movimientoAlmacenRepository = movimientoAlmacenRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public Task<IReadOnlyList<SapProveedorDto>> ObtenerProveedoresMercanciaAsync(CancellationToken cancellationToken = default)
        => _sapProveedorRepository.ObtenerPorGrupoAsync(GrupoMercanciaCodigo, cancellationToken);

    public Task<IReadOnlyList<SapProveedorDto>> ObtenerProveedoresAcarreoAsync(CancellationToken cancellationToken = default)
        => _sapProveedorRepository.ObtenerPorGrupoAsync(GrupoAcarreoCodigo, cancellationToken);

    public async Task<IReadOnlyList<OrdenCorteDto>> ObtenerAsync()
    {
        var ordenes = await _ordenCorteRepository.ObtenerAsync();
        return ordenes.Select(MapearDto).ToList();
    }

    public async Task<OrdenCorteDto?> ObtenerPorFolioAsync(string folio)
        => (await ObtenerAsync()).FirstOrDefault(o => o.Folio == folio);

    public async Task<IReadOnlyList<AcuerdoVigenteDto>> ObtenerAcuerdosVigentesAsync(DateTime fecha)
    {
        var acuerdos = await _acuerdoCorteRepository.ObtenerAsync();
        return acuerdos
            .Where(a => a.Activo && a.FechaInicio.Date <= fecha.Date && a.FechaFin.Date >= fecha.Date)
            .OrderByDescending(a => a.FechaCreacion)
            .Select(a => new AcuerdoVigenteDto(
                a.Id, a.Folio, a.ProductorId, a.ProductorNombre, a.ProductoId, a.ProductoNombre,
                a.VariedadId, a.VariedadNombre, a.TipoCorteId, a.TipoCorteNombre))
            .ToList();
    }

    public async Task<(int Id, string Folio)> CrearAsync(OrdenCorteDto datos)
    {
        var entidad = await ResolverYValidarAsync(datos);

        var resultado = await _ordenCorteRepository.InsertarAsync(entidad);
        await RegistrarMovimientoSalidaAsync(resultado.Id, entidad);

        var creado = (await _ordenCorteRepository.ObtenerAsync(resultado.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return resultado;
    }

    public async Task ActualizarAsync(OrdenCorteDto datos)
    {
        // Una vez que la Orden de Corte fue recibida (tiene línea en
        // Recepcion.RecepcionFrutaOrdenCorte), se bloquea su edición por completo — mismo
        // criterio que RecepcionFrutaService.ActualizarAsync con Lotes. El movimiento de Almacén
        // ya se calculó con estos datos al recibirse; editarla después lo dejaría desfasado.
        var disponible = await _ordenCorteRepository.EstaDisponibleParaRecepcionAsync(datos.Id);
        if (!disponible)
        {
            throw new ValidationException("Esta Orden de Corte ya fue recibida en una Recepción de Fruta y no se puede editar.");
        }

        var anterior = (await _ordenCorteRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("La orden de corte que intentas actualizar ya no existe.");

        var entidad = await ResolverYValidarAsync(datos);
        entidad.Id = datos.Id;
        await _ordenCorteRepository.ActualizarAsync(entidad);
        await RegistrarMovimientoSalidaAsync(datos.Id, entidad);

        var nuevo = (await _ordenCorteRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _ordenCorteRepository.ObtenerAsync(id)).FirstOrDefault();

        await _ordenCorteRepository.EliminarAsync(id);
        await _movimientoAlmacenRepository.EliminarMovimientosCajaCampoPorOrigenAsync("OrdenCorte", id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // Reemplaza los movimientos de caja de campo de esta orden — borra y vuelve a insertar en
    // cada Crear/Actualizar para que el saldo del Almacén nunca quede desfasado por una edición
    // (ver módulo Almacenes). La caja sale de Existencia y entra a EnCampo (todavía no vuelve del
    // corte) — dos movimientos con el mismo origen, se reemplazan juntos.
    private async Task RegistrarMovimientoSalidaAsync(int ordenCorteId, OrdenCorte entidad)
    {
        await _movimientoAlmacenRepository.EliminarMovimientosCajaCampoPorOrigenAsync("OrdenCorte", ordenCorteId);

        if (entidad.CajaCampoId is null)
        {
            return;
        }

        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var cantidad = (short)entidad.CajasEntregadas;

        await _movimientoAlmacenRepository.InsertarMovimientoCajaCampoAsync(new MovimientoCajaCampo
        {
            Fecha = entidad.Fecha,
            CajaCampoId = entidad.CajaCampoId.Value,
            Cuenta = CuentaAlmacen.Existencia.ToString(),
            TipoMovimiento = TipoMovimientoAlmacen.Salida.ToString(),
            Cantidad = cantidad,
            OrigenModulo = "OrdenCorte",
            OrigenId = ordenCorteId,
            Usuario = usuario,
        });

        await _movimientoAlmacenRepository.InsertarMovimientoCajaCampoAsync(new MovimientoCajaCampo
        {
            Fecha = entidad.Fecha,
            CajaCampoId = entidad.CajaCampoId.Value,
            Cuenta = CuentaAlmacen.EnCampo.ToString(),
            TipoMovimiento = TipoMovimientoAlmacen.Entrada.ToString(),
            Cantidad = cantidad,
            OrigenModulo = "OrdenCorte",
            OrigenId = ordenCorteId,
            Usuario = usuario,
        });
    }

    // Valida toda la orden y resuelve en el servidor los campos que se guardan como snapshot
    // (nunca se confía en lo que mande la UI para estos): Productor/RegistroSagarpa vienen del
    // Acuerdo/Huerta, Precio Acarreo y Kg Mínimo de ListaPrecioAcarreo+Zona según Cajas
    // Entregadas, y Costo/Pago/Cuadrilla del renglón activo de ListaPrecioCorte del jefe de
    // cuadrilla elegido — así si esos catálogos cambian después, la orden ya guardada no se
    // altera.
    private async Task<OrdenCorte> ResolverYValidarAsync(OrdenCorteDto datos)
    {
        if (datos.AcuerdoCorteId <= 0)
        {
            throw new ValidationException("Selecciona el acuerdo de corte");
        }

        var acuerdo = (await _acuerdoCorteRepository.ObtenerAsync(datos.AcuerdoCorteId)).FirstOrDefault()
            ?? throw new ValidationException("El acuerdo de corte seleccionado ya no existe.");

        if (!acuerdo.Activo)
        {
            throw new ValidationException("El acuerdo de corte seleccionado está inactivo.");
        }

        if (datos.Fecha.Date < acuerdo.FechaInicio.Date || datos.Fecha.Date > acuerdo.FechaFin.Date)
        {
            throw new ValidationException($"El acuerdo '{acuerdo.Folio}' no está vigente para la fecha {datos.Fecha:dd/MM/yyyy}.");
        }

        if (datos.HuertaId <= 0)
        {
            throw new ValidationException("Selecciona la huerta");
        }

        var huerta = (await _huertaRepository.ObtenerAsync(datos.HuertaId)).FirstOrDefault()
            ?? throw new ValidationException("La huerta seleccionada ya no existe.");

        if (huerta.ProductorId != acuerdo.ProductorId)
        {
            throw new ValidationException("La huerta seleccionada no pertenece al productor del acuerdo.");
        }

        if (datos.FloracionId <= 0)
        {
            throw new ValidationException("Selecciona la floración");
        }

        var floracion = (await _floracionRepository.ObtenerAsync(datos.FloracionId)).FirstOrDefault()
            ?? throw new ValidationException("La floración seleccionada ya no existe.");

        if (datos.VariedadId <= 0)
        {
            throw new ValidationException("Selecciona la variedad");
        }

        var variedad = (await _variedadRepository.ObtenerAsync(datos.VariedadId)).FirstOrDefault()
            ?? throw new ValidationException("La variedad seleccionada ya no existe.");

        if (string.IsNullOrWhiteSpace(datos.PagarCorteACardCode) || string.IsNullOrWhiteSpace(datos.PagarCorteANombre))
        {
            throw new ValidationException("Selecciona a quién se le paga el corte");
        }

        if (string.IsNullOrWhiteSpace(datos.TransportistaCardCode) || string.IsNullOrWhiteSpace(datos.TransportistaNombre))
        {
            throw new ValidationException("Selecciona el transportista");
        }

        if (datos.CajasEntregadas is not (300 or 400 or 500))
        {
            throw new ValidationException("Las cajas a entregar deben ser 300, 400 o 500");
        }

        if (datos.CajaCampoId is null or <= 0)
        {
            throw new ValidationException("Selecciona el color de caja de campo");
        }

        if (huerta.MunicipioId is null)
        {
            throw new ValidationException("La huerta seleccionada no tiene municipio asignado — no se puede calcular el precio de acarreo.");
        }

        var listaAcarreo = (await _listaPrecioAcarreoRepository.ObtenerAsync())
            .FirstOrDefault(l => l.MunicipioId == huerta.MunicipioId.Value);
        if (listaAcarreo is null)
        {
            throw new ValidationException("El municipio de la huerta no tiene precio de acarreo capturado (Acopio > Precios de Acarreo).");
        }

        var precioAcarreo = datos.CajasEntregadas switch
        {
            300 => listaAcarreo.Precio300,
            400 => listaAcarreo.Precio400,
            _ => listaAcarreo.Precio500,
        };

        var zona = (await _zonaRepository.ObtenerAsync(listaAcarreo.ZonaId)).FirstOrDefault()
            ?? throw new ValidationException("La zona de acarreo del municipio de la huerta ya no existe.");

        var kgMinimo = datos.CajasEntregadas switch
        {
            300 => zona.KgMinimo300,
            400 => zona.KgMinimo400,
            _ => zona.KgMinimo500,
        };

        if (string.IsNullOrWhiteSpace(datos.JefeCuadrillaCardCode))
        {
            throw new ValidationException("Selecciona el jefe de cuadrilla (empresa de corte)");
        }

        var jefeCuadrilla = (await _listaPrecioCorteRepository.ObtenerAsync())
            .FirstOrDefault(l => l.Activo && l.CardCode == datos.JefeCuadrillaCardCode);
        if (jefeCuadrilla is null)
        {
            throw new ValidationException("El jefe de cuadrilla elegido no tiene precios capturados o está inactivo (Acopio > Precios de Corte).");
        }

        if (datos.JefeAcopioId <= 0)
        {
            throw new ValidationException("Selecciona el jefe de acopio");
        }

        var jefeAcopio = (await _jefeAcopioRepository.ObtenerAsync(datos.JefeAcopioId)).FirstOrDefault()
            ?? throw new ValidationException("El jefe de acopio seleccionado ya no existe.");

        return new OrdenCorte
        {
            Fecha = datos.Fecha.Date,
            AcuerdoCorteId = acuerdo.Id,
            ProductorId = acuerdo.ProductorId,
            HuertaId = huerta.Id,
            FloracionId = floracion.Id,
            RegistroSagarpa = huerta.RegistroSagarpa,
            VariedadId = variedad.Id,
            PagarCorteACardCode = datos.PagarCorteACardCode.Trim(),
            PagarCorteANombre = datos.PagarCorteANombre.Trim(),
            TransportistaCardCode = datos.TransportistaCardCode.Trim(),
            TransportistaNombre = datos.TransportistaNombre.Trim(),
            PrecioAcarreo = precioAcarreo,
            NoCandado = datos.NoCandado,
            CajasEntregadas = datos.CajasEntregadas,
            JefeCuadrillaCardCode = jefeCuadrilla.CardCode!,
            JefeCuadrillaNombre = jefeCuadrilla.CardName,
            CostoKg = jefeCuadrilla.PrecioKg,
            PagoDia = jefeCuadrilla.PrecioDia,
            CuadrillaApoyo = jefeCuadrilla.CuadrillaApoyo,
            KgMinimo = kgMinimo,
            JefeAcopioId = jefeAcopio.Id,
            JefeAcopioNombre = jefeAcopio.Nombre,
            PuntoReunion = datos.PuntoReunion,
            Observaciones = datos.Observaciones,
            Cancelado = datos.Cancelado,
            CajaCampoId = datos.CajaCampoId,
        };
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, OrdenCorte? anterior, OrdenCorte? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static OrdenCorteDto MapearDto(OrdenCorte o) => new(
        o.Id,
        o.Folio,
        o.Fecha,
        o.AcuerdoCorteId,
        o.AcuerdoCorteFolio,
        o.ProductorId,
        o.ProductorNombre,
        o.HuertaId,
        o.HuertaNombre,
        o.FloracionId,
        o.FloracionNombre,
        o.RegistroSagarpa,
        o.VariedadId,
        o.VariedadNombre,
        o.TipoCorteNombre,
        o.TipoPagoNombre,
        o.PagarCorteACardCode,
        o.PagarCorteANombre,
        o.TransportistaCardCode,
        o.TransportistaNombre,
        o.PrecioAcarreo,
        o.NoCandado,
        o.CajasEntregadas,
        o.JefeCuadrillaCardCode,
        o.JefeCuadrillaNombre,
        o.CostoKg,
        o.PagoDia,
        o.CuadrillaApoyo,
        o.KgMinimo,
        o.JefeAcopioId,
        o.JefeAcopioNombre,
        o.PuntoReunion,
        o.Observaciones,
        o.Cancelado,
        o.CajaCampoId,
        o.CajaCampoNombre,
        o.EstaEnRecepcion);
}
