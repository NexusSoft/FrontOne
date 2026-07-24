using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SapB1.Models;

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
}
