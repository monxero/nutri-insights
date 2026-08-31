using NutriInsights.Domain.GeneracionTexto;

namespace NutriInsights.Infrastructure.GeneracionTexto;

/// <summary>
/// Implementación de prueba de IGeneradorTextoIA, sin conexión real a ningún
/// proveedor de IA. Existe para demostrar la separación de ADR-001/012: el resto de
/// la aplicación depende de la interfaz, no de esta clase. Reemplazar por la
/// implementación real de Gemini en Etapa 6.
/// </summary>
public class GeneradorTextoIASimulado : IGeneradorTextoIA
{
    public Task<string> GenerarTextoAsync(
        string mensaje,
        string? contexto = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult("[Respuesta de IA simulada, sin conectar aún]");
    }
}