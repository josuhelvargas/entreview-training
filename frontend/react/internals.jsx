                        //Mapa mental rápido (qué es cada cosa)

// 1.JSX: azúcar sintáctico. Babel/TypeScript lo transforma a llamadas React.createElement(...).
// 2.React Element: un objeto plano (inmutable) que describe qué quieres ver: { type, props, key }. No es el DOM.
// 3.Fiber: la estructura interna de React (un nodo por cada elemento renderizado) que permite partir el trabajo, priorizar, y recordar estado/effects.
// 4.Reconciliation (diffing): algoritmo que compara elementos nuevos vs. anteriores para decidir qué cambiar.
// 5.Renderer: la “capa” que sabe pintar (DOM en web, nativo en ReactNative). Para web es react-dom.
// 6.Scheduler: decide cuándo trabajar (prioridades, pausas, batching). Clave en React 18 (concurrencia).



            //0.Como funciona Babel?
                //Babel es un compilador de JavaScript.
                // Su trabajo principal es convertir código moderno (ES6+, ES2020+, etc.) a código JavaScript más antiguo para que los navegadores que no soportan nuevas características puedan ejecutarlo.


// 1.Creacion de el elemento react 
<Button onclick={onBuy()}> Comprar</Button>
React.createElement('Button', {onClick:onBuy}, 'Comprar'); //objeto inmutable

// 2.Creacion de un arbol de elemtos que se materializa en una arbol de Fibers.
//Cada Fiber tiene un type, pendingPorpps, memoizedProps, (hook state) child, sibling, return, (puntero)flags, (que hacer ?colocar, borrar,actualizar) alternate (versiona tnerior para comparar)


// 2.1 Fase Render (pura, sin tocar el DOM)
    // Se reconstruye el árbol de fibers “trabajado” comparando Elements nuevos vs. anteriores (reconciliation).
    // Se calcula qué cambiar (se marcan flags).
    // Puede pausarse y reanudarse (concurrent features).
    // No hay efectos secundarios del mundo real aquí (no DOM, no timers). Es “pura”.


// 2.2 Fase Commit (efectos reales)
    // React aplica los cambios marcados:
    // crea/mueve/elimina nodos del DOM,
    // adjunta listeners,
    // ejecuta useLayoutEffect (sync) y luego useEffect (async post-paint).
    // Es rápida y no se puede pausar: así se evita que el DOM quede a medias.



// 3) ¿Cómo decide React qué cambiar? (Reconciliation / Diff)
    // Regla de oro:
    // Si el tipo del elemento no cambió (p. ej. <button> sigue siendo <button> o el mismo componente <PriceCard>), React reutiliza el fiber y hace diff de props/hijos.
    // Si el tipo cambió, descarta ese subárbol y crea uno nuevo.
    // Listas y key
    // En listas, usa key para estabilidad de cada ítem.
    // Buena key: un id único y estable.
    // Mala key: el índice (provoca re-montajes y pérdida de estado al reordenar).
    // Con key, React hace el diff O(n) con heurísticas: mover, insertar, borrar lo mínimo.
    // Senior tip: una key incorrecta = bugs



//  4) Estado y actualizaciones (de dónde sale el re-render)
    // 4.1 setState / useState no muta: encola “updates”

    // Cada actualización se encola en la cola de updates del Fiber.                   
    // React re-renderiza usando las nuevas props/estado hasta estabilizar.
    // Batching: múltiples setState en el mismo evento se agrupan → un solo render/commit.

    // Funcional vs. valor
     setCount(c => c + 1); // funcional (usa c “actual” de la cola): más seguro en ráfagas
     setCount(count + 1);  // captura el valor “visto” en esta renderización

    // Para eventos rápidos o concurrentes, prefiere la forma funcional.
    // 4.2 Priorización y Lanes (React 18)
    // React asigna “carriles” (lanes) y prioridades a las updates.
    // Cosas urgentes (input del usuario) reciben prioridad mayor que trabajo pesado (carga de datos).
    //Si algo tarda, React puede interrumpir y mostrar una transición (startTransition).


// 5) Hooks (cómo “vive” el estado en un Fiber)
    // Cada Fiber de un componente funcional mantiene una lista enlazada de hooks en el orden en que se declaran.
    // En cada render, React itera esos hooks en el mismo orden.
    // Por eso las reglas de hooks: no llamarlos en condiciones/loops que cambien su orden.
    // Hooks clave
    // useState / useReducer: estado local.
    // useMemo / useCallback: memoización de valores/funciones derivados para evitar trabajo/re-renders.
    // useRef: referencia estable (no provoca render).
    // useEffect: efectos post-pintado (event listeners globales, fetches, timers). Limpieza al re-render/unmount.
    // useLayoutEffect: igual que useEffect pero sincrónico antes del pintado (medir layout, sincronizar scroll). Úsalo con cuidado (bloquea el paint).
    // useContext: lee valores del árbol de Context (propagación optimizada internamente).
    // useTransition: marca trabajo como no urgente para transiciones suaves.
    // useDeferredValue: difiere valores pesados cuando cambia un input.


//6) Eventos en React (Synthetic Events)

    //React crea una capa de eventos sintéticos que normaliza diferencias entre navegadores.
    //                    
    //Desde React 17+, el registro de eventos se hace en el root en vez de document, con un sistema de bubbling/capturing propio.
    //                    
    //Beneficios: compatibilidad y control. Coste: un poco de overhead (pero mínimo y muy probado).


//7) Concurrent Rendering (React 18)
    // No significa multihilo, sino que React puede interrumpir el trabajo de render para dejar respirar a la UI.
    // Con createRoot y APIs como startTransition, puedes:
    // mantener la UI responsiva (inputs no “se pegan”),
    // mostrar estados de transición (spinners, placeholders),
    // evitar bloquear el hilo por renders enormes.
    // Ejemplo:
    // import { startTransition } from 'react';

    onChange={(e) => {
      setQuery(e.target.value);                 // alta prioridad (input)
      startTransition(() => setFilter(query));  // baja prioridad (filtrar lista grande)
    }}


//8) Renderers y el DOM real
    //React en sí no “pinta”. react-dom traduce las flags del Fiber a operaciones DOM:
    //appendChild, removeChild, setAttribute, addEventListener, etc.
    //Otras plataformas: react-native, react-three-fiber, etc. Cada uno implementa su renderer.



// 9) Efectos: orden y limpieza
    // Orden típico en una actualización:
    // Commit DOM (aplica cambios).
    // Ejecuta useLayoutEffect (sincrónico). Si retornan limpieza, primero limpia el anterior.
    // Pintado del navegador.
    // Ejecuta useEffect (asíncrono microtask-ish). Si retornan limpieza, se ejecuta antes de la próxima vez que corra el mismo efecto o al unmount.
    // Senior tip: parpadeos/“jumps” suelen ser por trabajo visual en useEffect en lugar de useLayoutEffect.


//     10) Render vs. Re-render vs. Reconciliation
    // Renderizar: correr la función de tu componente para producir Elements.
    // Re-render: vuelve a correr por cambio de props/estado/context.
    // Reconciliation: comparar el nuevo árbol con el anterior para decidir qué DOM cambiar.
    // No todo re-render implica tocar el DOM; si el diff dice “nada cambió”, el commit es mínimo o nulo.



//     11) Rendimiento: lo que un senior vigila
    // Keys correctas en listas.
    // Evitar renders inútiles:
    // React.memo para componentes puros,
    // useMemo/useCallback para valores/handlers caros o que disparan renders en hijos memorizados.
    // Dividir trabajo:
    // startTransition,
    // división por rutas/secciones,
    // virtualización de listas.
    // Efectos mínimos: no meter lógica de negocio en effects; idempotentes; dependencias correctas.
    // Controlados vs. no controlados: inputs controlados pueden re-renderizar más; si no necesitas control fino, considera no controlados o técnicas híbridas.
    // Evitar mutaciones: React confía en referencias para saber si algo cambió. Mutar rompe el memo/diff.
    // Profiler: medir antes de optimizar.

// 12) Pequeño recorrido de “un clic” cambiando estado

    // Usuario clic → handler llama setState.
    // setState encola un update en el Fiber correspondiente.
    // Scheduler asigna prioridad y planifica trabajo.
    // Fase Render: React recorre el subárbol, ejecuta funciones de componentes y hooks, calcula cambios (flags).
    // Fase Commit: aplica cambios al DOM, ejecuta layout effects, pinta, ejecuta effects.
    // UI actualizada. Si hay más updates en cola (p. ej., un fetch que resolvió), repite.


//14) Donde encaja RHF (React Hook Form)

    // RHF no cambia cómo React funciona; lo usa a su favor:
    // guarda el estado del formulario en refs y proxies para no re-renderizar todo,
    // integra fácil con controlados (via Controller) y no controlados (input nativo),
    // se lleva de maravilla con Zod para validar antes del submit o onChange.
    // Patrón senior: UI controlada solo donde importa (p. ej., tu selector de “Extras”), inputs nativos no controlados cuando no necesitas re-renders.

// 15) Checklist mental de Senior

    // ¿Mis keys son estables?
    // ¿Estoy mutando estructuras (arrays/objetos) en lugar de crear nuevas copias?
    // ¿Dónde memoizar (de verdad) aporta? (memo, useMemo, useCallback)
    // ¿Puedo partir el trabajo con startTransition?
    // ¿Mis effects tienen dependencias correctas y limpian recursos?
    // ¿Estoy bloqueando el paint con trabajo pesado sin necesidad?
    // ¿Necesito useLayoutEffect o basta useEffect?
    // ¿Estoy tirando renders por props que cambian de referencia sin cambiar de valor?
    // ¿He medido con el Profiler o adivino?


//preguntas 
                                        // Como es que babel /typescript conveirte el jsx en un react element(un arbol de em,lemetos virtuales del DOM) ? 

    //Los tipos solo son durante desarrollo 
    //babel 
    //AL final React toma ese árbol de llamadas y crea el Fiber Tree y Virtual DOM interno.


                                                //como funcionan los fibers por dentro ? 
    //Una vez que se pinta el reacty elements tree se conviertne ane nodosl mutables con mucha metadata par apoder dividir el trabajo en trozos.
    //priorizar actualizaciones, recodar estado previo y el nuevo.
    //pausar , reanudar, interrumpir.

    // Un Fiber típico contiene:
    // tag: tipo de nodo (función, host <div>, texto, etc.).
    // type: el componente o tag (MyForm o 'div').
    // key: clave para reconciliar listas.
    // stateNode: el “nodo real” asociado (un DOM node para <div>, o la instancia para class components).
    // Estructura enlazada: child, sibling, return (padre).
    // Props/Estado:
    // pendingProps (las props nuevas que se intentan aplicar),
    // memoizedProps / memoizedState (las props/estado ya “commit-adas”).
    // Efectos/flags: qué hay que hacer en el commit (insertar, actualizar, borrar).
    // alternate: el truco clave: referencia a la “otra versión” del mismo fiber (el árbol work-in-progress vs el árbol actual). Esto permite el double buffering.
    
    // Piensa en dos árboles Fiber:
    // Árbol actual (en pantalla).
    // Árbol work-in-progress (el que React construye “en segundo plano” a partir de tus cambios).
    // Cuando acaba el render, intercambia (swap) las referencias: el WIP pasa a ser el actual.

    // 3) Las dos fases del render en React
    // Render (reconciliación):
    // React lee tu estado/props actuales + JSX → construye/actualiza el árbol work-in-progress (Fibers).
    // Calcula qué cambiar (flags: Placement, Update, Deletion…).
    // Esta fase puede ser interrumpida y retomada (Concurrent Mode).
    // No toca el DOM real todavía.
    // Commit:
    // React aplica los cambios al DOM real de forma rápida y atómica.
    // Llama a efectos (useLayoutEffect/useEffect) en el orden correcto.
    // Esta fase no es interrumpible; se hace de corrido.


    // 4) ¿Dónde entra el “estado”?
    // Al llamar setState/setX (hooks), React empuja una actualización en la cola del Fiber correspondiente (para function components, una lista enlazada de updates por hook).
    // El scheduler asigna prioridades (lanes) y dispara el render para recomputar el VDOM y el árbol work-in-progress.
    // Luego hace commit. Tu UI cambia.




        //¿Cómo decide React qué re-renderizar? (Diff/Reconciliación paso a paso)


            // 1) Comparación por tipo (nodo a nodo)

            // Cuando React compara el árbol “anteror” vs el “nuevo”:
            // Mismo type (p. ej., sigue siendo <button> o el mismo componente UserCard)
            // → Reutiliza el Fiber y el DOM node; actualiza props y sigue con los hijos.
            // Distinto type
            // → Descarta ese subárbol y crea uno nuevo (más costoso)!!!!

            //2) Listas y keys (regla de oro)

            //En listas, React alinea los elementos por key:
            //Si las keys coinciden entre renders, React mueve/actualiza lo necesario sin destruir todo.
            //Si faltan keys o cambian mal, React “asume” reordenes incorrectos y puede recrear nodos (perdes foco/estado local).


            //3) Bailout (saltarse renders)
                //React evita recalcular subárboles cuando puede:
                //Si el Fiber detecta que props y estado no cambiaron (por referencia), no baja a los hijos.
                //Con React.memo en function components, si props son iguales (shallow), salta el re-render del componente.
                //useMemo/useCallback ayudan a mantener identidades estables para no disparar renders en hijos dependientes.

            //4) Hooks y colas de updates
                // Cada useState mantiene una cola de actualizaciones. Al hacer setState, se encola un objeto {action, lane}.
                // En render, React reduce esa cola para obtener el nuevo estado. Si el resultado no cambia por igualdad referencial y las props tampoco, puede bail out.



                // 5) Prioridades y scheduler (Concurrent Rendering)
                    // React asigna lanes (prioridades). Interacciones de usuario tienen mayor prioridad que tareas en background.
                    // Si algo más urgente llega (ej. tecleas), React puede pausar un render en curso y reanudar luego.
                    // // Esto mejora la percepción de fluidez.


                // 6) Fases de efectos
                        // useLayoutEffect corre sincrónico después del commit (bloquea pintura si es largo). Úsalo para medir DOM o sincronizaciones que afecten layout.
                        // useEffect corre después de pintar (no bloquea UI). Úsalo para fetch, timers, subscripciones, etc.
                        // En re-renders, React compara dependencias; si no cambiaron, no re-ejecuta el efecto.







//Como funcionan los hooks por dentro ?
                    // 1) Cómo funcionan los Hooks por dentro
// Idea clave:

// Un componente funcional se vuelve a ejecutar completo en cada render.
// Entonces… ¿cómo React recuerda los valores anteriores del estado?

// Respuesta: React guarda el estado en el Fiber, no dentro de tu función.


// Fiber
//  ├─ hook #1 → { state, queue }
//  ├─ hook #2 → { ref }
//  └─ hook #3 → { effect, deps }

// React no ejecuta el efecto en el render.
// Lo agenda para la fase de commit.

// Cada hook useEffect guarda:

// hook = {
//   effectFn,
//   deps,
//   cleanupFn?
// }


// En commit:
// Si deps cambiaron → ejecuta cleanup() previo (si existe) → luego ejecuta effect().
// Si deps no cambiaron, no se ejecuta nada.


//que es eso de useRef? 

// Hook	            Guarda dato	            Cambiar el dato re-renderiza?	    Uso principal
// useState	        estado del componente	✅ Sí	                            UI dependiente del estado
// useRef	        valor persistente	    ❌ No	                            Guardar cosas fuera del flujo de UI (timers, nodos, contadores, caches, etc.)

//Como se lleva a cabo el algortimo de difgf para acutalizar el dom ? 
// Hooks funcionan porque React guarda su estado en un Fiber que mantiene una lista ordenada de hooks.

// useRef guarda valores sin provocar re-render.

// El algoritmo de diff compara elementos por tipo + keys para actualizar el DOM de forma mínima.



//muestrame el flujo en foma de acordeon del flujo funcional, que hace cada cosa, el hook ,el fiber, en que momenbtio entran un diagrama del sistema para entenderlo mejor y en que momento se conectan 

// TU CÓDIGO (JSX + useState + useEffect)
//         │
//         ▼
// TRANSFORMACIÓN (Babel/TypeScript)
//         │
//         ▼
// React Elements (Árbol Inmutable - Virtual DOM Descriptivo)
//         │
//         ▼
// Fiber Tree (Árbol Vivo con estado, hooks, efectos y prioridad)
//         │
//  ┌───────────────────────────Render Phase───────────────────────────┐
//  │ Reconcile (comparación árbol anterior vs nuevo - diff algorithm) │
//  └─────────────────────────────────────────────────────────────────┘
//         │
//         ▼
//  ┌───────────────────────────Commit Phase───────────────────────────┐
//  │ Cambios reales en el DOM + useLayoutEffect + useEffect           │
//  └─────────────────────────────────────────────────────────────────┘
//         │
//         ▼
// UI ACTUALIZADA EN PANTALLA




//                 RENDER PHASE (Pausable, No DOM)
// ────────────────────────────────────────────────────────
//  FiberTree (actual)      JSX Output (nuevo)
//        │                          │
//        └───────── compare (diff) ─┘
//                    │
//           produce Work-In-Progress Fiber Tree
//                    │
//                (listo para aplicar)
// ────────────────────────────────────────────────────────

//                 COMMIT PHASE (No Pausable)
// ────────────────────────────────────────────────────────
//    Work-In-Progress Fiber Tree
//                │
//      Aplicar flags al DOM real
//                │
//       Ejecutar useLayoutEffect
//                │
//            Pintar UI
//                │
//          Ejecutar useEffect
// ────────────────────────────────────────────────────────


// | Hook          | Se procesa en | Guarda información en               | Causa re-render            |
// | ------------- | ------------- | ----------------------------------- | -------------------------- |
// | `useState`    | Render Phase  | `hook.state`, `hook.queue` en Fiber | ✅ Sí                       |
// | `useEffect`   | Commit Phase  | `hook.effect`, `hook.deps`          | ✅ Sí (cuando deps cambian) |
// | `useRef`      | Render Phase  | `hook.current` en Fiber             | ❌ No                       |
// | `useMemo`     | Render Phase  | valor memoizado en hook             | ❌ No                       |
// | `useCallback` | Render Phase  | función memoizada en hook           | ❌ No                       |


// JSX describe UI →
// React Elements forman VDOM →
// Fiber Tree mantiene estado →
// Hooks viven dentro de cada Fiber →
// Diff decide el mínimo cambio →
// Commit actualiza DOM →
// Efectos se ejecutan →
// UI final se muestra


// Eficiencia se da porque: 
// Nunca vuelve a pintar todo

// Usa un árbol mutable interno (Fiber) para recordar estado/historial

// Divide el render en fases

// Solo cambia lo mínimo necesario en el DOM real


// un diagrama (ASCII) del ciclo Render/Commit para imprimir,
// un playground de ejemplos (inputs controlados vs. no controlados, keys, memo),
// una chuleta de hooks con cuándo usarlos,
// o aplicar esto a tu componente Extras (con transiciones, virtualización y RHF).