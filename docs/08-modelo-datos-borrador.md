# Documento 08 — Modelo de datos

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Este documento baja cada entidad del documento 07 a una tabla real, con columnas y tipos concretos, para PostgreSQL vía Entity Framework Core (documento 06). Cada tabla remite a la entidad de dominio que la origina y al RF que la justifica.

## Convenciones

- Identificadores: `Guid` como clave primaria en todas las tablas, **incluida `AspNetUsers`**. Por defecto, ASP.NET Identity usa `string` como tipo de columna (aunque internamente ya genera un Guid convertido a texto) — se configura explícitamente `ApplicationUser : IdentityUser<Guid>` para que todo el esquema sea consistentemente Guid, evitando mezclar tipos de clave entre Identity y el resto de las tablas.
- Valores nutricionales: `decimal`, normalizados **por 100g** en `Alimento` — el escalado a la cantidad real ocurre en el motor de cálculo (documento 06), no se almacena preescalado.
- Todo campo que el documento 04 marcó como opcional se modela como `NULL`-able — la completitud progresiva (RF-31) exige que la ausencia de un dato no rompa nada.

## Resuelto: la decisión pendiente del documento 07

`Alimento` se mantiene como **una sola tabla** (identidad + información nutricional juntas), no separada en dos. Motivo: en este proyecto no existe un caso de uso donde se necesite la identidad de un alimento sin su información nutricional — siempre se consultan juntas. Separar agregaría un `JOIN` en cada consulta sin beneficio real. Se anota como decisión, no se abre ADR aparte porque es de bajo impacto.

## Esquema relacional (resumen visual)

```
ApplicationUser
      │
      ├──────< Registro
      │              │
      │              └──< ItemDeRegistro >── Alimento ──> CategoriaAlimento
      │                                           │
      ├──────< Objetivo                           └── (UnidadMedida, vía AlimentoUnidadEquivalencia)
      │
      └──────< Alimento (solo origen personal)

CategoriaAlimento ──< CalificadorCantidad
```

## 1. ApplicationUser (extiende `IdentityUser`)

Entidad de dominio: `Usuario` (documento 07, sección 1).

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | (definido por Identity) | No | Clave primaria heredada |
| Peso | decimal | Sí | kg |
| Estatura | decimal | Sí | cm — requerida por Mifflin-St Jeor |
| Sexo | enum (Masculino/Femenino) | Sí | Requerido por Mifflin-St Jeor. **Nota:** este campo representa el coeficiente que exige la fórmula (dos constantes distintas según sexo biológico para el cálculo), no una clasificación de identidad — se deja explícito para que no se confunda con otro propósito más adelante. |
| FechaNacimiento | date | Sí | La edad es un dato derivado de esta fecha y la fecha actual — no se almacena directamente, para evitar inconsistencias con el paso del tiempo |
| NivelActividad | enum (Sedentario/Ligero/Moderado/Activo/MuyActivo) | Sí | Factor de ajuste para gasto energético |

**Justificación:** RF-30, RF-31, RNF-06 (heredado de Identity).

## 2. CategoriaAlimento

Entidad de dominio: `Categoría de alimento` (documento 07, sección 6).

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| Nombre | string | No | Ej. "proteína animal - carne roja" |
| PorcionReferenciaGramos | decimal | No | Base para el diccionario de calificadores |

**Justificación:** patrón de variedad semanal, diccionario de calificadores (documento 04, sección C).

## 3. UnidadMedida

Tabla de referencia, no enum — misma razón que `CategoriaAlimento`: el conjunto de unidades crece con el tiempo (hoy gramos/mililitros/unidad/cucharada/taza/porción, mañana probablemente lata, rebanada, vaso, scoop). Modelarla como tabla permite agregar una unidad nueva insertando una fila, sin recompilar ni desplegar la aplicación.

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| Nombre | string | No | Ej. "gramos", "cucharada", "lata" |

**Nota importante:** esta tabla solo identifica *qué unidad es* — no guarda ninguna equivalencia en gramos, porque esa equivalencia no es propiedad de la unidad, sino de la combinación alimento+unidad (1 taza de arroz cocido ≈195g, 1 taza de aceite ≈218g — valores distintos para la misma unidad). Ver `AlimentoUnidadEquivalencia` a continuación.

## 4. AlimentoUnidadEquivalencia

Tabla de relación entre `Alimento` y `UnidadMedida`, porque la equivalencia en gramos depende de ambos a la vez, no de uno solo.

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| AlimentoId | Guid (FK) | No | |
| UnidadMedidaId | Guid (FK) | No | |
| EquivalenteEnGramos | decimal | No | Ej. "taza" + "arroz cocido" → 195g |

**Justificación:** equivalencias de unidades caseras verificadas (decisión de `CONTEXTO.md`), diccionario de calificadores por categoría (documento 04).

## 5. CalificadorCantidad

Tabla de apoyo — no es una entidad nombrada en el documento 07, pero se desprende directamente de la regla de calificadores por categoría ya definida en `CONTEXTO.md`.

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| CategoriaAlimentoId | Guid (FK) | No | |
| Calificador | enum (Poco/Normal/Bastante/Mucho) | No | |
| MinGramos | decimal | No | Piso no-cero, incluso para "Poco" |
| MaxGramos | decimal | No | |

**Justificación:** diccionario de calificadores de cantidad por categoría (documento 04).

## 6. Alimento

Entidad de dominio: `Alimento` (documento 07, sección 4).

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| Nombre | string | No | |
| CategoriaAlimentoId | Guid (FK) | No | Categoría principal, no universal (documento 07) |
| Origen | enum (OpenFoodFacts/TablaCurada/Personal) | No | |
| UsuarioPropietarioId | Guid (FK a ApplicationUser) | Sí | Solo si Origen = Personal. **Restricción lógica** (documentada aunque EF Core no genere un CHECK automático): si Origen = Personal → obligatorio; si Origen ≠ Personal → debe ser nulo. |
| CodigoExterno | string | Sí | Código de OpenFoodFacts, para re-sincronizar si hace falta |
| CaloriasPor100g | decimal | Sí | Los nutrientes se modelan como opcionales porque distintas fuentes tienen distinto nivel de completitud (una fuente puede no documentar fibra, otra puede tener solo calorías) — no es un caso aislado de RF-18 |
| ProteinaPor100g | decimal | Sí | |
| CarbohidratosPor100g | decimal | Sí | |
| GrasaPor100g | decimal | Sí | |
| FibraPor100g | decimal | Sí | No todas las fuentes documentan fibra |
| NivelConfianza | enum (EtiquetaVerificada/BaseDatosReferencia/Estimado) | No | Distingue un dato de etiqueta (alta confianza, no absoluta) de una estimación. **No se deriva automáticamente de `Origen`:** dentro de un mismo origen la confianza real varía (ej. OpenFoodFacts mezcla datos de marca verificados con datos aportados por cualquier usuario) — por eso es un campo semi-independiente, no un espejo de `Origen`. **Simplificación reconocida:** se modela a nivel de todo el alimento, no por nutriente individual — válido porque no se soporta editar un solo nutriente de un alimento existente en el MVP. Si esa capacidad se agrega en el futuro, este campo tendría que moverse a un nivel más granular (por nutriente), lo cual también revivirá la pregunta de separar identidad e información nutricional en tablas distintas. |

**Justificación:** RF-12 (jerarquía de niveles), RF-17 (catálogo personal), documento 06 (arquitectura híbrida).

**Simplificación deliberada, no un olvido:** los nutrientes se modelan como columnas fijas (calorías, proteína, carbohidratos, grasa, fibra) en vez de una tabla normalizada `Nutriente` + `AlimentoNutriente` (Id/Nombre/Unidad y AlimentoId/NutrienteId/ValorPor100g). Es una decisión consciente para el MVP, coherente con el alcance de macronutrientes del documento 00 — si en el futuro se agregan micronutrientes (hierro, calcio, potasio, vitamina D, sodio), el patrón normalizado sería la extensión natural, evitando modificar la tabla `Alimento` cada vez que se agregue un nutriente nuevo.

## 7. Registro

Entidad de dominio: `Registro` (documento 07, sección 2).

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| UsuarioId | Guid (FK) | No | |
| Fecha | date | No | El día al que pertenece el registro — no necesariamente "hoy" (RF-01, RF-10) |
| Comida | enum (Desayuno/Almuerzo/Cena/Colacion) | Sí | Nunca obligatoria (RF-03) |
| CreadoEn | datetime | No | Momento real de creación, distinto de `Fecha` — útil para auditoría, no para cálculo |

**Justificación:** RF-01 a RF-19.

## 8. ItemDeRegistro

Entidad de dominio: `ItemDeRegistro` (documento 07, sección 3).

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| RegistroId | Guid (FK) | No | |
| AlimentoId | Guid (FK) | Sí | Nulo si el usuario no dio ningún dato (RF-19) |
| DescripcionLibre | string | Sí | Usado cuando no hay `Alimento` asociado, para que el registro siga siendo legible |
| Cantidad | decimal | Sí | |
| UnidadMedidaId | Guid (FK) | Sí | Referencia a `UnidadMedida` — no texto libre, evita variantes inconsistentes ("g", "gr", "gramos") que un campo de texto acumularía con el tiempo |
| FraccionAplicada | decimal | Sí | Ej. 0.5 para "la mitad del plato" (RF-16, RF-38). **Regla de exclusión mutua:** solo se usa cuando el `Alimento` referenciado es un plato/receta completa con rendimiento total (ej. la cazuela que rindió 6 porciones) — nunca junto con `Cantidad`+`UnidadMedida` para el mismo ítem. Una medida directa como "media taza de arroz" siempre se representa como `Cantidad=0.5, UnidadMedidaId=taza`, nunca como `Cantidad=1` con `FraccionAplicada=0.5` — evita que la misma cantidad tenga dos representaciones válidas distintas. |
| NivelEstimacion | enum (1Preciso/2Generico/3TipoComida/4Autoestimado/SinDatos) | No | Jerarquía completa del documento 04 |
| ValorCaloriasManual | decimal | Sí | Usado en el caso "solo calorías, sin desglose" (RF-18) |
| CaloriasSnapshot | decimal | Sí | Copia del valor de `Alimento` al momento del registro, ya escalada a la cantidad — ver ADR-011 |
| ProteinaSnapshot | decimal | Sí | Ídem |
| CarbohidratosSnapshot | decimal | Sí | Ídem |
| GrasaSnapshot | decimal | Sí | Ídem |
| FibraSnapshot | decimal | Sí | Ídem |

**Justificación:** RF-08 a RF-19 (jerarquía de estimación, autoestimación, sin datos), RF-16 (fracción de un plato).

## 9. Objetivo

Entidad de dominio: `Objetivo` (documento 07, sección 5).

| Columna | Tipo | Nullable | Nota |
|---|---|---|---|
| Id | Guid | No | |
| UsuarioId | Guid (FK) | No | |
| Nutriente | enum (Proteina/Calorias/Carbohidratos/Grasa/Fibra/Variedad) | No | |
| Tipo | enum (Piso/Techo/Variedad) | No | Solo tres valores — "sin objetivo" es ausencia de filas, no un valor |
| Valor | decimal | Sí | Nulo si Tipo = Variedad (no tiene un número asociado) |

**Restricción de integridad recomendada:** único por (`UsuarioId`, `Nutriente`, `Tipo`) — evita que un usuario tenga dos objetivos "piso" simultáneos para la misma proteína, por ejemplo.

**Justificación:** RF-20, RF-21, RF-23.

## 10. Comportamiento ante eliminación (DELETE)

| Acción | Comportamiento |
|---|---|
| Eliminar `ApplicationUser` (RF-34, cuenta completa) | CASCADE a `Registro` (y por herencia a `ItemDeRegistro`), `Objetivo`, y `Alimento` con `Origen = Personal` perteneciente a ese usuario. |
| Eliminar un `Registro` individual (RF-32, edición desde pantalla) | CASCADE a sus `ItemDeRegistro`. |
| Eliminar un `Alimento` público (`OpenFoodFacts`/`TablaCurada`) | No ocurre por acción directa del usuario — se gestiona por importación/curación, no expuesto en la interfaz. Si se intentara, RESTRICT si existen `ItemDeRegistro` que lo referencian. |
| Eliminar un `Alimento` personal desde el catálogo, con historial que lo referencia | Con el snapshot (ADR-011), ya no hay riesgo de corromper totales históricos — se puede permitir el borrado directo. `ItemDeRegistro` conserva `AlimentoId` como referencia nula o "alimento eliminado", sin afectar sus valores de snapshot ya guardados. Simplifica la decisión que antes requería RESTRICT o soft-delete. |

**Resuelto (ver ADR-011):** `ItemDeRegistro` guarda una copia (snapshot) de los valores nutricionales al momento del registro, ya escalados a la cantidad — los totales de un día pasado nunca cambian aunque `Alimento` se corrija después. `AlimentoId` se mantiene solo como referencia de trazabilidad, no como fuente del cálculo.

## 11. Índices anticipados (documentados, no implementados aún)

| Tabla | Índice | Motivo |
|---|---|---|
| `Registro` | (`UsuarioId`, `Fecha`) | La consulta más frecuente del sistema — progreso por usuario y rango de fechas |
| `Alimento` | `Nombre` | Búsqueda por nombre al registrar |
| `Alimento` | `CodigoExterno` | Resolución rápida contra caché de OpenFoodFacts |
| `Alimento` | `UsuarioPropietarioId` | Consulta del catálogo personal |

---

**Pregunta abierta para el documento 09 — resolución de alimentos:** cuando el usuario registra "dos huevos", ¿cómo se decide qué fila de `Alimento` usar si existen varias posibles coincidencias (ej. "Huevo de gallina" en la tabla curada vs. "Huevos Santa Marta XL" en OpenFoodFacts)? No es una decisión de esquema, es una regla de comportamiento del motor de búsqueda/resolución. Regla preliminar a desarrollar: un nombre genérico sin marca (ej. "huevo") debería preferir la tabla curada o el catálogo personal sobre una coincidencia específica de marca en OpenFoodFacts, ya que un término genérico no debería resolverse arbitrariamente a un producto de marca específica.

**Nota de trazabilidad:** todas las referencias de RF en este documento fueron verificadas contra el estado actual del documento 04 (no citadas de memoria) antes de darlo por cerrado.