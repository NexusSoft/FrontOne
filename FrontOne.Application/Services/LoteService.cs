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

    // Código de empresa fijo del folio juliano de 11 dígitos — se repite igual todos los años,
    // confirmado con 2 ejemplos reales del usuario (folio 162/163 de 2026, folio 1 de 2024).
    // Ver contexto/lotes.md para el detalle completo de la fórmula.
    private const string CodigoEmpresaReferencia = "089";

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

    // La Referencia depende del Folio, que se genera hasta el INSERT (secuencia). Por eso se
    // inserta primero sin Referencia y se completa con un UPDATE inmediato en cuanto se conoce
    // el Folio — el encabezado nunca queda visible al usuario con Referencia vacía porque todo
    // esto ocurre dentro de la misma llamada a CrearAsync.
    public async Task<(int Id, string Folio)> CrearAsync(LoteDto datos)
    {
        var entidad = Validar(datos);

        var resultado = await _loteRepository.InsertarAsync(entidad);

        entidad.Id = resultado.Id;
        entidad.Referencia = CalcularReferencia(entidad.Fecha, resultado.Folio);
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
        // La Referencia ya se calculó al crear el Lote y no depende de ningún otro campo editable
        // — se conserva tal cual, nunca se recalcula en una actualización.
        entidad.Referencia = anterior.Referencia;
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

    // Agrega una Recepción al detalle del Lote. Valida disponibilidad (que no esté ya en otro
    // Lote) y, si el Lote ya tiene líneas, que la nueva Recepción comparta Huerta/Acuerdo de
    // Corte/Proveedor de "Pagar el Corte a" con las ya agregadas — regla explícita del usuario.
    // El picker (SeleccionarRecepcionForm) ya filtra en SQL a solo las compatibles, pero el
    // Service no confía ciegamente en lo que mande la UI (defensa en profundidad).
    public async Task AgregarLineaAsync(int loteId, int recepcionFrutaId)
    {
        var recepcion = await _recepcionFrutaRepository.ObtenerParaLotePorIdAsync(recepcionFrutaId)
            ?? throw new ValidationException("Esa Recepción de Fruta no existe o no tiene una Orden de Corte asociada.");

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

    // Fórmula del folio juliano de 11 dígitos (ver contexto/lotes.md): "089" (código de empresa,
    // fijo) + centena del día juliano de la Fecha + Folio del Lote a 5 dígitos + decena/unidad
    // del día juliano. Verificada con 2 ejemplos reales: 30/07/2026 (día 211) folio 162 →
    // 08920016211; 19/11/2024 (día 324) folio 1 → 08930000124.
    internal static string CalcularReferencia(DateTime fecha, string folio)
    {
        var diaJuliano = fecha.DayOfYear.ToString("000");
        var folioReferencia = int.Parse(folio).ToString("00000");
        return CodigoEmpresaReferencia + diaJuliano[0] + folioReferencia + diaJuliano[1..];
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
            Referencia = datos.Referencia,
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
        l.Referencia,
        l.Observaciones,
        l.Kilogramos,
        l.Personalizado,
        l.LineaProduccionId,
        l.LineaProduccionNombre,
        l.PorcentajeMateriaSeca,
        l.Estatus,
        l.Tickets,
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
        d.PorcentajeMateriaSeca);

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
        r.PagarCorteANombre);
}
