using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;
using FrontOne.Shared.Utils;

namespace FrontOne.Application.Services;

public class PalletService
{
    private const string Modulo = "Pallets";

    private readonly IPalletRepository _palletRepository;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PalletService(
        IPalletRepository palletRepository,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _palletRepository = palletRepository;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<PalletDto>> ObtenerAsync()
        => (await _palletRepository.ObtenerAsync()).Select(MapearDto).ToList();

    public async Task<PalletDto?> ObtenerPorIdAsync(int id)
        => (await _palletRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public async Task<IReadOnlyList<PalletDetalleDto>> ObtenerDetalleAsync(int palletId)
        => (await _palletRepository.ObtenerDetalleAsync(palletId)).Select(MapearDetalleDto).ToList();

    public Task<IReadOnlyList<LoteEnProcesoParaPalletDto>> ObtenerLotesEnProcesoAsync(int? lineaProduccionId = null)
        => _palletRepository.ObtenerLotesEnProcesoAsync(lineaProduccionId);

    public Task<PalletReporteDto?> ObtenerParaReporteAsync(int id)
        => _palletRepository.ObtenerParaReporteAsync(id);

    // Huella ligera (Total + última fecha de modificación) para que PalletsForm sepa si el grid
    // quedó desactualizado sin tener que traer la lista completa cada vez — ver
    // Produccion.sp_Pallet_ObtenerUltimaModificacion.
    public Task<PalletUltimaModificacionDto> ObtenerUltimaModificacionAsync()
        => _palletRepository.ObtenerUltimaModificacionAsync();

    public Task<EtiquetaCajaDatosDto?> ObtenerDatosEtiquetaCajaAsync(int palletId)
        => _palletRepository.ObtenerDatosEtiquetaCajaAsync(palletId);

    public Task<EtiquetaPalletEncabezadoDto?> ObtenerDatosEtiquetaPalletEncabezadoAsync(int palletId)
        => _palletRepository.ObtenerDatosEtiquetaPalletEncabezadoAsync(palletId);

    public Task<IReadOnlyList<EtiquetaPalletDetalleDto>> ObtenerDatosEtiquetaPalletDetalleAsync(int palletId)
        => _palletRepository.ObtenerDatosEtiquetaPalletDetalleAsync(palletId);

    public Task<EtiquetaSagarpaDatosDto?> ObtenerDatosEtiquetaSagarpaAsync(int palletId)
        => _palletRepository.ObtenerDatosEtiquetaSagarpaAsync(palletId);

    public async Task<int> CrearAsync(int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal)
    {
        ValidarLineaProduccion(lineaProduccionId);
        ValidarPesoReal(pesoReal);
        ValidarProductoEncabezado(esMixto, productoTerminadoId);

        var id = await _palletRepository.InsertarAsync(lineaProduccionId, esMixto, productoTerminadoId, pesoReal);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await LeerSnapshotAsync(id));

        return id;
    }

    // Pallet Neutro: ajuste de Merma/Diferencia a Favor para cerrar una Corrida cuando Kilos
    // Procesados no cierra exacto contra Kilos a Procesar. A diferencia de una línea normal, el
    // Kilogramos aquí sí puede ser negativo (Diferencia a Favor resta) — el signo ya lo resuelve
    // quien llama (PalletNeutroCapturaForm) según el producto elegido.
    public async Task<int> CrearNeutroAsync(int corridaId, int productoTerminadoId, decimal kilogramos)
    {
        if (corridaId <= 0)
        {
            throw new ValidationException("Selecciona una corrida");
        }

        if (productoTerminadoId <= 0)
        {
            throw new ValidationException("Selecciona un producto terminado");
        }

        if (kilogramos == 0)
        {
            throw new ValidationException("Captura un monto de kilogramos distinto de cero");
        }

        var id = await _palletRepository.CrearNeutroAsync(corridaId, productoTerminadoId, kilogramos);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await LeerSnapshotAsync(id));

        return id;
    }

    public async Task ActualizarEncabezadoAsync(int id, int lineaProduccionId, bool esMixto, int? productoTerminadoId, decimal? pesoReal)
    {
        ValidarLineaProduccion(lineaProduccionId);
        ValidarPesoReal(pesoReal);
        ValidarProductoEncabezado(esMixto, productoTerminadoId);

        var anterior = await LeerSnapshotAsync(id);

        await _palletRepository.ActualizarEncabezadoAsync(id, lineaProduccionId, esMixto, productoTerminadoId, pesoReal);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    // Bloquear es una modificación definitiva del encabezado (pasa a Estatus Empacado y congela
    // todo) — se audita como Modificar, no como una acción propia: la auditoría del proyecto solo
    // maneja Crear/Modificar/Eliminar y el snapshot antes/después ya deja ver qué cambió.
    public async Task BloquearAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _palletRepository.BloquearAsync(id);
        await RecalcularGs1VoiceCodePalletAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _palletRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // Si el pallet ya tiene una línea del mismo Lote (Corrida) y el mismo Producto, las cajas (o
    // los kilogramos, si es Granel) se suman a esa línea existente en vez de crear un renglón
    // duplicado — así el detalle nunca queda con dos filas idénticas por capturar el mismo
    // lote/producto en pasadas distintas.
    public async Task<int> AgregarLineaAsync(int palletId, int corridaId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
    {
        ValidarLinea(corridaId, productoTerminadoId, cajas, kilogramos);

        var detalleActual = await _palletRepository.ObtenerDetalleAsync(palletId);
        var existente = detalleActual.FirstOrDefault(d => d.CorridaId == corridaId && d.ProductoTerminadoId == productoTerminadoId);

        var anterior = await LeerSnapshotAsync(palletId);

        int id;
        if (existente is not null)
        {
            // Caja: suma Cajas (el SP recalcula Kilogramos). Granel: suma Kilogramos directo (no
            // hay Cajas que sumar). Se sigue el mismo criterio que ya trae la línea existente.
            int? cajasTotal = cajas is null ? null : (existente.Cajas ?? 0) + cajas;
            decimal? kilogramosTotal = kilogramos is null ? null : existente.Kilogramos + kilogramos;
            await _palletRepository.ActualizarDetalleAsync(existente.Id, productoTerminadoId, cajasTotal, kilogramosTotal, porcentajeMateriaSeca);
            id = existente.Id;
        }
        else
        {
            var insertado = await _palletRepository.InsertarDetalleAsync(palletId, corridaId, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca);
            id = insertado.Id;
        }

        await RecalcularGs1VoiceCodePalletAsync(palletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(palletId));

        return id;
    }

    public async Task ActualizarLineaAsync(int palletId, int id, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
    {
        if (productoTerminadoId <= 0)
        {
            throw new ValidationException("Selecciona un producto terminado");
        }

        if (cajas is null or <= 0 && kilogramos is null or <= 0)
        {
            throw new ValidationException("Captura las cajas o los kilogramos de esta línea");
        }

        var anterior = await LeerSnapshotAsync(palletId);

        await _palletRepository.ActualizarDetalleAsync(id, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca);
        await RecalcularGs1VoiceCodePalletAsync(palletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(palletId));
    }

    // Recalcula CodigoGs1128/VoiceCode de TODAS las líneas del pallet (no solo la que se acaba de
    // tocar) — así una línea que quedó en NULL porque en su momento el Lote/Producto no tenía
    // GTIN/Código de Trazabilidad se corrige sola en cuanto ese catálogo se completa y el usuario
    // vuelve a modificar el pallet (agregar/editar/eliminar una línea) o lo cierra (Bloquear), sin
    // necesidad de correr el SP manualmente. CodigoGs1128 se recalcula 100% en el SP (T-SQL); el
    // VoiceCode necesita CRC-16 (no práctico en T-SQL) — se calcula aquí con VoicePickCodeCalculator
    // y se persiste con un UPDATE dedicado por línea.
    private async Task RecalcularGs1VoiceCodePalletAsync(int palletId)
    {
        await _palletRepository.RecalcularGs1128PalletAsync(palletId);

        var filas = await _palletRepository.ObtenerParaRecalcularVoiceCodeAsync(palletId);
        foreach (var fila in filas)
        {
            var (low, high) = VoicePickCodeCalculator.Calcular(fila.CodigoGtin, fila.CodigoTrazabilidad, fila.FechaLote);
            await _palletRepository.ActualizarVoiceCodeDetalleAsync(fila.Id, low, high);
        }
    }

    public async Task EliminarLineaAsync(int palletId, int id)
    {
        var anterior = await LeerSnapshotAsync(palletId);

        await _palletRepository.EliminarDetalleAsync(id);
        await RecalcularGs1VoiceCodePalletAsync(palletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(palletId));
    }

    // El snapshot de auditoría de un Pallet es siempre encabezado + detalle completo: un cambio
    // en una línea altera Estatus/% Materia Seca/kilos del encabezado, así que registrar solo una
    // de las dos partes dejaría el registro incompleto.
    private async Task<SnapshotPallet?> LeerSnapshotAsync(int id)
    {
        var encabezado = (await _palletRepository.ObtenerAsync(id)).FirstOrDefault();
        if (encabezado is null)
        {
            return null;
        }

        var detalle = await _palletRepository.ObtenerDetalleAsync(id);
        return new SnapshotPallet(encabezado, detalle);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, SnapshotPallet? anterior, SnapshotPallet? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarLineaProduccion(int lineaProduccionId)
    {
        if (lineaProduccionId <= 0)
        {
            throw new ValidationException("Selecciona una línea de producción");
        }
    }

    private static void ValidarPesoReal(decimal? pesoReal)
    {
        if (pesoReal is < 0)
        {
            throw new ValidationException("El peso real no puede ser negativo");
        }
    }

    // Un pallet no mixto necesita un producto de referencia desde el encabezado — es lo que fija
    // el objetivo de cajas y lo que se autoselecciona en cada línea de detalle.
    private static void ValidarProductoEncabezado(bool esMixto, int? productoTerminadoId)
    {
        if (!esMixto && productoTerminadoId is null or <= 0)
        {
            throw new ValidationException("Selecciona un producto para este pallet");
        }
    }

    // No exige "cajas > 0" incondicional: una línea Granel llega sin cajas (null) y en su lugar
    // trae kilogramos > 0. Exactamente uno de los dos debe venir con valor válido.
    private static void ValidarLinea(int corridaId, int productoTerminadoId, int? cajas, decimal? kilogramos)
    {
        if (corridaId <= 0)
        {
            throw new ValidationException("Selecciona un lote en proceso");
        }

        if (productoTerminadoId <= 0)
        {
            throw new ValidationException("Selecciona un producto terminado");
        }

        if (cajas is null or <= 0 && kilogramos is null or <= 0)
        {
            throw new ValidationException("Captura las cajas o los kilogramos de esta línea");
        }
    }

    private static PalletDto MapearDto(Pallet p) => new(
        p.Id,
        p.Folio,
        p.FechaCreacion,
        p.HoraCreacion,
        p.Estatus,
        p.LineaProduccionId,
        p.LineaProduccionNombre,
        p.EsMixto,
        p.ProductoTerminadoId,
        p.PorcentajeMateriaSeca,
        p.PesoReal,
        p.Bloqueado,
        p.FechaBloqueo,
        p.NoReempaque,
        p.PrimeraCorrida,
        p.EsNeutro,
        p.TotalCajas,
        p.TotalKilogramos,
        p.ProductoDescripcion,
        p.ProductoCodigoSap,
        p.FechaCreacionRegistro);

    private static PalletDetalleDto MapearDetalleDto(PalletDetalle d) => new(
        d.Id,
        d.PalletId,
        d.CorridaId,
        d.LoteId,
        d.LoteFolio,
        d.ProductoTerminadoId,
        d.ProductoCodigoSap,
        d.ProductoDescripcion,
        d.Cajas,
        d.Kilogramos,
        d.PorcentajeMateriaSeca,
        d.CajasPorPallet,
        d.LoteEnProceso,
        d.CodigoGs1128,
        d.VoiceCodeLow,
        d.VoiceCodeHigh);

    private sealed record SnapshotPallet(Pallet Encabezado, IReadOnlyList<PalletDetalle> Detalle);
}
