using NutriInsights.Domain.CategoriaAlimento;
using NutriInsights.Importador;
using Xunit;

namespace NutriInsights.Tests;

public class MapeadorCategoriasUsdaTests
{
    [Fact]
    public void Mapea_categoria_conocida_a_su_guid_correspondiente()
    {
        var resultado = MapeadorCategoriasUsda.TryMapear("Fruits and Fruit Juices", out var categoriaId);

        Assert.True(resultado);
        Assert.Equal(CategoriasAlimentoSemilla.FrutasId, categoriaId);
    }

    [Fact]
    public void No_mapea_categoria_excluida_a_proposito()
    {
        var resultado = MapeadorCategoriasUsda.TryMapear("Spices and Herbs", out _);

        Assert.False(resultado);
    }
}