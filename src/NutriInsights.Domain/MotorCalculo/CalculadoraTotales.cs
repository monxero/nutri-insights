using ItemDeRegistroEntity = NutriInsights.Domain.ItemDeRegistro.ItemDeRegistro;

namespace NutriInsights.Domain.MotorCalculo;

public static class CalculadoraTotales
{
    public static TotalesNutricionales CalcularTotalesRegistro(IEnumerable<ItemDeRegistroEntity> items)
    {
        decimal calorias = 0m;
        decimal proteina = 0m;
        decimal carbohidratos = 0m;
        decimal grasa = 0m;
        decimal fibra = 0m;
        var hayDatosFaltantes = false;

        foreach (var item in items)
        {
            if (item.CaloriasSnapshot is null && item.ValorCaloriasManual is null)
            {
                hayDatosFaltantes = true;
                continue;
            }

            calorias += item.CaloriasSnapshot ?? item.ValorCaloriasManual ?? 0m;
            proteina += item.ProteinaSnapshot ?? 0m;
            carbohidratos += item.CarbohidratosSnapshot ?? 0m;
            grasa += item.GrasaSnapshot ?? 0m;
            fibra += item.FibraSnapshot ?? 0m;
        }

        return new TotalesNutricionales(calorias, proteina, carbohidratos, grasa, fibra, hayDatosFaltantes);
    }
}