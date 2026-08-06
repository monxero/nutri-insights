namespace NutriInsights.Importador;

public record ArchivoUsda(List<AlimentoUsdaDto?> FoundationFoods);

public record AlimentoUsdaDto(
    long FdcId,
    string Description,
    FoodCategoryDto? FoodCategory,
    List<FoodNutrientDto> FoodNutrients);

public record FoodCategoryDto(string Description);

public record FoodNutrientDto(NutrientDto Nutrient, decimal? Amount);

public record NutrientDto(string Number, string Name, string UnitName);