using NutriInsights.Domain.MotorCalculo;
using NutriInsights.Domain.Usuario;

namespace NutriInsights.Tests;

public class CalculadoraGastoEnergeticoTests
{
    [Fact]
    public void CalculaGastoEnergeticoCorrectoParaHombreModeradamenteActivo()
    {
        var resultado = CalculadoraGastoEnergetico.CalcularGastoEnergeticoTotal(
            pesoKg: 70m,
            estaturaCm: 175m,
            edadAnios: 30,
            sexo: Sexo.Masculino,
            nivelActividad: NivelActividad.Moderado);

        Assert.Equal(2555.5625m, resultado);
    }

    [Fact]
    public void CalculaGastoEnergeticoCorrectoParaMujerSedentaria()
    {
        var resultado = CalculadoraGastoEnergetico.CalcularGastoEnergeticoTotal(
            pesoKg: 60m,
            estaturaCm: 165m,
            edadAnios: 25,
            sexo: Sexo.Femenino,
            nivelActividad: NivelActividad.Sedentario);

        Assert.Equal(1614.3m, resultado);
    }
}