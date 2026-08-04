using System.Text.Json.Serialization;

namespace NutriInsights.Infrastructure.BusquedaAlimentos.OpenFoodFacts;

internal sealed class SolicitudBusquedaOpenFoodFactsDto
{
    [JsonPropertyName("q")]
    public required string Q { get; set; }

    [JsonPropertyName("langs")]
    public string[] Langs { get; set; } = ["en"];

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = 10;

    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;

    [JsonPropertyName("fields")]
    public string[]? Fields { get; set; }

    [JsonPropertyName("index_id")]
    public string IndexId { get; set; } = "off";
}