using System.Text.Json.Serialization;

namespace FrontOne.Infrastructure.SapB1.Models;

// SAP Service Layer envuelve toda colección OData en "value" y pagina de a 20 filas por
// default — "odata.nextLink" trae el endpoint relativo de la siguiente página, o null si
// ya no hay más.
internal class SapOrdersResponse
{
    [JsonPropertyName("value")]
    public List<SapOrderRaw> Value { get; set; } = [];

    [JsonPropertyName("odata.nextLink")]
    public string? NextLink { get; set; }
}

internal class SapOrderRaw
{
    [JsonPropertyName("DocEntry")]
    public int DocEntry { get; set; }

    [JsonPropertyName("DocNum")]
    public int DocNum { get; set; }

    [JsonPropertyName("CardCode")]
    public string CardCode { get; set; } = string.Empty;

    [JsonPropertyName("CardName")]
    public string CardName { get; set; } = string.Empty;

    [JsonPropertyName("NumAtCard")]
    public string? NumAtCard { get; set; }

    [JsonPropertyName("DocDate")]
    public DateTime DocDate { get; set; }

    [JsonPropertyName("DocDueDate")]
    public DateTime DocDueDate { get; set; }

    [JsonPropertyName("TaxDate")]
    public DateTime? TaxDate { get; set; }

    [JsonPropertyName("DocCurrency")]
    public string DocCurrency { get; set; } = string.Empty;

    [JsonPropertyName("DocRate")]
    public decimal DocRate { get; set; }

    [JsonPropertyName("DocTotal")]
    public decimal DocTotal { get; set; }

    [JsonPropertyName("VatSum")]
    public decimal VatSum { get; set; }

    [JsonPropertyName("DiscountPercent")]
    public decimal DiscountPercent { get; set; }

    [JsonPropertyName("DocumentStatus")]
    public string DocumentStatus { get; set; } = string.Empty;

    [JsonPropertyName("Comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("Address")]
    public string? Address { get; set; }

    [JsonPropertyName("SalesPersonCode")]
    public int? SalesPersonCode { get; set; }

    // Campo de usuario (UDF) capturado en SAP sobre el encabezado del pedido.
    [JsonPropertyName("U_FolioFronterra")]
    public string? FolioFronterra { get; set; }

    [JsonPropertyName("DocumentLines")]
    public List<SapOrderLineRaw> DocumentLines { get; set; } = [];
}

internal class SapOrderLineRaw
{
    [JsonPropertyName("ItemCode")]
    public string ItemCode { get; set; } = string.Empty;

    [JsonPropertyName("ItemDescription")]
    public string ItemDescription { get; set; } = string.Empty;

    [JsonPropertyName("Quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("Price")]
    public decimal Price { get; set; }

    [JsonPropertyName("LineTotal")]
    public decimal LineTotal { get; set; }

    [JsonPropertyName("WarehouseCode")]
    public string? WarehouseCode { get; set; }
}
