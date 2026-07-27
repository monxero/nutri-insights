# Documento 02 — Estado del arte

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Este documento analiza qué ofrecen las aplicaciones de nutrición existentes, qué hacen bien, y dónde se quedan cortas, organizadas en dos categorías porque el panorama competitivo cambió significativamente en el último año: la categoría establecida de registro manual, y una categoría emergente de registro asistido por IA que no existía o era marginal hasta hace poco. Para facilitar la comparación, cada categoría se analiza considerando cuatro dimensiones comunes: facilidad de registro, exactitud de las estimaciones, transparencia respecto a la incertidumbre, y confiabilidad del proceso de generación de resultados.

## 1. Categoría 1 — Apps de registro manual establecidas

**MyFitnessPal, Cronometer, MacroFactor, Yazio, Lose It!**

Todas comparten el mismo flujo de trabajo fundamental: buscar el alimento en una base de datos, seleccionarlo, ajustar la porción, confirmar. Ninguna de las tres apps más comparadas de forma independiente (MyFitnessPal, Cronometer, MacroFactor) ofrece reconocimiento por foto como método principal de registro — todas dependen de búsqueda manual, escaneo de código de barras, o entrada de alimentos personalizados. El tiempo de registro medido independientemente ronda 35-45 segundos por comida.

Lo que hacen bien, cada una en su especialidad:
- **MyFitnessPal:** la base de datos más grande (más de 14 millones de entradas), fuerte en encontrar productos específicos y comida de restaurantes — a costa de exactitud, con una variación medida de forma independiente de aproximadamente 6.8%, la más alta de las tres.
- **Cronometer:** la mayor profundidad de micronutrientes (84 nutrientes por entrada) con datos verificados contra bases oficiales (USDA, NCCDB) y la mayor exactitud medida (~3.5% de variación).
- **MacroFactor:** un algoritmo de TDEE adaptativo que ajusta el objetivo calórico según la tendencia real de peso del usuario, en vez de un objetivo fijo calculado una sola vez.

Lo que ninguna resuelve:
- **Ninguna comunica incertidumbre.** Cada estimación se presenta como un número único y definitivo, nunca como un rango con nivel de confianza — a pesar de que la propia industria reconoce variaciones de 3.5% a 6.8% entre ellas.
- **El costo de mantenimiento es alto y constante.** El flujo de búsqueda-selección-ajuste-confirmación no se reduce con el tiempo ni se adapta a un usuario que ya aprendió sus propios hábitos — el usuario experimentado paga la misma fricción que el usuario nuevo.
- **Ninguna distingue entre un registro impreciso y uno preciso de forma explícita para el usuario** — todo el peso de la precisión recae en el esfuerzo manual de quien registra.

## 2. Categoría 2 — Apps de registro asistido por IA (categoría emergente)

**Cal AI, Foodvisor, Nutrola, Welling, Fuel, PlateLens, SnapCalorie, entre otras**

Esta categoría reduce de forma significativa la fricción asociada al registro manual, descrita previamente en el documento 01: fotografiar una comida y recibir una estimación toma segundos, no los 35-45 segundos de una app de registro manual. Welling incorpora además un asistente conversacional basado en IA capaz de responder preguntas relacionadas con objetivos nutricionales, diferenciándose de otras aplicaciones centradas exclusivamente en el reconocimiento de alimentos.

Sin embargo, la evidencia de exactitud real —medida de forma independiente, no según el marketing de cada app— muestra limitaciones que todavía no presentan una solución consistente entre las aplicaciones evaluadas:

- Un benchmark de seis apps evaluadas con las mismas fotos estandarizadas encontró un error porcentual medio (MAPE) que va desde 1.4% en el mejor caso hasta 19.8% en el peor, con los flujos de trabajo manuales tradicionales (Cronometer, MacroFactor) superando como grupo a las apps de solo-foto, salvo una excepción notable.
- Algunas apps muestran sesgos sistemáticos, no solo error aleatorio: una subestima de forma crónica, otra sobreestima de forma crónica en platos mixtos — errores direccionales que no se cancelan entre sí con el uso repetido, a diferencia de un error simétrico.
- Una fuente que evalúa esta categoría señala explícitamente que los mejores sistemas deberían mostrar niveles de confianza en sus estimaciones y permitir corregirlas con facilidad — una recomendación que, al presentarse como aspiración, sugiere que la mayoría de las apps evaluadas todavía no lo hace.

**Sobre el riesgo de alucinación:** esta categoría depende más del modelo de lenguaje para generar respuestas, y eso trae consigo un riesgo real. Un caso documentado describe a un usuario de una app de nutrición con IA notando que la aplicación "se confundía" en ocasiones (lo que la fuente llama alucinación), mientras una nutricionista consultada advertía que estos errores pueden tener consecuencias reales de salud, en particular para condiciones como la diabetes. Una fuente independiente sobre arquitectura de este tipo de herramientas señala que separar el cálculo determinista de la generación libre del modelo de lenguaje es un mecanismo reconocido para evitar este problema.

## 3. Fuentes de datos utilizadas por el ecosistema

OpenFoodFacts es la fuente de datos abierta y colaborativa más usada actualmente por aplicaciones de esta categoría, aunque no la única posible — bases como USDA FoodData Central o bases nacionales de composición de alimentos cumplen un rol equivalente en otros contextos. OpenFoodFacts no es una aplicación de consumo, es infraestructura: su fortaleza es la cobertura de productos envasados con código de barras; su límite es la cobertura débil de alimentos genéricos y crudos, al no estar centrada en productos de marca.

## 4. Tabla comparativa

| Característica | Registro manual | Registro por IA |
|---|---|---|
| Fricción | Alta | Baja |
| Exactitud | Alta y estable | Variable según modelo |
| Transparencia de incertidumbre | Baja | Baja |
| Dependencia del esfuerzo del usuario | Alta | Baja |
| Transparencia sobre el proceso de cálculo | Alta (trazable a una entrada específica de base de datos) | Baja (estimación tipo "caja negra", generalmente sin explicación del proceso) |

## 5. Aspectos aún no resueltos por el estado del arte

La revisión realizada muestra que las aplicaciones recientes incorporan avances importantes, especialmente en la reducción de la fricción mediante IA. Sin embargo, todavía persisten aspectos donde no existe una solución claramente consolidada entre las aplicaciones analizadas:

- La comunicación explícita de la incertidumbre de las estimaciones.
- La diferenciación entre errores aleatorios y sesgos sistemáticos.
- La transparencia sobre los límites del modelo utilizado.
- El equilibrio entre facilidad de uso y confiabilidad de los resultados.

Estos aspectos representan áreas abiertas dentro del estado actual de la categoría y constituyen oportunidades para futuras propuestas de diseño.

## Fuentes

- Comparaciones independientes MyFitnessPal / Cronometer / MacroFactor (2026): intakenutrition.io, aifithub.io, calorie-trackers.com, nutriscan.app
- Benchmark de seis apps de reconocimiento por foto (2026): clinicalnutritionreport.com
- Casos y arquitecturas de apps de IA nutricional: welling.ai, fuelnutrition.app, nutrola.app, promealplan.com
- Caso de alucinación documentado: today.com (NBC)
- OpenFoodFacts: openfoodfacts.org