using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// Shape de ProductTreeLines no validado contra el ambiente real de SAP — ajustar nombres de
// propiedades si Service Layer devuelve un contrato distinto. Se documentan aquí los nombres
// estándar publicados en la documentación de SAP B1 Service Layer para el recurso ProductTrees.
internal class SapProductTreeResponse
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("ProductTreeLines")]
    public List<SapProductTreeLineRaw> ProductTreeLines { get; set; } = [];
}

internal class SapProductTreeLineRaw
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("ItemDescription")]
    public string? ItemDescription { get; set; }

    [JsonPropertyName("Quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("UomCode")]
    public string? UomCode { get; set; }

    [JsonPropertyName("Warehouse")]
    public string? Warehouse { get; set; }

    // Propiedades adicionales que SAP pudiera devolver y que no están mapeadas explícitamente
    // (ej. UoMEntry, IssueMethod) se capturan aquí para no romper la deserialización.
    [JsonExtensionData]
    public Dictionary<string, object>? DatosAdicionales { get; set; }
}
