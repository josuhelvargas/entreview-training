 
 
 //1. Nullable Reference Types (NRT) – el cambio filosófico grande

//Antes de C# 8, cualquier string podía ser null, pero el compilador no ayudaba.
//Con NRT, C# distingue entre:

string → no debería ser null

string? → puede ser null

// Activación

// Normalmente en el .csproj:

<PropertyGroup>
  <Nullable>enable</Nullable>
</PropertyGroup>


//O por archivo:

#nullable enable

// Ejemplo básico
#nullable enable

public class Persona
{
    // No debería ser null
    public string Nombre { get; set; }

    // Puede ser null
    public string? Apodo { get; set; }

    public Persona(string nombre)
    {
        Nombre = nombre;          // OK
        // Apodo no se inicializa: compilador avisa si lo usas sin comprobar
    }

    public void Imprimir()
    {
        Console.WriteLine(Nombre.ToUpper()); // OK

        // Console.WriteLine(Apodo.ToUpper()); // WARNING: posible null reference

        if (Apodo != null)
        {
            Console.WriteLine(Apodo.ToUpper()); // OK, ya se hizo null-check
        }
    }
}

// Por qué es importante

// Te obliga a modelar explícitamente la posibilidad de null.

// Mucho menos NullReferenceException en producción.

// Cambia la manera de diseñar tus DTOs, Entities, ViewModels, etc.












//2. Async Streams – IAsyncEnumerable<T>

// Antes:

// Para colecciones → IEnumerable<T> / IQueryable<T>

// Para async → Task<T>

// Con C# 8 aparece IAsyncEnumerable<T>: secuencias asíncronas.

// Ejemplo
public async IAsyncEnumerable<int> ContarAsync()
{
    for (int i = 1; i <= 5; i++)
    {
        await Task.Delay(500); // Simula IO
        yield return i;
    }
}

public async Task UsarContadorAsync()
{
    await foreach (var numero in ContarAsync())
    {
        Console.WriteLine(numero);
    }
}

// Cuándo usarlo

// Lectura paginada desde base de datos / API.

// Streams de mensajes (Kafka, RabbitMQ, etc.).

// Procesos largos donde quieres empezar a consumir resultados sin esperar al final.











// 3. Indexes y Ranges – sintaxis tipo Python

// Nuevos tipos:

// Index → ^1 (desde el final)

// Range → start..end

// Ejemplo con arrays
var numeros = new[] { 10, 20, 30, 40, 50 };

// Índices
int ultimo = numeros[^1];    // 50
int penultimo = numeros[^2]; // 40

// Ranges (sub-arreglos)
int[] sub1 = numeros[1..4];  // {20, 30, 40}
int[] sub2 = numeros[..3];   // {10, 20, 30}
int[] sub3 = numeros[2..];   // {30, 40, 50}

// Uso típico

// Muy práctico para slicing en lógica de negocio, manipulaciones de strings o buffers.

// Hace el código más legible que Array.Copy o Substring con “magic numbers”.
















//4. Switch Expressions – estilo más funcional

// De:

string ObtenerDescripcion(DayOfWeek dia)
{
    switch (dia)
    {
        case DayOfWeek.Monday:
        case DayOfWeek.Tuesday:
            return "Inicio de semana";
        case DayOfWeek.Saturday:
        case DayOfWeek.Sunday:
            return "Fin de semana";
        default:
            return "Mitad de semana";
    }
}


// A:

string ObtenerDescripcion(DayOfWeek dia) =>
    dia switch
    {
        DayOfWeek.Monday or DayOfWeek.Tuesday => "Inicio de semana",
        DayOfWeek.Saturday or DayOfWeek.Sunday => "Fin de semana",
        _ => "Mitad de semana"
    };

// Ventajas

// Expresivo, conciso, amigable para programación funcional.

// Menos break, menos ruido.

// Fácil de combinar con pattern matching.
















// 5. Pattern Matching mejorado

// C# 8 añade varios patrones nuevos:

// Property patterns

// Tuple patterns

Positional patterns (cuando usas deconstruct);

// 5.1 Property Patterns
public record Direccion(string Ciudad, string Pais);
public record Persona(string Nombre, int Edad, Direccion Direccion);

string Clasificar(Persona p) =>
    p switch
    {
        { Edad: < 18 } => "Menor de edad",
        { Direccion: { Pais: "México" } } => "Adulto en México",
        _ => "Otro"
    };

// 5.2 Tuple Patterns
string ClasificarPunto(int x, int y) =>
    (x, y) switch
    {
        (0, 0) => "Origen",
        (_, 0) => "Eje X",
        (0, _) => "Eje Y",
        _ => "Cuadrante"
    };

// 5.3 Positional patterns (con Deconstruct)
public readonly struct Punto
{
    public int X { get; }
    public int Y { get; }

    public Punto(int x, int y) => (X, Y) = (x, y);

    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}

string ClasificarPunto(Punto p) =>
    p switch
    {
        (0, 0) => "Origen",
        (_, 0) => "Eje X",
        (0, _) => "Eje Y",
        _ => "Cuadrante"
    };













// 6. Using Declarations – menos scope boilerplate

Antes:

using (var stream = new FileStream("archivo.txt", FileMode.Open))
using (var reader = new StreamReader(stream))
{
    var contenido = reader.ReadToEnd();
    Console.WriteLine(contenido);
}


// Con C# 8:

public void LeerArchivo()
{
    using var stream = new FileStream("archivo.txt", FileMode.Open);
    using var reader = new StreamReader(stream);

    var contenido = reader.ReadToEnd();
    Console.WriteLine(contenido);
} // aquí se dispone automáticamente

// Ventajas

// El using se convierte en una declaración, no en un bloque.

// Menos indentación, más limpio.

// Ideal para métodos donde usas varios recursos desechables.















// 7. Default Interface Methods

// Permite tener implementaciones por defecto en interfaces.

public interface IRepositorio<T>
{
    T ObtenerPorId(int id);

    // método con implementación por defecto
    IEnumerable<T> ObtenerTodos()
    {
        Console.WriteLine("Implementación por defecto de ObtenerTodos");
        return Enumerable.Empty<T>();
    }
}

public class RepositorioUsuario : IRepositorio<string>
{
    public string ObtenerPorId(int id) => "Usuario " + id;

    // Puede usar la implementación por defecto de ObtenerTodos()
}

// Por qué es importante

// Permite evolucionar interfaces públicas sin romper todas las implementaciones.

// Útil en librerías y frameworks.

// Pero: úsalo con cuidado, puede complicar jerarquías (preferible no abusar).




















// 8. Readonly Members en structs

// Para struct inmutables, puedes marcar miembros como readonly:

public readonly struct Vector2
{
    public double X { get; }
    public double Y { get; }

    public Vector2(double x, double y) => (X, Y) = (x, y);

    public readonly double Magnitud => Math.Sqrt(X * X + Y * Y);

    public readonly override string ToString() => $"({X}, {Y})";
}

// Beneficios
// El compilador puede evitar copias y defensives copies.
// Más performance y seguridad de inmutabilidad.

















// 9. Static Local Functions
// Las funciones locales ahora pueden ser static para garantizar que:
// No capturan variables del entorno externo.
// Mejoran rendimiento y claridad.

public int SumarPositivos(int[] numeros)
{
    return numeros.Where(EsPositivo).Sum();

    static bool EsPositivo(int n) => n > 0; // no captura nada de fuera
}






// 10. Otros detalles útiles de C# 8


// 10.1 Null-coalescing assignment ??=
List<string>? nombres = null;

nombres ??= new List<string>(); // si es null, inicializa

nombres.Add("Josue");



// 10.2 stackalloc mejorado, Span<T>/ReadOnlySpan<T>
// Más soporte para memoria stack y trabajo de bajo nivel con Span<T>:

Span<int> datos = stackalloc int[3] { 1, 2, 3 };

// Resumen corto

// C# 8 marca un cambio clave:
// Más seguridad (NRT, readonly members).
// Más expresividad funcional (switch expressions, pattern matching).
// Mejor manejo de recursos e IO (async streams, using declarations).
// Evolución de librerías (default interface methods).