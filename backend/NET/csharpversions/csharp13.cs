🚀 C# 13 — Principales Mejoras, Explicadas con Ejemplos y Casos de Uso
⭐ 1. Params Collections (params de colecciones)

La mejora más útil de C# 13.
Antes solo se podía usar params con arreglos.
C# 13 permite params con List<T>, Span<T>, ReadOnlySpan<T>, y otros tipos.

✔ Ejemplo antes (solo arrays)
void Procesar(params int[] valores)
{
    foreach (var v in valores)
        Console.WriteLine(v);
}

Procesar(1, 2, 3);

✔ Ahora con C# 13
void Procesar(params List<int> valores)
{
    foreach (var v in valores)
        Console.WriteLine(v);
}

Procesar(new List<int> { 1, 2, 3 });

✔ Uso práctico

Útil cuando tus APIs ya trabajan con List<T>, no con arrays:

Servicios que agregan elementos a listas de dominio

Funciones utilitarias donde la lista debe seguir viva fuera del método

Workflows que usan colecciones extensibles

Parsers, validadores, pipelines

✔ Cuándo usarlo

Cuando el método recibirá colecciones mutables.

Cuando no quieres convertir de lista → array cada vez (costo extra).

Cuando diseñas librerías que deben ser compatibles con Minimal APIs o LINQ moderno.

⭐ 2. Using Aliases para Tipos y Espacios de Nombres Anidados

Ahora puedes crear alias incluso para namespaces completos y para tipos genéricos anidados, dándote más control cuando nombres chocan.

✔ Ejemplo
using MiRepo = MyCompany.Data.Repositories;
using ListaEnteros = System.Collections.Generic.List<int>;

var lista = new ListaEnteros { 1, 2, 3 };
MiRepo.UserRepository repo = new();

✔ Cuándo usarlo

Cuando tienes nombres largos en dominios complejos.

Cuando trabajas con DDD y sigues bounded contexts con nombres similares.

Cuando evitas colisiones entre:

System.Text.Json.JsonSerializer

Newtonsoft.Json.JsonSerializer

⭐ 3. Interceptores (Preview) — El futuro del código generativo

Los interceptores permiten que el compilador reemplace la llamada a un método por otra implementación generada en build time.

Es una forma evolucionada de source generators.

✔ Ejemplo simplificado
// Método original
int Calcular() => 5;

// El interceptor puede reemplazar esta llamada:
int CalcularInterceptado() => 25;


En realidad se usan atributos especiales, pero la idea es que la llamada cambia sin modificar código fuente.

✔ En qué casos se aplica

Logging automático

Métricas automáticas

Validación automática (como FluentValidation)

Caching automático

Reemplazar reflección por código generado (mayor rendimiento)

✔ Casos reales

Serializadores rápidos sin escribir manualmente parseadores

Auditar métodos en producción

Minimizar boilerplate

⭐ 4. Mejoras en Métodos Asíncronos en Constructores (Preview)

C# 13 introduce capacidades experimentales que permiten un mejor soporte para inicializaciones asincrónicas, aunque no permite constructores async directamente.

Lo que se habilita es un patrón más seguro:

✔ Patrón recomendado con C# 13
public class Servicio
{
    public required Task Inicializacion { get; init; }

    public Servicio()
    {
        Inicializacion = InicializarAsync();
    }

    private async Task InicializarAsync()
    {
        await Task.Delay(100);
        Console.WriteLine("Completado");
    }
}


Antes esto era mucho más restrictivo y requería patrones complejos.

✔ Cuándo usarlo

Cuando un servicio necesita cargar:

Configuraciones

Recursos externos

Conexiones

Datos iniciales

⭐ 5. Pattern Matching Mejorado

En C# 13 se mejora la legibilidad y se amplía el soporte a expresiones adicionales, haciéndolo aún más flexible.

✔ Ejemplo
object valor = 30;

if (valor is int >= 0 and < 100)
{
    Console.WriteLine("Es un entero entre 0 y 99.");
}

✔ Cuándo usarlo

En validadores de reglas de negocio

Motores de promoción o descuentos

Orquestadores

Web APIs que devuelven tipos discriminados

(Esto a ti te sirve muchísimo en motores de validación basados en reglas.)

⭐ 6. Mejoras en ref fields en structs

Ahora se puede usar ref en más escenarios, haciendo más viable escribir tipos de bajo nivel muy eficientes.

✔ Ejemplo
public struct BufferWrapper
{
    public ref int Value;
}

✔ Cuándo usarlo

Código de alto rendimiento

Librerías de compresión

Parsers binarios

Procesamiento en memoria (SIMD, Span<T>)

✔ Beneficio

Reduces copias innecesarias → mejor rendimiento.

⭐ 7. Extendido soporte para operadores with en tipos más complejos (preview)

Aunque with nació para records, C# 13 incrementa compatibilidad futura para proporcionar una experiencia más uniforme.

⭐ Tabla Resumen (para entrevista)
Feature	Qué mejora	Cuándo usarlo
Params Collections	Ahora params acepta List<T> y otras colecciones	APIs de dominio, funciones utilitarias, pipelines
Using alias mejorado	Aliases para namespaces y tipos complejos	Evitar colisiones, DDD, código más limpio
Interceptores	Reemplazo automático de código	Logging, métricas, caching, código generativo
Async initialization improvements	Mejor soporte a inicialización asíncrona	Servicios que deben cargar recursos externos
Pattern matching extendido	Reglas más expresivas	Validaciones, engines de reglas, cálculos
Ref fields mejorados	Mayor control sobre memoria	Código de alto rendimiento / bajo nivel