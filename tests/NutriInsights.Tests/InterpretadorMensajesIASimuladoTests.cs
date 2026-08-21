using NutriInsights.Domain.InterpretacionMensajes;
using NutriInsights.Infrastructure.InterpretacionMensajes;

namespace NutriInsights.Tests;

public class InterpretadorMensajesIASimuladoTests
{
    [Fact]
    public async Task DevuelveListaVaciaAlInterpretarCualquierMensaje()
    {
        IInterpretadorMensajesIA interpretador = new InterpretadorMensajesIASimulado();

        var resultado = await interpretador.InterpretarAsync(
            "comí dos huevos",
            new DateOnly(2026, 8, 21));

        Assert.Empty(resultado);
    }
}