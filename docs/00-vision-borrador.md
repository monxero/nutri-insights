# Documento 00 — Visión del producto

**Versión:** 0.3 (borrador) · **Estado:** En revisión · **Última actualización:** 19-07-2026

> Si alguien leyera únicamente este documento, debería entender perfectamente qué es esta aplicación, por qué existe y qué límites tiene deliberadamente.

## 1. Qué es este proyecto, en una frase

Este proyecto pertenece al dominio de **educación nutricional y seguimiento de hábitos alimentarios**. Es una aplicación que ayuda a las personas a mejorar sus hábitos alimentarios mediante un registro conversacional simple y un análisis honesto de la información disponible.

La tecnología con la que se construye —.NET, Blazor, un modelo de lenguaje— es una decisión de implementación, no la identidad del producto. Si en cinco años se reemplaza el modelo de IA por otro, o incluso se elimina la IA por completo, la descripción de arriba debería seguir siendo válida. Esa es la prueba que cualquier decisión de arquitectura debe pasar: **el dominio es permanente, la tecnología no.**

## 2. ¿Qué problema existe?

Cumplir un objetivo nutricional simple —por ejemplo, consumir suficiente proteína en el día— es sorprendentemente difícil sin conocimiento previo. Y el problema empieza antes de lo que parece: muchas personas ni siquiera saben cuál debería ser su objetivo, porque no comprenden para qué sirve cada macronutriente ni cómo se relaciona con su situación (por ejemplo, empezar a entrenar). Para quienes sí tienen un objetivo claro, tampoco es fácil saber cuánto aporta lo que comieron — descubrirlo por su cuenta implica estudiar, calcular y buscar en internet cada vez.

Las aplicaciones de seguimiento nutricional existentes resuelven esto exigiendo un nivel de precisión que la mayoría de las personas no está dispuesta a sostener en el tiempo: pesar cada alimento, registrar cada ingrediente, contar cada caloría. Ese nivel de exigencia funciona para un grupo reducido de usuarios muy comprometidos, pero es precisamente la razón por la que la mayoría abandona estas aplicaciones a las pocas semanas.

## 3. ¿Por qué existe este problema?

Porque la mayoría de las herramientas actuales parten de una premisa equivocada: que la precisión perfecta es un requisito para que la información sea útil. En la práctica, esa exigencia genera fricción, y la fricción genera abandono. Una persona que deja de registrar porque le resultó tedioso no solo pierde la herramienta — vuelve exactamente al mismo punto de desconocimiento del que partió.

## 4. ¿Por qué queremos resolverlo?

Este proyecto nace de una necesidad personal real y concreta: alcanzar un consumo diario de proteína suficiente, en un contexto de entrenamiento físico, sin la carga mental de contar y pesar cada comida. Al investigar esto, quedó claro que el problema no es exclusivo de una necesidad personal — es un problema compartido por cualquier persona con una preocupación nutricional mínima que no está dispuesta a convertirse en experta en nutrición para resolverla.

El objetivo no es competir con aplicaciones clínicas ni con herramientas para atletas de alto rendimiento. Es construir algo que funcione para el resto: personas con objetivos simples y reales, que valoran más la constancia que la precisión perfecta.

## 5. Usuario objetivo

Una persona interesada en mejorar sus hábitos alimentarios, con uno o más objetivos concretos (por ejemplo, un mínimo de proteína diaria, o un techo calórico), que **no** quiere ni necesita llevar un control nutricional exhaustivo.

Esto excluye deliberadamente del alcance a personas con necesidades médicas específicas (diabetes, condiciones renales, trastornos alimentarios, etc.) — no porque no les pueda servir información general, sino porque para esos casos la aplicación redirige explícitamente a un profesional de la salud, en vez de intentar cubrir esa necesidad.

## 6. Qué NO queremos hacer

- No reemplazar a un nutricionista ni a un médico.
- No entregar diagnósticos ni tratamientos de ninguna condición de salud.
- No exigir precisión perfecta como condición para que el registro sea útil (desarrollado en la sección 7).
- No ocultar ni aparentar certeza que no existe (desarrollado en la sección 8, principio de honestidad).
- No presionar, castigar ni bloquear por falta de registro (desarrollado en la sección 8).
- No convertirse en un asistente de propósito general: fuera del dominio de macronutrientes, hábitos alimentarios y educación nutricional básica, la aplicación declina amablemente y redirige (ya sea a un profesional de la salud, o simplemente explicando que esa función no existe).

## 7. Filosofía del producto

La aplicación está diseñada para ayudar al usuario a comprender y mejorar sus hábitos alimentarios sin exigir un registro perfecto ni una precisión imposible. El registro puede ser tan simple o tan detallado como el usuario desee, y nunca se rechaza por incompleto (ver principio de honestidad en la sección siguiente).

**Aprender antes que registrar.** El registro no es el producto — es la materia prima. El valor real que recibe el usuario son las respuestas e ideas que se derivan de lo registrado ("hoy llevas entre 90 y 120g de proteína", "esta semana te ha faltado variedad en tus fuentes de proteína"), no la lista de comidas en sí. Por eso la experiencia principal de la aplicación se construye alrededor de respuestas e insights, no de una tabla de registros.

**Definición asistida de objetivos.** Un usuario que no tiene claro cuál debería ser su objetivo nutricional no debería quedar sin poder usar la aplicación por eso — puede decírselo directamente, y la aplicación lo ayuda a llegar a un número concreto y razonable, que el usuario adopta como propio. La aplicación sugiere; nunca impone un objetivo. (El mecanismo específico de cómo se resuelve esto se define en el documento de requerimientos funcionales, no aquí.)

## 8. Principios de diseño

**Honestidad sobre la incertidumbre.** La aplicación nunca aparentará conocer información que no posee, ni ocultará la incertidumbre de una estimación. Toda estimación se comunica como lo que es — un rango, no una cifra falsa con precisión inventada. Un registro más específico reduce el rango; uno más vago lo amplía, pero nunca se rechaza. **Qué significa "precisión" en este proyecto:** no se busca exactitud científica, sino la mejor estimación posible con la información disponible en cada momento. El objetivo no es obtener números exactos, sino construir hábitos sostenibles.

**Mínima intervención, con un complemento activo.** La aplicación nunca interrumpe innecesariamente al usuario — solo pregunta cuando la respuesta cambiaría de forma significativa la estimación, no por preguntar. Pero cuando existe una forma simple de mejorar mucho la calidad de un dato con una sola pregunta bien dirigida, la aplicación sí la hace. No son principios opuestos: mínima intervención evita la fricción innecesaria; el complemento activo evita que "no molestar" se convierta en excusa para estimaciones peores de lo necesario.

**El usuario siempre decide.** La aplicación informa, explica y propone — nunca decide, nunca juzga, nunca felicita de forma exagerada, nunca culpa. La interpretación de qué hacer con la información entregada es siempre responsabilidad del usuario.

**El usuario es responsable de sus propios datos.** La aplicación analiza lo que se le entrega; no cuestiona ni juzga las decisiones alimentarias del usuario. Esto no significa aceptar cualquier valor sin ningún filtro: un dato claramente fuera de todo rango humano plausible (ej. "comí 40kg de pollo") dispara la misma pregunta guiada que cualquier otra ambigüedad — no por juicio nutricional, sino por plausibilidad básica del dato.

**No presionar, no bloquear, no castigar.** La ausencia de registro no es un error ni una falla del usuario — es información. Días sin registrar se reconocen sin culpa, y la aplicación sigue funcionando con lo que sí tiene disponible.

**La aplicación no busca maximizar el tiempo de uso del usuario. Busca maximizar el valor obtenido cada vez que decide utilizarla.** Es una diferencia deliberada frente a la mayoría de aplicaciones actuales, que sí optimizan por tiempo en pantalla o frecuencia de uso.

**El proyecto prioriza soluciones simples y robustas antes que funcionalidades numerosas.** Ante la duda entre agregar una función más o fortalecer lo que ya existe, se prioriza lo segundo.

## 8b. Qué es un hábito, para efectos de este proyecto

Un hábito alimentario es un patrón repetido de decisiones relacionadas con la alimentación que puede observarse a partir del historial registrado, sin necesidad de juzgarlo como bueno o malo. Esta definición es la base sobre la que se construyen las estadísticas y los patrones de variedad descritos más adelante en el proyecto.

## 9. Rol de la Inteligencia Artificial

La IA no constituye el núcleo del sistema. Toda la lógica de negocio pertenece completamente a la aplicación desarrollada en .NET. La IA actúa únicamente como una interfaz inteligente entre el lenguaje natural del usuario y el modelo de datos de la aplicación.

Sus responsabilidades son exclusivamente:

- interpretar registros y consultas escritas en lenguaje natural;
- solicitar aclaraciones puntuales cuando la información sea insuficiente y esa aclaración realmente cambie el resultado;
- explicar conceptos generales de nutrición dentro del dominio de macronutrientes;
- sugerir alternativas alimentarias acordes al contexto entregado por la aplicación;
- redactar respuestas claras, naturales y con tono neutro (sin culpa ni alabanza exagerada).

La IA nunca debe:

- inventar datos inexistentes ni completar información faltante sin decirlo;
- calcular necesidades nutricionales, estadísticas o cualquier resultado que deba ser determinista — eso siempre lo hace la aplicación, usando fórmulas y tablas de nutrición conocidas y citables;
- tomar decisiones de negocio;
- realizar diagnósticos médicos ni elaborar tratamientos;
- reemplazar a un profesional de la salud;
- usar memoria conversacional para inventar un registro a partir de una referencia vaga ("lo de siempre").

Toda decisión importante la toma la aplicación. La IA interpreta; la aplicación decide.

## 10. Filosofía técnica

La aplicación debe seguir un comportamiento determinista: ante la misma información registrada, siempre debe producir el mismo resultado. La base de datos constituye la única fuente de verdad respecto a la información registrada por el usuario. Todo cálculo nutricional, estadístico o histórico se realiza mediante la lógica implementada en .NET — nunca generado por el modelo de lenguaje.

(Nota: el determinismo aplica a los datos y cálculos que entran a la base de datos, no a la redacción exacta de las respuestas conversacionales — un modelo de lenguaje puede variar la fraseología de una respuesta sin que eso comprometa la consistencia de los datos subyacentes.)

## 11. Qué significa éxito

No existe una única forma de éxito para esta aplicación, porque cada usuario tiene una relación distinta con ella. Un usuario que la usa durante años porque le sigue siendo útil no representa un fracaso del producto — todo lo contrario. Un usuario que aprende rápido, internaliza el conocimiento nutricional básico y con el tiempo la necesita cada vez menos, tampoco es un fracaso — es la otra cara exacta de la misma filosofía ("aprender antes que registrar").

El éxito no se mide en número de registros ni en retención. Se mide en si la información que la aplicación entrega es real, honesta, y le ahorra al usuario el tiempo y la carga mental que hoy le exige entender su propia alimentación — sin importar si eso significa que la use toda la vida o que algún día ya no la necesite.

Existe también una tercera trayectoria posible, igual de válida: un usuario que, al notar que datos más precisos producen respuestas y estadísticas más útiles, empieza por iniciativa propia a pesar y medir con más cuidado lo que registra. Ese usuario termina pareciéndose a alguien que usa una aplicación rigurosa tradicional — pero llegó ahí motivado por el valor que vio, no porque la aplicación se lo exigiera desde el primer día. Esa es precisamente la diferencia con el problema descrito en la sección 3: esta aplicación no rechaza la precisión, simplemente no la exige como condición de entrada.

## 12. Alcance general

La aplicación permite registrar comidas mediante lenguaje natural, consultarlas, obtener estadísticas simples (día/semana/mes), visualizar tendencias básicas y recibir educación nutricional general, enfocada en los cuatro macronutrientes y calorías. No pretende reemplazar aplicaciones clínicas ni sistemas de seguimiento médico.