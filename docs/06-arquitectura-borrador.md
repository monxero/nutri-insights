# Documento 06 — Arquitectura

**Versión:** 0.2 (borrador completo) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Cada decisión de este documento debe poder señalar qué RF o RNF la motiva — no se justifican por preferencia personal. Este documento se construye bloque por bloque; cada sección incluye por qué se eligió (RF/RNF que motiva) y cómo funciona (mecanismo técnico).

## 0. Vista general de capas

**El recorrido completo de una interacción, de principio a fin:**

```
Usuario
  ↓
Frontend
  ↓
Backend (orquesta)
  ↓
IA (solo si hace falta interpretar lenguaje natural)
  ↓
Fuente nutricional (OpenFoodFacts o tabla curada)
  ↓
Motor determinista (calcula)
  ↓
Persistencia (guarda)
  ↓
Respuesta (vuelve al usuario)
```

Este flujo resume los seis bloques que siguen — cada uno de ellos es una parada de este recorrido, desarrollada en detalle más abajo.

El usuario solo interactúa con el **Frontend**. El Frontend nunca habla directamente con la base de datos, la IA, ni la fuente de datos nutricionales — todo pasa por el **Backend**, el único componente que conoce esas cuatro piezas. Esta forma es la representación directa de RNF-11 (poder sustituir el proveedor de IA sin modificar la lógica de negocio): si el Frontend hablara directo con el proveedor de IA, esa garantía se rompería.

Los cuatro servicios que usa el Backend (Persistencia, Autenticación, IA, Datos nutricionales) están al mismo nivel entre sí — son servicios que el Backend consume, no capas apiladas una sobre otra.

## 1. Frontend — Blazor Web App (render mode `InteractiveServer`) + MudBlazor

**Por qué:**
- RNF-16 (usable en móvil vía navegador desde el día uno, sin app nativa): `InteractiveServer` tiene una carga inicial más liviana que WebAssembly.
- RNF-14 (reutilizable en una futura app nativa, minimizando reescritura): los componentes Razor se reutilizan tal cual en .NET MAUI Blazor Hybrid.
- La app depende del servidor en casi cada interacción (IA, base de datos) — el beneficio principal de WebAssembly (independencia del servidor) no aplica a este caso de uso.
- MudBlazor se eligió sobre DevExpress por ser gratuita y de código abierto, evitando cualquier ambigüedad de licenciamiento (ver `CONTEXTO.md` para el análisis completo de por qué se descartó DevExpress para este proyecto).

**Cómo funciona:**
- El código C# de cada componente se ejecuta en el servidor, no en el navegador del usuario.
- Una conexión persistente (SignalR) transmite los eventos de la interfaz (clics, texto escrito) hacia el servidor.
- El servidor ejecuta la lógica correspondiente y calcula qué cambió en la interfaz.
- Solo el fragmento de HTML que cambió se envía de vuelta al navegador — no la página completa.
- Si la conexión persistente con el servidor se interrumpe, las funciones interactivas dejan de estar disponibles temporalmente hasta que la conexión se restablece.
- MudBlazor provee componentes de interfaz ya construidos (botones, formularios, tablas) que se usan dentro de los componentes Razor propios, sin cambiar el mecanismo de comunicación anterior.

## 2. Backend — lógica de negocio y motor de cálculo determinista

**Por qué:**
- RNF-11 (sustituir el proveedor de IA sin tocar la lógica de negocio): solo es posible si el motor de cálculo nunca llama ni depende del SDK de IA — solo recibe datos ya estructurados.
- RNF-20 (el motor determinista debe cubrirse con pruebas unitarias): esta separación es lo que hace esas pruebas triviales de escribir, sin necesitar simular llamadas de red.
- RF-22 (cálculo con fórmulas y tablas reconocidas, nunca texto generado libremente): el motor de cálculo es la única pieza autorizada a producir esos números.

**Cómo funciona:**
- El mensaje o solicitud del usuario llega al Backend, que orquesta el resto del proceso.
- Cuando la solicitud requiere interpretar lenguaje natural, el Backend delega esa tarea a la capa de IA para obtener datos estructurados (alimentos, cantidades, fechas). Las operaciones que no requieren interpretación de lenguaje natural (ver el panel, editar un registro, iniciar sesión, consultar estadísticas) se resuelven completamente dentro del Backend, sin pasar por la IA.
- Cuando sí hay datos estructurados de por medio, se pasan al motor de cálculo, que nunca conoce ni depende del proveedor de IA. Funciona como una calculadora: recibe números, devuelve un resultado, sin importarle de dónde vinieron.
- El resultado se guarda mediante la capa de persistencia.
- El Backend toma ese resultado ya calculado y construye la respuesta final que ve el usuario — este último paso también es responsabilidad del Backend, no de la IA ni del motor de cálculo por separado.

  *Ejemplo completo del recorrido:* Usuario escribe "hoy comí dos huevos" → la IA devuelve `{alimento: huevo, cantidad: 2}` → el motor calcula 156 kcal y 12.6g de proteína → la persistencia guarda ese registro → el Backend arma la respuesta final: *"Registro guardado. Llevas 42g de proteína hoy."*
- En términos de estructura de proyecto: el motor de cálculo vive como una librería de clases lógicamente separada dentro de la misma solución .NET, sin ninguna referencia al SDK de IA en su código — si alguna vez necesitara importar algo relacionado con IA para funcionar, es señal de que la separación se rompió.

## 3. Persistencia — PostgreSQL + Entity Framework Core

**Por qué:**
- RNF-13 (múltiples usuarios concurrentes sin cambios estructurales): el sistema requiere una base de datos relacional diseñada para entornos multiusuario, con soporte robusto para concurrencia, transacciones e integridad de datos. (SQLite fue descartada por estar orientada principalmente a aplicaciones locales y escenarios de baja concurrencia.)
- RNF-08 (los datos de un usuario nunca visibles para otro): un esquema relacional con relaciones bien definidas (registros ligados a un usuario por clave foránea) hace de este aislamiento una garantía de la estructura de datos, no algo que dependa de que el código nunca falle.
- La estructura de datos ya diseñada (objetivos tipados, registros con fecha y comida opcional, platos personales reutilizables) encaja naturalmente en tablas relacionadas — el caso de uso típico para una base relacional.
- Se eligió PostgreSQL sobre SQL Server para este proyecto: gratuita sin límites de tamaño ni restricciones de uso en producción, y corre nativamente en el entorno Linux/WSL ya utilizado. Sigue siendo "SQL" en el sentido genérico que pide la oferta laboral.

**Cómo funciona:**
- Entity Framework Core implementa un modelo de mapeo objeto-relacional (ORM): el desarrollador trabaja con clases C# normales (ej. `class Comida`), y EF Core se encarga de traducirlas hacia tablas y registros de la base de datos, y viceversa.
- El `DbContext` es el objeto que representa la sesión con la base de datos; a través de él se piden las operaciones de guardar, buscar o actualizar. Cada `DbSet<T>` dentro del DbContext corresponde a una tabla.
- Las consultas se realizan normalmente mediante LINQ sobre las entidades del modelo — prácticamente nunca se escribe SQL a mano. Por ejemplo: `db.Comidas.Where(c => c.UsuarioId == id).OrderBy(c => c.Fecha)`. EF Core traduce esa expresión de C# al SQL correspondiente para PostgreSQL.
- Las migraciones son el mecanismo por el cual, al cambiar una clase C#, EF Core genera automáticamente el script SQL necesario para actualizar la estructura de la base de datos — se editan las clases, no la base de datos directamente. Esto es especialmente relevante ahora, porque el modelo de datos todavía cambiará mientras se completan los documentos 07 y 08.

## 4. Autenticación — ASP.NET Identity

**Por qué:**
- RF-30 (registrarse e iniciar sesión con cuenta propia): Identity resuelve esto de fábrica, sin construir el sistema desde cero.
- RNF-06 (credenciales almacenadas de forma segura, nunca en texto plano): aplicado por defecto, sin implementación manual.
- RNF-08 (datos de un usuario nunca visibles para otro): Identity provee el identificador de usuario usado como clave foránea en cada tabla del dominio — la pieza que hace cumplible ese aislamiento a nivel de base de datos.
- Se reutiliza la base ya usada en Kino-Analizer, con dos ajustes: política de contraseña más estricta (mínimo 8 caracteres, con símbolo o mayúscula, en vez de los 6 caracteres sin restricciones anteriores), y sin roles (no hay caso de uso en el MVP que los requiera).

**Cómo funciona el login básico:**
- El usuario ingresa sus credenciales; Identity compara el hash de la contraseña ingresada contra el hash almacenado, sin exponer la contraseña original en ningún momento.
- Si coincide, se crea una cookie de sesión — la prueba de que el usuario inició sesión.
- Cada solicitud posterior del navegador incluye esa cookie automáticamente, e Identity la valida en cada una sin que el usuario tenga que autenticarse de nuevo.

**Integración con el dominio:**
- Identity se integra en el mismo `DbContext` ya usado para el resto del dominio (heredando de `IdentityDbContext<IdentityUser>`), creando automáticamente las tablas necesarias (`AspNetUsers`, `AspNetRoles`, `AspNetUserClaims`, entre otras) en la misma base de datos — aunque en este proyecto varias de ellas permanecerán prácticamente vacías al no utilizar roles ni características avanzadas.
- El `Id` de `AspNetUsers` es la clave foránea usada en las tablas de comidas, objetivos y perfil — la conexión concreta entre "usuario autenticado" y "los datos le pertenecen a ese usuario".

**Modelo de autenticación elegido — cookies, no tokens:**
- Se usa autenticación por cookies mientras el Frontend y el Backend corran en el mismo proceso del servidor.
- Si en el futuro la aplicación expone una API para clientes externos desacoplados, conviene reconsiderar tokens (JWT) para esas comunicaciones.
- Ejemplo concreto de este caso, verificado contra la documentación oficial de Microsoft: el patrón recomendado para .NET MAUI Blazor Hybrid conectado a un Web App compartido usa autenticación por tokens guardados en almacenamiento seguro del dispositivo, no cookies compartidas con el navegador — así que si este proyecto migra a una app nativa vía MAUI, esa integración específica sí necesitaría tokens, aunque el resto de la app siga usando cookies.

**Integración con Blazor — el límite entre dos modelos de comunicación:**
- Confirmado en la documentación oficial de Microsoft: las pantallas propias de Identity (registro, login, logout, gestión de cuenta) deben construirse como **Razor Pages**, no como componentes Blazor interactivos.
- Motivo oficial: Identity está diseñado para el modelo de petición/respuesta HTTP tradicional, distinto al modelo de conexión persistente que usa Blazor Server. Microsoft desaconseja explícitamente construir esas pantallas específicas como componentes Blazor.
- El resto de la aplicación (chat, panel, edición de registros) sigue siendo Blazor interactivo normal — solo necesita consultar el estado de autenticación ya establecido por Identity (mediante `AuthorizeView` o el atributo `[Authorize]`), sin reconstruir el proceso de login.

## 5. Integración de IA — Gemini con structured output

**Por qué:**
- RF-01 a RF-19 (todo el registro conversacional): necesitan un modelo capaz de interpretar lenguaje natural y devolver datos utilizables, no cualquier modelo de texto libre.
- RNF-11 (sustituir el proveedor de IA sin tocar la lógica de negocio): solo es posible si lo que sale de la IA es un objeto de datos con forma fija (structured output), no texto libre que el Backend tendría que interpretar con reglas frágiles — coherente con la evidencia externa del documento 02 sobre separar cálculo de generación para evitar alucinaciones.
- RNF-15 (costo operativo sostenible a escala): Gemini 2.5 Flash tiene un nivel gratuito verificado (10 solicitudes por minuto), suficiente para uso personal.
- Se ancla a una versión estable en producción, no a una versión preview, para evitar cambios de comportamiento no anunciados.

**Cómo funciona el structured output:**
- Gemini recibe un esquema (JSON Schema) que define la forma exacta esperada de la respuesta. La API valida que la respuesta cumpla ese esquema antes de entregarla a la aplicación, evitando depender de texto libre.
- Ejemplo simplificado de esquema para un registro:

```json
{
  "alimentos": [
    {
      "nombre": "huevo",
      "cantidad": 2,
      "unidad": "unidad",
      "comida": null
    }
  ]
}
```

  Sin importar cómo esté redactado el mensaje original, la respuesta siempre tiene esta forma, lista para deserializar directo a objetos C#.

**Dos roles distintos de la IA — y un tercer caso que no la necesita:**
- **Extracción:** interpretar el mensaje del usuario y devolver datos estructurados (lo descrito arriba).
- **Generación de texto conversacional:** redactar respuestas abiertas cuando hace falta (explicar un concepto de nutrición, sugerir alternativas de un nutriente) — una llamada distinta, con un propósito distinto.
- **Confirmaciones simples: no requieren IA en absoluto.** Una respuesta como "Registrado. Llevas 42g de proteína hoy." ya tiene todos sus números calculados por el motor determinista — se arma rellenando una plantilla en C#, alternando entre unas pocas variantes para no sonar repetitivo (documento 09). Esto ahorra costo y latencia, y garantiza que el tono nunca se desvíe, sin dejarle a un modelo la decisión de cómo frasear algo tan simple. Además, evita consumir cuota del proveedor de IA en tareas completamente deterministas.

## 6. Fuente de datos nutricionales — arquitectura híbrida

**Por qué:**
- RF-12 (jerarquía de 4 niveles de estimación): necesita al menos una fuente confiable de datos reales en el nivel 1, y ninguna fuente sola cubre bien los dos tipos de alimento que se van a registrar.
- Hallazgo del documento 02: OpenFoodFacts tiene fuerte cobertura de productos envasados con marca, pero cobertura débil de alimentos genéricos y crudos, por no estar diseñada para eso — de ahí la necesidad de complementarla, no reemplazarla.
- RNF-05 (si la fuente externa no responde, recurrir a la tabla curada propia): la tabla propia también es el respaldo de resiliencia cuando OpenFoodFacts falla.

**Cómo funciona:**
- Un producto envasado con marca reconocible se resuelve consultando OpenFoodFacts; un alimento genérico o crudo se resuelve consultando la tabla curada propia. Ambos caminos alimentan al mismo motor de cálculo.
OpenFoodFacts se consulta de dos formas distintas, según el caso:
- **Búsqueda por nombre** (caso principal, cuando el usuario describe un producto en lenguaje natural): contra Search-a-licious (`search.openfoodfacts.org`), un servicio separado y en beta, mediante `POST /search` con parámetro `q` de texto libre. La API v2 no soporta búsqueda de texto libre por nombre, solo filtros estructurados (categoría, marca, código) — verificado directamente contra la documentación y el comportamiento real de la API en la Etapa 3.
- **Re-consulta por código conocido** (cuando ya se guardó un `CodigoExterno` de una consulta anterior): contra la API v2 (`world.openfoodfacts.org/api/v2/product/{código}`), sin necesidad de API key para lectura.

Ambas formas se identifican con un encabezado `User-Agent` propio, requisito documentado por OpenFoodFacts. La respuesta trae los valores nutricionales normalizados por 100g, listos para escalar según la cantidad registrada.
- **Son 2 fuentes en producción, no 3.** OpenFoodFacts se consulta en tiempo real, como una dependencia externa viva (con todo lo que eso implica: RNF-03 a RNF-05, manejo de fallas). La tabla propia, en cambio, no depende de ninguna consulta externa en producción — tiene **2 orígenes de contenido**, ambos resueltos antes de que la app llegue a un usuario final:
  1. **Semilla importada de USDA FoodData Central** (categoría "Foundation Foods", ~8.000 alimentos genéricos y crudos medidos en laboratorio, dominio público bajo licencia CC0) — importada, traducida y curada una sola vez (con actualizaciones periódicas, ya que USDA revisa esta categoría un par de veces al año), no consultada en vivo.
  2. **Curación regional propia**, para alimentos y platos específicamente chilenos que no tienen buen equivalente en la semilla de USDA — se completa manualmente o mediante el flujo conversacional de agregar un plato nuevo (RF-17).
- **Decisión de diseño (se desprende de RNF-05):** cuando un usuario registra un producto de OpenFoodFacts por primera vez, se guarda una copia de esos datos en la propia base de datos — no solo el registro del usuario, sino la ficha del producto en sí. La próxima vez que ese mismo producto se registre, se resuelve directo desde la base de datos local, sin una nueva llamada a la API externa — un producto ya consultado una vez sigue disponible aunque OpenFoodFacts esté caído después.
- **Consideración resuelta con un mecanismo ya existente, no una funcionalidad nueva:** si la búsqueda contra OpenFoodFacts no encuentra un producto que el usuario nombró con precisión (ej. "galletas Nick" cuando el nombre real es "Nik"), no hace falta implementar búsqueda difusa contra la API externa — el mismo mecanismo de pregunta guiada ya definido (RF-08, RF-17) resuelve esto: la IA admite que no lo encontró y pregunta si el nombre está bien escrito. Si genuinamente no está en ninguna fuente, el usuario probablemente tiene el envase a mano y puede ingresar manualmente los datos básicos de la etiqueta (calorías, proteína, carbohidratos) — no se necesita la ficha completa para que el registro sea útil, coherente con la jerarquía de niveles de estimación.

---

**Documento 06 completo.** Los seis bloques (Frontend, Backend, Persistencia, Autenticación, Integración de IA, Fuente de datos nutricionales) están desarrollados con su justificación (RF/RNF) y su mecanismo técnico.