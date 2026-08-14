namespace NutriInsights.Domain.InterpretacionMensajes;

public interface IInterpretadorMensajesIA
{
    Task<IReadOnlyList<AlimentoExtraido>> InterpretarAsync(
        string mensaje,
        DateOnly fechaReferencia,
        CancellationToken cancellationToken = default);
}