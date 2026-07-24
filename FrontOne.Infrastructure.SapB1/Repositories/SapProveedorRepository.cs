using FrontOne.Domain.DTOs;
using FrontOne.Domain.Interfaces;
using FrontOne.Infrastructure.SapB1.Models;

namespace FrontOne.Infrastructure.SapB1.Repositories;

public class SapProveedorRepository : ISapProveedorRepository
{
    private readonly ISapServiceLayerClient _client;

    public SapProveedorRepository(ISapServiceLayerClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<SapProveedorDto>> ObtenerPorGrupoAsync(int grupoCodigo, CancellationToken cancellationToken = default)
    {
        var proveedores = new List<SapProveedorDto>();
        // Solo proveedores (CardType 'cSupplier') vigentes (Valid 'tYES') del grupo indicado —
        // deja fuera los congelados/dados de baja en SAP.
        string? endpoint = $"BusinessPartners?$filter=GroupCode eq {grupoCodigo} and CardType eq 'cSupplier' and Valid eq 'tYES'" +
            "&$select=CardCode,CardName&$orderby=CardName";

        // SAP Service Layer pagina de a 20 filas por default — hay que seguir odata.nextLink
        // hasta que ya no venga, si no solo se trae la primera página.
        while (endpoint is not null)
        {
            var respuesta = await _client.GetAsync<SapBusinessPartnersResponse>(endpoint, cancellationToken);
            if (respuesta is null)
            {
                break;
            }

            proveedores.AddRange(respuesta.Value.Select(p => new SapProveedorDto(p.CardCode, p.CardName)));
            endpoint = respuesta.NextLink;
        }

        return proveedores;
    }
}
