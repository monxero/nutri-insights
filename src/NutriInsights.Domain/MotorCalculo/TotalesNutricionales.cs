namespace NutriInsights.Domain.MotorCalculo;

public record TotalesNutricionales(
    decimal Calorias,
    decimal Proteina,
    decimal Carbohidratos,
    decimal Grasa,
    decimal Fibra,
    bool HayDatosFaltantes);