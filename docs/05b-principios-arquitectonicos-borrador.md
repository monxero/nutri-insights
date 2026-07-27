# Principios arquitectónicos

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

Este documento no toma decisiones — los ADR (documento 11) hacen eso. Reúne las ideas de ingeniería que aparecen una y otra vez, con distintas palabras, a lo largo de los documentos 04 a 11. Sirve como puente entre la filosofía de producto (documento 00, que trata cómo la app se comporta con el usuario) y el detalle técnico que sigue — cada principio de aquí se puede ver aplicado en decisiones concretas señaladas más abajo.

## 1. El comportamiento debe ser determinista siempre que sea posible

Ante la misma información, el sistema produce siempre el mismo resultado. No es una preferencia de estilo — es lo que permite pruebas unitarias confiables (RNF-20) y lo que motivó guardar valores ya calculados en vez de fórmulas pendientes de resolver (ADR-011).

**Se ve en:** ADR-001, ADR-011, RNF-11, RNF-20.

## 2. La IA interpreta; el backend decide

Ningún cálculo, ninguna decisión de negocio, sale de la capa de interpretación de lenguaje natural. La IA transforma texto en datos estructurados; todo lo demás — cálculo, persistencia, reglas — vive en código determinista que no depende de ningún modelo de lenguaje.

**Se ve en:** ADR-001, ADR-012, documento 00 sección 9, documento 06 bloque 2.

## 3. Los datos históricos nunca cambian silenciosamente

Un registro pasado refleja lo que se sabía en ese momento — no se recalcula si la fuente de datos mejora después. Un dato antiguo con más incertidumbre no es un dato "malo", es honesto sobre su propio momento.

**Se ve en:** ADR-011 (snapshot de valores), RF-19 (advertir en vez de recalcular con datos faltantes).

## 4. La honestidad tiene prioridad sobre la completitud

El sistema prefiere decir "no sé" o "esto es un rango amplio" antes que aparentar una certeza que no posee. Aplica igual a un alimento sin datos completos, a una consulta sin suficiente historial, o a una respuesta que la IA no puede fundamentar.

**Se ve en:** RNF-19, RF-19, RF-27, documento 00 principio de honestidad.

## 5. El dominio es independiente del proveedor tecnológico

Cualquier decisión de negocio o de comportamiento debe seguir siendo cierta aunque cambie la tecnología específica que la implementa — el proveedor de IA, el motor de base de datos, el framework de interfaz. Es la prueba que se aplicó en el documento 00 ("¿sigue siendo cierto si cambia la tecnología?") y que después se generalizó a los documentos 01 a 09.

**Se ve en:** ADR-012, documento 00 sección 1, documento 01 (prueba equivalente para el problema).

## 6. La simplicidad prevalece hasta que exista una necesidad real de complejidad

Ninguna estructura se normaliza, separa, o generaliza "por si acaso" — se mantiene simple hasta que un caso de uso concreto exige lo contrario, y esa exigencia queda documentada explícitamente como decisión, no como descuido.

**Se ve en:** ADR-008 (Alimento como una sola tabla), documento 08 (nutrientes como columnas fijas, no tabla normalizada), documento 03 (priorizar soluciones simples y robustas).

## 7. Extensibilidad vía datos, no vía código

Cuando un conjunto de valores puede crecer con el tiempo sin cambiar ninguna lógica de negocio, se modela como datos consultables (una tabla), no como algo fijo en el código (un enum) — agregar un valor nuevo no debería exigir recompilar ni desplegar la aplicación.

**Se ve en:** ADR-013, `CategoriaAlimento`, `UnidadMedida`.

---

**Nota de uso:** cuando una decisión nueva parezca no encajar en ningún ADR existente, conviene revisar primero si contradice alguno de estos principios. Los principios representan el criterio general; los ADR documentan cómo ese criterio se aplicó en decisiones concretas.