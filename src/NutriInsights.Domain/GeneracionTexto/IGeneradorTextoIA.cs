namespace NutriInsights.Domain.GeneracionTexto;

public interface IGeneradorTextoIA
{
    Task<string> GenerarTextoAsync(
        string mensaje,
        string? contexto = null,
        CancellationToken cancellationToken = default);
}