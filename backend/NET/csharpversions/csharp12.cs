⭐ C# 12 (2023–2024) — Visión general

C# 12 sigue empujando el lenguaje hacia:

Sintaxis más funcional (expresiva, concisa)

Manteniendo la orientación a objetos

Mejoras de rendimiento y ergonomía para código “real” (APIs, librerías, parsers, etc.)

Features que vamos a ver:

Primary constructors para cualquier tipo

Collection expressions ([1, 2, 3])

Default lambda parameters

Inline arrays

Interceptors (preview)

Alias de tipos definidos por el usuario (UDT alias)

1️⃣ Primary constructors para cualquier tipo

Antes de C# 12, los primary constructors solo existían en record:

public record Persona(string Nombre, int Edad);


En C# 12 puedes usarlos en:

class

struct

record class

record struct

🧠 ¿Qué son?

Te permiten declarar los parámetros del constructor en la firma del tipo, y luego usar esos parámetros directamente para inicializar campos/propiedades.

🔧 Ejemplo simple con class
public class Cliente(string nombre, int edad)
{
    public string Nombre { get; } = nombre;
    public int Edad { get; } = edad;
}


Uso:

var c = new Cliente("Josue", 33);
Console.WriteLine(c.Nombre); // Josue

💡 Cuándo usarlo

Cuando tu tipo es esencialmente un contenedor de datos + algo de lógica.

Cuando quieres evitar boilerplate de:

constructor

asignación de propiedades

campos privados innecesarios

Ejemplo típico: servicios donde los parámetros se usan como dependencias:

public class PedidoService(ILogger<PedidoService> logger, IPedidoRepository repo)
{
    public void Procesar(string pedidoId)
    {
        logger.LogInformation("Procesando pedido {PedidoId}", pedidoId);
        // ...
    }
}


Aquí no necesitas ni escribir el constructor entero; los parámetros son MIEMBROS implícitos dentro del tipo.

2️⃣ Collection expressions — [1, 2, 3] para casi todo
🧠 ¿Qué son?

Una sintaxis unificada para inicializar colecciones:

int[] a = [1, 2, 3];
List<string> nombres = ["Ana", "Luis", "Josue"];
Span<int> span = [1, 2, 3];
IEnumerable<int> query = [1, 2, 3];


El compilador traduce eso internamente a lo que corresponda (new[] {}, new List<T> {}, etc.) según el tipo.

🔧 Ejemplo incluyendo “spread” (..)
int[] baseArray = [1, 2, 3];
int[] extendido = [0, ..baseArray, 4]; 
// Resultado: [0,1,2,3,4]

💡 Cuándo usarlo

Cuando quieras escribir listas literales de forma corta, como en JavaScript/TypeScript.

En tests, para construir colecciones rápido.

Para componer colecciones (extender, concatenar) sin tanto ruido:

var defaultStores = ["BA", "MB", "BAE"];
var extraStores = ["CHIH", "SON"];

var allStores = [..defaultStores, ..extraStores];


Esto se ve hermoso en lógica de reglas, mapeos, listas de configuración, etc.

3️⃣ Default lambda parameters

Antes, las lambdas no podían tener parámetros con valores por defecto.
En C# 12 sí:

🔧 Ejemplo básico
var saludar = (string nombre = "invitado") => 
    Console.WriteLine($"Hola, {nombre}");

saludar();           // Hola, invitado
saludar("Josue");    // Hola, Josue

💡 Cuándo usarlo

Cuando tienes delegados o callbacks que casi siempre usan un valor por defecto.

Factories de servicios, configuraciones, handlers, donde solo a veces quieres customizar un parámetro.

Ejemplo más realista: logger custom

Action<string, LogLevel> log = (mensaje, nivel = LogLevel.Information) =>
{
    Console.WriteLine($"[{nivel}] {mensaje}");
};

log("Inicio");                      // Information
log("Error crítico", LogLevel.Error);

4️⃣ Inline arrays
🧠 ¿Qué son?

Permiten definir estructuras con un array fijo embebido dentro del tipo, todo en la pila / memoria contigua, sin heap allocations adicionales.

Son útiles para:

escenarios de alto rendimiento

buffers pequeños y fijos

interoperabilidad con código nativo

🔧 Ejemplo conceptual
using System.Runtime.CompilerServices;

[InlineArray(4)]
public struct SmallBuffer
{
    private int _element0;
}


Ahora puedes usar SmallBuffer como si fuera un array de 4 int:

var buffer = new SmallBuffer();
buffer[0] = 10;
buffer[1] = 20;

for (int i = 0; i < 4; i++)
{
    Console.WriteLine(buffer[i]);
}

💡 Cuándo usarlo

Donde antes usarías stackalloc + arrays temporales, pero quieres tipos reutilizables.

Parsers, algoritmos numéricos, seguridad, estructuras pequeñas.

Ejemplo: un buffer de 16 bytes para un token corto:

[InlineArray(16)]
public struct Token16
{
    private byte _element0;
}

5️⃣ Interceptors (preview)

Nota: feature en preview, orientada a tooling, AOP y generación de código.

🧠 Idea general

Los interceptors permiten que una llamada a método pueda ser interceptada y redirigida a otra implementación en tiempo de compilación (no “runtime” clásico como AOP con proxies).

Ejemplo típico:

Tienes un método “virtual” generado automáticamente.

Un interceptor puede “insertar” lógica que se ejecuta en lugar del método original.

Esto está muy orientado a:

Generadores de código (source generators)

Frameworks que quieran insertar lógica transversal:

logging

caching

validación

telemetría

Por ser preview, la sintaxis exacta puede cambiar, pero la idea es:
“haz que esta llamada en el código realmente llame a este otro método generado”.

💡 Cuándo (en el futuro) usarlo

Si estás construyendo frameworks o librerías base (como tú con engines de validación, orquestadores, etc.).

Para evitar boilerplate de cross-cutting concerns sin usar proxies dinámicos en runtime.

Para un developer de aplicaciones “normal”, de momento es más algo a conocer conceptualmente que algo que uses todos los días en producción.

6️⃣ Alias de tipos definidos por el usuario (User-defined type alias)
🧠 ¿Qué son?

Van más allá del clásico:

using MiDiccionario = System.Collections.Generic.Dictionary<string, int>;


En C# 12, los alias de tipo son más potentes y se integran mejor en el sistema de tipos.

La idea es que puedas definir “nuevos nombres significativos” para tipos existentes y usarlos como si fueran tipos propios, mejorando:

legibilidad del dominio

consistencia

documentación

🔧 Ejemplo conceptual
using StoreId = int;
using Amount = decimal;

public class Transaccion
{
    public StoreId Store { get; set; }
    public Amount Total { get; set; }
}


Esto deja más claro al leer el código:

Transaccion t = new()
{
    Store = 123,
    Total = 999.99m
};


No estás viendo solo int y decimal, sino conceptos de dominio.

Ojo: esto sigue siendo alias, no nuevos tipos fuertes a nivel CLR, pero a nivel de expresión del dominio ayuda mucho.

💡 Cuándo usarlo

Cuando un tipo primitivo (int, decimal, string) representa algo con semántica fuerte:

StoreId, HeroId, PromotionId, Money, Percentage

Para dar más expresividad a firmas de métodos:

using PromotionId = int;
using StoreGroup = string;

PromotionId CreatePromotion(StoreGroup group, decimal discount) { ... }

🎯 Resumen final (para tener en mente)

C# 12 como hito: “el lenguaje unifica sintaxis funcional y orientada a objetos”

Primary constructors
→ acercan class/struct a la ergonomía de los records, menos boilerplate, más enfoque en el dominio.

Collection expressions [1,2,3]
→ sintaxis tipo JavaScript / F# para colecciones, mejorando testability, expresividad y composición.

Default lambda parameters
→ lambdas que se comportan más como métodos normales, ideales para configuraciones y callbacks.

Inline arrays
→ herramientas de alto rendimiento y bajo nivel, para librerías, parsers, engines.

Interceptors (preview)
→ base para futuros frameworks de AOP / generación de código sin proxys runtime.

User-defined type alias
→ hace que tu código hable el lenguaje del dominio, no solo tipos primitivos.



















🟣 C# 12 — Otras mejoras importantes no mencionadas
1️⃣ Params collections con collection expressions

Ahora puedes usar params con la nueva sintaxis [ ].

Ejemplo:

void Procesar(params int[] valores) { }

Procesar([1,2,3]); // nuevo

Cuándo importa

Cuando diseñas APIs que aceptan múltiples elementos (builders, validators).

Tests con múltiples escenarios.

2️⃣ Mejoras en ref readonly y en capturas de lambdas

Las lambdas ahora respetan mejor los qualifiers ref, ref readonly, reduciendo copias innecesarias.

Cuándo importa

código de alto rendimiento

procesamiento de memoria intensivo

generadores de código

3️⃣ Nameof mejoras – soporte más amplio

Ahora nameof funciona mejor con:

alias definidos por el usuario

tipos generados

métodos estáticos en generic attributes

No es gigantesco, pero sí útil para código limpio y reflección ligera.

4️⃣ Switch expressions con más optimizaciones de salto

El compilador ahora genera bytecode más eficiente en árboles grandes de patrones.

Cuándo importa

→ Si tienes motores de reglas con muchos patterns (como tus validaciones dinámicas), esto te da rendimiento gratuito.

5️⃣ Generic math refinado (introducido en C# 11, potenciado en C# 12)

Esto es MUY importante y todavía poco conocido.
Llegó en .NET 7 pero afecta C# 11–12.

Ejemplo:

static T Sumar<T>(T a, T b) where T : INumber<T>
    => a + b;


Puedes escribir funciones matemáticas genéricas sin duplicar código para int, decimal, double, etc.

Cuándo usarlo

librerías matemáticas

validadores numéricos

normalización de datos

pipelines de promoción que validen cantidades dinámicamente

6️⃣ Primary constructors + property patterns

C# 12 permite mezclar ambos mundos:

class Usuario(string nombre)
{
    public string Nombre => nombre;
}

bool EsJosue(object o) => o is Usuario("Josue");


Esto abre la puerta a un estilo de código más funcional y expresivo.

7️⃣ Better method group conversions

Ahora el compilador puede inferir conversiones más avanzadas entre:

lambdas

method groups

delegates genéricos

Lo usarás sin darte cuenta en configuraciones fluentes tipo:

builder.Services.AddSingleton(Logica.Validar);