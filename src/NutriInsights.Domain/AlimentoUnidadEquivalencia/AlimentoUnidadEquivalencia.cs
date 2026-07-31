using AlimentoEntity = NutriInsights.Domain.Alimento.Alimento;
using UnidadMedidaEntity = NutriInsights.Domain.UnidadMedida.UnidadMedida;

namespace NutriInsights.Domain.AlimentoUnidadEquivalencia;

public class AlimentoUnidadEquivalencia
{
    public Guid Id { get; set; }
    public Guid AlimentoId { get; set; }
    public AlimentoEntity Alimento { get; set; } = null!;
    public Guid UnidadMedidaId { get; set; }
    public UnidadMedidaEntity UnidadMedida { get; set; } = null!;
    public decimal EquivalenteEnGramos { get; set; }

}