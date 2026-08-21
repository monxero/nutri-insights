using NutriInsights.Domain.InterpretacionMensajes;

namespace NutriInsights.Infrastructure.InterpretacionMensajes;

/// <summary>
/// Implementación de prueba de IInterpretadorMensajesIA, sin conexión real a ningún
/// proveedor de IA. Existe para demostrar la separación de ADR-001/012: el resto de
/// la aplicación depende de la interfaz, no de esta clase. Reemplazar por la
/// implementación real de Gemini en Etapa 6.
/// </summary>
public class InterpretadorMensajesIASimulado : IInterpretadorMensajesIA
{
    public Task<IReadOnlyList<AlimentoExtraido>> InterpretarAsync(
        string mensaje,
        DateOnly fechaReferencia,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<AlimentoExtraido>>(
            Array.Empty<AlimentoExtraido>());
    }
}