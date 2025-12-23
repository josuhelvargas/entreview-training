1. ¿Cómo funciona jsdom ?
📖 Conceptos Fundamentales
jsdom es una implementación en JavaScript puro del estándar DOM y HTML del navegador.Básicamente, es un "navegador falso" que corre en Node.js.
🏗️ Arquitectura Interna de jsdom
┌─────────────────────────────────────────────────────────┐
│                    Node.js Process                       │
│                                                          │
│  ┌────────────────────────────────────────────────┐   │
│  │              jsdom Instance                     │   │
│  │                                                 │   │
│  │  ┌──────────────────────────────────────┐     │   │
│  │  │         Window Object                │     │   │
│  │  │  • navigator                         │     │   │
│  │  │  • location                          │     │   │
│  │  │  • localStorage                      │     │   │
│  │  │  • document ──────────┐             │     │   │
│  │  └───────────────────────┼─────────────┘     │   │
│  │                          │                     │   │
│  │                          ▼                     │   │
│  │  ┌──────────────────────────────────────┐     │   │
│  │  │       Document Object                │     │   │
│  │  │  • createElement()                   │     │   │
│  │  │  • querySelector()                   │     │   │
│  │  │  • body ────────────┐               │     │   │
│  │  └─────────────────────┼───────────────┘     │   │
│  │                        │                       │   │
│  │                        ▼                       │   │
│  │  ┌──────────────────────────────────────┐     │   │
│  │  │         DOM Tree                     │     │   │
│  │  │                                      │     │   │
│  │  │    <html>                            │     │   │
  │  │  │      └─ <body>                       │     │   │
    │  │  │           ├─ <div id="root">        │     │   │
      │  │  │           │    └─ <button>Click</button> │   │
      │  │  │           └─ <script src="...">     │     │   │
        │  │  └──────────────────────────────────────┘     │   │
        │  │                                                 │   │
        │  │  ┌──────────────────────────────────────┐     │   │
        │  │  │      Event System                    │     │   │
        │  │  │  • addEventListener()                │     │   │
        │  │  │  • dispatchEvent()                   │     │   │
        │  │  │  • Event bubbling/capturing          │     │   │
        │  │  └──────────────────────────────────────┘     │   │
        │  │                                                 │   │
        │  │  ┌──────────────────────────────────────┐     │   │
        │  │  │      CSS Parser (básico)             │     │   │
        │  │  │  • getComputedStyle()                │     │   │
        │  │  │  • style property                    │     │   │
        │  │  └──────────────────────────────────────┘     │   │
        │  └────────────────────────────────────────────────┘   │
        └─────────────────────────────────────────────────────────┘
        💻 Código: Cómo Funciona Internamente
        javascript// Implementación simplificada de cómo jsdom crea el DOM

        class JSDOM {
          constructor(html, options = {}) {
          // 1. Parsear HTML string a tokens
          this.tokens = this.tokenize(html);

        // 2. Crear árbol DOM desde tokens
        this.document = this.buildDOMTree(this.tokens);

        // 3. Crear objeto window
        this.window = this.createWindow(this.document);

        // 4. Inyectar APIs del navegador
        this.injectBrowserAPIs(this.window);
  }

        tokenize(html) {
    // Convierte "<div>Hello</div>" en tokens
    const tokens = [];
        let current = 0;

        while (current < html.length) {
      if (html[current] === '<') {
          // Tag opening/closing
          let tag = '';
        while (html[current] !== '>') {
          tag += html[current];
        current++;
        }
        tag += '>';
        tokens.push({type: 'tag', value: tag });
        current++;
      } else {
          // Text content
          let text = '';
        while (html[current] !== '<' && current < html.length) {
          text += html[current];
        current++;
        }
        if (text) tokens.push({type: 'text', value: text });
      }
    }

        return tokens;
  }

        buildDOMTree(tokens) {
    const document = {
          nodeType: 9, // DOCUMENT_NODE
        childNodes: [],
        createElement: function(tagName) {
        return {
          nodeType: 1, // ELEMENT_NODE
        tagName: tagName.toUpperCase(),
        childNodes: [],
        attributes: { },

        appendChild(child) {
          this.childNodes.push(child);
        child.parentNode = this;
          },

        querySelector(selector) {
            // Búsqueda simple por ID
            if (selector.startsWith('#')) {
              const id = selector.slice(1);
        return this.findById(id);
            }
        // Búsqueda por tag
        return this.findByTagName(selector);
          },

        findById(id) {
            if (this.attributes.id === id) return this;
        for (const child of this.childNodes) {
              if (child.nodeType === 1) {
                const found = child.findById(id);
        if (found) return found;
              }
            }
        return null;
          },

        addEventListener(event, handler) {
            if (!this._eventListeners) this._eventListeners = { };
        if (!this._eventListeners[event]) {
          this._eventListeners[event] = [];
            }
        this._eventListeners[event].push(handler);
          },

        dispatchEvent(event) {
            const listeners = this._eventListeners?.[event.type] || [];
            listeners.forEach(handler => handler(event));
          }
        };
      },

        createTextNode: function(text) {
        return {
          nodeType: 3, // TEXT_NODE
        textContent: text
        };
      }
    };

        // Construir árbol desde tokens
        const stack = [document];
    
    tokens.forEach(token => {
      if (token.type === 'tag') {
        if (token.value.startsWith('</')) {
        // Closing tag
        stack.pop();
        } else {
          // Opening tag
          const tagName = token.value.match(/<(\w+)/)[1];
      const element = document.createElement(tagName);

      // Parsear atributos
      const attrMatches = token.value.matchAll(/(\w+)="([^"]*)"/g);
      for (const match of attrMatches) {
        element.attributes[match[1]] = match[2];
          }

      stack[stack.length - 1].appendChild(element);
          
          if (!token.value.endsWith('/>')) {
        stack.push(element);
          }
        }
      } else if (token.type === 'text') {
        const textNode = document.createTextNode(token.value);
      stack[stack.length - 1].appendChild(textNode);
      }
    });

      return document;
  }

      createWindow(document) {
    return {
        document,

        // APIs del navegador
        localStorage: new LocalStorageMock(),
      sessionStorage: new SessionStorageMock(),

      navigator: {
        userAgent: 'Mozilla/5.0 (jsdom)',
      language: 'en-US'
      },

      location: {
        href: 'http://localhost/',
      protocol: 'http:',
      host: 'localhost'
      },

      setTimeout: global.setTimeout,
      setInterval: global.setInterval,
      clearTimeout: global.clearTimeout,
      clearInterval: global.clearInterval,

      // Event constructor
      Event: class Event {
        constructor(type, options = {}) {
        this.type = type;
      this.bubbles = options.bubbles || false;
      this.cancelable = options.cancelable || false;
        }
      }
    };
  }

      injectBrowserAPIs(window) {
        // Inyectar APIs globales
        window.alert = function (msg) {
          console.log('Alert:', msg);
        };

      window.confirm = function(msg) {
      return true; // Siempre retorna true en tests
    };

      // getComputedStyle básico
      window.getComputedStyle = function(element) {
      return element.style || { };
    };
  }
}

      // Mock de localStorage
      class LocalStorageMock {
        constructor() {
        this.store = {};
  }

      getItem(key) {
    return this.store[key] || null;
  }

      setItem(key, value) {
        this.store[key] = String(value);
  }

      removeItem(key) {
        delete this.store[key];
  }

      clear() {
        this.store = {};
  }
}
      🎯 Ejemplo Real de Uso
      javascript// En tu test
      import {JSDOM} from 'jsdom';

describe('DOM manipulation', () => {
        let dom;
      let document;
  
  beforeEach(() => {
        // Crear instancia de jsdom
        dom = new JSDOM(`
      <!DOCTYPE html>
      <html>
        <body>
          <div id="root"></div>
        </body>
      </html>
    `);

      document = dom.window.document;

      // Hacer document global para tu código
      global.document = document;
      global.window = dom.window;
  });
  
  test('creates and appends element', () => {
    const root = document.getElementById('root');
      const button = document.createElement('button');
      button.textContent = 'Click me';
      button.id = 'myButton';

      root.appendChild(button);

      // jsdom mantiene el árbol DOM actualizado
      expect(document.getElementById('myButton')).toBeTruthy();
      expect(button.textContent).toBe('Click me');
  });
  
  test('handles events', () => {
    const button = document.createElement('button');
      let clicked = false;
    
    button.addEventListener('click', () => {
        clicked = true;
    });

      // Simular click
      button.dispatchEvent(new dom.window.Event('click'));

      expect(clicked).toBe(true);
  });
});
      ⚠️ Limitaciones de jsdom
      javascript// ❌ Cosas que jsdom NO puede hacer:

      // 1. Layout y Rendering
      const element = document.createElement('div');
      element.style.width = '100px';
      console.log(element.offsetWidth); // ❌ Siempre 0, no calcula layout

      // 2. CSS Complejo
      const computed = window.getComputedStyle(element);
      console.log(computed.display); // ❌ Muy limitado

      // 3. APIs modernas del navegador
      // ❌ Canvas rendering
      // ❌ WebGL
      // ❌ Web Workers
      // ❌ Service Workers
      // ❌ IntersectionObserver

      // Para estos casos usa:
      // - Puppeteer / Playwright (navegador real headless)
      // - Mocks específicos
      ```

      ---

      ## 2. ¿Cómo funciona Babel?

      ### 📖 Conceptos Fundamentales

      **Babel** es un transpilador que convierte código JavaScript moderno (ES6+) a versiones antiguas compatibles con navegadores viejos.

      ### 🏗️ Pipeline de Babel (3 Fases)
      ```
      ┌──────────────────────────────────────────────────────────┐
      │                    FASE 1: PARSING                        │
      │                                                           │
      │  Código Fuente (String)                                  │
      │  ┌─────────────────────────────────────────┐            │
│  │ const greeting = (name) => {             │            │
      │  │   return `Hello ${name}!`;               │            │
│  │ };                                       │            │
      │  └─────────────────────────────────────────┘            │
      │                      │                                    │
      │                      ▼                                    │
      │              Lexical Analysis                             │
      │              (Tokenization)                               │
      │                      │                                    │
      │                      ▼                                    │
      │  Tokens                                                   │
      │  ┌─────────────────────────────────────────┐            │
      │  │ [                                        │            │
      │  │   {type: 'keyword', value: 'const' },  │            │
      │  │   {type: 'identifier', value: 'greeting' }, │       │
      │  │   {type: 'punctuator', value: '=' },   │            │
      │  │   {type: 'punctuator', value: '(' },   │            │
      │  │   {type: 'identifier', value: 'name' }, │           │
      │  │   {type: 'punctuator', value: ')' },   │            │
      │  │   {type: 'punctuator', value: '=>' },  │            │
      │  │   ...                                    │            │
      │  │ ]                                        │            │
      │  └─────────────────────────────────────────┘            │
      │                      │                                    │
      │                      ▼                                    │
      │              Syntactic Analysis                           │
      │              (AST Generation)                             │
      │                      │                                    │
      │                      ▼                                    │
      │  Abstract Syntax Tree (AST)                              │
      │  ┌─────────────────────────────────────────┐            │
      │  │ {                                        │            │
      │  │   type: "VariableDeclaration",          │            │
      │  │   kind: "const",                        │            │
      │  │   declarations: [{                      │            │
      │  │     type: "VariableDeclarator",         │            │
      │  │     id: {type: "Identifier", name: "greeting" }, │  │
      │  │     init: {                              │            │
      │  │       type: "ArrowFunctionExpression",  │            │
      │  │       params: [{type: "Identifier", name: "name" }],│
      │  │       body: {                            │            │
      │  │         type: "BlockStatement",          │            │
      │  │         body: [{                         │            │
      │  │           type: "ReturnStatement",       │            │
      │  │           argument: {                    │            │
      │  │             type: "TemplateLiteral",     │            │
      │  │             ...                          │            │
│  │           }                              │            │
│  │         }]                               │            │
│  │       }                                  │            │
│  │     }                                    │            │
│  │   }]                                     │            │
│  │ }                                        │            │
      │  └─────────────────────────────────────────┘            │
      └──────────────────────────────────────────────────────────┘

      ┌──────────────────────────────────────────────────────────┐
      │                FASE 2: TRANSFORMATION                     │
      │                                                           │
      │  AST Original                                             │
      │        │                                                  │
      │        ▼                                                  │
      │  ┌──────────────────────────────┐                       │
      │  │   Plugin 1: arrow-functions   │                       │
│  │   Transforma: () => { }        │                       │
      │  │   A: function() { }            │                       │
      │  └──────────────┬────────────────┘                       │
      │                 ▼                                         │
      │  ┌──────────────────────────────┐                       │
      │  │   Plugin 2: template-literals │                       │
      │  │   Transforma: `Hello ${x}`    │                       │
      │  │   A: "Hello " + x             │                       │
      │  └──────────────┬────────────────┘                       │
      │                 ▼                                         │
      │  ┌──────────────────────────────┐                       │
      │  │   Plugin 3: const-let         │                       │
      │  │   Transforma: const x = 1     │                       │
      │  │   A: var x = 1                │                       │
      │  └──────────────┬────────────────┘                       │
      │                 ▼                                         │
      │  AST Transformado                                         │
      │  ┌─────────────────────────────────────────┐            │
      │  │ {                                        │            │
      │  │   type: "VariableDeclaration",          │            │
      │  │   kind: "var", // ← Cambió de const     │            │
      │  │   declarations: [{                      │            │
      │  │     init: {                              │            │
      │  │       type: "FunctionExpression", // ← Cambió │      │
      │  │       params: [{name: "name" }],       │            │
      │  │       body: {                            │            │
      │  │         body: [{                         │            │
      │  │           argument: {                    │            │
      │  │             type: "BinaryExpression", // ← Cambió │  │
      │  │             operator: "+",               │            │
      │  │             left: {value: "Hello " },   │            │
      │  │             right: {name: "name" }      │            │
│  │           }                              │            │
│  │         }]                               │            │
│  │       }                                  │            │
│  │     }                                    │            │
│  │   }]                                     │            │
│  │ }                                        │            │
      │  └─────────────────────────────────────────┘            │
      └──────────────────────────────────────────────────────────┘

      ┌──────────────────────────────────────────────────────────┐
      │                 FASE 3: CODE GENERATION                   │
      │                                                           │
      │  AST Transformado                                         │
      │        │                                                  │
      │        ▼                                                  │
      │  Generador de Código                                      │
      │  (Traversa el AST y genera strings)                      │
      │        │                                                  │
      │        ▼                                                  │
      │  Código Transpilado                                       │
      │  ┌─────────────────────────────────────────┐            │
      │  │ var greeting = function(name) {          │            │
      │  │   return "Hello " + name + "!";          │            │
│  │ };                                       │            │
      │  └─────────────────────────────────────────┘            │
      └──────────────────────────────────────────────────────────┘
      💻 Implementación Simplificada
      javascript// Implementación básica de cómo funciona Babel

      class SimpleBabel {
        // FASE 1: PARSING
        parse(code) {
    // Tokenize
    const tokens = this.tokenize(code);

      // Generate AST
      const ast = this.generateAST(tokens);

      return ast;
  }

      tokenize(code) {
    const tokens = [];
      let current = 0;

      while (current < code.length) {
        let char = code[current];

      // Whitespace
      if (/\s/.test(char)) {
        current++;
      continue;
      }

      // Parentheses
      if (char === '(') {
        tokens.push({ type: 'paren', value: '(' });
      current++;
      continue;
      }

      if (char === ')') {
        tokens.push({ type: 'paren', value: ')' });
      current++;
      continue;
      }

      // Arrow
      if (char === '=' && code[current + 1] === '>') {
        tokens.push({ type: 'arrow', value: '=>' });
      current += 2;
      continue;
      }

      // Keywords & Identifiers
      if (/[a-z]/i.test(char)) {
        let value = '';
      while (/[a-z0-9]/i.test(char)) {
        value += char;
      char = code[++current];
        }

      const keywords = ['const', 'let', 'var', 'function', 'return'];
      const type = keywords.includes(value) ? 'keyword' : 'identifier';

      tokens.push({type, value});
      continue;
      }

      // Template Literals
      if (char === '`') {
        let value = '';
      char = code[++current];

      while (char !== '`') {
          if (char === '$' && code[current + 1] === '{') {
            // Template expression
            tokens.push({type: 'templateStart', value });
      current += 2;

      let expr = '';
            while (code[current] !== '}') {
        expr += code[current++];
            }
      tokens.push({type: 'templateExpression', value: expr });
      current++;

      value = '';
      char = code[current];
          } else {
        value += char;
      char = code[++current];
          }
        }

      tokens.push({type: 'templateEnd', value });
      current++;
      continue;
      }

      current++;
    }

      return tokens;
  }

      generateAST(tokens) {
        let current = 0;

      function walk() {
        let token = tokens[current];

      // Arrow Function
      if (token.type === 'paren' && tokens[current + 2]?.type === 'arrow') {
        const node = {
        type: 'ArrowFunctionExpression',
      params: [],
      body: null
        };

      current++; // skip '('

      // Get params
      while (tokens[current].type !== 'paren' || tokens[current].value !== ')') {
          if (tokens[current].type === 'identifier') {
        node.params.push({
          type: 'Identifier',
          name: tokens[current].value
        });
          }
      current++;
        }

      current++; // skip ')'
        current++; // skip '=>'

      // Get body
      node.body = walk();

      return node;
      }

      // Template Literal
      if (token.type === 'templateStart' || token.type === 'templateEnd') {
        const node = {
        type: 'TemplateLiteral',
      quasis: [],
      expressions: []
        };

      while (token && (token.type === 'templateStart' ||
      token.type === 'templateExpression' ||
      token.type === 'templateEnd')) {
          if (token.type === 'templateStart' || token.type === 'templateEnd') {
        node.quasis.push({
          type: 'TemplateElement',
          value: token.value
        });
          } else if (token.type === 'templateExpression') {
        node.expressions.push({
          type: 'Identifier',
          name: token.value
        });
          }

      current++;
      token = tokens[current];
        }

      return node;
      }

      current++;
      return null;
    }

      const ast = {
        type: 'Program',
      body: []
    };

      while (current < tokens.length) {
      const node = walk();
      if (node) ast.body.push(node);
    }

      return ast;
  }

      // FASE 2: TRANSFORMATION
      transform(ast, plugins) {
        // Traverse AST y aplicar plugins
        function traverse(node, parent) {
          // Aplicar cada plugin
          plugins.forEach(plugin => {
            plugin.visitor(node, parent);
          });

          // Recursively traverse children
          for (const key in node) {
            if (Array.isArray(node[key])) {
              node[key].forEach(child => {
                if (typeof child === 'object' && child !== null) {
                  traverse(child, node);
                }
              });
            } else if (typeof node[key] === 'object' && node[key] !== null) {
              traverse(node[key], node);
            }
          }
        }
    
    traverse(ast, null);
      return ast;
  }

      // FASE 3: CODE GENERATION
      generate(ast) {
        function gen(node) {
          switch (node.type) {
            case 'Program':
              return node.body.map(gen).join('\n');

            case 'ArrowFunctionExpression':
              // Transform to regular function
              const params = node.params.map(p => p.name).join(', ');
              const body = gen(node.body);
              return `function(${params}) { ${body} }`;

            case 'FunctionExpression':
              const p = node.params.map(p => p.name).join(', ');
              const b = gen(node.body);
              return `function(${p}) { ${b} }`;

            case 'TemplateLiteral':
              // Transform to string concatenation
              let result = '';
              node.quasis.forEach((quasi, i) => {
                result += `"${quasi.value}"`;
                if (node.expressions[i]) {
                  result += ` + ${gen(node.expressions[i])} + `;
                }
              });
              return result.replace(/ \+ $/, '');

            case 'Identifier':
              return node.name;

            case 'BlockStatement':
              return node.body.map(gen).join('\n');

            case 'ReturnStatement':
              return `return ${gen(node.argument)};`;

            default:
              return '';
          }
        }
    
    return gen(ast);
  }
}

      // Plugins de ejemplo
      const arrowFunctionPlugin = {
        visitor(node, parent) {
    if (node.type === 'ArrowFunctionExpression') {
        node.type = 'FunctionExpression';
    }
  }
};

      const templateLiteralPlugin = {
        visitor(node, parent) {
    if (node.type === 'TemplateLiteral') {
      // Convertir a BinaryExpression (concatenación)
      const parts = [];
      node.quasis.forEach((quasi, i) => {
        if (quasi.value) {
        parts.push({ type: 'StringLiteral', value: quasi.value });
        }
      if (node.expressions[i]) {
        parts.push(node.expressions[i]);
        }
      });

      // Crear expresión de concatenación
      let result = parts[0];
      for (let i = 1; i < parts.length; i++) {
        result = {
          type: 'BinaryExpression',
          operator: '+',
          left: result,
          right: parts[i]
        };
      }

      // Reemplazar nodo
      Object.assign(node, result);
    }
  }
};

      // Uso
      const babel = new SimpleBabel();
const code = `const greet = (name) => { return \`Hello \${name}!\`; }`;

      const ast = babel.parse(code);
      const transformedAST = babel.transform(ast, [
      arrowFunctionPlugin,
      templateLiteralPlugin
      ]);
      const output = babel.generate(transformedAST);

      console.log(output);
// Output: var greet = function(name) { return "Hello " + name + "!"; }
      🎯 Configuración Real de Babel
      javascript// .babelrc o babel.config.js
      module.exports = {
        presets: [
      [
      '@babel/preset-env',
      {
        targets: {
        browsers: ['last 2 versions', 'ie >= 11']
        },
      useBuiltIns: 'usage',
      corejs: 3
      }
      ],
      '@babel/preset-react',
      '@babel/preset-typescript'
      ],
      plugins: [
      '@babel/plugin-proposal-class-properties',
      '@babel/plugin-proposal-optional-chaining',
      [
      '@babel/plugin-transform-runtime',
      {
        regenerator: true
      }
      ]
      ]
};

// Lo que hace cada preset/plugin:

// @babel/preset-env
// Input: const x = () => { };
// Output: var x = function() { };

// @babel/preset-react
// Input: <div>Hello</div>
// Output: React.createElement('div', null, 'Hello')

// @babel/preset-typescript
// Input: const x: number = 5;
// Output: const x = 5;

// @babel/plugin-proposal-class-properties
// Input: class A {x = 1; }
// Output: class A {constructor() {this.x = 1; } }

      // @babel/plugin-proposal-optional-chaining
      // Input: obj?.prop?.nested
      // Output: obj == null ? void 0 : obj.prop == null ? void 0 : obj.prop.nested

      3. ¿Qué es CommonJS exactamente?
      📖 Conceptos Fundamentales
      CommonJS es un sistema de módulos para JavaScript creado para Node.js. Define cómo los archivos pueden importar y exportar código.
      🆚 CommonJS vs ES Modules
      javascript// ==================== COMMONJS ====================
      // math.js (exportar)
      function add(a, b) {
  return a + b;
}

      function subtract(a, b) {
  return a - b;
}

      // Exportar con module.exports
      module.exports = {
        add,
        subtract
      };

      // O exportar individual
      exports.add = add;
      exports.subtract = subtract;

      // app.js (importar)
      const math = require('./math');
      console.log(math.add(2, 3)); // 5

      // O destructuring
      const {add, subtract} = require('./math');
      console.log(add(2, 3)); // 5


      // ==================== ES MODULES ====================
      // math.js (exportar)
      export function add(a, b) {
  return a + b;
}

      export function subtract(a, b) {
  return a - b;
}

      // O default export
      export default