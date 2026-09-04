using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SapB1.Models;

namespace FrontOne.Infrastructure.SapB1.Repositories;

public class SapPedidoRepository : ISapPedidoRepository
{
    private readonly ISapServiceLayerClient _client;

    public SapPedidoRepository(ISapServiceLayerClient client)
    {
        _client = client;
    }

    public Task<IReadOnlyList<SapPedidoDto>> ObtenerTop500Async(CancellationToken cancellationToken = default)
        => ObtenerAsync(500, filtroEstatus: null, cancellationToken);

    // TOP 100 abiertos — buscador embebido del Contenedor (mismo criterio TOP 100 del resto del proyecto).
    public Task<IReadOnlyList<SapPedidoDto>> ObtenerAbiertosAsync(CancellationToken cancellationToken = default)
        => ObtenerAsync(100, filtroEstatus: "bost_Open", cancellationToken);

    private async Task<IReadOnlyList<SapPedidoDto>> ObtenerAsync(int top, string? filtroEstatus, CancellationToken cancellationToken)
    {
        // $select sin DocumentLines — el listado no necesita las líneas, y traerlas aquí
        // multiplicaría el peso de la respuesta sin usarlas.
        var pedidos = new List<SapPedidoDto>();
        var filtro = filtroEstatus is null ? string.Empty : $"&$filter=DocumentStatus eq '{filtroEstatus}'";
        string? endpoint = "Orders?$select=DocEntry,DocNum,CardCode,CardName,DocDate,DocDueDate,DocTotal,DocCurrency,DocumentStatus,Comments,U_FolioFronterra" +
            $"&$orderby=DocEntry desc&$top={top}{filtro}";

        // SAP Service Layer pagina de a 20 filas por default — hay que seguir odata.nextLink
        // hasta completar el $top pedido o hasta que ya no venga.
        while (endpoint is not null && pedidos.Count < top)
        {
            var respuesta = await _client.GetAsync<SapOrdersResponse>(endpoint, cancellationToken);
            if (respuesta is null)
            {
                break;
            }

            pedidos.AddRange(respuesta.Value.Select(o => new SapPedidoDto(
                o.DocEntry, o.DocNum, o.CardCode, o.CardName, o.DocDate, o.DocDueDate,
                o.DocTotal, o.DocCurrency, TraducirEstatus(o.DocumentStatus), o.Comments, o.FolioFronterra)));
            endpoint = respuesta.NextLink;
        }

        return pedidos;
    }

    public async Task<SapPedidoDetalleDto?> ObtenerPorDocEntryAsync(int docEntry, CancellationToken cancellationToken = default)
    {
        var pedido = await _client.GetAsync<SapOrderRaw>($"Orders({docEntry})", cancellationToken);
        if (pedido is null)
        {
            return null;
        }

        var lineas = pedido.DocumentLines
            .Select(l => new SapPedidoLineaDto(l.ItemCode, l.ItemDescription, l.Quantity, l.Price, l.LineTotal, l.WarehouseCode))
            .ToList();

        return new SapPedidoDetalleDto(
            pedido.DocEntry, pedido.DocNum, pedido.CardCode, pedido.CardName, pedido.NumAtCard,
            pedido.DocDate, pedido.DocDueDate, pedido.TaxDate, pedido.DocCurrency, pedido.DocRate,
            pedido.DocTotal, pedido.VatSum, pedido.DiscountPercent, TraducirEstatus(pedido.DocumentStatus),
            pedido.Comments, pedido.Address, pedido.SalesPersonCode?.ToString(), pedido.FolioFronterra, lineas);
    }

    private static string TraducirEstatus(string documentStatus) => documentStatus switch
    {
        "bost_Open" => "Abierto",
        "bost_Close" => "Cerrado",
        "bost_Cancel" => "Cancelado",
        "bost_Delivered" => "Entregado",
        _ => documentStatus,
    };
}
