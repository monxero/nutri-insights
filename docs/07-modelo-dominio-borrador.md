# Documento 07 — Modelo de dominio

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Este documento describe los conceptos del sistema en lenguaje de dominio — qué es cada cosa y cómo se relaciona con las demás — sin bajar todavía a tablas ni tipos de columna (eso es el documento 08). Cada entidad debe poder justificarse contra uno o más RF del documento 04.

## Mapa conceptual de entidades

```
                        Usuario
                 ┌─────────┼─────────┐
                 ↓         ↓         ↓
            Registro   Objetivo   Alimento (con
           (0 o más)  (0 o más)   origen = personal)
                 ↓                     ↓
             ItemDeRegistro ───→ Alimento ←───┘
```

Un `Usuario` tiene muchos `Registro` y cero o más `Objetivo`. Cada `Registro` contiene uno o más `ItemDeRegistro`, y cada ítem referencia un `Alimento`. El "catálogo personal" no es una entidad separada — es una consulta sobre `Alimento` filtrada por usuario propietario (ver sección 3).

**La IA no es una entidad del dominio.** Es un servicio externo utilizado por el Backend para transformar lenguaje natural en datos estructurados (documento 06) — no aparece en este modelo por la misma razón que ninguna tecnología aparece en el documento 04 o 05: el dominio debe seguir siendo el mismo si mañana cambia el proveedor de IA.

## 1. Usuario

Representa a una persona con cuenta propia en la aplicación.

- **Atributos principales:** identidad de cuenta (gestionada por Identity, documento 06), perfil (peso, estatura, sexo, edad, nivel de actividad — todos opcionales, completitud progresiva).
- **Por qué estos atributos y no otros:** no es una lista arbitraria — son exactamente los datos que exige la fórmula de cálculo elegida (Mifflin-St Jeor, la ecuación estándar basada en evidencia para gasto energético, que requiere peso, estatura, edad y sexo) más el nivel de actividad para el factor de ajuste. Si en el documento 06/08 se incorpora una fórmula adicional que necesite un dato distinto, esta lista se amplía en consecuencia — el criterio es siempre "qué necesita el cálculo", no una lista fija de antemano.
- **Relaciones:** tiene muchos `Registro`, tiene cero o más `Objetivo`, puede tener `Alimento` propios (con origen "personal").
- **Justificación:** RF-30 (cuenta propia), RF-31 (perfil progresivo), RNF-08 (aislamiento de datos entre usuarios).

## 2. Registro

Representa un evento de "algo que el usuario comió", ubicado en el tiempo. Un registro puede contener varios alimentos a la vez (ej. "pollo con arroz" es un registro con dos ítems).

- **Atributos principales:** fecha, comida (opcional: desayuno/almuerzo/cena/colación — nunca obligatoria).
- **Relaciones:** pertenece a un `Usuario`, contiene uno o más `ItemDeRegistro`.
- **Justificación:** RF-01 a RF-19 (todo el registro conversacional), RF-16 (fracción de un plato).

## 3. ItemDeRegistro

Representa un alimento específico dentro de un registro, con identidad propia — no es una simple lista, porque cada ítem tiene sus propios atributos independientes de los demás ítems del mismo registro.

- **Atributos principales:** el `Alimento` referenciado, cantidad, unidad, nivel de certeza de la estimación (nivel 1 a 4 de la jerarquía definida en el documento 04) o valor autoestimado por el usuario, fracción aplicada si corresponde (RF-16, RF-38), o "sin datos" si el usuario declinó dar cualquier número (RF-19).
- **Relaciones:** pertenece a un `Registro`, referencia un `Alimento`.
- **Justificación:** RF-08 a RF-19 (niveles de estimación, fracciones, autoestimación).

## 4. Alimento

Representa "qué es" algo comestible — desde un ingrediente simple (pollo, arroz) hasta un plato compuesto (cazuela de ave) o un producto envasado específico (galletas Nik).

- **Dos aspectos distintos, útil tenerlos presentes aunque hoy convivan en la misma tabla:** la **identidad** del alimento (su nombre, qué es) y su **información nutricional** (valores por 100g o por porción). Hoy se describen juntos por simplicidad; el documento 08 evaluará si conviene separarlos en tablas distintas o mantenerlos unidos.
- **Atributos principales:** nombre, categoría principal (ver sección 5), origen de los datos, usuario propietario opcional (ausente si es de origen público), valores nutricionales de referencia.
- **Tres orígenes posibles**, coherentes con el documento 06:
  1. **OpenFoodFacts** — productos envasados con marca, consultados en vivo, sin propietario (públicos).
  2. **Tabla curada propia** — alimentos genéricos, con semilla de USDA FoodData Central + curación regional, sin propietario (públicos).
  3. **Catálogo personal** — no es una entidad separada. Es un `Alimento` que puede tener opcionalmente un usuario propietario, visible solo para ese usuario. "Ver mi catálogo personal" es una consulta (`WHERE UsuarioPropietario = X`), no un objeto del dominio aparte.
- **Relaciones:** puede estar referenciado por muchos `ItemDeRegistro`, puede tener un `Usuario` propietario.
- **Justificación:** RF-12 (jerarquía de niveles de estimación), RF-17 (catálogo personal), documento 06 (arquitectura híbrida de datos).

## 5. Objetivo

Representa una meta nutricional definida por el usuario.

- **Atributos principales:** nutriente al que aplica (proteína, calorías, etc.), tipo (piso / techo / variedad), valor.
- **Relaciones:** pertenece a un `Usuario`; un usuario puede tener cero, uno, o varios objetivos simultáneos, de tipos distintos. "Sin objetivo" no es un valor almacenado — es simplemente la ausencia de filas de `Objetivo` para ese usuario.
- **Justificación:** RF-20 (colección de objetivos tipados), RF-21 (definición asistida), RF-23 (cambiar objetivo por chat).

## 6. Categoría de alimento

Representa la agrupación usada para dos propósitos distintos: calcular el patrón de variedad semanal, y aplicar el diccionario de calificadores de cantidad correcto.

- **Ejemplos:** "proteína animal - carne roja", "proteína animal - pescado", "verdura", "carbohidrato cocido".
- **Precisión importante:** no toda clasificación es universal ni obvia (una pizza, ¿es carbohidrato, comida preparada, o comida rápida?). Por eso: cada `Alimento` tiene una **categoría principal utilizada por los algoritmos del sistema** — existe para un propósito concreto (variedad, calificadores), no pretende ser una taxonomía nutricional universal.
- **Relaciones:** cada `Alimento` referencia una categoría principal.
- **Justificación:** patrón de variedad semanal (documento 04, sección C), diccionario de calificadores por categoría (documento 04).

## 7. Decisión explícita: la conversación no es una entidad persistida

El chat es **efímero** para efectos de este proyecto: sirve para interpretar el mensaje del usuario y producir un `Registro` o un `Objetivo`; una vez hecho eso, la conversación en sí no es un hecho del dominio que necesite guardarse permanentemente. El contexto de corto plazo que sí se necesita durante una sesión activa (para resolver el "día de referencia", o para corregir "perdón, eran dos" dentro del mismo hilo) es un detalle de infraestructura de la capa de IA, no una entidad del dominio de nutrición. Si en el futuro se decide ofrecer historial de chat navegable como funcionalidad, ahí sí aparecería como entidad — hoy no forma parte del alcance.

---

**Pendiente de aclarar en el documento 08:** si `Alimento` (identidad) y su información nutricional se separan en dos tablas o se mantienen juntas — se decide con más criterio cuando se vea el esquema relacional completo.