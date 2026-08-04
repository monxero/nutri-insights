using System.Net;
using System.Text;
using NutriInsights.Infrastructure.BusquedaAlimentos.OpenFoodFacts;


namespace NutriInsights.Tests;

internal sealed class HttpMessageHandlerFalso : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly string _cuerpoJson;

    public HttpMessageHandlerFalso(string cuerpoJson, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _cuerpoJson = cuerpoJson;
        _statusCode = statusCode;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var respuesta = new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent(_cuerpoJson, Encoding.UTF8, "application/json")
        };

        return Task.FromResult(respuesta);
    }
}

public class BuscadorAlimentosOpenFoodFactsTests
{
    private static BuscadorAlimentosOpenFoodFacts CrearBuscador(string cuerpoJsonFalso)
    {
        var handlerFalso = new HttpMessageHandlerFalso(cuerpoJsonFalso);
        var httpClient = new HttpClient(handlerFalso)
        {
            BaseAddress = new Uri("https://search.openfoodfacts.org/")
        };

        return new BuscadorAlimentosOpenFoodFacts(httpClient);
    }

    [Fact]
    public async Task MapeaUnProductoConTodosLosNutrientesPresentes()
    {
        const string jsonFalso = """
        {
          "hits": [
            {
              "code": "0009800800049",
              "product_name": "Nutella & go! hazelnut spread + breadsticks",
              "brands": ["Nutella"],
              "nutriments": {
                "energy-kcal_100g": 519.23,
                "proteins_100g": 7.69,
                "carbohydrates_100g": 63.46,
                "fat_100g": 25,
                "fiber_100g": 3.8
              }
            }
          ]
        }
        """;

        var buscador = CrearBuscador(jsonFalso);

        var resultado = await buscador.BuscarPorNombreAsync("nutella");

        var candidato = Assert.Single(resultado);
        Assert.Equal("0009800800049", candidato.CodigoExterno);
        Assert.Equal("Nutella & go! hazelnut spread + breadsticks", candidato.Nombre);
        Assert.Equal("Nutella", candidato.Marca);
        Assert.Equal(519.23m, candidato.CaloriasPor100g);
        Assert.Equal(3.8m, candidato.FibraPor100g);
    }

    [Fact]
    public async Task MapeaUnProductoSinNutrimentsAValoresNulos()
    {
        const string jsonFalso = """
        {
          "hits": [
            {
              "code": "12214181",
              "product_name": "nutella"
            }
          ]
        }
        """;

        var buscador = CrearBuscador(jsonFalso);

        var resultado = await buscador.BuscarPorNombreAsync("nutella");

        var candidato = Assert.Single(resultado);
        Assert.Equal("12214181", candidato.CodigoExterno);
        Assert.Null(candidato.Marca);
        Assert.Null(candidato.CaloriasPor100g);
        Assert.Null(candidato.FibraPor100g);
    }
}