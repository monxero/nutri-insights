# Contexto del proyecto — App de hábitos alimentarios

> Última actualización: 17 de julio de 2026. Este documento resume el estado actual del proyecto para que cualquier conversación nueva pueda retomarlo sin perder continuidad. Léase junto a `REGLAS-DE-TRABAJO.md`.

## 1. Qué es el proyecto

Aplicación web en .NET (Blazor como interfaz principal, stack no cerrado más allá de eso) para registrar alimentación mediante lenguaje natural, comprender hábitos y aprender nutrición básica — sin reemplazar a un profesional de la salud. Ver visión completa (pendiente de redactar como Documento 00, contenido ya definido en conversación).

Contexto adicional: el usuario está postulando a una oferta de Desarrollador Junior de Sistemas (Blazor, .NET, DevExpress, SQL) con cierre a fin de mes. El proyecto sirve como pieza de portafolio, pero también como uso personal real (registro de consumo de proteína) y como ejercicio de aprendizaje de desarrollo profesional de principio a fin.

## 2. Metodología documental acordada

Orden de documentos (propuesto por otro LLM, ajustado en conversación):

| # | Documento | Estado |
|---|---|---|
| 00 | Visión del producto | Pendiente — contenido ya definido, falta redactar |
| 01 | Problema | Pendiente |
| 02 | Estado del arte | Pendiente |
| 03 | Alcance MVP | Pendiente |
| 04 | Requerimientos funcionales | Pendiente |
| 05 | Requerimientos no funcionales (incluye sección de seguridad/privacidad y sección de estrategia de calidad/testing) | Pendiente |
| 06 | Arquitectura (incluye decisión de fuente de datos nutricionales y decisión sobre DevExpress) | Pendiente |
| 07 | Modelo de dominio | Pendiente |
| 08 | Modelo de datos | Pendiente |
| 09 | Contrato de integración de IA | Pendiente — precedido por ejercicio de diálogos de ejemplo |
| 10 | Roadmap | Pendiente |
| 11 | Registro de decisiones arquitectónicas (ADRs) | Pendiente |

**Priorización de plazos:** documentos 00-05 antes del lunes 20 de julio. Documentos 06-11 sin fecha rígida, se maduran con calma porque ahí vive la mayoría de la deuda técnica del proyecto.

**Paso intermedio antes del documento 09:** ejercicio de diálogos de ejemplo (usuario ↔ IA ↔ dato guardado) para validar el contrato de IA con casos concretos antes de formalizarlo. Pendiente de realizar.

## 3. Decisiones ya tomadas

- **Separación de responsabilidades:** la IA interpreta lenguaje natural, la aplicación .NET decide y calcula. Ningún cálculo nutricional o estadístico se delega a la IA. Comportamiento determinista: mismo dato registrado → mismo resultado.
- **Incertidumbre como principio central:** los registros se guardan con grado de confiabilidad/certeza. Un registro impreciso ("comí un poco de pollo") es válido y se usa igual, pero la respuesta refleja la incertidumbre como rango (ej. "90-120g de proteína"), nunca como número falso con precisión inventada.
- **Ausencia de registro no es error:** si el usuario no registró en varios días, el sistema no bloquea ni presiona — responde con lo que sí tiene, indicando explícitamente qué falta.
- **Ante ambigüedad, la recomendación favorece el objetivo del usuario** (ej. usar el valor más bajo del rango si el objetivo es asegurar consumo mínimo de proteína), no el promedio estadístico neutro.
- **Negativas de la IA nunca son en seco:** cuando la IA no puede responder algo (ej. pregunta médica/diagnóstica), siempre acompaña la negativa con: (1) qué sí puede hacer, (2) por qué no puede responder eso, (3) redirección a profesional competente si aplica. Ejemplo de referencia ya validado (caso diabetes) — ver historial de conversación.
- **Testing:** no se crea documento aparte. Se incluye como sección corta dentro del documento 05, cubriendo unit tests sobre el motor de cálculo determinista y tests de contrato sobre la interfaz IA↔aplicación.
- **DevExpress:** no descartado ni confirmado. El proyecto no es comercial, así que no hay bloqueo de licencia real. Se evaluará explícitamente en el documento 06 junto a alternativas gratuitas (MudBlazor, Radzen, Blazor puro).
- **"Aprender" antes que "registrar":** el registro es un medio, no el fin. La pantalla principal de la app debe priorizar respuestas/insights ("hoy llevas 75-95g de proteína") sobre una tabla de comidas registradas. Implicancia directa para el documento 04 (requerimientos funcionales) y para el documento 00.
- **Los registros son materia prima, no el producto.** El valor percibido por el usuario son las respuestas derivadas del registro, no el registro en sí. Afecta el diseño del modelo de dominio (documento 07).
- **Prueba de independencia de proveedor de IA:** la descripción del producto debe seguir siendo válida aunque se reemplace el modelo de IA usado. Sirve como criterio de validación para cualquier decisión de arquitectura que involucre la capa de IA — si una decisión ata el producto a un proveedor específico, está mal diseñada.
- **Fuente de datos nutricionales:** tendencia hacia arquitectura híbrida — OpenFoodFacts para productos envasados con marca (buena cobertura, sin costo, sin auth para lectura, pero de confiabilidad variable por ser crowdsourced y centrado en productos con código de barras) + tabla propia curada para alimentos genéricos/crudos (pollo, palta, huevo, etc.), que OFF no cubre bien por diseño. Decisión formal pendiente para el documento 06.

## 4. Reglas de trabajo activas

Ver `REGLAS-DE-TRABAJO.md` — resumen: mensajes largos que se responden en orden de lectura, no asumir estado de tecnologías sin verificar, corrección mutua explicada, priorizar calidad de proceso sobre velocidad.

## 5. Próximo paso

Ejercicio de diálogos de ejemplo (usuario ↔ IA) para validar el contrato de IA antes de redactarlo formalmente en el documento 09.
