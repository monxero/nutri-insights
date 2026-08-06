namespace NutriInsights.Domain.BusquedaAlimentos;

public record CandidatoAlimentoExterno(
    string CodigoExterno,
    string Nombre,
    string? Marca,
    decimal? CaloriasPor100g,
    decimal? ProteinaPor100g,
    decimal? CarbohidratosPor100g,
    decimal? GrasaPor100g,
    decimal? FibraPor100g);