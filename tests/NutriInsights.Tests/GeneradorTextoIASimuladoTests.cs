using NutriInsights.Domain.GeneracionTexto;
using NutriInsights.Infrastructure.GeneracionTexto;

namespace NutriInsights.Tests;

public class GeneradorTextoIASimuladoTests
{
    [Fact]
    public async Task DevuelveTextoPlaceholderAlGenerarTexto()
    {
        IGeneradorTextoIA generador = new GeneradorTextoIASimulado();

        var resultado = await generador.GenerarTextoAsync("¿qué es la proteína?");

        Assert.Equal("[Respuesta de IA simulada, sin conectar aún]", resultado);
    }
}