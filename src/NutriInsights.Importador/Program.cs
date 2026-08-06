using Microsoft.EntityFrameworkCore;
using NutriInsights.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using NutriInsights.Importador;

var opcionesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var textoJson = File.ReadAllText("../../tmp-importacion/FoodData_Central_foundation_food_json_2026-04-30.json");
var archivo = JsonSerializer.Deserialize<ArchivoUsda>(textoJson, opcionesJson)!;

var sandia = archivo.FoundationFoods.First(a => a is not null && a.FdcId == 2747676)!;

Console.WriteLine($"Nombre: {sandia.Description}");
Console.WriteLine($"Categoría: {sandia.FoodCategory?.Description}");
foreach (var nutriente in sandia.FoodNutrients)
{
    Console.WriteLine($"  {nutriente.Nutrient.Number} - {nutriente.Nutrient.Name}: {nutriente.Amount} {nutriente.Nutrient.UnitName}");
}