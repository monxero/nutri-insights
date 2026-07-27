# Documento 01 — El problema

**Versión:** 0.2 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Antes de definir una solución es necesario comprender si el problema realmente existe y cuál es su magnitud. Este documento recopila evidencia proveniente de literatura científica, reportes de la industria y experiencias de usuarios, con el objetivo de caracterizar el problema que enfrentan actualmente las aplicaciones de nutrición y hábitos alimentarios — sin todavía proponer cómo resolverlo. Las decisiones de diseño derivadas de esta evidencia se presentan en los documentos posteriores.

## 1. El problema en cifras

El abandono de aplicaciones de nutrición y hábitos saludables no es una excepción — es el comportamiento predominante. Una revisión de estudios sobre abandono de aplicaciones de salud encontró que en la mayoría de los estudios revisados, más de la mitad de los usuarios abandonaban la aplicación dentro de los primeros 100 días, con una tasa mediana de abandono cercana al 70%, cifra consistente con reportes de la industria que ubican el abandono de apps de fitness y nutrición en torno al 69% dentro de los primeros 90 días. Un estudio específico sobre apps de dieta encontró una tasa de abandono del 86%.

Otras fuentes de la industria son igual de contundentes: las tasas de retención de apps de dieta y nutrición caen a aproximadamente 30% después del primer mes, y cerca del 70% de los usuarios abandona dentro de las primeras dos semanas cuando la aplicación resulta demasiado compleja o consume demasiado tiempo.

No es un problema marginal — es el patrón dominante de la categoría completa de aplicaciones a la que pertenece este proyecto.

## 2. La principal causa identificada: la fricción del registro manual

La causa que aparece de forma más consistente entre las fuentes no es falta de interés del usuario — es la carga que impone el registro mismo. Un análisis de la industria describe cómo aplicaciones como MyFitnessPal experimentan picos de uso en enero seguidos de fatiga significativa a medida que el ingreso manual de datos se vuelve una carga.

Hay evidencia más reciente que refuerza directamente esta causa: herramientas de conteo de calorías con entrada asistida por IA (como registro por foto) muestran aproximadamente el doble de retención a 30 días frente a las herramientas de entrada manual tradicionales, con el registro por foto tomando 3-5 segundos frente a 45-90 segundos de búsqueda y entrada manual en una base de datos, según un estudio de interacción humano-computadora de 2024 citado por la fuente. Aunque esta cifra proviene del blog de un producto competidor y debe tomarse con cautela por su naturaleza promocional, la dirección del hallazgo es consistente con el resto de la evidencia: la fricción de entrada es la variable que más se correlaciona con el abandono, no la falta de interés en mejorar la alimentación.

## 3. Lo que dicen los propios usuarios

Buscando en foros y comunidades de estas aplicaciones aparecen dos patrones recurrentes que ilustran cómo este problema se manifiesta en la experiencia cotidiana de los usuarios.

**El registro exhaustivo se vuelve una carga desproporcionada.** En un foro de usuarios de aplicaciones de conteo de calorías, una persona describe el hábito de registrar de forma extremadamente meticulosa —anotando el agua, el peso diario, cada comida en gramos— como una actividad casi compulsiva, similar a estar constantemente revisando redes sociales. En otro foro, un usuario relata cómo su pareja registra cada bocado, incluso una fracción de una barra de chocolate, calificando el comportamiento como rayano en lo excesivo.

**Cuando el usuario internaliza el conocimiento, dejar de registrar en detalle no es necesariamente un fracaso.** En un hilo de discusión, un usuario relata que registraba todo religiosamente al principio, pero que tras varios meses de cambio de hábitos, ya casi no necesita usar la aplicación porque su alimentación se volvió lo bastante estable como para saber de memoria cuántos carbohidratos o calorías aporta cada comida. Este patrón resulta especialmente interesante porque muestra que algunos usuarios dejan de necesitar un registro detallado una vez que internalizan determinados conocimientos sobre su alimentación.

## 4. Síntesis

La evidencia presentada muestra un patrón consistente entre estudios académicos, reportes de la industria y experiencias de usuarios:

- El abandono temprano constituye el comportamiento predominante en las aplicaciones de nutrición.
- La principal causa identificada no es la falta de motivación inicial, sino la fricción asociada al registro continuo de información.
- Cuando los usuarios desarrollan conocimiento suficiente sobre sus propios hábitos alimentarios, la necesidad de registrar cada comida disminuye de manera natural.

Estos hallazgos definen el problema que este proyecto pretende abordar. Las decisiones de diseño derivadas de esta evidencia se presentan en los documentos posteriores.

## Fuentes

- Journal of Medical Internet Research (2024) — revisión de abandono de apps de salud y estilo de vida: https://www.jmir.org/2024/1/e56897
- Diet and Nutrition Apps Statistics and Facts (2026), media.market.us: https://media.market.us/diet-and-nutrition-apps-statistics/
- Digital Yield Group — Health & Fitness Apps Churn: https://digitalyieldgroup.com/blog/health-fitness-apps-the-resolutioner-churn-problem/
- Nutrola — estadísticas de apps de conteo de calorías (2026), fuente promocional, usada con cautela: https://nutrola.app/en/blog/how-many-people-use-calorie-tracking-apps-2026-global-statistics
- Foro de Cronometer y foro de AnandTech — testimonios de usuarios (citados de forma anónima y parafraseada)