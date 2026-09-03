using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Entities;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

// Reempaques es un módulo de construcción, no un mundo paralelo: desarma pallets ya armados y
// deposita sus kilos (reservados por lote dentro del folio) en pallets del módulo de Pallets —
// mismo Produccion.Pallet/PalletDetalle, nunca una tabla propia. Por eso depende de PalletService:
// toda escritura de una línea de salida reusa RecalcularGs1VoiceCodePalletAsync/EliminarLineaAsync
// en vez de reimplementarlos.
public class ReempaqueService
{
    private const string Modulo = "Reempaques";

    private readonly IReempaqueRepository _reempaqueRepository;
    private readonly PalletService _palletService;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ReempaqueService(
        IReempaqueRepository reempaqueRepository,
        PalletService palletService,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _reempaqueRepository = reempaqueRepository;
        _palletService = palletService;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IReadOnlyList<ReempaqueDto>> ObtenerAsync()
        => (await _reempaqueRepository.ObtenerAsync()).Select(MapearDto).ToList();

    public async Task<ReempaqueDto?> ObtenerPorIdAsync(int id)
        => (await _reempaqueRepository.ObtenerAsync(id)).Select(MapearDto).FirstOrDefault();

    public async Task<ReempaqueDto?> ObtenerPorFolioAsync(string folio)
    {
        var reempaque = await _reempaqueRepository.ObtenerPorFolioAsync(folio);
        return reempaque is null ? null : MapearDto(reempaque);
    }

    public Task<IReadOnlyList<ReempaquePalletDisponibleDto>> ObtenerPalletsOrigenDisponiblesAsync(string? folio = null)
        => _reempaqueRepository.ObtenerPalletsOrigenDisponiblesAsync(folio);

    public Task<IReadOnlyList<ReempaquePalletDisponibleDto>> ObtenerPalletsDestinoDisponiblesAsync(int reempaqueId, string? folio = null)
        => _reempaqueRepository.ObtenerPalletsDestinoDisponiblesAsync(reempaqueId, folio);

    public async Task<IReadOnlyList<ReempaqueDetalleDto>> ObtenerDetalleEntradaAsync(int reempaqueId)
        => (await _reempaqueRepository.ObtenerDetalleEntradaAsync(reempaqueId)).Select(MapearDetalleDto).ToList();

    public Task<IReadOnlyList<ReempaqueSalidaFilaDto>> ObtenerDetalleSalidaAsync(int reempaqueId)
        => _reempaqueRepository.ObtenerDetalleSalidaAsync(reempaqueId);

    public async Task<int> CrearAsync(string motivo)
    {
        ValidarMotivo(motivo);

        var id = await _reempaqueRepository.InsertarAsync(motivo);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await LeerSnapshotAsync(id));

        return id;
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _reempaqueRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    // Solo se puede agregar un pallet completo (nunca una parte) — el SP genera una fila de saldo
    // por cada línea de su detalle, ya reservada al folio, sin tocar la Corrida original.
    public async Task AgregarPalletOrigenAsync(int reempaqueId, int palletId)
    {
        if (palletId <= 0)
        {
            throw new ValidationException("Selecciona un pallet");
        }

        var anterior = await LeerSnapshotAsync(reempaqueId);

        await _reempaqueRepository.AgregarPalletOrigenAsync(reempaqueId, palletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(reempaqueId));
    }

    public async Task QuitarPalletOrigenAsync(int reempaqueId, int palletId)
    {
        var anterior = await LeerSnapshotAsync(reempaqueId);

        await _reempaqueRepository.QuitarPalletOrigenAsync(reempaqueId, palletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(reempaqueId));
    }

    // Deposita cajas/kilogramos de una línea de saldo en un pallet DESTINO ya existente (normal o
    // nacido de un reempaque anterior) — nunca crea un pallet propio del módulo. Reusa
    // PalletService.RecalcularGs1VoiceCodePalletAsync vía el mismo contrato de retorno que
    // sp_PalletDetalle_Insertar, así que GS1/VoiceCode salen correctos sin duplicar el cálculo.
    public async Task<int> AgregarLineaSalidaAsync(int reempaqueId, int palletId, int reempaqueDetalleId, int productoTerminadoId, int? cajas, decimal? kilogramos, decimal porcentajeMateriaSeca)
    {
        ValidarLinea(palletId, reempaqueDetalleId, productoTerminadoId, cajas, kilogramos);

        var anterior = await LeerSnapshotAsync(reempaqueId);

        var insertado = await _reempaqueRepository.InsertarLineaEnPalletAsync(palletId, reempaqueDetalleId, productoTerminadoId, cajas, kilogramos, porcentajeMateriaSeca);
        await _palletService.RecalcularGs1VoiceCodePalletAsync(palletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(reempaqueId));

        return insertado.Id;
    }

    // Pallet Neutro del reempaque: mismos productos SAP (MERMA/Diferencia a Favor) que el neutro de
    // producción normal — Kilogramos puede ser negativo, el signo lo resuelve quien llama según el
    // producto elegido. Un solo pallet neutro por folio de reempaque (ver sp_Reempaque_CrearNeutro).
    public async Task<int> CrearNeutroAsync(int reempaqueId, int reempaqueDetalleId, int productoTerminadoId, decimal kilogramos)
    {
        if (reempaqueDetalleId <= 0)
        {
            throw new ValidationException("Selecciona el lote a ajustar");
        }

        if (productoTerminadoId <= 0)
        {
            throw new ValidationException("Selecciona un producto terminado");
        }

        if (kilogramos == 0)
        {
            throw new ValidationException("Captura un monto de kilogramos distinto de cero");
        }

        var anterior = await LeerSnapshotAsync(reempaqueId);

        var palletNeutroId = await _reempaqueRepository.CrearNeutroAsync(reempaqueId, reempaqueDetalleId, productoTerminadoId, kilogramos);
        await _palletService.RecalcularGs1VoiceCodePalletAsync(palletNeutroId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(reempaqueId));

        return palletNeutroId;
    }

    // Quitar una línea de salida es una operación sobre el Pallet destino, no sobre el reempaque —
    // el SP (sp_PalletDetalle_Eliminar, ver 024_Alter_PalletDetalle_OrigenReempaque.sql) ya
    // ramifica por origen y devuelve el saldo a ReempaqueDetalle solo, sin necesidad de que este
    // servicio lo haga aparte.
    public Task EliminarLineaSalidaAsync(int palletId, int palletDetalleId)
        => _palletService.EliminarLineaAsync(palletId, palletDetalleId);

    // Cierre: exige saldo 0 en CADA lote (sin compensar entre ellos, ver Produccion.
    // sp_Reempaque_Cerrar) — si no cuadra, el SP lanza SqlRepositoryException con el detalle de
    // qué lotes faltan y este método no llega a auditar nada. Los pallets destino no se tocan: no
    // se bloquean al cerrar, siguen su vida normal en el módulo de Pallets.
    public async Task CerrarAsync(int id)
    {
        var anterior = await LeerSnapshotAsync(id);

        await _reempaqueRepository.CerrarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await LeerSnapshotAsync(id));
    }

    private async Task<SnapshotReempaque?> LeerSnapshotAsync(int id)
    {
        var encabezado = (await _reempaqueRepository.ObtenerAsync(id)).FirstOrDefault();
        if (encabezado is null)
        {
            return null;
        }

        var entrada = await _reempaqueRepository.ObtenerDetalleEntradaAsync(id);
        var salida = await _reempaqueRepository.ObtenerDetalleSalidaAsync(id);
        return new SnapshotReempaque(encabezado, entrada, salida);
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, SnapshotReempaque? anterior, SnapshotReempaque? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }

    private static void ValidarMotivo(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new ValidationException("Captura el motivo del reempaque");
        }
    }

    private static void ValidarLinea(int palletId, int reempaqueDetalleId, int productoTerminadoId, int? cajas, decimal? kilogramos)
    {
        if (palletId <= 0)
        {
            throw new ValidationException("Selecciona el pallet destino");
        }

        if (reempaqueDetalleId <= 0)
        {
            throw new ValidationException("Selecciona el lote de origen");
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

    private static ReempaqueDto MapearDto(Reempaque r) => new(
        r.Id, r.Folio, r.FechaCreacion, r.HoraCreacion, r.Motivo, r.Estatus,
        r.KilosAProcesar, r.KilosProcesados, r.Diferencia, r.FechaCierre, r.FechaCreacionRegistro);

    private static ReempaqueDetalleDto MapearDetalleDto(ReempaqueDetalle d) => new(
        d.Id, d.ReempaqueId, d.PalletOrigenId, d.PalletFolio, d.LoteId, d.LoteFolio, d.PorcentajeMateriaSeca,
        d.ProductoTerminadoOrigenId, d.ProductoDescripcion, d.CajasEntrada, d.KilosEntrada, d.KilosDisponibles);

    private sealed record SnapshotReempaque(Reempaque Encabezado, IReadOnlyList<ReempaqueDetalle> Entrada, IReadOnlyList<ReempaqueSalidaFilaDto> Salida);
}
