//reduce sirve para acumular.

//objetos con categorias y precios
const items = [
  { name: 'apple', category: 'fruit', price: 1.2 },
  { name: 'banana', category: 'fruit', price: 0.8 },
  { name: 'carrot', category: 'vegetable', price: 0.5 }
];

const totalPrice = items.reduce((accumulator, item) => {
  return accumulator + item.price;
})
const grupedByCategory = items.reduce((accumulator, item) => {
  accumulator[item.category] = accumulator[item.category] || [];
  accumulator[item.category].push(item);
  return accumulator;
}, {});




//Diferencia entre var const y let





//Se debe utilizar === mas que == ya que formza el tipado por valor. 






//Closures.
//ejemplo : 
//Una función que recuerda variables aunque la función padre ya terminó.
function createCounter() {
  let count = 0;
  const counter = () => {
    count++;
    return count;
  }

  return counter;
}

const myCounter = createCounter(); // crea una instancia de counter
console.log(myCounter()); // 1
console.log(myCounter()); // 2
console.log(myCounter()); // 3



//Cuando usar closures?
// 3️⃣ Cuándo usar closures conscientemente(casos reales)
// ✅ Caso 1: Estado privado
function createUserSession(userId) {
  let lastActivity = Date.now();

  return {
    touch() {
      lastActivity = Date.now();
    },
    isExpired() {
      return Date.now() - lastActivity > 300000;
    }
  };
}


// 📌 Ideal para:

// Auth

// Cache

// Session handling

// ✅ Caso 2: Factory de funciones
function createLogger(prefix) {
  return function (message) {
    console.log(`[${prefix}] ${message}`);
  };
}

const apiLogger = createLogger('API');
const uiLogger = createLogger('UI');

apiLogger('Request sent');
uiLogger('Button clicked');

// ✅ Caso 3: Callbacks async(MUY común)
function fetchWithRetry(url, retries) {
  return function retry() {
    fetch(url).catch(() => {
      if (retries > 0) {
        retries--;
        retry();
      }
    });
  };
}

// 4️⃣ Closures y MEMORIA(aquí está lo importante)

// 👉 El closure mantiene referencias vivas
// 👉 Si referencias cosas grandes → memory leak

// ❌ Ejemplo PELIGROSO(memory leak)
function createHandler() {
  const hugeData = new Array(1_000_000).fill('*');

  return function () {
    console.log(hugeData.length);
  };
}

const handler = createHandler();
// hugeData nunca se libera ❌


// 📌 Aunque solo uses.length, toda la memoria queda retenida.

// ✅ Solución: minimizar lo capturado
function createHandler() {
  const size = 1_000_000;

  return function () {
    console.log(size);
  };
}

// 🧹 Buenas prácticas con closures
// ✔ Captura SOLO lo necesario
// ✔ Evita closures en loops largos
// ✔ Limpia referencias cuando ya no se usen
// ✔ No cierres sobre objetos enormes
// ✔ En React: cuidado con closures + useEffect
// ⚠️ Bug típico en React(closure stale)
useEffect(() => {
  setInterval(() => {
    console.log(count);
  }, 1000);
}, []);


// ❌ count queda congelado

// ✅ Solución
useEffect(() => {
  const id = setInterval(() => {
    setCount(c => c + 1);
  }, 1000);

  return () => clearInterval(id);
}, []);

// 5️⃣ Cómo NO abusar de closures(regla senior)

// ❌ No usarlos para todo
// ❌ No reemplazar clases innecesariamente
// ❌ No ocultar lógica compleja







// 1️⃣ Diferencia conceptual entre ambos enfoques
// 🟦 Opción A — Closure(factory de funciones)
function createLogger(prefix) {
  return function (message) {
    console.log(`[${prefix}] ${message}`);
  };
}

// Qué está pasando realmente

// createLogger crea un scope

// prefix queda capturado en un closure

// Cada llamada genera una función distinta con su propio estado

const apiLogger = createLogger('API');
const uiLogger = createLogger('UI');


// 📌 Memoria:

// apiLogger → closure → prefix = "API"
// uiLogger  → closure → prefix = "UI"

// 🟨 Opción B — Función directa(sin closure)
function createLoggerDirect(prefix, message) {
  console.log(`[${prefix}] ${message}`);
}

// Qué pasa aquí

// No se crea ningún closure persistente

// Cada llamada:

// recibe argumentos

// ejecuta

// se libera

createLoggerDirect('API', 'Request sent');


// 📌 Memoria:

// call stack → ejecuta → libera

// 2️⃣ Diferencia CLAVE(lo que evalúan en entrevistas)
// Aspecto	             Closure	Función directa
// Estado persistente	   ✅ Sí	    ❌ No
// Configuración previa	 ✅ Sí	    ❌ No
// Reutilización	       Muy alta	 Media
// Overhead de memoria	 Mayor	   Mínimo


// Ideal para	Factories, DI, config	Llamadas simples
// 🎯 Ejemplo mental claro
// Closure

// “Configuro una vez, uso muchas”

const apiLogger2 = createLogger('API');
apiLogger2('User created');
apiLogger2('Request failed');

// Función directa

// “Paso todo cada vez”

createLoggerDirect('API', 'User created');
createLoggerDirect('API', 'Request failed');

// 3️⃣ ¿Cuál es mejor ?

// ❌ No hay una mejor universal
// ✅ Depende del caso

// Usa CLOSURE cuando:

// ✔ El prefijo es fijo
// ✔ Se reutiliza mucho
// ✔ Quieres encapsulación
// ✔ Evitas repetir parámetros

// Usa FUNCIÓN DIRECTA cuando:

// ✔ Llamadas aisladas
// ✔ No hay estado
// ✔ Lo usas poco
// ✔ Quieres cero overhead

// 📌 Respuesta senior:

// “Closure para configuración persistente, función directa para operaciones puntuales.”

// 4️⃣ Ahora lo importante: MEMORIA y LEAKS
// ❓ ¿Los closures causan memory leaks ?

// 👉 NO por sí solos
// 👉 El leak ocurre si siguen referenciados cuando ya no deberían

🧨 Caso peligroso(loops largos / listeners)
function attachLogger(element) {
  const bigData = new Array(1_000_000).fill('*');

  element.addEventListener('click', () => {
    console.log(bigData.length);
  });
}


// 🚨 PROBLEMA:

// El listener mantiene el closure vivo

// bigData nunca se libera

// Leak clásico en SPAs

// 5️⃣ ¿Cómo liberar memoria en closures ? (respuesta directa)
// ❌ ¿Asignar null funciona ?

// 👉 Sí, PERO solo si rompes TODAS las referencias

// ✅ Estrategia 1 — Eliminar el listener(la más correcta)
function attachLogger(element) {
  const bigData = new Array(1_000_000).fill('*');

  function handler() {
    console.log(bigData.length);
  }

  element.addEventListener('click', handler);

  return () => {
    element.removeEventListener('click', handler);
  };
}


// ✔ Rompes la referencia
// ✔ Closure elegible para GC
// ✔ Solución senior

⚠️ Estrategia 2 — Null explícito(útil pero limitada)
let handler = createLogger('API');

// cuando ya no se usa
handler = null;


// 📌 Funciona SOLO si:

// No hay otras referencias

// No está registrado en listeners / timers

// ❌ Estrategia INCORRECTA
// prefix = null; // ❌ no existe en ese scope


// No puedes limpiar una variable cerrada desde fuera.

// 🧹 Estrategia 3 — Minimizar lo capturado(mejor práctica)
function createLogger(prefix) {
  const tag = `[${prefix}]`;

  return function (message) {
    console.log(tag, message);
  };
}


// ✔ Capturas un string pequeño
// ✔ No objetos grandes

// ⏱️ Estrategia 4 — Limpiar timers
const interval = setInterval(logger, 1000);

// limpieza
clearInterval(interval);


// 🚨 Si no limpias → closure vive para siempre

// 6️⃣ Resumen claro(esto es oro para entrevistas)
// Diferencia esencial

// Closure: función + memoria

// Directa: función + stack

// Memory leaks NO vienen de closures

// 👉 vienen de referencias activas

// Regla senior

// “Si lo registras, lo limpias.”