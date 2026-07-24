using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// SAP Service Layer envuelve toda colección OData en "value" y pagina de a 20 filas por
// default — "odata.nextLink" trae el endpoint relativo de la siguiente página, o null si
// ya no hay más.
internal class SapItemsResponse
{
    [JsonPropertyName("value")]
    public List<SapItemRaw> Value { get; set; } = [];

    [JsonPropertyName("odata.nextLink")]
    public string? NextLink { get; set; }
}

internal class SapItemRaw
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("ItemName")]
    public string ItemName { get; set; } = string.Empty;
}
