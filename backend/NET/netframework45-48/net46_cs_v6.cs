🔵 .NET Framework 4.6
1. RyuJIT 64-bit JIT compiler introduced
🔍 ¿Qué es RyuJIT?

Antes de .NET 4.6, el runtime usaba un JIT distinto para 32-bit y 64-bit.
RyuJIT es el nuevo compilador JIT de 64 bits que:

Genera código nativo mucho más rápido.

Mejora el rendimiento de código intensivo en CPU.

Tiene mejor soporte para SIMD (via System.Numerics.Vector<T>).

Permite optimizaciones más agresivas (inlining, loop unrolling, etc.).

No cambias código para “usar RyuJIT”, simplemente se activa cuando tu app corre como 64-bit (.NET 4.6+).

🧩 Ejemplo de código que se beneficia de RyuJIT
using System;
using System.Numerics;

public static class VectorMath
{
    // .NET 4.6 + 64-bit JIT (RyuJIT)
    public static float Sum(float[] values)
    {
        int i = 0;
        int vectorSize = Vector<float>.Count;
        var acc = new Vector<float>(0);

        // Bucle vectorizado (SIMD)
        for (; i <= values.Length - vectorSize; i += vectorSize)
        {
            var v = new Vector<float>(values, i);
            acc += v;
        }

        float result = 0;
        for (int j = 0; j < vectorSize; j++)
        {
            result += acc[j];
        }

        // Resto escalar
        for (; i < values.Length; i++)
        {
            result += values[i];
        }

        return result;
    }
}


Con RyuJIT, este tipo de código vectorizado es mucho más rápido que con el antiguo JIT de 64-bit.

2. TLS 1.2 support
🔍 ¿Por qué importa?

En 2015+ la industria empezó a exigir TLS 1.2 (bancos, APIs seguras, PCI-DSS, etc.).
.NET Framework 4.6 añadió mejor soporte y defaults más seguros para TLS.

✅ Forzar TLS 1.2 en tu app .NET 4.6
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

public static class HttpClientFactory
{
    static HttpClientFactory()
    {
        // Fuerza el uso de TLS 1.2 (y TLS 1.1 si quieres)
        ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
    }

    public static HttpClient Create()
    {
        // HttpClient respeta el SecurityProtocol configurado
        return new HttpClient
        {
            BaseAddress = new Uri("https://secure-api.example.com/")
        };
    }
}

public class SecureService
{
    public async Task<string> GetSecureDataAsync()
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync("data");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}


Esto es típico en servicios legacy que empiezan a consumir APIs modernas con TLS 1.2 obligatorio.

3. Roslyn compiler enables C# 6

Con .NET 4.6 llega Roslyn, que es:

Compilador open-source, escrito en C#.

Ofrece APIs para análisis y refactorización de código.

Habilita C# 6 con nuevas features de lenguaje.

🧠 Algunas features clave de C# 6

Interpolación de strings

Null-conditional (?.)

nameof

Expression-bodied members

Using static

🧩 Ejemplo práctico C# 6 (sobre .NET 4.6)
using System;
using static System.Console;

public class User
{
    public string FirstName { get; }
    public string LastName { get; }

    // Auto-property initializer + expression-bodied constructor (C# 6+ tiene improvements)
    public User(string firstName, string lastName) =>
        (FirstName, LastName) = (firstName, lastName);

    // Expression-bodied property
    public string FullName => $"{FirstName} {LastName}";

    // Expression-bodied method
    public override string ToString() => FullName;
}

public class Logger
{
    public void LogUser(User user)
    {
        // Null-conditional
        var name = user?.FullName ?? "[unknown user]";

        // nameof
        WriteLine($"[{nameof(Logger)}] Processing {name}");
    }
}

class Program
{
    static void Main()
    {
        var user = new User("Ada", "Lovelace");
        var logger = new Logger();
        logger.LogUser(user);

        User? nullUser = null;
        logger.LogUser(nullUser);  // No NullReferenceException gracias a ?. y ??
    }
}


Estos ejemplos muestran claramente C# 6 sobre .NET 4.6.

🔵 .NET Framework 4.6.1 / 4.6.2
1. Improved cryptography & X509

.NET 4.6.1/4.6.2 incluyen:

Mejor integración con CNG (Cryptography Next Generation).

Mejor soporte para algoritmos modernos (SHA-256, SHA-384, SHA-512).

Mejoras en X509Certificate2, cadenas de certificados, validación, etc.

🧩 Ejemplo: cargar y validar un certificado moderno
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class CertificateValidator
{
    public void ValidateCertificate(string certPath, string password)
    {
        // Carga un PFX con claves modernas (RSA 2048+, SHA-256)
        var cert = new X509Certificate2(
            certPath,
            password,
            X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.EphemeralKeySet);

        Console.WriteLine($"Subject: {cert.Subject}");
        Console.WriteLine($"Issuer:  {cert.Issuer}");
        Console.WriteLine($"NotBefore: {cert.NotBefore}");
        Console.WriteLine($"NotAfter:  {cert.NotAfter}");

        using (var rsa = cert.GetRSAPublicKey())
        {
            Console.WriteLine($"Key size: {rsa.KeySize} bits");
        }
    }
}

2. Always Encrypted for SQL Server

No es exclusivo del framework, pero .NET 4.6.1+ se integra muy bien:

Los datos se cifran en el cliente antes de salir de tu proceso.

SQL Server almacena los datos cifrados.

El servidor no ve los datos en texto plano (protección frente a DBA malicioso o ataque en el servidor).

🔧 Cadena de conexión típica con Always Encrypted
// Ejemplo de connection string
// Requiere .NET 4.6+ y SQL Server 2016+
string connectionString =
    "Server=.;Database=BankDb;Integrated Security=true;" +
    "Column Encryption Setting=Enabled;";

🧩 Uso con parámetros (los datos se cifran en el cliente)
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class PaymentRepository
{
    private readonly string _connectionString;

    public PaymentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertCardAsync(string cardNumber, string holderName)
    {
        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT INTO Payments (CardNumber, HolderName)
                VALUES (@cardNumber, @holderName);";

            cmd.Parameters.Add(new SqlParameter("@cardNumber", SqlDbType.NVarChar)
            {
                Value = cardNumber
            });

            cmd.Parameters.Add(new SqlParameter("@holderName", SqlDbType.NVarChar)
            {
                Value = holderName
            });

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}


Si las columnas CardNumber y HolderName están configuradas como Always Encrypted, el driver de ADO.NET se encarga de cifrar/descifrar.

3. Compression performance improvements

.NET 4.6.1/4.6.2 mejoran el rendimiento de:

GZipStream

DeflateStream

Escenarios de compresión/ descompresión en servidores (por ejemplo, Web API).

No cambias el código, pero tus operaciones de compresión se vuelven más rápidas.

🧩 Ejemplo: comprimir un archivo grande
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

public class CompressionService
{
    public async Task CompressAsync(string sourceFile, string destinationGzip)
    {
        using (var sourceStream = new FileStream(
            sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var destStream = new FileStream(
            destinationGzip, FileMode.Create, FileAccess.Write, FileShare.None))
        using (var gzipStream = new GZipStream(destStream, CompressionLevel.Optimal))
        {
            // En .NET 4.6.1+ el rendimiento es mejor, sin cambiar el código
            await sourceStream.CopyToAsync(gzipStream);
        }
    }

    public async Task DecompressAsync(string sourceGzip, string destinationFile)
    {
        using (var sourceStream = new FileStream(
            sourceGzip, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var destStream = new FileStream(
            destinationFile, FileMode.Create, FileAccess.Write, FileShare.None))
        using (var gzipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
        {
            await gzipStream.CopyToAsync(destStream);
        }
    }
}

🧾 Resumen tipo entrevista (4.6 / 4.6.1 / 4.6.2)

RyuJIT 64-bit
→ Nuevo JIT 64-bit más rápido, mejor soporte SIMD, optimiza loops y código intensivo en CPU. Lo aprovechas con apps 64-bit y System.Numerics.Vector<T>.

TLS 1.2
→ Soporte fortalecido para protocolos modernos; configuras ServicePointManager.SecurityProtocol y usas HttpClient para integrarte con APIs seguras.

Roslyn + C# 6
→ Nuevo compilador gestionado y open-source, habilita features como interpolación de strings, null-conditional, nameof, expression-bodied, etc.

Cryptografía y X509 mejorados
→ Mejor integración con CNG, certificados SHA-256+, APIs modernas en X509Certificate2.

Always Encrypted
→ El cliente (.NET) cifra datos antes de enviarlos, SQL Server solo ve datos cifrados; ideal para datos sensibles (tarjetas, documentos, etc.).

Compresión mejorada
→ GZipStream y DeflateStream más rápidos, mismo código, mejor throughput.

Si quieres, en el siguiente paso puedo:

Hacer una tabla comparativa 4.5 vs 4.6 vs 4.6.1/4.6.2, con columnas: Feature, Beneficio, Código de ejemplo.

Generar un PDF de este módulo igual que el anterior para tu curso de “Evolución de .NET + C# para entrevistas avanzadas”.

You said:
explica en mator detalle nameof el null conditiaonl y el using static 
ChatGPT said:
You're giving feedback on a new version of ChatGPT.
Which response do you prefer? Responses may take a moment to load.
ChatGPT
Response 1

Aquí tienes una explicación extremadamente clara, profunda y técnica, ideal para entrevista o para tu curso. Incluye cómo funciona, cuándo usarlo, trampas comunes y ejemplos avanzados.




🔵 1. nameof (C# 6)

nameof devuelve el nombre de una variable, propiedad, método, clase, parámetro u otro símbolo como string, pero sin riesgo de error al renombrar.

✔️ ¿Qué problema soluciona?

Antes de nameof, los desarrolladores escribían strings “mágicos”:

throw new ArgumentNullException("firstName");


Si luego renombrabas firstName → el string no cambiaba → bugs silenciosos.

✔️ ¿Qué hace exactamente?

Compila la expresión dentro de nameof(...) y extrae su nombre textual.

throw new ArgumentNullException(nameof(firstName));


Si renombramos la variable, el compilador también renombra la cadena → cero errores silenciosos.

✔️ Ejemplos prácticos
1. Validación de argumentos
public void Register(string email)
{
    if (email == null)
        throw new ArgumentNullException(nameof(email));
}

2. Notificación de cambios (WPF, WinForms, MVVM)
OnPropertyChanged(nameof(FirstName));

3. Evitar strings mágicos en logging
_logger.LogInformation("Processing {method}", nameof(ProcessPayment));

4. Validar parámetros opcionales
void Save(string fileName, string directory = null)
{
    if (directory == null)
        throw new ArgumentNullException(nameof(directory));
}

✔️ Internamente

nameof no evalúa valores, solo evalúa símbolos → genera strings en tiempo de compilación.

🔵 2. Null-Conditional Operator ?. (C# 6)

El operador ?. permite acceder a miembros solo si el objeto no es null.
Si es null → devuelve null y no lanza excepción.

✔️ Antes de C# 6
if (user != null && user.Address != null)
{
    city = user.Address.City;
}


Código repetitivo, verboso.

✔️ Con ?.
var city = user?.Address?.City;


Si cualquier parte es null → city = null.

✔️ Se puede encadenar infinitamente
var state = order?.Customer?.Address?.State;

✔️ Null-coalescing combinados: ?. + ??
var city = user?.Address?.City ?? "Unknown";

✔️ Llamar métodos de forma segura
user?.SendEmail();


Si user es null → la llamada se ignora sin excepción.

✔️ Acceder a eventos de forma thread-safe

Antes:

var handler = PropertyChanged;
if (handler != null)
    handler(this, args);


Después:

PropertyChanged?.Invoke(this, args);

✔️ Evaluación técnica interna

Si la parte izquierda es null, devuelve null.

Si no, evalúa la parte derecha.

No genera excepciones por acceso nulo.

🔵 3. using static (C# 6)

Permite importar los miembros estáticos de una clase directamente al scope actual.

✔️ Problema antes de C# 6
var pi = Math.PI;
var area = Math.Pow(radius, 2) * Math.PI;

✔️ Con using static
using static System.Math;

var pi = PI;
var area = Pow(radius, 2) * PI;

Beneficio:

Código más limpio y expresivo.

Muy útil en utilidades matemáticas, helpers, enums estáticos, logger estáticos, etc.

✔️ Ejemplo usando Console sin escribir Console.
using static System.Console;

WriteLine("Hello");
ForegroundColor = ConsoleColor.Red;
WriteLine("Red text");
ResetColor();

✔️ Ejemplo con una clase personalizada
public static class Utils
{
    public static void Log(string message) => Console.WriteLine(message);
    public static int Multiply(int a, int b) => a * b;
}

using static Utils;

Log("Processing...");
var x = Multiply(3, 4);

✔️ Ejemplo de enums y atributos de bandera
using static System.IO.FileAttributes;

var attr = ReadOnly | Hidden | Archive;

✔️ Cómo compila internamente

No copia los métodos.
Los referencia como si hubieras escrito:

Utils.Log("...");


Solo ahorra sintaxis.

🔵 Resumen ultra-claro para entrevista
✔ nameof

Produce strings seguros, mantenibles y renombrables.

Resuelve error humano en refactors.

Ideal para validaciones y MVVM.

✔ Null-conditional ?.

Previene NullReferenceException.

Acorta código.

Se puede combinar con ?? y métodos, props y eventos.

✔ using static

Limpia la sintaxis al usar miembros estáticos.

No cambia el comportamiento, solo la forma de escribir.

Útil en matemáticas, logging, utilidades.