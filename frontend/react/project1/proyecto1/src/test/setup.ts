import '@testing-library/jest-dom/vitest'; //habilitar las librerias de react-testing library par aqu elas utilice tambien vitest.(por ejemplo metodo  tobeindocument()

// La diferencia fundamental (en una frase)

// vitest.config.ts configura cómo Vitest se ejecuta.
// test/setup.ts configura qué existe dentro del entorno de los tests.

// Son 0
// 1️⃣ vitest.config.ts — Configuración del motor

// Piensa en vitest.config.ts como el BIOS / sistema operativo del runner.

// Aquí defines:

// 🧠 Cómo Vitest corre

// 🌍 Dónde corre

// 🔌 Qué herramientas se conectan





// 2️⃣ test/setup.ts — Configuración del mundo interno del test

// Este archivo sí se ejecuta dentro del runtime de los tests.

// Piensa en setup.ts como:

// “Lo primero que corre dentro del navegador falso antes de que se ejecute cualquier test”

// Cosas que se hacen aquí

// Registrar matchers globales

// Configurar mocks globales

// Configurar polyfills

// Resetear estado entre tests

// Inicializar MSW (mock server)