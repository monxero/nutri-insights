namespace NutriInsights.Domain.Registro;

public class Registro
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public DateOnly Fecha { get; set; }
    public Comida? Comida { get; set; }
    public DateTime CreadoEn { get; set; }
}