using System.Text.Json;
using FrontOne.Domain.DTOs;
using FrontOne.Domain.Enums;
using FrontOne.Domain.Interfaces;
using FrontOne.Shared.Exceptions;
using FrontOne.Shared.Security;

namespace FrontOne.Application.Services;

// Contenedor surte un Pedido de venta ABIERTO en SAP con pallets ya armados en Producción. El
// pedido nunca se edita aquí (solo lectura de SAP, ver PedidoService/contexto/embarques.md) —
// este servicio solo agrupa pallets bajo un folio propio y, al hacerlo, marca cada pallet como
// Embarcado (Estatus 8) para sacarlo del circuito de Reempaques (ver 026_Alter_Pallet_Embarcado.sql).
public class ContenedorService
{
    private const string Modulo = "Embarques";

    private readonly IContenedorRepository _contenedorRepository;
    private readonly ISapPedidoRepository _sapPedidoRepository;
    private readonly ProductoTerminadoService _productoTerminadoService;
    private readonly AuditService _auditService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ContenedorService(
        IContenedorRepository contenedorRepository,
        ISapPedidoRepository sapPedidoRepository,
        ProductoTerminadoService productoTerminadoService,
        AuditService auditService,
        ICurrentUserProvider currentUserProvider)
    {
        _contenedorRepository = contenedorRepository;
        _sapPedidoRepository = sapPedidoRepository;
        _productoTerminadoService = productoTerminadoService;
        _auditService = auditService;
        _currentUserProvider = currentUserProvider;
    }

    public Task<IReadOnlyList<ContenedorDto>> ObtenerAsync()
        => _contenedorRepository.ObtenerAsync();

    public async Task<ContenedorDto?> ObtenerPorIdAsync(int id)
        => (await _contenedorRepository.ObtenerAsync(id)).FirstOrDefault();

    public Task<IReadOnlyList<ContenedorPalletDto>> ObtenerPalletsAsync(int contenedorId)
        => _contenedorRepository.ObtenerPalletsAsync(contenedorId);

    public Task<IReadOnlyList<ContenedorResumenCalibreDto>> ObtenerResumenAsync(int contenedorId)
        => _contenedorRepository.ObtenerResumenAsync(contenedorId);

    public Task<IReadOnlyList<PalletDisponibleEmbarqueDto>> ObtenerPalletsDisponiblesAsync(string? folio, IReadOnlyList<string> codigosSapPermitidos)
        => _contenedorRepository.ObtenerPalletsDisponiblesAsync(folio, string.Join(",", codigosSapPermitidos));

    public Task<IReadOnlyList<SapPedidoDto>> ObtenerPedidosAbiertosAsync(CancellationToken cancellationToken = default)
        => _sapPedidoRepository.ObtenerAbiertosAsync(cancellationToken);

    // Combina las líneas del pedido en SAP con lo ya surtido en pallets: Pallet (conversión
    // teórica Cajas/CajasPorPallet), Kilogramos (real, ya embarcado) y Status Pendiente/Surtido.
    public async Task<IReadOnlyList<ContenedorPedidoLineaDto>> ObtenerLineasPedidoAsync(int contenedorId, int docEntry, CancellationToken cancellationToken = default)
    {
        var pedido = await _sapPedidoRepository.ObtenerPorDocEntryAsync(docEntry, cancellationToken);
        if (pedido is null)
        {
            return Array.Empty<ContenedorPedidoLineaDto>();
        }

        var surtido = (await _contenedorRepository.ObtenerSurtidoAsync(contenedorId)).ToDictionary(s => s.CodigoSap);
        var productos = (await _productoTerminadoService.ObtenerAsync()).ToDictionary(p => p.CodigoSap);

        return pedido.Lineas.Select(linea =>
        {
            productos.TryGetValue(linea.Codigo, out var producto);
            surtido.TryGetValue(linea.Codigo, out var surtidoLinea);

            var cajasSurtidas = surtidoLinea?.CajasSurtidas ?? 0;
            var pallet = producto?.CajasPorPallet is > 0 ? linea.Cantidad / producto.CajasPorPallet.Value : (decimal?)null;
            var presentacion = producto?.Presentacion.ToString() ?? string.Empty;
            var porcentaje = linea.Cantidad > 0 ? cajasSurtidas / linea.Cantidad * 100m : 0m;
            var status = cajasSurtidas >= linea.Cantidad ? "Surtido" : "Pendiente";

            return new ContenedorPedidoLineaDto(
                linea.Codigo, linea.Descripcion, linea.Cantidad, presentacion, pallet,
                surtidoLinea?.KilogramosSurtidos ?? 0m, porcentaje, status);
        }).ToList();
    }

    public async Task<int> CrearAsync(DateTime fecha, SapPedidoDto pedido, string? observaciones)
    {
        if (pedido is null)
        {
            throw new ValidationException("Selecciona el pedido de SAP a surtir");
        }

        var id = await _contenedorRepository.InsertarAsync(
            fecha, pedido.DocEntry, pedido.DocNum, pedido.FolioFronterra, pedido.CardCode, pedido.CardName, observaciones);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Crear, null, await ObtenerPorIdAsync(id));

        return id;
    }

    public async Task ActualizarAsync(int id, DateTime fecha, string? observaciones)
    {
        var anterior = await ObtenerPorIdAsync(id);

        await _contenedorRepository.ActualizarAsync(id, fecha, observaciones);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await ObtenerPorIdAsync(id));
    }

    public async Task EliminarAsync(int id)
    {
        var anterior = await ObtenerPorIdAsync(id);

        await _contenedorRepository.EliminarAsync(id);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Eliminar, anterior, null);
    }

    public async Task AgregarPalletAsync(int contenedorId, int palletId, int posicion, decimal? temperatura)
    {
        if (palletId <= 0)
        {
            throw new ValidationException("Selecciona un pallet");
        }

        if (posicion <= 0)
        {
            throw new ValidationException("Captura la posición del pallet en el contenedor");
        }

        var anterior = await ObtenerPorIdAsync(contenedorId);

        await _contenedorRepository.AgregarPalletAsync(contenedorId, palletId, posicion, temperatura);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await ObtenerPorIdAsync(contenedorId));
    }

    public async Task QuitarPalletAsync(int contenedorId, int contenedorPalletId)
    {
        var anterior = await ObtenerPorIdAsync(contenedorId);

        await _contenedorRepository.QuitarPalletAsync(contenedorPalletId);

        await RegistrarAuditoriaAsync(TipoAccionAuditoria.Modificar, anterior, await ObtenerPorIdAsync(contenedorId));
    }

    private Task RegistrarAuditoriaAsync(TipoAccionAuditoria accion, ContenedorDto? anterior, ContenedorDto? nuevo)
    {
        var usuario = _currentUserProvider.NombreUsuario ?? "desconocido";
        var valoresAnteriores = anterior is null ? null : JsonSerializer.Serialize(anterior);
        var valoresNuevos = nuevo is null ? null : JsonSerializer.Serialize(nuevo);

        return _auditService.RegistrarAsync(usuario, accion, Modulo, valoresAnteriores, valoresNuevos);
    }
}
