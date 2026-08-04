using System.Net.Http.Json;
using NutriInsights.Domain.BusquedaAlimentos;

namespace NutriInsights.Infrastructure.BusquedaAlimentos.OpenFoodFacts;

public sealed class BuscadorAlimentosOpenFoodFacts : IBuscadorAlimentosExternos
{
    private readonly HttpClient _httpClient;

    private static readonly string[] CamposSolicitados =
        ["code", "product_name", "brands", "nutriments"];

    public BuscadorAlimentosOpenFoodFacts(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CandidatoAlimentoExterno>> BuscarPorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var solicitud = new SolicitudBusquedaOpenFoodFactsDto
        {
            Q = nombre,
            Fields = CamposSolicitados
        };

        var respuestaHttp = await _httpClient.PostAsJsonAsync("search", solicitud, cancellationToken);
        respuestaHttp.EnsureSuccessStatusCode();

        var respuesta = await respuestaHttp.Content
            .ReadFromJsonAsync<RespuestaBusquedaOpenFoodFactsDto>(cancellationToken);

        return respuesta is null
            ? []
            : respuesta.Hits.Select(MapearACandidato).ToList();
    }

    private static CandidatoAlimentoExterno MapearACandidato(ProductoOpenFoodFactsDto producto) =>
        new(
            CodigoExterno: producto.Code,
            Nombre: producto.ProductName ?? "(sin nombre)",
            Marca: producto.Brands?.FirstOrDefault(),
            CaloriasPor100g: producto.Nutriments?.EnergiaKcal100g,
            ProteinaPor100g: producto.Nutriments?.Proteinas100g,
            CarbohidratosPor100g: producto.Nutriments?.Carbohidratos100g,
            GrasaPor100g: producto.Nutriments?.Grasa100g,
            FibraPor100g: producto.Nutriments?.Fibra100g);
}