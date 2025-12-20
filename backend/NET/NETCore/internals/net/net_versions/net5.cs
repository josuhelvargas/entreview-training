// 1) Unificación del “.NET” (fin del branding raro)

// Qué cambió: .NET 5 fue el primer release “unificado” después de .NET Core 3.1 (ya no “.NET Core 4”).
// Impacto práctico: en proyectos nuevos normalmente ya apuntas a net5.0.

// <PropertyGroup>
//   <TargetFramework>net5.0</TargetFramework>
// </PropertyGroup>


// Por qué importa: simplifica multi-targeting y roadmap (luego 6, 7, 8…).

// 2) Single-file publish (mejorado) + deployments más simples

// Qué mejora: distribuir una app como un solo archivo (ideal para CLIs, tools internas, instalaciones).

// Publicar en un solo archivo
// dotnet publish -c Release -r win-x64 \
//   /p:PublishSingleFile = true \
//   / p:SelfContained = true


// SelfContained = true: incluye runtime(no requiere .NET instalado).

// PublishSingleFile = true: genera un ejecutable único.

// Caso real: herramientas internas para ETLs, scripts corporativos, utilidades de soporte.

// 3) System.Text.Json maduró mucho (más utilizable en enterprise)

// Aunque inició en .NET Core 3, en .NET 5 se volvió más usable para escenarios comunes.

// Ejemplo: opciones típicas de JSON (camelCase, enums como string)
// using System.Text.Json;
// using System.Text.Json.Serialization;

// var options = new JsonSerializerOptions
// {
//   PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//   WriteIndented = true
// };
// options.Converters.Add(new JsonStringEnumConverter());

// var json = JsonSerializer.Serialize(new
// {
//   status = Status.Success,
//   amount = 123.45m
// }, options);

// Console.WriteLine(json);

// enum Status { Success, Error }


// Por qué importa: menos dependencia de Newtonsoft en muchos proyectos; menos overhead de serialización.

// 4) C# 9 (usado con .NET 5) — mejoras enormes de DX
// 4.1 record para modelos inmutables (ideal para DTOs)
// public record UserDto(int Id, string Name);

// var u1 = new UserDto(1, "Ana");
// var u2 = u1 with { Name = "Ana Maria" };

// Console.WriteLine(u1 == u2); // false (comparación por valor)


// Por qué es útil:

// Igualdad por valor

// with para clon inmutable

// DTOs limpios para APIs, mensajes, eventos

// 4.2 Pattern matching mejorado
// static string Describe(object input) => input switch
// {
//   null => "null",
//   int n and > 0 => "positive int",
//   int => "int",
//   string { Length: > 10 } s => $"long string: {s.Length}",
//   _ => "unknown"
// };


// Caso real: validaciones, reglas, transformaciones de datos.

// 4.3 init setters (inmutabilidad con sintaxis de objetos)
// public class Settings
// {
//   public string BaseUrl { get; init; } = "";
//   public int TimeoutSeconds { get; init; }
// }

// var s = new Settings { BaseUrl = "https://api", TimeoutSeconds = 10 };
// // s.TimeoutSeconds = 99; ❌ ya no (solo init)


// Caso real: opciones de configuración, modelos que no quieres que cambien.

// 5) Mejoras importantes de performance (te “caen gratis”)

// .NET 5 trajo optimizaciones en:

// JIT

// GC

// BCL (Collections, LINQ en algunos casos, crypto, etc.)

// ASP.NET Core

// Ejemplo práctico: usar HttpClientFactory + SocketsHttpHandler afinado

// services.AddHttpClient("external", client =>
// {
//     client.Timeout = TimeSpan.FromSeconds(10);
// })
// .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
//  {
//    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
//    AutomaticDecompression = System.Net.DecompressionMethods.All
//  });


// Por qué importa: performance y estabilidad de red con menos problemas de sockets.

// 6) gRPC / HTTP/2 más sólido para microservicios internos

// Si estás en arquitectura de microservicios, .NET 5 ya estaba en una etapa muy “usable” con gRPC.

// Server (mínimo)
// app.UseRouting();
// app.UseEndpoints(endpoints =>
// {
//   endpoints.MapGrpcService<GreeterService>();
// });

// Client
// using var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://localhost:5001");
// var client = new Greeter.GreeterClient(channel);
// var reply = await client.SayHelloAsync(new HelloRequest { Name = "Josue" });


// Caso real: comunicación interna high-throughput.

// 7) Multi-targeting moderno y transición suave

// Muy común en enterprise: librerías que deben funcionar en varios runtimes.

// <PropertyGroup>
//   <TargetFrameworks>net5.0; netstandard2.0</TargetFrameworks>
// </PropertyGroup>


// Caso real: shared libs reutilizables entre servicios nuevos y apps legacy.

// 8) “Source Generators” ya eran parte clave del ecosistema

// No es “de .NET 5” estrictamente (se introdujeron antes), pero en .NET 5 se volvieron muy comunes en tooling.

// Ejemplo conceptual (sin escribir todo el generator)

// Generar mappers

// Generar código de serialización

// Validaciones compile-time

// Beneficio: menos reflection, mejor startup, mejor AOT-ready a futuro.