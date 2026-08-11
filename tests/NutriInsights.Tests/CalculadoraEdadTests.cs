using NutriInsights.Domain.MotorCalculo;

namespace NutriInsights.Tests;

public class CalculadoraEdadTests
{
    [Fact]
    public void CalculaEdadCorrectaCuandoYaPasoElCumpleanosEsteAnio()
    {
        var fechaNacimiento = new DateOnly(1990, 3, 15);
        var fechaActual = new DateOnly(2026, 8, 11);

        var edad = CalculadoraEdad.CalcularEdad(fechaNacimiento, fechaActual);

        Assert.Equal(36, edad);
    }

    [Fact]
    public void CalculaEdadCorrectaCuandoAunNoLlegaElCumpleanosEsteAnio()
    {
        var fechaNacimiento = new DateOnly(1990, 12, 15);
        var fechaActual = new DateOnly(2026, 8, 11);

        var edad = CalculadoraEdad.CalcularEdad(fechaNacimiento, fechaActual);

        Assert.Equal(35, edad);
    }

    [Fact]
    public void CalculaEdadCorrectaCuandoEsExactamenteElDiaDelCumpleanos()
    {
        var fechaNacimiento = new DateOnly(1990, 8, 11);
        var fechaActual = new DateOnly(2026, 8, 11);

        var edad = CalculadoraEdad.CalcularEdad(fechaNacimiento, fechaActual);

        Assert.Equal(36, edad);
    }
}