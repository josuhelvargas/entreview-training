🚀 C# 10 — Las mejoras más importantes (análisis + código + cuándo usarlas)

C# 10 llegó junto con .NET 6 (LTS) y consolidó una sintaxis más limpia, expresiva y enfocada en product-ividad, minimal APIs y compilación más eficiente.

⭐ 1. Global Using Directives
✔ Qué es

Permite declarar using que se aplican a todo el proyecto, evitando repetir los mismos archivos una y otra vez.

✔ Ejemplo

Archivo: GlobalUsings.cs

global using System;
global using System.Collections.Generic;
global using Microsoft.Extensions.Logging;


Cualquier archivo en el proyecto:

public class Demo
{
    List<string> nombres = new(); // funciona sin using!
}

✔ Por qué importa

Código más limpio.

Reduce ruido en Minimal APIs, ASP.NET Core y microservicios.

En proyectos grandes evita cientos de líneas repetidas.

⭐ 2. File-scoped Namespace (🎉 uno de los cambios más usados)
✔ Antes
namespace MiApp.Servicios
{
    public class ClienteService
    {
    }
}

✔ Ahora
namespace MiApp.Servicios;

public class ClienteService
{
}

✔ Ventajas

Menos indentación.

Archivos más limpios.

Perfecto para arquitecturas limpias, DDD, microservicios.

⭐ 3. Record Structs

Ahora puedes tener records (antes solo classes) pero como structs (tipo valor).

✔ Ejemplo
public readonly record struct Punto(int X, int Y);

✔ Beneficios

Inmutables.

Comparación por valor.

Sin overhead de tipo referencia.

Útiles en cálculos, gráficos, juegos, pipelines de datos o telemetría.

⭐ 4. Improvements to Lambda Expressions

Lambdas ahora:

Tienen tipo natural (ya no siempre Func<>)

Pueden declarar atributos

Pueden declarar tipo de retorno explícito

Pueden inferir parámetros

✔ Ejemplo 1: tipo natural automático
var suma = (int a, int b) => a + b;

// suma es un delegate fuertemente tipado
Console.WriteLine(suma(3, 4));

✔ Ejemplo 2: lambdas con atributos
var log = [Logger] (string mensaje) =>
{
    Console.WriteLine(mensaje);
};

✔ Ejemplo 3: lambdas con tipo de retorno explícito
Func<int, int, int> resta = (int x, int y) => x - y;

✔ Beneficio

Hace las lambdas más potentes dentro de:

Minimal APIs

Mediation pipelines

Event handlers avanzados

Inyección de dependencias basada en delegados

⭐ 5. Constant Interpolated Strings

En C# 10, si todos los valores interpolados son constantes, puedes usar interpolación en constantes.

✔ Ejemplo
const string Version = "v1";
const string Ruta = $"api/{Version}/clientes"; // válido en C# 10

✔ Útil para:

Rutas

Mensajes constantes

Claves de cache

Identificadores de dominio

⭐ 6. Improvements in Pattern Matching

Aunque no tan revolucionario como C# 8 o 9, C# 10 añade:

Pattern and

Pattern or mejorado

Parenthesized patterns

✔ Ejemplo
string Clasificar(int edad) =>
    edad switch
    {
        < 0 => "No válido",
        >= 0 and < 18 => "Menor",
        >= 18 and < 65 => "Adulto",
        >= 65 => "Mayor",
    };

✔ Beneficio

Código más expresivo y cercano a reglas de dominio (lo cual tú usas en reglas dinámicas 👌).

⭐ 7. Property Patterns Mejorados

Permiten asignaciones anidadas más claras.

✔ Ejemplo
public record Direccion(string Ciudad, string Pais);
public record Persona(string Nombre, Direccion Direccion);

bool EsMexicano(Persona p) =>
    p is { Direccion: { Pais: "México" } };

⭐ 8. Sealed ToString() en Records

Ahora puedes sellar el comportamiento de ToString() generado automáticamente.

public record Cliente(string Nombre)
{
    public sealed override string ToString() => $"Cliente: {Nombre}";
}

⭐ 9. Enhanced #region para mejores grupos de código

Ahora puedes colocar #region en más ubicaciones, incluso dentro de namespace file-scoped.

⭐ 10. Struct Parameterless Constructors

Antes no podías definir un constructor vacío en un struct.
Ahora sí:

public struct Medida
{
    public int Valor;

    public Medida()
    {
        Valor = 10;
    }
}

⭐ 11. Better overload resolution for interpolated strings

C# ahora es más inteligente escogiendo el método correcto cuando usas cadenas interpoladas.

Ejemplo:

void Log(string mensaje) => Console.WriteLine("String");
void Log(FormattableString mensaje) => Console.WriteLine("Formattable");

Log($"Hola {DateTime.Now}");


C# 10 elige FormattableString cuando corresponde.