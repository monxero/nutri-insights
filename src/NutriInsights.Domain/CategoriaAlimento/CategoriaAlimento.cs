namespace NutriInsights.Domain.CategoriaAlimento;

public class CategoriaAlimento
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PorcionReferenciaGramos { get; set; }
}