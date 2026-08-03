using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// SAP Service Layer envuelve toda colección OData en "value" — se usa solo para resolver el
// código numérico (Number) de un grupo de artículos a partir de su nombre (GroupName).
internal class SapItemGroupsResponse
{
    [JsonPropertyName("value")]
    public List<SapItemGroupRaw> Value { get; set; } = [];
}

internal class SapItemGroupRaw
{
    [JsonPropertyName("Number")]
    public int Number { get; set; }
}
