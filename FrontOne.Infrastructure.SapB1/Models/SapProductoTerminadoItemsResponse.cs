using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// SAP Service Layer envuelve toda colección OData en "value" y pagina de a 20 filas por
// default — "odata.nextLink" trae el endpoint relativo de la siguiente página, o null si
// ya no hay más.
internal class SapProductoTerminadoItemsResponse
{
    [JsonPropertyName("value")]
    public List<SapProductoTerminadoItemRaw> Value { get; set; } = [];

    [JsonPropertyName("odata.nextLink")]
    public string? NextLink { get; set; }
}

internal class SapProductoTerminadoItemRaw
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("ItemName")]
    public string ItemName { get; set; } = string.Empty;

    [JsonPropertyName("ForeignName")]
    public string? ForeignName { get; set; }

    // Llega como cadena "tYES"/"tNO", no como booleano — se mapea en el repositorio.
    [JsonPropertyName("Valid")]
    public string Valid { get; set; } = "tNO";
}
