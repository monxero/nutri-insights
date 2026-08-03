using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NutriInsights.Infrastructure.Persistence;

namespace NutriInsights.Tests;

public class IdentityPasswordPolicyTests
{
    private static UserManager<ApplicationUser> CrearUserManager()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "identity-test-db"));

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<UserManager<ApplicationUser>>();
    }

    [Fact]
    public void ElUserManagerSeConstruyeCorrectamente()
    {
        var userManager = CrearUserManager();

        Assert.NotNull(userManager);
    }

    [Fact]
    public async Task RechazaContrasenaMasCortaQueElMinimo()
    {
        var userManager = CrearUserManager();
        var usuario = new ApplicationUser { UserName = "prueba@test.com", Email = "prueba@test.com" };

        var resultado = await userManager.CreateAsync(usuario, "Abc123!");

        Assert.False(resultado.Succeeded);
    }

    [Fact]
    public async Task AceptaContrasenaQueCumpleElMinimo()
    {
        var userManager = CrearUserManager();
        var usuario = new ApplicationUser { UserName = "valida@test.com", Email = "valida@test.com" };

        var resultado = await userManager.CreateAsync(usuario, "Abcdef1!");

        Assert.True(resultado.Succeeded);
    }
}