using NutriInsights.Domain.Registro;

namespace NutriInsights.Domain.InterpretacionMensajes;

public record AlimentoExtraido(
    string DescripcionOriginal,
    decimal? Cantidad,
    string? Unidad,
    Comida? Comida,
    decimal? FraccionAplicada,
    DateOnly? Fecha
);