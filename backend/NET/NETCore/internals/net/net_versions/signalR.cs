// SignalR no es para “todo”. Es una herramienta muy específica para comunicación en tiempo real cuando el servidor necesita empujar datos al cliente sin que el cliente esté preguntando constantemente.

// Te explico cuándo, cuándo NO, y cómo se usa correctamente, con ejemplos reales.

// 🧠 ¿Qué es SignalR (en una frase correcta)?

// SignalR es un framework para comunicación bidireccional en tiempo real entre servidor y clientes, usando una conexión persistente (WebSockets si es posible).

// No es polling, no es REST, no es streaming tradicional.

// 1️⃣ ¿CUÁNDO se debe usar SignalR?

// Úsalo cuando se cumplan al menos una de estas condiciones:

// ✅ Caso 1: El servidor inicia la comunicación

// Ejemplos reales:

// Notificaciones(“tu pedido cambió de estado”)

// Progreso de procesos largos (ETL, uploads, validaciones)

// Eventos del sistema (errores, alertas)

// Cambios de estado (check-in, dashboards)

// 👉 Con REST, el cliente tendría que preguntar cada X segundos.
// 👉 Con SignalR, el servidor empuja el cambio.

// ✅ Caso 2: Datos cambian frecuentemente

// Ejemplos:

// Dashboards en tiempo real

// Métricas

// Monitoreo

// Juegos

// Trading / cotizaciones

// ✅ Caso 3: Múltiples clientes deben sincronizarse

// Ejemplos:

// Chats

// Colaboración en tiempo real

// Estados compartidos (“usuario X está escribiendo”)

// ✅ Caso 4: UX necesita “inmediatez”

// Ejemplos:

// Barra de progreso

// Toasts en vivo

// Actualización automática sin refresh

// 2️⃣ ¿CUÁNDO NO usar SignalR?

// ❌ NO es buena idea cuando:

// CRUD tradicional

// APIs públicas

// Requests esporádicos

// Lógica simple request/response

// Necesitas cache HTTP/CDN

// Mobile con conectividad inestable (a veces)

// 👉 SignalR no reemplaza REST
// 👉 Es un complemento, no la base de tu API

// 3️⃣ ¿Cómo funciona internamente? (modelo mental)
// Cliente
//   ↓ (HTTP)
// Negociación
//   ↓
// WebSocket (si se puede)
//   ↓
// Conexión persistente
//   ↓
// Hub (servidor)
//   ↓
// Mensajes bidireccionales


// Transportes (en orden de preferencia):

// WebSockets

// Server-Sent Events (SSE)

// Long Polling

// SignalR elige automáticamente.

// 4️⃣ Ejemplo REAL: progreso de un proceso largo
// Escenario

// Usuario inicia un proceso

// Backend tarda varios segundos/minutos

// Usuario ve progreso en tiempo real

// 4.1 Backend: Hub
// public class ProgressHub : Hub
// {
//   public async Task JoinJob(string jobId)
//   {
//     await Groups.AddToGroupAsync(Context.ConnectionId, jobId);
//   }
// }

// 4.2 Backend: servicio que empuja progreso
// public class JobProcessor
// {
//   private readonly IHubContext<ProgressHub> _hub;

//   public JobProcessor(IHubContext<ProgressHub> hub)
//   {
//     _hub = hub;
//   }

//   public async Task RunAsync(string jobId)
//   {
//     for (int i = 0; i <= 100; i += 10)
//     {
//       await Task.Delay(500);

//       await _hub.Clients
//           .Group(jobId)
//           .SendAsync("progress", i);
//     }
//   }
// }

// 4.3 Frontend(JS / React / Angular)
// const connection = new signalR.HubConnectionBuilder()
//   .withUrl("/hub/progress")
//   .withAutomaticReconnect()
//   .build();

// connection.on("progress", percent =>
// {
// console.log(`Progress: ${ percent}%`);
// });

// await connection.start();
// await connection.invoke("JoinJob", jobId);

// Resultado UX

// Sin polling

// Sin refresh

// Actualización instantánea

// Mejor experiencia

// 5️⃣ Conceptos CLAVE de SignalR
// 🔹 Hub

// Punto central de comunicación.
// No guarda estado.

// public class ChatHub : Hub { }

// 🔹 Clients

// A quién envías mensajes:

// Clients.All
// Clients.Caller
// Clients.Others
// Clients.User(userId)
// Clients.Group(groupName)

// 🔹 Groups

// Agrupan conexiones (MUY usado):

// await Groups.AddToGroupAsync(connectionId, group);


// Casos reales:

// Usuario

// Proceso

// Sala

// Evento

// 🔹 Reconexión automática (IMPORTANTE)
// .withAutomaticReconnect()


// WebSockets se caen.
// SignalR reconecta solo.

// 6️⃣ Autenticación y SignalR

// SignalR respeta el auth pipeline.

// [Authorize]
// public class NotificationsHub : Hub { }


// En frontend:

// cookies

// bearer token

// access token factory

// 7️⃣ Escalamiento (cuando hay múltiples servidores)

// ❗ Importante en producción.

// Por defecto:

// SignalR NO escala entre instancias

// Soluciones:

// Redis Backplane

// Azure SignalR Service

// services.AddSignalR()
//         .AddStackExchangeRedis("localhost");

// 8️⃣ Comparación rápida
// Tecnología	Uso
// REST	Request/response
// Polling	Malo para tiempo real
// SSE	Unidireccional
// SignalR	Bidireccional
// gRPC	RPC eficiente
// WebSockets puro	Más bajo nivel
// 🧠 Regla mental FINAL (muy importante)

// Usa SignalR solo cuando el servidor necesita hablar primero.

// Si el cliente siempre pregunta:
// 👉 REST / HTTP

// Si el servidor debe avisar:
// 👉 SignalR

// Si quieres, en el siguiente paso puedo:

// diseñar cuándo SignalR y cuándo REST en una arquitectura real

// mostrar errores comunes (memory leaks, hubs con estado)

// explicar SignalR + Redis + Kubernetes

// comparar SignalR vs SSE vs WebSockets puros