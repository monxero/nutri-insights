# Documento 04 — Requerimientos funcionales

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Este documento formaliza, como requerimientos verificables, las capacidades declaradas en el documento 03 (Alcance MVP). Cada requerimiento describe **qué** debe hacer el sistema, no cómo se implementa — el "cómo" pertenece a los documentos 06/07/08/09. Cuando existe un caso de prueba ya validado en `09-anexo-dialogos-ejemplo.md`, se referencia como criterio de aceptación.

## A. Registro conversacional de comidas

| ID | Requerimiento | Criterio de aceptación |
|---|---|---|
| RF-01 | El sistema debe permitir registrar comidas mediante lenguaje natural, para el día actual o cualquier día pasado, incluyendo mensajes que mezclan varios días o varias comidas por día. | Diálogos 16, 16b |
| RF-02 | El sistema debe tolerar errores de escritura y mezcla de idiomas sin fallar la extracción. | Diálogos 30, 31 |
| RF-03 | La etiqueta de comida (desayuno/almuerzo/cena/colación) debe ser opcional en cada registro, nunca obligatoria. | Diálogo 16b |
| RF-04 | El sistema debe poder registrar y responder una consulta dentro de un mismo mensaje. | Diálogo 23 |
| RF-05 | El sistema debe distinguir entre agregar un registro nuevo y corregir el último registro, según las señales de lenguaje del mensaje ("se me olvidó decir que...", "también..." = agregar; "en realidad eran...", "perdón, era..." = corregir). | Diálogos 25, 26, 13 |
| RF-06 | La corrección conversacional solo aplica dentro del hilo activo de la conversación (últimos mensajes). Corregir un registro de días atrás se hace desde la pantalla de edición (ver RF-32), no por chat. | Diálogo 13b |
| RF-07 | Cuando el usuario declara explícitamente que no recuerda una comida, el sistema debe aceptar el vacío sin insistir — no dispara pregunta de refinamiento ni autoestimación. | Diálogo 16b |
| RF-08 | Ante un valor fuera de todo rango humano plausible, el sistema debe señalarlo y, cuando exista una corrección obvia y probable, sugerirla directamente. | Diálogos 29, y el caso base "40kg de pollo" |
| RF-09 | El sistema debe detectar un mensaje idéntico recibido en una ventana de tiempo corta y preguntar si corresponde a un registro adicional o a un envío duplicado. | Diálogo 24 |
| RF-10 | El sistema debe resolver a qué día pertenece un registro según la regla de "día de referencia": por defecto hoy; si el usuario nombra otro día, ese día se mantiene mientras el flujo sea continuo; se resetea (o se pregunta) ante un tema ajeno de por medio, al volver a "hoy", o al iniciar una sesión en un día real distinto. | Diálogo 32 |
| RF-11 | Cada registro debe confirmarse de inmediato (nunca bloqueando a la espera de aprobación), con el nivel de detalle de la confirmación escalado según la complejidad del mensaje. | Diálogos 19 (mínimo) y 16b (detallado) |
| RF-12 | El sistema debe estimar un alimento según una jerarquía de cuatro niveles: (1) ingrediente identificado con precisión, (2) componente genérico reconocible pero no exacto, (3) plato no identificable estimado por tipo de comida, (4) información insuficiente incluso para el nivel 3 → se pide al usuario un valor autoestimado. Un dato de etiqueta de producto envasado (nivel 1) reduce el rango considerablemente pero conserva un margen de incertidumbre pequeño, no cero. | Casos de nivel 1-4 documentados en `CONTEXTO.md` |
| RF-13 | Ante ambigüedad de cantidad, el sistema debe hacer como máximo una pregunta de refinamiento con ancla concreta (ej. "¿como 100g?"), solo si la respuesta cambiaría significativamente el rango calculado. | Diálogo 2 |
| RF-14 | Si el usuario no da más detalle tras la pregunta de refinamiento, el sistema usa como respaldo un diccionario de calificadores de cantidad por categoría de alimento (no absoluto ni por alimento individual), con piso mínimo no-cero. | Diálogo 3 |
| RF-15 | El sistema no debe preguntar cuando la ambigüedad no cambia significativamente el resultado (ej. variedad de una fruta, una pizca de condimento). | Diálogos 19, 20 |
| RF-16 | El usuario debe poder indicar que comió una fracción (mitad, un cuarto, etc.) de un plato completo — propio o compartido con otra persona — y el sistema aplica esa fracción sobre la estimación total del plato, no sobre un ingrediente individual. | Diálogo 33 |
| RF-17 | Cuando un alimento o plato no existe en ninguna fuente de datos —incluyendo un producto envasado que no aparece en la fuente externa—, el sistema puede ofrecer agregarlo al catálogo personal del usuario mediante preguntas que llenen la información faltante para estimar sus macronutrientes. Si el usuario entrega toda la información de una vez (escrita directamente, pegada desde otra fuente, o leída de una etiqueta), el sistema no hace preguntas de relleno innecesarias — confirma y guarda directamente. Si la descripción corresponde a una receta de varias porciones (ej. "para 6 personas"), el sistema pregunta qué cantidad consumió el usuario respecto al total, para no atribuir toda la receta a un solo registro. El alimento o plato queda guardado y reutilizable bajo ese nombre para futuros registros. | Diálogos 34, 37, 38, 41b |
| RF-18 | Si el usuario declina el registro guiado de un plato nuevo, puede dar en su lugar una estimación aproximada de solo calorías, sin desglose de macronutrientes. El sistema la acepta y la suma al total calórico del día, sin afectar el desglose de otros nutrientes. | Diálogo 35 |
| RF-19 | Si el usuario no da ningún dato numérico (ni siquiera una estimación de calorías), el sistema registra que la comida existió, sin ningún valor asociado. Cualquier consulta de totales que abarque ese registro debe advertir explícitamente que hay datos faltantes, en vez de presentar el total como si estuviera completo. | Diálogo 36 |

## B. Objetivos personales

| ID | Requerimiento | Criterio de aceptación |
|---|---|---|
| RF-20 | El sistema debe permitir definir cero o más objetivos simultáneos, de tres tipos: piso (mínimo), techo (máximo), o variedad (cualitativo). La ausencia de objetivos definidos es el estado por defecto — "sin objetivo" no se almacena como un tipo, es simplemente que no existe ningún objetivo guardado para ese usuario. | — |
| RF-21 | El sistema debe ofrecer definición asistida de objetivos cuando el usuario no tiene uno claro, mediante preguntas guiadas que alimenten el cálculo determinista de necesidades (RF-22) — la aplicación sugiere, el usuario decide el valor final. | — |
| RF-22 | El sistema debe calcular las necesidades nutricionales utilizando métodos deterministas y reproducibles, basados en fórmulas y tablas reconocidas — nunca como texto generado libremente. | Fuente: ISSN Position Stand (documento 01/06) |
| RF-23 | El sistema debe permitir cambiar un objetivo existente por conversación en cualquier momento. | Diálogo 27 |

## C. Consultas y seguimiento

| ID | Requerimiento | Criterio de aceptación |
|---|---|---|
| RF-24 | El sistema debe responder consultas de progreso (día/semana/mes) distinguiendo tres tipos de información: progreso hacia un objetivo explícito, observación incidental neutra (sin objetivo asociado), y patrón de variedad semanal (conteo de categorías). | Diálogo 7 |
| RF-25 | El sistema debe permitir consultar rangos de fecha flexibles (ayer, semana pasada, mes pasado, últimos N días), no solo el período actual. | — |
| RF-26 | El sistema debe poder sugerir fuentes alternativas de un nutriente cuando el usuario las pide, sin depender de un perfil de restricciones guardado. | Diálogo 10 |
| RF-27 | Ante una consulta sobre datos o estadísticas que no existen o no se calculan, el sistema debe decirlo explícitamente — nunca inventar una respuesta. | Diálogo 28 |
| RF-28 | El sistema debe ofrecer un panel visual simple (día/semana/mes) con las mismas cifras que entrega el chat, sin gráficos de tendencia elaborados. | — |
| RF-29 | Ante vacíos de registro (días o comidas sin datos), el sistema debe reconocerlos explícitamente sin tono de reclamo y responder con lo disponible. | Diálogo 11 |

## D. Cuenta y datos personales

| ID | Requerimiento | Criterio de aceptación |
|---|---|---|
| RF-30 | El sistema debe permitir registrarse e iniciar sesión con cuenta propia (email + contraseña). | — |
| RF-31 | El perfil (peso, estatura, sexo, edad, actividad) se completa mediante formulario, no conversación, con todos los campos opcionales al inicio (completitud progresiva). Los atributos incluidos están determinados por lo que exigen las fórmulas de cálculo (ej. Mifflin-St Jeor para gasto energético requiere peso, estatura, edad y sexo) — la lista se amplía si una fórmula futura lo requiere. | — |
| RF-32 | Si falta un dato de perfil necesario para una consulta, el sistema debe explicarlo en el momento y pedir el dato puntual, sin bloquear el resto de la funcionalidad. | — |
| RF-33 | El sistema debe permitir editar o eliminar un registro pasado desde una pantalla simple. | — |
| RF-34 | El sistema debe permitir eliminar la cuenta completa, con borrado en cascada de los datos asociados. | — |

## E. Límites de comportamiento (aplican transversalmente a todo lo anterior)

| ID | Requerimiento | Criterio de aceptación |
|---|---|---|
| RF-35 | Ante una consulta médica o de diagnóstico, el sistema nunca responde en seco — explica qué sí puede hacer y redirige a un profesional de la salud. | Diálogo 8 |
| RF-36 | Ante una solicitud fuera del dominio de nutrición (ej. recordatorios), el sistema explica amablemente que esa función no existe, sin necesidad de redirección médica. | Diálogo 9 |

> **Requerimiento de tono removido de esta tabla:** "ninguna respuesta debe incluir alabanza exagerada ni culpa" es un atributo transversal de calidad (tono), no una función con disparador específico — pertenece al documento 05 (Requerimientos no funcionales) junto a rendimiento, seguridad y accesibilidad, no a este documento.

---

## F. Adiciones posteriores (pertenecen conceptualmente a la sección A)

| ID | Requerimiento | Criterio de aceptación |
|---|---|---|
| RF-37 | Si el usuario registra un plato con el mismo nombre que uno ya guardado, pero con una diferencia que cambiaría significativamente el cálculo (ej. cambio de fuente de proteína), el sistema lo señala y pregunta si es una variante distinta, sugiriendo un nombre diferenciado — el usuario decide el nombre final o si prefiere no diferenciar. Si declina, el sistema respeta esa decisión sin insistir. | Diálogo 39 |
| RF-38 | El sistema debe permitir crear un plato nuevo por referencia a uno ya guardado (ej. "como la cazuela de ave, pero con vacuno en vez de pollo"), copiando su composición y recalculando solo el nutriente afectado por el cambio indicado. | Diálogo 40 |
| RF-39 | El sistema debe poder explicar cómo interpretar una etiqueta nutricional cuando el usuario lo pregunta — qué es una porción, la diferencia entre porción y envase total, qué significa el % de valor diario, cómo leer una lista de ingredientes. Es información educativa general dentro del dominio de macronutrientes, no un análisis del producto específico del usuario salvo que ya esté registrado. | Diálogo 42 |

---

**Nota de trazabilidad:** los IDs de este documento (RF-01 a RF-39) son la referencia que deben usar los documentos 07 (modelo de dominio), 08 (modelo de datos) y 09 (contrato de IA) al justificar por qué existe cada entidad, campo o regla — así cualquier decisión posterior es trazable hasta un requerimiento concreto, no una intuición suelta.