using NutriInsights.Domain.MotorCalculo;

namespace NutriInsights.Tests;

public class CalculadoraProteinaTests
{
    [Fact]
    public void CalculaRangoDeMantencionCuandoNoHayDeficit()
    {
        var (minimo, maximo) = CalculadoraProteina.CalcularRangoProteina(
            pesoKg: 70m,
            enDeficitCalorico: false);

        Assert.Equal(98m, minimo);
        Assert.Equal(140m, maximo);
    }

    [Fact]
    public void CalculaRangoMasAltoCuandoHayDeficitCalorico()
    {
        var (minimo, maximo) = CalculadoraProteina.CalcularRangoProteina(
            pesoKg: 70m,
            enDeficitCalorico: true);

        Assert.Equal(161m, minimo);
        Assert.Equal(217m, maximo);
    }
}