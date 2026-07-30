using CategoriaAlimentoEntity = NutriInsights.Domain.CategoriaAlimento.CategoriaAlimento;

namespace NutriInsights.Domain.CalificadorCantidad;

public class CalificadorCantidad
{
    public Guid Id { get; set; }
    public Guid CategoriaAlimentoId { get; set; }
    public CategoriaAlimentoEntity CategoriaAlimento { get; set; } = null!;
    public Calificador Calificador { get; set; }
    public decimal MinGramos { get; set; }
    public decimal MaxGramos { get; set; }
}