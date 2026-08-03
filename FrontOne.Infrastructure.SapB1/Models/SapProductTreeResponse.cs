using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// Shape verificado en vivo contra el ambiente real de SAP (GET ProductTrees('PT-00007')) el
// 2026-08-01 — la descripción del componente viene en ItemName (no ItemDescription) y la
// Unidad de Medida en InventoryUOM (no UomCode). No cambiar sin volver a validar contra SAP.
internal class SapProductTreeResponse
{
    [JsonPropertyName("TreeCode")]
    public string TreeCode { get; set; } = string.Empty;

    [JsonPropertyName("ProductTreeLines")]
    public List<SapProductTreeLineRaw> ProductTreeLines { get; set; } = [];
}

internal class SapProductTreeLineRaw
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("ItemName")]
    public string? ItemName { get; set; }

    [JsonPropertyName("Quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("InventoryUOM")]
    public string? InventoryUOM { get; set; }

    [JsonPropertyName("Warehouse")]
    public string? Warehouse { get; set; }

    // Propiedades adicionales que SAP devuelve y no se usan (Price, IssueMethod, ChildNum, etc.)
    // se capturan aquí para no romper la deserialización.
    [JsonExtensionData]
    public Dictionary<string, object>? DatosAdicionales { get; set; }
}
