using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// SAP Service Layer envuelve toda colección OData en "value" y pagina de a 20 filas por
// default — "odata.nextLink" trae el endpoint relativo de la siguiente página, o null si
// ya no hay más.
internal class SapBusinessPartnersResponse
{
    [JsonPropertyName("value")]
    public List<SapBusinessPartnerRaw> Value { get; set; } = [];

    [JsonPropertyName("odata.nextLink")]
    public string? NextLink { get; set; }
}

internal class SapBusinessPartnerRaw
{
    [JsonPropertyName("CardCode")]
    public string CardCode { get; set; } = string.Empty;

    [JsonPropertyName("CardName")]
    public string CardName { get; set; } = string.Empty;
}
