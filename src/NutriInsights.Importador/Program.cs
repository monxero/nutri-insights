using Microsoft.EntityFrameworkCore;
using NutriInsights.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;


var configuracion = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var cadenaConexion = configuracion.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(cadenaConexion))
{
    Console.WriteLine("No se encontró la cadena de conexión.");
}
else
{
    Console.WriteLine($"Cadena de conexión encontrada, largo: {cadenaConexion.Length} caracteres.");
}

var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseNpgsql(cadenaConexion)
    .Options;

using var db = new ApplicationDbContext(opciones);

var cantidadCategorias = db.CategoriasAlimento.Count();
Console.WriteLine($"Categorías encontradas en la base: {cantidadCategorias}");