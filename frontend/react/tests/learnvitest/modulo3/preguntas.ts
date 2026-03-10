//npm install -D vitest
//npm run test -> se ejecuta el script "vitest"
// npm
//  ↓
// node_modules/.bin/vitest
//  ↓
// Vitest CLI
//  ↓
// Test Runner
//  ↓
// Vite module graph
//  ↓
// Execute tests


//package.json:
// {
//     "scripts":{
//         "test": "vitest",
//         "test:watch": "vitest --watch"
//     }
// }


//paso 1:
// //vitest.config.ts
// vite.config.ts
// package.json

//Despues de esto sfiles vitest sabe:
// • dónde están los tests
// • si usar globals
// • coverage
// • environment

//paso 2: escaneo de archivos .test.ts

//creacion del grafo de dependencias en lso test.Lo que permite : 
// • HMR
// • caching
// • dependency tracking



//Paso 3. Construccion del Module Graph

//Paso 4. Transformacion de codigo usando ES buildtypescript se covierte en js (muchoi mas rapido que babel con jest)
//paso 5 .Ejecucion de tests en multipoles woreker  threads lo qu ecausa ejecucion paralela
//paso 6.Assertions vitest utiliza una libreira de assertions compatibvle con Jest API


//cuando se ejecutal e comando de test:watch lo que hace es que vitest usa watchers del sistema operativo , 
// generalmente chikidar el cual detecta cambuos en los archivos,se actualiza el grafo con dependencia y solo s ecorren esos tests.
// vitest --watch
//       │
// start Vite server
//       │
// scan test files
//       │
// run tests
//       │
// listen for file changes
//       │
// change detected
//       │
// re-run affected tests


//------------------

// Algo importante: caching

// Vitest mantiene cache de módulos.

// node_modules/.vitest

// Esto permite:

// faster re-runs





// Ui para los tests dashboards: 
// Vitest también soporta:

// vitest --ui

// Esto abre un test dashboard.

// Interfaz web:

// http://localhost:51204

// Donde puedes ver:

// tests
// snapshots
// coverage
// logs