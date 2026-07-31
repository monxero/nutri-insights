namespace NutriInsights.Domain.Objetivo;

public class Objetivo
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Nutriente Nutriente { get; set; }
    public Tipo Tipo { get; set; }
    public decimal? Valor { get; set; }
}