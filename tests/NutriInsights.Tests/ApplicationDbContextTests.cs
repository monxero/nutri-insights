using Microsoft.EntityFrameworkCore;
using NutriInsights.Infrastructure.Persistence;

namespace NutriInsights.Tests;

public class ApplicationDbContextTests
{
    [Fact]
    public void SeConstruyeConTodasLasEntidadesDelDominio()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "test-db")
            .Options;

        using var context = new ApplicationDbContext(options);

        Assert.NotNull(context.Model);
    }
}