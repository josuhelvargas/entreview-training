using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace azurefunctiondemo2{
public static class HelloPost
{
    private record CreateLeadRequest(string? Email, string? FullName);

    [FunctionName("HelloPost")]  //asi se regitra mimetodo como una azure function , es el id de l a fx, la cual se logueara Function.HelloPost
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "leads")] HttpRequest req, //ruta api/leads sin autorizacion
        ILogger log)
    {
        string body = await new StreamReader(req.Body).ReadToEndAsync();

        CreateLeadRequest? payload;
        try
        {
            payload = JsonSerializer.Deserialize<CreateLeadRequest>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return new BadRequestObjectResult(new { error = "Invalid JSON body." });
        }

        if (payload is null)
            return new BadRequestObjectResult(new { error = "Body is required." });

        if (string.IsNullOrWhiteSpace(payload.Email))
            return new BadRequestObjectResult(new { error = "Email is required." });

        if (string.IsNullOrWhiteSpace(payload.FullName))
            return new BadRequestObjectResult(new { error = "FullName is required." });

        // Caso de uso real: aquí podrías guardar en DB/cola, o disparar webhook, etc.
        return new ObjectResult(new
        {
            message = "Lead created",
            payload.Email,
            payload.FullName
        })
        { StatusCode = StatusCodes.Status201Created };
    }
}
}


// 1) ¿Qué es un webhook y cómo funciona?

// Un webhook es una notificación HTTP que un sistema A le manda a tu sistema B cuando ocurre un evento.

// Ejemplos:

// Stripe → “pago confirmado”

// GitHub → “push/pull request”

// Shopify → “orden creada”

// Tu frontend → “formulario enviado”

// Flujo real

// En el sistema externo (Stripe/GitHub) configuras una URL:

// https://tudominio.com/api/webhooks/stripe

// Cuando ocurre el evento, Stripe manda un POST con JSON.

// Tu endpoint:

// valida autenticidad(firma/secreto)

// responde rápido(200 OK)

// y luego procesa (idealmente asíncrono)

// Por qué “responder rápido” importa

// Los proveedores reintentan si:

// tardas demasiado(timeout)

// respondes 500

// no respondes

// Por eso, patrón típico:

// Aceptar → encolar → responder 200/202

// Procesar después sin presión de tiempo.





// 2) Persistir en BD “a lot” (muchas inserciones) desde una Function

// Si recibes muchas cosas (formularios, eventos, logs), puedes persistir en BD, pero hay dos estilos:

// A) Sincrónico directo (simple)

// Webhook/formulario → validar → guardar en BD → responder 201/200

// ✅ Pros: simple
// ❌ Contras: si la BD está lenta, tu endpoint se vuelve lento y el proveedor reintenta.

// B) Asíncrono (recomendado para volumen)

// Webhook/formulario → validar → mandar mensaje a cola → responder 202
// Worker function → lee cola → guarda en BD (con retries controlados)

// ✅ Pros: robusto, escalable, tolerante a picos
// ✅ Evita timeouts y “reintentos duplicados” del proveedor
// ✅ Puedes hacer batch / control de concurrencia
// ❌ Contras: más componentes(pero vale la pena en producción)

// Concepto clave: Idempotencia

// En webhooks/eventos, puede llegar el mismo evento 2 veces.
// Entonces al persistir, usas una llave única (ej: EventId) y haces “insert if not exists” (o upsert). Esto evita duplicados.






// 3) ¿Qué es Azure Service Bus y cómo “envío un mensaje” ahí?

// Azure Service Bus es un servicio de mensajería “enterprise”:

// Garantiza entrega

// Soporta Topics/Subscriptions (pub-sub)

// Tiene Dead-letter queue

// Lock/complete (processing seguro)

// Buen control de reintentos y orden (según configuración)

// ¿Cuándo usarlo?

// Cuando necesitas robustez, reintentos confiables, y varios consumidores

// Integración entre microservicios/sistemas internos

// Workflows críticos

// En Azure Functions (la idea)

// Tu HTTP Function recibe el webhook/form → publica mensaje a Service Bus

// Otra Function (ServiceBusTrigger) procesa el mensaje y guarda en BD / llama APIs, etc.

// En código real, lo más común es:

// usar un output binding (lo más simple), o

// inyectar ServiceBusSender (más flexible)





// 4) ¿Qué es Storage Queue (Azure Queue Storage)?

// Storage Queue es mensajería más simple y barata basada en Azure Storage.

// ¿Cuándo usarla?

// Tareas sencillas “fire-and-forget”

// Backlogs simples

// Cuando costo y simplicidad importan más que features enterprise

// Diferencias rápidas vs Service Bus

// Más barata/simple

// Menos features avanzadas (pub-sub real, sesiones, reglas, etc.)





// 5) ¿Qué es Event Hubs?

// Azure Event Hubs es para ingesta de eventos a gran escala (streaming):

// Telemetría

// Logs

// Clickstream

// IoT

// Grandes volúmenes por segundo

// Piensa:

// Service Bus = “mensajes para procesos”

// Event Hubs = “flujo de eventos para análisis/stream”

// ¿Cuándo usarlo?

// Cuando tienes muchísimos eventos

// Cuando te importa procesar “streams” (y quizá guardar para analytics)

// Integración con:

// Stream processing

// Data Lake

// Analytics

// Consumers en paralelo por particiones






// 6) La idea central: Event - driven(respuesta a eventos)

// Tu confusión es normal: “¿por qué esto es mejor que desplegar microservicios?”

// La clave es que aquí no estás escogiendo “Functions vs microservicios”.
// Estás escogiendo un modelo de arquitectura:

// Modelo A: Request - driven(solo HTTP)

// Todo pasa en tiempo real:

// Cliente llama API

// API hace todo: valida, guarda, llama otros servicios, manda correo…

// Responde al final

// Problema:

// Si hay pico de tráfico, todo se satura

// Si una dependencia falla, tu endpoint falla

// Si tarda, timeouts y reintentos

// Escalar puede ser caro y complicado

// Modelo B: Event - driven(reaccionar a eventos)

// Divides en 2 fases:

// Ingesta rápida(HTTP/webhook)

// Procesamiento asíncrono(colas/streams)

// Esto “soluciona aspectos relacionados a respuesta a eventos” porque:

// Los eventos(webhooks, formularios, cambios) llegan en cualquier momento, en picos, con reintentos.

// Tu sistema no debe “romperse” por esos picos.






// 7) Por qué este enfoque suele ser mejor (cuando aplica)
// ✅ Beneficios fuertes

// Resiliencia: si BD o API externa cae, la cola retiene, reintenta, no pierdes eventos.

// Escalamiento automático: Functions escala según mensajes/eventos.

// Costo: pagas por uso (ideal para aprendizaje y cargas variables).

// Desacoplamiento: el webhook no depende de que “todo salga bien” en ese momento.

// Observabilidad: puedes medir colas, latencia, dead-letter, etc.

// ⚠️ Trade-offs reales

// Más piezas: cola + worker + DLQ + idempotencia

// Consistencia eventual: no todo pasa “ya”; pasa “en breve”

// Requiere disciplina (correlation id, retries, poison messages)







// 8) ¿Entonces microservicios ya no sirven?

// Sí sirven. De hecho, muchas veces:

// Tienes microservicios y

// usan colas/eventos y

// algunas partes son Functions

// La comparación correcta es:

// Microservicio = unidad de despliegue y dominio

// Event-driven + mensajería = forma de integración y procesamiento

// Azure Functions = runtime/serverless ideal para handlers, workers, webhooks, tareas reactivas

// Un microservicio también puede consumir Service Bus/Event Hubs.
// La diferencia es que con Functions lo haces más rápido y con menos “plomería” (infra y scaling).






// 9) “Lista utilizable” (qué escoger según el caso)

// Formulario simple (pocas solicitudes):
// HTTP Function → BD directo

// Formulario con picos / integraciones:
// HTTP Function → Cola (Service Bus o Storage Queue) → Worker → BD

// Webhook de Stripe/GitHub:
// HTTP Function(validar firma) → Cola → Worker (idempotente) → BD / acciones

// Telemetría masiva / eventos de tracking:
// HTTP → Event Hubs → Consumers (Functions/Stream Analytics) → Data Lake/DB

// Pub-sub (varios sistemas reaccionan al mismo evento):
// Service Bus Topic → múltiples subscriptions → múltiples handlers
