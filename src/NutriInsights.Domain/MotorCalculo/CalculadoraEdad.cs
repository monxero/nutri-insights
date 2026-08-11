namespace NutriInsights.Domain.MotorCalculo;

public static class CalculadoraEdad
{
    public static int CalcularEdad(DateOnly fechaNacimiento, DateOnly fechaActual)
    {
        var edad = fechaActual.Year - fechaNacimiento.Year;

        if (fechaActual < fechaNacimiento.AddYears(edad))
        {
            edad--;
        }

        return edad;
    }
}