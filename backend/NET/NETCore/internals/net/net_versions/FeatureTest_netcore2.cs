//Features dotnet core 2
//se logro retrocompatibilidad con apis previas de .net framework: ejem ADO.NET




using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using System.Drawing;
using System.Drawing.Imaging;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//anotaciones para campos ([Required, SrtingLength, EmailAddress, Range(18,100) edad DisplayName("Fecha de nacimiento ")])

using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

using System.Configuration;


using System.Security.Cryptography;
using System.Reflection;
using System.Text.RegularExpressions;


using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;

using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization;
using System.IO.Compression;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;




int[] nums = { 1, 5, 3, 9, 2 };
ref int max = ref UsarRefReturns.FindMax(nums);
max = 100; // Modifica el array original

var color = Color.FromArgb(255, 100, 50, 25);
var point = new Point(10, 20);
var size = new Size(100, 200);
var rectangle = new Rectangle(point, size);



var XmlDocument = new XmlDocument();
XmlDocument.LoadXml("<root><element>Josue</element></root>");
var elementValue = XmlDocument.SelectSingleNode("/root/element")?.InnerText;
if (elementValue != null)
{
  Console.WriteLine($"XmlDocument Element Value: {elementValue}");
}

// ConfigurationManager (necesita paquete adicional)
// Install-Package System.Configuration.ConfigurationManager

//var connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"]?.ConnectionString; 

//criptographic algorithms

// MD5
// using (var md5 = MD5.Create())
// {
//   var hash = md5.ComputeHash(Encoding.UTF8.GetBytes("password"));
//   var hashString = BitConverter.ToString(hash).Replace("-", "");
// }

// // SHA256
// using (var sha256 = SHA256.Create())
// {
//   var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes("password"));
// }

// // RSA
// using (var rsa = RSA.Create())
// {
//   var publicKey = rsa.ExportParameters(false);
//   var privateKey = rsa.ExportParameters(true);
// }

// // AES
// using (var aes = Aes.Create())
// {
//   aes.Key = GenerateKey();
//   aes.IV = GenerateIV();

//   using (var encryptor = aes.CreateEncryptor())
//   {
//     // Encriptar datos
//   }
// }


Span<byte> buffer = stackalloc byte[256];
Span<byte> slicedData = buffer.Slice(0, 128);
slicedData.Fill(0xFF);
foreach (var b in slicedData)
{
  Console.Write($"{b:X2} ");
}

Memory<byte> memoryBuffer = new byte[512];
memoryBuffer.GetType().GetProperty("Length");





public ref struct BufferReader
{
  private Span<byte> _buffer;

  public BufferReader(Span<byte> buffer)
  {
    _buffer = buffer;
  }
}

// Ref returns para evitar copias
public class UsarRefReturns
{
  public ref int FindMax(int[] numbers)
  {
    int maxIndex = 0;
    for (int i = 1; i < numbers.Length; i++)
    {
      if (numbers[i] > numbers[maxIndex])
        maxIndex = i;
    }
    return ref numbers[maxIndex];
  }

}
// Uso

public class CacheExample
{
  private readonly Dictionary<string, int> _cache = new Dictionary<string, int>();
  public async ValueTask<int> GetCachedValueAsync(string key)
  {
    if (_cache.TryGetValue(key, out int value))
    {
      // Retorno síncrono sin allocation de Task
      return value;
    }

    // Solo se crea Task si es necesario
    return await FetchFromDatabaseAsync(key);
  }
}

//Muestra un ejemplo practico de como utilizar Span y Memory en una coslucion concreta y por que no otro tipo de estructura. 
// 🎯 Problema real(muy común en backend)

// 👉 Procesar grandes payloads (logs, CSV, protocolos, headers, streams)
// 👉 Extraer campos SIN crear cientos de string, byte[], Substring, etc.

public static class useReadOnlySpan
{

  static void ParseLogLineExtractData(ReadOnlySpan<char> line,
                      out ReadOnlySpan<char> user,
                      out ReadOnlySpan<char> action,   //<- Estos campos reciben la linea sin copiarla cada vez que se accded al metodo.
                      out ReadOnlySpan<char> latencyMs)
  {
    user = default; // Inicializar como Span vacío
    action = default;
    latencyMs = 0;

    int fieldStart = 0;
    int fieldIndex = 0;
    for (int i = 0; i <= line.Length; i++)
    {
      if (i == line.Length || line[i] == ',')
      {
        var field = line.Slice(fieldStart, i - fieldStart);
        switch (fieldIndex)
        {
          case 0:
            user = field;
            break;
          case 1:
            action = field;
            break;
          case 2:
            latencyMs = field;
            break;
        }
        fieldIndex++;
        fieldStart = i + 1;
      }
    }
  }
}


// 🔁 ¿Y Memory<T>? (cuando Span NO alcanza)

// Span<T>:

// Vive en el stack

// NO puede cruzar await

// NO puede almacenarse en campos

// | Tipo | Vive | Uso |
// | ------------------- | ----- | --------------------- |
// | `Span < T >`           | Stack | Hot paths, parsing    |
// | `ReadOnlySpan<T>`   | Stack | Parsing seguro        |
// | `Memory<T>`         | Heap  | Async, fields         |
// | `ReadOnlyMemory<T>` | Heap  | Async + inmutabilidad |



// 🧠 ¿Por qué 4096 bytes?

// 4096 bytes = 4 KB

// Este tamaño aparece constantemente porque alinea muy bien con el hardware y el SO.

// 1️⃣ Tamaño de página de memoria del sistema operativo

// En la mayoría de los sistemas:

// 4 KB = tamaño de página de memoria

// Windows

// Linux

// macOS (comúnmente)

// 👉 Leer bloques alineados a páginas:

// reduce fallos de página

// mejora cache locality

// minimiza trabajo del kernel

// 📌 No es obligatorio, pero es un sweet spot.
public class UsageMemory {
public async Task ProcessAsync(Stream stream) //Puede ser networkstream, archivo, httpreuqesbody...etc.
{
  byte[] buffer = ArrayPool<byte>.Shared.Rent(4096); //se pide prestado un array de byres reutilixable del pool.(4kb) en lugar de crear mucha basura con new byte[]
  try
  {
    int read;
    while ((read = await stream.ReadAsync(buffer)) > 0)
    {
      Memory<byte> chunk = buffer.AsMemory(0, read); //crea una vista de solo lo leido (el bugfer puede ser superior a 4096 bytes)
      ProcessChunk(chunk);
    }
  }
  finally
  {
    ArrayPool<byte>.Shared.Return(buffer);//regresa array al pool par que otros lo utilicen
  }
}
// Procesamiento interno con Span

static void ProcessChunk(Memory<byte> chunk)
{
  Span<byte> span = chunk.Span;

  // parse protocol, headers, etc.
}

}






//Muestra un ejemplo practico de como utilizar Ref Structs para alto redmimiento.Se rleaciona con el hecho de tener grandes cantidades de datos en memoria en listas por ejemplo? objetos en forma de listas grandes?
// Qué es un ref struct y por qué es rápido

// Un ref struct:

// Vive solo en el stack

// No puede:

// ser campo de una clase

// ser capturado por lambdas/closures

// cruzar await/yield

// ir a heap (por diseño)

// Se usa para:

// “envolver” un Span<T> / punteros / buffers

// parsing rápido

// evitar crear objetos por cada operación

// Ejemplos built-in: Span<T>, ReadOnlySpan<T> son ref structs.






// Si tienes una lista gigante de objetos (List<MyClass>)

// El problema de performance normalmente es:

// demasiadas allocations(cada objeto)

// cache misses(memoria dispersa)

// GC pressure

// En esos casos, las soluciones típicas son:

// usar struct (value types) o record struct para datos pequeños

// usar arrays de structs (memoria contigua)

// usar ArrayPool<T>

// usar Span<T>/Memory<T> sobre buffers

// usar “SoA” (Structure of Arrays) si estás ultra fino

// ref struct se usa más como:
// ✅ “capa de parsing/lectura/operación” sobre datos contiguos
// que como contenedor de datos de largo plazo.



// ¿Por qué no hacerlo con struct normal?
// Porque un struct normal puede escaparse al heap indirectamente (guardado en object, boxing, capturas, etc.).
// ref struct impone reglas que garantizan:

// no lo guardas por error

// no lo usas async por error

// no lo capturas en lambdas por error

// Eso mantiene el modelo de “vista temporal” seguro y rápido.












//Explica por que valuetask es mas eficiente que Task en operaciones sincronas. Da un ejemplo practico y real de su usu.
// 🧠 ¿Por qué ValueTask puede ser más eficiente que Task?
// Problema de fondo con Task

// Cada vez que haces:

// return Task.FromResult(value);


// o

// return Task.CompletedTask;


// 👉 sigues creando un objeto Task en el heap
// 👉 Eso implica:

// allocation

// presión al GC

// especialmente caro en hot paths (miles/millones de llamadas)




// ¿Qué hace diferente ValueTask?

// ValueTask<T> es un struct, no una clase.

// Puede representar dos estados:

// ✅ Resultado ya disponible (sin heap allocation)

// ⏳ Operación realmente async (envuelve un Task<T>)

// 📌 En el caso síncrono, NO crea Task.



// | Caso |                         Task | ValueTask |
// | ------------------- | ------------- | ---------- |
// | Resultado inmediato | ❌ alloc | ✅ no alloc |
// | Async real          | ✅             | ✅          |
// | Hot path            | ❌ GC pressure | ✅ mejor    |
// | Uso simple          | ✅             | ⚠️ cuidado |




ValueTask:

🧠 ¿Por qué es más eficiente exactamente?
En el caso síncrono:

Task < T > → objeto en heap + referencias + GC

ValueTask<T> → struct en stack(o embebido)

// Resultado:

// menos allocs

// menos Gen0 GCs

// mejor throughput

// menor latencia

// 📌 Microsoft midió mejoras claras en:

// ASP.NET Core

// pipelines

// caching

// networking

//📌 Ejemplo aún más real: TryGet pattern async
 ValueTask<bool> TryGetUserAsync(int id, out User? user)
{
  if (_cache.TryGetValue(id, out user))
  {
    return new ValueTask<bool>(true);
  }

  user = null;
  return new ValueTask<bool>(false);
}


// Sin async, sin Task, sin alloc.

// ⚠️ CUÁNDO NO usar ValueTask

// Esto es tan importante como cuándo sí.

// ❌ No lo uses si:

// el método siempre es async real

// el método se llama pocas veces

// la API pública será consumida por muchos (DX)

// necesitas reusar la operación

// Ejemplo donde NO vale la pena:

 async ValueTask SaveAsync(Order order)
{
  await _db.SaveAsync(order); // siempre async
}


// 👉 Aquí no ganas nada, solo complejidad.

// 🧠 Regla de oro (la que usan equipos grandes)

// Usa ValueTask SOLO cuando el camino síncrono es el dominante.

// Ejemplos perfectos:

// cache en memoria

// pooling

// lookups

// parsing

// validaciones rápidas

// pipelines internos












//Que es con exactitud IActionResult en ASP.NET Core y como se utiliza para manejar respuestas HTTP en controladores web. Proporciona un ejemplo de codigo que demuestre su uso en una aplicacion web real.

// IActionResult es una abstracción que representa el resultado final de una acción HTTP en ASP.NET Core.

// 👉 No es la respuesta HTTP en sí
// 👉 Es una instrucción para que el framework construya la respuesta HTTP



//que es lo que hace:  services.AddAntiforgery(options =>
// {
//   options.HeaderName = "X-CSRF-TOKEN";
// });



// ¿Qué es Anti-Forgery / CSRF?

// CSRF (Cross-Site Request Forgery) es un ataque donde:

// El navegador del usuario ya tiene cookies válidas

// Un sitio malicioso hace que el navegador envíe una petición a tu API

// El servidor no puede distinguir si la petición fue legítima o forjada

// 👉 Anti-Forgery protege contra eso exigiendo un token secreto adicional que el atacante no puede conocer.

// Qué hace exactamente cada línea
// 1️⃣ services.AddAntiforgery(...)

// Registra en el DI container el sistema Anti-Forgery de ASP.NET Core.

// Esto habilita:

// Generación de tokens

// Validación de tokens

// Integración con filtros ([ValidateAntiForgeryToken])

// Cookies de antiforgery

// Sin esta línea, no existe protección CSRF.

// 2️⃣ options.HeaderName = "X-CSRF-TOKEN";

// Le dice al framework:

// “Cuando validemos CSRF, busca el token en este header HTTP”.

// Por defecto, ASP.NET Core espera el token en:

// Un campo oculto de formulario (__RequestVerificationToken)

// Pero en APIs modernas / SPAs, no hay formularios → se usa un header.

// Qué pasa internamente (flujo real)
// 🔁 Flujo completo Anti-CSRF

// El servidor genera un token

// El token se asocia a:

// la sesión

// la cookie de autenticación

// El cliente:

// envía el token en cada request peligrosa (POST, PUT, DELETE)

// El servidor:

// valida que el token coincida

// si no → 400 / 403








//dame mas detalles dde como debe estar estrucurrado el stratup (configureServices y configure)

// 1) Idea central
// ConfigureServices(IServiceCollection services)

// Aquí registras dependencias (DI) y configuras features:

// Controllers / MVC

// AuthN / AuthZ

// DbContext

// HttpClient

// Caching

// Antiforgery

// Swagger

// Health checks

// Options/config

// ✅ “Qué cosas existen y cómo se construyen”

// Configure(IApplicationBuilder app, IWebHostEnvironment env, …)

// Aquí armas el pipeline HTTP (orden importa):

// Exception handling

// HTTPS

// Static files

// Routing

// CORS

// Authentication

// Authorization

// Endpoints

// ✅ “Cómo se procesa cada request”




// 3) Reglas de oro del orden en Configure

// El orden importa porque cada middleware decide:

// si continúa al siguiente (next())

// si corta la ejecución

// Orden “mental” recomendado

// Error handling

// Security transport (HTTPS/HSTS)

// Static files

// Routing

// CORS

// AuthN

// AuthZ

// Endpoints









//como se utiliza el onbjeto cultureinfo y 
// CultureInfo.CurrentCulture = culture;
// CultureInfo.CurrentUICulture = culture;


// CultureInfo.CurrentCulture

// 👉 Controla formatos y parsing

// Afecta:

// DateTime.ToString()

// decimal.Parse()

// string.Format()

// ToString("C"), "N", "D"



// 🧠 ¿Qué es CultureInfo?

// CultureInfo describe cómo una cultura:

// escribe números

// formatea fechas

// separa decimales

// elige idioma de recursos (.resx)

// interpreta texto (parsing)

// Ejemplos:

// "en-US"

// "es-MX"

// "fr-FR"

// using System.Globalization;

// var culture = new CultureInfo("es-MX");

// 🔑 Diferencia CLAVE: CurrentCulture vs CurrentUICulture

// Esta distinción es fundamental.

// 1️⃣ CultureInfo.CurrentCulture

// 👉 Controla formatos y parsing

// Afecta:

// DateTime.ToString()

// decimal.Parse()

// string.Format()

// ToString("C"), "N", "D"

// Ejemplo
// var culture = new CultureInfo("es-MX");
// CultureInfo.CurrentCulture = culture;

// decimal price = 1234.56m;
// DateTime date = new DateTime(2025, 1, 19);

// Console.WriteLine(price.ToString("C"));
// // $1,234.56

// Console.WriteLine(date.ToString());
// // 19/01/2025


// Si fuera "en-US":

// $1,234.56
// 1 / 19 / 2025

// 2️⃣ CultureInfo.CurrentUICulture

// 👉 Controla idioma de recursos (UI)

// Afecta:

// .resx

// mensajes localizados

// textos traducidos

// Ejemplo con recursos

// Resources.resx

// Greeting = Hello


// Resources.es-MX.resx

// Greeting = Hola

// CultureInfo.CurrentUICulture = new CultureInfo("es-MX");

// Console.WriteLine(Resources.Greeting);
// // Hola


// 📌 NO afecta formatos numéricos o fechas, solo idioma.

// 🧩 Por qué normalmente se setean JUNTOS
// CultureInfo.CurrentCulture = culture;
// CultureInfo.CurrentUICulture = culture;


// Porque:

// el usuario espera idioma y formato coherentes

// UI + datos deben “hablar el mismo idioma”

// ⚠️ IMPORTANTE: Scope y Threading
// CultureInfo.CurrentCulture es por thread / async context

// En .NET moderno:

// fluye correctamente con async/await

// NO es global al proceso

// Esto significa:

// puedes tener usuarios con culturas distintas al mismo tiempo

// 🌐 Uso correcto en ASP.NET Core (PRODUCCIÓN)
// ❌ NO recomendado (manual en cada request)
// CultureInfo.CurrentCulture = new CultureInfo("es-MX");


// Esto:

// es frágil

// no escala

// rompe multi-usuario

// ✅ Forma correcta: Localization Middleware
// 1️⃣ Configurar servicios
// builder.Services.AddLocalization();

// builder.Services.Configure<RequestLocalizationOptions>(options =>
// {
//   var supportedCultures = new[]
//   {
//         new CultureInfo("en-US"),
//         new CultureInfo("es-MX")
//     };

//   options.DefaultRequestCulture = new RequestCulture("en-US");
//   options.SupportedCultures = supportedCultures;
//   options.SupportedUICultures = supportedCultures;
// });

// 2️⃣ Activar middleware
// var app = builder.Build();

// app.UseRequestLocalization();

// 3️⃣ ¿Cómo se decide la cultura?

// ASP.NET Core puede leerla de:

// Header Accept-Language

// Cookie

// Query string

// Provider custom (ej. usuario autenticado)

// Ejemplo HTTP:

// Accept - Language: es - MX


// Resultado:

// CultureInfo.CurrentCulture.Name   // es-MX
// CultureInfo.CurrentUICulture.Name // es-MX

// 🧪 Ejemplo REAL en un Controller
// [HttpGet("price")]
// public IActionResult GetPrice()
// {
//   decimal value = 1234.56m;
//   return Ok(new
//   {
//     formatted = value.ToString("C"),
//     culture = CultureInfo.CurrentCulture.Name
//   });
// }


// Con es-MX:

// {
//   "formatted": "$1,234.56",
//   "culture": "es-MX"
// }


// Con fr-FR:

// {
//   "formatted": "1 234,56 €",
//   "culture": "fr-FR"
// }

// 🧠 Cuándo usar InvariantCulture

// Muy importante en datos, protocolos, persistencia.

// ❌ Error común
// decimal.Parse("1234.56"); // depende de la cultura actual

// ✅ Correcto
// decimal.Parse("1234.56", CultureInfo.InvariantCulture);


// Usar InvariantCulture para:

// JSON manual

// CSV

// logs

// DB

// APIs internas

// 🧠 Reglas prácticas (las que usan equipos grandes)
// Escenario	Cultura
// UI / usuario	CurrentCulture
// Textos UI	CurrentUICulture
// Persistencia	InvariantCulture
// Protocolos	InvariantCulture
// Parsing externo	explícito
// 🧠 Resumen mental final

// CurrentCulture = cómo se interpretan y muestran datos
// CurrentUICulture = qué idioma se muestra

// Setear ambos:

// CultureInfo.CurrentCulture = culture;
// CultureInfo.CurrentUICulture = culture;


// es correcto solo en código local/controlado.
// En web apps, usa siempre UseRequestLocalization










// 1️⃣ ¿Cómo se utiliza PageModel? (Razor Pages)
// ¿Qué es PageModel?

// PageModel es la clase de lógica asociada a una página Razor (.cshtml).

// 👉 En Razor Pages:

// 1 página = 1 feature

// NO hay controllers

// El PageModel maneja:

// datos

// validación

// acciones HTTP (GET, POST, etc.)

// Estructura básica
// Pages/
//  └── Users/
//      ├── Create.cshtml
//      └── Create.cshtml.cs   ← PageModel

// Ejemplo real
// Create.cshtml.cs
// public class CreateModel : PageModel
// {
//     private readonly IUserService _service;

//     public CreateModel(IUserService service)
//     {
//         _service = service;
//     }

//     [BindProperty]
//     public CreateUserRequest User { get; set; } = new();

//     public void OnGet()
//     {
//         // cargar datos iniciales
//     }

//     public async Task<IActionResult> OnPostAsync()
//     {
//         if (!ModelState.IsValid)
//             return Page(); // vuelve a renderizar con errores

//         await _service.CreateAsync(User);
//         return RedirectToPage("Index");
//     }
// }

// Create.cshtml
// <form method="post">
//   <input asp-for="User.Name" />
//   <span asp-validation-for="User.Name"></span>

//   <button type="submit">Save</button>
// </form>

// Modelo mental
// Razor Pages	MVC
// PageModel	Controller
// OnGet / OnPost	Actions
// Page()	View()
// RedirectToPage	RedirectToAction
// 2️⃣ ¿Cómo funciona IConfiguration?
// ¿Qué es?

// IConfiguration es un árbol de claves/valores construido a partir de múltiples fuentes.

// Fuentes típicas (en orden de prioridad)

// appsettings.json

// appsettings.{Environment}.json

// User secrets

// Variables de entorno

// Línea de comandos

// 👉 La última sobrescribe a las anteriores.

// Ejemplo
// appsettings.json
// {
//   "Payments": {
//     "BaseUrl": "https://api.payments.com",
//     "Timeout": 30
//   }
// }

// Uso
// string url = Configuration["Payments:BaseUrl"];
// int timeout = Configuration.GetValue<int>("Payments:Timeout");

// Binding a Options (forma profesional)
// public class PaymentsOptions
// {
//     public string BaseUrl { get; set; } = "";
//     public int Timeout { get; set; }
// }

// services.Configure<PaymentsOptions>(
//     Configuration.GetSection("Payments"));

// public PaymentsClient(IOptions<PaymentsOptions> options)
// {
//     _baseUrl = options.Value.BaseUrl;
// }

// 3️⃣ Scopes en DI: Singleton, Scoped, Transient (y el Provider)
// Resumen rápido
// Lifetime	Cuándo se crea	Cuándo muere
// Singleton	1 vez	Al cerrar app
// Scoped	1 por request	Fin del request
// Transient	Cada vez	Inmediato
// Ejemplos prácticos
// Singleton
// services.AddSingleton<IClock, SystemClock>();


// ✔ Configuración
// ✔ Caches globales
// ❌ NO estado por usuario

// Scoped (el más usado en web)
// services.AddScoped<IUserService, UserService>();


// ✔ DbContext
// ✔ Servicios de negocio
// ✔ 1 instancia por request

// Transient
// services.AddTransient<IEmailFormatter, EmailFormatter>();


// ✔ Objetos livianos
// ❌ Evitar si usan DbContext

// ¿Qué es el ServiceProvider?

// Es el motor que crea y resuelve dependencias.

// var service = provider.GetRequiredService<IUserService>();


// ⚠️ Anti-pattern:

// services.BuildServiceProvider(); // ❌ rompe scopes

// 4️⃣ ¿Cómo funciona SignalR?
// ¿Qué es SignalR?

// Framework para comunicación en tiempo real:

// WebSockets (preferido)

// SSE

// Long polling (fallback)

// Flujo interno
// Cliente
//   ↓
// Negociación HTTP
//   ↓
// Conexión persistente
//   ↓
// Hub
//   ↓
// Clientes conectados

// Ejemplo real
// Hub
// public class NotificationsHub : Hub
// {
//     public async Task Send(string message)
//     {
//         await Clients.All.SendAsync("message", message);
//     }
// }

// Startup
// services.AddSignalR();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapHub<NotificationsHub>("/hub/notifications");
// });

// Cliente JS
// const conn = new signalR.HubConnectionBuilder()
//   .withUrl("/hub/notifications")
//   .build();

// conn.on("message", msg => console.log(msg));
// await conn.start();

// Casos reales

// ✔ Dashboards
// ✔ Notificaciones
// ✔ Progreso en tiempo real
// ✔ Chats
// ✔ Estados de procesos

// 5️⃣ UseHttpsRedirection y UseHsts
// UseHttpsRedirection
// app.UseHttpsRedirection();


// 🔹 Si llega:

// http://example.com


// ➡️ Redirige a:

// https://example.com


// Status: 307 / 308

// Protege datos en tránsito

// UseHsts
// app.UseHsts();


// 🔹 Envía header:

// Strict-Transport-Security: max-age=31536000


// 👉 Le dice al navegador:

// “Nunca vuelvas a usar HTTP para este dominio”

// ⚠️ Solo en producción

// Orden correcto
// if (!env.IsDevelopment())
// {
//     app.UseHsts();
// }

// app.UseHttpsRedirection();

// 🧠 Resumen mental final
// Concepto	Para qué sirve
// PageModel	Lógica de páginas Razor
// IConfiguration	Config unificada
// Singleton	Global
// Scoped	1 por request
// Transient	Siempre nuevo
// SignalR	Tiempo real
// HTTPS Redirect	Forzar HTTPS
// HSTS	HTTPS permanente




public class IndexModel : PageModel
{
  [BindProperty]
  public string Name { get; set; }

  public void OnGet()
  {
    // Lógica para GET
  }

  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
      return Page();

    // Procesar formulario
    return RedirectToPage("./Success");
  }
}


// Startup.cs mejorado
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    // Soporte mejorado para antiforgery
    services.AddAntiforgery(options =>
    {
      options.HeaderName = "X-CSRF-TOKEN";
    });

    // Razor Pages
    services.AddRazorPages();

    // MVC con compatibilidad 2.0
    services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_0);
  }
}


public class CultureUsage
{
  public void ConfigureCulture()
  {
    var culture = new CultureInfo("es-MX");
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;

    decimal price = 1234.56m;
    Console.WriteLine(price.ToString("C")); // $1,234.56
  }
  public void ShowDateInUsCulture(DateTime date)
  {
    var usCulture = new CultureInfo("en-US");
    string formattedDate = date.ToString("D", usCulture);
    Console.WriteLine($"Date in US format: {formattedDate}");
  }
}