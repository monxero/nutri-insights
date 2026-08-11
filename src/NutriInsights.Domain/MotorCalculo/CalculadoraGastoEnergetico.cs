using NutriInsights.Domain.Usuario;

namespace NutriInsights.Domain.MotorCalculo;

public static class CalculadoraGastoEnergetico
{

    public static decimal CalcularGastoEnergeticoTotal(
        decimal pesoKg,
        decimal estaturaCm,
        int edadAnios,
        Sexo sexo,
        NivelActividad nivelActividad)
    {
        var constanteSexo = sexo switch
        {
            Sexo.Masculino => 5m,
            Sexo.Femenino => -161m,
            _ => throw new ArgumentOutOfRangeException(nameof(sexo))
        };

        var tasaMetabolicaBasal = (10m * pesoKg) + (6.25m * estaturaCm) - (5m * edadAnios) + constanteSexo;

        var factorActividad = nivelActividad switch
        {
            NivelActividad.Sedentario => 1.2m,
            NivelActividad.Ligero => 1.375m,
            NivelActividad.Moderado => 1.55m,
            NivelActividad.Activo => 1.725m,
            NivelActividad.MuyActivo => 1.9m,
            _ => throw new ArgumentOutOfRangeException(nameof(nivelActividad))
        };

        return tasaMetabolicaBasal * factorActividad;
    }

}