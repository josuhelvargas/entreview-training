1️⃣ Raw string literals (""" ... """)
🧠 ¿Qué son?

Son literales de string donde:

No necesitas escapar comillas (") ni backslashes (\).

Perfectos para:

JSON, XML, HTML embebido

Regex complejas

Código generado / plantillas

🔍 Sintaxis básica
string json = """
{
  "nombre": "Josue",
  "edad": 33,
  "activo": true
}
""";


No hay que escribir \".

El contenido se respeta tal cual, incluyendo saltos de línea y espacios.

🔼 Versión con comillas dentro

Si necesitas """ dentro, aumentas el número de comillas:

string ejemplo = """""
Texto con """comillas""" dentro sin problemas
""""" ;

📌 ¿Cuándo usarlo?

Cuando tengas bloques de texto grandes y legibles (JSON, SQL, HTML).

Cuando quieras copiar/pegar un payload sin estar escapando caracteres.

Ideal para tests, snippets de ejemplo, SQL embebido.

💻 Ejemplo práctico (request HTTP con JSON)
var payload = """
{
  "event": "signup",
  "user": {
    "id": 123,
    "name": "Josue"
  }
}
""";

// Enviar en un HttpClient, por ejemplo
var content = new StringContent(payload, Encoding.UTF8, "application/json");










2️⃣ Generic Attributes
🧠 ¿Qué son?

Hasta C# 10, los atributos no podían ser genéricos.
En C# 11 puedes declarar:

public class ValidaTipoAttribute<T> : Attribute
{
    public string Mensaje { get; }

    public ValidaTipoAttribute(string mensaje)
    {
        Mensaje = mensaje;
    }
}


Y usarlos:

[ValidaTipo<int>("Debe ser entero")]
public class ServicioEnteros
{
}

📌 ¿Cuándo usarlo?

Cuando el atributo conceptualmente depende de un tipo:

Validaciones

Mapeos

Metadatos de serialización

Configuración de DI / factories / pipelines

Te evita estar pasando typeof(T) como Type y te da type safety.

💻 Ejemplo práctico (mapear un DTO a entidad)
[MapTo<ClienteEntity>]
public class ClienteDto
{
    public string Nombre { get; set; } = default!;
}

public class MapToAttribute<T> : Attribute { }


Y luego en algún escáner de assemblies:

var tipos = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.GetCustomAttributes(typeof(MapToAttribute<>), inherit: false).Any());













3️⃣ required members
🧠 ¿Qué son?

Permiten marcar propiedades/campos que deben ser inicializados al crear el objeto.
El compilador se queja si no los estableces.

public class Usuario
{
    public required string Nombre { get; init; }
    public required string Email { get; init; }
    public int Edad { get; init; } // opcional
}


Uso correcto:

var u = new Usuario
{
    Nombre = "Josue",
    Email = "josue@example.com",
    Edad = 33
};


Uso incorrecto (warning/error):

var u = new Usuario
{
    Nombre = "Josue"
    // Falta Email → el compilador avisa
};

📌 ¿Cuándo usarlo?

Cuando tu modelo tiene invariantes obligatorias:

Claves, IDs, emails, nombres, etc.

Entidades de dominio que no tienen sentido sin ciertos datos.

DTOs de entrada que deben venir completos.

Es muy útil mezclado con:

init setters

records

NRT (string vs string?)

💻 Ejemplo con record
public record Pedido
{
    public required string Id { get; init; }
    public required DateTime Fecha { get; init; }
    public decimal Total { get; init; }
}


Te garantiza que cualquier creación de Pedido incluye Id y Fecha.
















// 4️⃣ UTF-8 string literals ("hola"u8)
// 🧠 ¿Qué son?

// Permiten obtener directamente un ReadOnlySpan<byte> UTF-8 a partir de un string literal.

// ReadOnlySpan<byte> data = "hola"u8;


// Esto representa los bytes UTF-8 de "hola".

// 📌 ¿Cuándo usarlo?

// APIs de alto rendimiento que trabajan con bytes en lugar de string.

// Protocolos, parsers, serialización.

// Comparaciones rápidas contra tokens conocidos en un buffer.

// Ejemplo típico: parsers de JSON, HTTP, binarios, etc.

// 💻 Ejemplo práctico (comparar prefijo en UTF-8)
static bool ComienzaConHola(ReadOnlySpan<byte> buffer)
{
    ReadOnlySpan<byte> hola = "hola"u8;
    return buffer.StartsWith(hola);
}


Sin crear strings intermedias, todo en nivel de bytes → más rápido y con menos GC.
















5️⃣ Más pattern matching (mejoras C# 11)

C# 11 refina aún más el pattern matching con:

List patterns (muy potentes)

Mejores combinaciones con or, and, patrones de colección

🧠 List patterns

Permiten hacer pattern matching sobre arrays / listas:

int[] numeros = { 1, 2, 3, 4 };

if (numeros is [1, 2, .. var resto])
{
    // Empieza con 1, 2 y el resto queda en 'resto'
}

💻 Ejemplos útiles
5.1 Validar estructura de un array
string Analizar(int[] valores) =>
    valores switch
    {
        [] => "Vacío",
        [var unico] => $"Un solo elemento: {unico}",
        [var primero, var segundo] => $"Dos elementos: {primero}, {segundo}",
        [0, ..] => "Empieza con 0",
        [.., 0] => "Termina con 0",
        _ => "Lista general"
    };

5.2 Matching con strings (como tokens)
string ClasificarComando(string[] args) =>
    args switch
    {
        ["run"] => "Ejecutar por defecto",
        ["run", var modo] => $"Ejecutar en modo {modo}",
        ["config", "show"] => "Mostrar configuración",
        ["config", "set", var clave, var valor] => $"Set {clave} = {valor}",
        _ => "Comando no reconocido"
    };

📌 ¿Cuándo usarlo?

CLIs

Parsers de argumentos

Motores de reglas que dependen de estructuras de listas

Cualquier lógica donde te interese la forma y no solo el contenido

















6️⃣ Hito general: APIs más expresivas, interoperabilidad y rendimiento

C# 11 (junto con .NET 7) empuja tres grandes líneas:

APIs más expresivas

Raw strings → mejor documentación en el código

Generic attributes → metaprogramación más rica

Más pattern matching → reglas más legibles

Interoperabilidad moderna

UTF-8 literals → hablamos en “bytes” con el mundo (web, sockets, protocolos)

Mejor integración con Span<T>, Memory<T>

Rendimiento

Menos alocaciones de string (UTF-8 literals, pattern matching sobre Span, etc.)

Mejoras en el compilador y JIT que se apoyan en estas features

🧩 Mini resumen para entrevista / CV

En C# 11 destaco:

Raw string literals para manejar payloads complejos (JSON, SQL, Regex) sin escapes, muy útiles en tests y clientes HTTP.

Generic attributes para hacer metaprogramación más segura en tiempo de compilación, por ejemplo para mapping, validaciones o configuración.

required members para reforzar invariantes de dominio al nivel del compilador, asegurando que ciertos campos siempre sean inicializados.

UTF-8 string literals ("text"u8) para trabajar en bajo nivel con Span<byte> de forma eficiente, crucial en APIs de alto rendimiento y parsers.

Y mejoras en pattern matching, sobre todo list patterns, que permiten expresar reglas com














Otras caracteristicas: 


🔵 C# 11 — Otras características importantes que no mencionamos
1️⃣ Ref fields en ref struct y mejoras en tipos by-ref

C# 11 permite usar ref fields dentro de ref structs, lo que antes estaba prohibido.

Ejemplo
ref struct BufferWrapper
{
    private ref byte _firstByte;

    public BufferWrapper(ref byte firstByte)
    {
        _firstByte = ref firstByte;
    }
}

Cuándo importa

Si haces parsers, serializadores, span-based APIs, o librerías de alto rendimiento.

Si trabajas con stack allocation y memoria no administrada.

2️⃣ scoped ref y scoped parameters (seguridad de memoria)

C# 11 introduce scoped para indicar que un parámetro no puede escapar de su contexto.

Ejemplo
void Procesar(scoped ReadOnlySpan<byte> datos)
{
    // 'datos' no puede almacenarse en un campo → evita bugs de lifetime
}

Cuándo importa

Si trabajas con Span<T> y Memory<T>.

Si quieres evitar errores de lifetime que solo eran detectables en runtime.

Es un “mini borrow-checker” inspirado en Rust que mejora la seguridad.

3️⃣ Correcciones en struct pattern matching

Puedes hacer matching sobre structs sin caer en boxeo o restricciones anteriores.

4️⃣ Mejoras en el compilador y análisis de nullability

No es un feature “visible”, pero sí muy importante:
C# 11 mejora la precisión del análisis NRT, especialmente en:

constructores parcialmente inicializados

patrones

switch expressions complejos

Cuándo importa

Si usas NRT al máximo (como tú), el compilador ahora da menos falsos positivos y detecta más errores reales.

5️⃣ string literal improvements (line breaks uniformes)

Permite terminaciones consistentes, muy útil para raw strings mezcladas con normalizados.