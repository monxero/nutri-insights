using RegistroEntity = NutriInsights.Domain.Registro.Registro;
using AlimentoEntity = NutriInsights.Domain.Alimento.Alimento;
using UnidadMedidaEntity = NutriInsights.Domain.UnidadMedida.UnidadMedida;

namespace NutriInsights.Domain.ItemDeRegistro;

public class ItemDeRegistro
{
    public Guid Id { get; set; }

    public Guid RegistroId { get; set; }
    public RegistroEntity Registro { get; set; } = null!;

    public Guid? AlimentoId { get; set; }
    public AlimentoEntity? Alimento { get; set; }

    public string? DescripcionLibre { get; set; }

    public decimal? Cantidad { get; set; }

    public Guid? UnidadMedidaId { get; set; }
    public UnidadMedidaEntity? UnidadMedida { get; set; }

    public decimal? FraccionAplicada { get; set; }

    public NivelEstimacion NivelEstimacion { get; set; }

    public decimal? ValorCaloriasManual { get; set; }
    public decimal? CaloriasSnapshot { get; set; }
    public decimal? ProteinaSnapshot { get; set; }
    public decimal? CarbohidratosSnapshot { get; set; }
    public decimal? GrasaSnapshot { get; set; }
    public decimal? FibraSnapshot { get; set; }
}