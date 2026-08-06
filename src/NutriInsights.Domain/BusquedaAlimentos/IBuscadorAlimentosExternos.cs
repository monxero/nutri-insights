namespace NutriInsights.Domain.BusquedaAlimentos;

public interface IBuscadorAlimentosExternos
{
    Task<IReadOnlyList<CandidatoAlimentoExterno>> BuscarPorNombreAsync(
        string nombre,
        CancellationToken cancellationToken = default);
}