using CategoriaAlimentoEntity = NutriInsights.Domain.CategoriaAlimento.CategoriaAlimento;
using OrigenEntity = NutriInsights.Domain.Alimento.Origen;
using NivelConfianzaEntity = NutriInsights.Domain.Alimento.NivelConfianza;

namespace NutriInsights.Domain.Alimento;

public class Alimento
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public Guid CategoriaAlimentoId { get; set; }
    public CategoriaAlimentoEntity CategoriaAlimento { get; set; } = null!;
    public OrigenEntity Origen { get; set; }
    public Guid? UsuarioPropietarioId { get; set; }
    public string? CodigoExterno { get; set; }
    public decimal? CaloriasPor100g { get; set; } 
    public decimal? ProteinaPor100g { get; set; } 
    public decimal? CarbohidratosPor100g { get; set; } 
    public decimal? GrasaPor100g { get; set; } 
    public decimal? FibraPor100g { get; set; } 
    public NivelConfianzaEntity NivelConfianza { get; set; }
}