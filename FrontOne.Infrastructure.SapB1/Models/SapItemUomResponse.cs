using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// La Unidad de Medida no viene en la línea del ProductTree (siempre null ahí) — hay que
// resolverla consultando el maestro del artículo componente (Items). Verificado en vivo el
// 2026-08-01: Items('INI-00230').InventoryUOM sí trae el nombre real ("PIEZA").
internal class SapItemUomResponse
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("InventoryUOM")]
    public string? InventoryUOM { get; set; }
}
