using Microsoft.EntityFrameworkCore;
using NutriInsights.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using NutriInsights.Importador;
using NutriInsights.Domain.CategoriaAlimento;
using AlimentoEntity = NutriInsights.Domain.Alimento.Alimento;
using OrigenEntity = NutriInsights.Domain.Alimento.Origen;
using NivelConfianzaEntity = NutriInsights.Domain.Alimento.NivelConfianza;

var configuracion = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var cadenaConexion = configuracion.GetConnectionString("DefaultConnection");

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

using var db = new ApplicationDbContext(
    new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(cadenaConexion).Options);

var nuevos = 0;
var actualizados = 0;

foreach (var alimentoUsda in archivo.FoundationFoods)
{
    if (alimentoUsda is null) { nulos++; continue; }

    var categoriaTexto = alimentoUsda.FoodCategory?.Description;
    if (categoriaTexto is null) { sinCategoria++; continue; }

    if (!mapeoCategoriasUsda.TryGetValue(categoriaTexto, out var categoriaId))
    {
        categoriaNoMapeada++;
        continue;
    }

    var codigoExterno = alimentoUsda.FdcId.ToString();
    var macros = ExtraerMacros(alimentoUsda);

    var alimentoExistente = db.Alimentos.FirstOrDefault(a =>
        a.Origen == OrigenEntity.TablaCurada && a.CodigoExterno == codigoExterno);

    if (alimentoExistente is not null)
    {
        alimentoExistente.CaloriasPor100g = macros.Calorias;
        alimentoExistente.ProteinaPor100g = macros.Proteina;
        alimentoExistente.CarbohidratosPor100g = macros.Carbohidratos;
        alimentoExistente.GrasaPor100g = macros.Grasa;
        alimentoExistente.FibraPor100g = macros.Fibra;
        actualizados++;
    }
    else
    {
        db.Alimentos.Add(new AlimentoEntity
        {
            Id = Guid.NewGuid(),
            Nombre = alimentoUsda.Description,
            CategoriaAlimentoId = categoriaId,
            Origen = OrigenEntity.TablaCurada,
            CodigoExterno = codigoExterno,
            CaloriasPor100g = macros.Calorias,
            ProteinaPor100g = macros.Proteina,
            CarbohidratosPor100g = macros.Carbohidratos,
            GrasaPor100g = macros.Grasa,
            FibraPor100g = macros.Fibra,
            NivelConfianza = NivelConfianzaEntity.BaseDatosReferencia,
        });
        nuevos++;
    }

    procesados++;
}

db.SaveChanges();

Console.WriteLine($"Nuevos: {nuevos}, Actualizados: {actualizados}");

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

Console.WriteLine($"Total en el JSON: {archivo.FoundationFoods.Count}");
Console.WriteLine($"Nulos: {nulos}");
Console.WriteLine($"Sin categoría: {sinCategoria}");
Console.WriteLine($"Categoría no mapeada (excluidos): {categoriaNoMapeada}");
Console.WriteLine($"Procesados (listos para guardar): {procesados}");

