namespace NutriInsights.Domain.MotorCalculo;

public static class CalculadoraObjetivos
{
    public static IEnumerable<EvaluacionObjetivo> EvaluarObjetivos(
        TotalesNutricionales totales,
        IEnumerable<Objetivo.Objetivo> objetivos)
    {
        foreach (var objetivo in objetivos)
        {
            if (objetivo.Tipo == Objetivo.Tipo.Variedad || objetivo.Valor is null)
            {
                continue;
            }

            var valorActual = objetivo.Nutriente switch
            {
                Objetivo.Nutriente.Calorias => totales.Calorias,
                Objetivo.Nutriente.Proteina => totales.Proteina,
                Objetivo.Nutriente.Carbohidratos => totales.Carbohidratos,
                Objetivo.Nutriente.Grasa => totales.Grasa,
                Objetivo.Nutriente.Fibra => totales.Fibra,
                _ => (decimal?)null
            };

            if (valorActual is null)
            {
                continue;
            }

            var cumplido = objetivo.Tipo == Objetivo.Tipo.Piso
                ? valorActual >= objetivo.Valor
                : valorActual <= objetivo.Valor;

            yield return new EvaluacionObjetivo(objetivo, cumplido);
        }
    }
}