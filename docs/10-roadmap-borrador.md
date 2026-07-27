# Documento 10 — Roadmap

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Este documento organiza en versiones todo lo que quedó fuera del MVP (documento 03) a lo largo del proceso — no son promesas de fecha, son una secuencia razonable de prioridad. El criterio de agrupación: v2 son mejoras de bajo costo que reducen fricción sobre lo ya construido; v3 son capacidades nuevas de plataforma o modalidad, de mayor esfuerzo; el resto son ideas exploratorias sin compromiso.

## v1 — MVP

Definido completo en el documento 03. No se repite aquí.

## v2 — Reducir más fricción sobre lo ya construido

| Ítem | Por qué va aquí antes que en v3 |
|---|---|
| Comidas frecuentes guardadas explícitamente por el usuario, para que "lo de siempre" funcione de forma segura | Extiende el catálogo personal ya construido (RF-17) — no requiere infraestructura nueva, solo una capa de reconocimiento adicional |
| Calibración de estimaciones (diccionario de calificadores, categorías de nivel 2/3) con datos de uso real | Depende de tener usuarios reales generando datos — no se puede hacer antes del MVP, pero es la mejora más directa a la precisión del sistema una vez que existan |
| Corrección conversacional de registros fuera del hilo activo (más allá de RF-06) | El CRUD de pantalla ya cubre el caso base; esto es una mejora de comodidad, no una capacidad nueva |
| Consultas comparativas sobre el historial (ej. "¿cuánto pescado comí la semana pasada vs. carne?") | Reutiliza el motor de agregación ya construido para el panel día/semana/mes, con una consulta más flexible |

## v3 — Nueva plataforma o modalidad de entrada

| Ítem | Por qué va aquí y no en v2 |
|---|---|
| Registro por foto de comida | Requiere entrada multimodal (imagen + texto) en la integración de IA — extiende, pero no reutiliza directamente, la arquitectura de structured output actual (documento 06). Evidencia externa (documento 01) sugiere que es la mejora individual con más impacto potencial en retención. |
| App nativa instalable (Android/iOS) vía .NET MAUI Blazor Hybrid | Camino técnico ya confirmado (documento 06), pero es un esfuerzo de desarrollo paralelo real, no una extensión incremental de la app web |
| Gráficos y visualizaciones de tendencia más elaboradas en el panel | El panel simple del MVP cubre la necesidad básica; esto es una mejora de presentación, no de capacidad |

## Ideas exploratorias, sin compromiso de fecha ni de prioridad

- Detección de patrones de alimentación más sofisticados, más allá del conteo de variedad por categoría.
- Calibración del diccionario de calificadores por usuario individual, en vez de uno fijo compartido.
- Roles de usuario adicionales y proveedores de login externos (Google, etc.) — sin caso de uso identificado todavía.
- Exportar el historial completo (XML/JSON) — sin necesidad real de producto identificada; candidata a implementarse como pieza de portafolio puntual si se decide más adelante, conectando con el conocimiento de XML de la oferta laboral.

---

**Nota de mantenimiento:** cuando un ítem de este roadmap se implemente, se retira de aquí y su decisión de diseño correspondiente pasa al documento 11 (ADR) si fue una decisión arquitectónica relevante.