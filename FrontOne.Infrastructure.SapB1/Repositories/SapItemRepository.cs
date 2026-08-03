using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SapB1.Models;
using FrontOne.Shared.Exceptions;

namespace FrontOne.Infrastructure.SapB1.Repositories;

public class SapItemRepository : ISapItemRepository
{
    private readonly ISapServiceLayerClient _client;

    public SapItemRepository(ISapServiceLayerClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<SapItemDto>> ObtenerPorPrefijoAsync(string prefijo, CancellationToken cancellationToken = default)
    {
        var items = new List<SapItemDto>();
        // MP-00110 y MP-00200 nunca llevan precio en Lista de Precio Fruta — se excluyen aquí para
        // que ningún módulo que consuma este repo tenga que filtrarlas aparte.
        string? endpoint = $"Items?$filter=startswith(ItemCode,'{prefijo}') and ItemCode ne 'MP-00110' and ItemCode ne 'MP-00200'" +
            "&$select=ItemCode,ItemName&$orderby=ItemCode";

        // SAP Service Layer pagina de a 20 filas por default — hay que seguir odata.nextLink
        // hasta que ya no venga, si no solo se trae la primera página.
        while (endpoint is not null)
        {
            var respuesta = await _client.GetAsync<SapItemsResponse>(endpoint, cancellationToken);
            if (respuesta is null)
            {
                break;
            }

            items.AddRange(respuesta.Value.Select(i => new SapItemDto(i.ItemCode, i.ItemName)));
            endpoint = respuesta.NextLink;
        }

        return items;
    }

    public async Task<IReadOnlyList<SapProductoTerminadoDto>> ObtenerPorGrupoAsync(string nombreGrupo, CancellationToken cancellationToken = default)
    {
        // Primero se resuelve el código numérico del grupo de artículos a partir de su nombre —
        // Items solo se puede filtrar por ItemsGroupCode (int), no por el nombre del grupo.
        var grupoEndpoint = $"ItemGroups?$filter=GroupName eq '{nombreGrupo}'&$select=Number";
        var grupoRespuesta = await _client.GetAsync<SapItemGroupsResponse>(grupoEndpoint, cancellationToken);
        var grupoCodigo = grupoRespuesta?.Value.FirstOrDefault()?.Number
            ?? throw new SapException($"No se encontró el grupo de artículos '{nombreGrupo}' en SAP.");

        var productos = new List<SapProductoTerminadoDto>();
        string? endpoint = $"Items?$filter=ItemsGroupCode eq {grupoCodigo}" +
            "&$select=ItemCode,ItemName,ForeignName,Valid&$orderby=ItemCode";

        // SAP Service Layer pagina de a 20 filas por default — hay que seguir odata.nextLink
        // hasta que ya no venga, si no solo se trae la primera página.
        while (endpoint is not null)
        {
            var respuesta = await _client.GetAsync<SapProductoTerminadoItemsResponse>(endpoint, cancellationToken);
            if (respuesta is null)
            {
                break;
            }

            // Valid llega como cadena "tYES"/"tNO" en vez de booleano — se mapea aquí a Activo.
            productos.AddRange(respuesta.Value.Select(i =>
                new SapProductoTerminadoDto(i.ItemCode, i.ItemName, i.ForeignName, i.Valid == "tYES")));
            endpoint = respuesta.NextLink;
        }

        return productos;
    }

    public async Task<IReadOnlyList<SapListaMaterialesDto>> ObtenerListaMaterialesAsync(string itemCode, CancellationToken cancellationToken = default)
    {
        // ProductTrees es un recurso singular (por clave), no una colección OData — no pagina.
        var respuesta = await _client.GetAsync<SapProductTreeResponse>($"ProductTrees('{itemCode}')", cancellationToken);
        if (respuesta is null)
        {
            throw new SapException($"No se encontró la lista de materiales del artículo '{itemCode}' en SAP.");
        }

        return respuesta.ProductTreeLines
            .Select(l => new SapListaMaterialesDto(l.ItemCode, l.ItemDescription, l.Quantity, l.UomCode, l.Warehouse))
            .ToList();
    }
}
