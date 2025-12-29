🚀 C# 9 — Las mejoras más importantes (análisis + ejemplos + cuándo usarlas)

C# 9 vino con .NET 5 y marcó un antes y un después en diseño de modelos de dominio, DTOs, entidades inmutables y arquitectura limpia.

⭐ 1. Records — El cambio más grande en C# desde generics

Los records incorporan:

Igualdad por valor

Inmutabilidad por defecto

with expressions para crear copias modificadas

Deconstruct()

Mejor soporte para patrones

✔ Ejemplo básico
public record Persona(string Nombre, int Edad);

var p1 = new Persona("Josue", 33);
var p2 = new Persona("Josue", 33);

Console.WriteLine(p1 == p2); // true — comparación por valor

✔ Inmutabilidad + copia
var p3 = p1 with { Edad = 34 };

✔ Records posicionales vs tradicionales

Posicional:

public record Punto(int X, int Y);


Clásico (como class, pero record):

public record Cliente
{
    public string Nombre { get; init; }
    public int Id { get; init; }
}

✔ Por qué importan

Ideales para DTOs, eventos, mensajes, configuración, view models y entidades inmutables.

Reducen errores por mutabilidad.

Integran perfecto con pattern matching.

⭐ 2. init setters — Propiedades inmutables modernas

El complemento natural de los records es init:

public class Usuario
{
    public string Nombre { get; init; }
    public int Edad { get; init; }
}

var u = new Usuario
{
    Nombre = "Juan",
    Edad = 30
};

// u.Edad = 31; // ERROR: solo init

✔ Ventajas

Facilita la inmutabilidad en class sin usar records.

Permite inicialización fluida sin mutación posterior.

⭐ 3. with expressions para copiar objetos con modificaciones

Solo para record en C# 9.

var persona1 = new Persona("Ana", 25);
var persona2 = persona1 with { Edad = 26 };

✔ Beneficio

Sin mutación.

Ideal para diseño funcional o modelos de dominio.

⭐ 4. Pattern Matching Mejorado — Más poderoso y expresivo

C# 9 añadió:

✔ 4.1 Relational patterns (<, >, <=, >=)
string ClasificarEdad(int edad) =>
    edad switch
    {
        < 0 => "No válido",
        < 18 => "Menor",
        < 65 => "Adulto",
        _ => "Mayor"
    };

✔ 4.2 Logical patterns (and, or, not)
bool EsFinDeSemana(DayOfWeek dia) =>
    dia is DayOfWeek.Saturday or DayOfWeek.Sunday;


Combinado:

string Nota(double valor) =>
    valor switch
    {
        < 0 or > 10 => "Inválido",
        >= 9 and <= 10 => "Excelente",
        >= 7 and < 9 => "Bien",
        _ => "Insuficiente"
    };

✔ Por qué importa

Permite expresar reglas de negocio complejas sin ifs anidados.

Excelente para validadores, engines de reglas, orquestadores, etc.

⭐ 5. Top-level Statements — El inicio de las Minimal APIs

Ahora puedes escribir programas sin clase Program:

✔ Antes
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hola mundo");
    }
}

✔ Después
Console.WriteLine("Hola mundo");

✔ Beneficios

Más limpio.

Base de Minimal APIs en ASP.NET Core 6.

Ideal para scripts cortos, demos, CLI utilities.

⭐ 6. Target-typed new() — Menos repetición de tipos
✔ Ejemplo
List<int> numeros = new();
Dictionary<string, int> mapa = new();
Persona persona = new("Josue", 33);

✔ Ventaja

Código más compacto y legible.

⭐ 7. Mejoras en foreach para IAsyncEnumerable<T>

C# 9 añade más optimizaciones y mejor inferencia.

await foreach (var dato in ObtenerDatosAsync())
{
    Console.WriteLine(dato);
}

⭐ 8. Funcionalidad avanzada para native-sized integers

Se introducen:

nint

nuint

Usados para interoperabilidad con arquitecturas 32/64 bits.

nint x = 10;
nuint y = 20;

⭐ 9. Mejoras en partial methods

Antes: solo podían ser void y sin acceso.

C# 9 permite:

métodos partial con retorno

métodos public / internal

parámetros ref / out

public partial class Servicio
{
    public partial string Procesar(int valor);
}

⭐ 10. Support for covariant return types

Permite a métodos sobreescritos retornar tipos más específicos.

public class Animal { }
public class Perro : Animal { }

public class FabricaAnimales
{
    public virtual Animal Crear() => new Animal();
}

public class FabricaPerros : FabricaAnimales
{
    public override Perro Crear() => new Perro(); // ✔ permitido
}