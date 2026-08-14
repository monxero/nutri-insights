namespace NutriInsights.Domain.MotorCalculo;

public static class CalculadoraProteina
{
    public static (decimal minimo, decimal maximo) CalcularRangoProteina(
        decimal pesoKg,
        bool enDeficitCalorico)
    {
        if (enDeficitCalorico)
        {
            return (2.3m * pesoKg, 3.1m * pesoKg);
        }

        return (1.4m * pesoKg, 2.0m * pesoKg);
    }
}