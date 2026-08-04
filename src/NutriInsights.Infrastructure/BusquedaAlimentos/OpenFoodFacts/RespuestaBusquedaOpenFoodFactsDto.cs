using System.Text.Json.Serialization;

namespace NutriInsights.Infrastructure.BusquedaAlimentos.OpenFoodFacts;

internal sealed class RespuestaBusquedaOpenFoodFactsDto
{
    [JsonPropertyName("hits")]
    public List<ProductoOpenFoodFactsDto> Hits { get; set; } = [];
}

internal sealed class ProductoOpenFoodFactsDto
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("product_name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("brands")]
    public List<string>? Brands { get; set; }

    [JsonPropertyName("nutriments")]
    public NutrientesOpenFoodFactsDto? Nutriments { get; set; }
}

internal sealed class NutrientesOpenFoodFactsDto
{
    [JsonPropertyName("energy-kcal_100g")]
    public decimal? EnergiaKcal100g { get; set; }

    [JsonPropertyName("proteins_100g")]
    public decimal? Proteinas100g { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public decimal? Carbohidratos100g { get; set; }

    [JsonPropertyName("fat_100g")]
    public decimal? Grasa100g { get; set; }

    [JsonPropertyName("fiber_100g")]
    public decimal? Fibra100g { get; set; }
}