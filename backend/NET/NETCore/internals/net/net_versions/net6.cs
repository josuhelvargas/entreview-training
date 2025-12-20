// 1) Minimal Hosting Model (adiós Startup.cs obligatorio)
// ✅ Antes (3.1/5): Startup + Program
// public static IHostBuilder CreateHostBuilder(string[] args) =>
//     Host.CreateDefaultBuilder(args)
//         .ConfigureWebHostDefaults(webBuilder =>
//         {
//             webBuilder.UseStartup<Startup>();
//         });

// ✅ .NET 6: Program.cs simple
// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();

// var app = builder.Build();

// app.MapControllers();

// app.Run();


// Qué mejora:

// Menos boilerplate

// Configuración más clara en un solo lugar

// Mejor DX al iniciar proyectos

// 2) Minimal APIs (endpoints sin controllers)

// Perfecto para:

// microservicios pequeños

// BFF

// tools internas

// prototipos

// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();

// app.MapGet("/users/{id:int}", (int id) =>
// {
//     if (id <= 0) return Results.BadRequest("Invalid id");
//     return Results.Ok(new { id, name = "Ana" });
// });

// app.MapPost("/users", (UserDto user) =>
// {
//     // guardar...
//     return Results.Created($"/users/{user.Id}", user);
// });

// app.Run();

// record UserDto(int Id, string Name);


// Qué mejora:

// Menos clases

// Menos archivos

// Excelente para endpoints simples

// 3) Hot Reload (mejor flujo dev)

// Qué es: Cambias código y ves cambios sin reiniciar toda la app (según caso).

// Ejemplo: editas un endpoint minimal o una vista y el runtime aplica cambios.

// Qué mejora:

// Iteración más rápida (dev experience)

// Menos “stop/run”

// 4) C# 10 (normalmente asociado a .NET 6)
// 4.1 Global usings (menos using repetidos)
// GlobalUsings.cs
// global using System.Net.Http;
// global using Microsoft.Extensions.Logging;


// Ahora ya no lo repites en cada archivo.

// Qué mejora:

// código más limpio

// menos ruido en files

// 4.2 File-scoped namespaces (menos llaves)
// namespace MyApp.Services;

// public class PaymentsService
// {
// }


// En vez de:

// namespace MyApp.Services
// {
//     public class PaymentsService { }
// }


// Qué mejora: menos indentación y boilerplate.

// 4.3 record struct (inmutabilidad sin heap objects)

// Útil para valores pequeños que viajan mucho.

// public readonly record struct Money(decimal Amount, string Currency);

// var m = new Money(100m, "MXN");


// Qué mejora:

// value type (menos GC)

// semántica de “dato inmutable”

// 5) DateOnly y TimeOnly (modelado correcto de fechas/horas)

// Antes se usaba DateTime para todo (y eso causa bugs).
// Ahora puedes modelar mejor.

// DateOnly birthDate = new DateOnly(1992, 10, 5);
// TimeOnly startTime = new TimeOnly(9, 30);

// Console.WriteLine(birthDate); // 05/10/1992 (depende cultura)
// Console.WriteLine(startTime); // 09:30


// Caso real:

// cumpleaños

// fechas de vencimiento sin hora

// horarios

// Qué mejora: evita bugs de timezone/hora cuando no aplica.

// 6) Mejoras en performance “gratis” (runtime + GC + JIT)

// No es solo “marketing”: en .NET 6 suele verse:

// mejor throughput en ASP.NET Core

// mejoras en GC (menos pausas según patrón)

// mejoras en LINQ/collections/regex en escenarios comunes

// 📌 Lo aprovechas “sin cambiar código”.

// 7) HTTP/Networking: SocketsHttpHandler + HttpClientFactory más sólido (patrón estándar)

// Si consumes APIs externas, este patrón es lo correcto en .NET 6.

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHttpClient<PaymentsClient>(client =>
// {
//     client.BaseAddress = new Uri("https://payments.internal/");
//     client.Timeout = TimeSpan.FromSeconds(5);
// })
// .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
// {
//     PooledConnectionLifetime = TimeSpan.FromMinutes(10),
//     AutomaticDecompression = System.Net.DecompressionMethods.All
// });

// var app = builder.Build();
// app.Run();

// public sealed class PaymentsClient
// {
//     private readonly HttpClient _http;

//     public PaymentsClient(HttpClient http) => _http = http;

//     public Task<string> GetPaymentAsync(string id) =>
//         _http.GetStringAsync($"api/payments/{id}");
// }


// Qué mejora:

// estabilidad de conexiones

// menos sockets agotados

// configuración central

// 8) “Top-level statements” (apps pequeñas/CLIs más rápidas)

// En .NET 6 es súper común en tools internas:

// Console.WriteLine("Hello .NET 6!");


// Qué mejora: CLIs/PoCs sin boilerplate.

// 9) Implicit usings (en templates)

// Los templates de .NET 6 suelen activar:

// <PropertyGroup>
//   <ImplicitUsings>enable</ImplicitUsings>
// </PropertyGroup>


// Qué pasa:

// ya no necesitas using System; en muchos archivos.

// reduce ruido.

// 10) Mejor modelo de configuración y entorno (misma idea, más limpio en Program.cs)
// var builder = WebApplication.CreateBuilder(args);

// var baseUrl = builder.Configuration["Payments:BaseUrl"];
// builder.Services.Configure<PaymentsOptions>(builder.Configuration.GetSection("Payments"));

// var app = builder.Build();
// app.Run();

// public class PaymentsOptions
// {
//     public string BaseUrl { get; set; } = "";
// }


// Qué mejora: config + DI en un solo lugar, claro.

// Resumen final (lo más importante de .NET 6)

// ✅ Minimal hosting (WebApplication)
// ✅ Minimal APIs (MapGet/MapPost)
// ✅ C# 10: global usings + file-scoped namespaces + record struct
// ✅ DateOnly / TimeOnly
// ✅ Hot Reload mejorado
// ✅ Performance “gratis” y mejor infra web/networking
// ✅ LTS (estabilidad)