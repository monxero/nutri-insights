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
var macros = ExtraerMacros(sandia);
Console.WriteLine($"Calorías: {macros.Calorias}, Proteína: {macros.Proteina}, Carbohidratos: {macros.Carbohidratos}, Grasa: {macros.Grasa}, Fibra: {macros.Fibra}");

static (decimal? Calorias, decimal? Proteina, decimal? Carbohidratos, decimal? Grasa, decimal? Fibra) ExtraerMacros(AlimentoUsdaDto alimento)
{
    decimal? BuscarPorNumero(params string[] numeros) =>
        alimento.FoodNutrients.FirstOrDefault(n => numeros.Contains(n.Nutrient.Number))?.Amount;

    return (
        Calorias: BuscarPorNumero("208", "957"),
        Proteina: BuscarPorNumero("203"),
        Carbohidratos: BuscarPorNumero("205"),
        Grasa: BuscarPorNumero("204"),
        Fibra: BuscarPorNumero("291")
    );
}