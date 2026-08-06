using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

public class LoteService
{
    private const string Modulo = "Lotes";

    // Código de empresa fijo del Código de Trazabilidad (código de barras GS1-128, AI(10)
    // Batch/Lot) — se repite igual todos los años. Ver contexto/lotes.md para el detalle
    // completo de la fórmula (incluye el descifrado original de 11 dígitos y la extensión a 16
    // con el Id de Huerta).
    private const string CodigoEmpresaTrazabilidad = "089";

    private readonly ILoteRepository _loteRepository;
    private readonly IRecepcionFrutaRepository _recepcionFrutaRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public LoteService(
        ILoteRepository loteRepository,
        IRecepcionFrutaRepository recepcionFrutaRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _loteRepository = loteRepository;
        _recepcionFrutaRepository = recepcionFrutaRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<LoteDto>> ObtenerAsync()
    {
        var lotes = await _loteRepository.ObtenerAsync();
        return lotes.Select(MapearDto).ToList();
    }

    public async Task<LoteDto?> ObtenerPorIdAsync(int id)
        => (await _loteRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public async Task<LoteDto?> ObtenerPorRecepcionFrutaIdAsync(int recepcionFrutaId)
    {
        var loteId = await _loteRepository.ObtenerIdPorRecepcionFrutaIdAsync(recepcionFrutaId);
        return loteId is null ? null : await ObtenerPorIdAsync(loteId.Value);
    }

    public async Task<IReadOnlyList<LoteRecepcionDto>> ObtenerDetalleAsync(int loteId)
    {
        var lineas = await _loteRepository.ObtenerDetalleAsync(loteId);
        return lineas.Select(MapearDetalleDto).ToList();
    }

    public async Task<IReadOnlyList<RecepcionDisponibleParaLoteDto>> ObtenerRecepcionesDisponiblesTop100Async(
        int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode)
    {
        var recepciones = await _recepcionFrutaRepository.ObtenerTop100ParaLoteAsync(huertaId, acuerdoCorteId, pagarCorteACardCode);
        return recepciones.Select(MapearDisponibleDto).ToList();
    }

    public async Task<IReadOnlyList<RecepcionDisponibleParaLoteDto>> BuscarRecepcionesDisponiblesAsync(
        string filtro, int? huertaId, int? acuerdoCorteId, string? pagarCorteACardCode)
    {
        var recepciones = await _recepcionFrutaRepository.BuscarParaLoteAsync(filtro, huertaId, acuerdoCorteId, pagarCorteACardCode);
        return recepciones.Select(MapearDisponibleDto).ToList();
    }

    // El Código de Trazabilidad depende del Folio, que se genera hasta el INSERT (secuencia), y
    // de la Huerta, que no vive en el encabezado del Lote (se toma de sus Recepciones). Por eso
    // se inserta primero sin Código de Trazabilidad y se completa con un UPDATE inmediato en
    // cuanto se conoce el Folio — el encabezado nunca queda visible al usuario con este campo
    // vacío porque todo esto ocurre dentro de la misma llamada a CrearAsync. huertaId lo manda
    // LoteEditarForm tomándolo de la primera línea ya seleccionada en el grid (todavía no
    // persistida) — un Lote sin ninguna Recepción no tiene Huerta, así que no se puede calcular.
    public async Task<(int Id, string Folio)> CrearAsync(LoteDto datos, int huertaId)
    {
        if (huertaId <= 0)
        {
            throw new ValidationException("Agrega al menos una Recepción antes de guardar el Lote — el Código de Trazabilidad necesita saber de qué Huerta es.");
        }

        var entidad = Validar(datos);

        var resultado = await _loteRepository.InsertarAsync(entidad);

        entidad.Id = resultado.Id;
        entidad.CodigoTrazabilidad = CalcularCodigoTrazabilidad(entidad.Fecha, resultado.Folio, huertaId);
        await _loteRepository.ActualizarAsync(entidad);

        var creado = (await _loteRepository.ObtenerAsync(resultado.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, creado);

        return resultado;
    }

    public async Task ActualizarAsync(LoteDto datos)
    {
        var anterior = (await _loteRepository.ObtenerAsync(datos.Id)).FirstOrDefault()
            ?? throw new ValidationException("El Lote que intentas actualizar ya no existe.");

        var entidad = Validar(datos);
        entidad.Id = datos.Id;
        // El Código de Trazabilidad ya se calculó al crear el Lote y no depende de ningún otro
        // campo editable — se conserva tal cual, nunca se recalcula en una actualización.
        entidad.CodigoTrazabilidad = anterior.CodigoTrazabilidad;
        await _loteRepository.ActualizarAsync(entidad);

        var nuevo = (await _loteRepository.ObtenerAsync(datos.Id)).FirstOrDefault();
        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, nuevo);
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = (await _loteRepository.ObtenerAsync(id)).FirstOrDefault();

        await _loteRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // Agrega una Recepción al detalle del Lote. Valida que la descarga ya esté completa, que la
    // Recepción no esté ya en otro Lote y, si el Lote ya tiene líneas, que comparta Huerta/Acuerdo
    // de Corte/Proveedor de "Pagar el Corte a" con las ya agregadas — reglas explícitas del
    // usuario. El picker (SeleccionarRecepcionForm) ya filtra en SQL a solo las válidas, pero el
    // Service no confía ciegamente en lo que mande la UI (defensa en profundidad).
    public async Task AgregarLineaAsync(int loteId, int recepcionFrutaId)
    {
        var recepcion = await _recepcionFrutaRepository.ObtenerParaLotePorIdAsync(recepcionFrutaId)
            ?? throw new ValidationException("Esa Recepción de Fruta no existe o no tiene una Orden de Corte asociada.");

        // Mientras la descarga no esté completa, la Recepción no está cerrada (falta la pesada en
        // vacío): ni sus kilos ni sus cajas son definitivos, así que no puede conformar un Lote.
        if (!recepcion.DescargaCompleta)
        {
            throw new ValidationException(
                $"La Recepción '{recepcion.Folio}' todavía no tiene la descarga completa. Marca \"Descarga Completa\" en la Recepción antes de agregarla a un Lote.");
        }

        var enOtroLote = await _loteRepository.RecepcionEstaEnLoteAsync(recepcionFrutaId);
        if (enOtroLote)
        {
            throw new ValidationException("Esa Recepción ya forma parte de otro Lote.");
        }

        var lineasActuales = await _loteRepository.ObtenerDetalleAsync(loteId);
        if (lineasActuales.Count > 0)
        {
            var primera = lineasActuales[0];
            var recepcionPrimera = await _recepcionFrutaRepository.ObtenerParaLotePorIdAsync(primera.RecepcionFrutaId);
            if (recepcionPrimera is not null &&
                (recepcionPrimera.HuertaId != recepcion.HuertaId
                 || recepcionPrimera.AcuerdoCorteId != recepcion.AcuerdoCorteId
                 || recepcionPrimera.PagarCorteACardCode != recepcion.PagarCorteACardCode))
            {
                throw new ValidationException(
                    "Esa Recepción no se puede agregar: debe tener la misma Huerta, el mismo Acuerdo de Corte y el mismo proveedor de \"Pagar el Corte a\" que las demás Recepciones ya agregadas a este Lote.");
            }
        }

        await _loteRepository.InsertarDetalleAsync(loteId, recepcionFrutaId);

        // Al quedar incluida en el Lote, la Recepción recibe en su "No. de Lote" el Folio del
        // Lote que la contiene — pedido explícito del usuario.
        var lote = (await _loteRepository.ObtenerAsync(loteId)).FirstOrDefault();
        if (lote is not null)
        {
            await _recepcionFrutaRepository.ActualizarNoLoteAsync(recepcionFrutaId, lote.Folio);
        }
    }

    public async Task EliminarLineaAsync(int id)
    {
        var recepcionFrutaId = await _loteRepository.EliminarDetalleAsync(id);
        if (recepcionFrutaId.HasValue)
        {
            // La Recepción sale del Lote — se limpia el "No. de Lote" que se le había puesto.
            await _recepcionFrutaRepository.ActualizarNoLoteAsync(recepcionFrutaId.Value, null);
        }
    }

    // Fórmula del Código de Trazabilidad, 16 dígitos (ver contexto/lotes.md para el detalle e
    // historial completos): "089" (código de empresa, fijo) + Id de Huerta (5) + Folio del Lote
    // (5) + día juliano de la Fecha (3). Verificada con datos reales: Huerta 82303, Folio 18,
    // Fecha 31/07/2026 (día 212) → 0898230300018212.
    internal static string CalcularCodigoTrazabilidad(DateTime fecha, string folio, int huertaId)
    {
        var huertaCodigo = huertaId.ToString("00000");
        var folioCodigo = int.Parse(folio).ToString("00000");
        var diaJuliano = fecha.DayOfYear.ToString("000");
        return CodigoEmpresaTrazabilidad + huertaCodigo + folioCodigo + diaJuliano;
    }

    private static Lote Validar(LoteDto datos)
    {
        if (datos.LineaProduccionId <= 0)
        {
            throw new ValidationException("Selecciona la Línea de Producción del Lote.");
        }

        return new Lote
        {
            Fecha = datos.Fecha.Date,
            CodigoTrazabilidad = datos.CodigoTrazabilidad,
            Observaciones = datos.Observaciones,
            Kilogramos = datos.Kilogramos,
            Personalizado = datos.Personalizado,
            LineaProduccionId = datos.LineaProduccionId,
            PorcentajeMateriaSeca = datos.PorcentajeMateriaSeca,
            Estatus = datos.Estatus,
        };
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, Lote? anterior, Lote? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static LoteDto MapearDto(Lote l) => new(
        l.Id,
        l.Folio,
        l.Fecha,
        l.CodigoTrazabilidad,
        l.Observaciones,
        l.Kilogramos,
        l.Personalizado,
        l.LineaProduccionId,
        l.LineaProduccionNombre,
        l.PorcentajeMateriaSeca,
        l.Estatus,
        l.Recepciones,
        l.HuertaNombre,
        l.ProductorNombre);

    private static LoteRecepcionDto MapearDetalleDto(LoteRecepcion d) => new(
        d.Id,
        d.LoteId,
        d.RecepcionFrutaId,
        d.RecepcionFrutaFolio,
        d.NumeroTicket,
        d.Fecha,
        d.CoprefBico,
        d.PesoNeto,
        d.PorcentajeMateriaSeca,
        d.OrdenCorteFolio,
        d.AcuerdoCorteFolio);

    private static RecepcionDisponibleParaLoteDto MapearDisponibleDto(Domain.Entities.RecepcionDisponibleParaLote r) => new(
        r.Id,
        r.Folio,
        r.NumeroTicket,
        r.Fecha,
        r.PesoNeto,
        r.PorcentajeMateriaSeca,
        r.CoprefBico,
        r.HuertaId,
        r.HuertaNombre,
        r.AcuerdoCorteId,
        r.PagarCorteACardCode,
        r.PagarCorteANombre,
        r.OrdenCorteId,
        r.OrdenCorteFolio,
        r.AcuerdoCorteFolio);
}
