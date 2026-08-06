using NutriInsights.Domain.CategoriaAlimento;

namespace NutriInsights.Importador;

public static class MapeadorCategoriasUsda
{
    private static readonly Dictionary<string, Guid> Mapeo = new()
    {
        ["Vegetables and Vegetable Products"] = CategoriasAlimentoSemilla.VerdurasId,
        ["Fruits and Fruit Juices"] = CategoriasAlimentoSemilla.FrutasId,
        ["Dairy and Egg Products"] = CategoriasAlimentoSemilla.LacteosId,
        ["Cereal Grains and Pasta"] = CategoriasAlimentoSemilla.CarbohidratosId,
        ["Baked Products"] = CategoriasAlimentoSemilla.CarbohidratosId,
        ["Legumes and Legume Products"] = CategoriasAlimentoSemilla.ProteinaVegetalId,
        ["Finfish and Shellfish Products"] = CategoriasAlimentoSemilla.ProteinaAnimalId,
        ["Beef Products"] = CategoriasAlimentoSemilla.ProteinaAnimalId,
        ["Poultry Products"] = CategoriasAlimentoSemilla.ProteinaAnimalId,
        ["Pork Products"] = CategoriasAlimentoSemilla.ProteinaAnimalId,
        ["Sausages and Luncheon Meats"] = CategoriasAlimentoSemilla.ProteinaAnimalId,
        ["Lamb, Veal, and Game Products"] = CategoriasAlimentoSemilla.ProteinaAnimalId,
        ["Nut and Seed Products"] = CategoriasAlimentoSemilla.GrasasYFrutosSecosId,
        ["Fats and Oils"] = CategoriasAlimentoSemilla.GrasasYFrutosSecosId,
        ["Restaurant Foods"] = CategoriasAlimentoSemilla.OtrosProcesadosId,
        ["Soups, Sauces, and Gravies"] = CategoriasAlimentoSemilla.OtrosProcesadosId,
        ["Sweets"] = CategoriasAlimentoSemilla.OtrosProcesadosId,
        ["Beverages"] = CategoriasAlimentoSemilla.OtrosProcesadosId,
    };

    public static bool TryMapear(string categoriaUsda, out Guid categoriaId) =>
        Mapeo.TryGetValue(categoriaUsda, out categoriaId);
}