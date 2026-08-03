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
        // Si el artículo no tiene lista de materiales capturada en SAP, Service Layer responde
        // 404 (ODBC -2028 "No matching records found") — no es un error real, es un estado
        // válido (producto sin carta técnica todavía), así que se devuelve vacío en vez de
        // propagar la excepción con el JSON crudo de SAP.
        SapProductTreeResponse? respuesta;
        try
        {
            respuesta = await _client.GetAsync<SapProductTreeResponse>($"ProductTrees('{itemCode}')", cancellationToken);
        }
        catch (SapException ex) when (ex.HttpStatusCode == 404)
        {
            return [];
        }

        if (respuesta is null || respuesta.ProductTreeLines.Count == 0)
        {
            return [];
        }

        var unidadesPorComponente = await ObtenerUnidadesDeMedidaAsync(respuesta.ProductTreeLines, cancellationToken);

        return respuesta.ProductTreeLines
            .Select(l => new SapListaMaterialesDto(
                l.ItemCode,
                l.ItemName,
                l.Quantity,
                unidadesPorComponente.GetValueOrDefault(l.ItemCode),
                l.Warehouse))
            .ToList();
    }

    // La línea del BOM nunca trae la Unidad de Medida (InventoryUOM siempre null ahí) — se
    // resuelve consultando el maestro de cada artículo componente, en paralelo (uno por
    // ItemCode único, normalmente menos de 15 componentes por producto terminado).
    private async Task<Dictionary<string, string?>> ObtenerUnidadesDeMedidaAsync(
        IEnumerable<SapProductTreeLineRaw> lineas, CancellationToken cancellationToken)
    {
        var codigosUnicos = lineas.Select(l => l.ItemCode).Distinct().ToList();

        var tareas = codigosUnicos.Select(async codigo =>
        {
            try
            {
                var item = await _client.GetAsync<SapItemUomResponse>(
                    $"Items('{codigo}')?$select=ItemCode,InventoryUOM", cancellationToken);
                return (Codigo: codigo, Uom: item?.InventoryUOM);
            }
            catch (SapException)
            {
                // Si un componente puntual falla la consulta, no se tumba toda la carta —
                // esa línea simplemente queda sin Unidad de Medida.
                return (Codigo: codigo, Uom: (string?)null);
            }
        });

        var resultados = await Task.WhenAll(tareas);
        return resultados.ToDictionary(r => r.Codigo, r => r.Uom);
    }
}
