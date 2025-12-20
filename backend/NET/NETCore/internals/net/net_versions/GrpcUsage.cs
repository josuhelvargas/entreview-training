// 1) Crear el servidor gRPC
// a) Proyecto
// dotnet new grpc - n GrpcServer

// b) Define el contrato Protos/greet.proto
// syntax = "proto3";

// option csharp_namespace = "GrpcServer";

// package greet;

// service Greeter
// {
//   rpc SayHello (HelloRequest) returns (HelloReply);
// }

// message HelloRequest
// {
//   string name = 1;
// }

// message HelloReply
// {
//   string message = 1;
// }

// c) Implementa el servicio Services/GreeterService.cs
// using Grpc.Core;

// namespace GrpcServer;

// public class GreeterService : Greeter.GreeterBase
// {
//   public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
//   {
//     return Task.FromResult(new HelloReply
//     {
//       Message = $"Hello, {request.Name}!"
//     });
//   }
// }

// d) Registra el servicio en Program.cs
// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddGrpc();

// var app = builder.Build();

// app.MapGrpcService<GreeterService>();
// app.MapGet("/", () => "gRPC server is running.");

// app.Run();


// Ejecuta:

// dotnet run

// 2) Crear un cliente gRPC
// a) Proyecto cliente
// dotnet new console -n GrpcClient
// cd GrpcClient
// dotnet add package Grpc.Net.Client
// dotnet add package Google.Protobuf
// dotnet add package Grpc.Tools

// b) Copia el mismo.proto al cliente

// Por ejemplo en Protos/greet.proto(idéntico), y agrégalo al.csproj:

// <ItemGroup>
//   <Protobuf Include = "Protos\greet.proto" GrpcServices="Client" />
// </ItemGroup>

// c) Código del cliente Program.cs
// using Grpc.Net.Client;
// using GrpcServer; // namespace generado (según option csharp_namespace)

// using var channel = GrpcChannel.ForAddress("https://localhost:5001");
// var client = new Greeter.GreeterClient(channel);

// var reply = await client.SayHelloAsync(new HelloRequest { Name = "Josue" });

// Console.WriteLine(reply.Message);


// Ejecuta el cliente:

// dotnet run

// Cosas “pro” que casi siempre aplican en producción
// 1) Timeouts / deadlines

// En gRPC es buena práctica usar deadline:

// var reply = await client.SayHelloAsync(
//     new HelloRequest { Name = "Josue" },
//     deadline: DateTime.UtcNow.AddSeconds(2));

// 2) Errores tipados

// Del lado server puedes devolver:

// StatusCode.InvalidArgument(400-ish)

// StatusCode.NotFound

// etc.

// throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required"));

// 3) Auth(JWT / mTLS)

// gRPC corre sobre ASP.NET Core, así que puedes integrar AddAuthentication() y [Authorize] como en Web API (con detalles de metadata/header).

// 4) Streaming

// Si tu caso es “progress updates” o eventos, gRPC streaming puede competir con SignalR(depende del tipo de cliente).

// Regla rápida para decidir: gRPC vs REST vs SignalR

// REST: integraciones amplias + browser-friendly + simplicidad

// gRPC: microservicios internos + performance + contratos + streaming

// SignalR: UI en tiempo real(server push hacia navegador)











// Umbrales prácticos para considerar gRPC
// 1) Llamadas internas muy frecuentes (RPS alto) en hot paths

// Empieza a evaluar seriamente gRPC cuando un vínculo servicio→servicio cumple alguno:

// ≥ 500–1,000 RPS sostenidos entre dos servicios (especialmente si son varios endpoints)

// o picos ≥ 2,000–5,000 RPS

// o muchas llamadas por request (fan-out), por ejemplo:

// tu API recibe 200 RPS pero cada request hace 20 llamadas internas → 4,000 RPS internos

// 📌 En microservicios, el fan-out es el asesino: el tráfico interno crece mucho más que el tráfico externo.

// 2) Payloads medianos/grandes o mucha “charla” (chatty)

// Si tus llamadas REST cargan JSON de:

// ≥ 5–20 KB por request/response frecuentemente

// o haces muchas llamadas pequeñas (chatty) por transacción

// …gRPC suele empezar a ganar (menos bytes + menos overhead de parsing).

// 3) CPU/latencia “duele” y ya mediste que JSON es parte importante

// Si ves en profiling que:

// una porción grande de CPU se va en serialización/deserialización JSON

// o tu p95/p99 se infla por overhead de request/response

// …gRPC suele valer la pena incluso con RPS moderado.

// 4) Necesitas multiplexing y conexiones eficientes

// Si tienes:

// muchos clientes internos (N servicios hablando entre sí)

// conexiones que se abren/cierran mucho

// problemas de saturación de sockets o overhead de HTTP/1.1

// gRPC (HTTP/2) ayuda porque:

// multiplexa múltiples llamadas en una conexión

// reduce overhead de handshake/conexiones

// Cuándo NO necesitas gRPC aunque tengas tráfico

// Incluso con RPS alto, REST puede ser suficiente si:

// Tus payloads son muy pequeños y tu latencia ya es aceptable

// No tienes fan-out grande

// El cuello de botella está en DB/caching, no en HTTP/serialización

// Necesitas debuggability máxima y tu equipo no quiere complejidad extra

// Regla más útil (la que sí funciona en la práctica)
// ✅ Evalúa gRPC si se cumple cualquiera:

// ≥ ~1,000 RPS internos entre servicios de forma sostenida

// O fan-out: “cada request hace muchas llamadas internas” (10+ calls)

// O JSON + HTTP overhead es ≥ 10–20% del CPU del servicio (medido)

// O necesitas streaming/contratos estrictos (sin discusión)

// Si no se cumple, normalmente REST está bien.

// Mini fórmula para pensar “cuánto tráfico es mucho”

// Si tu endpoint externo hace fan-out:

// Internal RPS ≈ External RPS × Calls per request

// Ejemplo:

// external = 300 RPS

// calls/request = 15

// → internal ≈ 4,500 RPS

// Ahí gRPC suele volverse atractivo.

// Lo que hacen equipos grandes: “trigger de migración”

// Empiezan con REST por simplicidad

// Cuando ven:

// p95 se degrada

// CPU alta por JSON

// fan-out crece

// costos infra suben

// Migran los hot paths a gRPC (no todo).

// Checklist antes de decidir (rápido)

// ¿Son servicios internos controlados? (Sí → gRPC viable)

// ¿Tienes fan-out alto? (Sí → gRPC candidato)

// ¿HTTP/2 está bien soportado en tu infra? (LB/Ingress/Mesh)

// ¿Necesitas streaming o contratos fuertes? (Sí → gRPC gana)

// ¿Tu cuello es DB y no CPU de red? (Entonces gRPC puede no mover la aguja)

// Si me dices 3 números de tu caso, te doy una recomendación concreta:

// RPS externo estimado

// llamadas internas promedio por request (fan-out)

// tamaño promedio de payload (KB) o si son chatty/pequeños