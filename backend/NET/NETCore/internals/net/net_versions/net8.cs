// 1) Native AOT más usable (publicar binarios nativos)
// ¿Qué es?

// Compilas tu app a código nativo (sin JIT en runtime).
// Ideal para:

// CLIs

// servicios pequeños

// funciones/serverless

// apps donde el cold start importa

// Activación (csproj)
// <PropertyGroup>
//   <TargetFramework>net8.0</TargetFramework>
//   <PublishAot>true</PublishAot>
//   <InvariantGlobalization>true</InvariantGlobalization>
// </PropertyGroup>

// Publicar
// dotnet publish -c Release -r linux-x64


// Qué mejora:

// arranque más rápido

// menos memoria (en muchos escenarios)

// despliegue más “nativo”

// ⚠️ Trade-off: reflection / dynamic code requiere cuidado (trimming/AOT-friendly).

// 2) ASP.NET Core: mejor performance en APIs “sin tocar código”

// En .NET 8, Kestrel + pipeline + JSON + routing reciben mejoras internas.
// Para ti significa: upgrade → menos CPU/latencia en muchos workloads web.

// Ejemplo de API minimal ultra simple (y rápida):

// var app = WebApplication.CreateBuilder(args).Build();

// app.MapGet("/ping", () => Results.Ok(new { ok = true }));

// app.Run();


// Qué mejora: throughput y latencia por optimizaciones internas.

// 3) Mejoras fuertes de gRPC (productividad y performance)

// En microservicios, gRPC en .NET 8 suele ser más sólido/rápido y con mejor DX (interceptors/telemetría en ecosistema, HTTP/2 handling, etc.).

// Server gRPC (mínimo)
// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddGrpc();

// var app = builder.Build();
// app.MapGrpcService<GreeterService>();
// app.Run();


// Cuándo te pega: si haces alta frecuencia service-to-service.

// 4) Rate Limiting “first-class” (middleware estándar)

// Para APIs reales necesitas protegerte de abuso/bugs/cargas.

// Configurar un Rate Limiter
// using System.Threading.RateLimiting;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddRateLimiter(options =>
// {
//   options.AddFixedWindowLimiter("fixed", o =>
//   {
//     o.Window = TimeSpan.FromSeconds(10);
//     o.PermitLimit = 50; // 50 req cada 10s
//     o.QueueLimit = 0;
//   });
// });

// var app = builder.Build();

// app.UseRateLimiter();

// app.MapGet("/data", () => "ok")
//    .RequireRateLimiting("fixed");

// app.Run();


// Qué mejora: control central y fácil, sin libs externas.

// 5) Output Caching maduro para APIs

// Caching de respuestas en el servidor (no CDN), útil para endpoints GET con alto tráfico.

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddOutputCache();

// var app = builder.Build();
// app.UseOutputCache();

// app.MapGet("/products", () =>
// {
//   // simula consulta cara
//   return Enumerable.Range(1, 5).Select(i => new { id = i, name = $"P{i}" });
// })
// .CacheOutput(p => p.Expire(TimeSpan.FromSeconds(30)));

// app.Run();


// Por qué importa:

// baja CPU/DB hits

// mejora latencia

// muy simple de aplicar

// 6) Time Provider (mejor testabilidad y diseño)

// Antes se usaba DateTime.UtcNow (difícil de testear).
// Ahora puedes inyectar TimeProvider y controlarlo.

// public sealed class TokenService
// {
//   private readonly TimeProvider _time;

//   public TokenService(TimeProvider time) => _time = time;

//   public bool IsExpired(DateTimeOffset expiresAt) =>
//       _time.GetUtcNow() >= expiresAt;
// }


// Registro:

// builder.Services.AddSingleton(TimeProvider.System);
// builder.Services.AddSingleton<TokenService>();


// Qué mejora: tests deterministas, mejor diseño.

// 7) JSON: mejoras y más escenarios “source-generated”

// System.Text.Json sigue avanzando. En escenarios de alto rendimiento puedes usar JsonSerializerContext para reducir reflection/overhead.

// Source-generated JSON (patrón)
// using System.Text.Json.Serialization;

// [JsonSerializable(typeof(UserDto))]
// internal partial class AppJsonContext : JsonSerializerContext
// {
// }

// public record UserDto(int Id, string Name);


// Uso:

// var user = new UserDto(1, "Ana");
// string json = System.Text.Json.JsonSerializer.Serialize(user, AppJsonContext.Default.UserDto);


// Qué mejora:

// menos reflexión

// mejor AOT friendliness

// más performance en hot paths

// 8) Mejoras de performance generales (JIT/GC/BCL)

// .NET 8 trae mejoras “gratis” típicamente visibles en:

// throughput web

// LINQ en algunos patrones

// collections

// IO

// JIT (optimizaciones)

// GC (mejor comportamiento según cargas)

// Ejemplo práctico: usar FrozenDictionary para lookups read-only muy rápidos (útil en reglas/config estática).

// using System.Collections.Frozen;

// var dict = new Dictionary<string, int>
// {
//   ["A"] = 1,
//   ["B"] = 2
// }.ToFrozenDictionary();

// Console.WriteLine(dict["A"]);


// Cuándo importa: catálogos / lookup tables inmutables en hot paths.

// 9) Observabilidad/telemetría: ecosistema más alineado (OpenTelemetry / metrics)

// En .NET 8 el stack de observabilidad sigue madurando y es común estandarizar:

// logs estructurados

// metrics

// tracing

// Ejemplo de metrics básico (conceptual):

// using System.Diagnostics.Metrics;

// var meter = new Meter("MyApp");
// var counter = meter.CreateCounter<int>("requests_total");

// app.MapGet("/ping", () =>
// {
//   counter.Add(1);
//   return "pong";
// });


// Qué mejora: instrumentación consistente.

// Resumen final(lo que más conviene adoptar de .NET 8)

// ✅ Native AOT(cuando importa cold start / footprint)
// ✅ Rate Limiting estándar en middleware
// ✅ Output Caching más listo para producción
// ✅ TimeProvider para diseño/test
// ✅ System.Text.Json source-gen (AOT/perf)
// ✅ Frozen collections para hot lookups
// ✅ Optimización “gratis” de runtime/ASP.NET Core (más throughput)