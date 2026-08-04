# Documento 11 — Registro de decisiones arquitectónicas (ADR)

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Formato: Michael Nygard (Contexto / Alternativas evaluadas / Decisión / Consecuencias). Cada ADR consolida una decisión transversal y difícil de revertir que hasta ahora vivía dispersa entre varios documentos — el objetivo es que dentro de varios meses se pueda entender por qué el sistema es como es, sin reconstruir el razonamiento desde cero.

---

## ADR-001: Separación estricta entre interpretación (IA) y cálculo (motor determinista)

**Contexto:** hay que decidir si el modelo de lenguaje puede calcular directamente valores nutricionales, o si ese cálculo debe vivir siempre en código determinista.

**Alternativas evaluadas:**
- Dejar que el LLM calcule directamente — más simple de implementar, pero no reproducible (mismo input podría no dar exactamente el mismo output) y con riesgo real de alucinación, confirmado con evidencia externa (documento 02).
- Separar interpretación de cálculo — la IA solo extrae datos estructurados, el motor determinista calcula.

**Decisión:** separación estricta. La IA nunca calcula ni genera un número que alimente el sistema.

**Consecuencias:** permite sustituir el proveedor de IA sin tocar la lógica de negocio (RNF-11); habilita pruebas unitarias del motor sin necesidad de simular la IA (RNF-20); es el principio fundacional de todo el proyecto (documento 00).

---

## ADR-002: Blazor Web App con render mode `InteractiveServer`

**Contexto:** elegir cómo se construye el frontend, entre los modelos disponibles de Blazor.

**Alternativas evaluadas:**
- Blazor WebAssembly — corre en el navegador, independiente del servidor, pero la app depende del servidor en casi cada interacción de todas formas (IA, base de datos), por lo que su ventaja principal no aplica.
- Blazor Server como proyecto separado — modelo previo a .NET 8, hoy considerado una excepción a justificar, no el punto de partida.
- Blazor Web App unificado, con render mode configurable por componente — patrón actualmente recomendado por Microsoft.

**Decisión:** Blazor Web App, con `InteractiveServer` para prácticamente todos los componentes.

**Consecuencias:** carga inicial liviana (favorece RNF-16, uso en móvil); depende de una conexión persistente (SignalR) — sin conexión, la interactividad se pausa; deja abierto el camino a una app nativa vía MAUI Blazor Hybrid sin reescribir componentes; permite mezclar distintos render modes por componente si en el futuro aparece un caso concreto donde `InteractiveWebAssembly` tenga sentido — no hay que "casarse" con un único modelo para toda la aplicación.

---

## ADR-003: MudBlazor en vez de DevExpress

**Contexto:** la oferta laboral menciona DevExpress como conocimiento deseable, pero su uso más allá de la evaluación de 30 días no está permitido por su EULA (verificado contra la licencia oficial de DevExpress Blazor).

**Alternativas evaluadas:**
- DevExpress — descartado: el EULA prohíbe explícitamente crear proyectos reales con la licencia de evaluación.
- MudBlazor — gratuita, código abierto, amplia adopción en la comunidad .NET.
- Radzen — alternativa también gratuita, considerada pero no elegida.

**Decisión:** MudBlazor.

**Consecuencias:** sin riesgo de incumplimiento de licencia; el conocimiento de DevExpress de un proyecto anterior (Kino-Analizer) sigue siendo mencionable en entrevista como experiencia previa, mientras este proyecto demuestra adaptabilidad a otro ecosistema.

---

## ADR-004: PostgreSQL en vez de SQL Server

**Contexto:** elegir el motor de base de datos relacional.

**Alternativas evaluadas:**
- SQL Server — la edición Express tiene límites de tamaño y restricciones de uso en producción.
- SQLite — descartada por baja concurrencia de escritura, no apta para el escenario multiusuario (RNF-13).
- PostgreSQL — gratuita sin límites de tamaño ni restricciones de producción, corre nativamente en el entorno Linux/WSL ya utilizado, con proveedor EF Core (Npgsql) activo y mantenido (verificado).

**Decisión:** PostgreSQL.

**Consecuencias:** sin costo de licencia en ningún escenario; sigue siendo "SQL" en el sentido genérico que pide la oferta laboral; reduce la dependencia del ecosistema de un solo proveedor, al ser PostgreSQL un estándar ampliamente soportado fuera del mundo Microsoft.

---

## ADR-005: Identity configurado con `Guid` como tipo de clave

**Contexto:** ASP.NET Identity usa `string` como tipo de columna por defecto (aunque el valor interno ya es un Guid convertido a texto); el resto del esquema del proyecto usa `Guid` de forma consistente.

**Alternativas evaluadas:**
- Dejar Identity en `string` y el resto en `Guid` — inconsistencia de tipos entre tablas relacionadas.
- Configurar explícitamente `ApplicationUser : IdentityUser<Guid>` — patrón oficialmente documentado por Microsoft para este propósito.

**Decisión:** `Guid` en toda la aplicación, incluida `AspNetUsers`.

**Consecuencias:** esquema consistente; esta configuración debe hacerse desde el inicio del proyecto — cambiar el tipo de clave de Identity después de tener datos reales es una migración costosa.

---

## ADR-006: Razor Pages para las pantallas propias de Identity

**Contexto:** la documentación oficial de Microsoft advierte que Identity está diseñado para el modelo de petición/respuesta HTTP tradicional, distinto al modelo de conexión persistente de Blazor Server, y desaconseja construir las pantallas de login/registro/logout como componentes Blazor interactivos.

**Alternativas evaluadas:**
- Forzar Identity dentro de componentes Blazor interactivos — no soportado oficialmente, riesgo de comportamiento inesperado.
- Usar Razor Pages (otra tecnología de UI dentro de ASP.NET Core) específicamente para las pantallas de Identity.

**Decisión:** Razor Pages para login/registro/logout/gestión de cuenta; el resto de la aplicación permanece en Blazor interactivo, consultando el estado de autenticación ya establecido.

**Consecuencias:** dos tecnologías de interfaz coexisten en la misma solución, cada una en el dominio para el que fue diseñada — no es una inconsistencia, es la separación correcta.

---

## ADR-007: Arquitectura híbrida de fuentes nutricionales — 2 fuentes en producción

**Contexto:** ninguna fuente de datos nutricionales sola cubre bien productos envasados con marca y alimentos genéricos/crudos a la vez (documento 02).

**Alternativas evaluadas:**
- Solo OpenFoodFacts — cobertura débil en alimentos genéricos por diseño (está pensada para productos con código de barras).
- Construir una tabla propia desde cero — trabajo de investigación evitable.
- Tabla propia con semilla de USDA FoodData Central (categoría Foundation Foods, dominio público, verificado) + curación regional para alimentos específicamente chilenos.

**Decisión:** OpenFoodFacts consultado en vivo + tabla propia curada (semilla USDA importada una sola vez + curación regional). Dos fuentes en producción, no tres — USDA no es una dependencia en tiempo real.

**Consecuencias:** menor riesgo operativo (una dependencia externa en vivo, no dos); la curación regional (traducción, adaptación a alimentos chilenos) es trabajo real pendiente, no automático.

**Corrección (Etapa 3):** el número original de "~8.000 alimentos" citado en este ADR y en el documento 06 correspondía a SR Legacy (7.793 ítems), un dataset de USDA distinto y ya congelado desde 2018 — no es el que se usa en este proyecto. La categoría real usada, Foundation Foods, es mucho más pequeña y crece con cada release semestral: 287 alimentos en la versión de abril 2024, 395 en la versión de abril 2026 (verificado contra el JSON descargado). La decisión de usar Foundation Foods sobre SR Legacy se mantiene sin cambios (más reciente, activamente mantenida, con metadatos más ricos) — solo el número estaba mal.
---

## ADR-008: `Alimento` como una sola tabla (identidad + información nutricional juntas)

**Contexto:** el documento 07 dejó abierta la pregunta de separar la identidad de un alimento de sus valores nutricionales.

**Alternativas evaluadas:**
- Separar en `Alimento` + `AlimentoNutriente` normalizado — prepara mejor para micronutrientes futuros (hierro, calcio, vitamina D, etc.), pero agrega un `JOIN` a cada consulta sin beneficio inmediato.
- Mantener una sola tabla — más simple, suficiente para el alcance de macronutrientes del MVP (documento 00).

**Decisión:** una sola tabla para el MVP.

**Consecuencias:** documentado explícitamente como simplificación deliberada (documento 08) — si se agregan micronutrientes más adelante, esta decisión se revisita primero.

---

## ADR-009: Caché local de productos consultados a OpenFoodFacts

**Contexto:** RNF-05 exige resiliencia ante la caída de la fuente de datos externa.

**Alternativas evaluadas:**
- Consultar siempre en vivo — más simple, pero sin resiliencia ante fallas externas.
- Guardar una copia local tras la primera consulta de cada producto.

**Decisión:** cachear localmente. La primera vez que un producto se consulta, se guarda una copia en la base de datos propia; consultas futuras del mismo producto se resuelven localmente. La copia local pasa a ser la fuente preferente para consultas futuras — no es un espejo sincronizado con OpenFoodFacts, es una copia persistente. La sincronización con la fuente externa deja de ser parte del flujo normal; si se incorpora en el futuro, sería mediante una estrategia explícita de actualización, no automática.

**Consecuencias:** reduce llamadas repetidas a la API externa; un producto ya consultado sigue disponible aunque OpenFoodFacts esté caído después.

---

## ADR-010: Confirmaciones simples sin llamada adicional a la IA

**Contexto:** cada respuesta del sistema podría generarse vía IA, incluyendo confirmaciones triviales de registro.

**Alternativas evaluadas:**
- Toda respuesta pasa por la IA — más "inteligente" en apariencia, pero mayor costo, mayor latencia, y riesgo de que el tono varíe de forma impredecible.
- Confirmaciones simples mediante plantillas de texto en C#, con variación entre unas pocas frases; la IA se reserva para extracción y para generar texto abierto donde realmente aporta valor.

**Decisión:** plantillas C# para confirmaciones simples.

**Consecuencias:** ahorra costo y cuota del proveedor de IA en tareas completamente deterministas; el tono queda garantizado, sin depender de que el modelo "decida bien" cómo frasear algo trivial.

---

## ADR-011: `ItemDeRegistro` guarda una copia (snapshot) de los valores nutricionales, no solo una referencia viva

**Contexto:** el documento 00 exige comportamiento determinista — el mismo registro debe producir siempre el mismo resultado. Si `ItemDeRegistro` solo referenciara `AlimentoId` y recalculara contra sus valores actuales, un registro histórico podría cambiar de resultado si esos valores se corrigen después — rompiendo esa garantía.

**Alternativas evaluadas:**
- Referenciar `Alimento` en vivo, recalcular siempre con el valor actual — más simple, pero viola el determinismo histórico.
- Guardar una copia de los valores nutricionales en el momento del registro, además de mantener la referencia a `AlimentoId` para trazabilidad.

**Dentro de la opción de snapshot, dos formas posibles, y ambas se evaluaron explícitamente:**
- **Opción A — valores ya escalados y finales** (ej. "150g de pollo → 247 kcal, 46g proteína", el resultado final del cálculo, no la fórmula).
- **Opción B — valores por 100g + cantidad**, para recalcular el escalado cada vez que se necesite.

La opción B seguiría dependiendo de una regla de cálculo futura (cómo escalar cantidad × valor por 100g) — si esa lógica de escalado cambiara algún día, un registro histórico volvería a estar en riesgo de cambiar de resultado, reabriendo exactamente el problema que este ADR busca cerrar.

**Decisión:** `ItemDeRegistro` guarda snapshot con los **valores ya escalados y finales** (opción A) — calorías, proteína, carbohidratos, grasa, fibra, ya calculados para la cantidad exacta registrada, no una fórmula pendiente de resolver. `AlimentoId` se mantiene como referencia de trazabilidad ("esto fue pollo"), pero el cálculo de totales siempre usa el snapshot ya resuelto, nunca el valor actual de `Alimento` ni una recombinación futura.

**Consecuencias:** un registro pasado nunca cambia de resultado, coherente con la filosofía de que un dato pasado con menos precisión no es un dato "malo", solo refleja lo que se sabía en ese momento (documento 00). Corregir `Alimento` en el futuro mejora los registros nuevos, sin alterar silenciosamente los antiguos. Resuelve además, como efecto secundario, la pregunta pendiente sobre eliminar un alimento personal con historial: al no depender del valor en vivo, eliminar el `Alimento` ya no pone en riesgo los totales históricos.

---

## ADR-012: El comportamiento del sistema pertenece al dominio, no al proveedor de IA

**Contexto:** los modelos de lenguaje cambian constantemente — de proveedor, de versión, de comportamiento. Hay que decidir dónde vive la definición de cómo debe comportarse el sistema conversacionalmente.

**Alternativas evaluadas:**
- Dejar el comportamiento definido únicamente en el prompt del proveedor de IA — rápido de escribir, pero el comportamiento queda atado a ese proveedor específico y es difícil de auditar o versionar de forma independiente.
- Documentar el comportamiento como reglas de dominio independientes del proveedor, y derivar el prompt a partir de esas reglas.

**Decisión:** las reglas de conversación, límites y comportamiento se documentan primero como requerimientos funcionales y contrato de integración (documentos 04 y 09). El prompt del proveedor de IA se deriva de esos documentos, nunca al revés — el flujo es Documento 04 → Documento 09 → esquema/prompt → proveedor de IA, no proveedor de IA → comportamiento.

**Consecuencias:** el proveedor de IA puede sustituirse (Gemini por otro modelo) reescribiendo únicamente la adaptación técnica del contrato, sin tener que reconstruir el comportamiento desde cero ni arriesgarse a perder reglas por el camino; el comportamiento del sistema es auditable y versionado como cualquier otro documento del proyecto, no vive disperso en un prompt improvisado.

---

## ADR-013: Catálogos extensibles mediante tablas de referencia, no enums

**Contexto:** algunos conjuntos de valores (unidades de medida, categorías de alimento) pueden crecer durante la vida útil del sistema, sin que ese crecimiento requiera cambiar ninguna lógica de negocio.

**Alternativas evaluadas:**
- Enums compilados — simples y con seguridad de tipos en tiempo de compilación, pero cada valor nuevo exige recompilar y desplegar la aplicación.
- Tablas de referencia — agregar un valor nuevo es insertar una fila, sin tocar código.

**Decisión:** cuando un conjunto de valores pueda evolucionar con el tiempo sin requerir cambios de lógica, se modela como tabla de referencia, no como enum. Ya aplicado a `CategoriaAlimento` y `UnidadMedida`; el mismo criterio se aplica a cualquier catálogo futuro con la misma naturaleza (posiblemente tipos de objetivo, si ese conjunto llegara a crecer).

**Consecuencias:** agregar un valor nuevo a estos catálogos no exige recompilar ni volver a desplegar la aplicación — es una filosofía aplicada consistentemente, no una decisión aislada de una sola tabla.

---

**Nota de mantenimiento:** cada vez que una decisión de este tipo se tome de aquí en adelante (documentos 10 en adelante, o durante la implementación), debe agregarse aquí como un nuevo ADR, no quedar solo mencionada de pasada en otro documento.