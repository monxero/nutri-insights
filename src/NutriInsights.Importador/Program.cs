using Microsoft.EntityFrameworkCore;
using NutriInsights.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using NutriInsights.Importador;
using NutriInsights.Domain.CategoriaAlimento;

var opcionesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var textoJson = File.ReadAllText("../../tmp-importacion/FoodData_Central_foundation_food_json_2026-04-30.json");
var archivo = JsonSerializer.Deserialize<ArchivoUsda>(textoJson, opcionesJson)!;

var mapeoCategoriasUsda = new Dictionary<string, Guid>
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

var procesados = 0;
var nulos = 0;
var sinCategoria = 0;
var categoriaNoMapeada = 0;

foreach (var alimento in archivo.FoundationFoods)
{
    if (alimento is null)
    {
        nulos++;
        continue;
    }

    var categoriaTexto = alimento.FoodCategory?.Description;
    if (categoriaTexto is null)
    {
        sinCategoria++;
        continue;
    }

    if (!mapeoCategoriasUsda.TryGetValue(categoriaTexto, out var categoriaId))
    {
        categoriaNoMapeada++;
        continue;
    }

    procesados++;
}

Console.WriteLine($"Total en el JSON: {archivo.FoundationFoods.Count}");
Console.WriteLine($"Nulos: {nulos}");
Console.WriteLine($"Sin categoría: {sinCategoria}");
Console.WriteLine($"Categoría no mapeada (excluidos): {categoriaNoMapeada}");
Console.WriteLine($"Procesados (listos para guardar): {procesados}");

