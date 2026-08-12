using NutriInsights.Domain.MotorCalculo;
using NutriInsights.Domain.Objetivo;

namespace NutriInsights.Tests;

public class CalculadoraObjetivosTests
{
    private static readonly TotalesNutricionales Totales =
        new(Calorias: 2000m, Proteina: 90m, Carbohidratos: 200m, Grasa: 60m, Fibra: 25m, HayDatosFaltantes: false);

    [Fact]
    public void MarcaPisoComoCumplidoCuandoElTotalLoAlcanzaOSupera()
    {
        var objetivo = new Objetivo { Nutriente = Nutriente.Proteina, Tipo = Tipo.Piso, Valor = 80m };

        var resultado = CalculadoraObjetivos.EvaluarObjetivos(Totales, [objetivo]).Single();

        Assert.True(resultado.Cumplido);
    }

    [Fact]
    public void MarcaPisoComoNoCumplidoCuandoElTotalNoLoAlcanza()
    {
        var objetivo = new Objetivo { Nutriente = Nutriente.Proteina, Tipo = Tipo.Piso, Valor = 120m };

        var resultado = CalculadoraObjetivos.EvaluarObjetivos(Totales, [objetivo]).Single();

        Assert.False(resultado.Cumplido);
    }

    [Fact]
    public void MarcaTechoComoCumplidoCuandoElTotalNoLoSupera()
    {
        var objetivo = new Objetivo { Nutriente = Nutriente.Calorias, Tipo = Tipo.Techo, Valor = 2200m };

        var resultado = CalculadoraObjetivos.EvaluarObjetivos(Totales, [objetivo]).Single();

        Assert.True(resultado.Cumplido);
    }

    [Fact]
    public void MarcaTechoComoNoCumplidoCuandoElTotalLoSupera()
    {
        var objetivo = new Objetivo { Nutriente = Nutriente.Calorias, Tipo = Tipo.Techo, Valor = 1800m };

        var resultado = CalculadoraObjetivos.EvaluarObjetivos(Totales, [objetivo]).Single();

        Assert.False(resultado.Cumplido);
    }

    [Fact]
    public void IgnoraObjetivosDeTipoVariedad()
    {
        var objetivo = new Objetivo { Nutriente = Nutriente.Variedad, Tipo = Tipo.Variedad, Valor = null };

        var resultado = CalculadoraObjetivos.EvaluarObjetivos(Totales, [objetivo]);

        Assert.Empty(resultado);
    }
}