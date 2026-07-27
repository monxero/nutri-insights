# Documento 03 — Alcance del MVP

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

> Este documento es la lista definitiva de qué entra y qué no entra en la primera versión funcional del proyecto. No repite el razonamiento detrás de cada decisión — eso vive en `CONTEXTO.md` y, más adelante, en los documentos 04/07/08/09 correspondientes. Aquí solo se declara el límite.

## 1. Capacidades del usuario en la v1

> Prueba aplicada a cada línea: si mañana cambia la tecnología usada (otro modelo de IA, otra fuente de datos), ¿esta capacidad sigue siendo cierta? Si sí, pertenece aquí. Si no, es una dependencia técnica (sección 2) o pertenece al documento 06.

### Registrar comidas por conversación
- Registrar comidas en lenguaje natural, incluyendo cualquier día pasado y varias comidas en un mismo mensaje.
- Etiquetar la comida (desayuno/almuerzo/etc.) de forma opcional, nunca obligatoria.
- Registrar y consultar en un mismo mensaje, sin obligar al usuario a separarlos en turnos distintos.
- Agregar información a un registro reciente o corregirlo, dentro de la misma conversación activa (corregir un registro de días atrás se hace desde la pantalla de edición, no por chat).
- Cada registro se confirma de inmediato, con más o menos detalle según la complejidad del mensaje.
- El sistema es tolerante a la forma en que se escribe (errores, mezcla de idiomas) y solo pide un dato puntual cuando realmente lo necesita — no de forma sistemática. Casos concretos que debe manejar sin fricción adicional: cantidades poco claras, valores implausibles, mensajes repetidos por accidente, y referencias de fecha ambiguas. *(El detalle exacto de cada uno de estos casos se traslada al documento 04 cuando se escriba — aquí solo se declara que la capacidad existe.)*

### Definir y seguir objetivos personales
- Definir uno o más objetivos simultáneos: un mínimo a alcanzar, un máximo a no exceder, o simplemente variedad.
- Recibir ayuda conversacional para definir un objetivo si no se tiene uno claro.
- Cambiar un objetivo existente en cualquier momento, por conversación.

### Consultar y hacer seguimiento
- Preguntar por el progreso del día, la semana o el mes, con una respuesta honesta que distingue entre progreso hacia un objetivo, observaciones neutras sin objetivo asociado, y patrones de variedad.
- Consultar por rangos de fecha flexibles (ayer, la semana pasada, el mes pasado, los últimos N días), no solo el día/semana/mes actual — es la misma consulta ya definida, con más opciones de rango.
- Pedir fuentes alternativas de un nutriente en cualquier momento.
- Ver un panel simple (día/semana/mes) con las mismas cifras que entrega el chat, sin gráficos elaborados.
- Recibir una respuesta honesta cuando se pregunta por algo que la app no puede calcular o no tiene registrado — nunca una respuesta inventada.

### Gestionar la cuenta y los datos personales
- Registrarse e iniciar sesión con una cuenta propia.
- Completar el perfil (peso, sexo, edad, actividad) de forma progresiva, sin que todo sea obligatorio desde el inicio.
- Editar o eliminar un registro pasado desde una pantalla simple.
- Eliminar la cuenta completa, con borrado en cascada de los datos asociados.

## 2. Dependencias técnicas del MVP

> Estas líneas sí dejarían de ser ciertas si cambia la tecnología — por diseño no pertenecen a la sección anterior. El detalle de cada una se decide en el documento 06.

- Un modelo de lenguaje con salida estructurada confiable, para interpretar el registro conversacional.
- Una fuente de datos nutricionales para alimentos genéricos y productos comerciales.
- Un sistema de autenticación de usuarios.

## 3. Fuera del MVP (roadmap)

- Corregir un registro de un día anterior por conversación (v1 lo resuelve desde la pantalla de edición).
- Que la app reconozca referencias como "lo de siempre", mediante comidas frecuentes guardadas explícitamente por el usuario.
- Afinar las estimaciones con datos de uso real, en vez de los valores iniciales razonables de v1.
- Ajustar el diccionario de calificadores de cantidad por usuario individual, en vez de uno fijo compartido.
- Gráficos y visualizaciones de tendencia más elaboradas en el panel.
- Roles de usuario adicionales y proveedores de login externos.
- Detección de patrones de alimentación más sofisticados que el conteo de variedad por categoría.
- **Registro por foto de comida.** Arquitectónicamente sencillo de agregar más adelante: Gemini es multimodal nativo y ya acepta imágenes junto con texto en la misma llamada con `responseSchema` que se usa para texto — no requiere rediseño. Evidencia externa (documento 01) sugiere que es la mejora individual con más impacto potencial en retención, por la reducción drástica de fricción de entrada.
- **App nativa instalable (Android/iOS).** Confirmado: .NET MAUI con Blazor Hybrid permite reutilizar los mismos componentes Razor de la app web dentro de una app nativa — no exige reescribir la interfaz. Microsoft tiene una plantilla oficial para este escenario exacto (web + nativo compartiendo componentes).
- Exportar el historial completo (ej. a XML/JSON) — sin necesidad real identificada del producto; candidata a implementarse igual como pieza de portafolio puntual, conectada al conocimiento de XML de la oferta laboral, si se decide más adelante.
- Consultas comparativas o de contenido sobre el historial (ej. "¿cuánto pescado comí la semana pasada?", "¿cómo se compara mi consumo de pescado contra el de carne?") — más útiles que una simple lista de días, pero requieren un mecanismo de consulta distinto al filtro por rango de fechas del MVP. Idea, no decisión — evaluar cuando se retome.

## 4. Pendiente de decidir — todos resueltos, ver documento 06

- ~~Comportamiento ante fallas externas.~~ **Resuelto — ver documento 05, RNF-03 a RNF-05.**
- ~~Blazor Server vs. WebAssembly.~~ **Resuelto — Blazor Web App con render mode `InteractiveServer`, ver documento 06, bloque 1.**
- ~~DevExpress vs. alternativas gratuitas.~~ **Resuelto — MudBlazor, ver documento 06, bloque 1.**
- ~~Diseño responsivo para uso en navegador móvil.~~ **Resuelto como parte del mismo bloque — ya funciona en el navegador del teléfono desde el día uno.**

---

**Nota de proceso:** este documento probablemente se comprima más adelante, cuando el documento 04 (requerimientos funcionales) exista y absorba el detalle granular de cada capacidad. Eso no significa que este documento esté mal — es exactamente el mismo patrón de "vaciar hacia el documento correspondiente" que ya aplicamos entre `CONTEXTO.md` y este archivo.