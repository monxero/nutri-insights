# Documento 05 — Requerimientos no funcionales

**Versión:** 0.1 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

A diferencia del documento 04, estos requerimientos no tienen un disparador específico — son atributos de calidad que aplican transversalmente a todo el sistema. Se identifican con prefijo RNF para distinguirlos claramente de los RF.

## 1. Rendimiento

| ID | Requerimiento |
|---|---|
| RNF-01 | Una consulta o registro simple debe confirmarse en un tiempo percibido como conversacional (objetivo referencial: unos pocos segundos), reconociendo que parte de la latencia depende del proveedor de IA, fuera del control directo de la aplicación. |
| RNF-02 | Las operaciones que no dependen de IA (ver el panel día/semana/mes, editar un registro desde la pantalla, iniciar sesión) deben responder con la rapidez esperable de una aplicación web normal, sin depender de la disponibilidad del modelo de lenguaje. |

## 2. Disponibilidad y manejo de fallas externas

| ID | Requerimiento |
|---|---|
| RNF-03 | Si el proveedor de IA no responde o alcanza su límite de uso, el sistema debe informarlo con un mensaje claro y amable, sin fallar en silencio ni mostrar un error técnico crudo. |
| RNF-04 | Las funciones que no dependen de IA (panel, edición de registros, perfil, login) deben seguir operativas aunque el proveedor de IA esté caído — la app se degrada parcialmente, no por completo. |
| RNF-05 | Si la fuente de datos nutricionales externa no responde, el sistema debe recurrir a la tabla curada propia cuando el alimento esté ahí, e informar la limitación cuando no sea posible resolverlo con ninguna fuente disponible. |

## 3. Seguridad y privacidad

| ID | Requerimiento |
|---|---|
| RNF-06 | Las credenciales del usuario deben almacenarse de forma segura, nunca en texto plano. |
| RNF-07 | La comunicación entre el cliente y el servidor debe proteger la confidencialidad e integridad de los datos. |
| RNF-08 | Los datos de un usuario nunca deben ser visibles ni accesibles para otro usuario de la aplicación. |
| RNF-09 | El usuario debe poder saber, de forma simple y accesible, qué datos personales y de registro almacena la aplicación sobre él. |
| RNF-10 | Los datos del usuario no se comparten con terceros ni se usan con fines distintos al funcionamiento de la propia aplicación. |

## 4. Mantenibilidad

| ID | Requerimiento |
|---|---|
| RNF-11 | El sistema debe ser mantenible de forma que sea posible sustituir el proveedor de IA sin modificar la lógica de negocio ni los cálculos. |
| RNF-12 | Toda decisión de arquitectura relevante debe quedar registrada como ADR (documento 11), con alternativas evaluadas y motivo de la elección. |

## 5. Escalabilidad

| ID | Requerimiento |
|---|---|
| RNF-13 | El modelo de datos debe soportar múltiples usuarios concurrentes sin cambios estructurales (ya garantizado por la decisión de autenticación real, documento 03). |
| RNF-14 | La interfaz debe poder reutilizarse en futuras plataformas móviles nativas, minimizando la necesidad de reescribirla. |
| RNF-15 | El costo operativo del uso de IA debe mantenerse sostenible a medida que crece la base de usuarios. |

## 6. Usabilidad y accesibilidad

| ID | Requerimiento |
|---|---|
| RNF-16 | La interfaz debe ser responsiva y usable desde un navegador móvil desde el primer día (ver documento 03) — no requiere una app nativa para ser utilizable en un teléfono. |
| RNF-17 | El tiempo y esfuerzo necesarios para registrar una comida deben mantenerse consistentemente bajos — es la métrica de usabilidad central del proyecto (documento 01, documento 02), no una preferencia estética. |

## 7. Tono y calidad conversacional

| ID | Requerimiento |
|---|---|
| RNF-18 | Ninguna respuesta debe incluir alabanza exagerada ni culpa — el tono es neutro e informativo en toda interacción, sin excepción. *(Requerimiento reubicado desde el documento 04, ver nota ahí.)* |
| RNF-19 | El sistema nunca debe aparentar una certeza que no posee, en ninguna respuesta, sin excepción — es la aplicación transversal del principio de honestidad (documento 00) a la calidad de cada interacción, no solo a los registros individuales. |

## 8. Estrategia de calidad y testing

| ID | Requerimiento |
|---|---|
| RNF-20 | El motor de cálculo determinista (necesidades nutricionales, agregación de macros, rangos) debe cubrirse con pruebas unitarias — es la parte del sistema donde un error es menos tolerable, por ser la fuente de verdad numérica. |
| RNF-21 | La capa de interpretación de lenguaje natural debe evaluarse con un set de pruebas de regresión compuesto por mensajes reales, no sintéticos — candidatos ya identificados durante este proceso: registro retroactivo de una semana con comidas múltiples (diálogo 16b), texto mal escrito (diálogo 30), mezcla de idiomas (diálogo 31), receta de varias porciones (diálogo 38), y el mensaje real de brunch + almuerzo que originó los diálogos 33-40. Este set debe volver a correrse cada vez que cambie el prompt o el modelo de IA usado. |
| RNF-22 | Pruebas de interfaz, carga, y accesibilidad automatizada quedan fuera del MVP — se documenta la decisión explícitamente para no fingir una cobertura de calidad que no existe. |

---

**Nota de trazabilidad:** los IDs de este documento (RNF-01 a RNF-22) se referencian igual que los RF del documento 04 — cualquier decisión de arquitectura (documento 06) que afecte rendimiento, seguridad, disponibilidad o mantenibilidad debe poder señalar qué RNF motiva esa decisión.

**Nota de alcance:** este documento no nombra tecnologías específicas (HTTPS, Identity, Blazor, proveedor de IA, fuente de datos externa) — esas decisiones y su justificación viven en el documento 06 (arquitectura) y en los ADR del documento 11. Cada RNF de aquí debería seguir siendo cierto sin importar qué tecnología concreta se elija para cumplirlo.