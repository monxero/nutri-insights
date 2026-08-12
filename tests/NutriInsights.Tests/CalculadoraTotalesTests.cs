using NutriInsights.Domain.ItemDeRegistro;
using NutriInsights.Domain.MotorCalculo;

namespace NutriInsights.Tests;

public class CalculadoraTotalesTests
{
    [Fact]
    public void SumaCorrectamenteVariosItemsConDatosCompletos()
    {
        var items = new[]
        {
            new ItemDeRegistro { CaloriasSnapshot = 250m, ProteinaSnapshot = 20m, CarbohidratosSnapshot = 10m, GrasaSnapshot = 15m, FibraSnapshot = 2m },
            new ItemDeRegistro { CaloriasSnapshot = 150m, ProteinaSnapshot = 5m, CarbohidratosSnapshot = 30m, GrasaSnapshot = 1m, FibraSnapshot = 4m }
        };

        var totales = CalculadoraTotales.CalcularTotalesRegistro(items);

        Assert.Equal(400m, totales.Calorias);
        Assert.Equal(25m, totales.Proteina);
        Assert.False(totales.HayDatosFaltantes);
    }

    [Fact]
    public void SumaSoloCaloriasCuandoItemUsaValorCaloriasManual()
    {
        var items = new[]
        {
            new ItemDeRegistro { CaloriasSnapshot = 200m, ProteinaSnapshot = 15m, CarbohidratosSnapshot = 20m, GrasaSnapshot = 5m, FibraSnapshot = 3m },
            new ItemDeRegistro { ValorCaloriasManual = 400m }
        };

        var totales = CalculadoraTotales.CalcularTotalesRegistro(items);

        Assert.Equal(600m, totales.Calorias);
        Assert.Equal(15m, totales.Proteina);
        Assert.False(totales.HayDatosFaltantes);
    }

    [Fact]
    public void MarcaHayDatosFaltantesCuandoUnItemNoTieneNingunDato()
    {
        var items = new[]
        {
            new ItemDeRegistro { CaloriasSnapshot = 200m, ProteinaSnapshot = 15m },
            new ItemDeRegistro()
        };

        var totales = CalculadoraTotales.CalcularTotalesRegistro(items);

        Assert.Equal(200m, totales.Calorias);
        Assert.True(totales.HayDatosFaltantes);
    }
}