# Diálogos de ejemplo — validación del contrato de IA (borrador)

> Este documento no es el contrato formal (documento 09). Es un ejercicio de diseño: valida los patrones de comportamiento con casos concretos antes de redactar el contrato definitivo. Cada diálogo señala qué patrón está probando y qué queda registrado en la base de datos.

---

## 1. Registro simple — ingrediente identificado con precisión (nivel 1 de estimación)

**Usuario:** Almorcé 1/4 taza de quinoa, un tarro de atún de 125g, 3 mandarinas, un plato de lechuga y una cerveza sin alcohol.

**IA:** Registrado. Con esto, tu almuerzo aporta aproximadamente 480-560 kcal y 30-36g de proteína — la mayor parte por el atún.

**Queda guardado:** cada ingrediente con su cantidad exacta y su cálculo de macros correspondiente, sin necesidad de calificadores ni preguntas adicionales.

---

## 2. Registro con calificador ambiguo — pregunta guiada con ancla concreta

**Usuario:** Comí poco arroz con posta.

**IA:** Registrado con posta. Para el arroz, ¿fue como 100g (una porción chica) o menos?

**Usuario:** No lo pesé, pero podrían ser 150g.

**IA:** Perfecto, actualizado. Con eso, este plato aporta aproximadamente [rango calculado con 150g de arroz + posta].

**Queda guardado:** el número que dio el usuario (150g), no una estimación del diccionario de calificadores — la pregunta guiada resolvió la ambigüedad directamente.

---

## 3. Registro con calificador ambiguo — usuario no da más detalle (diccionario como respaldo)

**Usuario:** Comí poca ensalada.

**IA:** Registrado. ¿Tienes una idea de cuánto, aunque sea aproximada?

**Usuario:** No sé, poca no más.

**IA:** Sin problema, lo dejo como una porción pequeña de verduras (10-30g aprox.).

**Queda guardado:** rango de la categoría "verduras de hoja, poco" (10-30g) — el diccionario actúa solo porque el usuario no pudo o no quiso dar un ancla.

---

## 4. Componente genérico reconocible pero no exacto (nivel 2 de estimación)

**Usuario:** Comí pollo con una salsa blanca y arroz.

**IA:** Registrado. El pollo y el arroz los puedo calcular con precisión si me dices la cantidad aproximada. La salsa la estimo como una salsa cremosa típica — aporta principalmente grasas, en un rango moderado, sin que sepa la receta exacta.

**Queda guardado:** pollo y arroz con su propio nivel de precisión según lo que el usuario responda; la salsa con un rango de la categoría "salsas cremosas", marcada con confianza baja.

---

## 5. Plato no identificable, estimado por tipo de comida (nivel 3)

**Usuario:** Almorcé un guiso que hizo mi abuela, no sé bien qué llevaba.

**IA:** Con esa descripción no puedo identificar los ingredientes, así que lo registro como un almuerzo típico: aproximadamente 300-600 kcal. Es una estimación amplia — si en algún momento recuerdas algunos ingredientes, la puedo afinar.

**Queda guardado:** rango por tipo de comida ("almuerzo"), marcado explícitamente con confianza baja — no se mezcla con estimaciones de nivel 1 o 2 en la misma escala de certeza.

---

## 6. Información insuficiente incluso para el nivel 3 (nivel 4 — autoestimación del usuario)

**Usuario:** Comí algo por ahí, no fue mucho.

**IA:** Con esa información no puedo estimar nada por mi cuenta. ¿Te gustaría darme un número aproximado tú mismo, aunque sea a ojo?

**Usuario:** Tal vez unas 300 calorías.

**IA:** Registrado como una estimación tuya: 300 kcal aproximadas, sin desglose de macronutrientes.

**Queda guardado:** el valor autoestimado por el usuario, marcado explícitamente como tal (distinto de un cálculo del sistema).

---

## 7. Consulta de progreso con objetivos múltiples + observación incidental + patrón semanal

*(Usuario con dos objetivos activos: piso de proteína ≥100g/día, techo de calorías por déficit)*

**Usuario:** ¿Cómo voy hoy?

**IA:** Con lo registrado hoy llevas aproximadamente 80-100g de proteína y tus calorías rondan 1800-2200. Vas bien con la proteína, y dentro de tu rango de déficit. La fibra del día fue baja, aunque no es algo que estés siguiendo como objetivo. Esta semana, en cambio, llevas buena variedad de verduras.

**Nota de diseño:** tres tipos de información en una sola respuesta — progreso evaluado contra objetivos explícitos (proteína, calorías), observación neutra sin juicio (fibra, sin objetivo asociado), y patrón de variedad semanal (categorías distintas, no ligado a un número).

---

## 8. Pregunta fuera de dominio — médica

**Usuario:** Tengo diabetes, ¿qué debería comer?

**IA:** Puedo ayudarte a entender el contenido nutricional de los alimentos y a registrar tus hábitos alimentarios, pero no puedo entregar recomendaciones para tratar una condición de salud. Si tu consulta está relacionada con el manejo de la diabetes, lo más recomendable es conversarlo con un médico o un nutricionista.

**Nota de diseño:** patrón ya validado — nunca una negativa en seco, siempre con qué sí puede hacer y redirección.

---

## 9. Pregunta fuera de dominio — no médica (función distinta, no nutricional)

**Usuario:** Recuérdame comer hamburguesa todos los jueves.

**IA:** Eso no es algo que pueda hacer — no manejo recordatorios ni notificaciones, solo registro y análisis de lo que comes. Si quieres, puedo ayudarte a llevar un registro de tus hamburguesas de los jueves para que veas cómo encajan en tu semana.

**Nota de diseño:** el límite de dominio no es solo sobre salud — también aplica a funciones que la app simplemente no ofrece.

---

## 10. Alternativas de fuentes de un nutriente

**Usuario:** ¿Qué me da proteína?

**IA:** Pescado, carnes, huevos y atún son fuentes comunes de proteína.

**Usuario:** El atún y las carnes me salen caros, ¿algo más económico?

**IA:** Huevos, legumbres (lentejas, garbanzos, porotos) y pechuga de pollo suelen ser opciones más económicas. ¿Quieres que te diga cuánta proteína aporta cada una?

**Nota de diseño:** sin perfil de restricciones guardado — el usuario simplemente vuelve a preguntar y la IA responde en el momento.

---

## 11. Consulta semanal con datos incompletos (manejo no punitivo)

**Usuario:** ¿Cómo estuvo mi semana en verduras?

**IA:** No tengo registros para el jueves ni el viernes, así que esa parte de la semana no puedo evaluarla. Con lo que sí anotaste: lechuga el lunes y martes, tomate el miércoles. Podrías variar con algo distinto esta semana, como zanahoria o espinaca.

**Nota de diseño:** el vacío se reconoce sin culpa, se usa lo disponible, no se bloquea la respuesta. Frase ajustada para que no suene a reclamo ("no tengo registros para..." en vez de "no registraste...").

---

## 12. Referencia vaga sin memoria conversacional inventada

**Usuario:** Hoy comí lo de siempre.

**IA:** No sé a qué te refieres con "lo de siempre" — no guardo eso automáticamente. ¿Me describes qué comiste?

**Nota de diseño:** la IA nunca usa memoria conversacional para inventar un registro a partir de una referencia implícita. (Idea de roadmap, no MVP: permitir que el usuario defina explícitamente "comidas frecuentes" guardadas, para que este tipo de referencia funcione de forma segura en el futuro.)

---

## 13. Corrección dentro del mismo hilo activo

**Usuario:** Comí una marraqueta.

**IA:** Listo, quedó guardado.

**Usuario:** Perdón, eran dos.

**IA:** Corregido, quedaron registradas las dos marraquetas.

**Nota de diseño:** corrección de algo dicho en los últimos mensajes de la conversación activa — se modifica el registro, no se duplica. Distinto de corregir un registro de días atrás (eso sigue siendo CRUD de pantalla, no conversacional).

---

## 13b. Intento de corregir un registro antiguo por chat (fuera del hilo activo)

**Usuario:** Oye, la semana pasada anoté mal el almuerzo del martes, en realidad no era pollo, era pescado.

**IA:** Eso no lo puedo corregir directamente desde el chat — solo puedo ajustar cosas dichas en la conversación actual. Para editar un registro de otro día, puedes ir a tu historial y modificarlo ahí directamente.

**Nota de diseño:** mismo tono amable y sin fricción que el resto de los límites de la app, pero la corrección de registros pasados no es una capacidad conversacional — se resuelve con la función de editar/eliminar desde la pantalla de historial (CRUD simple), no por chat.

---

## 14. El usuario cambia de opinión a mitad de conversación

**Usuario:** Comí pollo.

**IA:** ¿Qué cantidad aproximada?

**Usuario:** Mejor olvídalo.

**IA:** Sin problema, no quedó nada registrado.

**Nota de diseño:** cancelación elegante, sin insistir ni preguntar por qué.

---

## 15. Consulta que exige exactitud imposible

**Usuario:** ¿Cuánta proteína llevo exactamente hoy?

**IA:** No puedo darte una cifra exacta porque algunos registros de hoy fueron aproximados. Con lo disponible, tu consumo estimado está entre 95 y 115g.

**Nota de diseño:** refuerza la filosofía de incertidumbre incluso cuando el usuario pide precisión directamente.

---

## 16. Mensaje que mezcla varios días

**Usuario:** El lunes comí pollo con arroz, el martes solo una ensalada, y hoy en el desayuno tomé café con pan.

**IA:** Registrado: lunes (pollo con arroz), martes (ensalada), hoy (café con pan de desayuno).

**Nota de diseño:** caso de prueba real para la capa de parsing — requiere interpretación temporal correcta, no solo extracción de alimentos. Candidato para el set de casos de prueba de la sección de testing (documento 05).

---

## 16b. Registro retroactivo de una semana completa, con comidas múltiples por día y vacíos explícitos

**Usuario:** El lunes desayuné [x,y,z], almorcé [a,b,c], cené [y,g,j]. El martes no recuerdo qué desayuné, almorcé [a,b,c], cené [x,y,x]. El miércoles no recuerdo nada. El jueves [algo, algo, algo]. Y hoy zapallitos.

**IA:** Registrado lo que compartiste — lunes completo (desayuno, almuerzo y cena), martes con almuerzo y cena (sin desayuno), jueves y hoy con lo indicado. No tengo nada para el miércoles ni para el desayuno del martes — no pasa nada, la semana queda evaluada con lo disponible.

**Nota de diseño — dos reglas que expone este caso:**
1. **"No recuerdo" no dispara pregunta guiada ni autoestimación (nivel 4).** El usuario ya declaró explícitamente que no tiene el dato — insistir violaría el principio de mínima intervención. Se acepta el vacío igual que un día sin registrar, pero a nivel de una comida individual, no solo de día completo.
2. **El modelo de datos es una lista de registros por día, cada uno con una etiqueta de comida OPCIONAL** (desayuno/almuerzo/cena/colación) — no una jerarquía obligatoria día→comida→alimentos. Un alimento comido fuera de una comida reconocible (ej. un paquete de papas fritas a media tarde) se guarda igual, sin categoría forzada, y suma al total del día de todas formas. Corrección sobre una versión anterior de esta nota, que proponía la comida como contenedor obligatorio — no lo es.

Este mensaje (con datos genéricos en vez de [x,y,z]) es un buen candidato real para el set de pruebas de regresión del parsing — no por la cantidad de ítems (eso es normal y esperable), sino para validar que el modelo atribuye correctamente cada ítem a su día, incluso en mensajes largos.

---

## 17. Preguntas educativas generales, dentro de dominio

**Usuario:** ¿Qué alimentos tienen proteína?

**IA:** [responde con fuentes generales]

**Usuario:** ¿Qué es un macronutriente?

**IA:** [responde con definición general]

**Usuario:** ¿Qué diferencia hay entre proteína animal y vegetal?

**IA:** [responde con diferencia general, sin recomendaciones médicas]

**Nota de diseño:** todo esto sigue dentro del dominio de macronutrientes. Distinto de preguntas sobre interacciones/absorción de nutrientes específicos (ej. combinar legumbres con vitamina C), que se acercan más a consejo específico y quedan fuera.

---

## 18. Plato con nombre conocido pero receta variable

**Usuario:** Comí una ensalada César.

**IA:** Puedo registrarla como una ensalada César típica, pero su composición varía bastante según quién la prepare. Si conoces algún ingrediente principal o el tamaño de la porción, puedo afinar la estimación.

**Nota de diseño:** caso límite entre nivel 1 y nivel 2 — nombre reconocible, composición no fija.

---

## 19. La IA decide NO preguntar (la pregunta no vale la fricción)

**Usuario:** Comí una manzana.

**IA:** Listo, quedó guardada.

**Nota de diseño:** no pregunta variedad ni peso — la variación en calorías de una manzana es pequeña, no justifica interrumpir. Valida el principio unificador: solo se pregunta cuando la respuesta cambiaría significativamente el rango calculado.

---

## 20. Dato irrelevante que no amerita conversación

**Usuario:** Al arroz le puse una pizca de orégano.

**IA:** Anotado.

**Nota de diseño:** se registra sin iniciar una conversación — el dato no modifica de forma perceptible el cálculo.

---

## 21. Consulta sin intención de registrar

**Usuario:** Hoy no quiero registrar nada. ¿Cómo voy con la proteína normalmente?

**IA:** Según tu historial de las últimas semanas, sueles rondar entre 85 y 110g diarios.

**Nota de diseño:** registrar y consultar son acciones independientes — no se obliga a registrar antes de poder consultar.

---

## 22. El caso que resume la filosofía completa

**Usuario:** No recuerdo exactamente qué comí.

**IA:** No pasa nada. Podemos registrar una estimación aproximada. Mientras más detalles recuerdes, más precisos serán los resultados, pero un registro incompleto sigue siendo útil.

**Nota de diseño:** no castigar, no bloquear, seguir adelante — resume el principio central del proyecto.

---

## 23. Registro y consulta en el mismo mensaje

**Usuario:** Hoy desayuné dos huevos y una tostada. ¿Cómo voy de proteína?

**IA:** Registrado el desayuno. Con eso, llevas aproximadamente 15-18g de proteína en lo que va del día.

**Nota de diseño:** no son acciones separadas — la IA registra primero y responde la consulta usando el total ya actualizado, en un solo mensaje.

---

## 24. Mensaje duplicado accidental

**Usuario:** Almorcé pollo con arroz.

*(cinco segundos después, mismo contenido)*

**Usuario:** Almorcé pollo con arroz.

**IA:** Parece que este mensaje se repitió — ¿fueron dos porciones separadas, o se envió dos veces por error?

**Nota de diseño:** mismo mecanismo de pregunta guiada que cualquier otra ambigüedad, disparado por un mensaje idéntico en una ventana de tiempo muy corta.

---

## 25. Agregar un registro olvidado (no corregir)

**Usuario:** Hoy almorcé pollo con arroz.

*(más tarde)*

**Usuario:** Ah, se me olvidó decir que desayuné avena con plátano.

**IA:** Agregado el desayuno — avena con plátano. El almuerzo queda igual.

**Nota de diseño:** "se me olvidó decir" es señal de agregar un registro nuevo, no de modificar el existente — distinto del caso 13 (corrección del mismo ítem).

---

## 26. Completar un registro con más detalle (no reemplazar)

**Usuario:** Almorcé pollo.

*(más tarde)*

**Usuario:** Ah, también comí arroz.

**IA:** Agregado el arroz al almuerzo.

**Nota de diseño:** "también" señala adición al mismo registro, no reemplazo — mismo principio que el caso 25, con otra señal de lenguaje.

---

## 27. Cambiar un objetivo por chat

**Usuario:** Desde mañana quiero apuntar a 120g de proteína en vez de 100.

**IA:** Listo, actualicé tu objetivo de proteína a 120g diarios a partir de mañana.

**Nota de diseño:** los objetivos, a diferencia del perfil (formulario), sí se gestionan conversacionalmente — es una extracción estructurada simple (nutriente, tipo, valor), igual que registrar comida.

---

## 28. Consulta sobre una estadística que no existe

**Usuario:** ¿Cuál fue mi mejor martes?

**IA:** No tengo una forma de determinar cuál fue tu "mejor" día — no evalúo tus días como buenos o malos. Sí puedo mostrarte cómo estuvo tu consumo de proteína cada martes de este mes, si te sirve.

**Nota de diseño:** refuerza la honestidad aplicada a consultas, no solo a registros — nunca se inventa una respuesta que suene completa cuando no existe el dato o el cálculo.

---

## 29. Dato absurdo con sugerencia de corrección probable

**Usuario:** Comí 80 huevos.

**IA:** Ese número parece inusualmente alto — ¿quisiste decir 8?

**Nota de diseño:** refinamiento del chequeo de cordura (caso "40kg de pollo") — cuando hay una corrección obvia y probable (error de tipeo), se sugiere directamente en vez de solo señalar que el valor es raro.

---

## 30. Texto mal escrito

**Usuario:** komi arro cn pollo i una coca

**IA:** Registrado: arroz con pollo y una Coca-Cola.

**Nota de diseño:** exactamente el tipo de tolerancia que justifica usar un LLM en vez de un parser rígido de reglas. Candidato directo para el set de pruebas de regresión.

---

## 31. Mezcla de idiomas

**Usuario:** Comí chicken con rice.

**IA:** Registrado: pollo con arroz.

**Nota de diseño:** no debería romper el parsing — mismo principio de tolerancia que el caso anterior.

---

## 32. Día de referencia interrumpido por un tema ajeno

**Usuario:** El lunes comí pollo con arroz.

**IA:** Registrado.

**Usuario:** Oye, ¿qué es un macronutriente?

**IA:** [responde la pregunta general]

**Usuario:** Ah, también comí una manzana.

**IA:** ¿Esa manzana fue el lunes también, o es de hoy?

**Nota de diseño:** valida la distinción entre ambigüedad de cantidad (tolerante, casi nunca amerita pregunta) y ambigüedad de atribución (día/comida/registro — umbral bajo, casi siempre amerita pregunta). Aunque el impacto nutricional de una manzana es mínimo, ubicarla en el día equivocado corrompería silenciosamente los datos de ambos días. El tema ajeno en el medio (la pregunta sobre macronutrientes) resetea la continuidad del "día de referencia".

---

## 33. Fracción de un plato completo (compartido o no)

**Usuario:** Fui a un brunch, pedí una tostada de salmón, palta, crema ácida y rúcula (venía sobre 2 tostadas), la compartí con otra persona que pidió una de huevo, tocino y tomate cherry — comí una tostada de cada una.

**IA:** Registrado: una tostada de tu pedido (salmón, palta, crema ácida, rúcula) y una tostada del pedido de tu acompañante (huevo, tocino, tomate cherry) — la mitad de cada plato, ya que cada uno venía en dos tostadas.

**Nota de diseño:** el usuario no describió "medio plato" de forma literal — el sistema tuvo que inferir la fracción a partir de "venía sobre 2 tostadas" + "comí una de cada una". Válido tanto para comida compartida como para un plato propio del que se come solo una fracción (ej. "comí 1/4 de un pastel de papas").

---

## 34. Agregar un plato nuevo al catálogo personal por conversación guiada

**Usuario:** Comí un plato de cazuela.

**IA:** No tengo la cazuela en mis registros — ¿quieres que la agreguemos? Cuéntame qué lleva.

**Usuario:** Es un caldo de pollo con papas, choclo y zapallo.

**IA:** ¿Aproximadamente cuánta papa tiene el plato?

*(la IA sigue preguntando lo mínimo necesario hasta poder estimar macros del plato completo)*

**IA:** Listo, guardé "cazuela" con esa composición — la próxima vez que la registres, ya no hace falta describirla de nuevo.

**Nota de diseño:** capacidad distinta al nivel 3 de estimación (que es un cálculo puntual, no reutilizable). Aquí el plato queda guardado en el catálogo personal del usuario bajo ese nombre.

---

## 35. Declinar el registro guiado, con estimación de solo calorías

**Usuario:** Comí un plato de cazuela.

**IA:** No tengo la cazuela en mis registros — ¿quieres que la agreguemos? Cuéntame qué lleva.

**Usuario:** No quiero ingresarlo, pero estimo unas 500 calorías.

**IA:** Entendido, registré 500 kcal aproximadas para ese plato. No queda desglose de proteína, carbohidratos ni grasas para este registro.

**Nota de diseño:** variante de nivel 4 (autoestimación del usuario) que acepta un valor de solo calorías, sin forzar un desglose completo de macronutrientes — el resto de las consultas del día simplemente no tienen ese desglose para este ítem.

---

## 36. Plato sin ningún dato numérico — advertencia en consultas de totales

**Usuario:** Comí un plato de cazuela.

**IA:** No tengo la cazuela en mis registros — ¿quieres que la agreguemos? Cuéntame qué lleva.

**Usuario:** No, déjalo así no más.

**IA:** Listo, quedó anotado que comiste cazuela hoy, sin datos nutricionales asociados.

*(más tarde en la conversación)*

**Usuario:** ¿Cuántas calorías llevo hoy?

**IA:** Llevas aproximadamente 1800-2200 kcal registradas. Además comiste un plato del que no tengo ningún dato numérico, así que el total real podría ser mayor — no puedo estimar cuánto.

**Nota de diseño:** distinto del nivel 4 (autoestimación) y del caso 35 (solo calorías) — aquí no hay ningún número que agregar. El sistema no inventa un valor por defecto, pero tampoco oculta que el total está incompleto: advertir la falta de datos es tan importante como comunicar un rango amplio, solo que aplicado a nivel de totales en vez de a nivel de un ítem individual.

---

## 37. Información completa entregada de una vez (sin preguntas de relleno)

**Usuario:** Comí un plato de cazuela.

**IA:** No tengo la cazuela en mis registros — ¿quieres que la agreguemos? Cuéntame qué lleva.

**Usuario:** *(pega una descripción completa, por ejemplo copiada de otro LLM)* "Cazuela chilena tradicional: caldo de pollo con presa de pollo, papa entera, choclo, zapallo, zanahoria y arroz o fideos, porción típica de 400-450g."

**IA:** Listo, guardé "cazuela" con esa composición — la próxima vez que la registres, ya no hace falta describirla de nuevo.

**Nota de diseño:** el sistema no vuelve a preguntar papa/choclo/zapallo por separado, porque la información ya llegó completa en un solo mensaje. El origen del texto (escrito por el usuario o copiado de otra fuente) no afecta el tratamiento — se procesa igual que cualquier descripción directa, sin verificarla ni cuestionarla.

---

## 38. Receta de varias porciones — evitar atribuir toda la olla a un registro

**Usuario:** Comí un plato de cazuela.

**IA:** No tengo la cazuela en mis registros — ¿quieres que la agreguemos? Cuéntame qué lleva.

**Usuario:** *(pega una receta)* "Cazuela de ave para 6 personas: 1 pollo entero, 6 papas, 2 zapallos, 6 trozos de choclo, arroz."

**IA:** Esa receta rinde para 6 personas — ¿qué parte comiste tú, aproximadamente una sexta parte, o otra cantidad?

**Usuario:** Sí, como una sexta parte.

**IA:** Listo, guardé "cazuela de ave" calculando tu porción como una sexta parte de esa receta.

**Nota de diseño:** distinto de RF-16 (fracción de un plato ya servido) — aquí la fracción aplica sobre el rendimiento total de una receta, no sobre una porción individual ya preparada. Sin esta pregunta, el sistema atribuiría el total de la olla a un solo registro, un error de magnitud, no de precisión menor.

---

## 39. Discrepancia con un plato ya guardado

**Usuario:** Comí cazuela, pero esta vez de vacuno.

**IA:** Guardaste "cazuela" antes con pollo — como esta es de vacuno, cambia bastante la proteína y la grasa. ¿Quieres guardarla como una entrada distinta, por ejemplo "cazuela de vacuno"?

**Usuario:** Sí, guárdala aparte.

**IA:** Listo, guardé "cazuela de vacuno" como entrada nueva. La anterior sigue disponible como "cazuela" — si quieres, puedo renombrarla a "cazuela de ave" para que quede más claro.

**Nota de diseño — rama alternativa (libre albedrío del usuario):** si en vez de aceptar el usuario responde "no, da lo mismo, trátala igual", el sistema registra bajo "cazuela" tal como está guardada, sin insistir ni forzar la diferenciación.

---

## 40. Crear un plato nuevo por referencia a uno existente

**Usuario:** Agregué la cazuela de vacuno — es como la cazuela de ave, pero con vacuno en vez de pollo.

**IA:** Entendido, la guardo como "cazuela de vacuno" con los mismos ingredientes que la de ave, cambiando el pollo por vacuno y ajustando la proteína y grasa según ese cambio.

**Nota de diseño:** el sistema copia la composición del plato referenciado en vez de partir de cero, y solo recalcula el nutriente afectado por el ingrediente reemplazado.

---

## 41. Producto envasado no encontrado — corrección de nombre o entrada manual

**Usuario:** Comí un paquete de galletas Nick.

**IA:** No encontré "galletas Nick" — ¿está bien escrito el nombre, o es de otra marca?

**Usuario:** Ah, es Nik, sin la "c".

**IA:** Listo, la encontré — registrado.

**Nota de diseño:** el mismo mecanismo de pregunta guiada (RF-08) resuelve la mayoría de los casos de nombre mal escrito, sin necesitar búsqueda difusa contra la API externa.

---

## 41b. Producto genuinamente no encontrado — entrada manual desde la etiqueta

**Usuario:** Comí un paquete de galletas Tost Chile.

**IA:** No encontré ese producto en ninguna fuente — ¿tienes el envase a mano? Si me pasas las calorías, proteína y carbohidratos que aparecen en la etiqueta, lo registro igual.

**Usuario:** Sí, dice 140 calorías, 2g de proteína, 18g de carbohidratos por porción de 25g.

**IA:** Registrado, y guardé "galletas Tost Chile" en tu catálogo personal — la próxima vez que la registres, ya no hace falta que vuelvas a leer la etiqueta.

**Nota de diseño:** no se requiere la ficha completa de OpenFoodFacts para que el registro sea útil — los datos básicos de la etiqueta bastan, coherente con la jerarquía de niveles de estimación. Esta es exactamente la misma capacidad que RF-17 (agregar un plato nuevo al catálogo), aplicada a un producto envasado en vez de una preparación casera — mismo mecanismo, sin distinción entre los dos casos. Si el usuario no quiere revisar el envase, sigue disponible el nivel 4 (autoestimación aproximada, sin guardarse como reutilizable).

---

## 42. Educación sobre cómo interpretar una etiqueta nutricional

**Usuario:** Estoy viendo el paquete y no entiendo por qué la porción dice 30g si el paquete completo tiene 150g.

**IA:** Es normal — los fabricantes definen una "porción" como la cantidad que consideran una ración típica, que casi nunca es el paquete entero. Todos los valores de la etiqueta (calorías, proteína, etc.) están calculados para esos 30g, no para el paquete completo. Si te comiste el paquete entero, hay que multiplicar esos valores por 5 (150g ÷ 30g).

**Nota de diseño:** información educativa general dentro del dominio, no un análisis del registro específico del usuario salvo que ya lo haya registrado — conecta directamente con el flujo de entrada manual desde etiqueta (diálogo 41b), donde es más probable que surja esta duda.

---

## Variación de confirmaciones (aplicado en los ejemplos de arriba)

Para no sonar robótico repitiendo "Registrado" en cada respuesta: alternar entre "Listo, quedó guardado", "Registrado", "Anotado", "Corregido", "Actualizado", según el contexto de cada diálogo.

## Pendiente de agregar

- Ninguno por ahora. El registro de perfil se resolvió como formulario simple (no conversacional), así que no requiere un diálogo de ejemplo aquí — ver `CONTEXTO.md`.