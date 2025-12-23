### 💻 Implementación Interna de CommonJS
  //javascript
// Así implementa Node.js require() internamente

class Module {
  constructor(id, parent) {
    this.id = id;
    this.exports = {}; // Lo que se exporta
    this.parent = parent;
    this.filename = null;
    this.loaded = false;
    this.children = [];
  }
  
  static _cache = {}; // Caché de módulos
  static _pathCache = {}; // Caché de rutas resueltas
  
  // El famoso require()
  static require(path) {
    // 1. Resolver ruta absoluta
    const filename = Module._resolveFilename(path);
    
    // 2. Revisar caché
    if (Module._cache[filename]) {
      return Module._cache[filename].exports;
    }
    
    // 3. Crear nuevo módulo
    const module = new Module(filename, this);
    
    // 4. Cachear antes de cargar (para dependencias circulares)
    Module._cache[filename] = module;
    
    // 5. Cargar módulo
    module.load(filename);
    
    // 6. Retornar exports
    return module.exports;
  }
  
  static _resolveFilename(request) {
    // Lógica de resolución de Node.js
    
    // Si es módulo nativo (fs, path, etc.)
    if (Module._isNative(request)) {
      return request;
    }
    
    // Si es ruta relativa o absoluta
    if (request.startsWith('./') || request.startsWith('../') || 
        request.startsWith('/')) {
      return path.resolve(request);
    }
    
    // Si es node_modules
    return Module._findInNodeModules(request);
  }
  
  static _findInNodeModules(request) {
    // Buscar en node_modules subiendo directorios
    let currentDir = process.cwd();
    
    while (true) {
      const modulePath = path.join(currentDir, 'node_modules', request);
      
      if (fs.existsSync(modulePath)) {
        // Revisar package.json
        const packagePath = path.join(modulePath, 'package.json');
        if (fs.existsSync(packagePath)) {
          const pkg = JSON.parse(fs.readFileSync(packagePath));
          const main = pkg.main || 'index.js';
          return path.join(modulePath, main);
        }
        
        // Buscar index.js
        const indexPath = path.join(modulePath, 'index.js');
        if (fs.existsSync(indexPath)) {
          return indexPath;
        }
      }
      
      // Subir un directorio
      const parentDir = path.dirname(currentDir);
      if (parentDir === currentDir) break; // Raíz alcanzada
      currentDir = parentDir;
    }
    
    throw new Error(`Cannot find module '${request}'`);
  }
  
  load(filename) {
    // Leer contenido del archivo
    const content = fs.readFileSync(filename, 'utf8');
    
    // Envolver en función
    this._compile(content, filename);
    
    this.loaded = true;
  }
  
  _compile(content, filename) {
    // Envolver código en función con inyección de variables
    const wrapper = Module.wrap(content);
    
    // Compilar función
    const compiledWrapper = vm.runInThisContext(wrapper, {
      filename,
      lineOffset: 0,
      displayErrors: true
    });
    
    // Preparar argumentos
    const dirname = path.dirname(filename);
    const require = Module.createRequire(filename);
    const args = [
      this.exports,    // exports
      require,         // require
      this,            // module
      filename,        // __filename
      dirname          // __dirname
    ];
    
    // Ejecutar función
    return compiledWrapper.apply(this.exports, args);
  }
  
  static wrap(script) {
    // El famoso wrapper de Node.js
    return `(function (exports, require, module, __filename, __dirname) {
${ script }
  }); `;
  }
  
  static createRequire(filename) {
    // Crear función require ligada al módulo
    return function require(path) {
      return Module.require(path);
    };
  }
}

// Ejemplo de uso interno
// Cuando haces: const fs = require('fs');
// Node.js hace internamente:

/*
1. Module.require('fs')
2. Resuelve 'fs' como módulo nativo
3. No necesita leer archivo (es built-in)
4. Retorna binding nativo
*/

// Cuando haces: const myModule = require('./myModule');
/*
1. Module.require('./myModule')
2. Resuelve ruta: /path/to/project/myModule.js
3. Revisa caché: Module._cache['/path/to/project/myModule.js']
4. Si no está en caché:
   a. Lee archivo
   b. Envuelve en función:
      (function (exports, require, module, __filename, __dirname) {
        // Contenido de myModule.js
        exports.foo = 'bar';
      })
   c. Ejecuta función
   d. Cachea module.exports
5. Retorna module.exports
*/


//### 🔄 Dependencias Circulares
  //javascript
// a.js
console.log('a starting');
exports.done = false;
const b = require('./b.js');
console.log('in a, b.done =', b.done);
exports.done = true;
console.log('a done');

// b.js
console.log('b starting');
exports.done = false;
const a = require('./a.js');
console.log('in b, a.done =', a.done);
exports.done = true;
console.log('b done');

// main.js
console.log('main starting');
const a = require('./a.js');
const b = require('./b.js');
console.log('in main, a.done =', a.done, 'b.done =', b.done);

// Output:
// main starting
// a starting
// b starting
// in b, a.done = false  ← a aún no terminó de cargar
// b done
// in a, b.done = true
// a done
// in main, a.done = true b.done = true

// ¿Por qué funciona?
// 1. main.js requiere a.js
// 2. a.js se cachea INMEDIATAMENTE (exports = {})
// 3. a.js requiere b.js
// 4. b.js requiere a.js
// 5. a.js YA está en caché, retorna exports actual (incompleto)
// 6. b.js termina de cargar
// 7. a.js termina de cargar


//### ⚖️ CommonJS vs ES Modules - Diferencias Técnicas

//   | Característica | CommonJS | ES Modules |
// | ----------------| ----------| ------------|
// | ** Loading ** | Síncrono | Asíncrono |
// | ** Parsing ** | Runtime | Parse time |
// | ** Exports ** | Dinámico | Estático |
// | ** Tree Shaking ** | ❌ No | ✅ Sí |
// | ** Top - level await ** | ❌ No | ✅ Sí |
// | ** `this` ** | `module.exports` | `undefined` |
//   // ```javascript


// ❌ NO FUNCIONA en CommonJS
if (condition) {
  const module = require('./module'); // ✅ Funciona (dinámico)
}

// ❌ NO FUNCIONA en ES Modules
if (condition) {
  import module from './module'; // ❌ Syntax error (debe ser top-level)
}

// CommonJS - exports dinámico
if (process.env.NODE_ENV === 'production') {
  module.exports = require('./prod');
} else {
  module.exports = require('./dev');
}
// ✅ Funciona

// ES Modules - exports estático
export function foo() {}
if (condition) {
  export function bar() {} // ❌ Syntax error
}
```

// ---

// ## 4. ¿Qué debería haber en beforeEach / afterEach ?

// ### 📋 Guía Completa
//   javascript
describe('User Service Tests', () => {
  
  // ============================================
  // beforeEach - CONFIGURACIÓN ANTES DE CADA TEST
  // ============================================
  
  beforeEach(() => {
    // ✅ 1. RESETEAR MOCKS
    jest.clearAllMocks(); // Limpia historial de llamadas
    jest.restoreAllMocks(); // Restaura implementaciones originales
    
    // ✅ 2. CREAR INSTANCIAS FRESCAS
    // Evita que tests compartan estado
    database = new TestDatabase();
    userService = new UserService(database);
    
    // ✅ 3. SEED DATA (datos de prueba)
    testUser = {
      id: 1,
      name: 'John Doe',
      email: 'john@example.com'
    };
    
    // ✅ 4. CONFIGURAR MOCKS CON VALORES POR DEFECTO
    mockAxios.get.mockResolvedValue({ data: testUser });
    mockAxios.post.mockResolvedValue({ data: { success: true } });
    
    // ✅ 5. CONFIGURAR ENTORNO
    process.env.API_KEY = 'test-key';
    localStorage.clear();
    
    // ✅ 6. RESETEAR TIMERS (si usas fake timers)
    jest.useFakeTimers();
    
    // ✅ 7. SETUP DEL DOM (en tests de UI)
    document.body.innerHTML = '<div id="root"></div>';
    
    // ✅ 8. CONFIGURAR SPIES
    consoleErrorSpy = jest.spyOn(console, 'error').mockImplementation();
  });
  
  // ============================================
  // afterEach - LIMPIEZA DESPUÉS DE CADA TEST
  // ============================================
  
  afterEach(() => {
    // ✅ 1. LIMPIAR TIMERS
    jest.clearAllTimers();
    jest.useRealTimers();
    
    // ✅ 2. LIMPIAR DOM
    document.body.innerHTML = '';
    
    // ✅ 3. CERRAR CONEXIONES
    if (database) {
      database.close();
    }
    
    // ✅ 4. RESTAURAR SPIES
    if (consoleErrorSpy) {
      consoleErrorSpy.mockRestore();
    }
    
    // ✅ 5. LIMPIAR STORAGE
    localStorage.clear();
    sessionStorage.clear();
    
    // ✅ 6. LIMPIAR VARIABLES GLOBALES
    delete global.testVariable;
    
    // ✅ 7. RESETEAR MÓDULOS (si hay estado compartido)
    jest.resetModules();
    
    // ✅ 8. VERIFICAR NO HAY WARNINGS/ERRORS
    expect(consoleErrorSpy).not.toHaveBeenCalled();
  });
  
  // Tests...
  test('creates user successfully', async () => {
    const user = await userService.createUser(testUser);
    expect(user.name).toBe('John Doe');
  });
});


// ### 🎯 Ejemplos por Tipo de Test

// #### ** Tests de Componentes React **
//   javascript
describe('Button Component', () => {
  let container;
  
  beforeEach(() => {
    // Setup DOM
    container = document.createElement('div');
    document.body.appendChild(container);
    
    // Mock event handlers
    mockOnClick = jest.fn();
    
    // Reset React
    ReactDOM.unmountComponentAtNode(container);
  });
  
  afterEach(() => {
    // Unmount React component
    ReactDOM.unmountComponentAtNode(container);
    
    // Remove DOM
    container.remove();
    container = null;
    
    // Clear mocks
    jest.clearAllMocks();
  });
  
  test('calls onClick when clicked', () => {
    render(<Button onClick={mockOnClick}>Click</Button>, container);
    const button = container.querySelector('button');
    button.click();
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });
});
// ```

// #### ** Tests de API / Network **
//   ```javascript
describe('API Service', () => {
  let mockAxios;
  let apiService;
  
  beforeEach(() => {
    // Mock axios
    mockAxios = {
      get: jest.fn(),
      post: jest.fn(),
      put: jest.fn(),
      delete: jest.fn()
    };
    
    // Create service with mock
    apiService = new APIService(mockAxios);
    
    // Default successful responses
    mockAxios.get.mockResolvedValue({ data: {} });
    mockAxios.post.mockResolvedValue({ data: {} });
  });
  
  afterEach(() => {
    // Verificar no hay requests pendientes
    expect(mockAxios.get).toHaveBeenCalledTimes(
      mockAxios.get.mock.calls.length
    );
    
    jest.clearAllMocks();
  });
  
  test('fetches user data', async () => {
    mockAxios.get.mockResolvedValue({ data: { name: 'John' } });
    const user = await apiService.getUser(1);
    expect(user.name).toBe('John');
  });
});
```

#### ** Tests con Timers **
  ```javascript
describe('Debounce Function', () => {
  beforeEach(() => {
    jest.useFakeTimers();
    mockCallback = jest.fn();
  });
  
  afterEach(() => {
    jest.clearAllTimers();
    jest.useRealTimers();
  });
  
  test('debounces calls', () => {
    const debounced = debounce(mockCallback, 1000);
    
    debounced();
    debounced();
    debounced();
    
    expect(mockCallback).not.toHaveBeenCalled();
    
    jest.advanceTimersByTime(1000);
    
    expect(mockCallback).toHaveBeenCalledTimes(1);
  });
});
// ```

// #### ** Tests de Database **
//   ```javascript
describe('User Repository', () => {
  let db;
  let repository;
  
  beforeEach(async () => {
    // Crear DB en memoria
    db = await createTestDatabase();
    
    // Seed inicial
    await db.query(`
      CREATE TABLE users(
    id INT PRIMARY KEY,
    name VARCHAR(255),
    email VARCHAR(255)
  )
  `);
    
    await db.query(`
      INSERT INTO users VALUES
  (1, 'John', 'john@test.com'),
  (2, 'Jane', 'jane@test.com')
    `);
    
    repository = new UserRepository(db);
  });
  
  afterEach(async () => {
    // Limpiar todas las tablas
    await db.query('DROP TABLE IF EXISTS users');
    
    // Cerrar conexión
    await db.close();
  });
  
  test('finds user by id', async () => {
    const user = await repository.findById(1);
    expect(user.name).toBe('John');
  });
});
// ```

// ### ❌ Anti - Patrones Comunes
//   ```javascript
describe('Bad Practices', () => {
  
  // ❌ NO compartir estado mutable
  const sharedArray = []; // Peligroso!
  
  beforeEach(() => {
    sharedArray.push(1); // Cada test afecta al siguiente
  });
  
  // ❌ NO hacer assertions en beforeEach/afterEach
  beforeEach(() => {
    const result = someFunction();
    expect(result).toBe(true); // ❌ Malo
  });
  
  // ❌ NO hacer trabajo asíncrono sin await
  beforeEach(() => {
    fetchData(); // ❌ No espera que termine
  });
  
  // ✅ Hacer esto en su lugar
  beforeEach(async () => {
    await fetchData(); // ✅ Espera correctamente
  });
  
  // ❌ NO configurar cosas que nunca se usan
  beforeEach(() => {
    mockA = jest.fn();
    mockB = jest.fn();
    mockC = jest.fn();
    mockD = jest.fn(); // Si ningún test usa mockD, no lo pongas aquí
  });
  
  // ✅ Solo configura lo que TODOS los tests necesitan
  beforeEach(() => {
    mockA = jest.fn(); // Usado por todos
  });
  
  test('specific test', () => {
    const mockD = jest.fn(); // ✅ Solo este test lo necesita
  });
});
```

### 🎯 Regla de Oro
  // ```javascript
// PREGUNTA: ¿Esto va en beforeEach o en el test?

// ✅ beforeEach si:
// - TODOS los tests lo necesitan
// - Es configuración compartida
// - Es limpieza necesaria

// ✅ En el test si:
// - Solo ESE test lo necesita
// - Es específico del caso de prueba
// - Hace el test más legible
